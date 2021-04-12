using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartStructer
{
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public Mesh mesh;
	public GameObject obj;

	public void Clear()
	{
		if(mesh != null)
			GameObject.Destroy(mesh);
		if (mesh != null)
			GameObject.Destroy(meshRenderer.material);
		if (meshFilter != null)
			GameObject.Destroy(meshFilter);
		if (meshRenderer != null)
			GameObject.Destroy(meshRenderer);
		if (obj != null)
			GameObject.Destroy(obj);

		mesh = null;
		meshFilter = null;
		meshRenderer = null;
		obj = null;
	}

}

public class ApartmentDong : MonoBehaviour
{
	public Dongdata dongdata;
	public List<PartStructer> structreList;
	public Material material;
	
	public void SetDong(Dongdata data, Material material)
	{
		dongdata = data;
		this.material = material;
		structreList = new List<PartStructer>();
		CreateStructre();
	}

	public void CreateStructre()
	{		
		for(int roomtypeIdx = 0; roomtypeIdx < dongdata.roomtypes.Length;++roomtypeIdx)
		{
			for(int i =0; i< dongdata.roomtypes[roomtypeIdx].vectorList.Count; ++i)
			{
				PartStructer data = new PartStructer();
				data.obj = new GameObject(dongdata.roomtypes[roomtypeIdx].meta.룸타입id.ToString());
				data.mesh = new Mesh();
				data.meshFilter = data.obj.AddComponent<MeshFilter>();
				CreateMesh(data, dongdata.roomtypes[roomtypeIdx].vectorList[i]);
				SetUv(data, dongdata.roomtypes[roomtypeIdx].meta, 
					  dongdata.roomtypes[roomtypeIdx].vectorList[i]);
				data.obj.transform.parent = this.gameObject.transform;
				structreList.Add(data);
			}
		}
	}

	private bool CreateMesh(PartStructer part, List<Vector3> vetextList)
	{
		try
		{
			if (part == null)
			{
				return false;
			}
			if (vetextList == null || vetextList.Count < 0)
			{
				return false;
			}
			if (part.mesh == null)
			{
				part.mesh = new Mesh();
			}
			part.mesh.vertices = vetextList.ToArray();
			List<int> triangleList = new List<int>();
			triangleList.Clear();
			for (int i = 0; i < vetextList.ToArray().Length; ++i)
			{
				triangleList.Add(i);
			}

			part.mesh.triangles = triangleList.ToArray();
			part.mesh.RecalculateNormals();
			part.mesh.RecalculateBounds();
			part.meshFilter.mesh = part.mesh;
			return true;
		}
		catch
		{
			Debug.LogError("ApartmentDong.CreateMesh fail");
			return false;
		}
		
	}
	private void SetUv(PartStructer part, Meta_1 meta_1, List<Vector3> vetextList)
	{
		Vector2[] uvs = new Vector2[vetextList.Count];
		if (part.meshRenderer == null)
		{
			part.meshRenderer = part.obj.AddComponent<MeshRenderer>();
		}
		part.meshRenderer.material = new Material(material);

		for (int i = 0; i < uvs.Length;)
		{
			float Angle = GetAngleBetweenVector(part.mesh.normals[i],
												Vector3.forward,
												Vector3.up);
			float u = 0;
			float v = 0;	
			if (part.mesh.normals[i] == Vector3.up ||
				part.mesh.normals[i] == Vector3.down)
			{
				u = 0.75f;
				v = 0;

				uvs[i] = new Vector2(u, v + 0.5f);
				uvs[++i] = new Vector2(u + 0.25f, v);
				uvs[++i] = new Vector2(u, v);

				uvs[++i] = new Vector2(u, v + 0.5f);
				uvs[++i] = new Vector2(u + 0.25f, v + 0.5f);
				uvs[++i] = new Vector2(u + 0.25f, v);
			}
			else if (Angle <= 220 && Angle <= 180)
			{
				uvs[i] = new Vector2(u + 0.5f, v);
				uvs[++i] = new Vector2(u, v);
				uvs[++i] = new Vector2(u, v + 0.5f);

				uvs[++i] = new Vector2(u, v + 0.5f);
				uvs[++i] = new Vector2(u + 0.5f, v + 0.5f);
				uvs[++i] = new Vector2(u + 0.5f, v);
			}
			else
			{
				u = 0.5f;
				v = 0f;

				uvs[i] = new Vector2(u + 0.25f, v);
				uvs[++i] = new Vector2(u, v);
				uvs[++i] = new Vector2(u, v + 0.75f);

				uvs[++i] = new Vector2(u, v + 0.75f);
				uvs[++i] = new Vector2(u + 0.25f, v + 0.75f);
				uvs[++i] = new Vector2(u + 0.25f, v);
			}
			++i;
		}
		part.meshRenderer.material.SetTextureScale("_BaseMap", new Vector2(1f, (int)(part.mesh.bounds.size.y / 3)));
		part.mesh.uv = uvs;
	}
	private float GetAngleBetweenVector(Vector3 a, Vector3 b, Vector3 n)
	{
		float angle = Vector3.Angle(a, b);
		float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));	
		float signed_angle = angle * sign;
		float angle360 = (signed_angle + 180) % 360;

		return angle360;
	}

	public void Clear()
	{
		if(structreList != null)
		{
			for (int i = 0; i < structreList.Count; i++)
			{
				structreList[i].Clear();
			}
			structreList.Clear();
		}
		
		material = null;
		dongdata = null;
	}
}
