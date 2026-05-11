using UnityEngine;

public class CameraVerticalSmoother : MonoBehaviour
{
	public Transform positionTarget;

	public Transform rotationTarget;

	public float lerpCoef;

	public FpsController myController;

	private void Awake()
	{
		myController = GetComponentInParent<FpsController>();
		base.transform.parent = null;
	}

	private void Update()
	{
		if (myController.yVelocity == 0f && !myController.godMode)
		{
			float y = Mathf.Lerp(base.transform.position.y, positionTarget.position.y, lerpCoef * Time.deltaTime);
			base.transform.position = new Vector3(positionTarget.position.x, y, positionTarget.position.z);
		}
		else
		{
			base.transform.position = positionTarget.position;
		}
		base.transform.rotation = rotationTarget.rotation;
	}
}
