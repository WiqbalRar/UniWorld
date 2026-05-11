using UnityEngine;

namespace VoxelEngine
{
	public class TerrainGeneratorSIMD_Desert : TerrainGeneratorSIMD
	{
		public float terrainScale = 20f;

		public float canyonMaxHeight = 2f;

		public float canyonGradient = 3f;

		public Color32 sandColor = new Color32(240, 190, 2, byte.MaxValue);

		public Color32 stoneColor = new Color32(120, 120, 80, byte.MaxValue);

		public override void Awake()
		{
			SetInterpBitStep(1);
			SetNoiseArraySize(2);
			minHeight = 0f - terrainScale;
			maxHeight = terrainScale * (canyonMaxHeight + 1f);
		}

		public override void GenerateChunk(Chunk chunk)
		{
			Voxel[] voxelData = chunk.voxelData;
			int num = chunk.chunkPos.y << 5;
			int num2 = 0;
			float[] interpNoise = GetInterpNoise(0, chunk.chunkPos);
			float[] interpNoise2 = GetInterpNoise(1, chunk.chunkPos);
			for (int i = 0; i < interpSize; i++)
			{
				for (int j = 0; j < interpSize; j++)
				{
					float num3 = (j << interpBitStep) + num;
					for (int k = 0; k < interpSize; k++)
					{
						interpNoise2[num2] -= 0.6f;
						interpNoise[num2] = Mathf.Min(interpNoise[num2] + canyonMaxHeight, Mathf.Max(interpNoise[num2], canyonGradient * interpNoise2[num2] * Mathf.Abs(interpNoise2[num2])));
						interpNoise[num2] *= terrainScale;
						interpNoise[num2] -= num3;
						num2++;
					}
				}
			}
			num2 = 0;
			for (int l = 0; l < 32; l++)
			{
				for (int m = 0; m < 32; m++)
				{
					for (int n = 0; n < 32; n++)
					{
						TerrainGeneratorBase.ChunkFillUpdate(chunk, voxelData[num2++] = new Voxel(VoxelInterpLookup(l, m, n, interpNoise)));
					}
				}
			}
		}

		public override Color32 DensityColor(Voxel voxel)
		{
			if (voxel.density < 3.33f)
			{
				return Color32.Lerp(sandColor, stoneColor, voxel.density * 0.3f);
			}
			return stoneColor;
		}
	}
}
