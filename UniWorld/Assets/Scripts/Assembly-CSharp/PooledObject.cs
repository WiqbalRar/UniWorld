using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
	public abstract void Initialize(params object[] parameters);

	public void Recycle()
	{
		OnRecycle();
		ObjectPool.instance.Recycle(base.gameObject);
	}

	protected virtual void OnRecycle()
	{
	}
}
