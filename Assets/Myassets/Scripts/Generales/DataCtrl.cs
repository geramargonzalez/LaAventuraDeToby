using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DataCtrl : MonoBehaviour {


	public static DataCtrl instance = null;
	public GameData data;

	public bool devMode;

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

		//Debug.Log (dataFilePath);


	}


	public void RefreshData(){

		if(File.Exists(dataFilePath)){

			FileStream fs = new FileStream (dataFilePath, FileMode.Open);

			data = (GameData)bf.Deserialize (fs);

			fs.Close ();

		}


		SetearNumeroNivel ();

	
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

	// Copiar para la batallas de las tablas
	public void SaveData(GameData data){

		FileStream fs = new FileStream (dataFilePath, FileMode.Create);

		bf.Serialize (fs, data);

		fs.Close (); 	

	}


	public bool IsUnlocked(int levelNumber){

		return data.niveles [levelNumber].unlocked;
	}


	public int GetStars(int levelNumber){

		return data.niveles[levelNumber].bonesStars;
	}

	public void CheckDB(){

		if (!File.Exists (dataFilePath)) {

			#if UNITY_ANDROID

				CopyDB();

			#endif

		} else {

			//Pregunta si es una apliacion de escritorio
			if (SystemInfo.deviceType == DeviceType.Desktop) {

				string dstFile = System.IO.Path.Combine(Application.streamingAssetsPath, "game.dat");
				File.Delete (dstFile);
				File.Copy (dataFilePath,dstFile);

			}


			if(devMode){

				// Pregunta si es una aplicacion movil
				if (SystemInfo.deviceType == DeviceType.Handheld) {
					File.Delete (dataFilePath);
					CopyDB ();

				}

			}
		
		
			RefreshData ();
		
		}
	}

	public void CopyDB(){

		string srcFile = System.IO.Path.Combine(Application.streamingAssetsPath, "game.dat");

		WWW downloader = new WWW(srcFile);

		while(!downloader.isDone){

		}

		// Then save to Aplication.
		File.WriteAllBytes(dataFilePath,downloader.bytes);
		RefreshData();
	}


	public void ResetData(){

		FileStream fs = new FileStream (dataFilePath, FileMode.Create);

		data.nivel = 0;

		data.vidas = 5;

		data.bones = 0;

		data.puntos = 0;

		data.fallos = 0;

		UnLockedNivel ();

		data.tiempoActual = data.ResetTime ();

		data.yaJugo = false;

		data.jumpSpeed = 900f;

		data.speedBoost = 20f;

		data.posActualEnemigo = 0;

		bf.Serialize (fs, data);

		fs.Close ();	

	}


	public void UnLockedNivel(){

		for(int i = 0; i < data.niveles.Length; i++){

			if(data.niveles[i].nivel != 0){

				Debug.Log (data.niveles[i].nivel + " "  + data.niveles[i].unlocked );

				data.niveles [i].unlocked = false;
			
			}


			data.niveles [i].bonesStars = 0;

			data.niveles[i].fallosMultiplicacion = 0;

			data.niveles[i].fallosSuma = 0;

			data.niveles[i].fallosResta = 0;

			data.niveles[i].fallosDivision = 0;

			data.niveles[i].aciertosMultiplicacion = 0;

			data.niveles[i].aciertosSuma = 0;

			data.niveles[i].aciertosResta = 0;

			data.niveles[i].aciertosDivision = 0;

			data.niveles[i].promedio = 0;


		}
	}

	public void SetearNumeroNivel(){
		data.SetearNumeroDeNiveles();
	}
}
