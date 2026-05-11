using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private struct TargetBlock
	{
		public byte voxelID;

		public Vector3 truePosition;

		public Vector3Int position;

		public Vector3Int normal;

		public Vector3Int trueNormal;

		public byte orientation;

		public TargetBlock(byte voxelID, Vector3Int position, Vector3Int normal, Vector3 truePosition, byte orientation)
		{
			this.voxelID = voxelID;
			this.truePosition = truePosition;
			this.position = position;
			if (Mathf.Abs(normal.x) + Mathf.Abs(normal.y) + Mathf.Abs(normal.z) > 1)
			{
				if (normal.x != 0)
				{
					this.normal = new Vector3Int(normal.x, 0, 0);
				}
				else
				{
					this.normal = new Vector3Int(normal.y, 0, 0);
				}
			}
			else
			{
				this.normal = normal;
			}
			trueNormal = normal;
			this.orientation = orientation;
		}
	}

	public static Player instance;

	public Material AlphaBlendMat;

	public LayerMask lookMask;

	public LayerMask mineMask;

	public GameObject outlineBlock;

	public Transform playerPositionTransform;

	private FpsController myFpsController;

	public int selectedItem;

	public int minSelectedItem;

	public int maxSelectedItem;

	public byte selectedItemByte;

	private bool rapidFire;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		StorageManager.LoadPlayerData(out var playerPosition, out var playerRotation, out var verticalVelocity, out var worldShiftOffset);
		base.transform.position = playerPosition;
		base.transform.rotation = playerRotation;
		myFpsController = GetComponent<FpsController>();
		myFpsController.yVelocity = verticalVelocity;
		WorldShifter.instance.offset = worldShiftOffset;
		if (playerPosition.y != 0f)
		{
			myFpsController.positionInitialized = true;
		}
		selectedItem = 0;
	}

	private void Update()
	{
		if (ChunkManager.instance.drawChunkBorders)
		{
			VoxelTools.DrawChunkBorders(base.transform.position);
		}
		if (Time.timeScale == 0f || !myFpsController.canMove || PlayerInventory.instance.isOpen)
		{
			if (PlayerInventory.instance.isOpen && Time.timeScale != 0f)
			{
				PlayerBlockSelection();
			}
			return;
		}
		if (ActionManager.GetActionDown("RapidFire"))
		{
			rapidFire = !rapidFire;
		}
		PlayerBlockSelection();
		PlayerLookRay();
		UnderWaterDetection();
	}

	private void UnderWaterDetection()
	{
		byte voxelID = VoxelTools.GetVoxelID(VoxelTools.FloorPositionToInt(Camera.main.transform.position));
		int value = 0;
		if (voxelID >= 241 && voxelID <= 249)
		{
			byte voxelID2 = VoxelTools.GetVoxelID(VoxelTools.FloorPositionToInt(Camera.main.transform.position + Vector3Int.up));
			if (voxelID2 >= 241 && voxelID2 <= 249)
			{
				value = 1;
			}
			else if (Mathf.Floor(Camera.main.transform.position.y) + 0.1125f * ((float)(int)voxelID - 100f) >= Camera.main.transform.position.y)
			{
				value = 1;
			}
		}
		AlphaBlendMat.SetInt("_CamUnderWater", value);
	}

	private void OnDestroy()
	{
		StorageManager.SavePlayerData(base.transform.position, base.transform.rotation, GetComponent<FpsController>().yVelocity, WorldShifter.instance.offset);
	}

	private void PlayerLookRay()
	{
		TargetBlock targetedBlock;
		bool num = VoxelRaycast(Camera.main.transform.position, Camera.main.transform.forward, 300f, out targetedBlock);
		DebugScreen.instance.isLookingBlock = true;
		if (!num)
		{
			return;
		}
		OutlineMeshGenerator.instance.DrawOutline(targetedBlock.voxelID, targetedBlock.position + Vector3.one / 2f, Camera.main.transform.position, targetedBlock.orientation, targetedBlock.position);
		DebugScreen.instance.targetedBlockPos = targetedBlock.position;
		DebugScreen.instance.targetedBlockID = targetedBlock.voxelID;
		selectedItemByte = PlayerInventory.instance.GetSelectedItem();
		if (!rapidFire)
		{
			if (ActionManager.GetActionDown("PrimaryAction"))
			{
				VoxelEditor.instance.SingleModification(targetedBlock.position + WorldShifter.instance.offset, 0, 0);
			}
			if (ActionManager.GetActionDown("SecondaryAction") && selectedItemByte != 0)
			{
				PlaceBlock(targetedBlock, selectedItemByte);
			}
			if (ActionManager.GetActionDown("TertiaryAction"))
			{
				PlaceBlock(targetedBlock, 249);
			}
		}
		else
		{
			if (ActionManager.GetAction("PrimaryAction"))
			{
				VoxelEditor.instance.SingleModification(targetedBlock.position + WorldShifter.instance.offset, 0, 0);
			}
			if (ActionManager.GetAction("SecondaryAction") && selectedItemByte != 0)
			{
				PlaceBlock(targetedBlock, selectedItemByte);
			}
			if (ActionManager.GetAction("TertiaryAction"))
			{
				PlaceBlock(targetedBlock, 249);
			}
		}
	}

	private void PlaceBlock(TargetBlock targetedBlock, byte newVoxelValue)
	{
		if (targetedBlock.voxelID == 1)
		{
			targetedBlock.position -= targetedBlock.normal;
		}
		if (newVoxelValue >= 21 && newVoxelValue <= 21)
		{
			VoxelEditor.instance.SingleModification(targetedBlock.position + WorldShifter.instance.offset + targetedBlock.normal, newVoxelValue, GetTorchOrientation(targetedBlock));
		}
		else if (newVoxelValue >= 101 && newVoxelValue <= 113)
		{
			VoxelEditor.instance.SingleModification(targetedBlock.position + WorldShifter.instance.offset + targetedBlock.normal, newVoxelValue, GetStairOrientation(targetedBlock));
		}
		else if (newVoxelValue >= 114 && newVoxelValue <= 126)
		{
			VoxelEditor.instance.SingleModification(targetedBlock.position + WorldShifter.instance.offset + targetedBlock.normal, newVoxelValue, GetSlabOrientation(targetedBlock));
		}
		else
		{
			VoxelEditor.instance.SingleModification(targetedBlock.position + WorldShifter.instance.offset + targetedBlock.normal, newVoxelValue, 0);
		}
	}

	private byte GetStairOrientation(TargetBlock targetBlock)
	{
		float num = Vector2.SignedAngle(to: new Vector2(targetBlock.truePosition.x - Camera.main.transform.position.x, targetBlock.truePosition.z - Camera.main.transform.position.z), from: new Vector2(-1f, 1f));
		byte b = (byte)((num <= -90f) ? 1 : ((!(num <= 0f)) ? ((!(num <= 90f)) ? 2 : 3) : 0));
		if (targetBlock.normal.y < 0)
		{
			b += 4;
		}
		else if (targetBlock.normal.y != 1 && BetterMod(targetBlock.truePosition.y, 1f) > 0.5f)
		{
			b += 4;
		}
		return b;
	}

	private byte GetSlabOrientation(TargetBlock targetBlock)
	{
		float num = 0.2f;
		float num2 = 0.8f;
		if (targetBlock.normal.z == -1)
		{
			if (BetterMod(targetBlock.truePosition.x, 1f) >= num && BetterMod(targetBlock.truePosition.x, 1f) <= num2 && BetterMod(targetBlock.truePosition.y, 1f) >= num && BetterMod(targetBlock.truePosition.y, 1f) <= num2)
			{
				return 0;
			}
			float num3 = Vector2.SignedAngle(new Vector2(-1f, 1f), new Vector2(BetterMod(targetBlock.truePosition.x, 1f) - 0.5f, BetterMod(targetBlock.truePosition.y, 1f) - 0.5f));
			if (num3 > 90f)
			{
				return 5;
			}
			if (num3 > 0f)
			{
				return 3;
			}
			if (num3 > -90f)
			{
				return 4;
			}
			return 1;
		}
		if (targetBlock.normal.x == -1)
		{
			if (BetterMod(targetBlock.truePosition.z, 1f) >= num && BetterMod(targetBlock.truePosition.z, 1f) <= num2 && BetterMod(targetBlock.truePosition.y, 1f) >= num && BetterMod(targetBlock.truePosition.y, 1f) <= num2)
			{
				return 1;
			}
			float num4 = Vector2.SignedAngle(new Vector2(BetterMod(targetBlock.truePosition.z, 1f) - 0.5f, BetterMod(targetBlock.truePosition.y, 1f) - 0.5f), new Vector2(1f, 1f));
			if (num4 > 90f)
			{
				return 5;
			}
			if (num4 > 0f)
			{
				return 0;
			}
			if (num4 > -90f)
			{
				return 4;
			}
			return 2;
		}
		if (targetBlock.normal.z == 1)
		{
			if (BetterMod(targetBlock.truePosition.x, 1f) >= num && BetterMod(targetBlock.truePosition.x, 1f) <= num2 && BetterMod(targetBlock.truePosition.y, 1f) >= num && BetterMod(targetBlock.truePosition.y, 1f) <= num2)
			{
				return 2;
			}
			float num5 = Vector2.SignedAngle(new Vector2(BetterMod(targetBlock.truePosition.x, 1f) - 0.5f, BetterMod(targetBlock.truePosition.y, 1f) - 0.5f), new Vector2(1f, 1f));
			if (num5 > 90f)
			{
				return 5;
			}
			if (num5 > 0f)
			{
				return 1;
			}
			if (num5 > -90f)
			{
				return 4;
			}
			return 3;
		}
		if (targetBlock.normal.x == 1)
		{
			if (BetterMod(targetBlock.truePosition.z, 1f) >= num && BetterMod(targetBlock.truePosition.z, 1f) <= num2 && BetterMod(targetBlock.truePosition.y, 1f) >= num && BetterMod(targetBlock.truePosition.y, 1f) <= num2)
			{
				return 3;
			}
			float num6 = Vector2.SignedAngle(new Vector2(-1f, 1f), new Vector2(BetterMod(targetBlock.truePosition.z, 1f) - num2, BetterMod(targetBlock.truePosition.y, 1f) - num2));
			if (num6 > 90f)
			{
				return 5;
			}
			if (num6 > 0f)
			{
				return 2;
			}
			if (num6 > -90f)
			{
				return 4;
			}
			return 0;
		}
		if (targetBlock.normal.y == -1)
		{
			if (BetterMod(targetBlock.truePosition.x, 1f) >= num && BetterMod(targetBlock.truePosition.x, 1f) <= num2 && BetterMod(targetBlock.truePosition.z, 1f) >= num && BetterMod(targetBlock.truePosition.z, 1f) <= num2)
			{
				return 4;
			}
			float num7 = Vector2.SignedAngle(new Vector2(-1f, 1f), new Vector2(BetterMod(targetBlock.truePosition.x, 1f) - 0.5f, BetterMod(targetBlock.truePosition.z, 1f) - 0.5f));
			if (num7 > 90f)
			{
				return 2;
			}
			if (num7 > 0f)
			{
				return 3;
			}
			if (num7 > -90f)
			{
				return 0;
			}
			return 1;
		}
		if (BetterMod(targetBlock.truePosition.x, 1f) >= num && BetterMod(targetBlock.truePosition.x, 1f) <= num2 && BetterMod(targetBlock.truePosition.z, 1f) >= num && BetterMod(targetBlock.truePosition.z, 1f) <= num2)
		{
			return 5;
		}
		float num8 = Vector2.SignedAngle(new Vector2(-1f, 1f), new Vector2(BetterMod(targetBlock.truePosition.x, 1f) - 0.5f, BetterMod(targetBlock.truePosition.z, 1f) - 0.5f));
		if (num8 > 90f)
		{
			return 2;
		}
		if (num8 > 0f)
		{
			return 3;
		}
		if (num8 > -90f)
		{
			return 0;
		}
		return 1;
	}

	private byte GetTorchOrientation(TargetBlock targetBlock)
	{
		if (targetBlock.normal.z == -1)
		{
			return 0;
		}
		if (targetBlock.normal.x == -1)
		{
			return 1;
		}
		if (targetBlock.normal.z == 1)
		{
			return 2;
		}
		if (targetBlock.normal.x == 1)
		{
			return 3;
		}
		if (targetBlock.normal.y == -1)
		{
			return 4;
		}
		return 5;
	}

	private bool VoxelRaycast(Vector3 origin, Vector3 direction, float length, out TargetBlock targetedBlock)
	{
		direction = direction.normalized;
		targetedBlock = new TargetBlock(0, Vector3Int.zero, Vector3Int.zero, Vector3.zero, 0);
		Vector3Int vector3Int = VoxelTools.FloorPositionToInt(origin);
		Vector3 vector = new Vector3((direction.x == 0f) ? 0f : Mathf.Abs(1f / direction.x), (direction.y == 0f) ? 0f : Mathf.Abs(1f / direction.y), (direction.z == 0f) ? 0f : Mathf.Abs(1f / direction.z));
		Vector3 vector2 = new Vector3(((Mathf.Sign(direction.x) < 0f) ? BetterMod(origin.x, 1f) : (1f - BetterMod(origin.x, 1f))) * vector.x, ((Mathf.Sign(direction.y) < 0f) ? BetterMod(origin.y, 1f) : (1f - BetterMod(origin.y, 1f))) * vector.y, ((Mathf.Sign(direction.z) < 0f) ? BetterMod(origin.z, 1f) : (1f - BetterMod(origin.z, 1f))) * vector.z);
		if (vector2.x == 0f)
		{
			vector2.x = float.MaxValue;
		}
		if (vector2.y == 0f)
		{
			vector2.y = float.MaxValue;
		}
		if (vector2.z == 0f)
		{
			vector2.z = float.MaxValue;
		}
		Vector3Int zero = Vector3Int.zero;
		Vector3 truePosition = origin;
		new HashSet<Vector3Int>().Add(vector3Int);
		byte voxelID = 0;
		byte orientation = 0;
		Chunk chunk = null;
		float num2;
		for (float num = 0f; num < length; num += num2)
		{
			VoxelTools.GetBlockinfo(vector3Int + zero + WorldShifter.instance.offset, ref voxelID, ref orientation, ref chunk);
			if (voxelID != 0 && CollisionManager.RaycastCell(truePosition, direction * 2f, vector3Int + zero, voxelID, orientation, out var hitNormal, out truePosition))
			{
				targetedBlock = new TargetBlock(voxelID, vector3Int + zero, hitNormal, truePosition, orientation);
				if (chunk.neighbours.hasNeighbours)
				{
					return true;
				}
				return false;
			}
			num2 = Mathf.Min(vector2.x, vector2.y, vector2.z);
			if (num2 == vector2.x)
			{
				zero.x += (int)Mathf.Sign(direction.x);
				vector2.x = vector.x;
			}
			else
			{
				vector2.x -= num2;
			}
			if (num2 == vector2.y)
			{
				zero.y += (int)Mathf.Sign(direction.y);
				vector2.y = vector.y;
			}
			else
			{
				vector2.y -= num2;
			}
			if (num2 == vector2.z)
			{
				zero.z += (int)Mathf.Sign(direction.z);
				vector2.z = vector.z;
			}
			else
			{
				vector2.z -= num2;
			}
			truePosition += direction * num2;
		}
		return false;
	}

	private void PlayerBlockSelection()
	{
		if (Input.mouseScrollDelta != Vector2.zero)
		{
			PlayerInventory.instance.ChangeSelectedItem(Input.mouseScrollDelta.y);
		}
	}

	private float BetterMod(float a, float b)
	{
		return a - b * Mathf.Floor(a / b);
	}
}
