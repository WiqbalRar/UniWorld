using UnityEngine;

public class FollowTarget : MonoBehaviour
{
	public Transform target;

	public float lerpCoef;

	private void Update()
	{
		if (lerpCoef != 0f)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, target.position, lerpCoef * Time.deltaTime);
		}
		else
		{
			base.transform.position = target.position;
		}
	}
}
