using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnRespInter : MonoBehaviour {
	public int numero;



	void OnMouseOver(){

		if (Input.GetMouseButtonDown (0)) {

			SisJuegoIntermedia.instance.RecibirMultiplo (numero);

		}
	}
}
