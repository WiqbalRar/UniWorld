using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyEditor : MonoBehaviour
{
	public GameObject keyBindUI;

	public RectTransform verticalLayoutGroup;

	private List<GameObject> keybindUIList = new List<GameObject>();

	private KeyBindUI selectedKey;

	private int mouseX;

	private int mouseY;

	public Slider mouseSensitivitySlider;

	public TMP_InputField mouseSensitivityInputField;

	[DllImport("user32.dll")]
	public static extern bool SetCursorPos(int X, int Y);

	private void OnEnable()
	{
		DisplayKeyBinds();
	}

	private void DisplayKeyBinds()
	{
		foreach (KeyValuePair<string, KeyCode> item in KeyCodeManager.instance.actionKey)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(keyBindUI, verticalLayoutGroup);
			keybindUIList.Add(gameObject);
			gameObject.GetComponent<KeyBindUI>().Initialize(item.Key, item.Value, this);
		}
		verticalLayoutGroup.sizeDelta = new Vector2(0f, 65 * keybindUIList.Count);
		float num = 1f;
		if (PlayerPrefs.HasKey("MouseSensitivity"))
		{
			num = PlayerPrefs.GetFloat("MouseSensitivity");
		}
		else
		{
			PlayerPrefs.SetFloat("MouseSensitivity", num);
			PlayerPrefs.Save();
		}
		UpdateMouseSensitivity(num);
	}

	private void OnDisable()
	{
		foreach (GameObject keybindUI in keybindUIList)
		{
			UnityEngine.Object.Destroy(keybindUI);
		}
		keybindUIList.Clear();
	}

	public void KeySelected(KeyBindUI selectedKeyBindUI)
	{
		mouseX = (int)Input.mousePosition.x;
		mouseY = Screen.height - (int)Input.mousePosition.y;
		Cursor.lockState = CursorLockMode.Locked;
		selectedKey = selectedKeyBindUI;
		selectedKeyBindUI.keyName.text = "> " + selectedKeyBindUI.keyName.text + " <";
		selectedKeyBindUI.keyName.color = Color.cyan;
	}

	private void Update()
	{
		if (selectedKey != null)
		{
			ListenForKeyPress();
		}
	}

	private void ListenForKeyPress()
	{
		foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(value))
			{
				ChangeKeyBind(value);
				break;
			}
		}
	}

	private void ChangeKeyBind(KeyCode newKey)
	{
		Cursor.lockState = CursorLockMode.None;
		SetCursorPos(mouseX, mouseY);
		KeyCodeManager.instance.actionKey[selectedKey.actionName.text] = newKey;
		KeyCodeManager.instance.SaveKeyBinds(KeyCodeManager.instance.actionKey, Application.dataPath + "/keybinds.cfg");
		selectedKey.keyName.text = newKey.ToString();
		selectedKey.keyName.color = Color.white;
		selectedKey = null;
	}

	public void ResetKeyBinds()
	{
		KeyCodeManager.instance.ResetKeyBinds();
		foreach (GameObject keybindUI in keybindUIList)
		{
			UnityEngine.Object.Destroy(keybindUI);
		}
		keybindUIList.Clear();
		DisplayKeyBinds();
	}

	public void MouseSensitivitySliderChanged()
	{
		float num = Mathf.Round(mouseSensitivitySlider.value * 100f) / 100f;
		PlayerPrefs.SetFloat("MouseSensitivity", num);
		PlayerPrefs.Save();
		UpdateMouseSensitivity(num);
	}

	public void MouseSensitivityInputFieldChanged()
	{
		if (!float.TryParse(mouseSensitivityInputField.text, out var result))
		{
			return;
		}
		PlayerPrefs.SetFloat("MouseSensitivity", result);
		PlayerPrefs.Save();
		if (result <= 3f)
		{
			if (mouseSensitivitySlider.maxValue > 3f)
			{
				mouseSensitivitySlider.maxValue = 3f;
			}
		}
		else if (result < mouseSensitivitySlider.maxValue)
		{
			mouseSensitivitySlider.maxValue = result;
		}
		UpdateMouseSensitivity(result);
	}

	private void UpdateMouseSensitivity(float mouseSensitivity)
	{
		if (mouseSensitivitySlider.maxValue < mouseSensitivity)
		{
			mouseSensitivitySlider.maxValue = mouseSensitivity;
		}
		mouseSensitivitySlider.SetValueWithoutNotify(mouseSensitivity);
		mouseSensitivityInputField.SetTextWithoutNotify($"{mouseSensitivity:0.00}");
	}
}
