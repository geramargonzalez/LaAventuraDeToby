using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SistemaDejuego : MonoBehaviour {



	public int min, max;
	int numero1, numero2,resultado;
	List<int> respuestas = new List<int>();
	public List<Text> txtOpciones = new List<Text>();
	public GameObject[] enemies;

	private List<Transform> posiciones = new List<Transform>();
	private Transform tmp;

	private int fallos;

		   GameObject destruirTrolls;
	 	   Animator trollDeath;
		   public  bool attack = false;
	      public bool die = false;
	       
	private int cantidadTrolls;

	public GameObject player;
	PersonajeController persController;

	public bool ok = false;
	public bool generar = false;

	       int vidas;
	public Text txtVidas;

	       int puntos;
	public Text txtPuntos;

		   int monedas;
	public Text txMonedas;


	public Text txtCantEnemigos;
	public Text txtMsjFinal;

	// Use this for initialization
	void Start () {
		persController = player.GetComponent<PersonajeController>();
		vidas = 5;
		txtVidas.text = vidas.ToString();
		cantidadTrolls = enemies.Length;
		txtCantEnemigos.text = cantidadTrolls.ToString();

	}

	// Update is called once per frame
	void Update () {
		
	}

	public int GeneradorNumeroRandom(){
		float random = Random.Range((float)min,(float)max);
		return (int)random;
	}

	public int GeneradorNumeroRandomRespuesta(){
		float random = Random.Range(0.0f,90.0f);
		return (int)random;
	}


	public void EleccionTabla(){
		if (!destruirTrolls.activeSelf) {
			destruirTrolls.SetActive(true);
		}
		OkGenerar();
		if(generar){
			IngresarRespuestas();
			generar = false;
		}

	}

	 public void ResultadoOperacion(int num){
		if (resultado == respuestas [num]) {
			die = true;
			ok = true;
			if(attack == false){
				posiciones.Add(tmp);
				LimpiarRespuestas ();
				trollDeath.SetBool("Die", die);
				puntos = puntos + 1000;
				txtPuntos.text = puntos.ToString ();
				cantidadTrolls--;
				txtCantEnemigos.text = cantidadTrolls.ToString ();
				if(cantidadTrolls == 1){
					StartCoroutine(tiempoFinalEscapar());
				}
				//persController.AumentarJump();
			}


		} else {
			restarVidas();
			//persController.DisminuirJump();
		}

	}

	public void restarVidas(){
		attack = true;
		fallos++;
		vidas--;
		txtVidas.text = vidas.ToString();
		trollDeath.SetBool("Attack",true);
	}

	public void restarVidasPorcaida(){
		vidas--;
		txtVidas.text = vidas.ToString();

	}
		
	public int pasarNumero1(){
		numero1 = GeneradorNumeroRandom();
		return numero1 - 1;
	}
	public int pasarNumero2(){
		numero2 = GeneradorNumeroRandom();
		return numero2 - 1;
	}



	public void IngresarRespuestas(){
		List<int> arrDes = new List<int>();
		int tmp = 0;		
		for(int i = 0;  i < 3; i++){
			if(i < 2){
			  tmp = GeneradorNumeroRandomRespuesta();
			  arrDes.Add(tmp);
			} else if(i == 2){
			  
			  arrDes.Add(resultado);
			}

		}

		respuestas = DesordenarLista(arrDes);
		BotonRespuestas();
	}

	 public static List<T> DesordenarLista<T>(List<T> input)
	 {
		List<T> arr = input;
		List<T> arrDes = new List<T>();

		Random randNum = new Random();

		while (arr.Count > 0)
		 {
			float val = Random.Range (0f, arr.Count-1);
			arrDes.Add(arr[(int)val]);
			arr.RemoveAt((int)val);
		}

		return arrDes;
	}
		

	public void cargarPosiciones(Transform position){
		tmp = position;
		OkGenerar ();

	}
		

	public void OkGenerar(){
		for(int i = 0;  i < posiciones.Count; i++){
			if(tmp == posiciones[i]){
				generar = false;
			}
		}
		generar = true;
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
	public void OperacionMultiplicar(){
		resultado = numero1 * numero2;
	}

	public void recibirTroll(GameObject untroll){
		
		destruirTrolls = untroll;
		trollDeath = destruirTrolls.GetComponent<Animator>();
	}


	public void sumarMonedas(){
		monedas++;
		puntos = puntos + 100;
		txtPuntos.text = puntos.ToString();
		txMonedas.text = monedas.ToString();
		if(monedas == 100){
			//persController.AumentarSpeed();
			monedas = 0;
			txMonedas.text = monedas.ToString();
		}
	}

	public void GameOver(){
		if(vidas == 0){
			//JUEGOTERMINADO
			//COMENZAR DE NUEVO
		}
	}

	public void EnemigosVencidos(){
		if(cantidadTrolls == 1){
			PantallaTerminada();
		}
	}

	public void PantallaTerminada(){
		Cambiarescena();
	}

	IEnumerator tiempoFinalEscapar(){
		txtMsjFinal.text = "Tienes 15 segundos para llegar al castillo";
		yield return new WaitForSeconds (15f);	   
	}

	public void Cambiarescena(){
	}
}
