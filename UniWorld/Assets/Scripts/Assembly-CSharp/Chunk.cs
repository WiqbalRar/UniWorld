using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Chunk : PooledObject
{
	public struct Bounds
	{
		public Vector3Int chunkPosition;

		public Vector3Int LeftBotBack;

		public Vector3Int LeftBotFront;

		public Vector3Int LeftTopBack;

		public Vector3Int LeftTopFront;

		public Vector3Int RightBotBack;

		public Vector3Int RightBotFront;

		public Vector3Int RightTopBack;

		public Vector3Int RightTopFront;

		public Bounds(Vector3Int chunkPosition)
		{
			this.chunkPosition = chunkPosition;
			LeftBotBack = new Vector3Int(this.chunkPosition.x, 0, this.chunkPosition.z);
			LeftBotFront = new Vector3Int(this.chunkPosition.x, 0, this.chunkPosition.z + WIDTH);
			LeftTopBack = new Vector3Int(this.chunkPosition.x, HEIGHT, this.chunkPosition.z);
			LeftTopFront = new Vector3Int(this.chunkPosition.x, HEIGHT, this.chunkPosition.z + WIDTH);
			RightBotBack = new Vector3Int(this.chunkPosition.x + WIDTH, 0, this.chunkPosition.z);
			RightBotFront = new Vector3Int(this.chunkPosition.x + WIDTH, 0, this.chunkPosition.z + WIDTH);
			RightTopBack = new Vector3Int(this.chunkPosition.x + WIDTH, HEIGHT, this.chunkPosition.z);
			RightTopFront = new Vector3Int(this.chunkPosition.x + WIDTH, HEIGHT, this.chunkPosition.z + WIDTH);
		}
	}

	public struct Neighbours
	{
		public bool hasNeighbours;

		public Chunk leftBack;

		public Chunk back;

		public Chunk rightBack;

		public Chunk left;

		public Chunk right;

		public Chunk leftFront;

		public Chunk front;

		public Chunk rightFront;

		public Neighbours(Vector3Int chunkPosition)
		{
			hasNeighbours = true;
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(-WIDTH, 0, -WIDTH), out leftBack))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(0, 0, -WIDTH), out back))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(WIDTH, 0, -WIDTH), out rightBack))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(-WIDTH, 0, 0), out left))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(WIDTH, 0, 0), out right))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(-WIDTH, 0, WIDTH), out leftFront))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(0, 0, WIDTH), out front))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(WIDTH, 0, WIDTH), out rightFront))
			{
				hasNeighbours = false;
			}
		}

		public bool UpdateNeighbours(Vector3Int chunkPosition)
		{
			hasNeighbours = true;
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(-WIDTH, 0, -WIDTH), out leftBack))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(0, 0, -WIDTH), out back))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(WIDTH, 0, -WIDTH), out rightBack))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(-WIDTH, 0, 0), out left))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(WIDTH, 0, 0), out right))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(-WIDTH, 0, WIDTH), out leftFront))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(0, 0, WIDTH), out front))
			{
				hasNeighbours = false;
			}
			if (!ChunkManager.instance.activeChunks.TryGetValue(chunkPosition + new Vector3Int(WIDTH, 0, WIDTH), out rightFront))
			{
				hasNeighbours = false;
			}
			return hasNeighbours;
		}

		public bool AreNeighboursAvailableAndReady(int minimumGenerationStage)
		{
			if (leftBack.generationStage < minimumGenerationStage || leftBack.isRecycling || leftBack.canFinishRecycling)
			{
				return false;
			}
			if (back.generationStage < minimumGenerationStage || back.isRecycling || back.canFinishRecycling)
			{
				return false;
			}
			if (rightBack.generationStage < minimumGenerationStage || rightBack.isRecycling || rightBack.canFinishRecycling)
			{
				return false;
			}
			if (left.generationStage < minimumGenerationStage || left.isRecycling || left.canFinishRecycling)
			{
				return false;
			}
			if (right.generationStage < minimumGenerationStage || right.isRecycling || right.canFinishRecycling)
			{
				return false;
			}
			if (leftFront.generationStage < minimumGenerationStage || leftFront.isRecycling || leftFront.canFinishRecycling)
			{
				return false;
			}
			if (front.generationStage < minimumGenerationStage || front.isRecycling || front.canFinishRecycling)
			{
				return false;
			}
			if (rightFront.generationStage < minimumGenerationStage || rightFront.isRecycling || rightFront.canFinishRecycling)
			{
				return false;
			}
			return true;
		}

		public bool AreNeighboursPropagated()
		{
			if (!leftBack.lightHasBeenPropagated)
			{
				return false;
			}
			if (!back.lightHasBeenPropagated)
			{
				return false;
			}
			if (!rightBack.lightHasBeenPropagated)
			{
				return false;
			}
			if (!left.lightHasBeenPropagated)
			{
				return false;
			}
			if (!right.lightHasBeenPropagated)
			{
				return false;
			}
			if (!leftFront.lightHasBeenPropagated)
			{
				return false;
			}
			if (!front.lightHasBeenPropagated)
			{
				return false;
			}
			if (!rightFront.lightHasBeenPropagated)
			{
				return false;
			}
			return true;
		}

		public bool AreNeighboursSunLightInitialized()
		{
			if (!leftBack.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!back.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!rightBack.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!left.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!right.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!leftFront.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!front.sunLightHasBeenInitialized)
			{
				return false;
			}
			if (!rightFront.sunLightHasBeenInitialized)
			{
				return false;
			}
			return true;
		}

		public bool AreNeighboursNeighboured()
		{
			if (!leftBack.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!back.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!rightBack.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!left.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!right.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!leftFront.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!front.neighbours.hasNeighbours)
			{
				return false;
			}
			if (!rightFront.neighbours.hasNeighbours)
			{
				return false;
			}
			return true;
		}

		public void ForceNeighbourhood(Chunk chunk)
		{
			leftBack.neighbours.rightFront = chunk;
			leftBack.neighbours.front = left;
			leftBack.neighbours.right = back;
			back.neighbours.front = chunk;
			back.neighbours.left = leftBack;
			back.neighbours.right = rightBack;
			rightBack.neighbours.leftFront = chunk;
			rightBack.neighbours.left = back;
			rightBack.neighbours.front = right;
			left.neighbours.right = chunk;
			left.neighbours.back = leftBack;
			left.neighbours.front = leftFront;
			right.neighbours.left = chunk;
			right.neighbours.back = rightBack;
			right.neighbours.front = rightFront;
			leftFront.neighbours.rightBack = chunk;
			leftFront.neighbours.back = left;
			leftFront.neighbours.right = front;
			front.neighbours.back = chunk;
			front.neighbours.left = leftFront;
			front.neighbours.right = rightFront;
			rightFront.neighbours.leftBack = chunk;
			rightFront.neighbours.left = front;
			rightFront.neighbours.back = right;
		}

		public void StartUsingNeighbours(ref bool usingNeighbours)
		{
			leftBack.usedByNeighbours++;
			back.usedByNeighbours++;
			rightBack.usedByNeighbours++;
			left.usedByNeighbours++;
			right.usedByNeighbours++;
			leftFront.usedByNeighbours++;
			front.usedByNeighbours++;
			rightFront.usedByNeighbours++;
			usingNeighbours = true;
		}

		public void StopUsingNeighbours(ref bool usingNeighbours)
		{
			leftBack.usedByNeighbours--;
			back.usedByNeighbours--;
			rightBack.usedByNeighbours--;
			left.usedByNeighbours--;
			right.usedByNeighbours--;
			leftFront.usedByNeighbours--;
			front.usedByNeighbours--;
			rightFront.usedByNeighbours--;
			usingNeighbours = false;
		}
	}

	private int taskMinDelay = 20;

	private int taskMaxDelay = 100;

	[HideInInspector]
	public bool mustBeSaved;

	[HideInInspector]
	public bool modificationPending;

	[HideInInspector]
	public bool blockMeshRequireModification;

	[HideInInspector]
	public bool blockMeshHasBeenModified;

	public bool waterMeshRequireModification;

	public bool waterMeshHasBeenModified;

	[Header("Chunk status")]
	public bool isThreading;

	public bool isRecycling;

	public bool canFinishRecycling;

	[HideInInspector]
	public bool firstMeshGeneration = true;

	public int usedByNeighbours;

	public bool usingNeighbours;

	[HideInInspector]
	public Neighbours neighbours;

	[HideInInspector]
	public int[] leftBackDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] leftBackDecorationID = new byte[0];

	[HideInInspector]
	public int[] backDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] backDecorationID = new byte[0];

	[HideInInspector]
	public int[] rightBackDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] rightBackDecorationID = new byte[0];

	[HideInInspector]
	public int[] leftDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] leftDecorationID = new byte[0];

	[HideInInspector]
	public int[] rightDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] rightDecorationID = new byte[0];

	[HideInInspector]
	public int[] leftFrontDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] leftFrontDecorationID = new byte[0];

	[HideInInspector]
	public int[] frontDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] frontDecorationID = new byte[0];

	[HideInInspector]
	public int[] rightFrontDecorationIndex = new int[0];

	[HideInInspector]
	public byte[] rightFrontDecorationID = new byte[0];

	[HideInInspector]
	public List<int> leftBackDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> leftBackDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> backDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> backDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> rightBackDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> rightBackDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> leftDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> leftDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> rightDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> rightDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> leftFrontDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> leftFrontDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> frontDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> frontDecorationIDList = new List<byte>();

	[HideInInspector]
	public List<int> rightFrontDecorationIndexList = new List<int>();

	[HideInInspector]
	public List<byte> rightFrontDecorationIDList = new List<byte>();

	public static int WIDTH = 16;

	public static int HALFWIDTH = WIDTH / 2;

	public static int HEIGHT = 384;

	public static int WIDTHxHEIGHT = 6144;

	public static int WIDTHxWIDTH = 256;

	public static int WIDTHxHEIGHTxWIDTH = 98304;

	public static int WIDTHTUNNEL = 18;

	public static int HEIGHTTUNNEL = 386;

	[HideInInspector]
	public byte[] voxels = new byte[98304];

	public Dictionary<int, byte> blockOrientation = new Dictionary<int, byte>();

	public HashSet<int> plants = new HashSet<int>();

	public Bounds bounds;

	[HideInInspector]
	public float[] noiseSetC = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetE = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetR = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetM = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetW = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetW2 = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetT = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetH = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] noiseSetWhite0 = new float[WIDTHxWIDTH];

	public float[] noiseSetWhite1 = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] heightMap = new float[WIDTHxWIDTH];

	[HideInInspector]
	public float[] groundHeight = new float[WIDTHxWIDTH];

	[HideInInspector]
	public int[] highestX = new int[WIDTH];

	[HideInInspector]
	public int[] highestZ = new int[WIDTH];

	public int highestBlock;

	[HideInInspector]
	public int[] biomeArray = new int[WIDTHxWIDTH];

	[Header("Lighting")]
	public Texture3D lightTexture;

	[HideInInspector]
	public byte[] lightMap = new byte[WIDTHxHEIGHTxWIDTH];

	public Queue<LightManager.LightNode> blockLightBfsQueue = new Queue<LightManager.LightNode>();

	public Queue<LightManager.LightRemovalNode> blockLightRemovalBfsQueue = new Queue<LightManager.LightRemovalNode>();

	public Queue<LightManager.LightNode> sunLightBfsQueue = new Queue<LightManager.LightNode>();

	public Queue<LightManager.LightRemovalNode> sunLightRemovalBfsQueue = new Queue<LightManager.LightRemovalNode>();

	public Texture2D temperatureHumidityTexture;

	public bool hasTemperatureTexture;

	public bool lightmapHasBeenBaked;

	public bool sunLightHasBeenInitialized;

	public bool initialLightBakingRequired;

	public bool lightHasBeenPropagated;

	public bool pendingLightModifications;

	public bool pendingLightPropagation;

	public bool pendingLightRemoval;

	public bool isUpdatingLightMap;

	[Header("Mesh")]
	public MeshFilter myFilter;

	public MeshRenderer myRenderer;

	public MeshFilter myWaterFilter;

	public MeshRenderer myWaterRenderer;

	[Header("GenerationStage")]
	public int generationStage = -1;

	private Vector3[] vertices;

	private int[] triangles;

	private Vector3[] uvs;

	private Vector3[] waterVertices;

	private int[] waterTriangles;

	[Header("Water Data")]
	public HashSet<Vector3Int> waterSimulationSet = new HashSet<Vector3Int>();

	public HashSet<Vector3Int> newWaterSimulationSet = new HashSet<Vector3Int>();

	public HashSet<Vector3Int> updateSet = new HashSet<Vector3Int>();

	public HashSet<Vector3Int> newUpdateSet = new HashSet<Vector3Int>();

	public Dictionary<Vector3Int, byte> waterUpdateModifications = new Dictionary<Vector3Int, byte>();

	public override void Initialize(params object[] parameters)
	{
		if (generationStage == -1)
		{
			generationStage = 0;
			lightTexture = new Texture3D(WIDTH + 2, HEIGHT + 2, WIDTH + 2, TextureFormat.RG16, mipChain: false);
			myRenderer.material.SetTexture("_LightingTexture", lightTexture);
			temperatureHumidityTexture = new Texture2D(WIDTH + 2, WIDTH + 2, TextureFormat.RG16, mipChain: false);
			myRenderer.material.SetTexture("_TemperatureTexture", temperatureHumidityTexture);
			myRenderer.material.SetFloat("_LightMapIsBaked", 0f);
			myWaterRenderer.material.SetFloat("_LightMapIsBaked", 0f);
			myWaterRenderer.material.SetTexture("_LightingTexture", lightTexture);
		}
		base.transform.position = (Vector3Int)parameters[0] - WorldShifter.instance.offset;
		if (myFilter.mesh == null)
		{
			myFilter.mesh = new Mesh();
			myWaterFilter.mesh = new Mesh();
		}
		myFilter.mesh.indexFormat = IndexFormat.UInt32;
		myWaterFilter.mesh.indexFormat = IndexFormat.UInt32;
		pendingLightModifications = true;
		pendingLightPropagation = true;
		pendingLightRemoval = true;
		highestBlock = 0;
		bounds = new Bounds((Vector3Int)parameters[0]);
		neighbours = new Neighbours(bounds.chunkPosition);
	}

	public void UpdateLightTexture(ref NativeArray<byte> data)
	{
		for (int i = 0; i < WIDTH; i++)
		{
			for (int j = 0; j < HEIGHT; j++)
			{
				for (int k = 0; k < WIDTH; k++)
				{
					int num = k * HEIGHT * WIDTH + j * WIDTH + i;
					byte blockLight = LightManager.GetBlockLight(lightMap[num]);
					byte sunLight = LightManager.GetSunLight(lightMap[num]);
					int num2 = ((i + 1) * HEIGHTTUNNEL * WIDTHTUNNEL + (j + 1) * WIDTHTUNNEL + k + 1) * 2;
					byte b = voxels[num];
					if (blockLight == 0)
					{
						if (b == 0)
						{
							data[num2] = 5;
						}
						else
						{
							data[num2] = 2;
						}
					}
					else if (b >= 94 && b <= 99)
					{
						data[num2 + 1] = (byte)(NoiseManager.instance.blockLightValues[blockLight] / 3);
					}
					else
					{
						data[num2] = NoiseManager.instance.blockLightValues[blockLight];
					}
					if (sunLight == 0)
					{
						data[num2 + 1] = 0;
					}
					else if (b >= 94 && b <= 99)
					{
						data[num2 + 1] = (byte)(NoiseManager.instance.sunLightValues[sunLight] / 3);
					}
					else
					{
						data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
					}
				}
			}
		}
		for (int l = 0; l < HEIGHT; l++)
		{
			int num;
			byte blockLight;
			byte sunLight;
			int num2;
			byte b;
			for (int m = 0; m < WIDTH; m++)
			{
				num = 15 * HEIGHT * WIDTH + l * WIDTH + m;
				blockLight = LightManager.GetBlockLight(neighbours.left.lightMap[num]);
				sunLight = LightManager.GetSunLight(neighbours.left.lightMap[num]);
				num2 = ((m + 1) * HEIGHTTUNNEL * WIDTHTUNNEL + (l + 1) * WIDTHTUNNEL) * 2;
				b = neighbours.left.voxels[num];
				if (blockLight == 0)
				{
					if (b == 0)
					{
						data[num2] = 5;
					}
					else
					{
						data[num2] = 2;
					}
				}
				else
				{
					data[num2] = NoiseManager.instance.blockLightValues[blockLight];
				}
				if (sunLight == 0)
				{
					data[num2 + 1] = 0;
				}
				else
				{
					data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
				}
				num = l * WIDTH + m;
				blockLight = LightManager.GetBlockLight(neighbours.right.lightMap[num]);
				sunLight = LightManager.GetSunLight(neighbours.right.lightMap[num]);
				num2 = ((m + 1) * HEIGHTTUNNEL * WIDTHTUNNEL + (l + 1) * WIDTHTUNNEL + 17) * 2;
				b = neighbours.right.voxels[num];
				if (blockLight == 0)
				{
					if (b == 0)
					{
						data[num2] = 5;
					}
					else
					{
						data[num2] = 2;
					}
				}
				else
				{
					data[num2] = NoiseManager.instance.blockLightValues[blockLight];
				}
				if (sunLight == 0)
				{
					data[num2 + 1] = 0;
				}
				else
				{
					data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
				}
			}
			for (int n = 0; n < WIDTH; n++)
			{
				num = n * HEIGHT * WIDTH + l * WIDTH + 15;
				blockLight = LightManager.GetBlockLight(neighbours.back.lightMap[num]);
				sunLight = LightManager.GetSunLight(neighbours.back.lightMap[num]);
				num2 = ((l + 1) * WIDTHTUNNEL + n + 1) * 2;
				b = neighbours.back.voxels[num];
				if (blockLight == 0)
				{
					if (b == 0)
					{
						data[num2] = 5;
					}
					else
					{
						data[num2] = 2;
					}
				}
				else
				{
					data[num2] = NoiseManager.instance.blockLightValues[blockLight];
				}
				if (sunLight == 0)
				{
					data[num2 + 1] = 0;
				}
				else
				{
					data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
				}
				num = n * HEIGHT * WIDTH + l * WIDTH;
				blockLight = LightManager.GetBlockLight(neighbours.front.lightMap[num]);
				sunLight = LightManager.GetSunLight(neighbours.front.lightMap[num]);
				num2 = (17 * HEIGHTTUNNEL * WIDTHTUNNEL + (l + 1) * WIDTHTUNNEL + n + 1) * 2;
				b = neighbours.front.voxels[num];
				if (blockLight == 0)
				{
					if (b == 0)
					{
						data[num2] = 5;
					}
					else
					{
						data[num2] = 2;
					}
				}
				else
				{
					data[num2] = NoiseManager.instance.blockLightValues[blockLight];
				}
				if (sunLight == 0)
				{
					data[num2 + 1] = 0;
				}
				else
				{
					data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
				}
			}
			num = 15 * HEIGHT * WIDTH + l * WIDTH + 15;
			blockLight = LightManager.GetBlockLight(neighbours.leftBack.lightMap[num]);
			sunLight = LightManager.GetSunLight(neighbours.leftBack.lightMap[num]);
			num2 = (l + 1) * WIDTHTUNNEL * 2;
			b = neighbours.leftBack.voxels[num];
			if (blockLight == 0)
			{
				if (b == 0)
				{
					data[num2] = 5;
				}
				else
				{
					data[num2] = 2;
				}
			}
			else
			{
				data[num2] = NoiseManager.instance.blockLightValues[blockLight];
			}
			if (sunLight == 0)
			{
				data[num2 + 1] = 0;
			}
			else
			{
				data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
			}
			num = l * WIDTH + 15;
			blockLight = LightManager.GetBlockLight(neighbours.rightBack.lightMap[num]);
			sunLight = LightManager.GetSunLight(neighbours.rightBack.lightMap[num]);
			num2 = ((l + 1) * WIDTHTUNNEL + 17) * 2;
			b = neighbours.rightBack.voxels[num];
			if (blockLight == 0)
			{
				if (b == 0)
				{
					data[num2] = 5;
				}
				else
				{
					data[num2] = 2;
				}
			}
			else
			{
				data[num2] = NoiseManager.instance.blockLightValues[blockLight];
			}
			if (sunLight == 0)
			{
				data[num2 + 1] = 0;
			}
			else
			{
				data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
			}
			num = 15 * HEIGHT * WIDTH + l * WIDTH;
			blockLight = LightManager.GetBlockLight(neighbours.leftFront.lightMap[num]);
			sunLight = LightManager.GetSunLight(neighbours.leftFront.lightMap[num]);
			num2 = (17 * HEIGHTTUNNEL * WIDTHTUNNEL + (l + 1) * WIDTHTUNNEL) * 2;
			b = neighbours.leftFront.voxels[num];
			if (blockLight == 0)
			{
				if (b == 0)
				{
					data[num2] = 5;
				}
				else
				{
					data[num2] = 2;
				}
			}
			else
			{
				data[num2] = NoiseManager.instance.blockLightValues[blockLight];
			}
			if (sunLight == 0)
			{
				data[num2 + 1] = 0;
			}
			else
			{
				data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
			}
			num = l * WIDTH;
			blockLight = LightManager.GetBlockLight(neighbours.rightFront.lightMap[num]);
			sunLight = LightManager.GetSunLight(neighbours.rightFront.lightMap[num]);
			num2 = (17 * HEIGHTTUNNEL * WIDTHTUNNEL + (l + 1) * WIDTHTUNNEL + 17) * 2;
			b = neighbours.rightFront.voxels[num];
			if (blockLight == 0)
			{
				if (b == 0)
				{
					data[num2] = 5;
				}
				else
				{
					data[num2] = 2;
				}
			}
			else
			{
				data[num2] = NoiseManager.instance.blockLightValues[blockLight];
			}
			if (sunLight == 0)
			{
				data[num2 + 1] = 0;
			}
			else
			{
				data[num2 + 1] = NoiseManager.instance.sunLightValues[sunLight];
			}
		}
	}

	[ContextMenu("printqueuesLength")]
	private void testttt()
	{
		Debug.Log(sunLightRemovalBfsQueue.Count + " " + blockLightRemovalBfsQueue.Count + " " + sunLightBfsQueue.Count + " " + sunLightBfsQueue.Count + " ");
	}

	[ContextMenu("force texture")]
	private void forceit()
	{
		NativeArray<byte> data = lightTexture.GetPixelData<byte>(0);
		UpdateLightTexture(ref data);
		lightTexture.Apply();
	}

	public async void UpdateLightMap()
	{
		if (isRecycling || canFinishRecycling || isUpdatingLightMap || usedByNeighbours > 0 || !neighbours.UpdateNeighbours(bounds.chunkPosition) || !neighbours.AreNeighboursAvailableAndReady(3) || !neighbours.AreNeighboursNeighboured())
		{
			return;
		}
		neighbours.StartUsingNeighbours(ref usingNeighbours);
		if (sunLightBfsQueue.Count != 0 || blockLightBfsQueue.Count != 0)
		{
			pendingLightPropagation = true;
		}
		if (sunLightRemovalBfsQueue.Count != 0 && blockLightRemovalBfsQueue.Count != 0)
		{
			pendingLightRemoval = true;
		}
		if (pendingLightRemoval || pendingLightPropagation || !sunLightHasBeenInitialized)
		{
			isUpdatingLightMap = true;
			neighbours.ForceNeighbourhood(this);
			if (!sunLightHasBeenInitialized)
			{
				await Task.Run(delegate
				{
					LightManager.InitializeSunLight(this);
				});
			}
			if (neighbours.AreNeighboursSunLightInitialized())
			{
				await Task.Run(delegate
				{
					if (pendingLightRemoval)
					{
						LightManager.ProcessBlockLightRemoval(this);
						LightManager.ProcessSunLightRemoval(this);
					}
					if (pendingLightPropagation)
					{
						LightManager.ProcessBlockLightPropagation(this);
						LightManager.ProcessSunLightPropagation(this);
					}
					lightHasBeenPropagated = true;
				});
			}
			isUpdatingLightMap = false;
		}
		if ((pendingLightModifications && lightHasBeenPropagated && neighbours.AreNeighboursPropagated()) || (initialLightBakingRequired && !pendingLightPropagation && !pendingLightRemoval))
		{
			if (initialLightBakingRequired)
			{
				initialLightBakingRequired = false;
			}
			isUpdatingLightMap = true;
			NativeArray<byte> data = lightTexture.GetPixelData<byte>(0);
			await Task.Run(delegate
			{
				UpdateLightTexture(ref data);
			});
			lightTexture.Apply();
			pendingLightModifications = false;
			if (!lightmapHasBeenBaked)
			{
				lightmapHasBeenBaked = true;
				ChunkManager.instance.chunkLitCount++;
			}
			isUpdatingLightMap = false;
		}
		neighbours.StopUsingNeighbours(ref usingNeighbours);
	}

	private void UpdateTemperatureTexture()
	{
		NativeArray<byte> pixelData = temperatureHumidityTexture.GetPixelData<byte>(0);
		int num = 0;
		for (int i = 0; i < WIDTH + 2; i++)
		{
			for (int j = 0; j < WIDTH + 2; j++)
			{
				float num2;
				float num3;
				switch (j)
				{
				case 0:
					switch (i)
					{
					case 0:
						num2 = neighbours.leftBack.noiseSetT[15 * WIDTH + 15];
						num3 = neighbours.leftBack.noiseSetH[15 * WIDTH + 15];
						break;
					case 17:
						num2 = neighbours.leftFront.noiseSetT[15 * WIDTH];
						num3 = neighbours.leftFront.noiseSetH[15 * WIDTH];
						break;
					default:
						num2 = neighbours.left.noiseSetT[15 * WIDTH + (i - 1)];
						num3 = neighbours.left.noiseSetH[15 * WIDTH + (i - 1)];
						break;
					}
					break;
				case 17:
					switch (i)
					{
					case 0:
						num2 = neighbours.rightBack.noiseSetT[15];
						num3 = neighbours.rightBack.noiseSetH[15];
						break;
					case 17:
						num2 = neighbours.rightFront.noiseSetT[0];
						num3 = neighbours.rightFront.noiseSetH[0];
						break;
					default:
						num2 = neighbours.right.noiseSetT[i - 1];
						num3 = neighbours.right.noiseSetH[i - 1];
						break;
					}
					break;
				default:
					switch (i)
					{
					case 0:
						num2 = neighbours.back.noiseSetT[(j - 1) * WIDTH + 15];
						num3 = neighbours.back.noiseSetH[(j - 1) * WIDTH + 15];
						break;
					case 17:
						num2 = neighbours.front.noiseSetT[(j - 1) * WIDTH];
						num3 = neighbours.front.noiseSetH[(j - 1) * WIDTH];
						break;
					default:
						num2 = noiseSetT[(j - 1) * WIDTH + (i - 1)];
						num3 = noiseSetH[(j - 1) * WIDTH + (i - 1)];
						break;
					}
					break;
				}
				pixelData[num++] = (byte)((num2 + 1f) / 2f * 255f);
				pixelData[num++] = (byte)((num3 + 1f) / 2f * 255f);
			}
		}
		temperatureHumidityTexture.Apply();
	}

	public void ProcessWaterSimulation()
	{
		neighbours.UpdateNeighbours(bounds.chunkPosition);
		if (neighbours.hasNeighbours)
		{
			neighbours.StartUsingNeighbours(ref usingNeighbours);
			WaterSimulator.ProcessWaterSimulation(this);
			neighbours.StopUsingNeighbours(ref usingNeighbours);
		}
	}

	public void ProcessBlockUpdate()
	{
		neighbours.UpdateNeighbours(bounds.chunkPosition);
		if (neighbours.hasNeighbours)
		{
			neighbours.StartUsingNeighbours(ref usingNeighbours);
			WaterSimulator.ProcessUpdateSet(this);
			neighbours.StopUsingNeighbours(ref usingNeighbours);
		}
	}

	public async void GenerationStep1()
	{
		if (StorageManager.ExistsOnDisk(bounds.chunkPosition))
		{
			isThreading = true;
			await Task.Run(delegate
			{
				StorageManager.LoadChunk(this);
				NoiseManager.instance.FillImportantNoiseSet(Mathf.RoundToInt(bounds.LeftBotBack.x), Mathf.RoundToInt(bounds.LeftBotBack.y), Mathf.RoundToInt(bounds.LeftBotBack.z), this);
			});
			blockMeshRequireModification = true;
			waterMeshRequireModification = true;
			generationStage = 3;
			isThreading = false;
		}
		else
		{
			isThreading = true;
			await Task.Run(delegate
			{
				NoiseManager.instance.FillNoiseSets(Mathf.RoundToInt(bounds.LeftBotBack.x), Mathf.RoundToInt(bounds.LeftBotBack.y), Mathf.RoundToInt(bounds.LeftBotBack.z), this);
				NoiseManager.instance.ProcessSplines(this);
				NoiseManager.instance.GenerateSolidArray(this);
			});
			generationStage = 1;
			isThreading = false;
		}
	}

	public async void GenerationStep2()
	{
		neighbours.UpdateNeighbours(bounds.chunkPosition);
		if (neighbours.hasNeighbours)
		{
			neighbours.StartUsingNeighbours(ref usingNeighbours);
			isThreading = true;
			await Task.Run(delegate
			{
				DecorationManager.instance.Decorate(this);
				WaterSimulator.PrepareWaterSimulationSetOnChunkLoad(this);
			});
			blockMeshRequireModification = true;
			waterMeshRequireModification = true;
			generationStage = 2;
			isThreading = false;
			neighbours.StopUsingNeighbours(ref usingNeighbours);
		}
	}

	public async void GenerationStep3()
	{
		neighbours.UpdateNeighbours(bounds.chunkPosition);
		if (neighbours.hasNeighbours && neighbours.AreNeighboursAvailableAndReady(2))
		{
			neighbours.StartUsingNeighbours(ref usingNeighbours);
			isThreading = true;
			await Task.Run(delegate
			{
				AddNeighBoursToVoxelArray();
				NoiseManager.instance.StoreHighestXAndZRows(this);
			});
			generationStage = 3;
			isThreading = false;
			neighbours.StopUsingNeighbours(ref usingNeighbours);
		}
	}

	private void AddNeighBoursToVoxelArray()
	{
		bool flag = false;
		if (neighbours.leftBack.rightFrontDecorationID.Length == neighbours.leftBack.rightFrontDecorationIndex.Length)
		{
			for (int i = 0; i < neighbours.leftBack.rightFrontDecorationID.Length; i++)
			{
				byte b = neighbours.leftBack.rightFrontDecorationID[i];
				int num = neighbours.leftBack.rightFrontDecorationIndex[i];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("leftBack neighbour data incorrect, ID: " + neighbours.leftBack.rightFrontDecorationID.Length + " Index: " + neighbours.leftBack.rightFrontDecorationIndex.Length);
		}
		if (neighbours.back.frontDecorationID.Length == neighbours.back.frontDecorationIndex.Length)
		{
			for (int j = 0; j < neighbours.back.frontDecorationID.Length; j++)
			{
				byte b = neighbours.back.frontDecorationID[j];
				int num = neighbours.back.frontDecorationIndex[j];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("back neighbour data incorrect, ID: " + neighbours.back.frontDecorationID.Length + " Index: " + neighbours.back.frontDecorationIndex.Length);
		}
		if (neighbours.rightBack.leftFrontDecorationID.Length == neighbours.rightBack.leftFrontDecorationIndex.Length)
		{
			for (int k = 0; k < neighbours.rightBack.leftFrontDecorationID.Length; k++)
			{
				byte b = neighbours.rightBack.leftFrontDecorationID[k];
				int num = neighbours.rightBack.leftFrontDecorationIndex[k];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("rightBack neighbour data incorrect, ID: " + neighbours.rightBack.leftFrontDecorationID.Length + " Index: " + neighbours.rightBack.leftFrontDecorationIndex.Length);
		}
		if (neighbours.left.rightDecorationID.Length == neighbours.left.rightDecorationIndex.Length)
		{
			for (int l = 0; l < neighbours.left.rightDecorationID.Length; l++)
			{
				byte b = neighbours.left.rightDecorationID[l];
				int num = neighbours.left.rightDecorationIndex[l];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("left neighbour data incorrect, ID: " + neighbours.left.rightDecorationID.Length + " Index: " + neighbours.left.rightDecorationIndex.Length);
		}
		if (neighbours.right.leftDecorationID.Length == neighbours.right.leftDecorationIndex.Length)
		{
			for (int m = 0; m < neighbours.right.leftDecorationID.Length; m++)
			{
				byte b = neighbours.right.leftDecorationID[m];
				int num = neighbours.right.leftDecorationIndex[m];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("right neighbour data incorrect, ID: " + neighbours.right.leftDecorationID.Length + " Index: " + neighbours.right.leftDecorationIndex.Length);
		}
		if (neighbours.leftFront.rightBackDecorationID.Length == neighbours.leftFront.rightBackDecorationIndex.Length)
		{
			for (int n = 0; n < neighbours.leftFront.rightBackDecorationID.Length; n++)
			{
				byte b = neighbours.leftFront.rightBackDecorationID[n];
				int num = neighbours.leftFront.rightBackDecorationIndex[n];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("leftFront neighbour data incorrect, ID: " + neighbours.leftFront.rightBackDecorationID.Length + " Index: " + neighbours.leftFront.rightBackDecorationIndex.Length);
		}
		if (neighbours.front.backDecorationID.Length == neighbours.front.backDecorationIndex.Length)
		{
			for (int num3 = 0; num3 < neighbours.front.backDecorationID.Length; num3++)
			{
				byte b = neighbours.front.backDecorationID[num3];
				int num = neighbours.front.backDecorationIndex[num3];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("front neighbour data incorrect, ID: " + neighbours.front.backDecorationID.Length + " Index: " + neighbours.front.backDecorationIndex.Length);
		}
		if (neighbours.rightFront.leftBackDecorationID.Length == neighbours.rightFront.leftBackDecorationIndex.Length)
		{
			for (int num4 = 0; num4 < neighbours.rightFront.leftBackDecorationID.Length; num4++)
			{
				byte b = neighbours.rightFront.leftBackDecorationID[num4];
				int num = neighbours.rightFront.leftBackDecorationIndex[num4];
				flag = false;
				byte b2 = voxels[num];
				if (b2 == 0)
				{
					voxels[num] = b;
					flag = true;
					if (b >= 1 && b <= 20)
					{
						plants.Add(num);
					}
				}
				else if (b2 >= 94 && b2 <= 100 && b >= 22 && b <= 92)
				{
					voxels[num] = b;
					flag = true;
				}
				else if (b2 >= 1 && b2 <= 20)
				{
					if (b < 1 || b > 20)
					{
						plants.Remove(num);
					}
					voxels[num] = b;
					flag = true;
				}
				if (flag)
				{
					Vector3Int vector3Int = Precomputed.indexCoords[num];
					int num2 = vector3Int.x * WIDTH + vector3Int.z;
					if (b != 0 && (float)vector3Int.y > heightMap[num2])
					{
						heightMap[num2] = vector3Int.y;
					}
				}
			}
		}
		else
		{
			MonoBehaviour.print("rightFront neighbour data incorrect, ID: " + neighbours.rightFront.leftBackDecorationID.Length + " Index: " + neighbours.rightFront.leftBackDecorationIndex.Length);
		}
	}

	public async void GenerationStep4(bool delayTask)
	{
		int r = 0;
		if (delayTask && firstMeshGeneration)
		{
			r = UnityEngine.Random.Range(taskMinDelay, taskMaxDelay);
		}
		neighbours.UpdateNeighbours(bounds.chunkPosition);
		if (!neighbours.hasNeighbours)
		{
			return;
		}
		neighbours.StartUsingNeighbours(ref usingNeighbours);
		isThreading = true;
		await Task.Run(delegate
		{
			if (delayTask && firstMeshGeneration)
			{
				Thread.Sleep(r);
			}
			if (blockMeshRequireModification)
			{
				GreedyMesher.GenerateSolidSimpleMesh(voxels, ref vertices, ref triangles, ref uvs, neighbours, this);
				blockMeshRequireModification = false;
				blockMeshHasBeenModified = true;
			}
			if (waterMeshRequireModification)
			{
				GreedyMesher.GenerateWaterMesh(voxels, ref waterVertices, ref waterTriangles, this);
				waterMeshRequireModification = false;
				waterMeshHasBeenModified = true;
			}
		});
		if (firstMeshGeneration)
		{
			UpdateTemperatureTexture();
			firstMeshGeneration = false;
		}
		generationStage = 4;
		isThreading = false;
		neighbours.StopUsingNeighbours(ref usingNeighbours);
		ChunkManager.instance.chunkRenderCount++;
	}

	public void GenerationStep5()
	{
		if (blockMeshHasBeenModified)
		{
			myFilter.mesh.Clear();
			myFilter.mesh.vertices = vertices;
			myFilter.mesh.triangles = triangles;
			myFilter.mesh.SetUVs(0, uvs);
			myFilter.mesh.RecalculateNormals();
			blockMeshHasBeenModified = false;
		}
		if (waterMeshHasBeenModified)
		{
			myWaterFilter.mesh.Clear();
			myWaterFilter.mesh.vertices = waterVertices;
			myWaterFilter.mesh.triangles = waterTriangles;
			myWaterFilter.mesh.RecalculateNormals();
			waterMeshHasBeenModified = false;
		}
		generationStage = 5;
	}

	public async void PrepareToRecycle()
	{
		isRecycling = true;
		if (mustBeSaved)
		{
			isThreading = true;
			int r = UnityEngine.Random.Range(taskMinDelay, taskMaxDelay);
			await Task.Factory.StartNew(delegate
			{
				Thread.Sleep(r);
				StorageManager.SaveChunk(this);
			});
			isThreading = false;
			canFinishRecycling = true;
			isRecycling = false;
		}
		else
		{
			canFinishRecycling = true;
			isRecycling = false;
		}
	}

	protected override void OnRecycle()
	{
		firstMeshGeneration = true;
		generationStage = 0;
		canFinishRecycling = false;
		mustBeSaved = false;
		blockMeshRequireModification = false;
		blockMeshHasBeenModified = false;
		waterMeshRequireModification = false;
		waterMeshHasBeenModified = false;
		waterUpdateModifications.Clear();
		waterSimulationSet.Clear();
		newWaterSimulationSet.Clear();
		updateSet.Clear();
		newUpdateSet.Clear();
		blockLightBfsQueue.Clear();
		blockLightRemovalBfsQueue.Clear();
		sunLightBfsQueue.Clear();
		sunLightRemovalBfsQueue.Clear();
		hasTemperatureTexture = false;
		pendingLightModifications = false;
		lightmapHasBeenBaked = false;
		sunLightHasBeenInitialized = false;
		initialLightBakingRequired = false;
		lightHasBeenPropagated = false;
		pendingLightRemoval = false;
		pendingLightPropagation = false;
		isUpdatingLightMap = false;
		myRenderer.material.SetFloat("_LightMapIsBaked", 0f);
		myWaterRenderer.material.SetFloat("_LightMapIsBaked", 0f);
		Array.Clear(lightMap, 0, lightMap.Length);
		Array.Clear(voxels, 0, voxels.Length);
		Array.Clear(heightMap, 0, heightMap.Length);
		Array.Clear(groundHeight, 0, groundHeight.Length);
		blockOrientation.Clear();
		plants.Clear();
		Array.Clear(leftBackDecorationIndex, 0, leftBackDecorationIndex.Length);
		Array.Clear(leftBackDecorationID, 0, leftBackDecorationID.Length);
		Array.Clear(backDecorationIndex, 0, backDecorationIndex.Length);
		Array.Clear(backDecorationID, 0, backDecorationID.Length);
		Array.Clear(rightBackDecorationIndex, 0, rightBackDecorationIndex.Length);
		Array.Clear(rightBackDecorationID, 0, rightBackDecorationID.Length);
		Array.Clear(leftDecorationIndex, 0, leftDecorationIndex.Length);
		Array.Clear(leftDecorationID, 0, leftDecorationID.Length);
		Array.Clear(rightDecorationIndex, 0, rightDecorationIndex.Length);
		Array.Clear(rightDecorationID, 0, rightDecorationID.Length);
		Array.Clear(leftFrontDecorationIndex, 0, leftFrontDecorationIndex.Length);
		Array.Clear(leftFrontDecorationID, 0, leftFrontDecorationID.Length);
		Array.Clear(frontDecorationIndex, 0, frontDecorationIndex.Length);
		Array.Clear(rightFrontDecorationIndex, 0, rightFrontDecorationIndex.Length);
		Array.Clear(rightFrontDecorationID, 0, rightFrontDecorationID.Length);
		if (myFilter.mesh != null)
		{
			myFilter.mesh.Clear();
		}
		if (myWaterFilter.mesh != null)
		{
			myWaterFilter.mesh.Clear();
		}
	}

	private void OnDestroy()
	{
		if (mustBeSaved)
		{
			StorageManager.SaveChunk(this);
		}
	}
}
