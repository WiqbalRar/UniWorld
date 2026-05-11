using UnityEngine;

public static class TreeManager
{
	private static int minAcaciaHeight = 4;

	private static float maxAcaciaHeightFromMin = 7f;

	private static int fancyOakMinBranchCount = 2;

	private static float fancyOakbonusBranchCount = 6f;

	private static float fancyOakBonusHeight = 5f;

	private static int darkOakMinHeight = 6;

	private static float darkOakBonusHeight = 4f;

	private static int minJungleTreeHeight = 10;

	private static float maxJungleTreeBonusHeight = 12f;

	public static void GenerateAcacia(Chunk chunk, int x, int y, int z)
	{
		int num = 0;
		Vector3Int zero = Vector3Int.zero;
		float noise = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
		bool flag = false;
		int num2 = minAcaciaHeight + Mathf.RoundToInt(maxAcaciaHeightFromMin * Mathf.Abs(noise));
		for (int i = 0; i < num2; i++)
		{
			if (i > 1)
			{
				if (zero == Vector3Int.zero)
				{
					float noise2 = NoiseManager.instance.whiteNoise3.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + i + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
					if (noise2 > 0.5f)
					{
						noise2 = noise2 % 0.5f * 2f;
						if ((double)noise2 < 0.25)
						{
							zero.x = -1;
						}
						else if (noise2 < 0.5f)
						{
							zero.x = 1;
						}
						else if ((double)noise2 < 0.75)
						{
							zero.z = -1;
						}
						else
						{
							zero.z = 1;
						}
					}
				}
				else
				{
					num++;
				}
			}
			if (!flag && i > 3 && i + 1 < num2)
			{
				float noise3 = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
				if (noise > 0.7f)
				{
					flag = true;
					Vector3 vector = Vector3.zero;
					if (zero == Vector3Int.zero)
					{
						noise3 = noise3 % 0.3f * 3.33f;
						if ((double)noise3 < 0.25)
						{
							vector.x = -1f;
						}
						else if (noise3 < 0.5f)
						{
							vector.x = 1f;
						}
						else if ((double)noise3 < 0.75)
						{
							vector.z = -1f;
						}
						else
						{
							vector.z = 1f;
						}
					}
					else
					{
						noise3 = noise3 % 0.3f * 3.33f;
						vector = (((double)noise3 < 0.33) ? (Quaternion.Euler(0f, 90f, 0f) * vector) : ((!(noise3 < 0.66f)) ? (Quaternion.Euler(0f, 270f, 0f) * vector) : (Quaternion.Euler(0f, 180f, 0f) * vector)));
					}
					DecorationManager.instance.PlaceDecoration(chunk, x + (int)vector.x, y, z + (int)vector.z, 91);
					if ((double)(Mathf.Abs(noise) % 0.1f) > 0.05)
					{
						vector.x += vector.x;
						vector.z += vector.z;
						vector.y += 1f;
						DecorationManager.instance.PlaceDecoration(chunk, x + (int)vector.x, y + (int)vector.y, z + (int)vector.z, 91);
					}
					GenerateSquareWithoutCorners(chunk, x + (int)vector.x, y + (int)vector.y, z + (int)vector.z, 2, 98);
					GenerateSquare(chunk, x + (int)vector.x, y + 1 + (int)vector.y, z + (int)vector.z, 1, 98);
				}
			}
			x += zero.x;
			z += zero.z;
			DecorationManager.instance.PlaceDecoration(chunk, x, y, z, 91);
			y++;
			if (num == 3)
			{
				break;
			}
		}
		y--;
		GenerateSquareWithoutCorners(chunk, x, y, z, 3, 98);
		GenerateDiamondShape(chunk, x, y + 1, z, 2, 98);
	}

	public static void GenerateBasicTree(Chunk chunk, int x, int y, int z, byte logID, byte leafID, int minHeight = 5, float bonusHeight = 2f)
	{
		float noise = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
		int num = minHeight + Mathf.RoundToInt(bonusHeight * Mathf.Abs(noise));
		for (int i = 0; i < num; i++)
		{
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, logID);
		}
		GenerateDiamondShape(chunk, x, y--, z, 1, leafID);
		GenerateSquareWithPotentialCorners(chunk, x, y--, z, 1, 2, 0.6f, leafID);
		GenerateSquareWithPotentialCorners(chunk, x, y--, z, 2, 3, 0.65f, leafID);
		GenerateSquareWithPotentialCorners(chunk, x, y--, z, 2, 3, 0.65f, leafID, 1);
	}

	public static void GenerateFancyTree(Chunk chunk, int x, int y, int z, byte logID, byte leafID)
	{
		float noise = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
		float noise2 = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + 1 + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
		int num = fancyOakMinBranchCount + Mathf.RoundToInt(Mathf.Abs(noise2 * fancyOakbonusBranchCount));
		DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, logID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, logID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, logID);
		int num2 = Mathf.RoundToInt(fancyOakBonusHeight * Mathf.Abs(noise));
		for (int i = 0; i < num; i++)
		{
			float noise3 = NoiseManager.instance.whiteNoise3.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + i + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
			int num3 = 1 + Mathf.RoundToInt(Mathf.Abs(1f * noise3));
			int num4 = Mathf.RoundToInt(Mathf.Lerp(0f, num2, NoiseManager.instance.whiteNoise3.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y - i + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z)));
			Vector3Int zero = Vector3Int.zero;
			float num5 = Mathf.Abs(noise3) % 0.25f * 4f;
			zero = ((num5 < 0.125f) ? new Vector3Int(-1, 0, 0) : ((num5 < 0.25f) ? new Vector3Int(1, 0, 0) : ((num5 < 0.375f) ? new Vector3Int(0, 0, -1) : ((num5 < 0.5f) ? new Vector3Int(0, 0, 1) : (((double)num5 < 0.625) ? new Vector3Int(-1, 1, 0) : (((double)num5 < 0.75) ? new Vector3Int(1, 1, 0) : ((!((double)num5 < 0.875)) ? new Vector3Int(0, 1, 1) : new Vector3Int(0, 1, -1))))))));
			Vector3Int vector3Int = zero;
			for (int j = 0; j < num3; j++)
			{
				DecorationManager.instance.PlaceDecoration(chunk, x + vector3Int.x, y + num4 + vector3Int.y, z + vector3Int.z, logID);
				vector3Int += zero;
			}
			vector3Int -= zero;
			GenerateLeafBall(chunk, x + vector3Int.x, y + num4 + vector3Int.y, z + vector3Int.z, leafID);
		}
		for (int k = 0; k < num2; k++)
		{
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, logID);
		}
		y--;
		GenerateLeafBall(chunk, x, y, z, leafID);
	}

	public static void GenerateDarkOak(Chunk chunk, int x, int y, int z)
	{
		float noise = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
		int num = darkOakMinHeight + Mathf.RoundToInt(Mathf.Abs(noise) * darkOakBonusHeight);
		GeneratePillarWithProbableBonusHeight(chunk, x - 1, y - 2, z, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x - 1, y - 2, z + 1, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 2, z + 2, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x + 1, y - 2, z + 2, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x + 2, y - 2, z, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x + 2, y - 2, z + 1, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 2, z - 1, 2, 2, 88);
		GeneratePillarWithProbableBonusHeight(chunk, x + 1, y - 2, z - 1, 2, 2, 88);
		for (int i = y - 2; i <= y + num; i++)
		{
			GeneratesquareFromCorner(chunk, x, i, z, 2, 2, 88);
		}
		y += num;
		int num2 = 8;
		for (int j = 0; j < num2; j++)
		{
			int num3 = Mathf.RoundToInt(Mathf.Abs(NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y - j + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z)) * 8f);
			Vector3Int vector3Int = new Vector3Int(x, y, z);
			Vector3Int vector3Int2 = new Vector3Int(0, 0, 0);
			switch (num3)
			{
			case 0:
				vector3Int.x--;
				vector3Int2.x--;
				break;
			case 1:
				vector3Int.x--;
				vector3Int.z++;
				vector3Int2.x--;
				break;
			case 2:
				vector3Int.z += 2;
				vector3Int2.z++;
				break;
			case 3:
				vector3Int.x++;
				vector3Int.z += 2;
				vector3Int2.z++;
				break;
			case 4:
				vector3Int.x += 2;
				vector3Int.z++;
				vector3Int2.x++;
				break;
			case 5:
				vector3Int.x += 2;
				vector3Int2.x++;
				break;
			case 6:
				vector3Int.x++;
				vector3Int.z--;
				vector3Int2.z--;
				break;
			case 7:
				vector3Int.z--;
				vector3Int2.z--;
				break;
			}
			int num4 = 5;
			Vector3Int vector3Int3 = new Vector3Int(vector3Int2.z, 0, vector3Int2.x);
			for (int k = 0; k < num4; k++)
			{
				float noise2 = NoiseManager.instance.whiteNoise3.fastNoise.GetNoise(vector3Int.x + chunk.bounds.LeftBotBack.x, vector3Int.y + chunk.bounds.LeftBotBack.y - j, vector3Int.z + chunk.bounds.LeftBotBack.z);
				noise2 = Mathf.Abs(noise2);
				if ((double)noise2 % 0.5 > 0.25)
				{
					vector3Int.y++;
				}
				if (noise2 >= 0.5f)
				{
					if (noise2 > 0.75f)
					{
						vector3Int += vector3Int3;
					}
					else
					{
						vector3Int += -vector3Int3;
					}
				}
				DecorationManager.instance.PlaceDecoration(chunk, vector3Int.x, vector3Int.y, vector3Int.z, 88);
				if (k % 2 == 0)
				{
					GenerateLeafBall(chunk, vector3Int.x, vector3Int.y, vector3Int.z, 95);
				}
				vector3Int += vector3Int2;
			}
		}
		for (int l = 0; l < 3; l++)
		{
			int num5 = Mathf.RoundToInt(Mathf.Abs(NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y - l + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z)) * 4f);
			Vector3Int vector3Int4 = new Vector3Int(x, y, z);
			Vector3Int vector3Int5 = new Vector3Int(0, 0, 0);
			switch (num5)
			{
			case 0:
				vector3Int5.x--;
				break;
			case 1:
				vector3Int4.z++;
				vector3Int5.z++;
				break;
			case 2:
				vector3Int4.x++;
				vector3Int4.z++;
				vector3Int5.z++;
				break;
			case 3:
				vector3Int4.x++;
				vector3Int5.z--;
				break;
			}
			int num6 = 5;
			Vector3Int vector3Int6 = new Vector3Int(vector3Int5.z, 0, vector3Int5.x);
			for (int m = 0; m < num6; m++)
			{
				float noise3 = NoiseManager.instance.whiteNoise3.fastNoise.GetNoise(vector3Int4.x + chunk.bounds.LeftBotBack.x, vector3Int4.y + chunk.bounds.LeftBotBack.y - l, vector3Int4.z + chunk.bounds.LeftBotBack.z);
				noise3 = Mathf.Abs(noise3);
				if (noise3 > 0.8175f)
				{
					vector3Int4 += vector3Int6;
				}
				else if (noise3 > 0.75f)
				{
					vector3Int4 += -vector3Int6;
				}
				else if (noise3 > 0.625f)
				{
					vector3Int4 += vector3Int5;
				}
				else if (noise3 > 0.5f)
				{
					vector3Int4 -= vector3Int5;
				}
				DecorationManager.instance.PlaceDecoration(chunk, vector3Int4.x, vector3Int4.y, vector3Int4.z, 88);
				if (m % 2 == 0)
				{
					GenerateLeafBall(chunk, vector3Int4.x, vector3Int4.y, vector3Int4.z, 95);
				}
				vector3Int4.y++;
			}
		}
	}

	public static void GenerateIceSpike(Chunk chunk, int x, int y, int z)
	{
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 4, z, 13, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x + 1, y - 3, z, 8, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x - 1, y - 3, z, 8, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 3, z + 1, 8, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 3, z - 1, 8, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x + 1, y - 2, z + 1, 4, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x + 1, y - 2, z - 1, 4, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x - 1, y - 2, z + 1, 4, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x - 1, y - 2, z - 1, 4, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x + 2, y - 1, z, 1, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x - 2, y - 1, z, 1, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 1, z + 2, 1, 2, 27);
		GeneratePillarWithProbableBonusHeight(chunk, x, y - 1, z - 2, 1, 2, 27);
	}

	public static void GenerateSpruce(Chunk chunk, int x, int y, int z, int spruceID)
	{
		switch (spruceID)
		{
		case 0:
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceTip(chunk, x, y, z, 90, 97);
			break;
		case 1:
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceTip(chunk, x, y, z, 90, 97);
			break;
		case 2:
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer1(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceTip(chunk, x, y, z, 90, 97);
			break;
		case 3:
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer1(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceTip(chunk, x, y, z, 90, 97);
			break;
		case 4:
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer1(chunk, x, y, z, 90, 97);
			y += 2;
			GenerateSpruceLayer1(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			GenerateSpruceLayer2(chunk, x, y, z, 90, 97);
			y += 2;
			DecorationManager.instance.PlaceDecoration(chunk, x, y++, z, 90);
			GenerateSpruceTip(chunk, x, y, z, 90, 97);
			break;
		}
	}

	public static void GenerateJungleTree(Chunk chunk, int x, int y, int z)
	{
		float noise = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
		int num = minJungleTreeHeight + Mathf.RoundToInt(maxJungleTreeBonusHeight * Mathf.Abs(noise));
		GeneratesquareFromCorner(chunk, x, y - 1, z, 2, 2, 86);
		int num2 = 0;
		for (int i = y; i < y + num; i++)
		{
			GeneratesquareFromCorner(chunk, x, i, z, 2, 2, 92);
			if (i - y > 3 && (float)i < (float)(y + num) - 2f)
			{
				float noise2 = NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, i + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z);
				noise2 = Mathf.Abs(noise2);
				if (noise2 >= 0.8f && num2 <= 0)
				{
					noise2 = noise2 % 0.2f * 5f;
					if (noise2 < 0.25f)
					{
						x--;
					}
					else if (!(noise2 < 0.5f))
					{
						z = ((!(noise2 < 0.75f)) ? (z + 1) : (z - 1));
					}
					else
					{
						x++;
					}
					num2 = 4;
				}
				else if (noise2 <= 0.2f)
				{
					Vector3Int zero = Vector3Int.zero;
					Vector3Int zero2 = Vector3Int.zero;
					Vector3Int vector3Int = new Vector3Int(x, i, z);
					noise2 = noise2 % 0.2f * 5f;
					if (noise2 < 0.125f)
					{
						zero = new Vector3Int(0, 0, 0);
						zero2 = new Vector3Int(-1, 0, 0);
					}
					else if (noise2 < 0.25f)
					{
						zero = new Vector3Int(0, 0, 1);
						zero2 = new Vector3Int(-1, 0, 0);
					}
					else if (noise2 < 0.375f)
					{
						zero = new Vector3Int(0, 0, 1);
						zero2 = new Vector3Int(0, 0, 1);
					}
					else if (noise2 < 0.5f)
					{
						zero = new Vector3Int(1, 0, 1);
						zero2 = new Vector3Int(0, 0, 1);
					}
					else if (noise2 < 0.625f)
					{
						zero = new Vector3Int(1, 0, 0);
						zero2 = new Vector3Int(1, 0, 0);
					}
					else if (noise2 < 0.75f)
					{
						zero = new Vector3Int(1, 0, 1);
						zero2 = new Vector3Int(1, 0, 0);
					}
					else if (noise2 < 875f)
					{
						zero = new Vector3Int(0, 0, 0);
						zero2 = new Vector3Int(0, 0, -1);
					}
					else
					{
						zero = new Vector3Int(1, 0, 0);
						zero2 = new Vector3Int(0, 0, -1);
					}
					vector3Int += zero;
					int num3 = 1 + Mathf.RoundToInt(2f * (noise2 % 0.125f) * 8f);
					for (int j = 0; j < num3; j++)
					{
						vector3Int += zero2;
						if (NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, i + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z) > 0f || j == 2)
						{
							vector3Int.y++;
						}
						DecorationManager.instance.PlaceDecoration(chunk, vector3Int.x, vector3Int.y, vector3Int.z, 92);
					}
					GenerateJungleSubCanopy(chunk, vector3Int.x, vector3Int.y, vector3Int.z, 92, 99);
				}
			}
			num2--;
		}
		GenerateJungleCanopyBot(chunk, x, y + num, z, 92, 99);
		y++;
		GenerateJungleCanopyMid(chunk, x, y + num, z, 92, 99);
		y++;
		GenerateJungleCanopyTop(chunk, x, y + num, z, 99);
	}

	public static void GenerateBush(Chunk chunk, int x, int y, int z, byte logID, byte leafID)
	{
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z, logID);
		GenerateSquareWithPotentialCorners(chunk, x, y, z, 2, 3, 0.75f, leafID);
		GenerateSquareWithPotentialCorners(chunk, x, y + 1, z, 1, 3, 0.65f, leafID);
		GenerateSquareWithPotentialCorners(chunk, x, y - 1, z, 2, 3, 0.65f, leafID);
		if (NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + 2 + chunk.bounds.LeftBotBack.y, z) > 0f)
		{
			DecorationManager.instance.PlaceDecoration(chunk, x, y + 2, z, leafID);
		}
	}

	public static void GenerateJungleCanopyBot(Chunk chunk, int x, int y, int z, byte logID, byte leafID)
	{
		GeneratesquareFromCorner(chunk, x, y, z, 2, 2, logID);
		GeneratesquareFromCorner(chunk, x - 5, y, z, 1, 2, leafID);
		GeneratesquareFromCorner(chunk, x - 4, y, z - 2, 1, 6, leafID);
		GeneratesquareFromCorner(chunk, x - 3, y, z - 3, 1, 8, leafID);
		GeneratesquareFromCorner(chunk, x - 2, y, z - 4, 2, 10, leafID);
		GeneratesquareFromCorner(chunk, x, y, z - 5, 2, 5, leafID);
		GeneratesquareFromCorner(chunk, x, y, z + 2, 2, 5, leafID);
		GeneratesquareFromCorner(chunk, x + 2, y, z - 4, 2, 10, leafID);
		GeneratesquareFromCorner(chunk, x + 4, y, z - 3, 1, 8, leafID);
		GeneratesquareFromCorner(chunk, x + 5, y, z - 2, 1, 6, leafID);
		GeneratesquareFromCorner(chunk, x + 6, y, z, 1, 2, leafID);
	}

	public static void GenerateJungleCanopyMid(Chunk chunk, int x, int y, int z, byte logID, byte leafID)
	{
		GeneratesquareFromCorner(chunk, x, y, z, 2, 2, logID);
		GeneratesquareFromCorner(chunk, x - 4, y, z, 1, 2, leafID);
		GeneratesquareFromCorner(chunk, x - 3, y, z - 2, 1, 6, leafID);
		GeneratesquareFromCorner(chunk, x - 2, y, z - 3, 2, 8, leafID);
		GeneratesquareFromCorner(chunk, x, y, z - 4, 2, 4, leafID);
		GeneratesquareFromCorner(chunk, x, y, z + 2, 2, 4, leafID);
		GeneratesquareFromCorner(chunk, x + 2, y, z - 3, 2, 8, leafID);
		GeneratesquareFromCorner(chunk, x + 4, y, z - 2, 1, 6, leafID);
		GeneratesquareFromCorner(chunk, x + 5, y, z, 1, 2, leafID);
	}

	public static void GenerateJungleCanopyTop(Chunk chunk, int x, int y, int z, byte leafID)
	{
		GeneratesquareFromCorner(chunk, x - 2, y, z - 2, 6, 6, leafID);
		GeneratesquareFromCorner(chunk, x - 3, y, z, 1, 2, leafID);
		GeneratesquareFromCorner(chunk, x + 4, y, z, 1, 2, leafID);
		GeneratesquareFromCorner(chunk, x, y, z - 3, 2, 1, leafID);
		GeneratesquareFromCorner(chunk, x, y, z + 4, 2, 1, leafID);
	}

	public static void GenerateJungleSubCanopy(Chunk chunk, int x, int y, int z, byte logID, byte leafID)
	{
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z, logID);
		GenerateDiamondShape(chunk, x, y, z, 2, leafID);
		GenerateDiamondShape(chunk, x, y + 1, z, 1, leafID);
	}

	public static void GeneratesquareFromCorner(Chunk chunk, int x, int y, int z, int xLength, int zLength, byte voxelID)
	{
		for (int i = 0; i < xLength; i++)
		{
			for (int j = 0; j < zLength; j++)
			{
				DecorationManager.instance.PlaceDecoration(chunk, x + i, y, z + j, voxelID);
			}
		}
	}

	public static void GenerateSpruceTip(Chunk chunk, int x, int y, int z, byte logId, byte leafID)
	{
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z, logId);
		DecorationManager.instance.PlaceDecoration(chunk, x - 1, y, z, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x + 1, y, z, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z - 1, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z + 1, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y + 1, z, leafID);
	}

	public static void GenerateSpruceLayer1(Chunk chunk, int x, int y, int z, byte logId, byte leafID)
	{
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z, logId);
		GenerateSquareWithoutCorners(chunk, x, y, z, 2, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x + 3, y, z - 1, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x + 3, y, z, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x + 3, y, z + 1, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x - 3, y, z - 1, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x - 3, y, z, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x - 3, y, z + 1, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x - 1, y, z - 3, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z - 3, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x + 1, y, z - 3, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x - 1, y, z + 3, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z + 3, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x + 1, y, z + 3, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y + 1, z, logId);
		GenerateDiamondShape(chunk, x, y + 1, z, 2, leafID);
	}

	public static void GenerateSpruceLayer2(Chunk chunk, int x, int y, int z, byte logId, byte leafID)
	{
		DecorationManager.instance.PlaceDecoration(chunk, x, y, z, logId);
		GenerateSquareWithoutCorners(chunk, x, y, z, 2, leafID);
		DecorationManager.instance.PlaceDecoration(chunk, x, y + 1, z, logId);
		GenerateDiamondShape(chunk, x, y + 1, z, 1, leafID);
	}

	public static void GenerateDiamondShape(Chunk chunk, int x, int y, int z, int radius, byte voxelID)
	{
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius + Mathf.Abs(i); j <= radius - Mathf.Abs(i); j++)
			{
				DecorationManager.instance.PlaceDecoration(chunk, x + j, y, z + i, voxelID);
			}
		}
	}

	public static void GeneratePillarWithProbableBonusHeight(Chunk chunk, int x, int y, int z, int height, int bonusHeight, byte voxelID)
	{
		int num = y + height + Mathf.RoundToInt((float)bonusHeight * NoiseManager.instance.whiteNoise3.fastNoise.GetNoise(x + chunk.bounds.LeftBotBack.x, y + chunk.bounds.LeftBotBack.y, z + chunk.bounds.LeftBotBack.z));
		for (int i = y; i < num; i++)
		{
			DecorationManager.instance.PlaceDecoration(chunk, x, i, z, voxelID);
		}
	}

	public static void GenerateLeafBall(Chunk chunk, int x, int y, int z, byte leafID)
	{
		GenerateDiamondShape(chunk, x, y, z, 1, leafID);
		GenerateSquareWithoutCorners(chunk, x, y + 1, z, 2, leafID);
		GenerateSquareWithoutCorners(chunk, x, y + 2, z, 2, leafID);
		GenerateSquareWithoutCorners(chunk, x, y + 3, z, 2, leafID);
		GenerateDiamondShape(chunk, x, y + 4, z, 1, leafID);
	}

	public static void GenerateSquareWithoutCorners(Chunk chunk, int x, int y, int z, int radius, byte voxelID)
	{
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				if (Mathf.Abs(i) + Mathf.Abs(j) != radius * 2)
				{
					DecorationManager.instance.PlaceDecoration(chunk, x + i, y, z + j, voxelID);
				}
			}
		}
	}

	public static void GenerateSquareWithPotentialCorners(Chunk chunk, int x, int y, int z, int radius, int maxCorners, float cornerProbability, byte voxelID, int noiseOffset = 0)
	{
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				if (Mathf.Abs(i) + Mathf.Abs(j) != radius * 2)
				{
					DecorationManager.instance.PlaceDecoration(chunk, x + i, y, z + j, voxelID);
				}
				else if (maxCorners > 0 && Mathf.Abs(NoiseManager.instance.whiteNoise2.fastNoise.GetNoise(x + i + chunk.bounds.LeftBotBack.x + noiseOffset, y + chunk.bounds.LeftBotBack.y, z + j + chunk.bounds.LeftBotBack.z + noiseOffset)) < cornerProbability)
				{
					maxCorners--;
					DecorationManager.instance.PlaceDecoration(chunk, x + i, y, z + j, voxelID);
				}
			}
		}
	}

	public static void GenerateSquare(Chunk chunk, int x, int y, int z, int radius, byte voxelID)
	{
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				DecorationManager.instance.PlaceDecoration(chunk, x + i, y, z + j, voxelID);
			}
		}
	}
}
