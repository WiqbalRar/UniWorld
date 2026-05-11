using UnityEngine;

public class FpsController : MonoBehaviour
{
	[Header("Run Settings")]
	public float speed;

	public float groundedLerpCoef;

	public float aerialLerpCoef;

	[Header("Jump Settings")]
	public float jumpHeight = 1.5f;

	public float gravity = -9.81f;

	public float yVelocity;

	private Vector3 lastVelocity;

	public bool isGrounded;

	public bool isSneaking;

	public bool canMove;

	public bool positionInitialized;

	[Header("Sneaking")]
	public float sneakPlayerRadius;

	public Transform cameraRootHeight;

	public float normalCameraHeight;

	public float sneakingCameraHeight;

	public float sneakingLerpCoef;

	[Header("Collision")]
	public float collisionPlayerRadius;

	public Transform playerFeet;

	public Transform playerHead;

	[Header("GodMode")]
	public bool godMode;

	public float godModeOnJumpHeight;

	public float godModeOnSpeed;

	public float godModeOffJumpHeight;

	public float godModeOffSpeed;

	public Bounds playerBounds;

	protected virtual void Start()
	{
		playerBounds = new Bounds(Vector3.zero, new Vector3(0.5f, 1.8f, 0.5f));
	}

	public void EnableMovement()
	{
		if (positionInitialized)
		{
			return;
		}
		Vector3 position = base.transform.position;
		for (float num = (float)Chunk.HEIGHT - 0.5f; num >= 0f; num -= 1f)
		{
			position.y = num;
			byte voxelID = VoxelTools.GetVoxelID(VoxelTools.FloorPositionToInt(position));
			if (voxelID != 0)
			{
				position += Vector3.up * 2f;
				if (voxelID >= 241)
				{
					break;
				}
				base.transform.position = position;
				positionInitialized = true;
				Debug.Log($"Found spawn first try, id: {voxelID}");
				return;
			}
		}
		Vector3 position2 = position;
		for (float num2 = position.x - 32f; num2 < position.x + 32f; num2 += 1f)
		{
			for (float num3 = position.z - 32f; num3 < position.z + 32f; num3 += 1f)
			{
				for (float num4 = (float)Chunk.HEIGHT - 0.5f; num4 >= 0f; num4 -= 1f)
				{
					Vector3 vector = new Vector3(num2, num4, num3);
					byte voxelID2 = VoxelTools.GetVoxelID(VoxelTools.FloorPositionToInt(vector));
					if (voxelID2 != 0)
					{
						if (voxelID2 >= 241)
						{
							break;
						}
						vector += Vector3.up * 2f;
						base.transform.position = vector;
						positionInitialized = true;
						return;
					}
				}
			}
		}
		Debug.Log("Didint find a pos, defaulting spawn");
		base.transform.position = position2;
		positionInitialized = true;
	}

	public void Update()
	{
		if (!canMove || Time.timeScale == 0f || Time.deltaTime == 0f || PlayerInventory.instance.isOpen)
		{
			return;
		}
		if (ActionManager.GetActionDown("Noclip"))
		{
			godMode = !godMode;
			speed = (godMode ? godModeOnSpeed : godModeOffSpeed);
			jumpHeight = (godMode ? godModeOnJumpHeight : godModeOffJumpHeight);
		}
		if (godMode)
		{
			GodMovement();
			return;
		}
		if (yVelocity <= 0f)
		{
			CheckForGround();
		}
		else
		{
			isGrounded = false;
		}
		isSneaking = isGrounded && ActionManager.GetAction("Sneak");
		cameraRootHeight.transform.localPosition = Vector3.Lerp(cameraRootHeight.transform.localPosition, new Vector3(0f, isSneaking ? sneakingCameraHeight : normalCameraHeight, 0f), sneakingLerpCoef * Time.deltaTime);
		Vector3 vector = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
		float num = (ActionManager.GetAction("Forward") ? 1f : 0f) + (ActionManager.GetAction("Backward") ? (-1f) : 0f);
		float num2 = (ActionManager.GetAction("Right") ? 1f : 0f) + (ActionManager.GetAction("Left") ? (-1f) : 0f);
		Vector3 normalized = (base.transform.right * num2 + base.transform.forward * num).normalized;
		if (ActionManager.GetAction("Sprint") && num > 0f && !isSneaking)
		{
			normalized += base.transform.forward;
		}
		normalized *= speed * (isSneaking ? 0.5f : 1f);
		Vector3 vector2 = Vector3.Lerp(lastVelocity, normalized, (isGrounded ? groundedLerpCoef : aerialLerpCoef) * Time.deltaTime);
		if (vector2.magnitude < 0.1f && normalized == Vector3.zero)
		{
			vector2 = Vector3.zero;
		}
		Vector3 movement = vector2 * Time.deltaTime;
		CollisionManager.HorizontalMovement(base.transform, playerBounds, ref movement, new Vector3(0f, 0.5f, 0f), isSneaking, isGrounded);
		base.transform.position += movement;
		if (!isGrounded)
		{
			yVelocity += gravity * Time.deltaTime;
		}
		if (isGrounded && Input.GetKey(KeyCodeManager.instance.actionKey["Jump"]))
		{
			isGrounded = false;
			yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
		if (yVelocity != 0f)
		{
			CollisionManager.VerticalMovement(base.transform, playerBounds, ref yVelocity, ref isGrounded);
		}
		lastVelocity = (new Vector3(base.transform.position.x, 0f, base.transform.position.z) - vector) / Time.deltaTime;
	}

	private void CheckForGround()
	{
		isGrounded = CollisionManager.CheckCollision(base.transform.position + new Vector3(0f, -0.0011f, 0f), playerBounds);
		if (isGrounded)
		{
			yVelocity = 0f;
		}
	}

	private void GodMovement()
	{
		float num = (ActionManager.GetAction("Forward") ? 1f : 0f) + (ActionManager.GetAction("Backward") ? (-1f) : 0f);
		float num2 = (ActionManager.GetAction("Right") ? 1f : 0f) + (ActionManager.GetAction("Left") ? (-1f) : 0f);
		float num3 = (ActionManager.GetAction("Jump") ? 1f : 0f) + (ActionManager.GetAction("Sneak") ? (-1f) : 0f);
		Vector3 vector = (Camera.main.transform.forward * num + Camera.main.transform.right * num2 + Camera.main.transform.up * num3).normalized * godModeOnSpeed * (ActionManager.GetAction("Sprint") ? 10f : 1f);
		base.transform.position += vector * Time.deltaTime;
	}
}
