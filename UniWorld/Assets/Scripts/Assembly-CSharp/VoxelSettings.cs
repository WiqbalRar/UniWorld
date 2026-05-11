using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoxelSettings : MonoBehaviour
{
	[Header("Render distance")]
	public Slider renderDistanceSlider;

	public TMP_Text renderDistanceHandleValueText;

	[Header("Field of view")]
	public Slider fieldOfViewSlider;

	public TMP_Text fieldOfViewHandleValueText;

	private void OnEnable()
	{
		renderDistanceSlider.SetValueWithoutNotify(PlayerPrefs.GetInt("RenderDistance"));
		renderDistanceHandleValueText.text = renderDistanceSlider.value.ToString();
		fieldOfViewSlider.SetValueWithoutNotify(PlayerPrefs.GetInt("FieldOfView"));
		fieldOfViewHandleValueText.text = fieldOfViewSlider.value.ToString();
	}

	public void OnRenderDistanceChanged()
	{
		renderDistanceHandleValueText.text = renderDistanceSlider.value.ToString();
		PlayerPrefs.SetInt("RenderDistance", (int)renderDistanceSlider.value);
		PlayerPrefs.Save();
	}

	public void OnFieldOfViewChanged()
	{
		fieldOfViewHandleValueText.text = fieldOfViewSlider.value.ToString();
		PlayerPrefs.SetInt("FieldOfView", (int)fieldOfViewSlider.value);
		PlayerPrefs.Save();
		Camera[] array = Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].fieldOfView = fieldOfViewSlider.value;
		}
	}
}
