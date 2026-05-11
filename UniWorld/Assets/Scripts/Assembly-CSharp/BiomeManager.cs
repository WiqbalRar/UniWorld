using UnityEngine;

public class BiomeManager : MonoBehaviour
{
	public static BiomeManager instance;

	public float[] temperatureIndexes;

	public float[] humidityIndexes;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(this);
		}
		instance = this;
	}

	public int GetTemperatureIndex(float temperature)
	{
		for (int i = 0; i < temperatureIndexes.Length; i++)
		{
			if (temperature < temperatureIndexes[i])
			{
				return i;
			}
		}
		return temperatureIndexes.Length;
	}

	public int GetHumidityIndex(float humidity)
	{
		for (int i = 0; i < humidityIndexes.Length; i++)
		{
			if (humidity < humidityIndexes[i])
			{
				return i;
			}
		}
		return humidityIndexes.Length;
	}

	public int GetbiomeIDFromValues(float temperatureValue, float humidityValue)
	{
		return GetBiomeIDFromIndex(GetTemperatureIndex(temperatureValue), GetHumidityIndex(humidityValue));
	}

	public int GetBiomeIDFromIndex(int temperatureIndex, int humidityIndex)
	{
		if (temperatureIndex == 0)
		{
			if (humidityIndex <= 1)
			{
				return 0;
			}
			if (humidityIndex <= 3)
			{
				return 1;
			}
			return 2;
		}
		if (temperatureIndex <= 1)
		{
			if (humidityIndex <= 2)
			{
				return 3;
			}
			return 4;
		}
		if (temperatureIndex <= 3)
		{
			if (humidityIndex == 0)
			{
				return 11;
			}
			if (humidityIndex <= 2)
			{
				return 5;
			}
			if (humidityIndex <= 4)
			{
				return 6;
			}
			return 7;
		}
		if (temperatureIndex == 4 && humidityIndex <= 3)
		{
			return 8;
		}
		if (temperatureIndex == 5 && humidityIndex <= 3)
		{
			return 10;
		}
		return 9;
	}
}
