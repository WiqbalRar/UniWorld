using UnityEngine;

namespace VoxelEngine
{
	public class TerrainGeneratorSIMD_Caves : TerrainGeneratorSIMD
	{
		public float caveRatio = 0.88f;

		public Color32 stoneMinColor = new Color32(150, 150, 150, byte.MaxValue);

		public Color32 stoneMaxColor = new Color32(100, 100, 100, byte.MaxValue);

		public override void Awake()
		{
			SetInterpBitStep(2);
			SetNoiseArraySize(1);
		}

		public override void GenerateChunk(Chunk chunk)
		{
			Voxel[] voxelData = chunk.voxelData;
			int num = 0;
			float[] interpNoise = GetInterpNoise(0, chunk.chunkPos);
			for (int i = 0; i < interpSize; i++)
			{
				for (int j = 0; j < interpSize; j++)
				{
					for (int k = 0; k < interpSize; k++)
					{
						interpNoise[num] = (caveRatio - interpNoise[num]) * 32f;
						num++;
					}
				}
			}
			num = 0;
			for (int l = 0; l < 32; l++)
			{
				for (int m = 0; m < 32; m++)
				{
					for (int n = 0; n < 32; n++)
					{
						TerrainGeneratorBase.ChunkFillUpdate(chunk, voxelData[num++] = new Voxel(VoxelInterpLookup(l, m, n, interpNoise)));
					}
				}
			}
		}

		public override Color32 DensityColor(Voxel voxel)
		{
			return Color32.Lerp(stoneMinColor, stoneMaxColor, voxel.density);
		}
	}
}
