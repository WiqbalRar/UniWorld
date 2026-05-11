using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
	public enum Id
	{
		voxelChunk = 0
	}

	public static ObjectPool instance;

	public GameObject[] prefabs;

	public int[] poolSize;

	public List<GameObject>[] availableObjectPools;

	private Dictionary<GameObject, int> idDictionary = new Dictionary<GameObject, int>();

	private void Awake()
	{
		instance = this;
		if (poolSize.Length < prefabs.Length)
		{
			poolSize = new int[prefabs.Length];
		}
	}

	private void OnEnable()
	{
		availableObjectPools = new List<GameObject>[prefabs.Length];
		for (int i = 0; i < prefabs.Length; i++)
		{
			List<GameObject> list = new List<GameObject>();
			for (int j = 0; j < poolSize[i]; j++)
			{
				GameObject gameObject = Object.Instantiate(prefabs[i], Vector3.zero, Quaternion.identity);
				gameObject.SetActive(value: false);
				idDictionary.Add(gameObject, i);
				list.Add(gameObject);
			}
			availableObjectPools[i] = list;
		}
	}

	public GameObject RequestObject(int poolId, params object[] parameters)
	{
		GameObject gameObject;
		if (availableObjectPools[poolId].Count > 0)
		{
			gameObject = availableObjectPools[poolId][availableObjectPools[poolId].Count - 1];
			availableObjectPools[poolId].RemoveAt(availableObjectPools[poolId].Count - 1);
		}
		else
		{
			gameObject = Object.Instantiate(prefabs[poolId]);
			idDictionary.Add(gameObject, poolId);
		}
		gameObject.SetActive(value: true);
		gameObject.GetComponent<PooledObject>().Initialize(parameters);
		return gameObject;
	}

	public void Recycle(GameObject objectToRecycle)
	{
		int num = idDictionary[objectToRecycle];
		objectToRecycle.SetActive(value: false);
		availableObjectPools[num].Add(objectToRecycle);
	}

	public void ClearPool(int poolID)
	{
		for (int num = availableObjectPools[poolID].Count - 1; num >= 0; num--)
		{
			Object.Destroy(availableObjectPools[poolID][num]);
		}
		availableObjectPools[poolID].Clear();
	}
}
