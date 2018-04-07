using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	Animator anim;

	float speed;

	//public GameObject gameManager;


	//private GameObject player;
	public GameObject respuestas;

	public bool attack;
	bool move;

	public GameObject numeroUno;
		   Numero gameScript;

	public GameObject numeroDos;
		   Numero2 gameScript2;

	public GameObject signo;
		   setearOperacion scriptTipoOp;





	bool ok = true;

	public GameObject tablas;

	//Tiempo entre cada operacion de la tabla
	public float maxtime;
	float timeLeft;
	bool opHabilitada = false;

	public UI ui;

	// Use this for initialization
	void Start () {

		maxtime = 10;
		move = false;
		anim = gameObject.GetComponent<Animator> ();


	//	respuestas = GameObject.Find ("Respuestas");
	
		//respuestas.SetActive (false);
		tablas.SetActive (false);

		gameScript = numeroUno.GetComponent<Numero> ();
		gameScript2 = numeroDos.GetComponent<Numero2> ();
		scriptTipoOp =   signo.GetComponent<setearOperacion> ();


	}

	// Update is called once per frame
	void Update () {
		Mover ();

		if(timeLeft > 0 && opHabilitada){

			TimeOperaciones();

		}
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
		SistemaDejuego.instance.attack = false;
		anim.SetBool ("Attack", SistemaDejuego.instance.attack);
	}

	public void Atacar(){
		SistemaDejuego.instance.attack = true;
		anim.SetBool ("Attack",SistemaDejuego.instance.attack);
	}

	void OnTriggerEnter2D(Collider2D other) {


		SistemaDejuego.instance.recibirTroll(this.gameObject);

		Walk();

		if (other.gameObject.tag == "Player") {

			SistemaDejuego.instance.ActivarBotonRespuestas();

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

			//SistemaDejuego.instance.DesactivarBotonRespuestas ();

			Idle ();

		   detenerOperacion();

			//ok = true;

			//if(attack == true){
				//attack = false;
				//anim.SetBool("Attack", attack);
			//}
		}
	}


	void OnCollisionEnter2D(Collision2D coll) {
	
		if (coll.gameObject.tag == "Player")
			Atacar();
			respuestas.SetActive (false);
	
	}




	IEnumerator tiempoDecambio(){

			

			restaurarValoresTiempo ();

			//Desactivo los numeros.
			gameScript.desactivarObjetos();
			gameScript2.desactivarObjetos();
			scriptTipoOp.desactivarObjetos();


	
		//Muestro los numeros seleccionados para la operacion
		scriptTipoOp.setearSigno(SistemaDejuego.instance.ObtenerSigno());

		//Numero de la Izquiera
		gameScript.setearNumero(SistemaDejuego.instance.pasarNumeroIzquierda());

		//Numero de la Derecha
		gameScript2.setearNumero(SistemaDejuego.instance.pasarNumeroDerecha());



		//Multiplico las operaciones
		SistemaDejuego.instance.OperacionAritmetica();

		//seteo las tablas
		SistemaDejuego.instance.EleccionTabla();

		//Activo las tablas
		tablas.SetActive(true);

		//Muestros las respuestas
		respuestas.SetActive(true);

		//Espero
		yield return new WaitForSeconds(maxtime);

		//Si pasan los 10 segundos ataco al personaje
		ok = true;

		attack = SistemaDejuego.instance.AtaqueEnemigo();

	
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
		
	public void restaurarValoresTiempo(){
		ui.texttimeOp.color = Color.white;
		timeLeft = maxtime;
		opHabilitada = true;
	}

	public void TimeOperaciones(){
		
		timeLeft -= Time.deltaTime;

		ui.texttimeOp.text = "Reset operación: " + (int)timeLeft;

		if(timeLeft <= 0){
			resetearTabla ();

		}else if(timeLeft <= 5){
			ui.texttimeOp.color = Color.red;
		}
	}

	public void detenerOperacion(){
		StopCoroutine (tiempoDecambio ());
		ui.texttimeOp.text = "";
		opHabilitada = false;
		tablas.SetActive(false);
		respuestas.SetActive (false);

	}



}
