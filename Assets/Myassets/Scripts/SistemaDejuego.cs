using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SistemaDejuego : MonoBehaviour {


	//Numeros minimos y maximos para las tablas
	public int min, max;
	int numero1, numero2, resultado;

	List<int> respuestas = new List<int>();
	public GameObject[] enemies;
	private List<Transform> posiciones = new List<Transform>();
	public List<Text> txtOpciones = new List<Text>();


	Transform tmp;

	public bool attack = false;
		   bool generar = false;
		   bool die = false;


	GameData gData;
		 int time;


	//Jugador Actual
	GameObject player;
	PlayerController persController;   		

	//Enemigo Actual
	GameObject enemy;
	EnemyScript gnScript;	

	//Enemigo derrotado
	GameObject destruirTrolls;
	Animator trollDeath; 

	//Interface Grafica
	public UI ui;


	// Use this for initialization
	void Start () {
		
		player = GameObject.Find ("Dog");
		persController = player.GetComponent<PlayerController>();
		Comenzar ();


	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Comenzar(){
		//Cargo las variables
		gData.vidas = 5;
		gData.fallos = 0;
		gData.nivel = 0;
		//Cargo los textos
		ui.txtVidas.text = gData.vidas.ToString();
		gData.cantidadTrolls = enemies.Length;
		ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString();
		//

	}

	public int GeneradorNumeroRandom(){
		NivelDejuego();
		float random = Random.Range((float)min,(float)max);
		return (int)random;
	}

	public int GeneradorNumeroRandomRespuesta(){
		float random = Random.Range(0.0f,90.0f);
		return (int)random;
	}


	public void NivelDejuego(){
		if(gData.nivel == 0){
			min = 0;
			max = 3;
		} else if (gData.nivel == 1){
			min = 3;
			max = 6;
		}else if (gData.nivel == 2){
			min = 6;
			max = 9;
		} else if (gData.nivel == 2){
			//Nivel maximo es un repaso de todos lo demas
			min = 0;
			max = 9;
		}

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
			//ok = true;

			persController.AumentarJump();
			posiciones.Add(tmp);
			LimpiarRespuestas ();
			trollDeath.SetBool("Die", die);
			trollDeath.SetBool("Attack", false);
			gData.cantidadTrolls--;
			ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString ();

			if(gData.cantidadTrolls == 1){
					//StartCoroutine(tiempoFinalEscapar());
				}
				

		} else {

			SumarFallos();
			StartCoroutine(TiempoAtaqueEnemigo());
		}

	}

	public void sumarPuntos(){
		gData.puntos = gData.puntos + 1000;
		ui.txtPuntos.text = gData.puntos.ToString ();
	}


	public bool obtenerAttack(){
		return attack;
	}

	public bool AtaqueEnemigo(){
		attack = true;
		trollDeath.SetBool("Attack",true);
		return attack;
	}

	public void detenerAtaque(){
		attack = false;
		trollDeath.SetBool("Attack",false);
	}

	IEnumerator TiempoAtaqueEnemigo(){
		attack = true;
		trollDeath.SetBool("Attack",attack);
		yield return new WaitForSeconds(5.0f);
		attack = false;
		trollDeath.SetBool("Attack",attack);
	}

	public void SumarFallos(){
		gData.fallos++;
		persController.DisminuirJump ();
	}

	public void restarVidasPorcaida(){
		QuitarVidaHastaCero ();
		gnScript.resetearTabla ();

	}
	public void restarVidasPorAtaque(){
		StartCoroutine (tiempoPararestarVida());
	}

	IEnumerator tiempoPararestarVida(){
		yield return new WaitForSeconds(4.0f);
		QuitarVidaHastaCero();
	}

	void QuitarVidaHastaCero(){
		if(gData.vidas > 0){
			gData.vidas--;
			ui.txtVidas.text = gData.vidas.ToString();
		}
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

		respuestas = DesordenarListados(arrDes);
		BotonRespuestas();
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
		

	/*IEnumerator tiempoEspera(){
		float val = Random.Range (0f, arr.Count-1);
		arrDes.Add(arr[(int)val]);
		arr.RemoveAt((int)val);
		yield return new WaitForSeconds (0.2f);	
	}*/

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
		SetearActualEnemigos(untroll);
	}


	public void sumarMonedas(){
		gData.monedas++;
		gData.puntos = gData.puntos + 100;
		ui.txtPuntos.text = gData.puntos.ToString();
		ui.txMonedas.text = gData.monedas.ToString();
		if(gData.monedas == 100){
			gData.monedas = 0;
			ui.txMonedas.text = gData.monedas.ToString();
		}
	}

	public void GameOver(){
		if(gData.vidas == 0){

		}
	}

	public void EnemigosVencidos(){
		if(gData.cantidadTrolls == 1){
			PantallaTerminada();
		}
	}

	public void PantallaTerminada(){
		Cambiarescena();
	}

	/*IEnumerator tiempoFinalEscapar(){
		txtMsjFinal.text = "Tienes 15 segundos para llegar al castillo";
		yield return new WaitForSeconds (15f);	   
	}*/

	public void Cambiarescena(){
	}


	public void SetearActualEnemigos(GameObject pGO){
		for(int i = 0; i <= enemies.Length-1; i++){
			if(pGO.name == enemies[i].name){
				enemy = pGO;
				gnScript = enemy.GetComponent<EnemyScript>();
			}
		}
	}


	public void Contador(){
	}

}
