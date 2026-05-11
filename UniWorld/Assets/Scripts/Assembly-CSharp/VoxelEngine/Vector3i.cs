using UnityEngine;

namespace VoxelEngine
{
	public struct Vector3i
	{
		public int x;

		public int y;

		public int z;

		public static readonly Vector3i zero = new Vector3i(0, 0, 0);

		public static readonly Vector3i one = new Vector3i(1, 1, 1);

		public static readonly Vector3i forward = new Vector3i(0, 0, 1);

		public static readonly Vector3i back = new Vector3i(0, 0, -1);

		public static readonly Vector3i up = new Vector3i(0, 1, 0);

		public static readonly Vector3i down = new Vector3i(0, -1, 0);

		public static readonly Vector3i left = new Vector3i(-1, 0, 0);

		public static readonly Vector3i right = new Vector3i(1, 0, 0);

		public static readonly Vector3i[] directions = new Vector3i[6] { left, right, down, up, back, forward };

		private const int X_PRIME = 1619;

		private const int Y_PRIME = 31337;

		private const int Z_PRIME = 6971;

		public Vector3i(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3i(int x, int y)
		{
			this.x = x;
			this.y = y;
			z = 0;
		}

		public Vector3i(Vector3 v3)
		{
			x = Mathf.RoundToInt(v3.x);
			y = Mathf.RoundToInt(v3.y);
			z = Mathf.RoundToInt(v3.z);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(x, y, z);
		}

		public static int DistanceSquared(Vector3i a, Vector3i b)
		{
			int num = b.x - a.x;
			int num2 = b.y - a.y;
			int num3 = b.z - a.z;
			return num * num + num2 * num2 + num3 * num3;
		}

		public int DistanceSquared(Vector3i v)
		{
			return DistanceSquared(this, v);
		}

		public override int GetHashCode()
		{
			return (x * 1619) ^ (y * 31337) ^ (z * 6971);
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector3i vector3i))
			{
				return false;
			}
			if (x == vector3i.x && y == vector3i.y)
			{
				return z == vector3i.z;
			}
			return false;
		}

		public override string ToString()
		{
			return "Vector3i(" + x + " " + y + " " + z + ")";
		}

		public static bool operator ==(Vector3i a, Vector3i b)
		{
			if (a.x == b.x && a.y == b.y)
			{
				return a.z == b.z;
			}
			return false;
		}

		public static bool operator !=(Vector3i a, Vector3i b)
		{
			if (a.x == b.x && a.y == b.y)
			{
				return a.z != b.z;
			}
			return true;
		}

		public static bool operator >=(Vector3i a, int b)
		{
			if (a.x >= b && a.y >= b)
			{
				return a.z >= b;
			}
			return false;
		}

		public static bool operator <=(Vector3i a, int b)
		{
			if (a.x <= b && a.y <= b)
			{
				return a.z <= b;
			}
			return false;
		}

		public static Vector3i operator -(Vector3i a, Vector3i b)
		{
			return new Vector3i(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static Vector3i operator +(Vector3i a, Vector3i b)
		{
			return new Vector3i(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vector3i operator *(Vector3i a, int b)
		{
			return new Vector3i(a.x * b, a.y * b, a.z * b);
		}

		public static Vector3i operator /(Vector3i a, int b)
		{
			return new Vector3i(a.x / b, a.y / b, a.z / b);
		}

		public static Vector3i operator %(Vector3i a, int b)
		{
			return new Vector3i(a.x % b, a.y % b, a.z % b);
		}

		public static Vector3 operator *(Vector3i a, Vector3 b)
		{
			return new Vector3((float)a.x * b.x, (float)a.y * b.y, (float)a.z * b.z);
		}

		public static explicit operator Vector3(Vector3i v)
		{
			return new Vector3(v.x, v.y, v.z);
		}
	}
}
