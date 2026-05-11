using UnityEngine;

namespace VoxelEngine
{
	public class TerrainGenerator_GrassLand : TerrainGenerator
	{
		public float terrainScale = 20f;

		public Color32 grassColor = new Color32(112, 150, 48, byte.MaxValue);

		public Color32 dirtColor = new Color32(97, 75, 66, byte.MaxValue);

		public Color32 stoneColor = new Color32(150, 150, 150, byte.MaxValue);

		public override void Awake()
		{
			SetNoiseArraySize(2);
			SetInterpBitStep(2);
			minHeight = 0f - terrainScale - fastNoiseUnity[1].gradientPerturbAmp;
			maxHeight = terrainScale + fastNoiseUnity[1].gradientPerturbAmp;
		}

		public override void GenerateChunk(Chunk chunk)
		{
			float[] array = new float[interpSize * interpSize * interpSize];
			Voxel[] voxelData = chunk.voxelData;
			int num = chunk.chunkPos.x << 5;
			int num2 = chunk.chunkPos.y << 5;
			int num3 = chunk.chunkPos.z << 5;
			int num4 = 0;
			float decimalType = FastNoise.GetDecimalType();
			float num5 = decimalType;
			float num6 = decimalType;
			for (int i = 0; i < interpSize; i++)
			{
				for (int j = 0; j < interpSize; j++)
				{
					for (int k = 0; k < interpSize; k++)
					{
						decimalType = (i << interpBitStep) + num;
						num5 = (j << interpBitStep) + num2;
						num6 = (k << interpBitStep) + num3;
						GetFastNoise(1).GradientPerturb(ref decimalType, ref num5, ref num6);
						float num7 = GetFastNoise(0).GetNoise(decimalType, num5, num6);
						num7 *= terrainScale;
						num7 -= num5;
						array[num4++] = num7;
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
				return Color32.Lerp(grassColor, dirtColor, voxel.density * 0.2f);
			}
			if (voxel.density < 15f)
			{
				float num = (voxel.density - 5f) * 0.1f;
				return Color32.Lerp(dirtColor, stoneColor, num * num);
			}
			return stoneColor;
		}
	}
}
