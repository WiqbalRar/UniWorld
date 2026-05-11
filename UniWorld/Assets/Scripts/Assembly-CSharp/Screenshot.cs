using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
	private int width = 256;

	private int height = 256;

	public Camera screenshotCamera;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			TakeScreenshot();
		}
	}

	public void TakeScreenshot()
	{
		RenderTexture renderTexture = new RenderTexture(width, height, 24);
		screenshotCamera.targetTexture = renderTexture;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
		screenshotCamera.Render();
		RenderTexture.active = renderTexture;
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		screenshotCamera.targetTexture = null;
		RenderTexture.active = null;
		Object.Destroy(renderTexture);
		byte[] bytes = texture2D.EncodeToPNG();
		File.WriteAllBytes(StorageManager.savePath + "Thumbnail.png", bytes);
	}
}
