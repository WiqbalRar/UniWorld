using UnityEngine;

public class CloudManager : MonoBehaviour
{
	public int cloudCount;

	public GameObject cloudPrefab;

	public Transform[] clouds;

	public ParticleSystem[] particles;

	public float maxAxisDistance;

	public Vector3 cloudSpeed;

	public float cloudAltitude;

	public Vector3 minBoxSize;

	public Vector3 maxBoxSize;

	public float cubeUnitPerParticle = 25000f;

	public void Initialize()
	{
		ResetClouds();
	}

	[ContextMenu("ResetClouds")]
	private void ResetClouds()
	{
		Transform[] array;
		if (clouds.Length != 0)
		{
			array = clouds;
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i].gameObject);
			}
		}
		clouds = new Transform[cloudCount];
		particles = new ParticleSystem[cloudCount];
		for (int j = 0; j < cloudCount; j++)
		{
			GameObject gameObject = Object.Instantiate(cloudPrefab);
			particles[j] = gameObject.GetComponent<ParticleSystem>();
			clouds[j] = gameObject.transform;
			Vector3 scale = new Vector3(Random.Range(minBoxSize.x, maxBoxSize.x), Random.Range(minBoxSize.y, maxBoxSize.y), Random.Range(minBoxSize.z, maxBoxSize.z));
			ParticleSystem.ShapeModule shape = particles[j].shape;
			shape.scale = scale;
			ParticleSystem.EmissionModule emission = particles[j].emission;
			emission.rate = scale.x * scale.y * scale.z / cubeUnitPerParticle;
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = particles[j].velocityOverLifetime;
			velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(0f - cloudSpeed.x);
			velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(0f - cloudSpeed.y);
			velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(0f - cloudSpeed.z);
		}
		array = clouds;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].position = new Vector3(base.transform.position.x + Random.Range(0f - maxAxisDistance, maxAxisDistance), base.transform.position.y + cloudAltitude * Random.Range(0f, 1f), base.transform.position.z + Random.Range(0f - maxAxisDistance, maxAxisDistance));
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < clouds.Length; i++)
		{
			bool flag = false;
			clouds[i].position += cloudSpeed * Time.deltaTime;
			float f = clouds[i].position.x - base.transform.position.x;
			if (Mathf.Abs(f) > maxAxisDistance)
			{
				clouds[i].position -= Vector3.right * maxAxisDistance * Mathf.Sign(f) * 2f;
				flag = true;
			}
			float f2 = clouds[i].position.z - base.transform.position.z;
			if (Mathf.Abs(f2) > maxAxisDistance)
			{
				clouds[i].position -= Vector3.forward * maxAxisDistance * Mathf.Sign(f2) * 2f;
				flag = true;
			}
			if (flag)
			{
				Vector3 scale = new Vector3(Random.Range(minBoxSize.x, maxBoxSize.x), Random.Range(minBoxSize.y, maxBoxSize.y), Random.Range(minBoxSize.z, maxBoxSize.z));
				ParticleSystem.ShapeModule shape = particles[i].shape;
				shape.scale = scale;
				ParticleSystem.EmissionModule emission = particles[i].emission;
				emission.rate = scale.x * scale.y * scale.z / cubeUnitPerParticle;
			}
		}
	}
}
