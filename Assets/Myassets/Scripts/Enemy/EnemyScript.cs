using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {

	Animator anim;
	float speed;

	public bool attack;
	bool move;
	bool ok = true;


	public GameObject tablas;

	//Tiempo entre cada operacion de la tabla
	public float maxtime;
	float timeLeft;

	bool contarParaPromedio;
	public float timeRealiseOperation;


	bool opHabilitada = false;

	public UI ui;

	int pos;



	public Text respuesta1;
	public Text respuesta2;
	public Text signoTroll;
	public Text resultado;
	public Text equal;

	Text[] textosOperaciones;

	// Use this for initialization
	void Start () {

		maxtime = 10;
		move = false;
		attack = false;

		ui.texttimeOp = GameObject.Find ("ResetOperacion").GetComponent<Text>();
		tablas.SetActive (false);
		anim = GetComponent<Animator> ();
		agregarTextosParaRecorrerlos ();

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


		TimeParaPromedios ();
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
		PararTiempoOperaciones();
		yield return new WaitForSeconds(0.1f);
		Destroy (this.gameObject);
	}
		

	public void PararTiempoOperaciones (){
		if(contarParaPromedio){
			contarParaPromedio = false;
			SistemaDejuego.instance.ReciboTiempoParaPromedios (timeRealiseOperation);
		}
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

			contarParaPromedio = true;

			SistemaDejuego.instance.ObtenerEnemigoActual (this.gameObject);

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

			SistemaDejuego.instance.DesactivarBotonRespuestas();

			Idle ();

		   detenerOperacion();

		}
	}


	void OnCollisionEnter2D(Collision2D coll) {
	
		if (coll.gameObject.tag == "Player")
			Atacar();
	
	}





	IEnumerator tiempoDecambio(){

		restaurarValoresTiempo ();


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


	public void TimeParaPromedios(){

		if(contarParaPromedio){
		
			timeRealiseOperation += Time.deltaTime;
			//Debug.Log ("Tiempo de operaciones" + timeRealiseOperation);
		
		} 

		

	}



	public void detenerOperacion(){
		StopCoroutine (tiempoDecambio ());
		ui.texttimeOp.text = "";
		//limpiarTextosDelasOperaciones ();
		opHabilitada = false;

	}


	public void limpiarTextosDelasOperaciones(){
		respuesta1.text = " ";
		signoTroll.text = " ";
		respuesta2.text = " ";
		resultado.text  = " ";
		equal.text =  " ";

	}

	public void agregarTextosParaRecorrerlos(){
		textosOperaciones = new Text[5];
		textosOperaciones[0] = respuesta1;
		textosOperaciones[1] = signoTroll;
		textosOperaciones[2] = respuesta2;
		textosOperaciones[3] = equal;
		textosOperaciones[4] = resultado;

	}

	public void RecibirResultado(string resultadoOperacion){

		if (respuesta1.text == "_") {
			respuesta1.text = resultadoOperacion;
			SistemaDejuego.instance.SetDie (true);
		} else if (respuesta2.text ==  "_") {
			respuesta2.text = resultadoOperacion;
			SistemaDejuego.instance.SetDie (true);
		} else if (signoTroll.text ==  "_") {
			signoTroll.text = resultadoOperacion;
			SistemaDejuego.instance.SetDie (true);
		}else if (resultado.text ==  "_") {
			resultado.text = resultadoOperacion;
			SistemaDejuego.instance.SetDie (true);
		}

	}


}
