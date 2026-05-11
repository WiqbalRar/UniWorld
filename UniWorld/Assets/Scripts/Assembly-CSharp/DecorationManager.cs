using UnityEngine;

public class DecorationManager : MonoBehaviour
{
	public static DecorationManager instance;

	private float threeThreshold = 0.99f;

	[Header("Desert")]
	private float cactus2 = 0.99f;

	private float cactus3 = 0.995f;

	private float desertDeadBush = 0.97f;

	private float desertAcacia = 0.9696f;

	[Header("Savanna")]
	private float savannaAcacia = 0.995f;

	private float savannaDeadBush = 0.01f;

	private float savannaGrass;

	[Header("LightForest")]
	public float lightForestBasicOak = 0.995f;

	public float lightForestFancyOak = 0.99f;

	public float lightForestBasicBirch = 0.989f;

	public float lightForestGrass = -0.5f;

	public float lightForestDandelion = -0.49f;

	public float lightForestPoppy = -0.48f;

	public float lightForestOxeyeDaisy = -0.475f;

	public float lightForestAllium = -0.47f;

	[Header("Forest")]
	private float forestbasicOak = 0.975f;

	private float forestfancyOak = 0.945f;

	private float forestGrass = -0.5f;

	private float forestDandelion = -0.49f;

	private float forestPoppy = -0.48f;

	private float forestOxeyeDaisy = -0.475f;

	private float forestAllium = -0.47f;

	[Header("Dense Forest")]
	private float denseForestDarkOak = 0.985f;

	private float denseForestbasicOak = 0.975f;

	private float denseForestfancyOak = 0.97f;

	private float denseForestGrass = -0.5f;

	private float denseForestAzureBluet = -0.47f;

	private float denseForestWhiteTulip = -0.43f;

	private float denseForestOxeyeDaisy = -0.4f;

	private float denseForestLilyOfTheValley = -0.37f;

	private float basicBirch = 0.98f;

	private float fancyBirch = 0.955f;

	private float birchForestGrass = -0.5f;

	private float birchForestDandelion = -0.49f;

	private float birchForestPoppy = -0.48f;

	private float birchForestOxeyeDaisy = -0.475f;

	private float birchForestAllium = -0.47f;

	private float birchForestBirchSapling = -0.45f;

	[Header("Plains")]
	private float plainsBasicOak = 0.999f;

	private float plainsFancyOak = 0.9985f;

	private float plainsDandelion = -0.98f;

	private float plainsForestPoppy = -0.96f;

	private float plainsForestAzureBluet = -0.94f;

	private float plainsRedTulip = -0.93f;

	private float plainsOrangeTulip = -0.92f;

	private float plainsWhiteTulip = -0.91f;

	private float plainsOxeyeDaisy = -0.9f;

	private float plainsCornFlower = -0.89f;

	private float plainsBlueOrchid = -0.88f;

	private float plainsLilyOfTheValley = -0.87f;

	private float plainsPoppy = -0.85f;

	private float plainsGrassPlant;

	[Header("IceDesert")]
	public float iceSpike = 0.995f;

	[Header("Ice Light Taiga")]
	private float iceLightTaigaSpruce0 = 0.999f;

	private float iceLightTaigaSpruce1 = 0.9985f;

	private float iceLightTaigaSpruce2 = 0.998f;

	private float iceLightTaigaSpruce3 = 0.9975f;

	private float iceLightTaigaSpruce4 = 0.997f;

	private float iceLightTaigaSpruceSapling = 0.9925f;

	private float iceLightTaigaGrassPlant = -0.995f;

	[Header("Ice Taiga")]
	private float iceTaigaSpruce0 = 0.99f;

	private float iceTaigaSpruce1 = 0.98f;

	private float iceTaigaSpruce2 = 0.97f;

	private float iceTaigaSpruce3 = 0.96f;

	private float iceTaigaSpruce4 = 0.955f;

	private float iceTaigaSpruceSapling = 0.945f;

	private float iceTaigaGrassPlant = -0.995f;

	[Header("Grass Taiga")]
	private float grassTaigaSpruce0 = 0.995f;

	private float grassTaigaSpruce1 = 0.99f;

	private float grassTaigaSpruce2 = 0.985f;

	private float grassTaigaSpruce3 = 0.98f;

	private float grassTaigaSpruce4 = 0.975f;

	private float grassTaigaBasicOak = 0.96f;

	private float grassTaigaSpruceSapling = 0.9f;

	private float grassTaigaGrassPlant = -0.5f;

	private float grassTaigaBlueOrchid = -0.49f;

	private float grassTaigaAllium = -0.48f;

	private float grassTaigaCornFlower = -0.47f;

	[Header("Jungle")]
	private float jungleTree = 0.975f;

	private float jungleBasicJungle = 0.965f;

	private float jungleBush = 0.93f;

	private float jungleBasicOak = 0.925f;

	private float jungleFancyOak = 0.92f;

	private float jungleGrassPlant = 0.2f;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
	}

	public void Decorate(Chunk chunk)
	{
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.WIDTH; j++)
			{
				int num = i * Chunk.WIDTH + j;
				int num2 = BiomeManager.instance.GetbiomeIDFromValues(chunk.noiseSetT[num], chunk.noiseSetH[num]);
				int num3 = (int)chunk.groundHeight[num] + 1;
				if (num3 > NoiseManager.instance.waterLevel)
				{
					switch (num2)
					{
					case 10:
						DesertDecoration(chunk, i, num3, j);
						break;
					case 8:
						SavannaDecoration(chunk, i, num3, j);
						break;
					case 5:
						LightForestDecoration(chunk, i, num3, j);
						break;
					case 6:
						ForestDecoration(chunk, i, num3, j);
						break;
					case 7:
						DenseForestDecoration(chunk, i, num3, j);
						break;
					case 3:
						PlainsDecoration(chunk, i, num3, j);
						break;
					case 0:
						IceDesertDecoration(chunk, i, num3, j);
						break;
					case 1:
						IceLightTaiga(chunk, i, num3, j);
						break;
					case 2:
						IceTaiga(chunk, i, num3, j);
						break;
					case 4:
						GrassTaiga(chunk, i, num3, j);
						break;
					case 11:
						BirchForestDecoration(chunk, i, num3, j);
						break;
					case 9:
						JungleDecoration(chunk, i, num3, j);
						break;
					}
				}
			}
		}
		ConvertNeighboursListsToArrays(chunk);
	}

	public void DesertDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num >= cactus2)
		{
			PlaceDecoration(chunk, x, y, z, 93);
			PlaceDecoration(chunk, x, y + 1, z, 93);
			if (num > cactus3)
			{
				PlaceDecoration(chunk, x, y + 2, z, 93);
			}
		}
		else if (num >= desertDeadBush)
		{
			PlaceDecoration(chunk, x, y, z, 9);
		}
		else if (num >= desertAcacia)
		{
			TreeManager.GenerateAcacia(chunk, x, y, z);
		}
	}

	public void SavannaDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > savannaAcacia)
		{
			TreeManager.GenerateAcacia(chunk, x, y, z);
		}
		else if (num < savannaGrass)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
		else if (num < savannaDeadBush)
		{
			PlaceDecoration(chunk, x, y, z, 9);
		}
	}

	public void LightForestDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > lightForestBasicOak)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 87, 94);
		}
		else if (num > lightForestFancyOak)
		{
			TreeManager.GenerateFancyTree(chunk, x, y, z, 87, 94);
		}
		else if (num > lightForestBasicBirch)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 89, 96);
		}
		else if (num < lightForestGrass)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
		else if (num < lightForestDandelion)
		{
			PlaceDecoration(chunk, x, y, z, 8);
		}
		else if (num < lightForestPoppy)
		{
			PlaceDecoration(chunk, x, y, z, 10);
		}
		else if (num < lightForestOxeyeDaisy)
		{
			PlaceDecoration(chunk, x, y, z, 18);
		}
		else if (num < lightForestAllium)
		{
			PlaceDecoration(chunk, x, y, z, 12);
		}
	}

	public void ForestDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > forestbasicOak)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 87, 94);
		}
		else if (num > forestfancyOak)
		{
			TreeManager.GenerateFancyTree(chunk, x, y, z, 87, 94);
		}
		else if (num < forestGrass)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
		else if (num < forestDandelion)
		{
			PlaceDecoration(chunk, x, y, z, 8);
		}
		else if (num < forestPoppy)
		{
			PlaceDecoration(chunk, x, y, z, 10);
		}
		else if (num < forestOxeyeDaisy)
		{
			PlaceDecoration(chunk, x, y, z, 18);
		}
		else if (num < forestAllium)
		{
			PlaceDecoration(chunk, x, y, z, 12);
		}
	}

	public void DenseForestDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > denseForestDarkOak)
		{
			TreeManager.GenerateDarkOak(chunk, x, y, z);
		}
		else if (num > denseForestbasicOak)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 87, 94);
		}
		else if (num > denseForestfancyOak)
		{
			TreeManager.GenerateFancyTree(chunk, x, y, z, 87, 94);
		}
		else if (num < denseForestGrass)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
		else if (num < denseForestAzureBluet)
		{
			PlaceDecoration(chunk, x, y, z, 13);
		}
		else if (num < denseForestWhiteTulip)
		{
			PlaceDecoration(chunk, x, y, z, 16);
		}
		else if (num < denseForestOxeyeDaisy)
		{
			PlaceDecoration(chunk, x, y, z, 18);
		}
		else if (num < denseForestLilyOfTheValley)
		{
			PlaceDecoration(chunk, x, y, z, 20);
		}
	}

	public void BirchForestDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > basicBirch)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 89, 96);
		}
		else if (num > fancyBirch)
		{
			TreeManager.GenerateFancyTree(chunk, x, y, z, 89, 96);
		}
		else if (num < birchForestGrass)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
		else if (num < birchForestDandelion)
		{
			PlaceDecoration(chunk, x, y, z, 8);
		}
		else if (num < birchForestPoppy)
		{
			PlaceDecoration(chunk, x, y, z, 10);
		}
		else if (num < birchForestOxeyeDaisy)
		{
			PlaceDecoration(chunk, x, y, z, 18);
		}
		else if (num < birchForestAllium)
		{
			PlaceDecoration(chunk, x, y, z, 12);
		}
		else if (num < birchForestBirchSapling)
		{
			PlaceDecoration(chunk, x, y, z, 4);
		}
	}

	public void PlainsDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > plainsBasicOak)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 87, 94);
		}
		else if (num > plainsFancyOak)
		{
			TreeManager.GenerateFancyTree(chunk, x, y, z, 87, 94);
		}
		else if (num < plainsDandelion)
		{
			PlaceDecoration(chunk, x, y, z, 8);
		}
		else if (num < plainsForestPoppy)
		{
			PlaceDecoration(chunk, x, y, z, 10);
		}
		else if (num < plainsForestAzureBluet)
		{
			PlaceDecoration(chunk, x, y, z, 13);
		}
		else if (num < plainsRedTulip)
		{
			PlaceDecoration(chunk, x, y, z, (byte)((chunk.noiseSetW[x * Chunk.WIDTH + z] > 0f) ? 17 : 14));
		}
		else if (num < plainsOrangeTulip)
		{
			PlaceDecoration(chunk, x, y, z, (byte)((chunk.noiseSetW[x * Chunk.WIDTH + z] > 0f) ? 17 : 15));
		}
		else if (num < plainsWhiteTulip)
		{
			PlaceDecoration(chunk, x, y, z, (byte)((chunk.noiseSetW[x * Chunk.WIDTH + z] > 0f) ? 17 : 16));
		}
		else if (num < plainsOxeyeDaisy)
		{
			PlaceDecoration(chunk, x, y, z, 18);
		}
		else if (num < plainsCornFlower)
		{
			PlaceDecoration(chunk, x, y, z, 19);
		}
		else if (num < plainsBlueOrchid)
		{
			PlaceDecoration(chunk, x, y, z, 11);
		}
		else if (num < plainsLilyOfTheValley)
		{
			PlaceDecoration(chunk, x, y, z, 20);
		}
		else if (num < plainsPoppy)
		{
			PlaceDecoration(chunk, x, y, z, 10);
		}
		else if (num < plainsGrassPlant)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
	}

	public void IceDesertDecoration(Chunk chunk, int x, int y, int z)
	{
		if (chunk.noiseSetWhite0[x * Chunk.WIDTH + z] > iceSpike)
		{
			TreeManager.GenerateIceSpike(chunk, x, y, z);
		}
	}

	public void IceLightTaiga(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > iceLightTaigaSpruce0)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 0);
		}
		else if (num > iceLightTaigaSpruce1)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 1);
		}
		else if (num > iceLightTaigaSpruce2)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 2);
		}
		else if (num > iceLightTaigaSpruce3)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 3);
		}
		else if (num > iceLightTaigaSpruce4)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 4);
		}
		else if (num > iceLightTaigaSpruceSapling)
		{
			PlaceDecoration(chunk, x, y, z, 5);
		}
		if (num < iceLightTaigaGrassPlant)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
	}

	public void IceTaiga(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > iceTaigaSpruce0)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 0);
		}
		else if (num > iceTaigaSpruce1)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 1);
		}
		else if (num > iceTaigaSpruce2)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 2);
		}
		else if (num > iceTaigaSpruce3)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 3);
		}
		else if (num > iceTaigaSpruce4)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 4);
		}
		else if (num > iceTaigaSpruceSapling)
		{
			PlaceDecoration(chunk, x, y, z, 5);
		}
		if (num < iceTaigaGrassPlant)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
	}

	public void GrassTaiga(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > grassTaigaSpruce0)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 0);
		}
		else if (num > grassTaigaSpruce1)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 1);
		}
		else if (num > grassTaigaSpruce2)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 2);
		}
		else if (num > grassTaigaSpruce3)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 3);
		}
		else if (num > grassTaigaSpruce4)
		{
			TreeManager.GenerateSpruce(chunk, x, y, z, 4);
		}
		else if (num > grassTaigaBasicOak)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 87, 94);
		}
		else if (num > grassTaigaSpruceSapling)
		{
			PlaceDecoration(chunk, x, y, z, 5);
		}
		if (num < grassTaigaGrassPlant)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
		else if (num < grassTaigaBlueOrchid)
		{
			PlaceDecoration(chunk, x, y, z, 11);
		}
		else if (num < grassTaigaAllium)
		{
			PlaceDecoration(chunk, x, y, z, 12);
		}
		else if (num < grassTaigaCornFlower)
		{
			PlaceDecoration(chunk, x, y, z, 19);
		}
	}

	public void JungleDecoration(Chunk chunk, int x, int y, int z)
	{
		float num = chunk.noiseSetWhite0[x * Chunk.WIDTH + z];
		if (num > jungleTree)
		{
			TreeManager.GenerateJungleTree(chunk, x, y, z);
		}
		else if (num > jungleBasicJungle)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 92, 99, 6, 4f);
		}
		else if (num > jungleBush)
		{
			TreeManager.GenerateBush(chunk, x, y, z, 92, 99);
		}
		else if (num > jungleBasicOak)
		{
			TreeManager.GenerateBasicTree(chunk, x, y, z, 87, 94);
		}
		else if (num > jungleFancyOak)
		{
			TreeManager.GenerateFancyTree(chunk, x, y, z, 92, 99);
		}
		else if (num < jungleGrassPlant)
		{
			PlaceDecoration(chunk, x, y, z, 1);
		}
	}

	public void PlaceDecoration(Chunk chunk, int x, int y, int z, byte voxelID, bool forceOverwrite = false)
	{
		if (y < 0 || y > Chunk.HEIGHT - 1)
		{
			return;
		}
		if (x < 0)
		{
			x += Chunk.WIDTH;
			if (z < 0)
			{
				z += Chunk.WIDTH;
				chunk.leftBackDecorationIDList.Add(voxelID);
				chunk.leftBackDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			}
			else if (z > 15)
			{
				z -= Chunk.WIDTH;
				chunk.leftFrontDecorationIDList.Add(voxelID);
				chunk.leftFrontDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			}
			else
			{
				chunk.leftDecorationIDList.Add(voxelID);
				chunk.leftDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			}
			return;
		}
		if (x > 15)
		{
			x -= Chunk.WIDTH;
			if (z < 0)
			{
				z += Chunk.WIDTH;
				chunk.rightBackDecorationIDList.Add(voxelID);
				chunk.rightBackDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			}
			else if (z > 15)
			{
				z -= Chunk.WIDTH;
				chunk.rightFrontDecorationIDList.Add(voxelID);
				chunk.rightFrontDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			}
			else
			{
				chunk.rightDecorationIDList.Add(voxelID);
				chunk.rightDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			}
			return;
		}
		if (z < 0)
		{
			z += Chunk.WIDTH;
			chunk.backDecorationIDList.Add(voxelID);
			chunk.backDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			return;
		}
		if (z > 15)
		{
			z -= Chunk.WIDTH;
			chunk.frontDecorationIDList.Add(voxelID);
			chunk.frontDecorationIndexList.Add(x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z);
			return;
		}
		if ((float)y > chunk.heightMap[x * Chunk.WIDTH + z])
		{
			chunk.heightMap[x * Chunk.WIDTH + z] = y;
		}
		int num = x * Chunk.HEIGHT * Chunk.WIDTH + y * Chunk.WIDTH + z;
		if (forceOverwrite)
		{
			chunk.voxels[num] = voxelID;
			return;
		}
		byte b = chunk.voxels[num];
		switch (b)
		{
		case 0:
			chunk.voxels[num] = voxelID;
			if (voxelID >= 1 && voxelID <= 20)
			{
				chunk.plants.Add(num);
			}
			return;
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
		case 99:
		case 100:
			if (voxelID >= 22 && voxelID <= 92)
			{
				chunk.voxels[num] = voxelID;
				return;
			}
			break;
		}
		if (b >= 1 && b <= 20)
		{
			if (voxelID < 1 || voxelID > 20)
			{
				chunk.plants.Remove(num);
			}
			chunk.voxels[num] = voxelID;
		}
	}

	private void ConvertNeighboursListsToArrays(Chunk chunk)
	{
		chunk.leftBackDecorationIndex = chunk.leftBackDecorationIndexList.ToArray();
		chunk.leftBackDecorationID = chunk.leftBackDecorationIDList.ToArray();
		chunk.backDecorationIndex = chunk.backDecorationIndexList.ToArray();
		chunk.backDecorationID = chunk.backDecorationIDList.ToArray();
		chunk.rightBackDecorationIndex = chunk.rightBackDecorationIndexList.ToArray();
		chunk.rightBackDecorationID = chunk.rightBackDecorationIDList.ToArray();
		chunk.leftDecorationIndex = chunk.leftDecorationIndexList.ToArray();
		chunk.leftDecorationID = chunk.leftDecorationIDList.ToArray();
		chunk.rightDecorationIndex = chunk.rightDecorationIndexList.ToArray();
		chunk.rightDecorationID = chunk.rightDecorationIDList.ToArray();
		chunk.leftFrontDecorationIndex = chunk.leftFrontDecorationIndexList.ToArray();
		chunk.leftFrontDecorationID = chunk.leftFrontDecorationIDList.ToArray();
		chunk.frontDecorationIndex = chunk.frontDecorationIndexList.ToArray();
		chunk.frontDecorationID = chunk.frontDecorationIDList.ToArray();
		chunk.rightFrontDecorationIndex = chunk.rightFrontDecorationIndexList.ToArray();
		chunk.rightFrontDecorationID = chunk.rightFrontDecorationIDList.ToArray();
		chunk.leftBackDecorationIndexList.Clear();
		chunk.leftBackDecorationIDList.Clear();
		chunk.backDecorationIndexList.Clear();
		chunk.backDecorationIDList.Clear();
		chunk.rightBackDecorationIndexList.Clear();
		chunk.rightBackDecorationIDList.Clear();
		chunk.leftDecorationIndexList.Clear();
		chunk.leftDecorationIDList.Clear();
		chunk.rightDecorationIndexList.Clear();
		chunk.rightDecorationIDList.Clear();
		chunk.leftFrontDecorationIndexList.Clear();
		chunk.leftFrontDecorationIDList.Clear();
		chunk.frontDecorationIndexList.Clear();
		chunk.frontDecorationIDList.Clear();
		chunk.rightFrontDecorationIndexList.Clear();
		chunk.rightFrontDecorationIDList.Clear();
	}
}
