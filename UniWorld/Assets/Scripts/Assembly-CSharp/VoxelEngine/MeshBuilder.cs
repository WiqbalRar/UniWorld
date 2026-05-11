using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine
{
	public class MeshBuilder
	{
		public enum Direction
		{
			Left = 0,
			Right = 1,
			Down = 2,
			Up = 3,
			Back = 4,
			Forward = 5
		}

		public enum MeshType
		{
			Basic = 0,
			AmbientOcclusion = 1,
			Gradient = 2
		}

		public struct Quad
		{
			public Vector3 v0;

			public Vector3 v1;

			public Vector3 v2;

			public Vector3 v3;

			public Color32 color;

			public Direction direction;

			public int i0;

			public int i1;

			public int i2;

			public int i3;

			public Quad(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3, Color32 color, Direction direction)
			{
				this.v0 = v0;
				this.v1 = v1;
				this.v2 = v2;
				this.v3 = v3;
				this.color = color;
				this.direction = direction;
				i0 = (i1 = (i2 = (i3 = 0)));
			}

			public Quad(int i0, int i1, int i2, int i3, Color32 color)
			{
				this.i0 = i0;
				this.i1 = i1;
				this.i2 = i2;
				this.i3 = i3;
				this.color = color;
				direction = Direction.Left;
				v0 = (v1 = (v2 = (v3 = Vector3.zero)));
			}
		}

		private const int ADJ_BIT_SIZE = 6;

		private const int ADJ_SIZE = 64;

		private const int ADJ_VOXEL_STEP_X = 4096;

		private const int ADJ_VOXEL_STEP_Y = 64;

		private const int ADJ_VOXEL_STEP_Z = 1;

		public static readonly Vector3[] directionNormals = new Vector3[6]
		{
			Vector3.left,
			Vector3.right,
			Vector3.down,
			Vector3.up,
			Vector3.back,
			Vector3.forward
		};

		private static Chunk chunk;

		private static List<Quad> quads = new List<Quad>();

		private static Dictionary<Vector3, int> lightLevels = new Dictionary<Vector3, int>();

		private static Dictionary<int, Vector3> gradientVerts = new Dictionary<int, Vector3>();

		public static void Clean()
		{
			chunk = null;
			quads.Clear();
			lightLevels.Clear();
			gradientVerts.Clear();
		}

		public static Mesh BuildMesh(Chunk chunk, MeshType meshType)
		{
			MeshBuilder.chunk = chunk;
			Mesh mesh = null;
			mesh = meshType switch
			{
				MeshType.Basic => BasicMesh(), 
				MeshType.AmbientOcclusion => AmbientOcclusionMesh(), 
				MeshType.Gradient => GradientMesh(), 
				_ => throw new ArgumentOutOfRangeException("meshType", meshType, null), 
			};
			Clean();
			return mesh;
		}

		private static Mesh BasicMesh()
		{
			TerrainGeneratorBase terrainGenerator = chunk.voxelEngineManager.terrainGenerator;
			int num = -1;
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					for (int k = 0; k < 32; k++)
					{
						Voxel voxel = chunk.voxelData[++num];
						Voxel adjVoxelLeft = GetAdjVoxelLeft(num, i);
						Voxel adjVoxelDown = GetAdjVoxelDown(num, j);
						Voxel adjVoxelBack = GetAdjVoxelBack(num, k);
						if (voxel.IsSolid())
						{
							Color32 color = default(Color32);
							bool flag = false;
							if (!adjVoxelLeft.IsSolid())
							{
								flag = true;
								color = terrainGenerator.DensityColor(voxel);
								quads.Add(new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), color, Direction.Left));
							}
							if (!adjVoxelDown.IsSolid())
							{
								if (!flag)
								{
									flag = true;
									color = terrainGenerator.DensityColor(voxel);
								}
								quads.Add(new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), color, Direction.Down));
							}
							if (!adjVoxelBack.IsSolid())
							{
								if (!flag)
								{
									color = terrainGenerator.DensityColor(voxel);
								}
								quads.Add(new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), color, Direction.Back));
							}
						}
						else
						{
							if (adjVoxelLeft.IsSolid())
							{
								quads.Add(new Quad(new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), terrainGenerator.DensityColor(adjVoxelLeft), Direction.Right));
							}
							if (adjVoxelDown.IsSolid())
							{
								quads.Add(new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), terrainGenerator.DensityColor(adjVoxelDown), Direction.Up));
							}
							if (adjVoxelBack.IsSolid())
							{
								quads.Add(new Quad(new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), terrainGenerator.DensityColor(adjVoxelBack), Direction.Forward));
							}
						}
					}
				}
			}
			if (quads.Count == 0)
			{
				return null;
			}
			Vector3[] array = new Vector3[quads.Count * 4];
			Vector3[] array2 = new Vector3[quads.Count * 4];
			Color32[] array3 = new Color32[quads.Count * 4];
			int[] array4 = new int[quads.Count * 6];
			int num2 = 0;
			int num3 = 0;
			foreach (Quad quad in quads)
			{
				array4[num3++] = num2;
				array4[num3++] = num2 + 1;
				array4[num3++] = num2 + 2;
				array4[num3++] = num2;
				array4[num3++] = num2 + 2;
				array4[num3++] = num2 + 3;
				array3[num2] = quad.color;
				array2[num2] = directionNormals[(int)quad.direction];
				array[num2++] = quad.v0;
				array3[num2] = quad.color;
				array2[num2] = directionNormals[(int)quad.direction];
				array[num2++] = quad.v1;
				array3[num2] = quad.color;
				array2[num2] = directionNormals[(int)quad.direction];
				array[num2++] = quad.v2;
				array3[num2] = quad.color;
				array2[num2] = directionNormals[(int)quad.direction];
				array[num2++] = quad.v3;
			}
			return new Mesh
			{
				vertices = array,
				normals = array2,
				triangles = array4,
				colors32 = array3
			};
		}

		private static Mesh AmbientOcclusionMesh()
		{
			TerrainGeneratorBase terrainGenerator = chunk.voxelEngineManager.terrainGenerator;
			int num = -1058;
			for (int i = -1; i < 31; i++)
			{
				for (int j = -1; j < 31; j++)
				{
					for (int k = -1; k < 31; k++)
					{
						Voxel voxel;
						Voxel voxel2;
						Voxel voxel3;
						Voxel voxel4;
						if (i == -1 || j == -1 || k == -1)
						{
							voxel = GetAdjVoxel(++num, i, j, k);
							voxel2 = GetAdjVoxel(num - 1024, i - 1, j, k);
							voxel3 = GetAdjVoxel(num - 32, i, j - 1, k);
							voxel4 = GetAdjVoxel(num - 1, i, j, k - 1);
						}
						else
						{
							voxel = chunk.voxelData[++num];
							voxel2 = GetAdjVoxelLeft(num, i);
							voxel3 = GetAdjVoxelDown(num, j);
							voxel4 = GetAdjVoxelBack(num, k);
						}
						if (voxel.IsSolid())
						{
							Color32 color = default(Color32);
							bool flag = false;
							if (!voxel2.IsSolid())
							{
								flag = true;
								color = terrainGenerator.DensityColor(voxel);
								Quad item = new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), color, Direction.Left);
								item.i0 = LightLevelX(item.v0, color, j, k, -0.25f);
								item.i1 = LightLevelX(item.v1, color, j, k, -0.25f);
								item.i2 = LightLevelX(item.v2, color, j, k, -0.25f);
								item.i3 = LightLevelX(item.v3, color, j, k, -0.25f);
								quads.Add(item);
							}
							if (!voxel3.IsSolid())
							{
								if (!flag)
								{
									flag = true;
									color = terrainGenerator.DensityColor(voxel);
								}
								Quad item2 = new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), color, Direction.Down);
								item2.i0 = LightLevelY(item2.v0, color, i, k, -0.25f);
								item2.i1 = LightLevelY(item2.v1, color, i, k, -0.25f);
								item2.i2 = LightLevelY(item2.v2, color, i, k, -0.25f);
								item2.i3 = LightLevelY(item2.v3, color, i, k, -0.25f);
								quads.Add(item2);
							}
							if (!voxel4.IsSolid())
							{
								if (!flag)
								{
									color = terrainGenerator.DensityColor(voxel);
								}
								Quad item3 = new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), color, Direction.Back);
								item3.i0 = LightLevelZ(item3.v0, color, i, j, -0.25f);
								item3.i1 = LightLevelZ(item3.v1, color, i, j, -0.25f);
								item3.i2 = LightLevelZ(item3.v2, color, i, j, -0.25f);
								item3.i3 = LightLevelZ(item3.v3, color, i, j, -0.25f);
								quads.Add(item3);
							}
						}
						else
						{
							if (voxel2.IsSolid())
							{
								Quad item4 = new Quad(new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), terrainGenerator.DensityColor(voxel2), Direction.Right);
								item4.i0 = LightLevelX(item4.v0, item4.color, j, k, 0.25f);
								item4.i1 = LightLevelX(item4.v1, item4.color, j, k, 0.25f);
								item4.i2 = LightLevelX(item4.v2, item4.color, j, k, 0.25f);
								item4.i3 = LightLevelX(item4.v3, item4.color, j, k, 0.25f);
								quads.Add(item4);
							}
							if (voxel3.IsSolid())
							{
								Quad item5 = new Quad(new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k + 0.5f), new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), terrainGenerator.DensityColor(voxel3), Direction.Up);
								item5.i0 = LightLevelY(item5.v0, item5.color, i, k, 0.25f);
								item5.i1 = LightLevelY(item5.v1, item5.color, i, k, 0.25f);
								item5.i2 = LightLevelY(item5.v2, item5.color, i, k, 0.25f);
								item5.i3 = LightLevelY(item5.v3, item5.color, i, k, 0.25f);
								quads.Add(item5);
							}
							if (voxel4.IsSolid())
							{
								Quad item6 = new Quad(new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f), new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f), new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f), terrainGenerator.DensityColor(voxel4), Direction.Forward);
								item6.i0 = LightLevelZ(item6.v0, item6.color, i, j, 0.25f);
								item6.i1 = LightLevelZ(item6.v1, item6.color, i, j, 0.25f);
								item6.i2 = LightLevelZ(item6.v2, item6.color, i, j, 0.25f);
								item6.i3 = LightLevelZ(item6.v3, item6.color, i, j, 0.25f);
								quads.Add(item6);
							}
						}
					}
				}
			}
			if (quads.Count == 0)
			{
				return null;
			}
			Vector3[] array = new Vector3[quads.Count * 4];
			Vector3[] array2 = new Vector3[quads.Count * 4];
			Color32[] array3 = new Color32[quads.Count * 4];
			int[] array4 = new int[quads.Count * 6];
			int num2 = 0;
			int num3 = 0;
			foreach (Quad quad in quads)
			{
				if (quad.i0 + quad.i2 < quad.i1 + quad.i3)
				{
					array4[num3++] = num2;
					array4[num3++] = num2 + 1;
					array4[num3++] = num2 + 2;
					array4[num3++] = num2;
					array4[num3++] = num2 + 2;
					array4[num3++] = num2 + 3;
				}
				else
				{
					array4[num3++] = num2 + 1;
					array4[num3++] = num2 + 2;
					array4[num3++] = num2 + 3;
					array4[num3++] = num2 + 1;
					array4[num3++] = num2 + 3;
					array4[num3++] = num2;
				}
				array2[num2] = directionNormals[(int)quad.direction];
				array3[num2] = LightColorAdjust(quad.color, quad.i0);
				array[num2++] = quad.v0;
				array2[num2] = directionNormals[(int)quad.direction];
				array3[num2] = LightColorAdjust(quad.color, quad.i1);
				array[num2++] = quad.v1;
				array2[num2] = directionNormals[(int)quad.direction];
				array3[num2] = LightColorAdjust(quad.color, quad.i2);
				array[num2++] = quad.v2;
				array2[num2] = directionNormals[(int)quad.direction];
				array3[num2] = LightColorAdjust(quad.color, quad.i3);
				array[num2++] = quad.v3;
			}
			return new Mesh
			{
				vertices = array,
				normals = array2,
				triangles = array4,
				colors32 = array3
			};
		}

		private static Color32 LightColorAdjust(Color32 color, int lightLevel)
		{
			if (lightLevel != 0)
			{
				float num = 1f - (float)lightLevel * 0.2f;
				color.r = (byte)((float)(int)color.r * num);
				color.g = (byte)((float)(int)color.g * num);
				color.b = (byte)((float)(int)color.b * num);
			}
			return color;
		}

		private static int LightLevelX(Vector3 vert, Color32 color, int localY, int localZ, float xOffset)
		{
			int value = 0;
			vert.x += xOffset;
			if (!lightLevels.TryGetValue(vert, out value))
			{
				int localX = FastRound(vert.x);
				int num = FastFloor(vert.y);
				int num2 = FastFloor(vert.z);
				int num3 = 0;
				int num4 = 0;
				if (localY == num)
				{
					if (localZ == num2)
					{
						if (GetAdjVoxel(localX, num + 1, num2).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(localX, num, num2 + 1).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(localX, num + 1, num2 + 1).IsSolid())
						{
							num4++;
						}
					}
					else
					{
						if (GetAdjVoxel(localX, num + 1, num2 + 1).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(localX, num, num2).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(localX, num + 1, num2).IsSolid())
						{
							num4++;
						}
					}
				}
				else if (localZ == num2)
				{
					if (GetAdjVoxel(localX, num, num2).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(localX, num + 1, num2 + 1).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(localX, num, num2 + 1).IsSolid())
					{
						num4++;
					}
				}
				else
				{
					if (GetAdjVoxel(localX, num, num2 + 1).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(localX, num + 1, num2).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(localX, num, num2).IsSolid())
					{
						num4++;
					}
				}
				value = ((num3 != 2) ? (num3 + num4) : 3);
				lightLevels.Add(vert, value);
			}
			return value;
		}

		private static int LightLevelY(Vector3 vert, Color32 color, int localX, int localZ, float yOffset)
		{
			int value = 0;
			vert.y += yOffset;
			if (!lightLevels.TryGetValue(vert, out value))
			{
				int num = FastFloor(vert.x);
				int localY = FastRound(vert.y);
				int num2 = FastFloor(vert.z);
				int num3 = 0;
				int num4 = 0;
				if (localX == num)
				{
					if (localZ == num2)
					{
						if (GetAdjVoxel(num + 1, localY, num2).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num, localY, num2 + 1).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num + 1, localY, num2 + 1).IsSolid())
						{
							num4++;
						}
					}
					else
					{
						if (GetAdjVoxel(num + 1, localY, num2 + 1).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num, localY, num2).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num + 1, localY, num2).IsSolid())
						{
							num4++;
						}
					}
				}
				else if (localZ == num2)
				{
					if (GetAdjVoxel(num, localY, num2).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num + 1, localY, num2 + 1).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num, localY, num2 + 1).IsSolid())
					{
						num4++;
					}
				}
				else
				{
					if (GetAdjVoxel(num, localY, num2 + 1).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num + 1, localY, num2).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num, localY, num2).IsSolid())
					{
						num4++;
					}
				}
				value = ((num3 != 2) ? (num3 + num4) : 3);
				lightLevels.Add(vert, value);
			}
			return value;
		}

		private static int LightLevelZ(Vector3 vert, Color32 color, int localX, int localY, float zOffset)
		{
			int value = 0;
			vert.z += zOffset;
			if (!lightLevels.TryGetValue(vert, out value))
			{
				int num = FastFloor(vert.x);
				int num2 = FastFloor(vert.y);
				int localZ = FastRound(vert.z);
				int num3 = 0;
				int num4 = 0;
				if (localX == num)
				{
					if (localY == num2)
					{
						if (GetAdjVoxel(num + 1, num2, localZ).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num, num2 + 1, localZ).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num + 1, num2 + 1, localZ).IsSolid())
						{
							num4++;
						}
					}
					else
					{
						if (GetAdjVoxel(num + 1, num2 + 1, localZ).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num, num2, localZ).IsSolid())
						{
							num3++;
						}
						if (GetAdjVoxel(num + 1, num2, localZ).IsSolid())
						{
							num4++;
						}
					}
				}
				else if (localY == num2)
				{
					if (GetAdjVoxel(num, num2, localZ).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num + 1, num2 + 1, localZ).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num, num2 + 1, localZ).IsSolid())
					{
						num4++;
					}
				}
				else
				{
					if (GetAdjVoxel(num, num2 + 1, localZ).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num + 1, num2, localZ).IsSolid())
					{
						num3++;
					}
					if (GetAdjVoxel(num, num2, localZ).IsSolid())
					{
						num4++;
					}
				}
				value = ((num3 != 2) ? (num3 + num4) : 3);
				lightLevels.Add(vert, value);
			}
			return value;
		}

		private static int FastFloor(float f)
		{
			if (!(f >= 0f))
			{
				return (int)f - 1;
			}
			return (int)f;
		}

		private static int FastRound(float f)
		{
			if (!(f >= 0f))
			{
				return (int)(f - 0.5f);
			}
			return (int)(f + 0.5f);
		}

		private static Mesh GradientMesh()
		{
			TerrainGeneratorBase terrainGenerator = chunk.voxelEngineManager.terrainGenerator;
			int num = -1058;
			for (int i = -1; i < 31; i++)
			{
				for (int j = -1; j < 31; j++)
				{
					for (int k = -1; k < 31; k++)
					{
						int num2 = -1;
						Voxel voxel;
						Voxel voxel2;
						Voxel voxel3;
						Voxel voxel4;
						if (i == -1 || j == -1 || k == -1)
						{
							voxel = GetAdjVoxel(++num, i, j, k);
							voxel2 = GetAdjVoxel(num - 1024, i - 1, j, k);
							voxel3 = GetAdjVoxel(num - 32, i, j - 1, k);
							voxel4 = GetAdjVoxel(num - 1, i, j, k - 1);
						}
						else
						{
							voxel = chunk.voxelData[++num];
							voxel2 = GetAdjVoxelLeft(num, i);
							voxel3 = GetAdjVoxelDown(num, j);
							voxel4 = GetAdjVoxelBack(num, k);
						}
						if (voxel.IsSolid())
						{
							Color32 color = default(Color32);
							bool flag = false;
							if (!voxel2.IsSolid())
							{
								flag = true;
								color = terrainGenerator.DensityColor(voxel);
								if (num2 == -1)
								{
									num2 = AdjIndex(i, j, k);
								}
								Quad item = new Quad(num2, num2 + 1, num2 + 64 + 1, num2 + 64, color);
								if (!gradientVerts.ContainsKey(item.i0))
								{
									gradientVerts[item.i0] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j - 1, k - 1);
								}
								if (!gradientVerts.ContainsKey(item.i1))
								{
									gradientVerts[item.i1] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f) + VoxelGradient(i - 1, j - 1, k);
								}
								if (!gradientVerts.ContainsKey(item.i2))
								{
									gradientVerts[item.i2] = new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k + 0.5f) + VoxelGradient(i - 1, j, k);
								}
								if (!gradientVerts.ContainsKey(item.i3))
								{
									gradientVerts[item.i3] = new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j, k - 1);
								}
								quads.Add(item);
							}
							if (!voxel3.IsSolid())
							{
								if (!flag)
								{
									flag = true;
									color = terrainGenerator.DensityColor(voxel);
								}
								if (num2 == -1)
								{
									num2 = AdjIndex(i, j, k);
								}
								Quad item2 = new Quad(num2, num2 + 4096, num2 + 4096 + 1, num2 + 1, color);
								if (!gradientVerts.ContainsKey(item2.i0))
								{
									gradientVerts[item2.i0] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j - 1, k - 1);
								}
								if (!gradientVerts.ContainsKey(item2.i1))
								{
									gradientVerts[item2.i1] = new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i, j - 1, k - 1);
								}
								if (!gradientVerts.ContainsKey(item2.i2))
								{
									gradientVerts[item2.i2] = new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k + 0.5f) + VoxelGradient(i, j - 1, k);
								}
								if (!gradientVerts.ContainsKey(item2.i3))
								{
									gradientVerts[item2.i3] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f) + VoxelGradient(i - 1, j - 1, k);
								}
								quads.Add(item2);
							}
							if (!voxel4.IsSolid())
							{
								if (!flag)
								{
									color = terrainGenerator.DensityColor(voxel);
								}
								if (num2 == -1)
								{
									num2 = AdjIndex(i, j, k);
								}
								Quad item3 = new Quad(num2, num2 + 64, num2 + 4096 + 64, num2 + 4096, color);
								if (!gradientVerts.ContainsKey(item3.i0))
								{
									gradientVerts[item3.i0] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j - 1, k - 1);
								}
								if (!gradientVerts.ContainsKey(item3.i1))
								{
									gradientVerts[item3.i1] = new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j, k - 1);
								}
								if (!gradientVerts.ContainsKey(item3.i2))
								{
									gradientVerts[item3.i2] = new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k - 0.5f) + VoxelGradient(i, j, k - 1);
								}
								if (!gradientVerts.ContainsKey(item3.i3))
								{
									gradientVerts[item3.i3] = new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i, j - 1, k - 1);
								}
								quads.Add(item3);
							}
							continue;
						}
						if (voxel2.IsSolid())
						{
							if (num2 == -1)
							{
								num2 = AdjIndex(i, j, k);
							}
							Quad item4 = new Quad(num2 + 64, num2 + 64 + 1, num2 + 1, num2, terrainGenerator.DensityColor(voxel2));
							if (!gradientVerts.ContainsKey(item4.i0))
							{
								gradientVerts[item4.i0] = new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j, k - 1);
							}
							if (!gradientVerts.ContainsKey(item4.i1))
							{
								gradientVerts[item4.i1] = new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k + 0.5f) + VoxelGradient(i - 1, j, k);
							}
							if (!gradientVerts.ContainsKey(item4.i2))
							{
								gradientVerts[item4.i2] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f) + VoxelGradient(i - 1, j - 1, k);
							}
							if (!gradientVerts.ContainsKey(item4.i3))
							{
								gradientVerts[item4.i3] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j - 1, k - 1);
							}
							quads.Add(item4);
						}
						if (voxel3.IsSolid())
						{
							if (num2 == -1)
							{
								num2 = AdjIndex(i, j, k);
							}
							Quad item5 = new Quad(num2 + 1, num2 + 4096 + 1, num2 + 4096, num2, terrainGenerator.DensityColor(voxel3));
							if (!gradientVerts.ContainsKey(item5.i0))
							{
								gradientVerts[item5.i0] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k + 0.5f) + VoxelGradient(i - 1, j - 1, k);
							}
							if (!gradientVerts.ContainsKey(item5.i1))
							{
								gradientVerts[item5.i1] = new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k + 0.5f) + VoxelGradient(i, j - 1, k);
							}
							if (!gradientVerts.ContainsKey(item5.i2))
							{
								gradientVerts[item5.i2] = new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i, j - 1, k - 1);
							}
							if (!gradientVerts.ContainsKey(item5.i3))
							{
								gradientVerts[item5.i3] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j - 1, k - 1);
							}
							quads.Add(item5);
						}
						if (voxel4.IsSolid())
						{
							if (num2 == -1)
							{
								num2 = AdjIndex(i, j, k);
							}
							Quad item6 = new Quad(num2 + 4096, num2 + 4096 + 64, num2 + 64, num2, terrainGenerator.DensityColor(voxel4));
							if (!gradientVerts.ContainsKey(item6.i0))
							{
								gradientVerts[item6.i0] = new Vector3((float)i + 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i, j - 1, k - 1);
							}
							if (!gradientVerts.ContainsKey(item6.i1))
							{
								gradientVerts[item6.i1] = new Vector3((float)i + 0.5f, (float)j + 0.5f, (float)k - 0.5f) + VoxelGradient(i, j, k - 1);
							}
							if (!gradientVerts.ContainsKey(item6.i2))
							{
								gradientVerts[item6.i2] = new Vector3((float)i - 0.5f, (float)j + 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j, k - 1);
							}
							if (!gradientVerts.ContainsKey(item6.i3))
							{
								gradientVerts[item6.i3] = new Vector3((float)i - 0.5f, (float)j - 0.5f, (float)k - 0.5f) + VoxelGradient(i - 1, j - 1, k - 1);
							}
							quads.Add(item6);
						}
					}
				}
			}
			if (quads.Count == 0)
			{
				return null;
			}
			Vector3[] array = new Vector3[quads.Count * 4];
			Color32[] array2 = new Color32[quads.Count * 4];
			int[] array3 = new int[quads.Count * 6];
			int num3 = 0;
			int num4 = 0;
			foreach (Quad quad in quads)
			{
				array2[num3] = quad.color;
				array[num3++] = gradientVerts[quad.i0];
				array2[num3] = quad.color;
				array[num3++] = gradientVerts[quad.i1];
				array2[num3] = quad.color;
				array[num3++] = gradientVerts[quad.i2];
				array2[num3] = quad.color;
				array[num3++] = gradientVerts[quad.i3];
				if ((array[num3 - 4] - array[num3 - 2]).sqrMagnitude < (array[num3 - 3] - array[num3 - 1]).sqrMagnitude)
				{
					array3[num4++] = num3 - 4;
					array3[num4++] = num3 - 3;
					array3[num4++] = num3 - 2;
					array3[num4++] = num3 - 4;
					array3[num4++] = num3 - 2;
					array3[num4++] = num3 - 1;
				}
				else
				{
					array3[num4++] = num3 - 3;
					array3[num4++] = num3 - 2;
					array3[num4++] = num3 - 1;
					array3[num4++] = num3 - 3;
					array3[num4++] = num3 - 1;
					array3[num4++] = num3 - 4;
				}
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.triangles = array3;
			mesh.colors32 = array2;
			mesh.RecalculateNormals();
			return mesh;
		}

		private static Vector3 VoxelGradient(int localX, int localY, int localZ)
		{
			int num = localX * 1024 + localY * 32 + localZ;
			return Gradient(GetAdjVoxel(num, localX, localY, localZ).density, GetAdjVoxel(num + 1024, localX + 1, localY, localZ).density, GetAdjVoxel(num + 32, localX, localY + 1, localZ).density, GetAdjVoxel(num + 1024 + 32, localX + 1, localY + 1, localZ).density, GetAdjVoxel(++num, localX, localY, localZ + 1).density, GetAdjVoxel(num + 1024, localX + 1, localY, localZ + 1).density, GetAdjVoxel(num + 32, localX, localY + 1, localZ + 1).density, GetAdjVoxel(num + 1024 + 32, localX + 1, localY + 1, localZ + 1).density);
		}

		private static Vector3 Gradient(float a, float b, float c, float d, float e, float f, float g, float h)
		{
			float num = (a + b + c + d + e + f + g + h) * -0.125f;
			Vector3 vector = new Vector3(0f - a + b - c + d - e + f - g + h, 0f - a - b + c + d - e - f + g + h, 0f - a - b - c - d + e + f + g + h);
			vector *= 0.25f;
			num /= vector.sqrMagnitude;
			return vector * num;
		}

		private static int AdjIndex(int localX, int localY, int localZ)
		{
			return (localZ + 1) | (localY + 1 << 6) | (localX + 1 << 12);
		}

		private static Voxel GetAdjVoxel(int voxelIndex, int localX, int localY, int localZ)
		{
			int num = -2;
			if (localX < 0)
			{
				num += 2;
				voxelIndex += 32768;
			}
			if (localY < 0)
			{
				num += 3;
				voxelIndex += 1024;
			}
			if (localZ < 0)
			{
				num += 4;
				voxelIndex += 32;
			}
			if (num == -2)
			{
				return chunk.voxelData[voxelIndex];
			}
			return chunk.adjChunks[Math.Min(num, 6)].voxelData[voxelIndex];
		}

		private static Voxel GetAdjVoxel(int localX, int localY, int localZ)
		{
			int num = -2;
			if (localX < 0)
			{
				num += 2;
				localX += 32;
			}
			if (localY < 0)
			{
				num += 3;
				localY += 32;
			}
			if (localZ < 0)
			{
				num += 4;
				localZ += 32;
			}
			if (num == -2)
			{
				return chunk.GetVoxelUnsafe(localX, localY, localZ);
			}
			return chunk.adjChunks[Math.Min(num, 6)].GetVoxelUnsafe(localX, localY, localZ);
		}

		private static Voxel GetAdjVoxelLeft(int voxelIndex, int localX)
		{
			voxelIndex -= 1024;
			if (localX > 0)
			{
				return chunk.voxelData[voxelIndex];
			}
			voxelIndex += 32768;
			return chunk.adjChunks[0].voxelData[voxelIndex];
		}

		private static Voxel GetAdjVoxelDown(int voxelIndex, int localY)
		{
			voxelIndex -= 32;
			if (localY > 0)
			{
				return chunk.voxelData[voxelIndex];
			}
			voxelIndex += 1024;
			return chunk.adjChunks[1].voxelData[voxelIndex];
		}

		private static Voxel GetAdjVoxelBack(int voxelIndex, int localZ)
		{
			voxelIndex--;
			if (localZ > 0)
			{
				return chunk.voxelData[voxelIndex];
			}
			voxelIndex += 32;
			return chunk.adjChunks[2].voxelData[voxelIndex];
		}
	}
}
