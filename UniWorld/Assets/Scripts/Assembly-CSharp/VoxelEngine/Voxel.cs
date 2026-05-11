using System;

namespace VoxelEngine
{
	public struct Voxel
	{
		public static readonly Voxel Solid = new Voxel(1f);

		public static readonly Voxel Empty = new Voxel(-1f);

		private byte _densityByte;

		private const float DENSITY_BYTE_LIMIT = 8f;

		private const float DENSITY_BYTE_CONVERT = 15.9375f;

		private const float DENSITY_BYTE_CONVERT_INV = 0.0627451f;

		public float density
		{
			get
			{
				return ((float)(int)_densityByte - 127.5f) * 0.0627451f;
			}
			set
			{
				_densityByte = (byte)(Math.Min(8f, Math.Max(-8f, value)) * 15.9375f + 127.5f);
			}
		}

		public Voxel(float density = -1f)
		{
			_densityByte = (byte)(Math.Min(8f, Math.Max(-8f, density)) * 15.9375f + 127.5f);
		}

		public bool IsSolid()
		{
			return _densityByte > 127;
		}
	}
}
