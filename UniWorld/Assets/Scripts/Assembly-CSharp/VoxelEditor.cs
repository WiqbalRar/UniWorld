using System.Collections.Generic;
using UnityEngine;

public class VoxelEditor : MonoBehaviour
{
	private struct VoxelModification
	{
		public Chunk chunk;

		public List<int> voxelIndex;

		public List<Vector3Int> voxelPosition;

		public List<byte> voxelValue;

		public List<byte> voxelOrientation;

		public List<int> recycledVoxelIndex;

		public List<Vector3Int> recycledVoxelPosition;

		public List<byte> recycledVoxelValue;

		public List<byte> recycledVoxelOrientation;

		public VoxelModification(Chunk chunk, List<int> voxelIndex, List<Vector3Int> voxelPosition, List<byte> voxelValue, List<byte> voxelOrientation)
		{
			this = default(VoxelModification);
			this.chunk = chunk;
			this.voxelIndex = voxelIndex;
			this.voxelPosition = voxelPosition;
			this.voxelValue = voxelValue;
			this.voxelOrientation = voxelOrientation;
			recycledVoxelIndex = new List<int>();
			recycledVoxelPosition = new List<Vector3Int>();
			recycledVoxelValue = new List<byte>();
			recycledVoxelOrientation = new List<byte>();
		}

		public void RecycleElement(int index)
		{
			recycledVoxelIndex.Add(voxelIndex[index]);
			recycledVoxelPosition.Add(voxelPosition[index]);
			recycledVoxelValue.Add(voxelValue[index]);
			recycledVoxelOrientation.Add(voxelOrientation[index]);
		}

		public void UpdateFailedModifications()
		{
			voxelIndex.Clear();
			voxelIndex = recycledVoxelIndex;
			recycledVoxelIndex = new List<int>();
			recycledVoxelPosition.Clear();
			voxelPosition = recycledVoxelPosition;
			recycledVoxelPosition = new List<Vector3Int>();
			recycledVoxelValue.Clear();
			voxelValue = recycledVoxelValue;
			recycledVoxelValue = new List<byte>();
			recycledVoxelOrientation.Clear();
			voxelOrientation = recycledVoxelOrientation;
			recycledVoxelOrientation = new List<byte>();
		}
	}

	public static VoxelEditor instance;

	public LayerMask preventBlockPlacementMask;

	public Vector3 preventBlockPlacementHalfSize;

	private List<VoxelModification> pendingModifications = new List<VoxelModification>();

	private bool hasChainedModifications;

	private VoxelParticleManager myParticleManager;

	private FpsController playerController;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
		myParticleManager = Object.FindObjectOfType<VoxelParticleManager>();
	}

	private void Start()
	{
		playerController = Object.FindObjectOfType<FpsController>();
	}

	private void Update()
	{
		for (int num = pendingModifications.Count - 1; num >= 0; num--)
		{
			if (!pendingModifications[num].chunk.isThreading && !pendingModifications[num].chunk.isUpdatingLightMap)
			{
				if (!ChunkManager.instance.activeChunks.ContainsKey(pendingModifications[num].chunk.bounds.chunkPosition))
				{
					pendingModifications.RemoveAt(num);
				}
				else if (ApplyModification(num))
				{
					pendingModifications.RemoveAt(num);
				}
			}
		}
		if (!hasChainedModifications)
		{
			return;
		}
		for (int num2 = pendingModifications.Count - 1; num2 >= 0; num2--)
		{
			if (!pendingModifications[num2].chunk.isThreading && !pendingModifications[num2].chunk.isUpdatingLightMap)
			{
				if (!ChunkManager.instance.activeChunks.ContainsKey(pendingModifications[num2].chunk.bounds.chunkPosition))
				{
					pendingModifications.RemoveAt(num2);
				}
				else if (ApplyModification(num2))
				{
					pendingModifications.RemoveAt(num2);
				}
			}
		}
		hasChainedModifications = false;
	}

	public void FlagMeshForUpdate(Chunk chunk, byte oldVoxel, byte newVoxel, Vector3Int modificationPosition)
	{
		if (newVoxel >= 241 && newVoxel <= 249)
		{
			chunk.waterMeshRequireModification = true;
			chunk.generationStage = 3;
			if (oldVoxel >= 1 && oldVoxel <= 100)
			{
				chunk.blockMeshRequireModification = true;
			}
			if (modificationPosition.x == 0)
			{
				byte b = chunk.neighbours.left.voxels[15 * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.left.waterMeshRequireModification = true;
					chunk.neighbours.left.generationStage = 3;
				}
			}
			if (modificationPosition.x == 15)
			{
				byte b = chunk.neighbours.right.voxels[modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.right.waterMeshRequireModification = true;
					chunk.neighbours.right.generationStage = 3;
				}
			}
			if (modificationPosition.z == 0)
			{
				byte b = chunk.neighbours.back.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + 15];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.back.waterMeshRequireModification = true;
					chunk.neighbours.back.generationStage = 3;
				}
			}
			if (modificationPosition.z == 15)
			{
				byte b = chunk.neighbours.front.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.front.waterMeshRequireModification = true;
					chunk.neighbours.front.generationStage = 3;
				}
			}
		}
		else if ((newVoxel >= 1 && newVoxel <= 100) || (newVoxel >= 101 && newVoxel <= 126))
		{
			chunk.blockMeshRequireModification = true;
			chunk.generationStage = 3;
			if (oldVoxel >= 241 && oldVoxel <= 249)
			{
				chunk.waterMeshRequireModification = true;
			}
			if (modificationPosition.x == 0)
			{
				byte b = chunk.neighbours.left.voxels[15 * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.left.waterMeshRequireModification = true;
					chunk.neighbours.left.generationStage = 3;
				}
				if ((b >= 22 && b <= 100) || (newVoxel >= 101 && newVoxel <= 126))
				{
					chunk.neighbours.left.blockMeshRequireModification = true;
					chunk.neighbours.left.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[(modificationPosition.x - 1) * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
			if (modificationPosition.x == 15)
			{
				byte b = chunk.neighbours.right.voxels[modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.right.waterMeshRequireModification = true;
					chunk.neighbours.right.generationStage = 3;
				}
				if ((b >= 22 && b <= 100) || (newVoxel >= 101 && newVoxel <= 126))
				{
					chunk.neighbours.right.blockMeshRequireModification = true;
					chunk.neighbours.right.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[(modificationPosition.x + 1) * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
			if (modificationPosition.z == 0)
			{
				byte b = chunk.neighbours.back.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + 15];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.back.waterMeshRequireModification = true;
					chunk.neighbours.back.generationStage = 3;
				}
				if ((b >= 22 && b <= 100) || (newVoxel >= 101 && newVoxel <= 126))
				{
					chunk.neighbours.back.blockMeshRequireModification = true;
					chunk.neighbours.back.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z - 1];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
			if (modificationPosition.z == 15)
			{
				byte b = chunk.neighbours.front.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.front.waterMeshRequireModification = true;
					chunk.neighbours.front.generationStage = 3;
				}
				if ((b >= 22 && b <= 100) || (newVoxel >= 101 && newVoxel <= 126))
				{
					chunk.neighbours.front.blockMeshRequireModification = true;
					chunk.neighbours.front.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z + 1];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
			if (modificationPosition.y < Chunk.HEIGHT - 1)
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + (modificationPosition.y + 1) * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
			if (modificationPosition.y > 0)
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + (modificationPosition.y - 1) * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
		}
		else
		{
			if (newVoxel != 0)
			{
				return;
			}
			if ((oldVoxel >= 1 && oldVoxel <= 100) || (oldVoxel >= 101 && oldVoxel <= 126) || (oldVoxel >= 241 && oldVoxel <= 249))
			{
				chunk.blockMeshRequireModification = true;
				if (oldVoxel >= 241 && oldVoxel <= 249)
				{
					chunk.waterMeshRequireModification = true;
				}
			}
			else
			{
				if (oldVoxel < 241 || oldVoxel > 249)
				{
					return;
				}
				chunk.waterMeshRequireModification = true;
			}
			chunk.generationStage = 3;
			if (modificationPosition.x == 0)
			{
				byte b = chunk.neighbours.left.voxels[15 * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.left.waterMeshRequireModification = true;
					chunk.neighbours.left.generationStage = 3;
				}
				if (((oldVoxel >= 22 && oldVoxel <= 100) || (oldVoxel >= 101 && oldVoxel <= 126)) && ((b >= 22 && b <= 100) || (b >= 101 && b <= 126)))
				{
					chunk.neighbours.left.blockMeshRequireModification = true;
					chunk.neighbours.left.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[(modificationPosition.x - 1) * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
					chunk.generationStage = 3;
				}
			}
			if (modificationPosition.x == 15)
			{
				byte b = chunk.neighbours.right.voxels[modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.right.waterMeshRequireModification = true;
					chunk.neighbours.right.generationStage = 3;
				}
				if (((oldVoxel >= 22 && oldVoxel <= 100) || (oldVoxel >= 101 && oldVoxel <= 126)) && ((b >= 22 && b <= 100) || (b >= 101 && b <= 126)))
				{
					chunk.neighbours.right.blockMeshRequireModification = true;
					chunk.neighbours.right.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[(modificationPosition.x + 1) * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
					chunk.generationStage = 3;
				}
			}
			if (modificationPosition.z == 0)
			{
				byte b = chunk.neighbours.back.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + 15];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.back.waterMeshRequireModification = true;
					chunk.neighbours.back.generationStage = 3;
				}
				if (((oldVoxel >= 22 && oldVoxel <= 100) || (oldVoxel >= 101 && oldVoxel <= 126)) && ((b >= 22 && b <= 100) || (b >= 101 && b <= 126)))
				{
					chunk.neighbours.back.blockMeshRequireModification = true;
					chunk.neighbours.back.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z - 1];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
					chunk.generationStage = 3;
				}
			}
			if (modificationPosition.z == 15)
			{
				byte b = chunk.neighbours.front.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH];
				if (b >= 241 && b <= 249)
				{
					chunk.neighbours.front.waterMeshRequireModification = true;
					chunk.neighbours.front.generationStage = 3;
				}
				if (((oldVoxel >= 22 && oldVoxel <= 100) || (oldVoxel >= 101 && oldVoxel <= 126)) && ((b >= 22 && b <= 100) || (b >= 101 && b <= 126)))
				{
					chunk.neighbours.front.blockMeshRequireModification = true;
					chunk.neighbours.front.generationStage = 3;
				}
			}
			else
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + modificationPosition.y * Chunk.WIDTH + modificationPosition.z + 1];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
					chunk.generationStage = 3;
				}
			}
			if (modificationPosition.y < Chunk.HEIGHT - 1)
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + (modificationPosition.y + 1) * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
					chunk.generationStage = 3;
				}
			}
			if (modificationPosition.y > 0)
			{
				byte b = chunk.voxels[modificationPosition.x * Chunk.HEIGHT * Chunk.WIDTH + (modificationPosition.y - 1) * Chunk.WIDTH + modificationPosition.z];
				if (b >= 241 && b <= 249)
				{
					chunk.waterMeshRequireModification = true;
					chunk.generationStage = 3;
				}
			}
		}
	}

	public void BlockPlacementLogic(Chunk chunk, byte oldVoxel, byte newVoxel, Vector3Int modificationPosition, int modificationIndex, byte modificationOrientation)
	{
		byte b = 0;
		if ((newVoxel >= 101 && newVoxel <= 126) || (newVoxel >= 21 && newVoxel <= 21))
		{
			if (chunk.blockOrientation.ContainsKey(modificationIndex))
			{
				b = chunk.blockOrientation[modificationIndex];
				chunk.blockOrientation[modificationIndex] = modificationOrientation;
			}
			else
			{
				chunk.blockOrientation.Add(modificationIndex, modificationOrientation);
			}
		}
		else if (chunk.blockOrientation.ContainsKey(modificationIndex))
		{
			b = chunk.blockOrientation[modificationIndex];
			chunk.blockOrientation.Remove(modificationIndex);
		}
		if (newVoxel >= 1 && newVoxel <= 20)
		{
			if (!chunk.plants.Contains(modificationIndex))
			{
				chunk.plants.Add(modificationIndex);
			}
		}
		else if (oldVoxel >= 1 && oldVoxel <= 20 && (newVoxel < 1 || newVoxel > 20) && chunk.plants.Contains(modificationIndex))
		{
			chunk.plants.Remove(modificationIndex);
		}
		if (newVoxel != 0)
		{
			return;
		}
		if (oldVoxel < 241)
		{
			Vector3 position = modificationPosition + chunk.bounds.LeftBotBack - WorldShifter.instance.offset + Vector3.one / 2f;
			if (oldVoxel >= 1 && oldVoxel <= 20)
			{
				float num = chunk.noiseSetWhite1[modificationPosition.x * Chunk.WIDTH + modificationPosition.z];
				position += new Vector3(num * 0.3125f, 0f, num % 0.1f * 3.125f);
			}
			else if (oldVoxel >= 21 && oldVoxel <= 21)
			{
				position += VoxelBounds.torchTranslation[b];
			}
			myParticleManager.CreateParticle(position, oldVoxel, chunk.noiseSetT[modificationPosition.x * 16 + modificationPosition.z], chunk.noiseSetH[modificationPosition.x * 16 + modificationPosition.z], modificationPosition + chunk.bounds.LeftBotBack);
		}
		if (oldVoxel >= 22 && oldVoxel <= 126)
		{
			byte voxelID = 0;
			byte orientation = 0;
			Vector3Int vector3Int = modificationPosition + chunk.bounds.LeftBotBack;
			Vector3Int worldPosition = vector3Int - Vector3Int.right;
			VoxelTools.GetBlockinfo(worldPosition, ref voxelID, ref orientation);
			if (voxelID >= 21 && voxelID <= 21 && orientation == 1)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
			worldPosition = vector3Int + Vector3Int.right;
			VoxelTools.GetBlockinfo(worldPosition, ref voxelID, ref orientation);
			if (voxelID >= 21 && voxelID <= 21 && orientation == 3)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
			worldPosition = vector3Int - Vector3Int.up;
			VoxelTools.GetBlockinfo(worldPosition, ref voxelID, ref orientation);
			if (voxelID >= 21 && voxelID <= 21 && orientation == 4)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
			worldPosition = vector3Int + Vector3Int.up;
			VoxelTools.GetBlockinfo(worldPosition, ref voxelID, ref orientation);
			if (voxelID >= 21 && voxelID <= 21 && orientation == 5)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
			if (voxelID >= 1 && voxelID <= 20)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
			worldPosition = vector3Int - Vector3Int.forward;
			VoxelTools.GetBlockinfo(worldPosition, ref voxelID, ref orientation);
			if (voxelID >= 21 && voxelID <= 21 && orientation == 0)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
			worldPosition = vector3Int + Vector3Int.forward;
			VoxelTools.GetBlockinfo(worldPosition, ref voxelID, ref orientation);
			if (voxelID >= 21 && voxelID <= 21 && orientation == 2)
			{
				SingleModification(worldPosition, 0, 0);
				hasChainedModifications = true;
			}
		}
	}

	public bool BlockPlacementAllowanceCheck(Chunk chunk, byte oldVoxel, byte newVoxel, Vector3Int modificationPosition, int modificationIndex, byte modificationOrientation)
	{
		bool flag = false;
		if (oldVoxel == 0 || oldVoxel >= 241)
		{
			flag = true;
		}
		else if (oldVoxel >= 1 && oldVoxel <= 20 && (newVoxel >= 241 || (newVoxel >= 21 && newVoxel <= 126)))
		{
			flag = true;
		}
		if (flag)
		{
			if (newVoxel >= 1 && newVoxel <= 20)
			{
				byte voxelID = VoxelTools.GetVoxelID(modificationPosition + chunk.bounds.LeftBotBack - WorldShifter.instance.offset - Vector3Int.up);
				if (voxelID != 24 && voxelID != 86 && voxelID != 25)
				{
					flag = false;
				}
			}
			if (newVoxel >= 21 && newVoxel <= 21)
			{
				Vector3Int vector3Int = Vector3Int.zero;
				switch (modificationOrientation)
				{
				case 0:
					vector3Int = new Vector3Int(0, 0, 1);
					break;
				case 1:
					vector3Int = new Vector3Int(1, 0, 0);
					break;
				case 2:
					vector3Int = new Vector3Int(0, 0, -1);
					break;
				case 3:
					vector3Int = new Vector3Int(-1, 0, 0);
					break;
				case 4:
					vector3Int = new Vector3Int(0, 1, 0);
					break;
				case 5:
					vector3Int = new Vector3Int(0, -1, 0);
					break;
				}
				byte voxelID2 = 0;
				byte orientation = 0;
				VoxelTools.GetBlockinfo(modificationPosition + chunk.bounds.LeftBotBack + vector3Int, ref voxelID2, ref orientation);
				if (voxelID2 >= 22 && voxelID2 <= 100)
				{
					flag = true;
				}
				else if (voxelID2 >= 101 && voxelID2 <= 113)
				{
					switch (modificationOrientation)
					{
					case 0:
						flag = ((orientation == 2 || orientation == 6) ? true : false);
						break;
					case 1:
						flag = ((orientation == 3 || orientation == 7) ? true : false);
						break;
					case 2:
						flag = ((orientation == 0 || orientation == 4) ? true : false);
						break;
					case 3:
						flag = ((orientation == 1 || orientation == 5) ? true : false);
						break;
					case 4:
						flag = orientation <= 3;
						break;
					case 5:
						flag = orientation >= 4;
						break;
					}
				}
				else if (voxelID2 >= 114 && voxelID2 <= 126)
				{
					switch (modificationOrientation)
					{
					case 0:
						flag = orientation == 2;
						break;
					case 1:
						flag = orientation == 3;
						break;
					case 2:
						flag = orientation == 0;
						break;
					case 3:
						flag = orientation == 1;
						break;
					case 4:
						flag = orientation == 5;
						break;
					case 5:
						flag = orientation == 4;
						break;
					}
				}
				else
				{
					flag = false;
				}
			}
		}
		return flag;
	}

	public bool BlockPlacementCollisionCheck(byte oldVoxel, byte newVoxel, Vector3 boxCenter)
	{
		if (newVoxel >= 241 || (newVoxel >= 1 && newVoxel <= 20) || (newVoxel >= 21 && newVoxel <= 21) || !CollisionManager.CheckBox(playerController.transform.position, playerController.playerBounds, boxCenter))
		{
			return false;
		}
		return true;
	}

	private bool ApplyModification(int index)
	{
		bool flag = false;
		for (int num = pendingModifications[index].voxelIndex.Count - 1; num >= 0; num--)
		{
			if (pendingModifications[index].voxelIndex[num] > Chunk.WIDTHxHEIGHTxWIDTH)
			{
				MonoBehaviour.print("Better skip it as it is out of bounds");
			}
			else if (pendingModifications[index].voxelPosition[num].x == 0 && (pendingModifications[index].chunk.neighbours.left.isThreading || pendingModifications[index].chunk.neighbours.left.isUpdatingLightMap))
			{
				pendingModifications[index].RecycleElement(num);
				flag = true;
			}
			else if (pendingModifications[index].voxelPosition[num].x == 15 && (pendingModifications[index].chunk.neighbours.right.isThreading || pendingModifications[index].chunk.neighbours.right.isUpdatingLightMap))
			{
				pendingModifications[index].RecycleElement(num);
				flag = true;
			}
			else if (pendingModifications[index].voxelPosition[num].z == 0 && (pendingModifications[index].chunk.neighbours.back.isThreading || pendingModifications[index].chunk.neighbours.back.isUpdatingLightMap))
			{
				pendingModifications[index].RecycleElement(num);
				flag = true;
			}
			else if (pendingModifications[index].voxelPosition[num].z == 15 && (pendingModifications[index].chunk.neighbours.front.isThreading || pendingModifications[index].chunk.neighbours.front.isUpdatingLightMap))
			{
				pendingModifications[index].RecycleElement(num);
				flag = true;
			}
			else
			{
				byte b = pendingModifications[index].chunk.voxels[pendingModifications[index].voxelIndex[num]];
				byte b2 = pendingModifications[index].voxelValue[num];
				if (b2 > 0 && BlockPlacementAllowanceCheck(pendingModifications[index].chunk, b, b2, pendingModifications[index].voxelPosition[num], pendingModifications[index].voxelIndex[num], pendingModifications[index].voxelOrientation[num]))
				{
					if (!BlockPlacementCollisionCheck(b, b2, pendingModifications[index].chunk.bounds.LeftBotBack - WorldShifter.instance.offset + pendingModifications[index].voxelPosition[num] + Vector3.one / 2f))
					{
						pendingModifications[index].chunk.voxels[pendingModifications[index].voxelIndex[num]] = b2;
						BlockPlacementLogic(pendingModifications[index].chunk, b, b2, pendingModifications[index].voxelPosition[num], pendingModifications[index].voxelIndex[num], pendingModifications[index].voxelOrientation[num]);
						Vector3Int vector3Int = pendingModifications[index].voxelPosition[num];
						if (pendingModifications[index].chunk.highestX[vector3Int.z] < vector3Int.y)
						{
							pendingModifications[index].chunk.highestX[vector3Int.z] = vector3Int.y;
						}
						if (pendingModifications[index].chunk.highestZ[vector3Int.x] < vector3Int.y)
						{
							pendingModifications[index].chunk.highestZ[vector3Int.x] = vector3Int.y;
						}
						if (pendingModifications[index].chunk.highestBlock < vector3Int.y)
						{
							pendingModifications[index].chunk.highestBlock = vector3Int.y;
						}
						if (b2 >= 241 && b2 <= 249)
						{
							pendingModifications[index].chunk.waterSimulationSet.Add(pendingModifications[index].voxelPosition[num]);
							if ((b >= 85 && b <= 85) || (b >= 21 && b <= 21))
							{
								LightManager.RemoveBlockLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num]);
							}
						}
						else if ((b2 >= 85 && b2 <= 85) || (b2 >= 21 && b2 <= 21))
						{
							LightManager.PlaceBlockLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num], 15);
						}
						else if ((b2 >= 22 && b2 <= 92) || (b2 >= 101 && b2 <= 126))
						{
							LightManager.RemoveBlockLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num]);
							LightManager.RemoveSunLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num]);
						}
						else if (b2 >= 94 && b2 <= 100)
						{
							LightManager.RemoveSunLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num]);
						}
						FlagMeshForUpdate(pendingModifications[index].chunk, b, b2, pendingModifications[index].voxelPosition[num]);
					}
				}
				else if (b2 == 0)
				{
					pendingModifications[index].chunk.voxels[pendingModifications[index].voxelIndex[num]] = b2;
					BlockPlacementLogic(pendingModifications[index].chunk, b, b2, pendingModifications[index].voxelPosition[num], pendingModifications[index].voxelIndex[num], pendingModifications[index].voxelOrientation[num]);
					LightManager.RemoveBlockLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num]);
					LightManager.RemoveSunLight(pendingModifications[index].chunk, pendingModifications[index].voxelPosition[num]);
					FlagMeshForUpdate(pendingModifications[index].chunk, b, b2, pendingModifications[index].voxelPosition[num]);
				}
				pendingModifications[index].chunk.updateSet.Add(pendingModifications[index].voxelPosition[num]);
			}
		}
		pendingModifications[index].chunk.mustBeSaved = true;
		if (flag)
		{
			pendingModifications[index].UpdateFailedModifications();
			return false;
		}
		pendingModifications[index].chunk.modificationPending = false;
		return true;
	}

	public void SingleModification(Vector3Int worldPosition, byte newVoxelValue, byte voxelOrientation = 0)
	{
		if (worldPosition.y < Chunk.HEIGHT - 1 && worldPosition.y != 0)
		{
			Chunk chunk = null;
			int voxelIndex = 0;
			Vector3Int voxelPosition = Vector3Int.zero;
			VoxelTools.GetBlockinfo(worldPosition, ref chunk, ref voxelIndex, ref voxelPosition);
			if (!(chunk == null) && !chunk.isRecycling && !chunk.canFinishRecycling)
			{
				VoxelModification item = new VoxelModification(chunk, new List<int> { voxelIndex }, new List<Vector3Int> { voxelPosition }, new List<byte> { newVoxelValue }, new List<byte> { voxelOrientation });
				chunk.modificationPending = true;
				pendingModifications.Add(item);
			}
		}
	}
}
