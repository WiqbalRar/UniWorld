using UnityEngine;

namespace VoxelEngine
{
	public class ChunkGameObject
	{
		public GameObject gameObject;

		public MeshFilter meshFilter;

		public MeshRenderer meshRenderer;

		public MeshCollider meshCollider;

		public ChunkGameObject()
		{
			gameObject = new GameObject();
			gameObject.isStatic = true;
			meshFilter = gameObject.AddComponent<MeshFilter>();
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
		}

		public void Setup(Vector3i chunkPos, Vector3 realPos, Transform parentTransform)
		{
			gameObject.transform.parent = parentTransform;
			gameObject.transform.position = realPos;
			gameObject.name = "Chunk (" + chunkPos.x + "," + chunkPos.y + "," + chunkPos.z + ")";
			gameObject.SetActive(value: true);
		}

		public void Clean()
		{
			gameObject.SetActive(value: false);
			gameObject.name = "Chunk (Pooled)";
			Object.Destroy(meshFilter.sharedMesh);
			meshFilter.sharedMesh = null;
		}

		public void Destroy()
		{
			Object.Destroy(meshFilter.sharedMesh);
			meshFilter.sharedMesh = null;
			meshFilter = null;
			Object.Destroy(gameObject);
			gameObject = null;
		}
	}
}
