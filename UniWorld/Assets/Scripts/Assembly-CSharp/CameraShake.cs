using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public void SingleShake(float intensity = 0.05f, float halfDuration = 0.05f)
	{
		Vector3 vector = Random.insideUnitCircle.normalized;
		SingleShake(vector, intensity, halfDuration);
	}

	public void SingleShake(Vector2 target, float intensity = 0.05f, float halfDuration = 0.05f)
	{
		StartCoroutine(ShakeRoutine(halfDuration, target * intensity));
	}

	public void MultipleShakes(int count = 3, float intensity = 0.06f, float halfDuration = 0.05f, float delay = 0.025f)
	{
		StartCoroutine(MultipleShakesRoutine(count, intensity, halfDuration, delay));
	}

	public void MultipleShakesDecayingIntensity(int count = 10, float intensity = 0.1f, float halfDuration = 0.06f, float delay = 0.05f)
	{
		StartCoroutine(MultipleShakesDecayingIntensityRoutine(count, intensity, halfDuration, delay));
	}

	private IEnumerator MultipleShakesDecayingIntensityRoutine(int count, float intensity, float halfDuration, float delay)
	{
		for (int i = 0; i < count; i++)
		{
			SingleShake(intensity, halfDuration);
			intensity *= 0.85f;
			yield return new WaitForSeconds(delay);
		}
	}

	private IEnumerator MultipleShakesRoutine(int count, float intensity, float halfDuration, float delay)
	{
		for (int i = 0; i < count; i++)
		{
			SingleShake(intensity, halfDuration);
			yield return new WaitForSeconds(delay);
		}
	}

	private IEnumerator ShakeRoutine(float halfDuration, Vector2 target)
	{
		for (float i = 0f; i < halfDuration; i += Time.deltaTime)
		{
			Vector3 vector = Vector3.Lerp(Vector3.zero, target, Mathf.SmoothStep(0f, 1f, i / halfDuration));
			base.transform.localPosition += vector;
			yield return null;
		}
		for (float i = halfDuration; i > 0f; i -= Time.deltaTime)
		{
			Vector3 vector2 = Vector3.Lerp(Vector3.zero, target, Mathf.SmoothStep(0f, 1f, i / halfDuration));
			base.transform.localPosition += vector2;
			yield return null;
		}
	}

	private void Update()
	{
		base.transform.localPosition = Vector3.zero;
	}
}
