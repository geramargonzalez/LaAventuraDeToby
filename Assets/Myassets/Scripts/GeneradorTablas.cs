using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorTablas : MonoBehaviour {

	public GameObject gameManager;
	private SistemaDejuego sisJuego;

	public GameObject player;
	public GameObject tablas;
	public GameObject respuestas;
	public GameObject troll;


	public GameObject numeroUno;
	Numero gameScript;

	public GameObject numeroDos;
	Numero2 gameScript2;
	bool ok = true;

	void Start () {
		gameScript = numeroUno.GetComponent<Numero> ();
		gameScript2 = numeroDos.GetComponent<Numero2> ();
		sisJuego = gameManager.GetComponent<SistemaDejuego>();
	}



	void OnTriggerEnter2D(Collider2D other) {
		 sisJuego.recibirTroll(troll);
		if (other.gameObject.tag == "Player") {
			if (ok) {
				StartCoroutine (tiempoDecambio());
			} 
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			 StopCoroutine (tiempoDecambio ());
			 tablas.SetActive(false);
			respuestas.SetActive (false);
				ok = true;

		}
	}


	IEnumerator tiempoDecambio(){
		sisJuego.cargarPosiciones(troll.transform);
		gameScript.desactivarObjetos();
		gameScript2.desactivarObjetos();
		gameScript.setearNumero(sisJuego.pasarNumero1());
		gameScript2.setearNumero(sisJuego.pasarNumero2());
		sisJuego.OperacionMultiplicar();
		sisJuego.EleccionTabla();
		tablas.SetActive(true);
		respuestas.SetActive(true);
		yield return new WaitForSeconds(10.0f);
		ok = true;

	}

	void DeactivateChildren(GameObject g, bool a) {
		//g.activeSelf = a;
		g.SetActive(a);
		foreach (Transform child in g.transform) {
			DeactivateChildren(child.gameObject, a);
		}
	}
}
