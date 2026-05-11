using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace VoxelEngine
{
	public class VoxelEngineManager : MonoBehaviour
	{
		public TerrainGeneratorBase terrainGenerator;

		public Transform targetTransform;

		public float loadDistance = 400f;

		public float unloadDistanceModifier = 1.2f;

		public float yDistanceModifier = 1.5f;

		public int maxThreads = 8;

		public float targetFPS = 100f;

		public Material meshMaterial;

		public bool showDebugInfo = true;

		private static ObjectPool<Chunk> chunkPool = new ObjectPool<Chunk>(128);

		private Dictionary<Vector3i, Chunk> chunkMap = new Dictionary<Vector3i, Chunk>();

		private ChunkQueue chunkQueue = new ChunkQueue();

		private Queue<Vector3i> chunkMeshQueue = new Queue<Vector3i>();

		private Stack<Chunk> chunkUnloadStack = new Stack<Chunk>();

		private int yLoadTick = -1;

		private int unloadTick;

		private int threadCount;

		private int meshesLastFrame;

		private int updateTimerLastFrame;

		private float averageFPS;

		private float deltaTimeFPS;

		public Light directionalLight;

		public Light cameraLight;

		private void Start()
		{
			averageFPS = targetFPS;
			if (showDebugInfo)
			{
				UnityEngine.Debug.Log("FastNoiseSIMD level: " + FastNoiseSIMD.GetSIMDLevel());
			}
			ResetAll();
		}

		private void OnGUI()
		{
			int num = 18;
			Rect position = new Rect(4f, 0f, 300f, 20f);
			if (showDebugInfo)
			{
				GUI.Label(position, "Pooled Chunks: " + chunkPool.Count);
				position.y += num;
				GUI.Label(position, "Pooled Chunk GameObjects: " + Chunk.chunkGameObjectPool.Count);
				position.y += num;
				GUI.Label(position, "Chunks Loaded: " + chunkMap.Count);
				position.y += num;
				GUI.Label(position, "Chunk Queue: " + chunkQueue.Count);
				position.y += num;
				GUI.Label(position, "Chunk Mesh Queue: " + chunkMeshQueue.Count);
				position.y += num;
				GUI.Label(position, "Meshes Last Frame: " + meshesLastFrame);
				position.y += num;
				GUI.Label(position, "Update Time Last Frame: " + updateTimerLastFrame + "ms");
				position.y += num;
				GUI.Label(position, "Thread Count: " + threadCount);
				position.y += num;
				GUI.Label(position, "FPS: " + $"{averageFPS:0.0}");
			}
			position = new Rect(Screen.width - 172, 2f, 170f, 20f);
			num = 22;
			if (GUI.Button(position, "Grass Hills"))
			{
				terrainGenerator = Object.FindObjectOfType<TerrainGenerator_GrassLand>();
				ResetAll();
			}
			position.y += num;
			if (GUI.Button(position, "Alien Planet"))
			{
				terrainGenerator = Object.FindObjectOfType<TerrainGenerator_AlienPlanet>();
				ResetAll();
			}
			position.y += num;
			if (GUI.Button(position, "Cracked Surface"))
			{
				terrainGenerator = Object.FindObjectOfType<TerrainGenerator_CrackedSurface>();
				ResetAll();
			}
			position.y += num;
			if (GUI.Button(position, "Desert (SIMD)"))
			{
				terrainGenerator = Object.FindObjectOfType<TerrainGeneratorSIMD_Desert>();
				ResetAll();
			}
			position.y += num;
			if (GUI.Button(position, "Floating Islands (SIMD)"))
			{
				terrainGenerator = Object.FindObjectOfType<TerrainGeneratorSIMD_FloatingIslands>();
				ResetAll();
			}
			position.y += num;
			if (GUI.Button(position, "Caves (SIMD)"))
			{
				terrainGenerator = Object.FindObjectOfType<TerrainGeneratorSIMD_Caves>();
				ResetAll(useCameraLight: true);
			}
		}

		private void ResetAll(bool useCameraLight = false)
		{
			UnloadAllChunks();
			targetTransform.position = new Vector3(0f, 50f, 0f);
			if ((bool)cameraLight && (bool)directionalLight)
			{
				cameraLight.enabled = useCameraLight;
				directionalLight.enabled = !useCameraLight;
			}
		}

		private void Update()
		{
			deltaTimeFPS += (Time.deltaTime - deltaTimeFPS) * 0.1f;
			averageFPS = Mathf.Lerp(averageFPS, 1f / deltaTimeFPS, 0.05f);
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Application.Quit();
			}
		}

		private void LateUpdate()
		{
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			UpdateLoadingQueue();
			CheckUnloadChunks();
			LoadChunksFromQueue();
			MeshChunksFromQueue(stopwatch);
			updateTimerLastFrame = (int)stopwatch.ElapsedMilliseconds;
		}

		private void UpdateLoadingQueue()
		{
			float num = loadDistance * loadDistance;
			int num2 = (Mathf.CeilToInt(loadDistance) - 16 >> 5) + 1;
			int num3 = Mathf.CeilToInt((float)num2 * yDistanceModifier);
			Vector3i vector3i = default(Vector3i);
			Vector3 realPos = default(Vector3);
			Vector3i vector3i2 = new Vector3i(Mathf.RoundToInt(targetTransform.position.x) >> 5, Mathf.RoundToInt(targetTransform.position.y) >> 5, Mathf.RoundToInt(targetTransform.position.z) >> 5);
			for (int i = yLoadTick - num3; i < num3; i += 8)
			{
				vector3i.y = vector3i2.y + i;
				realPos.y = (i + vector3i2.y << 5) + 16;
				for (int j = -num2; j < num2; j++)
				{
					vector3i.x = vector3i2.x + j;
					realPos.x = (j + vector3i2.x << 5) + 16;
					for (int k = -num2; k < num2; k++)
					{
						vector3i.z = vector3i2.z + k;
						if (!chunkMap.ContainsKey(vector3i) && !chunkQueue.Contains(vector3i))
						{
							realPos.z = (k + vector3i2.z << 5) + 16;
							float num4 = ScaledTargetDistanceSq(realPos);
							if (num4 < num)
							{
								chunkQueue.Enqueue(num4, vector3i);
							}
						}
					}
				}
			}
			if (++yLoadTick >= 8)
			{
				yLoadTick = 0;
			}
		}

		private void CheckUnloadChunks()
		{
			float unloadDistanceSq = loadDistance * loadDistance * unloadDistanceModifier * unloadDistanceModifier;
			foreach (Chunk item in chunkMap.Values.Where((Chunk chunk) => (chunk.chunkPos.y & 0x1F) != unloadTick && ScaledTargetDistanceSq(chunk.realPos) > unloadDistanceSq))
			{
				chunkUnloadStack.Push(item);
			}
			if (++unloadTick > 31)
			{
				unloadTick = 0;
			}
			while (chunkUnloadStack.Count != 0)
			{
				UnloadChunk(chunkUnloadStack.Pop());
			}
		}

		private void LoadChunksFromQueue()
		{
			Vector3i pos = default(Vector3i);
			int num = Mathf.RoundToInt((float)maxThreads - (float)chunkMeshQueue.Count * 0.2f);
			while (threadCount < num && chunkQueue.Dequeue(out pos))
			{
				StartCoroutine(LoadChunkThreaded(pos));
			}
		}

		private void MeshChunksFromQueue(Stopwatch updateTimer)
		{
			meshesLastFrame = 0;
			int num = Mathf.RoundToInt(averageFPS - targetFPS);
			while (chunkMeshQueue.Count > 0)
			{
				if (chunkMap.TryGetValue(chunkMeshQueue.Dequeue(), out var value) && value.CanBuildMesh())
				{
					value.BuildMesh();
					meshesLastFrame++;
					if (updateTimer.ElapsedMilliseconds >= num)
					{
						break;
					}
				}
			}
		}

		public float ScaledTargetDistanceSq(Vector3 realPos)
		{
			return new Vector3(targetTransform.position.x - realPos.x, (targetTransform.position.y - realPos.y) * yDistanceModifier, targetTransform.position.z - realPos.z).sqrMagnitude;
		}

		public void LoadChunk(Vector3i chunkPos)
		{
			Chunk chunk = chunkPool.Get();
			chunk.Setup(chunkPos, this);
			if (chunk.CheckTerrainBounds())
			{
				chunk.GenerateVoxelData();
			}
			chunk.FillAdjChunks();
			chunkMap.Add(chunkPos, chunk);
			chunkQueue.Remove(chunkPos);
		}

		public IEnumerator LoadChunkThreaded(Vector3i chunkPos)
		{
			Chunk chunk = chunkPool.Get();
			chunk.Setup(chunkPos, this);
			if (chunk.CheckTerrainBounds())
			{
				threadCount++;
				bool done = false;
				Thread thread = new Thread((ThreadStart)delegate
				{
					chunk.GenerateVoxelData();
					done = true;
				});
				thread.Priority = System.Threading.ThreadPriority.BelowNormal;
				thread.Start();
				while (!done)
				{
					yield return null;
				}
				threadCount--;
			}
			chunk.FillAdjChunks();
			chunkMap.Add(chunkPos, chunk);
			chunkQueue.Remove(chunkPos);
		}

		public void UnloadAllChunks()
		{
			StopAllCoroutines();
			threadCount = 0;
			foreach (Chunk value in chunkMap.Values)
			{
				chunkUnloadStack.Push(value);
			}
			while (chunkUnloadStack.Count != 0)
			{
				UnloadChunk(chunkUnloadStack.Pop());
			}
			chunkQueue.Clear();
			chunkMeshQueue.Clear();
		}

		public void UnloadChunk(Chunk chunk)
		{
			chunkMap.Remove(chunk.chunkPos);
			if (chunkPool.Add(chunk))
			{
				chunk.Clean();
			}
			else
			{
				chunk.Destroy();
			}
		}

		public Chunk GetChunk(Vector3i chunkPos)
		{
			chunkMap.TryGetValue(chunkPos, out var value);
			return value;
		}

		public Chunk GetChunkUnsafe(Vector3i chunkPos)
		{
			return chunkMap[chunkPos];
		}

		public void QueueChunkMeshing(Vector3i chunkPos)
		{
			chunkMeshQueue.Enqueue(chunkPos);
		}
	}
}
