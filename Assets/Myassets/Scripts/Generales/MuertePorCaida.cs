using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuertePorCaida : MonoBehaviour {


	public Transform volver;
	GameObject camera;
	MainCamera mainCamera;


	void Start () {
		
		camera = GameObject.Find("Main Camera");
		mainCamera = camera.GetComponent<MainCamera> ();


	}


	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.tag == "Player") {

			mainCamera.QuitarYCamera ();
			SistemaDejuego.instance.PlayerDies (other.gameObject);
		
		}
	}
}


