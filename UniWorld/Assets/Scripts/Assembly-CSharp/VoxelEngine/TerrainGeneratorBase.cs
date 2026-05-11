using UnityEngine;

namespace VoxelEngine
{
	[ExecuteInEditMode]
	public abstract class TerrainGeneratorBase : MonoBehaviour
	{
		protected float minHeight = float.MinValue;

		protected float maxHeight = float.MaxValue;

		protected Voxel minVoxel = Voxel.Solid;

		protected Voxel maxVoxel = Voxel.Empty;

		protected int interpBitStep;

		protected int interpSize;

		protected int interpSizeSq;

		protected float interpScale;

		public abstract void GenerateChunk(Chunk chunk);

		public abstract Color32 DensityColor(Voxel voxel);

		public float MinHeight()
		{
			return minHeight;
		}

		public float MaxHeight()
		{
			return maxHeight;
		}

		public Voxel MinVoxel()
		{
			return minVoxel;
		}

		public Voxel MaxVoxel()
		{
			return maxVoxel;
		}

		public static void ChunkFillUpdate(Chunk chunk, Voxel voxel)
		{
			switch (chunk.fillType)
			{
			case Chunk.FillType.Empty:
				if (voxel.IsSolid())
				{
					chunk.fillType = Chunk.FillType.Mixed;
				}
				break;
			case Chunk.FillType.Solid:
				if (!voxel.IsSolid())
				{
					chunk.fillType = Chunk.FillType.Mixed;
				}
				break;
			case Chunk.FillType.Null:
				chunk.fillType = (voxel.IsSolid() ? Chunk.FillType.Solid : Chunk.FillType.Empty);
				break;
			case Chunk.FillType.Mixed:
				break;
			}
		}

		public virtual void Awake()
		{
		}

		protected void SetInterpBitStep(int interpBitStep)
		{
			this.interpBitStep = interpBitStep;
			interpSize = (32 >> interpBitStep) + 1;
			interpSizeSq = interpSize * interpSize;
			interpScale = 1f / (float)(1 << interpBitStep);
		}

		protected int InterpLookupIndex(int interpX, int interpY, int interpZ)
		{
			return interpZ + interpY * interpSize + interpX * interpSizeSq;
		}

		protected float VoxelInterpLookup(int localX, int localY, int localZ, float[] interpLookup)
		{
			float num = ((float)localX + 0.5f) * interpScale;
			float num2 = ((float)localY + 0.5f) * interpScale;
			float num3 = ((float)localZ + 0.5f) * interpScale;
			int num4 = FastFloor(num);
			int num5 = FastFloor(num2);
			int num6 = FastFloor(num3);
			num -= (float)num4;
			num2 -= (float)num5;
			num3 -= (float)num6;
			int num7 = InterpLookupIndex(num4, num5, num6);
			return Lerp(Lerp(Lerp(interpLookup[num7], interpLookup[num7 + interpSizeSq], num), Lerp(interpLookup[num7 + interpSize], interpLookup[num7 + interpSizeSq + interpSize], num), num2), Lerp(Lerp(interpLookup[++num7], interpLookup[num7 + interpSizeSq], num), Lerp(interpLookup[num7 + interpSize], interpLookup[num7 + interpSizeSq + interpSize], num), num2), num3);
		}

		private static float Lerp(float a, float b, float t)
		{
			return a + t * (b - a);
		}

		private static int FastFloor(float f)
		{
			if (!(f >= 0f))
			{
				return (int)f - 1;
			}
			return (int)f;
		}
	}
}
