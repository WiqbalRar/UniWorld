using System.Collections.Generic;
using UnityEngine;

public class OutlineMeshGenerator : MonoBehaviour
{
	public float width;

	public float[] clampValues;

	public MeshFilter myFilter;

	private Mesh mesh;

	public Material outlineMat;

	public static OutlineMeshGenerator instance;

	private void Start()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		instance = this;
		myFilter = GetComponent<MeshFilter>();
		mesh = new Mesh();
		Object.DontDestroyOnLoad(this);
	}

	public void DrawOutline(byte voxelID, Vector3 outlinePosition, Vector3 camPosition, byte orientation, Vector3Int intPosition)
	{
		if (voxelID <= 0)
		{
			return;
		}
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		float value = width * Vector3.Distance(outlinePosition, camPosition);
		value = Mathf.Clamp(value, clampValues[0], clampValues[1]);
		Matrix4x4 matrix;
		if (voxelID >= 101 && voxelID <= 113)
		{
			matrix = Matrix4x4.TRS(outlinePosition, VoxelBounds.stairRotations[orientation], Vector3.one);
			AppendStairs(value, list, list2);
		}
		else if (voxelID >= 114 && voxelID <= 126)
		{
			matrix = Matrix4x4.TRS(outlinePosition, VoxelBounds.slabRotation[orientation], Vector3.one);
			AppendBounds(VoxelBounds.slabBounds[0], value, list, list2);
		}
		else if (voxelID >= 21 && voxelID <= 21)
		{
			matrix = Matrix4x4.TRS(outlinePosition + VoxelBounds.torchTranslation[orientation], Quaternion.identity, Vector3.one);
			AppendBounds(VoxelBounds.torchBounds[0], value, list, list2);
		}
		else
		{
			switch (voxelID)
			{
			case 1:
			{
				Chunk chunk2 = null;
				int voxelIndex2 = 0;
				Vector3Int voxelPosition2 = Vector3Int.zero;
				VoxelTools.GetBlockinfo(intPosition + WorldShifter.instance.offset, ref chunk2, ref voxelIndex2, ref voxelPosition2);
				_ = chunk2.noiseSetWhite1[voxelPosition2.x * Chunk.WIDTH + voxelPosition2.z];
				matrix = Matrix4x4.TRS(outlinePosition, Quaternion.identity, Vector3.one);
				AppendBounds(VoxelBounds.grassPlantBounds, value, list, list2);
				break;
			}
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			{
				Chunk chunk = null;
				int voxelIndex = 0;
				Vector3Int voxelPosition = Vector3Int.zero;
				VoxelTools.GetBlockinfo(intPosition + WorldShifter.instance.offset, ref chunk, ref voxelIndex, ref voxelPosition);
				float num = chunk.noiseSetWhite1[voxelPosition.x * Chunk.WIDTH + voxelPosition.z];
				Vector3 vector = new Vector3(num * 0.3125f, 0f, num % 0.1f * 3.125f);
				matrix = Matrix4x4.TRS(outlinePosition + vector, Quaternion.identity, Vector3.one);
				AppendBounds(VoxelBounds.plantBounds, value, list, list2);
				break;
			}
			default:
				matrix = Matrix4x4.TRS(outlinePosition, Quaternion.identity, Vector3.one);
				AppendBlock(value, list, list2);
				break;
			}
		}
		mesh.Clear();
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		Graphics.DrawMesh(mesh, matrix, outlineMat, 0);
	}

	[ContextMenu("Buyild abd Show")]
	public void BuildAndShow()
	{
		Mesh mesh = new Mesh();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		AppendBounds(VoxelBounds.torchBounds[0], width, list, list2);
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		myFilter.mesh = mesh;
	}

	private void AppendBlock(float _width, List<Vector3> vertices, List<int> triangles)
	{
		AppendSegment(MeshHelper.blockVertices[0], MeshHelper.blockVertices[1], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[1], MeshHelper.blockVertices[3], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[3], MeshHelper.blockVertices[2], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[2], MeshHelper.blockVertices[0], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[4], MeshHelper.blockVertices[5], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[5], MeshHelper.blockVertices[7], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[7], MeshHelper.blockVertices[6], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[6], MeshHelper.blockVertices[4], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[0], MeshHelper.blockVertices[4], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[1], MeshHelper.blockVertices[5], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[3], MeshHelper.blockVertices[7], _width, vertices, triangles);
		AppendSegment(MeshHelper.blockVertices[2], MeshHelper.blockVertices[6], _width, vertices, triangles);
	}

	private void AppendStairs(float _width, List<Vector3> vertices, List<int> triangles)
	{
		AppendSegment(MeshHelper.stairVertices[0], MeshHelper.stairVertices[2], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[2], MeshHelper.stairVertices[8], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[8], MeshHelper.stairVertices[6], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[6], MeshHelper.stairVertices[0], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[0], MeshHelper.stairVertices[9], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[2], MeshHelper.stairVertices[11], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[8], MeshHelper.stairVertices[17], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[6], MeshHelper.stairVertices[15], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[0], MeshHelper.stairVertices[9], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[2], MeshHelper.stairVertices[11], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[8], MeshHelper.stairVertices[26], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[6], MeshHelper.stairVertices[24], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[9], MeshHelper.stairVertices[11], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[11], MeshHelper.stairVertices[14], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[14], MeshHelper.stairVertices[12], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[12], MeshHelper.stairVertices[9], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[12], MeshHelper.stairVertices[21], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[14], MeshHelper.stairVertices[23], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[21], MeshHelper.stairVertices[23], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[23], MeshHelper.stairVertices[26], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[26], MeshHelper.stairVertices[24], _width, vertices, triangles);
		AppendSegment(MeshHelper.stairVertices[24], MeshHelper.stairVertices[21], _width, vertices, triangles);
	}

	private void AppendBounds(Bounds boundToAppend, float _width, List<Vector3> vertices, List<int> triangles)
	{
		Vector3 min = boundToAppend.min;
		Vector3 max = boundToAppend.max;
		Vector3 vector = min;
		Vector3 vector2 = new Vector3(max.x, min.y, min.z);
		Vector3 vector3 = new Vector3(min.x, min.y, max.z);
		Vector3 vector4 = new Vector3(max.x, min.y, max.z);
		Vector3 vector5 = new Vector3(min.x, max.y, min.z);
		Vector3 vector6 = new Vector3(max.x, max.y, min.z);
		Vector3 vector7 = new Vector3(min.x, max.y, max.z);
		Vector3 vector8 = max;
		AppendSegment(vector, vector2, _width, vertices, triangles);
		AppendSegment(vector2, vector4, _width, vertices, triangles);
		AppendSegment(vector4, vector3, _width, vertices, triangles);
		AppendSegment(vector3, vector, _width, vertices, triangles);
		AppendSegment(vector5, vector6, _width, vertices, triangles);
		AppendSegment(vector6, vector8, _width, vertices, triangles);
		AppendSegment(vector8, vector7, _width, vertices, triangles);
		AppendSegment(vector7, vector5, _width, vertices, triangles);
		AppendSegment(vector, vector5, _width, vertices, triangles);
		AppendSegment(vector2, vector6, _width, vertices, triangles);
		AppendSegment(vector4, vector8, _width, vertices, triangles);
		AppendSegment(vector3, vector7, _width, vertices, triangles);
	}

	private void AppendSegment(Vector3 start, Vector3 end, float _width, List<Vector3> vertices, List<int> triangles)
	{
		Vector3 normalized = (end - start).normalized;
		Vector3 vector = normalized;
		float num = Vector3.Dot(vector, Vector3.up);
		if (num == 1f)
		{
			vector = Vector3.up;
		}
		else if (num == -1f)
		{
			vector = Vector3.down;
		}
		num = Vector3.Dot(vector, Vector3.up);
		Vector3 vector2 = ((num == 1f) ? Vector3.right : ((num != -1f) ? (Quaternion.LookRotation(normalized) * Vector3.right) : Vector3.left));
		Vector3 normalized2 = Vector3.Cross(vector, vector2).normalized;
		vector2 *= _width;
		normalized2 *= _width;
		vector *= _width;
		int count = vertices.Count;
		vertices.Add(start - vector2 - normalized2 - vector);
		vertices.Add(start + vector2 - normalized2 - vector);
		vertices.Add(end - vector2 - normalized2 + vector);
		vertices.Add(end + vector2 - normalized2 + vector);
		vertices.Add(start - vector2 + normalized2 - vector);
		vertices.Add(start + vector2 + normalized2 - vector);
		vertices.Add(end - vector2 + normalized2 + vector);
		vertices.Add(end + vector2 + normalized2 + vector);
		triangles.Add(count);
		triangles.Add(count + 4);
		triangles.Add(count + 5);
		triangles.Add(count);
		triangles.Add(count + 5);
		triangles.Add(count + 1);
		triangles.Add(count + 1);
		triangles.Add(count + 5);
		triangles.Add(count + 7);
		triangles.Add(count + 1);
		triangles.Add(count + 7);
		triangles.Add(count + 3);
		triangles.Add(count + 3);
		triangles.Add(count + 7);
		triangles.Add(count + 6);
		triangles.Add(count + 3);
		triangles.Add(count + 6);
		triangles.Add(count + 2);
		triangles.Add(count + 2);
		triangles.Add(count + 6);
		triangles.Add(count + 4);
		triangles.Add(count + 2);
		triangles.Add(count + 4);
		triangles.Add(count);
		triangles.Add(count + 4);
		triangles.Add(count + 6);
		triangles.Add(count + 7);
		triangles.Add(count + 4);
		triangles.Add(count + 7);
		triangles.Add(count + 5);
		triangles.Add(count);
		triangles.Add(count + 3);
		triangles.Add(count + 2);
		triangles.Add(count);
		triangles.Add(count + 1);
		triangles.Add(count + 3);
	}
}
