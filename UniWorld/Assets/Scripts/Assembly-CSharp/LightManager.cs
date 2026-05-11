using System;
using System.Collections.Generic;
using UnityEngine;

public static class LightManager
{
	public struct LightNode
	{
		public Vector3Int position;

		public Chunk chunk;

		public byte voxelId;

		public byte voxelOrientation;

		public LightNode(Vector3Int position, Chunk chunk, byte voxelId = 0, byte voxelOrientation = 0)
		{
			this.position = position;
			this.chunk = chunk;
			this.voxelId = voxelId;
			this.voxelOrientation = voxelOrientation;
		}
	}

	public struct LightRemovalNode
	{
		public Vector3Int position;

		public Chunk chunk;

		public byte voxelID;

		public byte voxelOrientation;

		public byte lightLevel;

		public LightRemovalNode(Vector3Int position, Chunk chunk, byte lightLevel, byte voxelID = 0, byte voxelOrientation = 0)
		{
			this.position = position;
			this.chunk = chunk;
			this.voxelID = voxelID;
			this.voxelOrientation = voxelOrientation;
			this.lightLevel = lightLevel;
		}
	}

	public static uint[] stairsLightExitBitmask = new uint[8]
	{
		(uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 18.0),
		(uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0),
		(uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 19.0),
		(uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 15.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 22.0),
		(uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 23.0)
	};

	public static uint[] stairsLightEntranceBitmask = new uint[8]
	{
		(uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0),
		(uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 22.0),
		(uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 16.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 12.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 11.0),
		(uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0)
	};

	public static uint[] slabLightExitBitmask = new uint[6]
	{
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 22.0),
		(uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0)
	};

	public static uint[] slabLightEntranceBitmask = new uint[6]
	{
		(uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 22.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0) + (uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0),
		(uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0) + (uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0) + (uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0)
	};

	public static uint[] dirMaskSubstractor = new uint[6]
	{
		(uint)Math.Pow(2.0, 16.0) + (uint)Math.Pow(2.0, 17.0) + (uint)Math.Pow(2.0, 18.0) + (uint)Math.Pow(2.0, 19.0),
		(uint)Math.Pow(2.0, 8.0) + (uint)Math.Pow(2.0, 9.0) + (uint)Math.Pow(2.0, 10.0) + (uint)Math.Pow(2.0, 11.0),
		(uint)Math.Pow(2.0, 20.0) + (uint)Math.Pow(2.0, 21.0) + (uint)Math.Pow(2.0, 22.0) + (uint)Math.Pow(2.0, 23.0),
		(uint)Math.Pow(2.0, 4.0) + (uint)Math.Pow(2.0, 5.0) + (uint)Math.Pow(2.0, 6.0) + (uint)Math.Pow(2.0, 7.0),
		(uint)Math.Pow(2.0, 0.0) + (uint)Math.Pow(2.0, 1.0) + (uint)Math.Pow(2.0, 2.0) + (uint)Math.Pow(2.0, 3.0),
		(uint)Math.Pow(2.0, 12.0) + (uint)Math.Pow(2.0, 13.0) + (uint)Math.Pow(2.0, 14.0) + (uint)Math.Pow(2.0, 15.0)
	};

	public static byte SetLights(byte sunLight, byte blockLight)
	{
		return (byte)((sunLight << 4) | blockLight);
	}

	public static void GetLights(byte combinedValue, out byte sunLight, out byte blockLight)
	{
		sunLight = (byte)(combinedValue >> 4);
		blockLight = (byte)(combinedValue & 0xF);
	}

	public static void SetSunLight(ref byte combinedValue, byte sunLight)
	{
		combinedValue = (byte)((sunLight << 4) | (combinedValue & 0xF));
	}

	public static void SetBlockLight(ref byte combinedValue, byte blockLight)
	{
		combinedValue = (byte)((combinedValue >> 4 << 4) | blockLight);
	}

	public static byte GetSunLight(byte combinedValue)
	{
		return (byte)(combinedValue >> 4);
	}

	public static byte GetBlockLight(byte combinedValue)
	{
		return (byte)(combinedValue & 0xF);
	}

	public static void PlaceBlockLight(Chunk chunk, Vector3Int position, byte newLightLevel)
	{
		int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
		byte combinedValue = chunk.lightMap[num];
		SetBlockLight(ref combinedValue, newLightLevel);
		chunk.lightMap[num] = combinedValue;
		byte b = chunk.voxels[num];
		byte value;
		if (b >= 101 && b >= 126)
		{
			chunk.blockOrientation.TryGetValue(num, out value);
		}
		else
		{
			value = 0;
		}
		chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk, b, value));
		chunk.pendingLightModifications = true;
		chunk.pendingLightPropagation = true;
	}

	public static void ProcessBlockLightPropagation(Chunk chunk)
	{
		HashSet<Chunk> hashSet = new HashSet<Chunk>();
		while (chunk.blockLightBfsQueue.Count > 0)
		{
			LightNode lightNode = chunk.blockLightBfsQueue.Dequeue();
			hashSet.Add(lightNode.chunk);
			if (lightNode.position.x == 0)
			{
				hashSet.Add(chunk.neighbours.left);
				if (lightNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.leftBack);
				}
				else if (lightNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.leftFront);
				}
			}
			else if (lightNode.position.x == 15)
			{
				hashSet.Add(chunk.neighbours.right);
				if (lightNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.rightBack);
				}
				else if (lightNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.rightFront);
				}
			}
			if (lightNode.position.z == 0)
			{
				hashSet.Add(chunk.neighbours.back);
			}
			else if (lightNode.position.z == 15)
			{
				hashSet.Add(chunk.neighbours.front);
			}
			Vector3Int position = lightNode.position;
			if (lightNode.chunk == null)
			{
				Vector3Int position2 = lightNode.position;
				Debug.Log("WOW CHUNK PROBLEM || " + position2.ToString());
				continue;
			}
			byte blockLight = GetBlockLight(lightNode.chunk.lightMap[position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z]);
			position.x--;
			Chunk chunk2;
			if (position.x < 0)
			{
				position.x = 15;
				chunk2 = lightNode.chunk.neighbours.left;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			byte b = chunk2.voxels[num];
			byte value;
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 0) && GetBlockLight(chunk2.lightMap[num]) + 2 <= blockLight)
			{
				SetBlockLight(ref chunk2.lightMap[num], (byte)(blockLight - 1));
				if (position.x == 0)
				{
					hashSet.Add(lightNode.chunk.neighbours.left);
					if (position.z == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftFront);
					}
					else if (position.z == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftBack);
					}
				}
				if (blockLight > 2)
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.x = lightNode.position.x + 1;
			if (position.x > 15)
			{
				position.x = 0;
				chunk2 = lightNode.chunk.neighbours.right;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 1) && GetBlockLight(chunk2.lightMap[num]) + 2 <= blockLight)
			{
				SetBlockLight(ref chunk2.lightMap[num], (byte)(blockLight - 1));
				if (position.x == 15)
				{
					hashSet.Add(lightNode.chunk.neighbours.right);
					if (position.z == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightFront);
					}
					else if (position.z == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightBack);
					}
				}
				if (blockLight > 2)
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.x = lightNode.position.x;
			position.z--;
			if (position.z < 0)
			{
				position.z = 15;
				chunk2 = lightNode.chunk.neighbours.back;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 4) && GetBlockLight(chunk2.lightMap[num]) + 2 <= blockLight)
			{
				SetBlockLight(ref chunk2.lightMap[num], (byte)(blockLight - 1));
				if (position.z == 0)
				{
					hashSet.Add(lightNode.chunk.neighbours.back);
					if (position.x == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightBack);
					}
					else if (position.x == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftBack);
					}
				}
				if (blockLight > 2)
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.z = lightNode.position.z + 1;
			if (position.z > 15)
			{
				position.z = 0;
				chunk2 = lightNode.chunk.neighbours.front;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 5) && GetBlockLight(chunk2.lightMap[num]) + 2 <= blockLight)
			{
				SetBlockLight(ref chunk2.lightMap[num], (byte)(blockLight - 1));
				if (position.z == 15)
				{
					hashSet.Add(lightNode.chunk.neighbours.front);
					if (position.x == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightFront);
					}
					else if (position.x == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftFront);
					}
				}
				if (blockLight > 2)
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.z = lightNode.position.z;
			chunk2 = lightNode.chunk;
			if (position.y > 0)
			{
				position.y--;
				num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
				b = chunk2.voxels[num];
				if (b >= 101 && b <= 126)
				{
					chunk2.blockOrientation.TryGetValue(num, out value);
				}
				else
				{
					value = 0;
				}
				if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 2) && GetBlockLight(chunk2.lightMap[num]) + 2 <= blockLight)
				{
					SetBlockLight(ref chunk2.lightMap[num], (byte)(blockLight - 1));
					if (blockLight > 2)
					{
						chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					}
				}
			}
			if (position.y >= Chunk.HEIGHT - 2)
			{
				continue;
			}
			position.y = lightNode.position.y + 1;
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 3) && GetBlockLight(chunk2.lightMap[num]) + 2 <= blockLight)
			{
				SetBlockLight(ref chunk2.lightMap[num], (byte)(blockLight - 1));
				if (blockLight > 2)
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
		}
		foreach (Chunk item in hashSet)
		{
			item.pendingLightModifications = true;
		}
		chunk.pendingLightPropagation = false;
	}

	public static void RemoveBlockLight(Chunk chunk, Vector3Int position)
	{
		int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
		byte combinedValue = chunk.lightMap[num];
		byte blockLight = GetBlockLight(combinedValue);
		byte b = chunk.voxels[num];
		byte value;
		if (b >= 101 && b <= 126)
		{
			chunk.blockOrientation.TryGetValue(num, out value);
		}
		else
		{
			value = 0;
		}
		chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk, blockLight, 0, value));
		SetBlockLight(ref combinedValue, 0);
		chunk.lightMap[num] = combinedValue;
		chunk.pendingLightRemoval = true;
		chunk.pendingLightModifications = true;
		if (chunk.blockLightRemovalBfsQueue.Peek().chunk == null)
		{
			Debug.Log("empty peek");
		}
	}

	public static void ProcessBlockLightRemoval(Chunk chunk)
	{
		HashSet<Chunk> hashSet = new HashSet<Chunk>();
		HashSet<Chunk> hashSet2 = new HashSet<Chunk>();
		while (chunk.blockLightRemovalBfsQueue.Count > 0)
		{
			LightRemovalNode lightRemovalNode = chunk.blockLightRemovalBfsQueue.Dequeue();
			if (lightRemovalNode.chunk == null)
			{
				Debug.Log("Skiping scuffed node");
				continue;
			}
			hashSet.Add(lightRemovalNode.chunk);
			if (lightRemovalNode.position.x == 0)
			{
				hashSet.Add(chunk.neighbours.left);
				if (lightRemovalNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.leftBack);
				}
				else if (lightRemovalNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.leftFront);
				}
			}
			else if (lightRemovalNode.position.x == 15)
			{
				hashSet.Add(chunk.neighbours.right);
				if (lightRemovalNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.rightBack);
				}
				else if (lightRemovalNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.rightFront);
				}
			}
			if (lightRemovalNode.position.z == 0)
			{
				hashSet.Add(chunk.neighbours.back);
			}
			else if (lightRemovalNode.position.z == 15)
			{
				hashSet.Add(chunk.neighbours.front);
			}
			Vector3Int position = lightRemovalNode.position;
			byte lightLevel = lightRemovalNode.lightLevel;
			position.x--;
			Chunk chunk2;
			if (position.x < 0)
			{
				position.x = 15;
				chunk2 = lightRemovalNode.chunk.neighbours.left;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			byte b = chunk2.voxels[num];
			byte value;
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			byte blockLight;
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 0))
			{
				blockLight = GetBlockLight(chunk2.lightMap[num]);
				if (blockLight != 0 && blockLight < lightLevel)
				{
					byte b2 = blockLight;
					SetBlockLight(ref chunk2.lightMap[num], 0);
					if (position.x == 0)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.left);
						if (position.z == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftFront);
						}
						else if (position.z == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftBack);
						}
					}
					if (b2 > 0)
					{
						chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b2, b, value));
					}
				}
				else
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position.x = lightRemovalNode.position.x + 1;
			if (position.x > 15)
			{
				position.x = 0;
				chunk2 = lightRemovalNode.chunk.neighbours.right;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 1))
			{
				blockLight = GetBlockLight(chunk2.lightMap[num]);
				if (blockLight != 0 && blockLight < lightLevel)
				{
					byte b3 = blockLight;
					SetBlockLight(ref chunk2.lightMap[num], 0);
					if (position.x == 15)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.right);
						if (position.z == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightFront);
						}
						else if (position.z == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightBack);
						}
					}
					if (b3 > 0)
					{
						chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b3, b, value));
					}
				}
				else
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position = lightRemovalNode.position;
			position.z--;
			if (position.z < 0)
			{
				position.z = 15;
				chunk2 = lightRemovalNode.chunk.neighbours.back;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 4))
			{
				blockLight = GetBlockLight(chunk2.lightMap[num]);
				if (blockLight != 0 && blockLight < lightLevel)
				{
					byte b4 = blockLight;
					SetBlockLight(ref chunk2.lightMap[num], 0);
					if (position.z == 0)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.back);
						if (position.x == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightBack);
						}
						else if (position.x == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftBack);
						}
					}
					if (b4 > 0)
					{
						chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b4, b, value));
					}
				}
				else
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position = lightRemovalNode.position;
			position.z = lightRemovalNode.position.z + 1;
			if (position.z > 15)
			{
				position.z = 0;
				chunk2 = lightRemovalNode.chunk.neighbours.front;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 5))
			{
				blockLight = GetBlockLight(chunk2.lightMap[num]);
				if (blockLight != 0 && blockLight < lightLevel)
				{
					byte b5 = blockLight;
					SetBlockLight(ref chunk2.lightMap[num], 0);
					if (position.z == 15)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.front);
						if (position.x == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightFront);
						}
						else if (position.x == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftFront);
						}
					}
					if (b5 > 0)
					{
						chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b5, b, value));
					}
				}
				else
				{
					chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position = lightRemovalNode.position;
			chunk2 = lightRemovalNode.chunk;
			if (position.y > 0)
			{
				position.y--;
				num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
				b = chunk2.voxels[num];
				if (b >= 101 && b <= 126)
				{
					chunk2.blockOrientation.TryGetValue(num, out value);
				}
				else
				{
					value = 0;
				}
				if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 2))
				{
					blockLight = GetBlockLight(chunk2.lightMap[num]);
					if (blockLight != 0 && blockLight < lightLevel)
					{
						byte b6 = blockLight;
						SetBlockLight(ref chunk2.lightMap[num], 0);
						if (b6 > 0)
						{
							chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b6, b, value));
						}
					}
					else
					{
						chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
						hashSet2.Add(chunk2);
					}
				}
			}
			position.y = lightRemovalNode.position.y;
			if (position.y >= Chunk.HEIGHT - 1)
			{
				continue;
			}
			position.y++;
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (!LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 3))
			{
				continue;
			}
			blockLight = GetBlockLight(chunk2.lightMap[num]);
			if (blockLight != 0 && blockLight < lightLevel)
			{
				byte b7 = blockLight;
				SetBlockLight(ref chunk2.lightMap[num], 0);
				if (b7 > 0)
				{
					chunk.blockLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b7, b, value));
				}
			}
			else
			{
				chunk.blockLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				hashSet2.Add(chunk2);
			}
		}
		chunk.pendingLightRemoval = false;
		foreach (Chunk item in hashSet)
		{
			item.pendingLightModifications = true;
		}
		foreach (Chunk item2 in hashSet2)
		{
			item2.pendingLightPropagation = true;
		}
	}

	public static void InitializeSunLight(Chunk chunk)
	{
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.WIDTH; j++)
			{
				int num = (int)chunk.heightMap[i * Chunk.WIDTH + j] + 1;
				int num2 = ((i != 0) ? ((int)chunk.heightMap[(i - 1) * Chunk.WIDTH + j]) : ((int)chunk.neighbours.left.heightMap[15 * Chunk.WIDTH + j]));
				int num3 = ((i != 15) ? ((int)chunk.heightMap[(i + 1) * Chunk.WIDTH + j]) : ((int)chunk.neighbours.right.heightMap[j]));
				int num4 = ((j != 0) ? ((int)chunk.heightMap[i * Chunk.WIDTH + j - 1]) : ((int)chunk.neighbours.back.heightMap[i * Chunk.WIDTH + 15]));
				int num5 = ((j != 15) ? ((int)chunk.heightMap[i * Chunk.WIDTH + j + 1]) : ((int)chunk.neighbours.front.heightMap[i * Chunk.WIDTH]));
				num = Mathf.Max(num, num2, num3, num4, num5);
				for (int num6 = Chunk.HEIGHT - 1; num6 > num; num6--)
				{
					SetSunLight(ref chunk.lightMap[i * Chunk.HEIGHT * Chunk.WIDTH + num6 * Chunk.WIDTH + j], 15);
				}
				PlaceSunLight(chunk, new Vector3Int(i, num, j), 15);
			}
		}
		chunk.sunLightHasBeenInitialized = true;
		chunk.initialLightBakingRequired = false;
	}

	public static void PlaceSunLight(Chunk chunk, Vector3Int position, byte newLightLevel)
	{
		int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
		byte combinedValue = chunk.lightMap[num];
		SetSunLight(ref combinedValue, newLightLevel);
		chunk.lightMap[num] = combinedValue;
		byte b = chunk.voxels[num];
		byte value;
		if (b >= 101 && b >= 126)
		{
			chunk.blockOrientation.TryGetValue(num, out value);
		}
		else
		{
			value = 0;
		}
		chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk, b, value));
		chunk.pendingLightModifications = true;
		chunk.pendingLightPropagation = true;
	}

	public static void ProcessSunLightPropagation(Chunk chunk)
	{
		HashSet<Chunk> hashSet = new HashSet<Chunk>();
		while (chunk.sunLightBfsQueue.Count > 0)
		{
			LightNode lightNode = chunk.sunLightBfsQueue.Dequeue();
			hashSet.Add(lightNode.chunk);
			if (lightNode.position.x == 0)
			{
				hashSet.Add(chunk.neighbours.left);
				if (lightNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.leftBack);
				}
				else if (lightNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.leftFront);
				}
			}
			else if (lightNode.position.x == 15)
			{
				hashSet.Add(chunk.neighbours.right);
				if (lightNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.rightBack);
				}
				else if (lightNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.rightFront);
				}
			}
			if (lightNode.position.z == 0)
			{
				hashSet.Add(chunk.neighbours.back);
			}
			else if (lightNode.position.z == 15)
			{
				hashSet.Add(chunk.neighbours.front);
			}
			Vector3Int position = lightNode.position;
			if (lightNode.chunk == null)
			{
				Vector3Int position2 = lightNode.position;
				Debug.Log("WOW CHUNK PROBLEM || " + position2.ToString());
				continue;
			}
			byte sunLight = GetSunLight(lightNode.chunk.lightMap[position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z]);
			position.x--;
			Chunk chunk2;
			if (position.x < 0)
			{
				position.x = 15;
				chunk2 = lightNode.chunk.neighbours.left;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			byte b = chunk2.voxels[num];
			byte value;
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 0) && GetSunLight(chunk2.lightMap[num]) + 2 <= sunLight)
			{
				SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
				if (position.x == 0)
				{
					hashSet.Add(lightNode.chunk.neighbours.left);
					if (position.z == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftFront);
					}
					else if (position.z == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftBack);
					}
				}
				if (sunLight > 2)
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.x = lightNode.position.x + 1;
			if (position.x > 15)
			{
				position.x = 0;
				chunk2 = lightNode.chunk.neighbours.right;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 1) && GetSunLight(chunk2.lightMap[num]) + 2 <= sunLight)
			{
				SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
				if (position.x == 15)
				{
					hashSet.Add(lightNode.chunk.neighbours.right);
					if (position.z == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightFront);
					}
					else if (position.z == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightBack);
					}
				}
				if (sunLight > 2)
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.x = lightNode.position.x;
			position.z--;
			if (position.z < 0)
			{
				position.z = 15;
				chunk2 = lightNode.chunk.neighbours.back;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 4) && GetSunLight(chunk2.lightMap[num]) + 2 <= sunLight)
			{
				SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
				if (position.z == 0)
				{
					hashSet.Add(lightNode.chunk.neighbours.back);
					if (position.x == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightBack);
					}
					else if (position.x == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftBack);
					}
				}
				if (sunLight > 2)
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.z = lightNode.position.z + 1;
			if (position.z > 15)
			{
				position.z = 0;
				chunk2 = lightNode.chunk.neighbours.front;
			}
			else
			{
				chunk2 = lightNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 5) && GetSunLight(chunk2.lightMap[num]) + 2 <= sunLight)
			{
				SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
				if (position.z == 15)
				{
					hashSet.Add(lightNode.chunk.neighbours.front);
					if (position.x == 15)
					{
						hashSet.Add(lightNode.chunk.neighbours.rightFront);
					}
					else if (position.x == 0)
					{
						hashSet.Add(lightNode.chunk.neighbours.leftFront);
					}
				}
				if (sunLight > 2)
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			position.z = lightNode.position.z;
			chunk2 = lightNode.chunk;
			if (position.y > 0)
			{
				position.y--;
				num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
				b = chunk2.voxels[num];
				if (b >= 101 && b <= 126)
				{
					chunk2.blockOrientation.TryGetValue(num, out value);
				}
				else
				{
					value = 0;
				}
				if (sunLight < 15)
				{
					if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 2) && GetSunLight(chunk2.lightMap[num]) + 2 <= sunLight)
					{
						SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
						if (sunLight > 2)
						{
							chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
						}
					}
				}
				else if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 2) && GetSunLight(chunk2.lightMap[num]) < sunLight)
				{
					if ((b >= 94 && b <= 100) || b >= 241)
					{
						SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
					}
					else
					{
						SetSunLight(ref chunk2.lightMap[num], sunLight);
					}
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
			if (sunLight >= 15 || position.y >= Chunk.HEIGHT - 1)
			{
				continue;
			}
			position.y = lightNode.position.y + 1;
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightNode.voxelId, lightNode.voxelOrientation, b, value, 3) && GetSunLight(chunk2.lightMap[num]) + 2 <= sunLight)
			{
				SetSunLight(ref chunk2.lightMap[num], (byte)(sunLight - 1));
				if (sunLight > 2)
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				}
			}
		}
		foreach (Chunk item in hashSet)
		{
			item.pendingLightModifications = true;
		}
		chunk.pendingLightPropagation = false;
	}

	public static void RemoveSunLight(Chunk chunk, Vector3Int position)
	{
		int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
		byte combinedValue = chunk.lightMap[num];
		byte sunLight = GetSunLight(combinedValue);
		byte b = chunk.voxels[num];
		byte value;
		if (b >= 101 && b >= 126)
		{
			chunk.blockOrientation.TryGetValue(num, out value);
		}
		else
		{
			value = 0;
		}
		chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk, sunLight, 0, value));
		SetSunLight(ref combinedValue, 0);
		chunk.lightMap[num] = combinedValue;
		chunk.pendingLightRemoval = true;
		chunk.pendingLightModifications = true;
		if (chunk.sunLightRemovalBfsQueue.Peek().chunk == null)
		{
			Debug.Log("empty peek");
		}
	}

	public static void ProcessSunLightRemoval(Chunk chunk)
	{
		HashSet<Chunk> hashSet = new HashSet<Chunk>();
		HashSet<Chunk> hashSet2 = new HashSet<Chunk>();
		while (chunk.sunLightRemovalBfsQueue.Count > 0)
		{
			LightRemovalNode lightRemovalNode = chunk.sunLightRemovalBfsQueue.Dequeue();
			if (lightRemovalNode.chunk == null)
			{
				Debug.Log("Skiping scuffed node");
				continue;
			}
			hashSet.Add(lightRemovalNode.chunk);
			if (lightRemovalNode.position.x == 0)
			{
				hashSet.Add(chunk.neighbours.left);
				if (lightRemovalNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.leftBack);
				}
				else if (lightRemovalNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.leftFront);
				}
			}
			else if (lightRemovalNode.position.x == 15)
			{
				hashSet.Add(chunk.neighbours.right);
				if (lightRemovalNode.position.z == 0)
				{
					hashSet.Add(chunk.neighbours.rightBack);
				}
				else if (lightRemovalNode.position.z == 15)
				{
					hashSet.Add(chunk.neighbours.rightFront);
				}
			}
			if (lightRemovalNode.position.z == 0)
			{
				hashSet.Add(chunk.neighbours.back);
			}
			else if (lightRemovalNode.position.z == 15)
			{
				hashSet.Add(chunk.neighbours.front);
			}
			Vector3Int position = lightRemovalNode.position;
			byte lightLevel = lightRemovalNode.lightLevel;
			position.x--;
			Chunk chunk2;
			if (position.x < 0)
			{
				position.x = 15;
				chunk2 = lightRemovalNode.chunk.neighbours.left;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			int num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			byte sunLight = GetSunLight(chunk2.lightMap[num]);
			byte b = chunk2.voxels[num];
			byte value;
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 0))
			{
				if (sunLight != 0 && sunLight < lightLevel)
				{
					byte b2 = sunLight;
					SetSunLight(ref chunk2.lightMap[num], 0);
					if (position.x == 0)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.left);
						if (position.z == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftFront);
						}
						else if (position.z == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftBack);
						}
					}
					if (b2 > 0)
					{
						chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b2, b, value));
					}
				}
				else
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position.x = lightRemovalNode.position.x + 1;
			if (position.x > 15)
			{
				position.x = 0;
				chunk2 = lightRemovalNode.chunk.neighbours.right;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			sunLight = GetSunLight(chunk2.lightMap[num]);
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 1))
			{
				if (sunLight != 0 && sunLight < lightLevel)
				{
					byte b3 = sunLight;
					SetSunLight(ref chunk2.lightMap[num], 0);
					if (position.x == 15)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.right);
						if (position.z == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightFront);
						}
						else if (position.z == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightBack);
						}
					}
					if (b3 > 0)
					{
						chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b3, b, value));
					}
				}
				else
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position = lightRemovalNode.position;
			position.z--;
			if (position.z < 0)
			{
				position.z = 15;
				chunk2 = lightRemovalNode.chunk.neighbours.back;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			sunLight = GetSunLight(chunk2.lightMap[num]);
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 4))
			{
				if (sunLight != 0 && sunLight < lightLevel)
				{
					byte b4 = sunLight;
					SetSunLight(ref chunk2.lightMap[num], 0);
					if (position.z == 0)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.back);
						if (position.x == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightBack);
						}
						else if (position.x == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftBack);
						}
					}
					if (b4 > 0)
					{
						chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b4, b, value));
					}
				}
				else
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position = lightRemovalNode.position;
			position.z = lightRemovalNode.position.z + 1;
			if (position.z > 15)
			{
				position.z = 0;
				chunk2 = lightRemovalNode.chunk.neighbours.front;
			}
			else
			{
				chunk2 = lightRemovalNode.chunk;
			}
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			sunLight = GetSunLight(chunk2.lightMap[num]);
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 5))
			{
				if (sunLight != 0 && sunLight < lightLevel)
				{
					byte b5 = sunLight;
					SetSunLight(ref chunk2.lightMap[num], 0);
					if (position.z == 15)
					{
						hashSet.Add(lightRemovalNode.chunk.neighbours.front);
						if (position.x == 15)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.rightFront);
						}
						else if (position.x == 0)
						{
							hashSet.Add(lightRemovalNode.chunk.neighbours.leftFront);
						}
					}
					if (b5 > 0)
					{
						chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b5, b, value));
					}
				}
				else
				{
					chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
					hashSet2.Add(chunk2);
				}
			}
			position = lightRemovalNode.position;
			chunk2 = lightRemovalNode.chunk;
			if (position.y > 0)
			{
				position.y--;
				num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
				sunLight = GetSunLight(chunk2.lightMap[num]);
				b = chunk2.voxels[num];
				if (b >= 101 && b <= 126)
				{
					chunk2.blockOrientation.TryGetValue(num, out value);
				}
				else
				{
					value = 0;
				}
				if (LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 2))
				{
					if ((sunLight != 0 && sunLight < lightLevel) || sunLight == 15)
					{
						byte b6 = sunLight;
						SetSunLight(ref chunk2.lightMap[num], 0);
						if (b6 > 0)
						{
							chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b6, b, value));
						}
					}
					else
					{
						chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
						hashSet2.Add(chunk2);
					}
				}
			}
			position.y = lightRemovalNode.position.y;
			if (position.y >= Chunk.HEIGHT - 1)
			{
				continue;
			}
			position.y++;
			num = position.x * Chunk.HEIGHT * Chunk.WIDTH + position.y * Chunk.WIDTH + position.z;
			sunLight = GetSunLight(chunk2.lightMap[num]);
			b = chunk2.voxels[num];
			if (b >= 101 && b <= 126)
			{
				chunk2.blockOrientation.TryGetValue(num, out value);
			}
			else
			{
				value = 0;
			}
			if (!LightPropagationAllowanceCheck(lightRemovalNode.voxelID, lightRemovalNode.voxelOrientation, b, value, 3))
			{
				continue;
			}
			if (sunLight != 0 && sunLight < lightLevel)
			{
				byte b7 = sunLight;
				SetSunLight(ref chunk2.lightMap[num], 0);
				if (b7 > 0)
				{
					chunk.sunLightRemovalBfsQueue.Enqueue(new LightRemovalNode(position, chunk2, b7, b, value));
				}
			}
			else
			{
				chunk.sunLightBfsQueue.Enqueue(new LightNode(position, chunk2, b, value));
				hashSet2.Add(chunk2);
			}
		}
		chunk.pendingLightRemoval = false;
		foreach (Chunk item in hashSet)
		{
			item.pendingLightModifications = true;
		}
		foreach (Chunk item2 in hashSet2)
		{
			item2.pendingLightPropagation = true;
		}
	}

	private static bool LightPropagationAllowanceCheck(byte originID, byte originOrientation, byte destinationID, byte destinationOrientation, byte dir)
	{
		bool result = false;
		bool flag = false;
		uint num = 0u;
		bool flag2 = originID <= 21 || (originID >= 94 && originID <= 100) || (originID >= 85 && originID <= 85) || originID >= 241;
		if (originID >= 101 && originID <= 113)
		{
			switch (originOrientation)
			{
			case 0:
				flag2 = ((dir != 5 && dir != 2) ? true : false);
				break;
			case 1:
				flag2 = ((dir != 1 && dir != 2) ? true : false);
				break;
			case 2:
				flag2 = ((dir != 4 && dir != 2) ? true : false);
				break;
			case 3:
				flag2 = ((dir != 0 && dir != 2) ? true : false);
				break;
			case 4:
				flag2 = ((dir != 5 && dir != 3) ? true : false);
				break;
			case 5:
				flag2 = ((dir != 1 && dir != 3) ? true : false);
				break;
			case 6:
				flag2 = ((dir != 4 && dir != 3) ? true : false);
				break;
			case 7:
				flag2 = ((dir != 0 && dir != 3) ? true : false);
				break;
			}
			if (flag2)
			{
				num = stairsLightExitBitmask[originOrientation] & dirMaskSubstractor[dir];
				flag = true;
			}
		}
		else if (originID >= 114 && originID <= 126)
		{
			switch (originOrientation)
			{
			case 0:
				flag2 = dir != 5;
				break;
			case 1:
				flag2 = dir != 1;
				break;
			case 2:
				flag2 = dir != 4;
				break;
			case 3:
				flag2 = dir != 0;
				break;
			case 4:
				flag2 = dir != 3;
				break;
			case 5:
				flag2 = dir != 2;
				break;
			}
			if (flag2)
			{
				num = slabLightExitBitmask[originOrientation] & dirMaskSubstractor[dir];
				flag = true;
			}
		}
		if (flag2)
		{
			result = destinationID <= 21 || (destinationID >= 94 && destinationID <= 100) || (destinationID >= 85 && destinationID <= 85) || destinationID >= 241;
			if (destinationID >= 101 && destinationID <= 113)
			{
				switch (destinationOrientation)
				{
				case 0:
					result = dir != 4 && dir != 3 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 1:
					result = dir != 0 && dir != 3 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 2:
					result = dir != 5 && dir != 3 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 3:
					result = dir != 1 && dir != 3 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 4:
					result = dir != 4 && dir != 2 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 5:
					result = dir != 0 && dir != 2 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 6:
					result = dir != 5 && dir != 2 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 7:
					result = dir != 1 && dir != 2 && (!flag || (((num & stairsLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				}
			}
			else if (destinationID >= 114 && destinationID <= 126)
			{
				switch (destinationOrientation)
				{
				case 0:
					result = dir != 4 && (!flag || (((num & slabLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 1:
					result = dir != 0 && (!flag || (((num & slabLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 2:
					result = dir != 5 && (!flag || (((num & slabLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 3:
					result = dir != 1 && (!flag || (((num & slabLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 4:
					result = dir != 2 && (!flag || (((num & slabLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				case 5:
					result = dir != 3 && (!flag || (((num & slabLightEntranceBitmask[destinationOrientation]) != 0) ? true : false));
					break;
				}
			}
		}
		return result;
	}
}
