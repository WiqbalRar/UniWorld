using UnityEngine;

public class ChunkTest : MonoBehaviour
{
	public Vector3Int ChunkPosition;

	public Vector3Int myPosition;

	private void Update()
	{
		myPosition = VoxelTools.FloorPositionToInt(base.transform.position);
		Debug.Log(myPosition.x + " || " + myPosition.x % 16);
		ChunkPosition = VoxelTools.GetChunkPosition(myPosition);
	}
}
