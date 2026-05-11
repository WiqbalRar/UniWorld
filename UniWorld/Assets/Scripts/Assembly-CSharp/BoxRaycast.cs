using UnityEngine;

public class BoxRaycast : MonoBehaviour
{
	public static BoxRaycast instance;

	public BoxCollider box;

	private void Awake()
	{
		if ((bool)instance)
		{
			Object.Destroy(this);
		}
		instance = this;
		CollisionManager.box = box;
	}
}
