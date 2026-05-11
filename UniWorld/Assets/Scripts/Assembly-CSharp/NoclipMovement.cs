using UnityEngine;

public class NoclipMovement : FPSCam
{
	private Vector3 direction;

	public float speed = 10f;

	public float boostMultiplier = 10f;

	public override void Update()
	{
		base.Update();
		direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		if (Input.GetKey(KeyCode.E))
		{
			direction.y += 1f;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			direction.y -= 1f;
		}
		Vector3 vector = (cameraTransform.right * direction.x + cameraTransform.up * direction.y + cameraTransform.forward * direction.z).normalized * speed * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f);
		base.transform.position += vector * Time.deltaTime;
	}
}
