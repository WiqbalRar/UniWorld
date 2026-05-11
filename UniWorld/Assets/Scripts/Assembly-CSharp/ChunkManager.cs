using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
	public static ChunkManager instance;

	public bool drawChunkBorders;

	public Transform playerTransform;

	private Vector3 lastPlayerPos = Vector3.zero;

	public Dictionary<Vector3Int, Chunk> activeChunks = new Dictionary<Vector3Int, Chunk>();

	private List<Vector3Int> toRecycle = new List<Vector3Int>();

	public int chunkRadius;

	public int waterUpdateRadius = 5;

	private int radius;

	public Vector3Int currentChunk = -Vector3Int.up;

	public int chunkRequiredBeforePlay = int.MaxValue;

	public int chunkLitRequiredBeforePlay = int.MaxValue;

	public int chunkRenderCount;

	public int chunkLitCount;

	private Coroutine chunkPoolClearingRoutine;

	private bool isUpdatingTicks;

	public bool forceGeneration = true;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
		UpdateChunkRadius();
		ThreadPool.SetMaxThreads(SystemInfo.processorCount, SystemInfo.processorCount);
	}

	public void UpdateChunkRadius()
	{
		int num = PlayerPrefs.GetInt("RenderDistance");
		float t = ((float)num - 7f) / 28f;
		Shader.SetGlobalVector("_FogData", new Vector2(((float)num - 5f) * 16f, Mathf.Lerp(16f, 96f, t)));
		if (num == chunkRadius)
		{
			return;
		}
		int num2 = chunkRadius;
		chunkRadius = num;
		if (num > num2)
		{
			chunkRequiredBeforePlay = (int)Mathf.Pow((chunkRadius - 2) * 2 + 1, 2f);
			chunkLitRequiredBeforePlay = (int)Mathf.Pow((chunkRadius - 5) * 2 + 1, 2f);
			chunkRenderCount = 0;
			foreach (KeyValuePair<Vector3Int, Chunk> activeChunk in activeChunks)
			{
				if (activeChunk.Value.generationStage == 5)
				{
					chunkRenderCount++;
				}
			}
		}
		if (chunkPoolClearingRoutine != null)
		{
			StopCoroutine(chunkPoolClearingRoutine);
		}
		chunkPoolClearingRoutine = StartCoroutine(ClearChunkPool());
		radius = chunkRadius * 16;
	}

	private IEnumerator ClearChunkPool()
	{
		int requiredChunks = (int)Mathf.Pow(chunkRadius * 2 + 1, 2f);
		while (activeChunks.Count != requiredChunks)
		{
			yield return null;
		}
		ObjectPool.instance.ClearPool(0);
		chunkPoolClearingRoutine = null;
	}

	private void Update()
	{
		if (chunkRenderCount >= chunkRequiredBeforePlay && !isUpdatingTicks)
		{
			TickManager.instance.UpdateTicks();
		}
		UpdateCurrentChunk();
		RecycleChunks();
		RequestChunks();
		if (!isUpdatingTicks && (TickManager.instance.mustUpdateBlock || TickManager.instance.mustUpdateWater))
		{
			ProcessUpdates();
		}
		UpdateChunkGenerationStage1();
		UpdateChunkGenerationStage2();
		UpdateChunkGenerationStage3And4And5();
	}

	private async void ProcessUpdates()
	{
		isUpdatingTicks = true;
		if (TickManager.instance.mustUpdateBlock)
		{
			TickManager.instance.mustUpdateBlock = false;
			UpdateBlocks();
		}
		if (TickManager.instance.mustUpdateWater)
		{
			TickManager.instance.mustUpdateWater = false;
			UpdateChunkWater();
		}
		isUpdatingTicks = false;
	}

	private void UpdateBlocks()
	{
		int num = waterUpdateRadius * 16;
		for (int i = currentChunk.x - num; i <= currentChunk.x + num; i += 16)
		{
			for (int j = currentChunk.z - num; j <= currentChunk.z + num; j += 16)
			{
				Chunk chunk = activeChunks[new Vector3Int(i, 0, j)];
				if (!chunk.isThreading)
				{
					chunk.ProcessBlockUpdate();
				}
			}
		}
	}

	private void UpdateChunkWater()
	{
		int num = waterUpdateRadius * 16;
		for (int i = currentChunk.x - num; i <= currentChunk.x + num; i += 16)
		{
			for (int j = currentChunk.z - num; j <= currentChunk.z + num; j += 16)
			{
				Chunk chunk = activeChunks[new Vector3Int(i, 0, j)];
				if (!chunk.isThreading)
				{
					chunk.ProcessWaterSimulation();
				}
			}
		}
	}

	private void UpdateChunkGenerationStage1()
	{
		for (int i = currentChunk.x - radius; i <= currentChunk.x + radius; i += 16)
		{
			for (int j = currentChunk.z - radius; j <= currentChunk.z + radius; j += 16)
			{
				Chunk chunk = activeChunks[new Vector3Int(i, 0, j)];
				if (!chunk.isThreading && chunk.generationStage == 0)
				{
					chunk.GenerationStep1();
				}
			}
		}
	}

	private void UpdateChunkGenerationStage2()
	{
		int num = radius - 16;
		for (int i = currentChunk.x - num; i <= currentChunk.x + num; i += 16)
		{
			for (int j = currentChunk.z - num; j <= currentChunk.z + num; j += 16)
			{
				Chunk chunk = activeChunks[new Vector3Int(i, 0, j)];
				if (!chunk.isThreading && chunk.generationStage == 1)
				{
					chunk.GenerationStep2();
				}
			}
		}
	}

	private void UpdateChunkGenerationStage3And4And5()
	{
		Vector3Int zero = Vector3Int.zero;
		int num = radius - 32;
		for (int i = currentChunk.x - num; i <= currentChunk.x + num; i += 16)
		{
			for (int j = currentChunk.z - num; j <= currentChunk.z + num; j += 16)
			{
				zero = new Vector3Int(i, 0, j);
				Chunk chunk = activeChunks[zero];
				if (chunk.isThreading)
				{
					continue;
				}
				if (chunk.generationStage == 2)
				{
					chunk.GenerationStep3();
				}
				if (!(Vector3Int.Distance(currentChunk, zero) < ((float)chunkRadius - 2f) * 16f) && !forceGeneration)
				{
					continue;
				}
				if (chunk.generationStage >= 3)
				{
					if (chunk.generationStage == 3)
					{
						chunk.GenerationStep4(chunkRenderCount >= chunkRequiredBeforePlay);
					}
					if (!chunk.isUpdatingLightMap)
					{
						chunk.UpdateLightMap();
					}
				}
				if (chunk.generationStage == 4 && chunk.lightmapHasBeenBaked)
				{
					chunk.GenerationStep5();
				}
			}
		}
	}

	private void RecycleChunks()
	{
		float num = currentChunk.x - radius;
		float num2 = currentChunk.x + radius;
		float num3 = currentChunk.z - radius;
		float num4 = currentChunk.z + radius;
		foreach (KeyValuePair<Vector3Int, Chunk> activeChunk in activeChunks)
		{
			if (activeChunk.Value.canFinishRecycling)
			{
				activeChunk.Value.Recycle();
				toRecycle.Add(activeChunk.Key);
			}
			else if (!activeChunk.Value.isThreading && !activeChunk.Value.isRecycling && !activeChunk.Value.modificationPending && activeChunk.Value.usedByNeighbours == 0 && !activeChunk.Value.usingNeighbours && ((float)activeChunk.Key.x < num || (float)activeChunk.Key.x > num2 || (float)activeChunk.Key.z < num3 || (float)activeChunk.Key.z > num4))
			{
				activeChunk.Value.PrepareToRecycle();
			}
		}
		for (int num5 = toRecycle.Count - 1; num5 >= 0; num5--)
		{
			activeChunks.Remove(toRecycle[num5]);
		}
		toRecycle.Clear();
	}

	private void RequestChunks()
	{
		for (int i = currentChunk.x - radius; i <= currentChunk.x + radius; i += 16)
		{
			for (int j = currentChunk.z - radius; j <= currentChunk.z + radius; j += 16)
			{
				Vector3Int vector3Int = new Vector3Int(i, 0, j);
				if (!activeChunks.ContainsKey(vector3Int))
				{
					GameObject gameObject = ObjectPool.instance.RequestObject(0, vector3Int);
					activeChunks.Add(vector3Int, gameObject.GetComponent<Chunk>());
				}
			}
		}
	}

	private bool UpdateCurrentChunk()
	{
		if (playerTransform != null)
		{
			lastPlayerPos = playerTransform.position;
		}
		Vector3Int chunkPosition = VoxelTools.GetChunkPosition(VoxelTools.FloorPositionToInt(lastPlayerPos) + WorldShifter.instance.offset);
		if (chunkPosition != currentChunk)
		{
			currentChunk = chunkPosition;
			return true;
		}
		return false;
	}
}
