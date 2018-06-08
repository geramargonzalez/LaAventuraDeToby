using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO; 										
using System.Runtime.Serialization.Formatters.Binary;	 
using DG.Tweening;

public class SisJuegoIntermedia : MonoBehaviour {


	//INSTANCIAS Y TRAMAS
	public static SisJuegoIntermedia instance = null;

	// GAMEDATA
	public GameData data;

	//UI
	public UI ui;
		  
	int divisor;
	int dividendo;
	int resto;
	int resultado;

	//
	public InputField inputUs;

	//Min y Max de los numeros para las operaciones
	int min;
	int max;
	int minResp;
	int maxResp;

	//RespuestaCorrecta
	int correcta;

	// La cantidad de operaciones que necesito para poder terminar, y que no se repita un numero divisor.
	int cantOperacionesParaTerminar;
	int contMismoNumero;

	//Tiempo
	float timeLeft;
	int tiempoStart;
	int seteoTiempo;
	bool ok;
	bool contarParaPromedio;
	float timeRealiseOperation;

	// PANEL DE FIN DE JUEGO
	public GameObject panel;

	public List<Text> txtOpciones = new List<Text>();
	List<int> respuestas = new List<int>();
	GameObject[] goRespuestas;


	//Animaciones del juego.
	Animator animDividendo;
	Animator animResto;
	Animator animResultado;
	Animator animTroll;
	Animator animDog;

	public GameObject troll;
	public GameObject dog;


	int promedioPorNivel;

	void Awake(){

		if(instance == null){

			instance = this;
		}

	}

	// Use this for initialization
	void Start () {

		DataCtrl.instance.RefreshData ();
		data = DataCtrl.instance.data;
		ui.contenedorGameOver.SetActive (false);
		Comenzar();

		animDividendo = ui.dividendotxt.GetComponent<Animator> ();
		animResto = ui.restotxt.GetComponent<Animator> ();
		animResultado = ui.resultadotxt.GetComponent<Animator> ();
		animTroll = troll.GetComponent<Animator> ();
		animDog = dog.GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

		Timer ();
		TimeParaPromedios();
	}

	public void Comenzar(){


		cargarValoresDelasOperaciones ();
		SetearMinMax();
		ReanudarTiempo ();
		GenerarRespuestas ();

	}

	public void SetearMinMax(){

		if (data.nivel < 1) {

			cantOperacionesParaTerminar = 5;
			min = 1;
			max = 10;
			minResp = 1;
			maxResp = 100;
			tiempoStart = 20;
			seteoTiempo = 1;
			data.numParaPromedio = 5;

		} else if (data.nivel > 1 && data.nivel < 6) {

			cantOperacionesParaTerminar = 10;
			min = 1;
			max = 10;
			minResp = 1;
			maxResp = 100;
			tiempoStart = 20;
			seteoTiempo = 1;
			data.numParaPromedio = 5;
		
		} 

		timeLeft = tiempoStart;

	}

	public void cargarValoresDelasOperaciones(){

		ui.dividendotxt.text = "Dividendo ";
		ui.restotxt.text = "Resto ";
		ui.resultadotxt.text =  "Resultado ";
		ui.divisortxt.text = "Divisor";

	}
	public void LimpiarValores(){

		ui.dividendotxt.text = " ";
		ui.restotxt.text = " ";
		ui.resultadotxt.text =  " ";
		ui.divisortxt.text = "";

	}


		
	public int GeneradorNumeroRandomMultiplo(){

		float random = Random.Range((float)min,(float)max);
		return (int)random;
	}


	public void GenerarDivisor(){

		if (cantOperacionesParaTerminar > 0) {

			divisor = GeneradorNumeroRandomMultiplo ();
			ui.divisortxt.text = divisor.ToString ();


		} else {

			if (data.fallos < 5) {

				GameOver ();

			} else {

				Comenzar ();

			}

		}


	}

	public void RestoDeOperacion(int num){

		dividendo = respuestas[num];
		resto = dividendo % divisor;
		ui.dividendotxt.text = dividendo.ToString ();
		ui.restotxt.text = resto.ToString ();

		if (resto == 0) {

			SumarPuntos ();
			contarParaPromedio = true;
			cantOperacionesParaTerminar--;

		} else {

			DescontarPuntosPorFallos ();

		}



	}

	public void ResultadoDelaOperacion(){

		resultado = dividendo / divisor;

		ui.resultadotxt.text = resultado.ToString ();
	}



	public List<int> GenerarNumeroMultipoRespuestas(){

		List<int> arrDes = new List<int>();
		bool ok = false;

		while(!ok){

			float random = Random.Range((float)minResp, (float)maxResp);
				
			  if(random != 0){

					if((int)random % divisor == 0){

						correcta = (int)random;
						arrDes.Add (correcta);
						ok = true;

					
					} 

				}
		
		}


		return arrDes;
	
	}

	public List<int> GenerarNumeroNoMultipoRespuestas(List<int> arrDes){

		bool ok = false;
		int pos = 0;
		int cont = 0;

		while(!ok){

			float random = Random.Range((float)minResp,(float)maxResp);

			if(random != 0){

				if((int)random % divisor != 0){

					pos = (int)random;
					arrDes.Add (pos);
					cont++;
				
				} 

				if(cont == 2){

					ok = true;
				
				}
			}
		
		 }

		return arrDes;

	}





	public static List<T> DesordenarListados<T>(List<T> input)
	{
		List<T> arr = input;
		List<T> arrDes = new List<T>();

		while (arr.Count > 0)
		{
			int val = Random.Range(0,arr.Count);
			arrDes.Add(arr[val]);
			arr.RemoveAt(val);
		}

		return arrDes;
	}


	public void GenerarRespuestas(){

		GenerarDivisor ();
		List<int> tmp =  GenerarNumeroNoMultipoRespuestas(GenerarNumeroMultipoRespuestas());
		Debug.Log (tmp.Count);
		respuestas = DesordenarListados<int>(tmp);
		BotonRespuestas ();

	
	}




	public void BotonRespuestas(){

		for(int i=0;  i < respuestas.Count; i++){
			
			txtOpciones[i].text = respuestas[i].ToString();

		}
	}


	public void LimpiarRespuestas(){

		for(int i=0;  i < respuestas.Count; i++){

			txtOpciones[i].text = " ";

		}

	}


	public void RecibirMultiplo(int numUsuario){

		StartCoroutine(ActivarAnimaciones());
		StartCoroutine (ocultarMostrar(numUsuario));
	}



	IEnumerator  ActivarAnimaciones(){

		animResto.SetBool ("entrar",true);
		animDividendo.SetBool ("entrar",true);
		animResultado.SetBool ("entrar",true);
		yield return new WaitForSeconds(1.0f);
		animResto.SetBool ("entrar",false);
		animDividendo.SetBool ("entrar",false);
		animResultado.SetBool ("entrar",false);

	}


	// Puntaje y Fin del juego
	public void GameOver(){
		
		if (cantOperacionesParaTerminar == 0 & data.fallos <= 3) {

			ui.contenedorGrl.SetActive (false);
			ui.contenedorGameOver.SetActive (true);
			promedioPorNivel = data.calcularPromedio ();
			data.SetearNivelACtual();
			DataCtrl.instance.SaveData(data);

		} else if(tiempoStart == 0 || data.fallos == 4) {

			ui.contenedorGrl.SetActive (false);
			ui.contTryAgain.SetActive (true);
		
		}



	}

	//Puntos por aciertos
	public void SumarPuntos(){

		data.puntos += 1000  -  (data.fallos*50);
		ui.txtPuntos.text = "Score " + data.puntos.ToString ();
		animDog.SetBool ("entrar", true);

	
	}


	public void DescontarPuntosPorFallos(){
	
		data.puntos -= 50 * cantOperacionesParaTerminar;
		ui.txtPuntos.text = "Score " + data.puntos;
		Fallos ();

	
	}


	//Tiempo del juego
	public void Timer(){

		if(ok){



			if (timeLeft > 0) {

				timeLeft -= Time.deltaTime;
				ui.textTimer.text = "Restan: " + (int)timeLeft;


			
			} else {

				tiempoStart = tiempoStart - seteoTiempo;

				if (tiempoStart > 0) {

					SeAcaboTiempo();
					Fallos();


				} else {

					PararTiempo();
					GameOver();
				}

				timeLeft = (float)tiempoStart;
			}
		}
	}


	public void SeAcaboTiempo(){
		
		StartCoroutine (MostrarCuentasComienzo());
	
	}

	public void Fallos(){

		data.fallos++;
		ui.textFallos.text = "Fallos " + data.fallos;
		animTroll.SetBool ("entrar",true);
	}

	public void PararTiempo(){
		ok = false;
		ReciboTiempoParaPromedios(timeLeft);
	
	}

	public void ReanudarTiempo(){
		ok = true;
		ResetearTiempo ();
	
	}
		
	public void ResetearTiempo(){
	
		tiempoStart = tiempoStart - seteoTiempo;
		timeLeft = tiempoStart;
	
	}



	IEnumerator MostrarCuentasComienzo(){
		
		yield return new WaitForSeconds(1.0f);
		GenerarRespuestas ();

	}


	IEnumerator ocultarMostrar(int numUsuario)
	{
		yield return new WaitForSeconds(0.5f);
		RestoDeOperacion (numUsuario);
		ResultadoDelaOperacion ();
		PararTiempo ();
		yield return new WaitForSeconds(2.0f);
		ui.panelMultiplo.SetActive (false);
		cargarValoresDelasOperaciones();
		yield return new WaitForSeconds(.5f);
		animTroll.SetBool ("entrar",false);
		animDog.SetBool ("entrar", false);
		GenerarRespuestas ();
		ui.panelMultiplo.SetActive (true);
		ResetearTiempo ();
	}



	public int NivelLogrado (){

		return data.nivel;

	}

	public int GetScore () {

		return data.puntos;

	}


	public int obtenerPromedio (){
		return promedioPorNivel;
	}

	public int obtenerFallos(){
		return data.fallos;
	}

	public int SetStarsAwarded (int levelNumber, int stars){

		return data.niveles[levelNumber].bonesStars = stars;

	}


	public void PuntosPorStars(int stars){

		if(stars == 1){

			data.puntos += 2500;

		} else if(stars == 2){

			data.puntos += 5000;

		} else if(stars == 3){

			data.puntos += 10000;

		}

	}


	public void ReciboTiempoParaPromedios(float tiempoOperacion){

		data.niveles[data.nivel].promedio += (int)tiempoOperacion;

	}


	public void TimeParaPromedios(){

		if (ok) {

			timeRealiseOperation += Time.deltaTime;


		} else {

			if(contarParaPromedio){

				contarParaPromedio = false;
				SisJuegoIntermedia.instance.ReciboTiempoParaPromedios (timeRealiseOperation);	
				timeRealiseOperation = 0;
			}

		
		}



	}


}
