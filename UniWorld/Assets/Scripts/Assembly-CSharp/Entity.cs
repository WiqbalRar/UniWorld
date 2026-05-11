using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	public int height;

	private float speed = 2f;

	public Animator myAnim;

	public Transform bodyRoot;

	private List<Vector3Int> path = new List<Vector3Int>();

	private Vector3 targetPos = Vector3.zero;

	public EntityLight myLight;

	private float yVelocity;

	private Bounds entityBounds = new Bounds(new Vector3(0f, 0.4f, 0f), new Vector3(0.8f, 0.8f, 0.8f));

	private bool isGrounded;

	private Vector3 lastVelocity = Vector3.zero;

	public float groundLerpCoef;

	public float gravity = -9.81f;

	private Vector3 lookVector = Vector3.zero;

	public float bodyVerticalLerpCoef = 15f;

	private void Awake()
	{
		bodyRoot.transform.parent = null;
	}

	[ContextMenu("GetPathToPlayer")]
	public void GetPathToPlayer()
	{
		VoxelPathfinder.GetPath(VoxelTools.FloorPositionToInt(base.transform.position), VoxelTools.FloorPositionToInt(Player.instance.transform.position), path, 1);
	}

	private void Update()
	{
		if (path.Count > 0 || !isGrounded)
		{
			MoveAlongPath(path);
			UpdateBodyRootPos();
			myLight.UpdateLight();
		}
	}

	private void MoveAlongPath(List<Vector3Int> path)
	{
		for (int i = 0; i < path.Count; i++)
		{
			VoxelTools.DrawBounds(new Bounds(VoxelTools.FloorPosition(path[i]) + Vector3.one / 2f, Vector3.one * 0.8f), Vector3.zero, Color.white);
		}
		VoxelTools.DrawBounds(entityBounds, base.transform.position, Color.white);
		if (Time.timeScale != 0f)
		{
			Vector3 directionFromPath = GetDirectionFromPath(path);
			if (directionFromPath.magnitude > 0.2f)
			{
				lookVector = directionFromPath;
			}
			directionFromPath = directionFromPath.normalized;
			directionFromPath *= speed;
			Vector3 vector = Vector3.Lerp(lastVelocity, directionFromPath, groundLerpCoef * Time.deltaTime);
			if (yVelocity <= 0f)
			{
				CheckForGround();
			}
			else
			{
				isGrounded = false;
			}
			Vector3 vector2 = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
			if (vector.magnitude < 0.1f && directionFromPath == Vector3.zero)
			{
				vector = Vector3.zero;
			}
			Vector3 movement = vector * Time.deltaTime;
			CollisionManager.HorizontalMovement(base.transform, entityBounds, ref movement, new Vector3(0f, 1f, 0f), isSneaking: false, isGrounded);
			Vector3.Distance(base.transform.position, targetPos);
			_ = base.transform.position + movement;
			base.transform.position += movement;
			if (!isGrounded)
			{
				yVelocity += gravity * Time.deltaTime;
			}
			if (yVelocity != 0f)
			{
				CollisionManager.VerticalMovement(base.transform, entityBounds, ref yVelocity, ref isGrounded);
			}
			lastVelocity = (new Vector3(base.transform.position.x, 0f, base.transform.position.z) - vector2) / Time.deltaTime;
		}
	}

	private Vector3 GetDirectionFromPath(List<Vector3Int> path)
	{
		if (path.Count == 0)
		{
			return Vector3.zero;
		}
		Vector3Int vector3Int = path[path.Count - 1];
		targetPos = vector3Int + new Vector3(0.5f, 0f, 0.5f);
		Vector3 b = targetPos;
		b.y = 0f;
		Vector3 position = base.transform.position;
		position.y = 0f;
		Vector3 result;
		if (Vector3.Distance(position, b) < 0.05f && isGrounded)
		{
			if (path.Count == 1)
			{
				result = targetPos - base.transform.position;
				result.y = 0f;
				path.Clear();
				myAnim.SetTrigger("GoIdle");
			}
			else
			{
				path.RemoveAt(path.Count - 1);
				vector3Int = path[path.Count - 1];
				targetPos = vector3Int + new Vector3(0.5f, 0f, 0.5f);
				result = targetPos - base.transform.position;
				result.y = 0f;
			}
		}
		else
		{
			result = targetPos - base.transform.position;
			result.y = 0f;
		}
		return result;
	}

	private void UpdateBodyRootPos()
	{
		Quaternion b = Quaternion.LookRotation(lookVector, Vector3.up);
		bodyRoot.rotation = Quaternion.Lerp(bodyRoot.rotation, b, groundLerpCoef * Time.deltaTime);
		if (yVelocity == 0f)
		{
			bodyRoot.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(bodyRoot.transform.position.y, base.transform.position.y, bodyVerticalLerpCoef * Time.deltaTime), base.transform.position.z);
		}
		else
		{
			bodyRoot.transform.position = base.transform.position;
		}
	}

	private void CheckForGround()
	{
		isGrounded = CollisionManager.CheckCollision(base.transform.position + new Vector3(0f, -0.0011f, 0f), entityBounds);
		if (isGrounded)
		{
			yVelocity = 0f;
		}
	}
}
