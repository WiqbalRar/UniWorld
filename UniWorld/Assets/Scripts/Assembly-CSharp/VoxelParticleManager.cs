using System.Collections.Generic;
using UnityEngine;

public class VoxelParticleManager : MonoBehaviour
{
	private struct ParticleGroup
	{
		public Vector3[] position;

		public Vector3[] velocity;

		public float[] remainingLife;

		public Matrix4x4[] matrices;

		public float voxelID;

		public float temperature;

		public float humidity;

		public Vector3Int worldPos;

		public ParticleGroup(Vector3 origin, float boxSize, float minDuration, float maxDuration, int minCount, int maxCount, byte voxelID, float temperature, float humidity, Vector3Int worldPos)
		{
			int num = Random.Range(minCount, maxCount);
			position = new Vector3[num];
			velocity = new Vector3[num];
			remainingLife = new float[num];
			if (voxelID >= 101 && voxelID <= 113)
			{
				this.voxelID = voxelID - 29;
			}
			else if (voxelID >= 114 && voxelID <= 126)
			{
				this.voxelID = voxelID - 42;
			}
			else if (voxelID == 86)
			{
				this.voxelID = 24f;
			}
			else
			{
				this.voxelID = (int)voxelID;
			}
			matrices = new Matrix4x4[num];
			this.temperature = (temperature + 1f) / 2f;
			this.humidity = (humidity + 1f) / 2f;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = new Vector3(Random.Range(0f - boxSize, boxSize), Random.Range(0f - boxSize, boxSize), Random.Range(0f - boxSize, boxSize));
				position[i] = origin + vector;
				vector.y = Mathf.Abs(vector.y);
				velocity[i] = new Vector3(vector.x * 3f, vector.y * 9f, vector.z * 3f);
				remainingLife[i] = Random.Range(minDuration, maxDuration);
			}
			this.worldPos = worldPos;
		}

		public bool ProcessParticlesToRender(out float index, Vector3 cameraPosition, Vector3 cameraUp)
		{
			bool result = false;
			index = voxelID;
			for (int i = 0; i < position.Length; i++)
			{
				if (remainingLife[i] > 0f)
				{
					result = true;
					position[i] += velocity[i] * Time.deltaTime;
					velocity[i].y += -30f * Time.deltaTime;
					remainingLife[i] -= Time.deltaTime;
					Quaternion q = Quaternion.LookRotation(position[i] - cameraPosition, cameraUp);
					matrices[i] = Matrix4x4.TRS(position[i], q, particleSize);
				}
				else
				{
					matrices[i] = Matrix4x4.TRS(position[i], Quaternion.identity, Vector3.zero);
				}
			}
			return result;
		}

		public void ShiftPositions(Vector3 offset)
		{
			for (int i = 0; i < position.Length; i++)
			{
				position[i] += offset;
			}
		}
	}

	public static VoxelParticleManager instance;

	public Mesh mesh;

	public Material particleMat;

	public Transform target;

	private static Vector3 particleSize = new Vector3(0.2f, 0.2f, 0.2f);

	private List<ParticleGroup> particleGroup = new List<ParticleGroup>();

	private MaterialPropertyBlock properties;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		mesh = new Mesh();
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(-0.5f, -0.5f, 0f),
			new Vector3(0.5f, -0.5f, 0f),
			new Vector3(-0.5f, 0.5f, 0f),
			new Vector3(0.5f, 0.5f, 0f)
		};
		int[] triangles = new int[6] { 0, 2, 3, 0, 3, 1 };
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		properties = new MaterialPropertyBlock();
	}

	public void LateUpdate()
	{
		DrawParticles();
	}

	[ContextMenu("CreateParticle")]
	public void CreateParticle(Vector3 position, byte voxelID, float temperature, float humidity, Vector3Int worldPos, float size = 0.35f)
	{
		if (voxelID != 0)
		{
			particleGroup.Add(new ParticleGroup(position, size, 0.15f, 0.35f, 32, 48, voxelID, temperature, humidity, worldPos));
		}
	}

	public void ShiftParticles(Vector3Int offsetInt)
	{
		Vector3 offset = offsetInt;
		for (int num = particleGroup.Count - 1; num >= 0; num--)
		{
			particleGroup[num].ShiftPositions(offset);
		}
	}

	public void DrawParticles()
	{
		if (particleGroup.Count <= 0)
		{
			return;
		}
		Vector3 position = Camera.main.transform.position;
		Vector3 up = Camera.main.transform.up;
		for (int num = particleGroup.Count - 1; num >= 0; num--)
		{
			if (particleGroup[num].ProcessParticlesToRender(out var index, position, up))
			{
				byte lightValue = VoxelTools.GetLightValue(particleGroup[num].worldPos);
				float value = (float)(int)NoiseManager.instance.blockLightValues[LightManager.GetBlockLight(lightValue)] / 255f;
				float value2 = (float)(int)NoiseManager.instance.sunLightValues[LightManager.GetSunLight(lightValue)] / 255f;
				properties.SetFloat("_Index", index);
				properties.SetFloat("_Temperature", particleGroup[num].temperature);
				properties.SetFloat("_Humidity", particleGroup[num].humidity);
				properties.SetFloat("_SunLight", value2);
				properties.SetFloat("_BlockLight", value);
				Graphics.DrawMeshInstanced(mesh, 0, particleMat, particleGroup[num].matrices, particleGroup[num].matrices.Length, properties);
			}
			else
			{
				particleGroup.RemoveAt(num);
			}
		}
	}
}
