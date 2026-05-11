using UnityEngine;

public class RenderTextureManager : MonoBehaviour
{
	[SerializeField]
	private Material setToMaterial;

	private void Awake()
	{
		RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 8);
		GetComponent<Camera>().targetTexture = renderTexture;
		setToMaterial.SetTexture("_BlendTex", renderTexture);
	}
}
