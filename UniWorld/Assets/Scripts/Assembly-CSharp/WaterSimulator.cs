using System.Collections.Generic;
using UnityEngine;

public static class WaterSimulator
{
	public static void ProcessWaterSimulation(Chunk chunk)
	{
		foreach (Vector3Int item in chunk.waterSimulationSet)
		{
			if (item.y <= 0)
			{
				continue;
			}
			byte b = chunk.voxels[item.x * Chunk.HEIGHT * Chunk.WIDTH + (item.y - 1) * Chunk.WIDTH + item.z];
			if (b <= 21 || (b >= 241 && b < 248))
			{
				byte value = 0;
				Vector3Int vector3Int = new Vector3Int(item.x, item.y - 1, item.z);
				if (chunk.waterUpdateModifications.TryGetValue(vector3Int, out value))
				{
					if (value < 248)
					{
						chunk.waterUpdateModifications[vector3Int] = 248;
					}
				}
				else
				{
					chunk.waterUpdateModifications.Add(vector3Int, 248);
				}
				chunk.newWaterSimulationSet.Add(vector3Int);
			}
			else if ((b >= 22 && b <= 100) || (b >= 101 && b <= 126))
			{
				byte b2 = chunk.voxels[item.x * Chunk.HEIGHT * Chunk.WIDTH + item.y * Chunk.WIDTH + item.z];
				if (b2 > 241)
				{
					NeighbourPropagation(chunk, new Vector3Int(item.x - 1, item.y, item.z), b2);
					NeighbourPropagation(chunk, new Vector3Int(item.x + 1, item.y, item.z), b2);
					NeighbourPropagation(chunk, new Vector3Int(item.x, item.y, item.z - 1), b2);
					NeighbourPropagation(chunk, new Vector3Int(item.x, item.y, item.z + 1), b2);
				}
			}
		}
		HashSet<Vector3Int> waterSimulationSet = chunk.waterSimulationSet;
		waterSimulationSet.Clear();
		chunk.waterSimulationSet = chunk.newWaterSimulationSet;
		chunk.newWaterSimulationSet = waterSimulationSet;
		ApplyWaterModifications(chunk);
	}

	public static void NeighbourPropagation(Chunk currentChunk, Vector3Int newPos, byte currentWaterValue)
	{
		Chunk chunk = currentChunk;
		if (newPos.x < 0)
		{
			newPos.x = 15;
			chunk = currentChunk.neighbours.left;
		}
		if (newPos.x > 15)
		{
			newPos.x = 0;
			chunk = currentChunk.neighbours.right;
		}
		if (newPos.z < 0)
		{
			newPos.z = 15;
			chunk = currentChunk.neighbours.back;
		}
		if (newPos.z > 15)
		{
			newPos.z = 0;
			chunk = currentChunk.neighbours.front;
		}
		byte b = chunk.voxels[newPos.x * Chunk.HEIGHT * Chunk.WIDTH + newPos.y * Chunk.WIDTH + newPos.z];
		byte b2 = (byte)((currentWaterValue == 249) ? 247 : ((byte)(currentWaterValue - 1)));
		if (b > 21 && (b < 241 || b >= b2))
		{
			return;
		}
		byte value = 0;
		if (chunk.waterUpdateModifications.TryGetValue(newPos, out value))
		{
			if (value < b2)
			{
				chunk.waterUpdateModifications[newPos] = b2;
			}
		}
		else
		{
			chunk.waterUpdateModifications.Add(newPos, b2);
		}
		chunk.newWaterSimulationSet.Add(newPos);
	}

	public static void ApplyWaterModifications(Chunk chunk)
	{
		if (chunk.waterUpdateModifications.Count <= 0)
		{
			return;
		}
		chunk.mustBeSaved = true;
		foreach (KeyValuePair<Vector3Int, byte> waterUpdateModification in chunk.waterUpdateModifications)
		{
			int num = waterUpdateModification.Key.x * Chunk.HEIGHT * Chunk.WIDTH + waterUpdateModification.Key.y * Chunk.WIDTH + waterUpdateModification.Key.z;
			byte b = chunk.voxels[num];
			chunk.voxels[num] = waterUpdateModification.Value;
			if ((b >= 21 && b <= 21) || (b >= 85 && b <= 85))
			{
				LightManager.RemoveBlockLight(chunk, waterUpdateModification.Key);
			}
			if (LightManager.GetSunLight(chunk.lightMap[num]) == 15 && waterUpdateModification.Value >= 241)
			{
				LightManager.RemoveSunLight(chunk, waterUpdateModification.Key);
			}
			if (waterUpdateModification.Value == 0)
			{
				LightManager.RemoveSunLight(chunk, waterUpdateModification.Key);
			}
			VoxelEditor.instance.BlockPlacementLogic(chunk, b, waterUpdateModification.Value, waterUpdateModification.Key, num, 0);
			VoxelEditor.instance.FlagMeshForUpdate(chunk, b, waterUpdateModification.Value, waterUpdateModification.Key);
		}
		chunk.waterUpdateModifications.Clear();
	}

	public static void ProcessUpdateSet(Chunk chunk)
	{
		byte b = 0;
		foreach (Vector3Int item in chunk.updateSet)
		{
			byte b2 = chunk.voxels[item.x * Chunk.HEIGHT * Chunk.WIDTH + item.y * Chunk.WIDTH + item.z];
			if (chunk.waterSimulationSet.Contains(item) && ((b2 >= 1 && b2 <= 100) || (b2 >= 101 && b2 <= 126)))
			{
				chunk.waterSimulationSet.Remove(item);
			}
			byte b3 = 0;
			if ((b2 >= 1 && b2 <= 100) || (b2 >= 101 && b2 <= 126) || b2 == 0)
			{
				if (item.y < Chunk.HEIGHT - 1)
				{
					b = chunk.voxels[item.x * Chunk.HEIGHT * Chunk.WIDTH + (item.y + 1) * Chunk.WIDTH + item.z];
					if (b >= 241 && b <= 249)
					{
						chunk.waterSimulationSet.Add(new Vector3Int(item.x, item.y + 1, item.z));
					}
				}
				Vector3Int vector3Int;
				if (item.y > 0)
				{
					vector3Int = new Vector3Int(item.x, item.y - 1, item.z);
					b = chunk.voxels[vector3Int.x * Chunk.HEIGHT * Chunk.WIDTH + vector3Int.y * Chunk.WIDTH + vector3Int.z];
					if (b >= 241 && b <= 249 && !WaterBlockHasSource(chunk, vector3Int, b))
					{
						chunk.newUpdateSet.Add(vector3Int);
					}
				}
				Chunk chunk2;
				if (item.x == 0)
				{
					chunk2 = chunk.neighbours.left;
					vector3Int = new Vector3Int(15, item.y, item.z);
				}
				else
				{
					chunk2 = chunk;
					vector3Int = new Vector3Int(item.x - 1, item.y, item.z);
				}
				b = chunk2.voxels[vector3Int.x * Chunk.HEIGHT * Chunk.WIDTH + vector3Int.y * Chunk.WIDTH + vector3Int.z];
				if (b >= 241 && b <= 249)
				{
					if (!WaterBlockHasSource(chunk2, vector3Int, b))
					{
						chunk2.newUpdateSet.Add(vector3Int);
					}
					else if (b2 <= 21)
					{
						chunk2.waterSimulationSet.Add(vector3Int);
					}
				}
				if (item.x == 15)
				{
					chunk2 = chunk.neighbours.right;
					vector3Int = new Vector3Int(0, item.y, item.z);
				}
				else
				{
					chunk2 = chunk;
					vector3Int = new Vector3Int(item.x + 1, item.y, item.z);
				}
				b = chunk2.voxels[vector3Int.x * Chunk.HEIGHT * Chunk.WIDTH + vector3Int.y * Chunk.WIDTH + vector3Int.z];
				if (b >= 241 && b <= 249)
				{
					if (!WaterBlockHasSource(chunk2, vector3Int, b))
					{
						chunk2.newUpdateSet.Add(vector3Int);
					}
					else if (b2 <= 21)
					{
						chunk2.waterSimulationSet.Add(vector3Int);
					}
				}
				if (item.z == 0)
				{
					chunk2 = chunk.neighbours.back;
					vector3Int = new Vector3Int(item.x, item.y, 15);
				}
				else
				{
					chunk2 = chunk;
					vector3Int = new Vector3Int(item.x, item.y, item.z - 1);
				}
				b = chunk2.voxels[vector3Int.x * Chunk.HEIGHT * Chunk.WIDTH + vector3Int.y * Chunk.WIDTH + vector3Int.z];
				if (b >= 241 && b <= 249)
				{
					if (!WaterBlockHasSource(chunk2, vector3Int, b))
					{
						chunk2.newUpdateSet.Add(vector3Int);
					}
					else if (b2 <= 21)
					{
						chunk2.waterSimulationSet.Add(vector3Int);
					}
				}
				if (item.z == 15)
				{
					chunk2 = chunk.neighbours.front;
					vector3Int = new Vector3Int(item.x, item.y, 0);
				}
				else
				{
					chunk2 = chunk;
					vector3Int = new Vector3Int(item.x, item.y, item.z + 1);
				}
				b = chunk2.voxels[vector3Int.x * Chunk.HEIGHT * Chunk.WIDTH + vector3Int.y * Chunk.WIDTH + vector3Int.z];
				if (b >= 241 && b <= 249)
				{
					if (!WaterBlockHasSource(chunk2, vector3Int, b))
					{
						chunk2.newUpdateSet.Add(vector3Int);
					}
					else if (b2 <= 21)
					{
						chunk2.waterSimulationSet.Add(vector3Int);
					}
				}
			}
			if (b2 < 241 || b2 > 249 || WaterBlockHasSource(chunk, item, b2))
			{
				continue;
			}
			b3 = (byte)((b2 > 241) ? ((byte)(b2 - 1)) : 0);
			if (chunk.waterUpdateModifications.TryGetValue(item, out var value))
			{
				if (value < b3)
				{
					chunk.waterUpdateModifications[item] = b3;
				}
			}
			else
			{
				chunk.waterUpdateModifications.Add(item, b3);
			}
			AddNeighboursBlockToNewUpdateSet(chunk, item, b3);
		}
		HashSet<Vector3Int> updateSet = chunk.updateSet;
		updateSet.Clear();
		chunk.updateSet = chunk.newUpdateSet;
		chunk.newUpdateSet = updateSet;
		ApplyWaterModifications(chunk);
	}

	public static void AddNeighboursBlockToNewUpdateSet(Chunk chunk, Vector3Int pos, byte newWaterValue)
	{
		if (newWaterValue > 0 && !chunk.waterSimulationSet.Contains(pos))
		{
			chunk.newUpdateSet.Add(pos);
		}
		else if (pos.y > 0 && !chunk.waterSimulationSet.Contains(pos))
		{
			chunk.newUpdateSet.Add(new Vector3Int(pos.x, pos.y - 1, pos.z));
		}
		Vector3Int vector3Int = new Vector3Int(pos.x - 1, pos.y, pos.z);
		Chunk chunk2;
		if (vector3Int.x < 0)
		{
			vector3Int.x = 15;
			chunk2 = chunk.neighbours.left;
		}
		else
		{
			chunk2 = chunk;
		}
		if (!chunk2.waterSimulationSet.Contains(vector3Int))
		{
			chunk2.newUpdateSet.Add(vector3Int);
		}
		else if (WaterBlockHasSource(chunk2, vector3Int, newWaterValue))
		{
			chunk2.waterSimulationSet.Remove(vector3Int);
			chunk2.newUpdateSet.Add(vector3Int);
		}
		vector3Int = new Vector3Int(pos.x + 1, pos.y, pos.z);
		if (vector3Int.x > 15)
		{
			vector3Int.x = 0;
			chunk2 = chunk.neighbours.right;
		}
		else
		{
			chunk2 = chunk;
		}
		if (!chunk2.waterSimulationSet.Contains(vector3Int))
		{
			chunk2.newUpdateSet.Add(vector3Int);
		}
		else if (WaterBlockHasSource(chunk2, vector3Int, newWaterValue))
		{
			chunk2.waterSimulationSet.Remove(vector3Int);
			chunk2.newUpdateSet.Add(vector3Int);
		}
		vector3Int = new Vector3Int(pos.x, pos.y, pos.z - 1);
		if (vector3Int.z < 0)
		{
			vector3Int.z = 15;
			chunk2 = chunk.neighbours.back;
		}
		else
		{
			chunk2 = chunk;
		}
		if (!chunk2.waterSimulationSet.Contains(vector3Int))
		{
			chunk2.newUpdateSet.Add(vector3Int);
		}
		else if (WaterBlockHasSource(chunk2, vector3Int, newWaterValue))
		{
			chunk2.waterSimulationSet.Remove(vector3Int);
			chunk2.newUpdateSet.Add(vector3Int);
		}
		vector3Int = new Vector3Int(pos.x, pos.y, pos.z + 1);
		if (vector3Int.z > 15)
		{
			vector3Int.z = 0;
			chunk2 = chunk.neighbours.front;
		}
		else
		{
			chunk2 = chunk;
		}
		if (!chunk2.waterSimulationSet.Contains(vector3Int))
		{
			chunk2.newUpdateSet.Add(vector3Int);
		}
		else if (WaterBlockHasSource(chunk2, vector3Int, newWaterValue))
		{
			chunk2.waterSimulationSet.Remove(vector3Int);
			chunk2.newUpdateSet.Add(vector3Int);
		}
	}

	public static bool WaterBlockHasSource(Chunk chunk, Vector3Int pos, byte waterValue)
	{
		if (waterValue == 249)
		{
			return true;
		}
		byte b = 0;
		if (pos.y < Chunk.HEIGHT - 1)
		{
			b = chunk.voxels[pos.x * Chunk.HEIGHT * Chunk.WIDTH + (pos.y + 1) * Chunk.WIDTH + pos.z];
			if (b >= 241 && b <= 249)
			{
				return true;
			}
		}
		if (pos.x == 0)
		{
			b = chunk.neighbours.left.voxels[15 * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH + pos.z];
			if (b > waterValue)
			{
				return true;
			}
		}
		else
		{
			b = chunk.voxels[(pos.x - 1) * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH + pos.z];
			if (b > waterValue)
			{
				return true;
			}
		}
		if (pos.x == 15)
		{
			b = chunk.neighbours.right.voxels[pos.y * Chunk.WIDTH + pos.z];
			if (b > waterValue)
			{
				return true;
			}
		}
		else
		{
			b = chunk.voxels[(pos.x + 1) * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH + pos.z];
			if (b > waterValue)
			{
				return true;
			}
		}
		if (pos.z == 0)
		{
			b = chunk.neighbours.back.voxels[pos.x * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH + 15];
			if (b > waterValue)
			{
				return true;
			}
		}
		else
		{
			b = chunk.voxels[pos.x * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH + pos.z - 1];
			if (b > waterValue)
			{
				return true;
			}
		}
		if (pos.z == 15)
		{
			b = chunk.neighbours.front.voxels[pos.x * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH];
			if (b > waterValue)
			{
				return true;
			}
		}
		else
		{
			b = chunk.voxels[pos.x * Chunk.HEIGHT * Chunk.WIDTH + pos.y * Chunk.WIDTH + pos.z + 1];
			if (b > waterValue)
			{
				return true;
			}
		}
		return false;
	}

	public static void PrepareWaterSimulationSetOnChunkLoad(Chunk chunk)
	{
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.HEIGHT; j++)
			{
				for (int k = 0; k < Chunk.WIDTH; k++)
				{
					byte b = chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + k];
					if (b >= 241 && b <= 249)
					{
						chunk.waterSimulationSet.Add(new Vector3Int(i, j, k));
					}
				}
			}
		}
	}
}
