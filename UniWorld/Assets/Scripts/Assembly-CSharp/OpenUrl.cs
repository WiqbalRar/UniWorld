using UnityEngine;

public class OpenUrl : MonoBehaviour
{
	public string url;

	public void OpenLink()
	{
		switch (Application.platform)
		{
		case RuntimePlatform.WebGLPlayer:
			Application.ExternalEval("window.open(\"" + url + "\")");
			break;
		case RuntimePlatform.WindowsPlayer:
			Application.OpenURL(url);
			break;
		case RuntimePlatform.WindowsEditor:
			Debug.Log("Editorlink");
			Application.OpenURL(url);
			break;
		}
	}
}
