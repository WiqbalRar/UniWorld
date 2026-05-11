using UnityEngine;

public class FPSCam : MonoBehaviour
{
	public Transform cameraTransform;

	public Transform bodyTransform;

	public float mouseSensitivity = 1f;

	private float xRotation;

	private FpsController myFpsController;

	private void Start()
	{
		float fieldOfView = PlayerPrefs.GetInt("FieldOfView");
		Camera[] array = Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].fieldOfView = fieldOfView;
		}
		myFpsController = GetComponent<FpsController>();
		Cursor.lockState = CursorLockMode.Locked;
		UpdateMouseSensitivity();
	}

	public void UpdateMouseSensitivity()
	{
		if (PlayerPrefs.HasKey("MouseSensitivity"))
		{
			mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
		}
	}

	public virtual void Update()
	{
		if (Time.timeScale != 0f && myFpsController.canMove && !PlayerInventory.instance.isOpen)
		{
			float num = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
			float num2 = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
			xRotation -= num2;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);
			cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
			bodyTransform.Rotate(Vector3.up * num);
		}
	}
}
