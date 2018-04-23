﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public void LoadScene(string sceneName){
		SceneManager.LoadScene (sceneName);
	}

	public void CurrenteScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}

	public void CurrenteSceneParaComenzarDeNuevo(){
		DataCtrl.instance.ResetData();
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}
}
