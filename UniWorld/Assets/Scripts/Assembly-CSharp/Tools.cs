using UnityEngine;

public static class Tools
{
	public static string CommaSeparator(int score)
	{
		if (score == 0)
		{
			return "0";
		}
		string text = "";
		while (score != 0)
		{
			string text2 = (score % 1000).ToString();
			if (score >= 1000)
			{
				for (int i = text2.Length; i < 3; i++)
				{
					text2 = text2.Insert(0, "0");
				}
			}
			text = text.Insert(0, text2);
			score -= score % 1000;
			score /= 1000;
			if (score == 0)
			{
				break;
			}
			text = text.Insert(0, ",");
		}
		return text;
	}

	public static void DrawSquare(Vector2 position, float size, Color color, bool drawInside, float duration = 0f)
	{
		DrawSquare(position, Vector2.one * size, color, drawInside, duration);
	}

	public static void DrawSquare(Vector2 position, Vector2 size, Color color, bool drawInside = true, float duration = 0f)
	{
		float num = size.x / 2f;
		float num2 = size.y / 2f;
		Debug.DrawLine(new Vector3(position.x - num, position.y + num2, 0f), new Vector3(position.x + num, position.y + num2, 0f), color, duration);
		Debug.DrawLine(new Vector3(position.x - num, position.y - num2, 0f), new Vector3(position.x + num, position.y - num2, 0f), color, duration);
		Debug.DrawLine(new Vector3(position.x - num, position.y - num2, 0f), new Vector3(position.x - num, position.y + num2, 0f), color, duration);
		Debug.DrawLine(new Vector3(position.x + num, position.y - num2, 0f), new Vector3(position.x + num, position.y + num2, 0f), color, duration);
		if (drawInside)
		{
			Debug.DrawLine(new Vector3(position.x - num, position.y + num2, 0f), new Vector3(position.x + num, position.y - num2, 0f), color, duration);
			Debug.DrawLine(new Vector3(position.x + num, position.y + num2, 0f), new Vector3(position.x - num, position.y - num2, 0f), color, duration);
		}
	}

	public static void DrawCube(Vector3 position, float size, Color color, float duration = 0f)
	{
		DrawCube(position, Vector3.one * size, color, duration);
	}

	public static void DrawCube(Vector3 position, Vector3 size, Color color, float duration = 0f)
	{
		float num = size.x / 2f;
		Vector3 vector = new Vector3(position.x - num, position.y - num, position.z - num);
		Vector3 vector2 = new Vector3(position.x - num, position.y - num, position.z + num);
		Vector3 vector3 = new Vector3(position.x - num, position.y + num, position.z - num);
		Vector3 vector4 = new Vector3(position.x - num, position.y + num, position.z + num);
		Vector3 vector5 = new Vector3(position.x + num, position.y - num, position.z - num);
		Vector3 vector6 = new Vector3(position.x + num, position.y - num, position.z + num);
		Vector3 vector7 = new Vector3(position.x + num, position.y + num, position.z - num);
		Vector3 vector8 = new Vector3(position.x + num, position.y + num, position.z + num);
		Debug.DrawLine(vector, vector2, color, duration);
		Debug.DrawLine(vector2, vector6, color, duration);
		Debug.DrawLine(vector6, vector5, color, duration);
		Debug.DrawLine(vector5, vector, color, duration);
		Debug.DrawLine(vector3, vector4, color, duration);
		Debug.DrawLine(vector4, vector8, color, duration);
		Debug.DrawLine(vector8, vector7, color, duration);
		Debug.DrawLine(vector7, vector3, color, duration);
		Debug.DrawLine(vector, vector3, color, duration);
		Debug.DrawLine(vector2, vector4, color, duration);
		Debug.DrawLine(vector6, vector8, color, duration);
		Debug.DrawLine(vector5, vector7, color, duration);
	}

	public static void DrawVoxelChunk(Vector3 position, int width, int height, Color color, float duration = 0f)
	{
		float num = (float)width / 2f;
		Vector3 vector = new Vector3(position.x - num, 0f, position.z - num);
		Vector3 vector2 = new Vector3(position.x - num, 0f, position.z + num);
		Vector3 vector3 = new Vector3(position.x - num, height, position.z - num);
		Vector3 vector4 = new Vector3(position.x - num, height, position.z + num);
		Vector3 vector5 = new Vector3(position.x + num, 0f, position.z - num);
		Vector3 vector6 = new Vector3(position.x + num, 0f, position.z + num);
		Vector3 vector7 = new Vector3(position.x + num, height, position.z - num);
		Vector3 vector8 = new Vector3(position.x + num, height, position.z + num);
		Debug.DrawLine(vector, vector2, color, duration);
		Debug.DrawLine(vector2, vector6, color, duration);
		Debug.DrawLine(vector6, vector5, color, duration);
		Debug.DrawLine(vector5, vector, color, duration);
		Debug.DrawLine(vector3, vector4, color, duration);
		Debug.DrawLine(vector4, vector8, color, duration);
		Debug.DrawLine(vector8, vector7, color, duration);
		Debug.DrawLine(vector7, vector3, color, duration);
		Debug.DrawLine(vector, vector3, color, duration);
		Debug.DrawLine(vector2, vector4, color, duration);
		Debug.DrawLine(vector6, vector8, color, duration);
		Debug.DrawLine(vector5, vector7, color, duration);
	}

	public static int RandomSign()
	{
		if (!(Random.value < 0.5f))
		{
			return 1;
		}
		return -1;
	}
}
