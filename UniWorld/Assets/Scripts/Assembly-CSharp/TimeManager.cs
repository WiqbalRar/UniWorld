using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public static TimeManager instance;

	public AnimationCurve directionalLightIntensity;

	public Transform sun;

	[Tooltip("Raw Time of day")]
	public float time;

	[Tooltip("Normalized Time of day")]
	public float normalizedTime;

	public float maxDayDuration;

	[Tooltip("Daytime speed scale")]
	public float timeScale;

	public float sunPercentage;

	private void Awake()
	{
		if ((bool)instance)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
	}

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			if (ActionManager.GetActionDown("Toggle Day Night cycle"))
			{
				timeScale = ((timeScale == 0f) ? 1 : 0);
			}
			if (ActionManager.GetActionDown("Night"))
			{
				time = 0f;
			}
			if (ActionManager.GetActionDown("Day"))
			{
				time = maxDayDuration / 2f;
			}
			if (ActionManager.GetAction("Add Time"))
			{
				time += maxDayDuration / 10f * Time.deltaTime;
			}
			if (ActionManager.GetAction("Remove Time"))
			{
				time -= maxDayDuration / 10f * Time.deltaTime;
			}
		}
		time += Time.deltaTime * timeScale;
		if (time >= maxDayDuration)
		{
			time -= maxDayDuration;
		}
		if (time < 0f)
		{
			time += maxDayDuration;
		}
		normalizedTime = time / maxDayDuration;
		sunPercentage = directionalLightIntensity.Evaluate(normalizedTime);
		float t = ((!(normalizedTime >= 0.15f) || !(normalizedTime <= 0.95f)) ? 0f : ((normalizedTime - 0.15f) / 0.85f));
		sun.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(220f, -40f, t), 0f, 0f));
		Shader.SetGlobalFloat("_SunPercentage", sunPercentage);
	}
}
