using UnityEngine;

public static class VoxelTools
{
	public static Vector3Int GetChunkPosition(Vector3Int worldPosition)
	{
		int num = worldPosition.x / Chunk.WIDTH;
		if (worldPosition.x < 0 && worldPosition.x % Chunk.WIDTH != 0)
		{
			num--;
		}
		int num2 = worldPosition.z / Chunk.WIDTH;
		if (worldPosition.z < 0 && worldPosition.z % Chunk.WIDTH != 0)
		{
			num2--;
		}
		return new Vector3Int(num * 16, 0, num2 * 16);
	}

	public static int GetVoxelArrayPositionInChunk(Vector3Int worldPosition)
	{
		int num = worldPosition.x % Chunk.WIDTH;
		if (num < 0)
		{
			num += 16;
		}
		int num2 = worldPosition.z % Chunk.WIDTH;
		if (num2 < 0)
		{
			num2 += 16;
		}
		return num * Chunk.HEIGHT * Chunk.WIDTH + worldPosition.y * Chunk.WIDTH + num2;
	}

	public static Vector3Int GetVoxelPositionInChunk(Vector3Int worldPosition)
	{
		int num = worldPosition.x % Chunk.WIDTH;
		if (num < 0)
		{
			num += 16;
		}
		int num2 = worldPosition.z % Chunk.WIDTH;
		if (num2 < 0)
		{
			num2 += 16;
		}
		int num3 = worldPosition.y;
		if (num3 >= Chunk.HEIGHT)
		{
			num3 = Chunk.HEIGHT - 1;
		}
		else if (num3 < 0)
		{
			num3 = 0;
		}
		return new Vector3Int(num, num3, num2);
	}

	public static byte GetVoxelID(Vector3Int worldPosition)
	{
		if (worldPosition.y >= Chunk.HEIGHT || worldPosition.y < 0)
		{
			return 0;
		}
		if (ChunkManager.instance.activeChunks.TryGetValue(GetChunkPosition(worldPosition) + WorldShifter.instance.offset, out var value))
		{
			return value.voxels[GetVoxelArrayPositionInChunk(worldPosition)];
		}
		return 0;
	}

	public static Vector3 GetVoxelPosition(Vector3 worldPosition)
	{
		return new Vector3(Mathf.Floor(worldPosition.x) + 0.5f, Mathf.Floor(worldPosition.y) + 0.5f, Mathf.Floor(worldPosition.z) + 0.5f);
	}

	public static Vector3 FloorPosition(Vector3 worldPosition)
	{
		return new Vector3(Mathf.Floor(worldPosition.x), Mathf.Floor(worldPosition.y), Mathf.Floor(worldPosition.z));
	}

	public static Vector3Int FloorPositionToInt(Vector3 worldPosition)
	{
		return new Vector3Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y), Mathf.FloorToInt(worldPosition.z));
	}

	public static void GetBlockinfo(Vector3Int worldPosition, ref Chunk chunk, ref int voxelIndex, ref Vector3Int voxelPosition)
	{
		chunk = ChunkManager.instance.activeChunks[GetChunkPosition(worldPosition)];
		int num = worldPosition.x % Chunk.WIDTH;
		if (num < 0)
		{
			num += 16;
		}
		int num2 = worldPosition.z % Chunk.WIDTH;
		if (num2 < 0)
		{
			num2 += 16;
		}
		voxelIndex = num * Chunk.HEIGHT * Chunk.WIDTH + worldPosition.y * Chunk.WIDTH + num2;
		voxelPosition = new Vector3Int(num, worldPosition.y, num2);
	}

	public static void GetBlockinfo(Vector3Int worldPosition, ref Chunk chunk, ref int voxelIndex, ref byte voxelID)
	{
		chunk = ChunkManager.instance.activeChunks[GetChunkPosition(worldPosition)];
		int num = worldPosition.x % Chunk.WIDTH;
		if (num < 0)
		{
			num += 16;
		}
		int num2 = worldPosition.z % Chunk.WIDTH;
		if (num2 < 0)
		{
			num2 += 16;
		}
		voxelIndex = num * Chunk.HEIGHT * Chunk.WIDTH + worldPosition.y * Chunk.WIDTH + num2;
		voxelID = chunk.voxels[voxelIndex];
	}

	public static void GetBlockinfo(Vector3Int worldPosition, ref Chunk chunk, ref int voxelIndex)
	{
		chunk = ChunkManager.instance.activeChunks[GetChunkPosition(worldPosition)];
		int num = worldPosition.x % Chunk.WIDTH;
		if (num < 0)
		{
			num += 16;
		}
		int num2 = worldPosition.z % Chunk.WIDTH;
		if (num2 < 0)
		{
			num2 += 16;
		}
		voxelIndex = num * Chunk.HEIGHT * Chunk.WIDTH + worldPosition.y * Chunk.WIDTH + num2;
	}

	public static byte GetLightValue(Vector3Int worldPosition)
	{
		Chunk chunk = null;
		int voxelIndex = 0;
		GetBlockinfo(worldPosition, ref chunk, ref voxelIndex);
		if ((bool)chunk)
		{
			return chunk.lightMap[voxelIndex];
		}
		return 0;
	}

	public static void GetBlockinfo(Vector3Int worldPosition, ref byte voxelID, ref byte orientation)
	{
		if (worldPosition.y < Chunk.HEIGHT && worldPosition.y >= 0 && ChunkManager.instance.activeChunks.ContainsKey(GetChunkPosition(worldPosition)))
		{
			Chunk chunk = ChunkManager.instance.activeChunks[GetChunkPosition(worldPosition)];
			int num = worldPosition.x % Chunk.WIDTH;
			if (num < 0)
			{
				num += 16;
			}
			int num2 = worldPosition.z % Chunk.WIDTH;
			if (num2 < 0)
			{
				num2 += 16;
			}
			int num3 = num * Chunk.HEIGHT * Chunk.WIDTH + worldPosition.y * Chunk.WIDTH + num2;
			voxelID = chunk.voxels[num3];
			if (chunk.blockOrientation.ContainsKey(num3))
			{
				orientation = chunk.blockOrientation[num3];
			}
			else
			{
				orientation = 0;
			}
		}
	}

	public static void GetBlockinfo(Vector3Int worldPosition, ref byte voxelID, ref byte orientation, ref Chunk chunk)
	{
		if (worldPosition.y < Chunk.HEIGHT && worldPosition.y >= 0 && ChunkManager.instance.activeChunks.ContainsKey(GetChunkPosition(worldPosition)))
		{
			chunk = ChunkManager.instance.activeChunks[GetChunkPosition(worldPosition)];
			int num = worldPosition.x % Chunk.WIDTH;
			if (num < 0)
			{
				num += 16;
			}
			int num2 = worldPosition.z % Chunk.WIDTH;
			if (num2 < 0)
			{
				num2 += 16;
			}
			int num3 = num * Chunk.HEIGHT * Chunk.WIDTH + worldPosition.y * Chunk.WIDTH + num2;
			voxelID = chunk.voxels[num3];
			if (chunk.blockOrientation.ContainsKey(num3))
			{
				orientation = chunk.blockOrientation[num3];
			}
			else
			{
				orientation = 0;
			}
		}
	}

	public static Chunk GetChunk(Vector3Int worldPosition)
	{
		return ChunkManager.instance.activeChunks[GetChunkPosition(worldPosition)];
	}

	public static int GetIndexX(int index)
	{
		index -= index % Chunk.WIDTH;
		index -= index % Chunk.WIDTHxHEIGHT;
		return index / Chunk.WIDTHxHEIGHT;
	}

	public static int GetIndexY(int index)
	{
		index %= Chunk.WIDTHxHEIGHT;
		index -= index % Chunk.WIDTH;
		return index / Chunk.WIDTH;
	}

	public static int GetIndexZ(int index)
	{
		return index % Chunk.WIDTH;
	}

	public static void DrawChunkBorders(Vector3 position)
	{
		DrawChunkBorders(new Chunk.Bounds(GetChunkPosition(FloorPositionToInt(position))), Color.red);
	}

	public static void DrawChunkBorders(Chunk.Bounds bounds, Color color)
	{
		Debug.DrawLine(bounds.LeftBotBack, bounds.RightBotBack, color);
		Debug.DrawLine(bounds.RightBotBack, bounds.RightBotFront, color);
		Debug.DrawLine(bounds.RightBotFront, bounds.LeftBotFront, color);
		Debug.DrawLine(bounds.LeftBotFront, bounds.LeftBotBack, color);
		Debug.DrawLine(bounds.LeftTopBack, bounds.RightTopBack, color);
		Debug.DrawLine(bounds.RightTopBack, bounds.RightTopFront, color);
		Debug.DrawLine(bounds.RightTopFront, bounds.LeftTopFront, color);
		Debug.DrawLine(bounds.LeftTopFront, bounds.LeftTopBack, color);
		Debug.DrawLine(bounds.LeftBotBack, bounds.LeftTopBack, color);
		Debug.DrawLine(bounds.RightBotBack, bounds.RightTopBack, color);
		Debug.DrawLine(bounds.LeftBotFront, bounds.LeftTopFront, color);
		Debug.DrawLine(bounds.RightBotFront, bounds.RightTopFront, color);
	}

	public static void DrawBounds(Bounds bounds, Vector3 position, Color color, float duration = 0f)
	{
		Vector3 vector = bounds.min + position;
		Vector3 vector2 = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z) + position;
		Vector3 vector3 = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z) + position;
		Vector3 vector4 = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z) + position;
		Vector3 vector5 = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z) + position;
		Vector3 vector6 = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z) + position;
		Vector3 vector7 = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z) + position;
		Vector3 vector8 = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z) + position;
		Debug.DrawLine(vector, vector5, color, duration);
		Debug.DrawLine(vector5, vector6, color, duration);
		Debug.DrawLine(vector6, vector2, color, duration);
		Debug.DrawLine(vector2, vector, color, duration);
		Debug.DrawLine(vector3, vector7, color, duration);
		Debug.DrawLine(vector7, vector8, color, duration);
		Debug.DrawLine(vector8, vector4, color, duration);
		Debug.DrawLine(vector4, vector3, color, duration);
		Debug.DrawLine(vector, vector3, color, duration);
		Debug.DrawLine(vector5, vector7, color, duration);
		Debug.DrawLine(vector2, vector4, color, duration);
		Debug.DrawLine(vector6, vector8, color, duration);
	}
}
