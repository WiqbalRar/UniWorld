using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSelectionButton : MonoBehaviour
{
	public string worldName;

	public string worldPath;

	public string seed;

	public TMP_Text nameText;

	public TMP_Text seedText;

	public TMP_Text dateTimeText;

	public Image thumbnail;

	public GameObject worldDataUI;

	public GameObject deleteUI;

	public GameObject deleteButton;

	public KeyValuePair<GameObject, DateTime> Initialize(string newName, string newSeed, string newWorldPath)
	{
		worldName = newName;
		worldPath = newWorldPath;
		seed = newSeed;
		nameText.text = worldName;
		seedText.text = "\"" + seed + "\"";
		if (File.Exists(worldPath + "/Thumbnail.png"))
		{
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(File.ReadAllBytes(worldPath + "/Thumbnail.png"));
			thumbnail.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
		}
		DateTime value;
		if (File.Exists(worldPath + "/player.dat"))
		{
			value = new FileInfo(worldPath + "/player.dat").LastWriteTime;
			dateTimeText.text = value.ToString("d/M/yyyy  H:mm:ss");
		}
		else
		{
			value = DateTime.UnixEpoch;
		}
		return new KeyValuePair<GameObject, DateTime>(base.gameObject, value);
	}

	public void OnClick()
	{
		if (deleteButton.activeInHierarchy)
		{
			GameManager.instance.LoadWorld(worldName, seed.GetHashCode());
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("Mouse exit UI element");
	}

	public void TrashClicked()
	{
		deleteButton.SetActive(value: false);
		worldDataUI.SetActive(value: false);
		deleteUI.SetActive(value: true);
	}

	public void DeleteYes()
	{
		Directory.Delete(worldPath, recursive: true);
		MainMenuManager mainMenuManager = UnityEngine.Object.FindObjectOfType<MainMenuManager>();
		mainMenuManager.MainMenu();
		mainMenuManager.Play();
	}

	public void DeleteNo()
	{
		deleteUI.SetActive(value: false);
		worldDataUI.SetActive(value: true);
		deleteButton.SetActive(value: true);
	}
}
