using System.Collections.Generic;
using UnityEngine;

public static class GreedyMesher
{
	public static void GenerateSolidSimpleMesh(byte[] voxels, ref Vector3[] verticesArray, ref int[] trianglesArray, ref Vector3[] uvsArray, Chunk.Neighbours neighbours, Chunk chunk)
	{
		int wIDTH = Chunk.WIDTH;
		int hEIGHT = Chunk.HEIGHT;
		int wIDTHxHEIGHT = Chunk.WIDTHxHEIGHT;
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector3> list3 = new List<Vector3>();
		for (int i = 0; i < wIDTH; i++)
		{
			for (int j = 0; j < hEIGHT; j++)
			{
				for (int k = 0; k < wIDTH; k++)
				{
					int num = i * wIDTHxHEIGHT + j * wIDTH + k;
					byte b = voxels[num];
					if (b != 0 && b < 241)
					{
						Vector3Int localPositionToCheck = new Vector3Int(i - 1, j, k);
						Chunk chunkToCheck;
						if (localPositionToCheck.x < 0)
						{
							localPositionToCheck.x = 15;
							chunkToCheck = neighbours.left;
						}
						else
						{
							chunkToCheck = chunk;
						}
						if (IsVisible(b, chunkToCheck, localPositionToCheck))
						{
							Vector3 item = new Vector3(i, j + 1, k + 1);
							Vector3 item2 = new Vector3(i, j + 1, k);
							Vector3 item3 = new Vector3(i, j, k + 1);
							Vector3 item4 = new Vector3(i, j, k);
							list.Add(item);
							list.Add(item2);
							list.Add(item3);
							list3.Add(new Vector3(0f, 1f, (int)b));
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item2);
							list.Add(item4);
							list.Add(item3);
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(1f, 0f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						localPositionToCheck = new Vector3Int(i + 1, j, k);
						if (localPositionToCheck.x == 16)
						{
							localPositionToCheck.x = 0;
							chunkToCheck = neighbours.right;
						}
						else
						{
							chunkToCheck = chunk;
						}
						if (IsVisible(b, chunkToCheck, localPositionToCheck))
						{
							Vector3 item5 = new Vector3(i + 1, j + 1, k);
							Vector3 item6 = new Vector3(i + 1, j + 1, k + 1);
							Vector3 item7 = new Vector3(i + 1, j, k);
							Vector3 item8 = new Vector3(i + 1, j, k + 1);
							list.Add(item5);
							list.Add(item6);
							list.Add(item7);
							list3.Add(new Vector3(0f, 1f, (int)b));
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item6);
							list.Add(item8);
							list.Add(item7);
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(1f, 0f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						localPositionToCheck = new Vector3Int(i, j, k - 1);
						if (localPositionToCheck.z < 0)
						{
							localPositionToCheck.z = 15;
							chunkToCheck = neighbours.back;
						}
						else
						{
							chunkToCheck = chunk;
						}
						if (IsVisible(b, chunkToCheck, localPositionToCheck))
						{
							Vector3 item9 = new Vector3(i, j + 1, k);
							Vector3 item10 = new Vector3(i + 1, j + 1, k);
							Vector3 item11 = new Vector3(i, j, k);
							Vector3 item12 = new Vector3(i + 1, j, k);
							list.Add(item9);
							list.Add(item10);
							list.Add(item11);
							list3.Add(new Vector3(0f, 1f, (int)b));
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item10);
							list.Add(item12);
							list.Add(item11);
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(1f, 0f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						localPositionToCheck = new Vector3Int(i, j, k + 1);
						if (localPositionToCheck.z == 16)
						{
							localPositionToCheck.z = 0;
							chunkToCheck = neighbours.front;
						}
						else
						{
							chunkToCheck = chunk;
						}
						if (IsVisible(b, chunkToCheck, localPositionToCheck))
						{
							Vector3 item13 = new Vector3(i + 1, j + 1, k + 1);
							Vector3 item14 = new Vector3(i, j + 1, k + 1);
							Vector3 item15 = new Vector3(i + 1, j, k + 1);
							Vector3 item16 = new Vector3(i, j, k + 1);
							list.Add(item13);
							list.Add(item14);
							list.Add(item15);
							list3.Add(new Vector3(0f, 1f, (int)b));
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item14);
							list.Add(item16);
							list.Add(item15);
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(1f, 0f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						localPositionToCheck = new Vector3Int(i, j - 1, k);
						chunkToCheck = chunk;
						if (localPositionToCheck.y == -1 || IsVisible(b, chunkToCheck, localPositionToCheck))
						{
							Vector3 item17 = new Vector3(i, j, k);
							Vector3 item18 = new Vector3((float)i + 1f, j, k);
							Vector3 item19 = new Vector3(i, j, k + 1);
							Vector3 item20 = new Vector3((float)i + 1f, j, k + 1);
							list.Add(item17);
							list.Add(item18);
							list.Add(item19);
							list3.Add(new Vector3(0f, 1f, (int)b));
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item18);
							list.Add(item20);
							list.Add(item19);
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(1f, 0f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						localPositionToCheck = new Vector3Int(i, j + 1, k);
						if (localPositionToCheck.y == Chunk.HEIGHT || IsVisible(b, chunkToCheck, localPositionToCheck))
						{
							Vector3 item21 = new Vector3(i, j + 1, k + 1);
							Vector3 item22 = new Vector3((float)i + 1f, j + 1, k + 1);
							Vector3 item23 = new Vector3(i, j + 1, k);
							Vector3 item24 = new Vector3((float)i + 1f, j + 1, k);
							list.Add(item21);
							list.Add(item22);
							list.Add(item23);
							list3.Add(new Vector3(0f, 1f, (int)b));
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item22);
							list.Add(item24);
							list.Add(item23);
							list3.Add(new Vector3(1f, 1f, (int)b));
							list3.Add(new Vector3(1f, 0f, (int)b));
							list3.Add(new Vector3(0f, 0f, (int)b));
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
					}
				}
			}
		}
		AppendSpecialBlocks(chunk, list, list3, list2);
		verticesArray = list.ToArray();
		trianglesArray = list2.ToArray();
		uvsArray = list3.ToArray();
		list.Clear();
		list2.Clear();
		list3.Clear();
	}

	private static bool IsVisible(int blockType, Chunk chunkToCheck, Vector3Int localPositionToCheck)
	{
		if (blockType > 0)
		{
			int num = chunkToCheck.voxels[localPositionToCheck.x * Chunk.WIDTHxHEIGHT + localPositionToCheck.y * Chunk.WIDTH + localPositionToCheck.z];
			if (blockType >= 22 && blockType <= 100 && (num <= 21 || (num >= 94 && num <= 100) || num >= 241 || (num >= 101 && num <= 126)))
			{
				if (blockType >= 100 && blockType <= 100)
				{
					if (num >= 100)
					{
						return num > 100;
					}
					return true;
				}
				return true;
			}
		}
		return false;
	}

	private static void AppendSpecialBlocks(Chunk chunk, List<Vector3> vertices, List<Vector3> uvs, List<int> triangles)
	{
		foreach (KeyValuePair<int, byte> item in chunk.blockOrientation)
		{
			byte b = chunk.voxels[item.Key];
			if (b >= 101 && b <= 113)
			{
				AppendStairs(b, item.Key, item.Value, vertices, uvs, triangles);
			}
			if (b >= 114 && b <= 126)
			{
				AppendSlab(b, item.Key, item.Value, vertices, uvs, triangles);
			}
			else if (b >= 21 && b <= 21)
			{
				AppendLightTorches(b, item.Key, item.Value, vertices, uvs, triangles);
			}
		}
		foreach (int plant in chunk.plants)
		{
			AppendPlant(chunk, chunk.voxels[plant], plant, vertices, uvs, triangles);
		}
	}

	private static void AppendStairs(byte stairId, int arrayIndex, byte orientation, List<Vector3> vertices, List<Vector3> uvs, List<int> triangles)
	{
		Vector3 vector = Precomputed.indexCoords[arrayIndex];
		vector += Vector3.one / 2f;
		new Vector2(stairId - 29, 0f);
		Quaternion quaternion = VoxelBounds.stairRotations[orientation];
		Vector3 vector2 = new Vector3(0.5f, 0.5f, (int)stairId);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsBot[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsBot[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsBot[orientation] * MeshHelper.uvCoords[0] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsBot[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsBot[orientation] * MeshHelper.uvCoords[8] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsBot[orientation] * MeshHelper.uvCoords[2] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[12]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[14]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[14]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[5] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[2] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[21]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[24]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[26]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[8] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[21]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[26]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[23]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[8] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsTop[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[12]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[4] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[3]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[12]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[1] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[4] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[3]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[24]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[21]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[1] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[7] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[3]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[24]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[1] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[6] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[14]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[4] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[14]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[5]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[4] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[1] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[5]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[23]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[26]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[1] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[7] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[8] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[5]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[26]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[1] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[8] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[2] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[5] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[2] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[12]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[21]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[23]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[8] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[12]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[23]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[14]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[8] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[26]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[24]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[8] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[26]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.stairUvRotationsSide[orientation] * MeshHelper.uvCoords[6] + vector2);
	}

	private static void AppendSlab(byte slabID, int arrayIndex, byte orientation, List<Vector3> vertices, List<Vector3> uvs, List<int> triangles)
	{
		Vector3 vector = Precomputed.indexCoords[arrayIndex];
		vector += Vector3.one / 2f;
		new Vector2(slabID - 42, 0f);
		Quaternion quaternion = MeshHelper.slabRotation[orientation];
		Vector3 vector2 = new Vector3(0.5f, 0.5f, (int)slabID);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsBot[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBot[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBot[orientation] * MeshHelper.uvCoords[8] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsBot[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBot[orientation] * MeshHelper.uvCoords[8] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBot[orientation] * MeshHelper.uvCoords[2] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[15]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[17]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsTop[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsTop[orientation] * MeshHelper.uvCoords[6] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsTop[orientation] * MeshHelper.uvCoords[8] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[17]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsTop[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsTop[orientation] * MeshHelper.uvCoords[8] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsTop[orientation] * MeshHelper.uvCoords[2] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[15]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsLeft[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsLeft[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsLeft[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[15]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsLeft[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsLeft[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsLeft[orientation] * MeshHelper.uvCoords[3] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[17]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsRight[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsRight[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsRight[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsRight[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsRight[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsRight[orientation] * MeshHelper.uvCoords[3] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[11]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsFront[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsFront[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsFront[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[9]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsFront[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsFront[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsFront[orientation] * MeshHelper.uvCoords[3] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[17]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[15]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsBack[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBack[orientation] * MeshHelper.uvCoords[3] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBack[orientation] * MeshHelper.uvCoords[5] + vector2);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[8]);
		vertices.Add(vector + quaternion * MeshHelper.stairVertices[17]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(MeshHelper.slabUvRotationsBack[orientation] * MeshHelper.uvCoords[2] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBack[orientation] * MeshHelper.uvCoords[0] + vector2);
		uvs.Add(MeshHelper.slabUvRotationsBack[orientation] * MeshHelper.uvCoords[3] + vector2);
	}

	private static void AppendLightTorches(byte torchId, int arrayIndex, byte orientation, List<Vector3> vertices, List<Vector3> uvs, List<int> triangles)
	{
		Vector3 vector = Precomputed.indexCoords[arrayIndex];
		Quaternion quaternion = MeshHelper.torchRotation[orientation];
		Vector3 vector2 = MeshHelper.torchTranslation[orientation];
		vector += Vector3.one / 2f + vector2;
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[4]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[5]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[0]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[5]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[1]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[1]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[5]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[7]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[1]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[7]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[3]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[3]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[7]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[6]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[3]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[2]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[4]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[4]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[0]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[4]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[6]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[7]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0.5f, (int)torchId));
		uvs.Add(new Vector3(0f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[4]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[7]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[5]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0.5f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.625f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.5f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[1]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[3]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[2]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0f, 0.125f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.125f, (int)torchId));
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[1]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[2]);
		vertices.Add(vector + quaternion * MeshHelper.torchVertices[0]);
		triangles.Add(vertices.Count - 3);
		triangles.Add(vertices.Count - 2);
		triangles.Add(vertices.Count - 1);
		uvs.Add(new Vector3(0f, 0f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0.125f, (int)torchId));
		uvs.Add(new Vector3(0.125f, 0f, (int)torchId));
	}

	private static void AppendPlant(Chunk chunk, byte plantId, int arrayIndex, List<Vector3> vertices, List<Vector3> uvs, List<int> triangles)
	{
		Vector3 vector = Precomputed.indexCoords[arrayIndex];
		float num = chunk.noiseSetWhite1[(int)(vector.x * (float)Chunk.WIDTH + vector.z)];
		float num2 = 1f - Mathf.Abs(num % 0.2f);
		int count = vertices.Count;
		if (plantId == 1)
		{
			vector += new Vector3(num * 0.25f, 0f, num % 0.5f / 2f);
			vertices.Add(vector + MeshHelper.grassPlantVertices[0] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[1] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[2] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[3] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[4] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[5] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[6] * num2);
			vertices.Add(vector + MeshHelper.grassPlantVertices[7] * num2);
		}
		else
		{
			vector += new Vector3(num * 0.3125f, 0f, num % 0.1f * 3.125f);
			vertices.Add(vector + MeshHelper.plantVertices[0] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[1] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[2] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[3] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[4] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[5] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[6] * num2);
			vertices.Add(vector + MeshHelper.plantVertices[7] * num2);
		}
		uvs.Add(new Vector3(0f, 0f, (int)plantId));
		uvs.Add(new Vector3(1f, 0f, (int)plantId));
		uvs.Add(new Vector3(0f, 0f, (int)plantId));
		uvs.Add(new Vector3(1f, 0f, (int)plantId));
		uvs.Add(new Vector3(0f, 1f, (int)plantId));
		uvs.Add(new Vector3(1f, 1f, (int)plantId));
		uvs.Add(new Vector3(0f, 1f, (int)plantId));
		uvs.Add(new Vector3(1f, 1f, (int)plantId));
		triangles.Add(count);
		triangles.Add(count + 4);
		triangles.Add(count + 7);
		triangles.Add(count);
		triangles.Add(count + 7);
		triangles.Add(count + 3);
		triangles.Add(count + 3);
		triangles.Add(count + 7);
		triangles.Add(count + 4);
		triangles.Add(count + 3);
		triangles.Add(count + 4);
		triangles.Add(count);
		triangles.Add(count + 2);
		triangles.Add(count + 6);
		triangles.Add(count + 5);
		triangles.Add(count + 2);
		triangles.Add(count + 5);
		triangles.Add(count + 1);
		triangles.Add(count + 1);
		triangles.Add(count + 5);
		triangles.Add(count + 6);
		triangles.Add(count + 1);
		triangles.Add(count + 6);
		triangles.Add(count + 2);
	}

	public static void GenerateWaterMesh(byte[] voxels, ref Vector3[] verticesArray, ref int[] trianglesArray, Chunk chunk)
	{
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.HEIGHT; j++)
			{
				for (int k = 0; k < Chunk.WIDTH; k++)
				{
					int num = voxels[i * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + k];
					if (num < 241)
					{
						continue;
					}
					int num2 = ((j < Chunk.HEIGHT - 1) ? voxels[i * Chunk.HEIGHT * Chunk.WIDTH + (j + 1) * Chunk.WIDTH + k] : 0);
					float num3 = ((num2 >= 241) ? ((float)j + 1f) : ((num >= 248) ? ((float)j + 0.9f) : ((float)j + 0.1125f * ((float)num - 240f))));
					if (num2 < 241)
					{
						Vector3 item = new Vector3(i, num3, k + 1);
						Vector3 item2 = new Vector3((float)i + 1f, num3, k + 1);
						Vector3 item3 = new Vector3(i, num3, k);
						Vector3 item4 = new Vector3((float)i + 1f, num3, k);
						list.Add(item);
						list.Add(item2);
						list.Add(item3);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item2);
						list.Add(item4);
						list.Add(item3);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item3);
						list.Add(item2);
						list.Add(item);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item3);
						list.Add(item4);
						list.Add(item2);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
					}
					num2 = ((j > 0) ? voxels[i * Chunk.HEIGHT * Chunk.WIDTH + (j - 1) * Chunk.WIDTH + k] : 0);
					if (num2 <= 21 || (num2 >= 94 && num2 <= 100) || (num2 >= 101 && num2 <= 126))
					{
						Vector3 item5 = new Vector3(i, j, k);
						Vector3 item6 = new Vector3((float)i + 1f, j, k);
						Vector3 item7 = new Vector3(i, j, k + 1);
						Vector3 item8 = new Vector3((float)i + 1f, j, k + 1);
						list.Add(item5);
						list.Add(item6);
						list.Add(item7);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item6);
						list.Add(item8);
						list.Add(item7);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						if (num2 == 0)
						{
							list.Add(item7);
							list.Add(item6);
							list.Add(item5);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item7);
							list.Add(item8);
							list.Add(item6);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						else
						{
							item5.y += 0.001f;
							item6.y += 0.001f;
							item7.y += 0.001f;
							item8.y += 0.001f;
							list.Add(item7);
							list.Add(item6);
							list.Add(item5);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item7);
							list.Add(item8);
							list.Add(item6);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
					}
					int num4 = i;
					int num5 = k;
					Chunk chunk2;
					if (i - 1 < 0)
					{
						chunk2 = chunk.neighbours.left;
						num4 = 15;
					}
					else
					{
						chunk2 = chunk;
						num4 = i - 1;
					}
					num2 = chunk2.voxels[num4 * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + k];
					float num6 = ((num2 < 241) ? ((float)j) : ((((j < Chunk.HEIGHT - 1) ? chunk2.voxels[num4 * Chunk.HEIGHT * Chunk.WIDTH + (j + 1) * Chunk.WIDTH + k] : 0) >= 241) ? ((float)j + 1f) : ((float)j + 0.1125f * ((num2 == 249) ? 8f : ((float)num2 - 240f)))));
					if (num2 <= 21 || (num2 >= 94 && num2 <= 100) || (num2 >= 101 && num2 <= 126) || (num2 >= 241 && (num2 < num || num6 > num3)))
					{
						Vector3 item9 = new Vector3(i, num3, k + 1);
						Vector3 item10 = new Vector3(i, num3, k);
						Vector3 item11 = new Vector3(i, num6, k + 1);
						Vector3 item12 = new Vector3(i, num6, k);
						list.Add(item9);
						list.Add(item10);
						list.Add(item11);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item10);
						list.Add(item12);
						list.Add(item11);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						if (num2 <= 20 || (num2 >= 241 && (num2 < num || num6 > num3)))
						{
							list.Add(item11);
							list.Add(item10);
							list.Add(item9);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item11);
							list.Add(item12);
							list.Add(item10);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						else
						{
							item9.x += 0.001f;
							item10.x += 0.001f;
							item11.x += 0.001f;
							item12.x += 0.001f;
							list.Add(item11);
							list.Add(item10);
							list.Add(item9);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item11);
							list.Add(item12);
							list.Add(item10);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
					}
					if (i + 1 == Chunk.WIDTH)
					{
						chunk2 = chunk.neighbours.right;
						num4 = 0;
					}
					else
					{
						chunk2 = chunk;
						num4 = i + 1;
					}
					num2 = chunk2.voxels[num4 * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + k];
					num6 = ((num2 < 241) ? ((float)j) : ((((j < Chunk.HEIGHT - 1) ? chunk2.voxels[num4 * Chunk.HEIGHT * Chunk.WIDTH + (j + 1) * Chunk.WIDTH + k] : 0) >= 241) ? ((float)j + 1f) : ((float)j + 0.1125f * ((num2 == 249) ? 8f : ((float)num2 - 240f)))));
					if (num2 <= 21 || (num2 >= 94 && num2 <= 100) || (num2 >= 101 && num2 <= 126) || (num2 >= 241 && (num2 < num || num6 > num3)))
					{
						Vector3 item13 = new Vector3(i + 1, num3, k);
						Vector3 item14 = new Vector3(i + 1, num3, k + 1);
						Vector3 item15 = new Vector3(i + 1, num6, k);
						Vector3 item16 = new Vector3(i + 1, num6, k + 1);
						list.Add(item13);
						list.Add(item14);
						list.Add(item15);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item14);
						list.Add(item16);
						list.Add(item15);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						if (num2 <= 21 || (num2 >= 241 && (num2 < num || num6 > num3)))
						{
							list.Add(item15);
							list.Add(item14);
							list.Add(item13);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item15);
							list.Add(item16);
							list.Add(item14);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						else
						{
							item13.x -= 0.001f;
							item14.x -= 0.001f;
							item15.x -= 0.001f;
							item16.x -= 0.001f;
							list.Add(item15);
							list.Add(item14);
							list.Add(item13);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item15);
							list.Add(item16);
							list.Add(item14);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
					}
					if (k - 1 < 0)
					{
						chunk2 = chunk.neighbours.back;
						num5 = 15;
					}
					else
					{
						chunk2 = chunk;
						num5 = k - 1;
					}
					num2 = chunk2.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + num5];
					num6 = ((num2 < 241) ? ((float)j) : ((((j < Chunk.HEIGHT - 1) ? chunk2.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + (j + 1) * Chunk.WIDTH + num5] : 0) >= 241) ? ((float)j + 1f) : ((float)j + 0.1125f * ((num2 == 249) ? 8f : ((float)num2 - 240f)))));
					if (num2 <= 21 || (num2 >= 94 && num2 <= 100) || (num2 >= 101 && num2 <= 126) || (num2 >= 241 && (num2 < num || num6 > num3)))
					{
						Vector3 item17 = new Vector3(i, num3, k);
						Vector3 item18 = new Vector3(i + 1, num3, k);
						Vector3 item19 = new Vector3(i, num6, k);
						Vector3 item20 = new Vector3(i + 1, num6, k);
						list.Add(item17);
						list.Add(item18);
						list.Add(item19);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item18);
						list.Add(item20);
						list.Add(item19);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						if (num2 <= 21 || (num2 >= 241 && (num2 < num || num6 > num3)))
						{
							list.Add(item19);
							list.Add(item18);
							list.Add(item17);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item19);
							list.Add(item20);
							list.Add(item18);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						else
						{
							item17.z += 0.001f;
							item18.z += 0.001f;
							item19.z += 0.001f;
							item20.z += 0.001f;
							list.Add(item19);
							list.Add(item18);
							list.Add(item17);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item19);
							list.Add(item20);
							list.Add(item18);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
					}
					if (k + 1 == Chunk.WIDTH)
					{
						chunk2 = chunk.neighbours.front;
						num5 = 0;
					}
					else
					{
						chunk2 = chunk;
						num5 = k + 1;
					}
					num2 = chunk2.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + num5];
					num6 = ((num2 < 241) ? ((float)j) : ((((j < Chunk.HEIGHT - 1) ? chunk2.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + (j + 1) * Chunk.WIDTH + num5] : 0) >= 241) ? ((float)j + 1f) : ((float)j + 0.1125f * ((num2 == 249) ? 8f : ((float)num2 - 240f)))));
					if (num2 <= 21 || (num2 >= 94 && num2 <= 100) || (num2 >= 101 && num2 <= 126) || (num2 >= 241 && (num2 < num || num6 > num3)))
					{
						Vector3 item21 = new Vector3(i + 1, num3, k + 1);
						Vector3 item22 = new Vector3(i, num3, k + 1);
						Vector3 item23 = new Vector3(i + 1, num6, k + 1);
						Vector3 item24 = new Vector3(i, num6, k + 1);
						list.Add(item21);
						list.Add(item22);
						list.Add(item23);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						list.Add(item22);
						list.Add(item24);
						list.Add(item23);
						list2.Add(list.Count - 3);
						list2.Add(list.Count - 2);
						list2.Add(list.Count - 1);
						if (num2 <= 21 || (num2 >= 241 && (num2 < num || num6 > num3)))
						{
							list.Add(item23);
							list.Add(item22);
							list.Add(item21);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item23);
							list.Add(item24);
							list.Add(item22);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
						else
						{
							item21.z -= 0.001f;
							item22.z -= 0.001f;
							item23.z -= 0.001f;
							item24.z -= 0.001f;
							list.Add(item23);
							list.Add(item22);
							list.Add(item21);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
							list.Add(item23);
							list.Add(item24);
							list.Add(item22);
							list2.Add(list.Count - 3);
							list2.Add(list.Count - 2);
							list2.Add(list.Count - 1);
						}
					}
				}
			}
		}
		verticesArray = list.ToArray();
		trianglesArray = list2.ToArray();
		list.Clear();
		list2.Clear();
	}
}
