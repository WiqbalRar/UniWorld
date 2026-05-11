using UnityEngine;

public static class ActionManager
{
	public static bool GetAction(string actionName)
	{
		return Input.GetKey(KeyCodeManager.instance.actionKey[actionName]);
	}

	public static bool GetActionDown(string actionName)
	{
		return Input.GetKeyDown(KeyCodeManager.instance.actionKey[actionName]);
	}

	public static bool GetActionUp(string actionName)
	{
		return Input.GetKeyUp(KeyCodeManager.instance.actionKey[actionName]);
	}
}
