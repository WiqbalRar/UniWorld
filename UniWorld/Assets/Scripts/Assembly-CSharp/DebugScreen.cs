using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugScreen : MonoBehaviour
{
	public static DebugScreen instance;

	public GameObject debugObject;

	public GameObject axisDrawer;

	public LineRenderer[] axisLines;

	public TMP_Text debugText;

	public GameObject performanceObject;

	private bool debugisEnabled;

	private bool performanceIsEnabled;

	[Header("LookedBlock")]
	public bool isLookingBlock;

	public Vector3 targetedBlockPos;

	public byte targetedBlockID;

	private Player myPlayer;

	private Bounds bounds = new Bounds(Vector3.zero, new Vector3(0.5f, 1.8f, 0.5f));

	private Bounds boundsExtended = new Bounds(Vector3.zero, new Vector3(0.55f, 1.85f, 0.55f));

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
		myPlayer = Object.FindObjectOfType<Player>();
	}

	private void Update()
	{
		if (ActionManager.GetActionDown("DebugScreen"))
		{
			debugisEnabled = !debugisEnabled;
			debugObject.SetActive(debugisEnabled);
			axisDrawer.SetActive(debugisEnabled);
		}
		if (ActionManager.GetActionDown("PerformanceScreen"))
		{
			performanceIsEnabled = !performanceIsEnabled;
			performanceObject.SetActive(performanceIsEnabled);
		}
		if (!debugisEnabled)
		{
			return;
		}
		string text = "";
		if (isLookingBlock)
		{
			text = text + "target: " + (VoxelTools.FloorPositionToInt(targetedBlockPos) + WorldShifter.instance.offset).ToString() + " ID: " + targetedBlockID + "\n";
		}
		Vector3Int zero = Vector3Int.zero;
		if (!(myPlayer != null))
		{
			return;
		}
		zero = VoxelTools.FloorPositionToInt(myPlayer.playerPositionTransform.position) + WorldShifter.instance.offset;
		text += "pos:    ";
		string text2 = text;
		Vector3Int vector3Int = zero;
		text = text2 + vector3Int.ToString() + "\n";
		string text3 = text;
		vector3Int = WorldShifter.instance.offset;
		text = text3 + "Origin shift: " + vector3Int.ToString() + "\n";
		Chunk chunk = VoxelTools.GetChunk(zero);
		Vector3Int voxelPositionInChunk = VoxelTools.GetVoxelPositionInChunk(zero);
		int num = voxelPositionInChunk.x * Chunk.HEIGHT * Chunk.WIDTH + voxelPositionInChunk.y * Chunk.WIDTH + voxelPositionInChunk.z;
		text = text + "SunLight: " + LightManager.GetSunLight(chunk.lightMap[num]) + " BlocLight: " + LightManager.GetBlockLight(chunk.lightMap[num]) + "\n";
		text = text + "HeightMap: " + chunk.heightMap[voxelPositionInChunk.x * Chunk.WIDTH + voxelPositionInChunk.z] + " | GroundHeight: " + chunk.groundHeight[voxelPositionInChunk.x * Chunk.WIDTH + voxelPositionInChunk.z] + "\n";
		text = text + "HighestX: " + chunk.highestX[voxelPositionInChunk.z] + " HighestZ: " + chunk.highestZ[voxelPositionInChunk.x] + "\n";
		float[] array = new float[1];
		NoiseManager.instance.noiseC.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "C:  ";
		text = text + array[0].ToString("N2") + " | ";
		text = text + NoiseManager.instance.SampleFakeSpline(array[0], "C").ToString("N2") + "\n";
		NoiseManager.instance.noiseE.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "E:  ";
		text = text + array[0].ToString("N2") + " | ";
		text = text + NoiseManager.instance.SampleFakeSpline(array[0], "E").ToString("N2") + "\n";
		NoiseManager.instance.noiseR.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "R: ";
		text = text + array[0].ToString("N2") + " | ";
		text = text + NoiseManager.instance.SampleFakeSpline(array[0], "R").ToString("N2") + "\n";
		NoiseManager.instance.noiseM.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "M: ";
		text = text + array[0].ToString("N2") + " | ";
		text = text + NoiseManager.instance.SampleFakeSpline(array[0], "M").ToString("N2") + "\n";
		NoiseManager.instance.noiseW.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "W:  ";
		text = text + array[0].ToString("N2") + " | ";
		text = text + NoiseManager.instance.SampleFakeSpline(array[0], "W").ToString("N2") + "\n";
		NoiseManager.instance.noiseW2.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "W2:  ";
		text = text + array[0].ToString("N2") + " | ";
		text = text + NoiseManager.instance.SampleFakeSpline(array[0], "W2").ToString("N2") + "\n";
		int num2 = 0;
		NoiseManager.instance.noiseT.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "T:  ";
		text = text + array[0].ToString("N2") + " | ";
		string text4 = text;
		int num3 = (num2 = BiomeManager.instance.GetTemperatureIndex(array[0]));
		text = text4 + num3 + "\n";
		int num4 = 0;
		NoiseManager.instance.noiseH.fastNoiseSIMD.FillNoiseSet(array, zero.x, 0, zero.z, 1, 1, 1);
		text += "H:  ";
		text = text + array[0].ToString("N2") + " | ";
		string text5 = text;
		num3 = (num4 = BiomeManager.instance.GetHumidityIndex(array[0]));
		text = text5 + num3 + "\n";
		int biomeIDFromIndex = BiomeManager.instance.GetBiomeIDFromIndex(num2, num4);
		text = text + "Biome: " + biomeIDFromIndex + " " + BiomeID.biomeName[biomeIDFromIndex] + "\n";
		text = text + "Time: " + TimeManager.instance.normalizedTime.ToString("N2") + " (" + Mathf.FloorToInt(TimeManager.instance.normalizedTime * 24f).ToString("D2") + ":" + Mathf.FloorToInt(TimeManager.instance.normalizedTime * 24f % 1f * 60f).ToString("D2") + ")";
		debugText.text = text;
		Vector3 vector = Camera.main.transform.position + Camera.main.transform.forward * 5f + Camera.main.transform.right * 0.2f;
		axisLines[0].SetPositions(new Vector3[3]
		{
			vector,
			vector + Vector3.right * 0.95f,
			vector + Vector3.right
		});
		axisLines[1].SetPositions(new Vector3[3]
		{
			vector,
			vector + Vector3.up * 0.95f,
			vector + Vector3.up
		});
		axisLines[2].SetPositions(new Vector3[3]
		{
			vector,
			vector + Vector3.forward * 0.95f,
			vector + Vector3.forward
		});
		VoxelTools.DrawBounds(bounds, myPlayer.transform.position, Color.green);
		VoxelTools.DrawBounds(boundsExtended, myPlayer.transform.position, Color.blue);
		HashSet<Bounds> hashSet = new HashSet<Bounds>();
		CollisionManager.GetOverlapingBounds(myPlayer.transform.position, boundsExtended, hashSet);
		foreach (Bounds item in hashSet)
		{
			VoxelTools.DrawBounds(item, Vector3.zero, Color.red);
		}
	}
}
