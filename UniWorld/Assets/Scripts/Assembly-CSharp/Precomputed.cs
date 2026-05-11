using UnityEngine;

public class Precomputed : MonoBehaviour
{
	public static Precomputed instance;

	public static Vector3Int[] indexCoords;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
		Precompute();
	}

	private void Precompute()
	{
		PrecomputeIndexCoords();
	}

	private void PrecomputeIndexCoords()
	{
		indexCoords = new Vector3Int[Chunk.WIDTHxHEIGHTxWIDTH];
		for (int i = 0; i < Chunk.WIDTH; i++)
		{
			for (int j = 0; j < Chunk.HEIGHT; j++)
			{
				for (int k = 0; k < Chunk.WIDTH; k++)
				{
					indexCoords[i * Chunk.HEIGHT * Chunk.WIDTH + j * Chunk.WIDTH + k] = new Vector3Int(i, j, k);
				}
			}
		}
	}
}
