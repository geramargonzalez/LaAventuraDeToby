using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarYCamera : MonoBehaviour {


	GameObject camera;
	MainCamera mainCamera;
	bool ok = true;

	// Use this for initialization
	void Start () {
		camera = GameObject.Find("Main Camera");
		mainCamera = camera.GetComponent<MainCamera> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(DataCtrl.instance.data.nivel == 0 && ok == true){
			mainCamera.setearYCamera ();
			ok = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Player") {
			mainCamera.setearYCamera ();
		}
	}
}
