using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public string worldName;

	public int seed;

	public bool useGreedyMeshing;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		SetupShaderVariables();
	}

	public void LoadWorld(string newWorldName, int newSeed)
	{
		worldName = newWorldName;
		seed = newSeed;
		SceneManager.LoadScene(1);
	}

	private void SetupShaderVariables()
	{
		Shader.SetGlobalVector("_DoubleSidedIndexData", new Vector3(87f, 93f, 15f));
		Shader.SetGlobalVector("_DirtGrassIndexData", new Vector4(86f, 24f, 101f, 24f));
		Shader.SetGlobalVector("_LeavesMinMax", new Vector2(94f, 99f));
		Shader.SetGlobalVector("_RotatableMinMax", new Vector2(22f, 26f));
		Shader.SetGlobalVector("_StairsIndexData", new Vector3(101f, 113f, 29f));
		Shader.SetGlobalVector("_SlabsIndexData", new Vector3(114f, 126f, 42f));
		Shader.SetGlobalVector("_NonTriplanarMinMax", new Vector2(1f, 21f));
		Shader.SetGlobalFloat("_PlantsMax", 20f);
	}

	[ContextMenu("Test Chunk")]
	private void Test()
	{
	}
}
