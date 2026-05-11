using UnityEngine;

public class FreeCam : MonoBehaviour
{
	public float moveSpeed = 12f;

	public float shiftSpeedModifier = 3f;

	public float lookSensitivty = 0.8f;

	public float verticalLookMinMax = 80f;

	private Vector3 lastMouse;

	private void Update()
	{
		UpdateRotation();
		UpdateMovement();
	}

	private void UpdateRotation()
	{
		if (Input.GetMouseButtonDown(0))
		{
			lastMouse = Input.mousePosition;
		}
		if (Input.GetMouseButton(0))
		{
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			localEulerAngles.x += (lastMouse.y - Input.mousePosition.y) * lookSensitivty;
			localEulerAngles.y += (Input.mousePosition.x - lastMouse.x) * lookSensitivty;
			if (localEulerAngles.x > 180f)
			{
				localEulerAngles.x -= 360f;
			}
			localEulerAngles.x = Mathf.Clamp(localEulerAngles.x, 0f - verticalLookMinMax, verticalLookMinMax);
			base.transform.localEulerAngles = localEulerAngles;
			lastMouse = Input.mousePosition;
		}
	}

	private void UpdateMovement()
	{
		float num = moveSpeed;
		if (Input.GetKey(KeyCode.LeftShift))
		{
			num *= shiftSpeedModifier;
		}
		Vector3 vector = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		vector = base.transform.rotation * vector;
		if (Input.GetKey(KeyCode.Q))
		{
			vector.y = -1f;
		}
		else if (Input.GetKey(KeyCode.E))
		{
			vector.y = 1f;
		}
		vector *= num * Time.deltaTime;
		base.transform.Translate(vector, Space.World);
	}
}
