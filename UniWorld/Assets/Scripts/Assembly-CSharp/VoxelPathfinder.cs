using System.Collections.Generic;
using UnityEngine;

public class VoxelPathfinder : MonoBehaviour
{
	public struct Node
	{
		public int g;

		public int h;

		public int f;

		public Vector3Int parent;

		public float rand;

		public Node(int g, int h, Vector3Int parent, float rand)
		{
			this.g = g;
			this.h = h;
			f = g + h;
			this.parent = parent;
			this.rand = rand;
		}
	}

	public struct PathNode
	{
		private Vector3Int position;

		private Vector3Int origin;

		private int remainingDistance;
	}

	[ContextMenu("TestPath")]
	public void TestPath()
	{
	}

	public static int GetPath(Vector3Int start, Vector3Int end, List<Vector3Int> path, int entityHeight, int maxDistance = 100)
	{
		path.Clear();
		Dictionary<Vector3Int, Node> dictionary = new Dictionary<Vector3Int, Node>();
		List<KeyValuePair<Vector3Int, Node>> list = new List<KeyValuePair<Vector3Int, Node>>();
		HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
		while (!IsEntityGrounded(end))
		{
			end.y--;
		}
		if (start == end)
		{
			path.Add(start);
			return 1;
		}
		int num = 0;
		int num2 = 0;
		list.Add(new KeyValuePair<Vector3Int, Node>(start, new Node(0, GetHeuristicValue(start, end), start, num2++)));
		hashSet.Add(start);
		while (list.Count > 0 && num <= 500)
		{
			num++;
			list.Sort(NodeComparer);
			KeyValuePair<Vector3Int, Node> keyValuePair = list[0];
			if (keyValuePair.Key == end)
			{
				path.Add(keyValuePair.Key);
				Vector3Int parent = keyValuePair.Value.parent;
				Vector3Int vector3Int = keyValuePair.Key;
				while (parent != vector3Int)
				{
					vector3Int = parent;
					parent = dictionary[vector3Int].parent;
					path.Add(vector3Int);
				}
				return num;
			}
			if (keyValuePair.Value.g < maxDistance && keyValuePair.Value.g + GetManhattanDistance(keyValuePair.Key, end) < maxDistance && IsEntityGrounded(keyValuePair.Key))
			{
				Vector3Int vector3Int2 = keyValuePair.Key - Vector3Int.right;
				int yOffset = 0;
				if (CanEntityMoveSideWays(keyValuePair.Key, vector3Int2, ref yOffset, entityHeight))
				{
					int num3 = 0;
					vector3Int2.y += yOffset;
					num3 += 1 + yOffset;
					while (!IsEntityGrounded(vector3Int2))
					{
						vector3Int2.y--;
						num3++;
					}
					if (!hashSet.Contains(vector3Int2) && !dictionary.ContainsKey(vector3Int2))
					{
						list.Add(new KeyValuePair<Vector3Int, Node>(vector3Int2, new Node(keyValuePair.Value.g + num3, GetHeuristicValue(vector3Int2, end), keyValuePair.Key, Random.Range(0f, 1f))));
						hashSet.Add(vector3Int2);
					}
				}
				vector3Int2 = keyValuePair.Key + Vector3Int.right;
				yOffset = 0;
				if (CanEntityMoveSideWays(keyValuePair.Key, vector3Int2, ref yOffset, entityHeight))
				{
					int num4 = 0;
					vector3Int2.y += yOffset;
					num4 += 1 + yOffset;
					while (!IsEntityGrounded(vector3Int2))
					{
						vector3Int2.y--;
						num4++;
					}
					if (!hashSet.Contains(vector3Int2) && !dictionary.ContainsKey(vector3Int2))
					{
						list.Add(new KeyValuePair<Vector3Int, Node>(vector3Int2, new Node(keyValuePair.Value.g + num4, GetHeuristicValue(vector3Int2, end), keyValuePair.Key, Random.Range(0f, 1f))));
						hashSet.Add(vector3Int2);
					}
				}
				vector3Int2 = keyValuePair.Key - Vector3Int.forward;
				yOffset = 0;
				if (CanEntityMoveSideWays(keyValuePair.Key, vector3Int2, ref yOffset, entityHeight))
				{
					int num5 = 0;
					vector3Int2.y += yOffset;
					num5 += 1 + yOffset;
					while (!IsEntityGrounded(vector3Int2))
					{
						vector3Int2.y--;
						num5++;
					}
					if (!hashSet.Contains(vector3Int2) && !dictionary.ContainsKey(vector3Int2))
					{
						list.Add(new KeyValuePair<Vector3Int, Node>(vector3Int2, new Node(keyValuePair.Value.g + num5, GetHeuristicValue(vector3Int2, end), keyValuePair.Key, Random.Range(0f, 1f))));
						hashSet.Add(vector3Int2);
					}
				}
				vector3Int2 = keyValuePair.Key + Vector3Int.forward;
				yOffset = 0;
				if (CanEntityMoveSideWays(keyValuePair.Key, vector3Int2, ref yOffset, entityHeight))
				{
					int num6 = 0;
					vector3Int2.y += yOffset;
					num6 += 1 + yOffset;
					while (!IsEntityGrounded(vector3Int2))
					{
						vector3Int2.y--;
						num6++;
					}
					if (!hashSet.Contains(vector3Int2) && !dictionary.ContainsKey(vector3Int2))
					{
						list.Add(new KeyValuePair<Vector3Int, Node>(vector3Int2, new Node(keyValuePair.Value.g + num6, GetHeuristicValue(vector3Int2, end), keyValuePair.Key, Random.Range(0f, 1f))));
						hashSet.Add(vector3Int2);
					}
				}
			}
			dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			hashSet.Remove(keyValuePair.Key);
			list.RemoveAt(0);
		}
		int num7 = int.MaxValue;
		KeyValuePair<Vector3Int, Node> keyValuePair2 = default(KeyValuePair<Vector3Int, Node>);
		foreach (KeyValuePair<Vector3Int, Node> item in dictionary)
		{
			if (item.Value.f < num7)
			{
				keyValuePair2 = item;
				num7 = item.Value.f;
			}
		}
		string text = keyValuePair2.ToString();
		Vector3Int vector3Int3 = start;
		MonoBehaviour.print(text + " || " + vector3Int3.ToString());
		path.Add(keyValuePair2.Key);
		Vector3Int parent2 = keyValuePair2.Value.parent;
		Vector3Int key = keyValuePair2.Key;
		while (parent2 != start)
		{
			key = parent2;
			parent2 = dictionary[key].parent;
			path.Add(key);
		}
		return num;
	}

	public static int NodeComparer(KeyValuePair<Vector3Int, Node> a, KeyValuePair<Vector3Int, Node> b)
	{
		if (a.Value.f < b.Value.f)
		{
			return -1;
		}
		if (a.Value.f > b.Value.f)
		{
			return 1;
		}
		if (a.Value.h < b.Value.h)
		{
			return -1;
		}
		if (a.Value.h > b.Value.h)
		{
			return 1;
		}
		if (a.Value.rand > b.Value.rand)
		{
			return -1;
		}
		return 1;
	}

	public static int GetHeuristicValue(Vector3Int pos, Vector3Int end)
	{
		return GetManhattanDistance(pos, end) * 2;
	}

	public static int GetManhattanDistance(Vector3Int start, Vector3Int end)
	{
		return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y) + Mathf.Abs(end.z - start.z);
	}

	public static bool CanEntityFit(Vector3Int pos, int entityHeight)
	{
		for (int i = 0; i < entityHeight; i++)
		{
			byte voxelID = VoxelTools.GetVoxelID(pos + Vector3Int.up * i);
			if (voxelID >= 22 && voxelID <= 126)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsEntityGrounded(Vector3Int pos)
	{
		pos.y--;
		byte voxelID = VoxelTools.GetVoxelID(pos);
		if (voxelID >= 22)
		{
			return voxelID <= 126;
		}
		return false;
	}

	public static bool CanEntityMoveSideWays(Vector3Int currentPos, Vector3Int destinationPos, ref int yOffset, int entityHeight = 2)
	{
		if (CanEntityFit(destinationPos, entityHeight))
		{
			return true;
		}
		if (CanEntityFit(currentPos + Vector3Int.up * entityHeight, 1) && CanEntityFit(destinationPos + Vector3Int.up, entityHeight))
		{
			yOffset = 1;
			return true;
		}
		yOffset = 0;
		return false;
	}
}
