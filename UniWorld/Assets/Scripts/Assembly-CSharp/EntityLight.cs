using UnityEngine;

public class EntityLight : MonoBehaviour
{
	public Material myMaterialAsset;

	private Material myMat;

	public MeshRenderer[] myRenderers;

	private bool isInitialized;

	private Vector3Int lastPos = Vector3Int.zero;

	private void Awake()
	{
		myMat = new Material(myMaterialAsset);
		MeshRenderer[] array = myRenderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = myMat;
		}
		UpdateLight();
	}

	public void UpdateLight()
	{
		Vector3Int vector3Int = VoxelTools.FloorPositionToInt(base.transform.position) + WorldShifter.instance.offset;
		if (!isInitialized || vector3Int != lastPos)
		{
			isInitialized = true;
			lastPos = vector3Int;
			byte lightValue = VoxelTools.GetLightValue(vector3Int);
			float value = (float)(int)NoiseManager.instance.blockLightValues[LightManager.GetBlockLight(lightValue)] / 255f;
			float value2 = (float)(int)NoiseManager.instance.sunLightValues[LightManager.GetSunLight(lightValue)] / 255f;
			myMat.SetFloat("_BlockLight", value);
			myMat.SetFloat("_SunLight", value2);
		}
	}
}
