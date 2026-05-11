using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StorageManager
{
	public static string savePath;

	public static void CreateSavePath(string worldName)
	{
		savePath = Application.dataPath + "/Saves~/" + worldName + "/";
	}

	public static void SaveChunk(Chunk chunk)
	{
		List<int> list = new List<int>();
		byte b = 0;
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.WIDTH; j++)
			{
				int num = 0;
				int num2 = 0;
				byte b2 = chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num * Chunk.WIDTH + j];
				for (num++; num < Chunk.HEIGHT; num++)
				{
					b = chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num * Chunk.WIDTH + j];
					if (b != b2)
					{
						list.Add(SetCollumnData(b2, num - num2));
						num2 = num;
						b2 = b;
					}
				}
				list.Add(SetCollumnData(b2, num - num2));
			}
		}
		List<int> list2 = new List<int>();
		List<byte> list3 = new List<byte>();
		foreach (KeyValuePair<int, byte> item in chunk.blockOrientation)
		{
			list2.Add(item.Key);
			list3.Add(item.Value);
		}
		List<int> list4 = new List<int>();
		list4.Add(chunk.leftBackDecorationIndex.Length);
		list4.Add(chunk.backDecorationIndex.Length);
		list4.Add(chunk.rightBackDecorationIndex.Length);
		list4.Add(chunk.leftDecorationIndex.Length);
		list4.Add(chunk.rightDecorationIndex.Length);
		list4.Add(chunk.leftFrontDecorationIndex.Length);
		list4.Add(chunk.frontDecorationIndex.Length);
		list4.Add(chunk.rightFrontDecorationIndex.Length);
		List<int> list5 = new List<int>();
		List<byte> list6 = new List<byte>();
		for (int k = 0; k < chunk.leftBackDecorationIndex.Length; k++)
		{
			list5.Add(chunk.leftBackDecorationIndex[k]);
			list6.Add(chunk.leftBackDecorationID[k]);
		}
		for (int l = 0; l < chunk.backDecorationIndex.Length; l++)
		{
			list5.Add(chunk.backDecorationIndex[l]);
			list6.Add(chunk.backDecorationID[l]);
		}
		for (int m = 0; m < chunk.rightBackDecorationIndex.Length; m++)
		{
			list5.Add(chunk.rightBackDecorationIndex[m]);
			list6.Add(chunk.rightBackDecorationID[m]);
		}
		for (int n = 0; n < chunk.leftDecorationIndex.Length; n++)
		{
			list5.Add(chunk.leftDecorationIndex[n]);
			list6.Add(chunk.leftDecorationID[n]);
		}
		for (int num3 = 0; num3 < chunk.rightDecorationIndex.Length; num3++)
		{
			list5.Add(chunk.rightDecorationIndex[num3]);
			list6.Add(chunk.rightDecorationID[num3]);
		}
		for (int num4 = 0; num4 < chunk.leftFrontDecorationIndex.Length; num4++)
		{
			list5.Add(chunk.leftFrontDecorationIndex[num4]);
			list6.Add(chunk.leftFrontDecorationID[num4]);
		}
		for (int num5 = 0; num5 < chunk.frontDecorationIndex.Length; num5++)
		{
			list5.Add(chunk.frontDecorationIndex[num5]);
			list6.Add(chunk.frontDecorationID[num5]);
		}
		for (int num6 = 0; num6 < chunk.rightFrontDecorationIndex.Length; num6++)
		{
			list5.Add(chunk.rightFrontDecorationIndex[num6]);
			list6.Add(chunk.rightFrontDecorationID[num6]);
		}
		byte[] array = new byte[(1 + list.Count + 1 + list2.Count + list4.Count + list5.Count) * 4 + (list6.Count + list3.Count)];
		int num7 = 0;
		Buffer.BlockCopy(BitConverter.GetBytes(list.Count), 0, array, 0, 4);
		num7 += 4;
		Buffer.BlockCopy(list.ToArray(), 0, array, num7, list.Count * 4);
		num7 += list.Count * 4;
		Buffer.BlockCopy(BitConverter.GetBytes(list2.Count), 0, array, num7, 4);
		num7 += 4;
		if (list2.Count > 0)
		{
			Buffer.BlockCopy(list2.ToArray(), 0, array, num7, list2.Count * 4);
			num7 += list2.Count * 4;
			Buffer.BlockCopy(list3.ToArray(), 0, array, num7, list3.Count);
			num7 += list3.Count;
		}
		Buffer.BlockCopy(list4.ToArray(), 0, array, num7, list4.Count * 4);
		num7 += list4.Count * 4;
		Buffer.BlockCopy(list5.ToArray(), 0, array, num7, list5.Count * 4);
		num7 += list5.Count * 4;
		Buffer.BlockCopy(list6.ToArray(), 0, array, num7, list6.Count);
		num7 += list6.Count;
		File.WriteAllBytes(GetChunkSavePath(chunk.bounds.chunkPosition), array);
	}

	public static void LoadChunk(Chunk chunk)
	{
		byte[] src = File.ReadAllBytes(GetChunkSavePath(chunk.bounds.chunkPosition));
		int num = 0;
		int[] array = new int[4];
		Buffer.BlockCopy(src, num, array, 0, 4);
		num += 4;
		int[] array2 = new int[array[0]];
		Buffer.BlockCopy(src, num, array2, 0, array2.Length * 4);
		num += array2.Length * 4;
		byte b = 0;
		int collumnLength = 0;
		int num2 = 0;
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.WIDTH; j++)
			{
				int num3 = 0;
				int num4 = -1;
				while (num3 < Chunk.HEIGHT)
				{
					b = 0;
					GetCollumnData(array2[num2++], ref b, ref collumnLength);
					if (b >= 22 && b <= 92)
					{
						num4 = num3 + collumnLength;
					}
					for (int k = 0; k < collumnLength; k++)
					{
						chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num3 * Chunk.WIDTH + j] = b;
						if (b >= 241 && b <= 249)
						{
							chunk.waterSimulationSet.Add(new Vector3Int(i, num3, j));
						}
						else if ((b >= 85 && b <= 85) || (b >= 21 && b <= 21))
						{
							LightManager.PlaceBlockLight(chunk, new Vector3Int(i, num3, j), 15);
						}
						if (b >= 1 && b <= 20)
						{
							chunk.plants.Add(i * Chunk.HEIGHT * Chunk.WIDTH + num3 * Chunk.WIDTH + j);
						}
						num3++;
					}
				}
				chunk.heightMap[i * Chunk.WIDTH + j] = num4;
			}
		}
		int[] array3 = new int[4];
		Buffer.BlockCopy(src, num, array3, 0, 4);
		num += 4;
		if (array3[0] > 0)
		{
			int[] array4 = new int[array3[0]];
			Buffer.BlockCopy(src, num, array4, 0, array4.Length * 4);
			num += array4.Length * 4;
			byte[] array5 = new byte[array3[0]];
			Buffer.BlockCopy(src, num, array5, 0, array5.Length);
			num += array5.Length;
			for (int l = 0; l < array4.Length; l++)
			{
				chunk.blockOrientation.Add(array4[l], array5[l]);
			}
		}
		int[] array6 = new int[8];
		Buffer.BlockCopy(src, num, array6, 0, array6.Length * 4);
		num += array6.Length * 4;
		int num5 = 0;
		int[] array7 = array6;
		foreach (int num6 in array7)
		{
			num5 += num6;
		}
		int num7 = num + num5 * 4;
		if (array6[0] > 0)
		{
			chunk.leftBackDecorationIndex = new int[array6[0]];
			Buffer.BlockCopy(src, num, chunk.leftBackDecorationIndex, 0, chunk.leftBackDecorationIndex.Length * 4);
			num += chunk.leftBackDecorationIndex.Length * 4;
			chunk.leftBackDecorationID = new byte[array6[0]];
			Buffer.BlockCopy(src, num7, chunk.leftBackDecorationID, 0, chunk.leftBackDecorationID.Length);
			num7 += chunk.leftBackDecorationID.Length;
		}
		if (array6[1] > 0)
		{
			chunk.backDecorationIndex = new int[array6[1]];
			Buffer.BlockCopy(src, num, chunk.backDecorationIndex, 0, chunk.backDecorationIndex.Length * 4);
			num += chunk.backDecorationIndex.Length * 4;
			chunk.backDecorationID = new byte[array6[1]];
			Buffer.BlockCopy(src, num7, chunk.backDecorationID, 0, chunk.backDecorationID.Length);
			num7 += chunk.backDecorationID.Length;
		}
		if (array6[2] > 0)
		{
			chunk.rightBackDecorationIndex = new int[array6[2]];
			Buffer.BlockCopy(src, num, chunk.rightBackDecorationIndex, 0, chunk.rightBackDecorationIndex.Length * 4);
			num += chunk.rightBackDecorationIndex.Length * 4;
			chunk.rightBackDecorationID = new byte[array6[2]];
			Buffer.BlockCopy(src, num7, chunk.rightBackDecorationID, 0, chunk.rightBackDecorationID.Length);
			num7 += chunk.rightBackDecorationID.Length;
		}
		if (array6[3] > 0)
		{
			chunk.leftDecorationIndex = new int[array6[3]];
			Buffer.BlockCopy(src, num, chunk.leftDecorationIndex, 0, chunk.leftDecorationIndex.Length * 4);
			num += chunk.leftDecorationIndex.Length * 4;
			chunk.leftDecorationID = new byte[array6[3]];
			Buffer.BlockCopy(src, num7, chunk.leftDecorationID, 0, chunk.leftDecorationID.Length);
			num7 += chunk.leftDecorationID.Length;
		}
		if (array6[4] > 0)
		{
			chunk.rightDecorationIndex = new int[array6[4]];
			Buffer.BlockCopy(src, num, chunk.rightDecorationIndex, 0, chunk.rightDecorationIndex.Length * 4);
			num += chunk.rightDecorationIndex.Length * 4;
			chunk.rightDecorationID = new byte[array6[4]];
			Buffer.BlockCopy(src, num7, chunk.rightDecorationID, 0, chunk.rightDecorationID.Length);
			num7 += chunk.rightDecorationID.Length;
		}
		if (array6[5] > 0)
		{
			chunk.leftFrontDecorationIndex = new int[array6[5]];
			Buffer.BlockCopy(src, num, chunk.leftFrontDecorationIndex, 0, chunk.leftFrontDecorationIndex.Length * 4);
			num += chunk.leftFrontDecorationIndex.Length * 4;
			chunk.leftFrontDecorationID = new byte[array6[5]];
			Buffer.BlockCopy(src, num7, chunk.leftFrontDecorationID, 0, chunk.leftFrontDecorationID.Length);
			num7 += chunk.leftFrontDecorationID.Length;
		}
		if (array6[6] > 0)
		{
			chunk.frontDecorationIndex = new int[array6[6]];
			Buffer.BlockCopy(src, num, chunk.frontDecorationIndex, 0, chunk.frontDecorationIndex.Length * 4);
			num += chunk.frontDecorationIndex.Length * 4;
			chunk.frontDecorationID = new byte[array6[6]];
			Buffer.BlockCopy(src, num7, chunk.frontDecorationID, 0, chunk.frontDecorationID.Length);
			num7 += chunk.frontDecorationID.Length;
		}
		if (array6[7] > 0)
		{
			chunk.rightFrontDecorationIndex = new int[array6[7]];
			Buffer.BlockCopy(src, num, chunk.rightFrontDecorationIndex, 0, chunk.rightFrontDecorationIndex.Length * 4);
			num += chunk.rightFrontDecorationIndex.Length * 4;
			chunk.rightFrontDecorationID = new byte[array6[7]];
			Buffer.BlockCopy(src, num7, chunk.rightFrontDecorationID, 0, chunk.rightFrontDecorationID.Length);
			num7 += chunk.rightFrontDecorationID.Length;
		}
		NoiseManager.instance.StoreHighestXAndZRows(chunk);
	}

	public static bool ExistsOnDisk(Vector3 chunkPosition)
	{
		return File.Exists(GetChunkSavePath(chunkPosition));
	}

	private static string GetChunkSavePath(Vector3 chunkPosition)
	{
		return savePath + (int)chunkPosition.x + ";" + (int)chunkPosition.z + ".vox";
	}

	public static int SetCollumnData(byte voxelID, int collumnLength)
	{
		return (voxelID << 16) | (collumnLength & 0xFFFF);
	}

	public static void GetCollumnData(int collumnData, ref byte voxelID, ref int collumnLength)
	{
		voxelID = (byte)(collumnData >> 16);
		collumnLength = collumnData & 0xFFFF;
	}

	public static void SaveIntInFirst16Bits(ref int collumnData, int value)
	{
		collumnData &= -65536;
		collumnData |= value & 0xFFFF;
	}

	public static void SaveIntInLast16Bits(ref int collumnData, int value)
	{
		collumnData &= 65535;
		collumnData |= (value & 0xFFFF) << 16;
	}

	public static int ReadIntFromFirst16Bits(int collumnData)
	{
		return collumnData & 0xFFFF;
	}

	public static int ReadIntFromLast16Bits(int collumnData)
	{
		return (collumnData >> 16) & 0xFFFF;
	}

	public static void SavePlayerData(Vector3 playerPosition, Quaternion playerRotation, float verticalVelocity, Vector3Int worldShiftOffset)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			binaryWriter.Write(playerPosition.x);
			binaryWriter.Write(playerPosition.y);
			binaryWriter.Write(playerPosition.z);
			binaryWriter.Write(playerRotation.x);
			binaryWriter.Write(playerRotation.y);
			binaryWriter.Write(playerRotation.z);
			binaryWriter.Write(playerRotation.w);
			binaryWriter.Write(verticalVelocity);
			binaryWriter.Write(worldShiftOffset.x);
			binaryWriter.Write(worldShiftOffset.z);
		}
		File.WriteAllBytes(savePath + "/player.dat", memoryStream.ToArray());
	}

	public static void LoadPlayerData(out Vector3 playerPosition, out Quaternion playerRotation, out float verticalVelocity, out Vector3Int worldShiftOffset)
	{
		if (!File.Exists(savePath + "/player.dat"))
		{
			playerPosition = new Vector3(0.5f, 0f, 0.5f);
			playerRotation = Quaternion.identity;
			verticalVelocity = 0f;
			worldShiftOffset = Vector3Int.zero;
			return;
		}
		using MemoryStream input = new MemoryStream(File.ReadAllBytes(savePath + "/player.dat"));
		using BinaryReader binaryReader = new BinaryReader(input);
		playerPosition = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		playerRotation = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		verticalVelocity = binaryReader.ReadSingle();
		worldShiftOffset = new Vector3Int(binaryReader.ReadInt32(), 0, binaryReader.ReadInt32());
	}
}
