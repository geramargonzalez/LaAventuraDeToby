using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorTablas : MonoBehaviour {

	private GameObject gameManager;
	private SistemaDejuego sisJuego;

	private GameObject player;

	private GameObject respuestas;

	public GameObject troll;
		   Animator trollDeath;
		   bool attack;

	public GameObject numeroUno;
	Numero gameScript;

	public GameObject numeroDos;
	Numero2 gameScript2;
	bool ok = true;

	public GameObject tablas;

	void Start () {

		gameManager = GameObject.Find ("SistemaJuego");
		sisJuego = gameManager.GetComponent<SistemaDejuego>();
		player = GameObject.Find ("Dog");
		respuestas = GameObject.Find ("Repuestas");

		gameScript = numeroUno.GetComponent<Numero> ();
		gameScript2 = numeroDos.GetComponent<Numero2> ();

		trollDeath = troll.GetComponent<Animator> ();
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
			if(attack == true){
				attack = false;
				trollDeath.SetBool("Attack", attack);
			}
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
		attack = sisJuego.AtaqueEnemigo();

	}

	void DeactivateChildren(GameObject g, bool a) {
		//g.activeSelf = a;
		g.SetActive(a);
		foreach (Transform child in g.transform) {
			DeactivateChildren(child.gameObject, a);
		}
	}

	public void resetearTabla(){
		StartCoroutine (tiempoDecambio());
	}
}
