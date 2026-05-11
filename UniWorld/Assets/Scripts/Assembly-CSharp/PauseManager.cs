using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	public GameObject playerUI;

	public GameObject pauseUI;

	public GameObject mainPauseUI;

	public GameObject settingsUI;

	public GameObject controlsUI;

	public GameObject loadingScreen;

	public bool isPaused;

	public Screenshot myScreenshot;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !PlayerInventory.instance.isOpen)
		{
			TogglePause();
		}
	}

	public void TogglePause()
	{
		if (loadingScreen != null)
		{
			return;
		}
		if (controlsUI.activeInHierarchy)
		{
			CloseControls();
			return;
		}
		if (settingsUI.activeInHierarchy)
		{
			SaveSettings();
			return;
		}
		isPaused = !isPaused;
		Cursor.lockState = ((!isPaused) ? CursorLockMode.Locked : CursorLockMode.None);
		pauseUI.SetActive(isPaused);
		mainPauseUI.SetActive(isPaused);
		playerUI.SetActive(!isPaused);
		if (settingsUI.activeInHierarchy)
		{
			ChunkManager.instance.UpdateChunkRadius();
		}
		settingsUI.SetActive(!isPaused);
		Time.timeScale = (isPaused ? 0f : 1f);
	}

	public void MainMenu()
	{
		myScreenshot.TakeScreenshot();
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}

	public void OpenSettings()
	{
		mainPauseUI.SetActive(value: false);
		settingsUI.SetActive(value: true);
	}

	public void OpenControls()
	{
		mainPauseUI.SetActive(value: false);
		controlsUI.SetActive(value: true);
	}

	public void CloseControls()
	{
		FPSCam fPSCam = Object.FindObjectOfType<FPSCam>();
		if (fPSCam != null)
		{
			fPSCam.UpdateMouseSensitivity();
		}
		mainPauseUI.SetActive(value: true);
		controlsUI.SetActive(value: false);
	}

	public void SaveSettings()
	{
		mainPauseUI.SetActive(value: true);
		settingsUI.SetActive(value: false);
		ChunkManager.instance.UpdateChunkRadius();
	}
}
