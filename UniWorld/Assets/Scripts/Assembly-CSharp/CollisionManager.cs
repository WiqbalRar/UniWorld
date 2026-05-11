using System.Collections.Generic;
using UnityEngine;

public static class CollisionManager
{
	public static BoxCollider box;

	public static Vector3 halfStep = new Vector3(0f, 0.5f, 0f);

	public static void HorizontalMovement(Transform actorTransform, Bounds actorBounds, ref Vector3 movement, Vector3 stepHeight, bool isSneaking, bool isGrounded)
	{
		HashSet<Bounds> hashSet = new HashSet<Bounds>();
		if (!CheckCollision(actorTransform.position + movement, actorBounds))
		{
			if (!isSneaking || CheckCollision(actorTransform.position + movement + new Vector3(0f, -0.0011f, 0f), actorBounds))
			{
				return;
			}
		}
		else if (!isSneaking && isGrounded)
		{
			if (stepHeight.y == 1f)
			{
				if (!CheckCollision(actorTransform.position + movement + halfStep, actorBounds))
				{
					movement += halfStep;
					return;
				}
				if (!CheckCollision(actorTransform.position + movement + stepHeight, actorBounds))
				{
					movement += stepHeight;
					return;
				}
			}
			else if (!CheckCollision(actorTransform.position + movement + stepHeight, actorBounds))
			{
				movement += stepHeight;
				return;
			}
		}
		hashSet.Clear();
		Vector3 vector = new Vector3(movement.x, movement.y, 0f);
		if (!CheckCollision(actorTransform.position + vector, actorBounds))
		{
			if (!isSneaking)
			{
				movement = vector;
				return;
			}
			if (CheckCollision(actorTransform.position + vector + new Vector3(0f, -0.0011f, 0f), actorBounds))
			{
				movement = vector;
				return;
			}
		}
		else if (!isSneaking && isGrounded)
		{
			if (stepHeight.y == 1f)
			{
				if (!CheckCollision(actorTransform.position + movement + halfStep, actorBounds))
				{
					movement += halfStep;
					return;
				}
				if (!CheckCollision(actorTransform.position + movement + stepHeight, actorBounds))
				{
					movement += stepHeight;
					return;
				}
			}
			else if (!CheckCollision(actorTransform.position + movement + stepHeight, actorBounds))
			{
				movement += stepHeight;
				return;
			}
		}
		hashSet.Clear();
		vector = new Vector3(0f, movement.y, movement.z);
		if (!CheckCollision(actorTransform.position + vector, actorBounds))
		{
			if (!isSneaking)
			{
				movement = vector;
				return;
			}
			if (CheckCollision(actorTransform.position + vector + new Vector3(0f, -0.0011f, 0f), actorBounds))
			{
				movement = vector;
				return;
			}
		}
		else if (!isSneaking && isGrounded)
		{
			if (stepHeight.y == 1f)
			{
				if (!CheckCollision(actorTransform.position + movement + halfStep, actorBounds))
				{
					movement += halfStep;
					return;
				}
				if (!CheckCollision(actorTransform.position + movement + stepHeight, actorBounds))
				{
					movement += stepHeight;
					return;
				}
			}
			else if (!CheckCollision(actorTransform.position + movement + stepHeight, actorBounds))
			{
				movement += stepHeight;
				return;
			}
		}
		movement = Vector3.zero;
	}

	public static void VerticalMovement(Transform actorTransform, Bounds actorBounds, ref float yVelocity, ref bool isGrounded)
	{
		new HashSet<Bounds>();
		Vector3 vector = new Vector3(0f, yVelocity * Time.deltaTime, 0f);
		if (!CheckCollision(actorTransform.position + vector, actorBounds))
		{
			actorTransform.position += Time.deltaTime * yVelocity * Vector3.up;
			return;
		}
		if (yVelocity < 0f)
		{
			isGrounded = true;
		}
		yVelocity = 0f;
	}

	public static bool CheckCollision(Vector3 actorDestination, Bounds actorBounds, HashSet<Bounds> collisionBounds)
	{
		Bounds bounds = actorBounds;
		bounds.center += actorDestination;
		foreach (Bounds collisionBound in collisionBounds)
		{
			if (bounds.Intersects(collisionBound))
			{
				return true;
			}
		}
		return false;
	}

	public static bool CheckCollision(Vector3 actorDestination, Bounds actorBounds)
	{
		HashSet<Bounds> collisionBounds = new HashSet<Bounds>();
		GetOverlapingBounds(actorDestination, actorBounds, collisionBounds);
		return CheckCollision(actorDestination, actorBounds, collisionBounds);
	}

	public static void GetOverlapingBounds(Vector3 actorDestination, Bounds actorBounds, HashSet<Bounds> collisionBounds)
	{
		Bounds bounds = actorBounds;
		bounds.center += actorDestination;
		Vector3Int vector3Int = VoxelTools.FloorPositionToInt(bounds.min);
		Vector3Int vector3Int2 = VoxelTools.FloorPositionToInt(bounds.max);
		for (int i = vector3Int.x; i <= vector3Int2.x; i++)
		{
			for (int j = vector3Int.y; j <= vector3Int2.y; j++)
			{
				for (int k = vector3Int.z; k <= vector3Int2.z; k++)
				{
					AppendBounds(new Vector3Int(i, j, k), collisionBounds);
				}
			}
		}
	}

	public static void GetTotalCollisionBounds(Vector3 actorPosition, Bounds actorBounds, Vector3 movement, HashSet<Bounds> collisionBounds)
	{
		float x;
		float x2;
		if (movement.x < 0f)
		{
			x = actorPosition.x + actorBounds.max.x;
			x2 = actorPosition.x + actorBounds.min.x + movement.x;
		}
		else
		{
			x = actorPosition.x + actorBounds.min.x;
			x2 = actorPosition.x + actorBounds.max.x + movement.x;
		}
		float y;
		float y2;
		if (movement.y < 0f)
		{
			y = actorPosition.y + actorBounds.max.y;
			y2 = actorPosition.y + actorBounds.min.y + movement.y;
		}
		else
		{
			y = actorPosition.y + actorBounds.min.y;
			y2 = actorPosition.y + actorBounds.max.y + movement.y;
		}
		float z;
		float z2;
		if (movement.z < 0f)
		{
			z = actorPosition.z + actorBounds.max.z;
			z2 = actorPosition.z + actorBounds.min.z + movement.z;
		}
		else
		{
			z = actorPosition.z + actorBounds.min.z;
			z2 = actorPosition.z + actorBounds.max.z + movement.z;
		}
		Vector3 worldPosition = new Vector3(x, y, z);
		Vector3 worldPosition2 = new Vector3(x2, y2, z2);
		Vector3Int vector3Int = VoxelTools.FloorPositionToInt(worldPosition);
		Vector3Int vector3Int2 = VoxelTools.FloorPositionToInt(worldPosition2);
		int num = (int)Mathf.Sign(movement.x);
		int num2 = (int)Mathf.Sign(movement.y);
		int num3 = (int)Mathf.Sign(movement.z);
		for (int i = vector3Int.x; (num < 0 && i >= vector3Int2.x) || (num > 0 && i <= vector3Int2.x); i += num)
		{
			for (int j = vector3Int.y; (num2 < 0 && j >= vector3Int2.y) || (num2 > 0 && j <= vector3Int2.y); j += num2)
			{
				for (int k = vector3Int.z; (num3 < 0 && k >= vector3Int2.z) || (num3 > 0 && k <= vector3Int2.z); k += num3)
				{
					AppendBounds(new Vector3Int(i, j, k), collisionBounds);
				}
			}
		}
	}

	public static void GetTotalCollisionBounds(Vector3 actorPosition, Vector3 movement, HashSet<Bounds> collisionBounds)
	{
		float x = actorPosition.x;
		float x2 = actorPosition.x + movement.x;
		float y = actorPosition.y;
		float y2 = actorPosition.y + movement.y;
		float z = actorPosition.z;
		float z2 = actorPosition.z + movement.z;
		Vector3 worldPosition = new Vector3(x, y, z);
		Vector3 worldPosition2 = new Vector3(x2, y2, z2);
		Vector3Int vector3Int = VoxelTools.FloorPositionToInt(worldPosition);
		Vector3Int vector3Int2 = VoxelTools.FloorPositionToInt(worldPosition2);
		int num = (int)Mathf.Sign(movement.x);
		int num2 = (int)Mathf.Sign(movement.y);
		int num3 = (int)Mathf.Sign(movement.z);
		for (int i = vector3Int.x; (num < 0 && i >= vector3Int2.x) || (num > 0 && i <= vector3Int2.x); i += num)
		{
			for (int j = vector3Int.y; (num2 < 0 && j >= vector3Int2.y) || (num2 > 0 && j <= vector3Int2.y); j += num2)
			{
				for (int k = vector3Int.z; (num3 < 0 && k >= vector3Int2.z) || (num3 > 0 && k <= vector3Int2.z); k += num3)
				{
					AppendBounds(new Vector3Int(i, j, k), collisionBounds);
				}
			}
		}
	}

	public static void AppendBounds(Vector3Int position, HashSet<Bounds> collisionBounds)
	{
		byte orientation = 0;
		byte voxelID = 0;
		VoxelTools.GetBlockinfo(position + WorldShifter.instance.offset, ref voxelID, ref orientation);
		switch (voxelID)
		{
		case 0:
			return;
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 63:
		case 64:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 80:
		case 81:
		case 82:
		case 83:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
		case 93:
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
		case 99:
		case 100:
			collisionBounds.Add(new Bounds(position + Vector3.one / 2f, Vector3.one));
			return;
		}
		if (voxelID >= 101 && voxelID <= 113)
		{
			Bounds[] stairBounds = VoxelBounds.stairBounds;
			for (int i = 0; i < stairBounds.Length; i++)
			{
				Bounds item = RotateBounds(stairBounds[i], VoxelBounds.stairRotations[orientation]);
				item.center += position + Vector3.one / 2f;
				collisionBounds.Add(item);
			}
		}
		else if (voxelID >= 114 && voxelID <= 126)
		{
			Bounds[] stairBounds = VoxelBounds.slabBounds;
			for (int i = 0; i < stairBounds.Length; i++)
			{
				Bounds item2 = RotateBounds(stairBounds[i], VoxelBounds.slabRotation[orientation]);
				item2.center += position + Vector3.one / 2f;
				collisionBounds.Add(item2);
			}
		}
	}

	public static void AppendBoundsForRaycast(Vector3Int position, HashSet<Bounds> collisionBounds, byte voxelID, byte orientation)
	{
		switch (voxelID)
		{
		case 0:
			return;
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 36:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 42:
		case 43:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 50:
		case 51:
		case 52:
		case 53:
		case 54:
		case 55:
		case 56:
		case 57:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 63:
		case 64:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 75:
		case 76:
		case 77:
		case 78:
		case 79:
		case 80:
		case 81:
		case 82:
		case 83:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 89:
		case 90:
		case 91:
		case 92:
		case 93:
		case 94:
		case 95:
		case 96:
		case 97:
		case 98:
		case 99:
		case 100:
			collisionBounds.Add(new Bounds(position + Vector3.one / 2f, Vector3.one));
			return;
		}
		if (voxelID >= 101 && voxelID <= 113)
		{
			Bounds[] stairBounds = VoxelBounds.stairBounds;
			for (int i = 0; i < stairBounds.Length; i++)
			{
				Bounds item = RotateBounds(stairBounds[i], VoxelBounds.stairRotations[orientation]);
				item.center += position + Vector3.one / 2f;
				collisionBounds.Add(item);
			}
			return;
		}
		if (voxelID >= 114 && voxelID <= 126)
		{
			Bounds[] stairBounds = VoxelBounds.slabBounds;
			for (int i = 0; i < stairBounds.Length; i++)
			{
				Bounds item2 = RotateBounds(stairBounds[i], VoxelBounds.slabRotation[orientation]);
				item2.center += position + Vector3.one / 2f;
				collisionBounds.Add(item2);
			}
			return;
		}
		if (voxelID >= 21 && voxelID <= 21)
		{
			Bounds[] stairBounds = VoxelBounds.torchBounds;
			for (int i = 0; i < stairBounds.Length; i++)
			{
				Bounds bounds = stairBounds[i];
				Bounds item3 = new Bounds(bounds.center + position + Vector3.one / 2f + VoxelBounds.torchTranslation[orientation], bounds.size);
				collisionBounds.Add(item3);
			}
			return;
		}
		switch (voxelID)
		{
		case 1:
		{
			Bounds item5 = new Bounds(VoxelBounds.grassPlantBounds.center + position + Vector3.one / 2f, VoxelBounds.grassPlantBounds.size);
			collisionBounds.Add(item5);
			break;
		}
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		{
			Chunk chunk = null;
			int voxelIndex = 0;
			Vector3Int voxelPosition = Vector3Int.zero;
			VoxelTools.GetBlockinfo(position + WorldShifter.instance.offset, ref chunk, ref voxelIndex, ref voxelPosition);
			float num = chunk.noiseSetWhite1[voxelPosition.x * Chunk.WIDTH + voxelPosition.z];
			Vector3 vector = new Vector3(num * 0.3125f, 0f, num % 0.1f * 3.125f);
			Bounds item4 = new Bounds(VoxelBounds.plantBounds.center + position + Vector3.one / 2f + vector, VoxelBounds.plantBounds.size);
			collisionBounds.Add(item4);
			break;
		}
		}
	}

	public static bool CheckBox(Vector3 actorPosition, Bounds actorBounds, Vector3 boxCenter)
	{
		Bounds bounds = actorBounds;
		bounds.center = actorPosition;
		Bounds bounds2 = new Bounds(boxCenter, Vector3.one);
		return bounds.Intersects(bounds2);
	}

	public static Bounds RotateBounds(Bounds bounds, Quaternion rotation)
	{
		Bounds result = default(Bounds);
		result.center = rotation * bounds.center;
		result.extents = rotation * bounds.extents;
		result.extents = new Vector3(Mathf.Abs(result.extents.x), Mathf.Abs(result.extents.y), Mathf.Abs(result.extents.z));
		return result;
	}

	public static bool RaycastCell(Vector3 rayOrigin, Vector3 rayDirection, Vector3Int cellPosition, byte voxelID, byte voxelOrientation, out Vector3Int hitNormal, out Vector3 truePosition)
	{
		Ray ray = new Ray(rayOrigin - rayDirection.normalized * 0.1f, rayDirection);
		hitNormal = Vector3Int.zero;
		HashSet<Bounds> hashSet = new HashSet<Bounds>();
		AppendBoundsForRaycast(cellPosition, hashSet, voxelID, voxelOrientation);
		bool result = false;
		float num = float.MaxValue;
		truePosition = rayOrigin;
		foreach (Bounds item in hashSet)
		{
			box.center = item.center;
			box.size = item.size;
			if (box.Raycast(ray, out var hitInfo, 3f) && hitInfo.distance < num)
			{
				num = hitInfo.distance;
				hitNormal = VoxelTools.FloorPositionToInt(hitInfo.normal);
				result = true;
				truePosition = hitInfo.point;
			}
		}
		return result;
	}
}
