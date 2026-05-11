using UnityEngine;

public class NoiseTest : MonoBehaviour
{
	[Header("Noise Repartition")]
	public int sampleCount;

	public FastNoiseUnity noise;

	public int slices = 20;

	public int[] results;

	public bool logResult;

	[Header("Generation duration")]
	private const int WIDTH = 16;

	private const int HEIGHT = 256;

	public int durationCount = 20;

	public int sampledScale = 1;

	[ContextMenu("Repartition per slice")]
	private void CalculateRepartitionPerSlice()
	{
		results = new int[slices];
		for (int i = 0; i < sampleCount; i++)
		{
			float num = noise.fastNoise.GetNoise(0f, 0f, i);
			float num2 = 2f / (float)slices;
			float num3 = -1f;
			for (int j = 0; j < slices; j++)
			{
				num3 += num2;
				if (num <= num3)
				{
					results[j]++;
					break;
				}
			}
		}
		if (logResult)
		{
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
			MonoBehaviour.print("=====================================================");
			float num4 = 2f / (float)slices;
			float num5 = -1f;
			for (int k = 0; k < slices; k++)
			{
				string text = "";
				text = text + "[" + Mathf.Round(num5 * 100f) / 100f + " To ";
				num5 += num4;
				float num6 = Mathf.Round(num5 * 100f) / 100f;
				text = text + num6 + "] = " + Mathf.Round((float)results[k] / (float)sampleCount * 100f * 1000f) / 1000f + " %";
				MonoBehaviour.print(text);
			}
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
		}
	}

	[ContextMenu("Repartition Total")]
	private void CalculateRepartitionTotal()
	{
		results = new int[slices];
		for (int i = 0; i < sampleCount; i++)
		{
			float num = noise.fastNoise.GetNoise(0f, 0f, i);
			float num2 = 2f / (float)slices;
			float num3 = -1f;
			for (int j = 0; j < slices; j++)
			{
				num3 += num2;
				if (num <= num3)
				{
					results[j]++;
					break;
				}
			}
		}
		if (logResult)
		{
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
			MonoBehaviour.print("=====================================================");
			float num4 = 2f / (float)slices;
			float num5 = -1f;
			float num6 = 0f;
			for (int k = 0; k < slices; k++)
			{
				string text = "";
				text = text + "[" + Mathf.Round(num5 * 100f) / 100f + " To ";
				num5 += num4;
				float num7 = Mathf.Round(num5 * 100f) / 100f;
				num6 += (float)results[k];
				text = text + num7 + "] = " + Mathf.Round(num6 / (float)sampleCount * 100f * 1000f) / 1000f + " %";
				MonoBehaviour.print(text);
			}
			Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
		}
	}
}
