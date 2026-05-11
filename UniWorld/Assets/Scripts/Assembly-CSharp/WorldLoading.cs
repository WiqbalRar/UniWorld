using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldLoading : MonoBehaviour
{
	public TMP_Text remainingTask;

	public TMP_Text loadingWorld;

	public string[] randomLines;

	public Image backGround;

	private void Start()
	{
		StartCoroutine(LoadingRoutine());
	}

	private IEnumerator LoadingRoutine()
	{
		while (ChunkManager.instance.chunkRequiredBeforePlay > ChunkManager.instance.chunkRenderCount || ChunkManager.instance.chunkLitRequiredBeforePlay > ChunkManager.instance.chunkLitCount)
		{
			string text = "Generating Chunks:\n" + ChunkManager.instance.chunkRenderCount + " / " + ChunkManager.instance.chunkRequiredBeforePlay + "\n" + Mathf.RoundToInt((float)ChunkManager.instance.chunkRenderCount / (float)ChunkManager.instance.chunkRequiredBeforePlay * 100f) + "%\n\n";
			text = text + "Baking Ligths:\n" + ChunkManager.instance.chunkLitCount + " / " + ChunkManager.instance.chunkLitRequiredBeforePlay + "\n" + Mathf.RoundToInt((float)ChunkManager.instance.chunkLitCount / (float)ChunkManager.instance.chunkLitRequiredBeforePlay * 100f) + "%";
			remainingTask.text = text;
			yield return null;
		}
		MonoBehaviour.print("Done, Chunks " + ChunkManager.instance.chunkRenderCount + " / " + ChunkManager.instance.chunkRequiredBeforePlay + "\nLights " + ChunkManager.instance.chunkLitCount + " / " + ChunkManager.instance.chunkLitRequiredBeforePlay + " Max block type: " + (byte)100);
		remainingTask.text = randomLines[Random.Range(0, randomLines.Length)];
		Object.FindObjectOfType<FpsController>().EnableMovement();
		yield return new WaitForSeconds(0.5f);
		remainingTask.text += "\n.";
		yield return new WaitForSeconds(0.5f);
		remainingTask.text += " .";
		yield return new WaitForSeconds(0.5f);
		remainingTask.text += " .";
		yield return new WaitForSeconds(0.5f);
		remainingTask.text = "";
		loadingWorld.text = "";
		StartCoroutine("FadeOut");
		Object.FindObjectOfType<FpsController>().canMove = true;
		Object.FindObjectOfType<CloudManager>().Initialize();
	}

	private IEnumerator FadeOut()
	{
		Color startColor = backGround.color;
		new Color(startColor.r, startColor.g, startColor.g, 0f);
		for (float i = 0f; i < 0.75f; i += Time.unscaledDeltaTime)
		{
			backGround.color = new Color(startColor.r, startColor.g, startColor.g, Mathf.SmoothStep(1f, 0f, i / 0.75f));
			yield return null;
		}
		Object.Destroy(base.gameObject);
	}
}
