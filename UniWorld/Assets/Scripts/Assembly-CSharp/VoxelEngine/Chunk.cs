using UnityEngine;

namespace VoxelEngine
{
	public class Chunk
	{
		public enum AdjDirection
		{
			Left = 0,
			Down = 1,
			Back = 2,
			LeftDown = 3,
			LeftBack = 4,
			DownBack = 5,
			LeftDownBack = 6
		}

		public enum FillType
		{
			Null = 0,
			Mixed = 1,
			Empty = 2,
			Solid = 3
		}

		public const int BIT_SIZE = 5;

		public const bool GENERATE_COLLIDERS = false;

		public const bool GRADIENT_MESH = false;

		public const bool GENERATE_AMBIENT_OCCLUSION = true;

		public const float AMBIENT_OCCLUSION_STRENGTH = 0.2f;

		public const int SIZE = 32;

		public const int SIZE2 = 16;

		public const int BIT_MASK = 31;

		internal const int VOXEL_STEP_X = 1024;

		internal const int VOXEL_STEP_Y = 32;

		internal const int VOXEL_STEP_Z = 1;

		internal const int VOXEL_STEP_CHUNK_X = 32768;

		internal const int VOXEL_STEP_CHUNK_Y = 1024;

		internal const int VOXEL_STEP_CHUNK_Z = 32;

		internal const int ADJ_CHUNK_SIZE = 7;

		internal const MeshBuilder.MeshType MESH_TYPE = MeshBuilder.MeshType.AmbientOcclusion;

		private static readonly Vector3i[] adjChunkVectors = new Vector3i[7]
		{
			new Vector3i(-1, 0, 0),
			new Vector3i(0, -1, 0),
			new Vector3i(0, 0, -1),
			new Vector3i(-1, -1, 0),
			new Vector3i(-1, 0, -1),
			new Vector3i(0, -1, -1),
			new Vector3i(-1, -1, -1)
		};

		public Vector3i chunkPos;

		public Vector3 realPos;

		public ChunkGameObject chunkGameObject;

		public VoxelEngineManager voxelEngineManager;

		internal FillType fillType;

		internal Voxel[] voxelData = new Voxel[32768];

		internal Chunk[] adjChunks = new Chunk[7];

		internal bool dirtyMesh;

		public static ObjectPool<ChunkGameObject> chunkGameObjectPool = new ObjectPool<ChunkGameObject>(64);

		public void Setup(Vector3i chunkPos, VoxelEngineManager voxelEngineManager)
		{
			this.chunkPos = chunkPos;
			this.voxelEngineManager = voxelEngineManager;
			realPos = new Vector3(chunkPos.x << 5, chunkPos.y << 5, chunkPos.z << 5);
			dirtyMesh = false;
		}

		public void Clean()
		{
			ReleaseChunkGameObject();
			ReleaseAdjChunks();
		}

		public void Destroy()
		{
			ReleaseChunkGameObject();
			ReleaseAdjChunks();
		}

		private void ReleaseAdjChunks()
		{
			for (int i = 0; i < 7; i++)
			{
				adjChunks[i] = null;
				voxelEngineManager.GetChunk(chunkPos - adjChunkVectors[i])?.UpdateAdjChunk(null, i);
			}
		}

		private void ReleaseChunkGameObject()
		{
			if (chunkGameObject != null)
			{
				if (chunkGameObjectPool.Add(chunkGameObject))
				{
					chunkGameObject.Clean();
				}
				else
				{
					chunkGameObject.Destroy();
				}
				chunkGameObject = null;
			}
		}

		public static int VoxelDataIndex(int localX, int localY, int localZ)
		{
			return localZ | (localY << 5) | (localX << 10);
		}

		public Voxel GetVoxelUnsafe(int localX, int localY, int localZ)
		{
			return voxelData[VoxelDataIndex(localX, localY, localZ)];
		}

		public Voxel GetVoxel(int localX, int localY, int localZ)
		{
			if ((localX & 0x1F) != localX || (localY & 0x1F) != localY || (localZ & 0x1F) != localZ)
			{
				return Voxel.Empty;
			}
			return voxelData[VoxelDataIndex(localX, localY, localZ)];
		}

		public void FillAdjChunks()
		{
			bool flag = true;
			FillType fillType = this.fillType;
			for (int i = 0; i < 7; i++)
			{
				adjChunks[i] = voxelEngineManager.GetChunk(chunkPos + adjChunkVectors[i]);
				if (adjChunks[i] != null)
				{
					fillType = ((adjChunks[i].fillType != fillType) ? FillType.Mixed : fillType);
				}
				else
				{
					flag = false;
				}
				voxelEngineManager.GetChunk(chunkPos - adjChunkVectors[i])?.UpdateAdjChunk(this, i);
			}
			if (flag)
			{
				if (fillType != FillType.Mixed)
				{
					dirtyMesh = false;
				}
				else if (dirtyMesh)
				{
					voxelEngineManager.QueueChunkMeshing(chunkPos);
				}
			}
		}

		public void UpdateAdjChunk(Chunk chunk, int side)
		{
			adjChunks[side] = chunk;
			if (dirtyMesh && chunk != null && CanBuildMesh())
			{
				voxelEngineManager.QueueChunkMeshing(chunkPos);
			}
		}

		public bool CanBuildMesh()
		{
			FillType fillType = this.fillType;
			for (int i = 0; i < 7; i++)
			{
				if (adjChunks[i] == null)
				{
					return false;
				}
				fillType = ((adjChunks[i].fillType != fillType) ? FillType.Mixed : fillType);
			}
			if (fillType != FillType.Mixed)
			{
				dirtyMesh = false;
				return false;
			}
			return true;
		}

		public bool CheckTerrainBounds()
		{
			dirtyMesh = true;
			if (voxelEngineManager.terrainGenerator.MinHeight() > realPos.y + 32f)
			{
				FloodVoxelData(voxelEngineManager.terrainGenerator.MinVoxel());
				return false;
			}
			if (voxelEngineManager.terrainGenerator.MaxHeight() < realPos.y)
			{
				FloodVoxelData(voxelEngineManager.terrainGenerator.MaxVoxel());
				return false;
			}
			return true;
		}

		private void FloodVoxelData(Voxel floodVoxel)
		{
			fillType = FillType.Null;
			TerrainGeneratorBase.ChunkFillUpdate(this, floodVoxel);
			int num = 0;
			for (int i = 0; i < 32768; i++)
			{
				voxelData[num++] = floodVoxel;
			}
		}

		public void GenerateVoxelData()
		{
			dirtyMesh = true;
			fillType = FillType.Null;
			voxelEngineManager.terrainGenerator.GenerateChunk(this);
		}

		public void BuildMesh()
		{
			if (!dirtyMesh)
			{
				return;
			}
			dirtyMesh = false;
			Mesh mesh = MeshBuilder.BuildMesh(this, MeshBuilder.MeshType.AmbientOcclusion);
			if (mesh == null)
			{
				ReleaseChunkGameObject();
				return;
			}
			if (chunkGameObject == null)
			{
				chunkGameObject = chunkGameObjectPool.Get();
				chunkGameObject.Setup(chunkPos, realPos, voxelEngineManager.gameObject.transform);
				chunkGameObject.meshRenderer.material = voxelEngineManager.meshMaterial;
			}
			chunkGameObject.meshFilter.sharedMesh = mesh;
		}
	}
}
