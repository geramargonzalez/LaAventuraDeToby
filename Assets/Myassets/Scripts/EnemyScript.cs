using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	Animator anim;

	float speed;

	public GameObject gameManager;
	private SistemaDejuego sisJuego;

	private GameObject player;
	private GameObject respuestas;

	public bool attack;
	bool move;

	public GameObject numeroUno;
		   Numero gameScript;

	public GameObject numeroDos;
		   Numero2 gameScript2;

	bool ok = true;

	public GameObject tablas;


	// Use this for initialization
	void Start () {
		move = false;
		anim = gameObject.GetComponent<Animator> ();

		sisJuego = gameManager.GetComponent<SistemaDejuego>();
	
		player = GameObject.Find ("Dog");
		respuestas = GameObject.Find ("Repuestas");

		gameScript = numeroUno.GetComponent<Numero> ();
		gameScript2 = numeroDos.GetComponent<Numero2> ();


	}

	// Update is called once per frame
	void Update () {
		Mover ();
	}

	void Mover(){
		if(move){
			transform.Translate(-5f * Time.deltaTime,0f, 0f);
		}
	}

	public void ocultarTroll(){
		this.gameObject.SetActive(false);
		anim.SetBool ("Die", false);

	}
		

	public void finAttack(){
		sisJuego.attack = false;
		anim.SetBool ("Attack", sisJuego.attack);
	}

	public void Atacar(){
		sisJuego.attack = true;
		anim.SetBool ("Attack", sisJuego.attack);
	}

	void OnTriggerEnter2D(Collider2D other) {
		sisJuego.recibirTroll(this.gameObject);
		Walk();

		if (other.gameObject.tag == "Player") {
			if (ok) {
				StartCoroutine (tiempoDecambio());

			} 
		}
	}
		

	public void Walk(){
		move = true;
		anim.SetBool ("walk",move);

	}

	public void Idle(){
		move = false;
		anim.SetBool ("walk",move);

	}
		
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			Idle ();
			StopCoroutine (tiempoDecambio ());
			tablas.SetActive(false);
			respuestas.SetActive (false);
			sisJuego.detenerAtaque ();
			ok = true;
			if(attack == true){
				attack = false;
				anim.SetBool("Attack", attack);
			}
		}
	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player")
			Atacar();
			sisJuego.restarVidasPorAtaque();

	}


	IEnumerator tiempoDecambio(){
		sisJuego.cargarPosiciones(this.gameObject.transform);
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
