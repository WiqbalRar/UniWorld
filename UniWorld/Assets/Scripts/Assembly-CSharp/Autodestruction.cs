using UnityEngine;

public class Autodestruction : MonoBehaviour
{
	public float delay;

	private void Start()
	{
		Object.Destroy(base.gameObject, delay);
	}
}
