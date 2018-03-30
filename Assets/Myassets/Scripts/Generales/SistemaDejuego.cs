using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO; 										  // Helps to work with files
using System.Runtime.Serialization.Formatters.Binary;	 // SRFB helps to work Serialization	

public class SistemaDejuego : MonoBehaviour {

	//Instancias y tramas
	public static SistemaDejuego instance;


	//Numeros minimos y maximos para las tablas
	public int min, max;
	int numero1, numero2, resultado;

	List<int> respuestas = new List<int>();

	//Todos los enemigos por Pantalla o operaciones a derrotar
	 GameObject[] enemies;

	private List<Transform> posiciones = new List<Transform>();
	public List<Text> txtOpciones = new List<Text>();


	Transform tmp;

	public bool attack = false;
		   bool generar = false;
		   bool die = false;

	//Variables con la data
	public GameData gData;

	public GameObject pnMenuJuegoTerminado;

	string dataFilePath;
	BinaryFormatter bf;  				

	// Todo lo que tiene que ver con el tiempo, maxTime = es el maximo tiempo por pantalla
	// timeLeft es el tiempo que le qued para terminar
	public float maxtime;
	float timeLeft;


	//Jugador Actual
	GameObject player;
	PlayerController persController;   		

	//Enemigo Actual
	GameObject enemy;
	EnemyScript gnScript;	

	// CameraActual  ActivarYCamera
	GameObject camera;
	MainCamera mainCamera;

	GameObject activarYCamera;

	//Enemigo derrotado
	GameObject destruirTrolls;
	Animator trollDeath; 

	//Interface Grafica
	public UI ui;


	float restartdevel;


	// Use this for initialization
	void Start () {

		if(instance == null){

			instance = this;
		}

		camera = GameObject.Find("Main Camera");
		mainCamera = camera.GetComponent<MainCamera> ();

		activarYCamera = GameObject.Find("ActivarYCamera");

		bf = new BinaryFormatter ();
		dataFilePath = Application.persistentDataPath + "/game.dat";

		player = GameObject.Find ("Dog");
		persController = player.GetComponent<PlayerController>();

		enemies = GameObject.FindGameObjectsWithTag("Enemigos");

		Comenzar ();

	}

	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape)){

			ResetData ();

		}


		if(timeLeft > 0){

			UpdateTime();

		}
	}


	public void SaveData () {

		FileStream fs = new FileStream (dataFilePath, FileMode.Create);

		gData.yaJugo = true;

		bf.Serialize (fs, gData);

		fs.Close (); 															// Close connection

	}


	public void LoadData () {

		if(File.Exists(dataFilePath)){

			FileStream fs = new FileStream (dataFilePath, FileMode.Open);

			ui.txMonedas.text =  gData.monedas.ToString();  				// Para cargar el los coin cuando hacen terminar.

			ui.txtPuntos.text = gData.puntos.ToString();

			ui.txtVidas.text = gData.vidas.ToString();

			gData = (GameData)bf.Deserialize (fs);
		}

	}

	void OnEnable(){

		LoadData();

	}

	void OnDisable(){

		SaveData ();

	}



	public void ResetData () {

		FileStream fs = new FileStream (dataFilePath, FileMode.Create);

		gData.vidas = 5;

		//UpdateHearts ();

		gData.monedas = 0;

		gData.puntos = 0;

		ui.txtPuntos.text = "0";

		ui.txMonedas.text = "0";

		RestaurarVidas ();

		bf.Serialize (fs, gData);

		fs.Close ();										 //Close Connection

	}


	public void Comenzar(){

		//Cargo las variables
		timeLeft = maxtime;
		mainCamera.QuitarYCamera ();

		if(!pnMenuJuegoTerminado.activeSelf){
			Time.timeScale = 1f;
		}


		if(!gData.yaJugo){
			gData.vidas = 5;

			gData.puntos = 0;
			gData.nivel = 0;
			gData.monedas = 0;
		}
		gData.cantidadTrolls = enemies.Length;
		ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString();
	
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
			min = 1;
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
				persController.QuedaUnSoloTroll ();
				}
			if(gData.cantidadTrolls == 0){
				persController.CeroTroll ();
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


	public void restarVidas(){
		CheckLives();
	}



	public void ActualizarUIVidas(){
		int tmp = gData.vidas;
		ui.vidasGo [tmp].SetActive (false);
	}

	public void RestaurarVidas(){
		for (int i = 0; i < 5; i++) {
			ui.vidasGo [i].SetActive (true);
		}
	}

	public void CheckLives(){

		gData.vidas--;

		if (gData.vidas == 0) {
			gData.vidas = 5;
			SaveData();
			Invoke ("GameOver", restartdevel);
		
		} else {
			ActualizarUIVidas();
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
		
		pnMenuJuegoTerminado.SetActive (true);
	}


	public void RestartLevel(){
		SceneManager.LoadScene ("Noche");

	}

	public void EnemigosVencidos(){
		if(gData.cantidadTrolls == 1){
			PantallaTerminada();
		}
	}

	public void PantallaTerminada(){
		Cambiarescena();
	}



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


	public void UpdateTime(){

		timeLeft -= Time.deltaTime;

		ui.textTimer.text = "Restan: " + (int)timeLeft;

		if(timeLeft <= 0){
			//SaveData();
			GameOver();

		}
	}

	public void Salir(){
		Application.Quit();
	}

}
