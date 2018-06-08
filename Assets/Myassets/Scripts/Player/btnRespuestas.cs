using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnRespuestas : MonoBehaviour {

	public int numero;



	void OnMouseOver(){

		if (Input.GetMouseButtonDown (0)) {

			SistemaDejuego.instance.ResultadoOperacion (numero);
		
		}
	}
}