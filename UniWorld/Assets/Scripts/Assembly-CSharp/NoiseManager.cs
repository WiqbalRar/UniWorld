using UnityEngine;

public class NoiseManager : MonoBehaviour
{
	public float maxSquachingHeight = 80f;

	public static NoiseManager instance;

	public int seed = 1337;

	[Header("Terrain Shaping Noise")]
	public FastNoiseSIMDUnity noiseC;

	public FastNoiseSIMDUnity noiseE;

	public FastNoiseSIMDUnity noiseR;

	public FastNoiseSIMDUnity noiseM;

	public FastNoiseSIMDUnity noiseW;

	public FastNoiseSIMDUnity noiseW2;

	public FastNoiseSIMDUnity noiseT;

	public FastNoiseSIMDUnity noiseH;

	public FastNoiseSIMDUnity surfaceDensityNoise;

	public FastNoiseSIMDUnity cheeseCaveNoise;

	public FastNoiseSIMDUnity tunnelNoise1;

	public FastNoiseSIMDUnity tunnelNoise2;

	public FastNoiseSIMDUnity whiteNoise0;

	public FastNoiseSIMDUnity whiteNoise1;

	public FastNoiseUnity whiteNoise2;

	public FastNoiseUnity whiteNoise3;

	[Header("Terrain Sampling Splines")]
	public AnimationCurve splineC;

	public AnimationCurve splineE;

	public AnimationCurve splineR;

	public AnimationCurve splineM;

	public AnimationCurve splineW;

	public AnimationCurve splineW2;

	[Header("Ore Spawning Noise")]
	public int fakeSplineResolution = 2001;

	public float[] fakeSplineC;

	public float[] fakeSplineE;

	public float[] fakeSplineR;

	public float[] fakeSplineM;

	public float[] fakeSplineW;

	public float[] fakeSplineW2;

	[Header("Tarrain Parameters")]
	public int waterLevel = 128;

	[Header("Light Parameters")]
	public AnimationCurve splineSunLight;

	public byte[] sunLightValues;

	public AnimationCurve splineBlockLight;

	public byte[] blockLightValues;

	[Header("Tunnels settings")]
	public float CheeseCaveThreshold = -0.8f;

	public float TUNNEL_THRESHOLD = 0.04f;

	public float IDEAL_OPENSIMPLEX2_PAIR_VERTICAL_SAMPLING_OFFSET = 0.4330127f;

	public float CAVE_SHAPE_CLOSEOFF_SENSITIVITY = 2f;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
		seed = GameManager.instance.seed;
		StorageManager.CreateSavePath(GameManager.instance.worldName);
		SetupSeeds();
		InitializeFakeSplines();
	}

	private void SetupSeeds()
	{
		noiseC.seed = seed;
		noiseE.seed = seed + 1;
		noiseR.seed = seed + 2;
		noiseM.seed = seed + 3;
		noiseW.seed = seed + 4;
		noiseW2.seed = seed + 5;
		noiseT.seed = seed + 6;
		noiseH.seed = seed + 7;
		surfaceDensityNoise.seed = seed + 8;
		cheeseCaveNoise.seed = seed + 9;
		tunnelNoise1.seed = seed + 10;
		tunnelNoise2.seed = seed + 11;
		whiteNoise0.seed = seed + 12;
		whiteNoise1.seed = seed + 13;
		whiteNoise2.seed = seed + 14;
		whiteNoise3.seed = seed + 15;
	}

	public void FillNoiseSets(int xStart, int yStart, int zStart, Chunk chunk)
	{
		noiseC.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetC, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseE.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetE, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseR.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetR, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseM.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetM, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseW.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetW, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseW2.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetW2, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseT.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetT, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseH.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetH, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		whiteNoise0.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetWhite0, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		whiteNoise1.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetWhite1, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
	}

	public void FillImportantNoiseSet(int xStart, int yStart, int zStart, Chunk chunk)
	{
		noiseT.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetT, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		noiseH.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetH, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
		whiteNoise1.fastNoiseSIMD.FillNoiseSet(chunk.noiseSetWhite1, xStart, yStart, zStart, Chunk.WIDTH, 1, Chunk.WIDTH);
	}

	public void ProcessSplines(Chunk chunk)
	{
		for (int i = 0; i < chunk.noiseSetC.Length; i++)
		{
			chunk.noiseSetC[i] = fakeSplineC[Mathf.RoundToInt((chunk.noiseSetC[i] + 1f) * 1000f)];
			chunk.noiseSetE[i] = fakeSplineE[Mathf.RoundToInt((chunk.noiseSetE[i] + 1f) * 1000f)];
			chunk.noiseSetR[i] = fakeSplineR[Mathf.RoundToInt((chunk.noiseSetR[i] + 1f) * 1000f)];
			chunk.noiseSetM[i] = fakeSplineM[Mathf.RoundToInt((chunk.noiseSetM[i] + 1f) * 1000f)];
			chunk.noiseSetW[i] = fakeSplineW[Mathf.RoundToInt((chunk.noiseSetW[i] + 1f) * 1000f)];
			chunk.noiseSetW2[i] = fakeSplineW2[Mathf.RoundToInt((chunk.noiseSetW2[i] + 1f) * 1000f)];
		}
	}

	public int GenerateHeightMap(Chunk chunk)
	{
		int num = 0;
		for (int i = 0; i < chunk.heightMap.Length; i++)
		{
			float num2 = chunk.noiseSetC[i] + chunk.noiseSetE[i];
			float num3 = num2 - (float)waterLevel;
			if (num3 < 17f && chunk.noiseSetR[i] < -0.4f)
			{
				float t = (1f + chunk.noiseSetR[i]) / 0.6f;
				float num4 = Mathf.SmoothStep((float)waterLevel - 7f, num2, t);
				if (num4 < num2)
				{
					float t2 = ((!(num3 >= 0f)) ? 0f : (num3 / 17f));
					num2 = Mathf.SmoothStep(num4, num2, t2);
				}
			}
			float num5 = 50f;
			float num6 = 8f;
			if (num2 >= (float)waterLevel + num6)
			{
				float a = (num2 - (float)waterLevel - num6) / num5;
				a = Mathf.Min(a, 1f);
				num2 += chunk.noiseSetM[i] * a;
				num2 = Mathf.Min(num2, Chunk.HEIGHT - 1);
			}
			chunk.heightMap[i] = num2;
			num = Mathf.Max(num, Mathf.CeilToInt(num2));
		}
		return num;
	}

	public void GenerateSolidArray(Chunk chunk)
	{
		int num = GenerateHeightMap(chunk) + 2 + (int)maxSquachingHeight;
		int num2 = num + 4 + (int)maxSquachingHeight;
		float[] array = new float[Chunk.WIDTHxHEIGHTxWIDTH];
		float[] array2 = new float[Chunk.WIDTH * num * Chunk.WIDTH];
		float[] array3 = new float[Chunk.WIDTHTUNNEL * num2 * Chunk.WIDTHTUNNEL];
		float[] array4 = new float[Chunk.WIDTHTUNNEL * num2 * Chunk.WIDTHTUNNEL];
		int num3 = Mathf.RoundToInt(chunk.bounds.LeftBotBack.x);
		int num4 = Mathf.RoundToInt(chunk.bounds.LeftBotBack.y);
		int num5 = Mathf.RoundToInt(chunk.bounds.LeftBotBack.z);
		surfaceDensityNoise.fastNoiseSIMD.FillNoiseSet(array, num3, num4, num5, Chunk.WIDTH, Chunk.HEIGHT, Chunk.WIDTH);
		cheeseCaveNoise.fastNoiseSIMD.FillSampledNoiseSet(array2, num3, num4, num5, Chunk.WIDTH, num, Chunk.WIDTH, 4);
		tunnelNoise1.fastNoiseSIMD.FillSampledNoiseSet(array3, num3 - 1, num4 - 1, num5 - 1, Chunk.WIDTHTUNNEL, num2, Chunk.WIDTHTUNNEL, 2);
		tunnelNoise2.fastNoiseSIMD.FillSampledNoiseSet(array4, num3 - 1, num4 - 1, num5 - 1, Chunk.WIDTHTUNNEL, num2, Chunk.WIDTHTUNNEL, 2);
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.WIDTH; j++)
			{
				int num6 = i * Chunk.WIDTH + j;
				int num7 = Mathf.RoundToInt(chunk.heightMap[num6]);
				int num8 = 1 + Mathf.RoundToInt((chunk.noiseSetWhite1[i * Chunk.WIDTH + j] + 1f) * 2f);
				int num9 = BiomeManager.instance.GetbiomeIDFromValues(chunk.noiseSetT[i * Chunk.WIDTH + j], chunk.noiseSetH[i * Chunk.WIDTH + j]);
				chunk.biomeArray[i * Chunk.WIDTH + j] = num9;
				for (int k = 0; k < num8; k++)
				{
					chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + k * Chunk.WIDTH + j] = 23;
				}
				int num10 = Mathf.RoundToInt(maxSquachingHeight * chunk.noiseSetW2[num6]);
				for (int l = num8; l <= num7 - num10; l++)
				{
					if (TryCave(i, l, j, num7, array3, array4, array2, chunk.noiseSetW[i * Chunk.WIDTH + j], num, num2))
					{
						chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + l * Chunk.WIDTH + j] = 0;
					}
					else
					{
						chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + l * Chunk.WIDTH + j] = 22;
					}
				}
				for (int m = num7 - num10 + 1; m <= num7 + num10; m++)
				{
					float num11 = (float)(m - (num7 - num10)) / (float)(num10 * 2 + 1);
					int num12 = i * Chunk.HEIGHT * Chunk.WIDTH + m * Chunk.WIDTH + j;
					float num13 = array[num12];
					if (num11 < 0.5f)
					{
						num13 = Mathf.Lerp(1f, num13, num11 * 2f);
					}
					else if (num11 >= 0.5f)
					{
						num13 = Mathf.Lerp(num13, -0.1f, (num11 - 0.5f) * 2f);
					}
					if (num13 > 0f)
					{
						if (TryCave(i, m, j, num7, array3, array4, array2, chunk.noiseSetW[i * Chunk.WIDTH + j], num, num2))
						{
							chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + m * Chunk.WIDTH + j] = 0;
						}
						else
						{
							chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + m * Chunk.WIDTH + j] = 22;
						}
					}
					else if (m <= waterLevel)
					{
						chunk.voxels[num12] = 249;
					}
				}
				for (int num14 = num7 + num10; num14 >= 0; num14--)
				{
					if (chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num14 * Chunk.WIDTH + j] != 0)
					{
						num7 = num14;
						break;
					}
				}
				chunk.heightMap[num6] = num7;
				for (int num15 = num7; num15 >= 0; num15--)
				{
					if (chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num15 * Chunk.WIDTH + j] != 249)
					{
						chunk.groundHeight[num6] = num15;
						break;
					}
				}
				for (int n = num7 + 1; n <= waterLevel; n++)
				{
					if (chunk.groundHeight[num6] <= (float)waterLevel)
					{
						chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + n * Chunk.WIDTH + j] = 249;
						chunk.heightMap[num6] = n;
					}
				}
				byte b = 86;
				switch (num9)
				{
				case 0:
				case 1:
				case 2:
					b = 26;
					break;
				case 10:
					b = 25;
					break;
				}
				int num16 = num7;
				while (chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num16 * Chunk.WIDTH + j] == 0 || chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num16 * Chunk.WIDTH + j] == 249)
				{
					num16--;
				}
				if (num16 < waterLevel && b == 86)
				{
					b = 24;
				}
				chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num16 * Chunk.WIDTH + j] = b;
				if (b == 86)
				{
					b = 24;
				}
				for (int num17 = num16 - 1; num17 >= num16 - num8; num17--)
				{
					if (chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num17 * Chunk.WIDTH + j] == 22)
					{
						chunk.voxels[i * Chunk.HEIGHT * Chunk.WIDTH + num17 * Chunk.WIDTH + j] = b;
					}
				}
			}
		}
	}

	private bool TryCave(int x, int y, int z, int height, float[] noiseSetTunnel1, float[] noiseSetTunnel2, float[] noiseSetCheeseCave, float w, int highestY, int caveHighestY)
	{
		float num = TUNNEL_THRESHOLD;
		float num2 = height - y;
		if (num2 < 10f)
		{
			num = Mathf.Lerp(w, TUNNEL_THRESHOLD, (num2 - 1f) / 5f);
		}
		float num3 = noiseSetCheeseCave[x * highestY * Chunk.WIDTH + y * Chunk.WIDTH + z];
		if (num2 <= 20f)
		{
			num3 = ((!(num2 < 8f)) ? (num3 + Mathf.Lerp(0.2f, w, (num2 - 8f) / 12f)) : 0f);
		}
		if (num3 < CheeseCaveThreshold)
		{
			return true;
		}
		float num4 = noiseSetTunnel1[(x + 1) * caveHighestY * Chunk.WIDTHTUNNEL + (y + 1) * Chunk.WIDTHTUNNEL + z + 1];
		float num5 = num4 * num4;
		if (num5 >= num)
		{
			return false;
		}
		float num6 = noiseSetTunnel2[(x + 1) * caveHighestY * Chunk.WIDTHTUNNEL + (y + 1) * Chunk.WIDTHTUNNEL + z + 1];
		num5 += num6 * num6;
		if (num5 >= num)
		{
			return false;
		}
		Vector3 derivative = GetDerivative(x + 1, y + 1, z + 1, noiseSetTunnel1, caveHighestY);
		Vector3 derivative2 = GetDerivative(x + 1, y + 1, z + 1, noiseSetTunnel2, caveHighestY);
		float num7 = derivative.x * derivative.x + derivative.y * derivative.y + derivative.z * derivative.z;
		float num8 = derivative2.x * derivative2.x + derivative2.y * derivative2.y + derivative2.z * derivative2.z;
		float num9 = derivative.x * derivative2.x + derivative.y * derivative2.y + derivative.z * derivative2.z;
		float num10 = num9 * num9 / (num7 * num8);
		float num11 = num10 * num10 * (TUNNEL_THRESHOLD * CAVE_SHAPE_CLOSEOFF_SENSITIVITY);
		num5 += num11;
		if (num5 >= num)
		{
			return false;
		}
		return true;
	}

	private Vector3 GetDerivative(int x, int y, int z, float[] tunnelArray, int caveHighestY)
	{
		float num = tunnelArray[x * caveHighestY * Chunk.WIDTHTUNNEL + (y + 1) * Chunk.WIDTHTUNNEL + z];
		float num2 = tunnelArray[x * caveHighestY * Chunk.WIDTHTUNNEL + (y - 1) * Chunk.WIDTHTUNNEL + z];
		float num3 = tunnelArray[(x - 1) * caveHighestY * Chunk.WIDTHTUNNEL + y * Chunk.WIDTHTUNNEL + z];
		float num4 = tunnelArray[(x + 1) * caveHighestY * Chunk.WIDTHTUNNEL + y * Chunk.WIDTHTUNNEL + z];
		float num5 = tunnelArray[x * caveHighestY * Chunk.WIDTHTUNNEL + y * Chunk.WIDTHTUNNEL + z - 1];
		return new Vector3(z: (tunnelArray[x * caveHighestY * Chunk.WIDTHTUNNEL + y * Chunk.WIDTHTUNNEL + z + 1] - num5) / 2f, x: (num - num2) / 2f, y: (num4 - num3) / 2f);
	}

	[ContextMenu("InitializeFakeSplines")]
	private void InitializeFakeSplines()
	{
		float num = 2f / ((float)fakeSplineResolution - 1f);
		float num2 = -1f;
		for (int i = 0; i < fakeSplineResolution; i++)
		{
			fakeSplineC[i] = splineC.Evaluate(num2);
			fakeSplineE[i] = splineE.Evaluate(num2);
			fakeSplineR[i] = splineR.Evaluate(num2);
			fakeSplineM[i] = splineM.Evaluate(num2);
			fakeSplineW[i] = splineW.Evaluate(num2);
			fakeSplineW2[i] = splineW2.Evaluate(num2);
			num2 += num;
		}
		InitializeLightSplines();
	}

	[ContextMenu("LightSplines")]
	public void InitializeLightSplines()
	{
		sunLightValues = new byte[16];
		blockLightValues = new byte[16];
		for (int i = 0; i < 16; i++)
		{
			sunLightValues[i] = (byte)Mathf.RoundToInt(splineSunLight.Evaluate((float)i / 15f));
			blockLightValues[i] = (byte)Mathf.RoundToInt(splineBlockLight.Evaluate((float)i / 15f));
		}
	}

	public float SampleFakeSpline(float noiseValue, string splineName)
	{
		return splineName switch
		{
			"C" => fakeSplineC[Mathf.RoundToInt((noiseValue + 1f) * 1000f)], 
			"E" => fakeSplineE[Mathf.RoundToInt((noiseValue + 1f) * 1000f)], 
			"R" => fakeSplineR[Mathf.RoundToInt((noiseValue + 1f) * 1000f)], 
			"M" => fakeSplineM[Mathf.RoundToInt((noiseValue + 1f) * 1000f)], 
			"W" => fakeSplineW[Mathf.RoundToInt((noiseValue + 1f) * 1000f)], 
			"W2" => fakeSplineW2[Mathf.RoundToInt((noiseValue + 1f) * 1000f)], 
			_ => 0f, 
		};
	}

	public void StoreHighestXAndZRows(Chunk chunk)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			num = 0;
			for (int j = 0; j < Chunk.WIDTH; j++)
			{
				int num3 = (int)chunk.heightMap[j * Chunk.WIDTH + i];
				for (int num4 = Chunk.HEIGHT - 1; num4 > num3; num4--)
				{
					if (chunk.voxels[j * Chunk.HEIGHT * Chunk.WIDTH + num4 * Chunk.WIDTH + i] != 0)
					{
						num3 = num4;
						break;
					}
				}
				num = Mathf.Max(num, num3);
			}
			chunk.highestX[i] = num;
			if (num2 < num)
			{
				num2 = num;
			}
		}
		for (int k = 0; k < Chunk.WIDTH; k++)
		{
			num = 0;
			for (int l = 0; l < Chunk.WIDTH; l++)
			{
				int num5 = (int)chunk.heightMap[k * Chunk.WIDTH + l];
				for (int num6 = Chunk.HEIGHT - 1; num6 > num5; num6--)
				{
					if (chunk.voxels[k * Chunk.HEIGHT * Chunk.WIDTH + num6 * Chunk.WIDTH + l] != 0)
					{
						num5 = num6;
						chunk.heightMap[k * Chunk.WIDTH + l] = num6;
						break;
					}
				}
				num = Mathf.Max(num, num5);
			}
			chunk.highestZ[k] = num;
		}
		chunk.highestBlock = num2;
	}

	[ContextMenu("TestBitSave")]
	public void Testit()
	{
		int value = 18;
		int value2 = 350;
		int collumnData = 0;
		StorageManager.SaveIntInFirst16Bits(ref collumnData, value);
		StorageManager.SaveIntInLast16Bits(ref collumnData, value2);
		MonoBehaviour.print("cdata : " + collumnData);
		MonoBehaviour.print(StorageManager.ReadIntFromFirst16Bits(collumnData));
		MonoBehaviour.print(StorageManager.ReadIntFromLast16Bits(collumnData));
	}
}
