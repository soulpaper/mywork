using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Dong
{
	public bool success;
	public int code;
	public Dongdata[] data;
	public Vector3 polposes;
}
[Serializable]
public class Dongdata
{
	public Roomtype[] roomtypes;
	public Meta_2 meta;
}

[Serializable]
public class Roomtype
{
	public string[] coordinatesBase64s;
	public Meta_1 meta;
	public List<List<Vector3>> vectorList;

}
[Serializable]
public class Meta_1
{
	public int 룸타입id;
}
[Serializable]
public class Meta_2
{
	public int db_id;
	public string 동;
	public int 지면높이;
}


public class MyFramework : MonoBehaviour
{
	TextAsset textData;
	public Dong dong;
	public Material material;
	public Dictionary<string, ApartmentDong> buildingDic;

	private Coroutine coroutine = null;

	public void Start()
	{
		coroutine = StartCoroutine(CreatVillage());
	}


	public void StartCreateBuilding()
	{
		if(coroutine != null)
		{
			Clear();
		}

		coroutine = StartCoroutine(CreatVillage());
	}
	
	public void Clear()
	{
		if(buildingDic != null)
		{
			foreach(var data in buildingDic.Values)
			{
				data.Clear();
			}
			buildingDic.Clear();
		}
	}


	IEnumerator CreatVillage()
	{
		GetVillageJosonData();
		SerializJsonData();
		buildingDic = new Dictionary<string, ApartmentDong>();
		for(int i =0; i< dong.data.Length;++i)
		{
			ApartmentDong data = new GameObject(dong.data[i].meta.동).AddComponent<ApartmentDong>();
			data.SetDong(dong.data[i], material);
			buildingDic.Add(dong.data[i].meta.동, data);
		}
		yield return null;
	}

	void GetVillageJosonData()
	{
		textData = Resources.Load("dong") as TextAsset;
		dong = JsonUtility.FromJson<Dong>(textData.text);
	}

	void SerializJsonData()
	{
		foreach (var data in dong.data)
		{
			foreach (var roomtypes in data.roomtypes)
			{				
				foreach (var pos in roomtypes.coordinatesBase64s)
				{
					List<Vector3> poslist = new List<Vector3>();
					byte[] bytearr = Convert.FromBase64String(pos);
					float[] floatarr = new float[bytearr.Length];
					Buffer.BlockCopy(bytearr, 0, floatarr, 0, bytearr.Length);
					for (int i = 0; i < floatarr.Length - 3; i += 3)
					{
						if (floatarr[i] == 0f && floatarr[i + 2] == 0f && floatarr[i + 1] == 0f)
						{
							continue;
						}

						poslist.Add(new Vector3(floatarr[i], floatarr[i + 2], floatarr[i + 1]));
					}

					if(roomtypes.vectorList == null)
					{
						roomtypes.vectorList = new List<List<Vector3>>();
					}

					roomtypes.vectorList.Add(poslist);
				}
			}
		}
	}
}
