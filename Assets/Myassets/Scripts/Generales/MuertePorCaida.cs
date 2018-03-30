using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuertePorCaida : MonoBehaviour {


	public Transform volver;

	public GameObject gameManager;
	private SistemaDejuego sisJuego;

	// CameraActual  ActivarYCamera
	GameObject camera;
	MainCamera mainCamera;


	void Start () {
		camera = GameObject.Find("Main Camera");

		mainCamera = camera.GetComponent<MainCamera> ();

		sisJuego = gameManager.GetComponent<SistemaDejuego>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.tag == "Player") {
			other.gameObject.transform.position = volver.position;
			sisJuego.restarVidas();
			mainCamera.QuitarYCamera ();
		}
	}
}


