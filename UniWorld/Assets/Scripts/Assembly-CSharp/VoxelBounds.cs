using UnityEngine;

public static class VoxelBounds
{
	public static Bounds[] stairBounds = new Bounds[2]
	{
		new Bounds(new Vector3(0f, -0.25f, -0.25f), new Vector3(1f, 0.5f, 0.5f)),
		new Bounds(new Vector3(0f, 0f, 0.25f), new Vector3(1f, 1f, 0.5f))
	};

	public static Quaternion[] stairRotations = new Quaternion[8]
	{
		Quaternion.Euler(0f, 0f, 0f),
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f),
		Quaternion.Euler(-180f, 180f, 0f),
		Quaternion.Euler(-180f, 270f, 0f),
		Quaternion.Euler(-180f, 0f, 0f),
		Quaternion.Euler(-180f, 90f, 0f)
	};

	public static Bounds[] slabBounds = new Bounds[1]
	{
		new Bounds(new Vector3(0f, -0.25f, 0f), new Vector3(1f, 0.5f, 1f))
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

	public static Bounds[] torchBounds = new Bounds[1]
	{
		new Bounds(new Vector3(0f, 0f, 0f), new Vector3(0.25f, 0.6875f, 0.25f))
	};

	public static Vector3[] torchTranslation = new Vector3[6]
	{
		new Vector3(0f, 0f, 0.375f),
		new Vector3(0.375f, 0f, 0f),
		new Vector3(0f, 0f, -0.375f),
		new Vector3(-0.375f, 0f, 0f),
		new Vector3(0f, 5f / 32f, 0f),
		new Vector3(0f, -5f / 32f, 0f)
	};

	public static Bounds grassPlantBounds = new Bounds(new Vector3(0f, -0.125f, 0f), new Vector3(0.75f, 0.75f, 0.75f));

	public static Bounds plantBounds = new Bounds(new Vector3(0f, -0.1875f, 0f), new Vector3(0.375f, 0.625f, 0.375f));
}
