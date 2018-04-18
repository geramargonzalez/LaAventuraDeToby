using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DataCtrl : MonoBehaviour {


	public static DataCtrl instance = null;
	public GameData data;

	string dataFilePath;	

	BinaryFormatter bf;  	

	void Awake(){

		if (instance == null) {

			instance = this;
			DontDestroyOnLoad (gameObject);

		} else {
		
			Destroy (gameObject);
		
		}


		bf = new BinaryFormatter ();
		dataFilePath = Application.persistentDataPath + "/game.dat";


	}


	public void RefreshData(){

		if(File.Exists(dataFilePath)){

			FileStream fs = new FileStream (dataFilePath, FileMode.Open);
			data = (GameData)bf.Deserialize (fs);
			fs.Close ();

			Debug.Log ("Refresh Data");
		}

	
	}


	void OnEnable(){

		//RefreshData();
		CheckDB();

	}

	void OnDisable(){

		//SaveData ();

	}


	public void SaveData (){

		FileStream fs = new FileStream (dataFilePath, FileMode.Create);

		bf.Serialize (fs, data);

		fs.Close (); 	
	
	}


	public bool IsUnlocked(int levelNumber){

		return data.niveles[levelNumber].unlocked;
	}


	public int GetStars(int levelNumber){

		return data.niveles[levelNumber].bonesStars;
	}

	public void CheckDB(){

		if (File.Exists (dataFilePath)) {

			#if UNITY_ANDROID
			string srcFile = System.IO.Path.Combine(Application.streamingAssetsPath, "game.dat");
			WWW downloader = new WWW(srcFile);

			while(!downloader.isDone){
				
			}

			// Then save to Aplication.
			File.WriteAllBytes(dataFilePath,downloader.bytes);
			RefreshData();
			#endif

		}
	}


}
