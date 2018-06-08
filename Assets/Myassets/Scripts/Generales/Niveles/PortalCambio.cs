using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCambio : MonoBehaviour {

	public GameObject gameManager;
	private SistemaDejuego sisJuego;

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.CompareTag("Player")){
			//sisJuego.EnemigosVencidos();
		}

	}

}
