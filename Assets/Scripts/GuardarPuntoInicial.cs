using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardarPuntoInicial : MonoBehaviour {


	public GameObject puntoInicial;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			puntoInicial.transform.position = this.transform.position;
		}
	}
}
