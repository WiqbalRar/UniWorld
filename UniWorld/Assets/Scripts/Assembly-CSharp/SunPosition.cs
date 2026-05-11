using UnityEngine;

public class SunPosition : MonoBehaviour
{
	public ParticleSystem myStars;

	public AnimationCurve starIntensity;

	public Material starMaterial;

	public Color starsVisible;

	public Color starsHidden;

	public float rotationSpeed;

	public Vector3 myRotastion;

	private void Update()
	{
		base.transform.rotation *= Quaternion.Euler(rotationSpeed * Time.deltaTime, 0f, 0f);
		Shader.SetGlobalVector("_SunDirection", base.transform.forward);
		SetStarColor();
	}

	private void SetStarColor()
	{
		float t = starIntensity.Evaluate(Vector3.Dot(base.transform.forward, Vector3.down));
		starMaterial.SetColor("_BaseColor", Color.Lerp(starsHidden, starsVisible, t));
	}
}
