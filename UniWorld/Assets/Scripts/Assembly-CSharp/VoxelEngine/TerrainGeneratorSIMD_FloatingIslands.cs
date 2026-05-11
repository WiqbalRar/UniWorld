using UnityEngine;

namespace VoxelEngine
{
	public class TerrainGeneratorSIMD_FloatingIslands : TerrainGeneratorSIMD
	{
		public float terrainScale = 20f;

		public Color32 grassColor = new Color32(112, 150, 48, byte.MaxValue);

		public Color32 dirtColor = new Color32(97, 75, 66, byte.MaxValue);

		public Color32 stoneColor = new Color32(150, 150, 150, byte.MaxValue);

		public override void Awake()
		{
			SetInterpBitStep(2);
			SetNoiseArraySize(2);
		}

		public override void GenerateChunk(Chunk chunk)
		{
			Voxel[] voxelData = chunk.voxelData;
			int num = 0;
			float[] interpNoise = GetInterpNoise(0, chunk.chunkPos);
			float[] interpNoise2 = GetInterpNoise(1, chunk.chunkPos);
			for (int i = 0; i < interpSize; i++)
			{
				for (int j = 0; j < interpSize; j++)
				{
					for (int k = 0; k < interpSize; k++)
					{
						interpNoise[num] -= 1f;
						interpNoise2[num] = Mathf.Abs(interpNoise2[num] * terrainScale) + (Mathf.Abs(interpNoise[num]) * interpNoise[num] + 0.2f) * 20f;
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
						TerrainGeneratorBase.ChunkFillUpdate(chunk, voxelData[num++] = new Voxel(VoxelInterpLookup(l, m, n, interpNoise2)));
					}
				}
			}
		}

		public override Color32 DensityColor(Voxel voxel)
		{
			if (voxel.density < 2f)
			{
				return Color32.Lerp(grassColor, dirtColor, voxel.density * 0.5f);
			}
			if (voxel.density < 6f)
			{
				float num = (voxel.density - 2f) * 0.25f;
				return Color32.Lerp(dirtColor, stoneColor, num * num);
			}
			return stoneColor;
		}
	}
}
