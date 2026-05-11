using TMPro;
using UnityEngine;

public class KeyBindUI : MonoBehaviour
{
	public TMP_Text actionName;

	public TMP_Text keyName;

	public KeyEditor myKeyEditor;

	public void Initialize(string actionName, KeyCode key, KeyEditor myKeyEditor)
	{
		this.actionName.text = actionName;
		keyName.text = key.ToString();
		this.myKeyEditor = myKeyEditor;
	}

	public void OnButtonClick()
	{
		myKeyEditor.KeySelected(this);
	}
}
