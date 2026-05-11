using UnityEngine;

namespace VoxelEngine
{
	public class TerrainGenerator_CrackedSurface : TerrainGenerator
	{
		public float terrainScale = 30f;

		public Color32 surfaceColor = new Color32(130, 130, 130, byte.MaxValue);

		public Color32 coreColor = new Color32(80, 0, 80, byte.MaxValue);

		public Color32 lavaColor = new Color32(243, 147, 0, byte.MaxValue);

		public override void Awake()
		{
			SetNoiseArraySize(1);
			SetInterpBitStep(2);
			minHeight = 0f - terrainScale;
			maxHeight = terrainScale;
		}

		public override void GenerateChunk(Chunk chunk)
		{
			float[] array = new float[interpSize * interpSize * interpSize];
			Voxel[] voxelData = chunk.voxelData;
			int num = chunk.chunkPos.x << 5;
			int num2 = chunk.chunkPos.y << 5;
			int num3 = chunk.chunkPos.z << 5;
			int num4 = 0;
			for (int i = 0; i < interpSize; i++)
			{
				float x = (i << interpBitStep) + num;
				for (int j = 0; j < interpSize; j++)
				{
					float num5 = (j << interpBitStep) + num2;
					for (int k = 0; k < interpSize; k++)
					{
						float z = (k << interpBitStep) + num3;
						float num6;
						if (num5 <= terrainScale * -0.6f)
						{
							num6 = 1000000f;
						}
						else
						{
							if (num5 < 0f)
							{
								num6 = GetFastNoise(0).GetNoise(x, 0f, z);
								num6 *= Mathf.Abs(num6) * 1.5f;
							}
							else
							{
								num6 = GetFastNoise(0).GetNoise(x, num5, z);
							}
							num6 *= terrainScale;
							num6 -= Mathf.Abs(num5);
						}
						array[num4++] = num6;
					}
				}
			}
			num4 = 0;
			for (int l = 0; l < 32; l++)
			{
				for (int m = 0; m < 32; m++)
				{
					for (int n = 0; n < 32; n++)
					{
						TerrainGeneratorBase.ChunkFillUpdate(chunk, voxelData[num4++] = new Voxel(VoxelInterpLookup(l, m, n, array)));
					}
				}
			}
		}

		public override Color32 DensityColor(Voxel voxel)
		{
			if (voxel.density < 5f)
			{
				return Color32.Lerp(surfaceColor, coreColor, voxel.density * 0.2f);
			}
			if (voxel.density >= 8f)
			{
				return lavaColor;
			}
			return coreColor;
		}
	}
}
