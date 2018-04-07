using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnRespuestas : MonoBehaviour {

	public int numero;


	// Use this for initialization
	void Start () {
		
	}

	void OnMouseOver(){

		if (Input.GetMouseButtonDown (0)) {

			Debug.Log ("Name: "+ gameObject.name);

			SistemaDejuego.instance.ResultadoOperacion (numero);
		
		}
	}
}