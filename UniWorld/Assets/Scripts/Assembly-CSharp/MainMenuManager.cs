using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	[Header("Main Menu")]
	public GameObject mainMenu;

	[Header("World Selection")]
	public GameObject playMenu;

	public GameObject worldSelectionButton;

	public VerticalLayoutGroup verticalLayoutGroup;

	public RectTransform verticalLayoutGroupTransform;

	private List<KeyValuePair<GameObject, DateTime>> sortingList = new List<KeyValuePair<GameObject, DateTime>>();

	[Header("World Creation")]
	public TMP_InputField worldName;

	public TMP_InputField worldSeed;

	public TMP_Text errorText;

	[Header("Settings")]
	public GameObject settingsMenu;

	[Header("Controls")]
	public GameObject controlsMenu;

	public EventSystem system;

	private void Awake()
	{
		if (!PlayerPrefs.HasKey("RenderDistance"))
		{
			PlayerPrefs.SetInt("RenderDistance", 25);
			PlayerPrefs.Save();
		}
		if (!PlayerPrefs.HasKey("FieldOfView"))
		{
			PlayerPrefs.SetInt("FieldOfView", 90);
			PlayerPrefs.Save();
		}
		if (!File.Exists(Application.dataPath + "/Saves~/"))
		{
			Directory.CreateDirectory(Application.dataPath + "/Saves~/");
		}
	}

	public void Update()
	{
		if (!Input.GetKeyDown(KeyCode.Tab))
		{
			return;
		}
		Selectable selectable = null;
		if (!(system.currentSelectedGameObject != null))
		{
			return;
		}
		selectable = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
		if (selectable == null)
		{
			selectable = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
		}
		if (selectable != null)
		{
			InputField component = selectable.GetComponent<InputField>();
			if (component != null)
			{
				component.OnPointerClick(new PointerEventData(system));
			}
			system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(system));
		}
	}

	public void Play()
	{
		mainMenu.SetActive(value: false);
		playMenu.SetActive(value: true);
		string text = Application.dataPath + "/Saves~/";
		string[] directories = Directory.GetDirectories(text, "*", SearchOption.TopDirectoryOnly);
		string[] array = directories;
		foreach (string text2 in array)
		{
			string newSeed = ReadStringFromFile(text2 + "/seed.txt");
			GameObject gameObject = UnityEngine.Object.Instantiate(worldSelectionButton, base.transform);
			sortingList.Add(gameObject.GetComponent<WorldSelectionButton>().Initialize(text2.Remove(0, text.Length), newSeed, text2));
		}
		sortingList.Sort((KeyValuePair<GameObject, DateTime> kvp1, KeyValuePair<GameObject, DateTime> kvp2) => kvp2.Value.CompareTo(kvp1.Value));
		for (int num = 0; num < sortingList.Count; num++)
		{
			sortingList[num].Key.transform.SetParent(verticalLayoutGroupTransform);
		}
		verticalLayoutGroupTransform.sizeDelta = new Vector2(0f, 130 * directories.Length);
	}

	public void MainMenu()
	{
		foreach (KeyValuePair<GameObject, DateTime> sorting in sortingList)
		{
			UnityEngine.Object.Destroy(sorting.Key);
		}
		sortingList.Clear();
		playMenu.SetActive(value: false);
		settingsMenu.SetActive(value: false);
		controlsMenu.SetActive(value: false);
		mainMenu.SetActive(value: true);
	}

	public void Settings()
	{
		playMenu.SetActive(value: false);
		mainMenu.SetActive(value: false);
		settingsMenu.SetActive(value: true);
		controlsMenu.SetActive(value: false);
	}

	public void Controls()
	{
		playMenu.SetActive(value: false);
		mainMenu.SetActive(value: false);
		settingsMenu.SetActive(value: false);
		controlsMenu.SetActive(value: true);
	}

	public void CreateWorld()
	{
		if (worldName.text.Length == 0)
		{
			errorText.text = "World name missing";
			return;
		}
		if (HasIllegalFileNameChars(worldName.text))
		{
			errorText.text = "Characters  ";
			errorText.text += "\\ / : * ? \" < > | '";
			errorText.text += "\nare not allowed";
			return;
		}
		string text = Application.dataPath + "/Saves~/" + worldName.text + "/";
		if (Directory.Exists(text))
		{
			errorText.text = "World name already exists";
			return;
		}
		if (worldSeed.text.Length == 0)
		{
			errorText.text = "World seed missing";
			return;
		}
		errorText.text = "";
		Directory.CreateDirectory(text);
		WriteStringToFile(text + "seed.txt", worldSeed.text);
		int hashCode = worldSeed.text.GetHashCode();
		GameManager.instance.LoadWorld(worldName.text, hashCode);
	}

	private bool HasIllegalFileNameChars(string fileName)
	{
		return fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private string ReadStringFromFile(string filePath)
	{
		string text = "";
		using StreamReader streamReader = new StreamReader(filePath);
		return streamReader.ReadLine();
	}

	private void WriteStringToFile(string filePath, string seed)
	{
		using StreamWriter streamWriter = new StreamWriter(filePath);
		streamWriter.WriteLine(seed);
	}
}
