using UnityEngine;

public static class MeshHelper
{
	public static Vector3[] blockVertices = new Vector3[8]
	{
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, 0.5f)
	};

	public static Vector2[] uv = new Vector2[4]
	{
		new Vector2(0f, 0f),
		new Vector2(0f, 1f),
		new Vector2(1f, 0f),
		new Vector2(1f, 1f)
	};

	public static Vector3[] stairVertices = new Vector3[27]
	{
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(0f, -0.5f, -0.5f),
		new Vector3(0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f, 0f),
		new Vector3(0f, -0.5f, 0f),
		new Vector3(0.5f, -0.5f, 0f),
		new Vector3(-0.5f, -0.5f, 0.5f),
		new Vector3(0f, -0.5f, 0.5f),
		new Vector3(0.5f, -0.5f, 0.5f),
		new Vector3(-0.5f, 0f, -0.5f),
		new Vector3(0f, 0f, -0.5f),
		new Vector3(0.5f, 0f, -0.5f),
		new Vector3(-0.5f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0.5f, 0f, 0f),
		new Vector3(-0.5f, 0f, 0.5f),
		new Vector3(0f, 0f, 0.5f),
		new Vector3(0.5f, 0f, 0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(0f, 0.5f, -0.5f),
		new Vector3(0.5f, 0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, 0f),
		new Vector3(0f, 0.5f, 0f),
		new Vector3(0.5f, 0.5f, 0f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(0f, 0.5f, 0.5f),
		new Vector3(0.5f, 0.5f, 0.5f)
	};

	public static Quaternion[] stairUvRotationsBot = new Quaternion[8]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 270f)
	};

	public static Quaternion[] stairUvRotationsTop = new Quaternion[8]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 90f)
	};

	public static Quaternion[] stairUvRotationsSide = new Quaternion[8]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 180f)
	};

	public static Vector3[] uvCoords = new Vector3[9]
	{
		new Vector3(-0.5f, -0.5f, 0f),
		new Vector3(0f, -0.5f, 0f),
		new Vector3(0.5f, -0.5f, 0f),
		new Vector3(-0.5f, 0f, 0f),
		new Vector3(0f, 0f, 0f),
		new Vector3(0.5f, 0f, 0f),
		new Vector3(-0.5f, 0.5f, 0f),
		new Vector3(0f, 0.5f, 0f),
		new Vector3(0.5f, 0.5f, 0f)
	};

	public static Quaternion[] slabRotation = new Quaternion[6]
	{
		Quaternion.Euler(-90f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(180f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Quaternion[] slabUvRotationsBot = new Quaternion[6]
	{
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Quaternion[] slabUvRotationsTop = new Quaternion[6]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, -90f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Quaternion[] slabUvRotationsLeft = new Quaternion[6]
	{
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Quaternion[] slabUvRotationsRight = new Quaternion[6]
	{
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Quaternion[] slabUvRotationsFront = new Quaternion[6]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Quaternion[] slabUvRotationsBack = new Quaternion[6]
	{
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 270f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(0f, 0f, 180f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Vector3[] torchVertices = new Vector3[8]
	{
		new Vector3(-0.0625f, -0.3125f, -0.0625f),
		new Vector3(0.0625f, -0.3125f, -0.0625f),
		new Vector3(-0.0625f, -0.3125f, 0.0625f),
		new Vector3(0.0625f, -0.3125f, 0.0625f),
		new Vector3(-0.0625f, 0.3125f, -0.0625f),
		new Vector3(0.0625f, 0.3125f, -0.0625f),
		new Vector3(-0.0625f, 0.3125f, 0.0625f),
		new Vector3(0.0625f, 0.3125f, 0.0625f)
	};

	public static Quaternion[] torchRotation = new Quaternion[6]
	{
		Quaternion.Euler(-30f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 30f),
		Quaternion.Euler(30f, 0f, 0f),
		Quaternion.Euler(0f, 0f, -30f),
		Quaternion.Euler(180f, 0f, 0f),
		Quaternion.Euler(0f, 0f, 0f)
	};

	public static Vector3[] torchTranslation = new Vector3[6]
	{
		new Vector3(0f, 0f, 0.425f),
		new Vector3(0.425f, 0f, 0f),
		new Vector3(0f, 0f, -0.425f),
		new Vector3(-0.425f, 0f, 0f),
		new Vector3(0f, 0.1875f, 0f),
		new Vector3(0f, -0.1875f, 0f)
	};

	public static Vector3[] plantVertices = new Vector3[8]
	{
		new Vector3(0.146f, 0f, 0.146f),
		new Vector3(0.854f, 0f, 0.146f),
		new Vector3(0.146f, 0f, 0.854f),
		new Vector3(0.854f, 0f, 0.854f),
		new Vector3(0.146f, 1f, 0.146f),
		new Vector3(0.854f, 1f, 0.146f),
		new Vector3(0.146f, 1f, 0.854f),
		new Vector3(0.854f, 1f, 0.854f)
	};

	public static Vector3[] grassPlantVertices = new Vector3[8]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(1f, 0f, 0f),
		new Vector3(0f, 0f, 1f),
		new Vector3(1f, 0f, 1f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1f, 1f, 0f),
		new Vector3(0f, 1f, 1f),
		new Vector3(1f, 1f, 1f)
	};
}
