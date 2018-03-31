using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MenuActivo ();
	}

	public void MenuActivo(){
		if (this.gameObject.activeSelf) {
			Time.timeScale = 0f;
		} 
	}


}
