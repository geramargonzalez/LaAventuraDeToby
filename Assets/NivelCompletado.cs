using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NivelCompletado : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
	
		if (other.gameObject.CompareTag ("Player")) {
				
			SistemaDejuego.instance.EnemigosVencidos();

		}

	}

}
