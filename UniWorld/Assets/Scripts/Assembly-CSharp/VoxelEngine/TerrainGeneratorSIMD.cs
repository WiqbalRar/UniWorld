using System;

namespace VoxelEngine
{
	public abstract class TerrainGeneratorSIMD : TerrainGeneratorBase
	{
		public FastNoiseSIMDUnity[] fastNoiseSIMDUnity = new FastNoiseSIMDUnity[1];

		protected void SetNoiseArraySize(int size)
		{
			Array.Resize(ref fastNoiseSIMDUnity, size);
		}

		protected float[] GetInterpNoise(int noiseArrayIndex, Vector3i chunkPos)
		{
			int num = 5 - interpBitStep;
			return fastNoiseSIMDUnity[noiseArrayIndex].fastNoiseSIMD.GetNoiseSet(chunkPos.x << num, chunkPos.y << num, chunkPos.z << num, interpSize, interpSize, interpSize, 1 << interpBitStep);
		}
	}
}
