using System.Collections.Generic;
using UnityEngine;

public class WorldShifter : MonoBehaviour
{
	public static WorldShifter instance;

	public Transform player;

	public CloudManager cloudManager;

	public VoxelParticleManager particleManager;

	public Vector3Int offset;

	public int shiftThreshold = 960;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
		offset = Vector3Int.zero;
		Shader.SetGlobalVector("_WorldShift", Vector2.zero);
		player = GameObject.FindGameObjectWithTag("Player").transform;
		particleManager = Object.FindObjectOfType<VoxelParticleManager>();
	}

	private void Update()
	{
		Vector3Int vector3Int = new Vector3Int(Mathf.RoundToInt(player.position.x / 16f) * 16, 0, Mathf.RoundToInt(player.position.z / 16f) * 16);
		if (Mathf.Abs(vector3Int.x) < shiftThreshold && Mathf.Abs(vector3Int.z) < shiftThreshold)
		{
			return;
		}
		offset += vector3Int;
		Shader.SetGlobalVector("_WorldShift", new Vector2(offset.x, offset.z));
		player.position -= (Vector3)vector3Int;
		foreach (KeyValuePair<Vector3Int, Chunk> activeChunk in ChunkManager.instance.activeChunks)
		{
			activeChunk.Value.transform.position -= (Vector3)vector3Int;
		}
		Transform[] clouds = cloudManager.clouds;
		for (int i = 0; i < clouds.Length; i++)
		{
			clouds[i].position -= (Vector3)vector3Int;
		}
		particleManager.ShiftParticles(-vector3Int);
	}
}
