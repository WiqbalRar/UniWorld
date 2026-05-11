using UnityEngine;

public class TickManager : MonoBehaviour
{
	public static TickManager instance;

	public int currentTick;

	public float ticksPerSecond = 20f;

	public float tickDuration;

	[Header("water")]
	public int requiredTicksForWaterUpdate = 5;

	private int nextWaterUpdateTick = 5;

	public bool mustUpdateWater;

	[Header("Update")]
	public int requiresTicksForBlockUpdate = 1;

	private int nextBlockUpdateTicks = 1;

	public bool mustUpdateBlock;

	public float lastTickTime;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
		tickDuration = 1f / ticksPerSecond;
	}

	public void UpdateTicks()
	{
		float time = Time.time;
		if (time - lastTickTime >= tickDuration)
		{
			currentTick++;
			if (currentTick == nextWaterUpdateTick)
			{
				mustUpdateWater = true;
				nextWaterUpdateTick = currentTick + requiredTicksForWaterUpdate;
			}
			if (currentTick == nextBlockUpdateTicks)
			{
				mustUpdateBlock = true;
				nextBlockUpdateTicks = currentTick + requiresTicksForBlockUpdate;
			}
			lastTickTime = time;
		}
	}
}
