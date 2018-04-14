using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

	Animator anim;

	float speed;

	//public GameObject gameManager;
	//private GameObject player;
   // public GameObject respuestas;

	public bool attack;
	bool move;

	//public GameObject numeroUno;
		 //  Numero gameScript;

	//public GameObject numeroDos;
		 //  Numero2 gameScript2;

	//public GameObject signo;
		   //setearOperacion scriptTipoOp;

	bool ok = true;

	public GameObject tablas;

	//Tiempo entre cada operacion de la tabla
	public float maxtime;
	float timeLeft;
	bool opHabilitada = false;


	public UI ui;

	int pos;

	public Text respuesta1;
	public Text respuesta2;
	public Text signoTroll;
	public Text resultado;
	public Text equal;



	// Use this for initialization
	void Start () {

		maxtime = 10;
		move = false;
		attack = false;

		//respuestas = GameObject.Find ("Respuestas");  txtequals
		ui.texttimeOp = GameObject.Find ("ResetOperacion").GetComponent<Text>();

		//ui.respuesta1 = GameObject.Find ("txtrespuesta1").GetComponent<Text>();
		//ui.respuesta2 = GameObject.Find ("txtrespuesta2").GetComponent<Text>();
		//ui.signoTroll = GameObject.Find ("txtsigno").GetComponent<Text>();
		//ui.equal = GameObject.Find ("txtequals").GetComponent<Text>();
		//ui.resultado =  GameObject.Find ("txtresultado").GetComponent<Text>();

		tablas.SetActive (false);

		//respuestas.SetActive (false);
		//gameScript = numeroUno.GetComponent<Numero> ();
		//gameScript2 = numeroDos.GetComponent<Numero2> ();
		//scriptTipoOp =   signo.GetComponent<setearOperacion> ();

		anim = GetComponent<Animator> ();

	}

	// Update is called once per frame
	void Update () {
		
		if(SistemaDejuego.instance.matarTroll()){
			
			detenerOperacion ();
			Death ();
	
		}

		if(SistemaDejuego.instance.obtenerAttack()){
			
			Atacar ();
		
		}
			
		Mover ();

		finAttack();

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
		
		StartCoroutine (DestruirTroll());
	}


	IEnumerator DestruirTroll(){
		
		SistemaDejuego.instance.SetDie(false);
		SistemaDejuego.instance.SetearCrearNuevoTroll (true);

		yield return new WaitForSeconds(0.05f);
	
		Destroy (this.gameObject);
	}
		

	public void finAttack(){
		anim.SetBool ("Attack", SistemaDejuego.instance.obtenerAttack());
	}

	public void Atacar(){
		anim.SetBool ("Attack", true);
	}

	public void Walk(){
		move = true;
		anim.SetBool ("walk" , move);

	}

	public void Idle(){
		move = false;
		anim.SetBool ("walk", move);

	}

	public void Death(){
		anim.SetBool ("Die", SistemaDejuego.instance.matarTroll());
	}

	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag("Player")) {

			SistemaDejuego.instance.ObtenerEnemigoActual (this.gameObject.transform);

			Walk();

			SistemaDejuego.instance.ActivarBotonRespuestas();

			if (ok) {
				StartCoroutine (tiempoDecambio());
			} 
		}
	}
		


	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {

			//SistemaDejuego.instance.DesactivarBotonRespuestas ();

			Idle ();

		   detenerOperacion();

		}
	}


	void OnCollisionEnter2D(Collision2D coll) {
	
		if (coll.gameObject.tag == "Player")
			Atacar();
			//respuestas.SetActive (false);
	
	}




	IEnumerator tiempoDecambio(){

		restaurarValoresTiempo ();

		//Desactivo los numeros.
		//gameScript.desactivarObjetos();
		//gameScript2.desactivarObjetos();
		//.desactivarObjetos();

		//Multiplico las operaciones
		SistemaDejuego.instance.OperacionAritmetica();

		//seteo las tablas
		SistemaDejuego.instance.EleccionTabla();

		respuesta1.text = SistemaDejuego.instance.devolverNumero1();
		signoTroll.text = SistemaDejuego.instance.devolverSigno();
		respuesta2.text = SistemaDejuego.instance.devolverNumero2();
		resultado.text  = SistemaDejuego.instance.devolverResultado();


		//Activo las tablas
		tablas.SetActive(true);

		//Muestros las respuestas
		//respuestas.SetActive(true);

		//Espero
		yield return new WaitForSeconds(maxtime);


		//Si pasan los 10 segundos ataco al personaje
		ok = true;

		attack = SistemaDejuego.instance.obtenerAttack ();

	
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
		limpiarTextosDelasOperaciones ();
		opHabilitada = false;
		//tablas.SetActive(false);
		//respuestas.SetActive (false);

	}


	public void limpiarTextosDelasOperaciones(){
		respuesta1.text = " ";
		signoTroll.text = " ";
		respuesta2.text = " ";
		resultado.text  = " ";
		equal.text =  " ";
	}



}
