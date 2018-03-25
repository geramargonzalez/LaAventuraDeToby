using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuertePorCaida : MonoBehaviour {


	public Transform volver;

	public GameObject gameManager;
	private SistemaDejuego sisJuego;


	void Start () {
		sisJuego = gameManager.GetComponent<SistemaDejuego>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			sisJuego.restarVidasPorcaida();
			other.gameObject.transform.position = volver.position;
		}
	}
}


