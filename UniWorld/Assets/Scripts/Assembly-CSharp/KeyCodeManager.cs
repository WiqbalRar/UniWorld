using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class KeyCodeManager : MonoBehaviour
{
	public static KeyCodeManager instance;

	public Dictionary<string, KeyCode> actionKey = new Dictionary<string, KeyCode>();

	[SerializeField]
	public StringKeyCodeDictionary defaultKey;

	public IDictionary<string, KeyCode> StringKeyCodeDictionary
	{
		get
		{
			return defaultKey;
		}
		set
		{
			defaultKey.CopyFrom(value);
		}
	}

	private void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!File.Exists(Application.dataPath + "/keybinds.cfg"))
		{
			SaveKeyBinds(defaultKey);
		}
		actionKey = LoadKeyBinds(Application.dataPath + "/keybinds.cfg");
		if (actionKey.Count != defaultKey.Count)
		{
			ResetKeyBinds();
		}
	}

	public void ResetKeyBinds()
	{
		SaveKeyBinds(defaultKey);
		actionKey = LoadKeyBinds();
	}

	public void SaveKeyBinds(Dictionary<string, KeyCode> dictionary, string filePath)
	{
		using StreamWriter streamWriter = File.CreateText(filePath);
		foreach (KeyValuePair<string, KeyCode> item in dictionary)
		{
			string key = item.Key;
			streamWriter.WriteLine(key + "," + item.Value);
		}
	}

	public void SaveKeyBinds(Dictionary<string, KeyCode> dictionary)
	{
		string filePath = Application.dataPath + "/keybinds.cfg";
		SaveKeyBinds(dictionary, filePath);
	}

	public Dictionary<string, KeyCode> LoadKeyBinds(string filePath)
	{
		Dictionary<string, KeyCode> dictionary = new Dictionary<string, KeyCode>();
		using StreamReader streamReader = File.OpenText(filePath);
		string text;
		while ((text = streamReader.ReadLine()) != null)
		{
			string[] array = text.Split(',');
			string key = array[0];
			KeyCode value = (KeyCode)Enum.Parse(typeof(KeyCode), array[1]);
			dictionary.Add(key, value);
		}
		return dictionary;
	}

	public Dictionary<string, KeyCode> LoadKeyBinds()
	{
		return LoadKeyBinds(Application.dataPath + "/keybinds.cfg");
	}
}
