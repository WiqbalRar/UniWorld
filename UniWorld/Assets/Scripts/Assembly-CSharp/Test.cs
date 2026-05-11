using UnityEngine;

public class Test : MonoBehaviour
{
	public int[] weights;

	public int[] summedWeights;

	public int valueToFind;

	[ContextMenu("Setup")]
	public void Setup()
	{
		summedWeights = new int[weights.Length];
		int num = 0;
		for (int i = 0; i < weights.Length; i++)
		{
			num += weights[i];
			summedWeights[i] = num;
		}
	}

	[ContextMenu("Test")]
	public void test()
	{
		MonoBehaviour.print(GoCheck());
	}

	private int GoCheck()
	{
		int num = 0;
		int num2 = weights.Length - 1;
		while (num <= num2)
		{
			int num3 = (num + num2) / 2;
			if (summedWeights[num3] > valueToFind)
			{
				num2 = num3 - 1;
			}
			else
			{
				num = num3 + 1;
			}
		}
		return num;
	}
}
