using UnityEngine;

public class DebugScript : MonoBehaviour
{
	public static DebugScript instance;

	public GameObject pigPrefab;

	private GameObject myPig;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
	}

	public void SpawnPig(Vector3 position)
	{
		myPig = Object.Instantiate(pigPrefab, position, Quaternion.identity);
	}
}
