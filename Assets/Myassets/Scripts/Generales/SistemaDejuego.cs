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

	//Todos los enemigos por Pantalla o Operaciones a derrotar
	GameObject[] enemies;

	GameObject[] goRespuestas;

	int signo;

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
	GameObject trollActual;
	Animator trollDeath; 

	//Interface Grafica
	public UI ui;

	int numeroUsuaio;

	float restartdevel;

	int bigBoneValue = 10;
	int orquitosValues = 500;
	int enemyValue = 1000;

	int enemigosActivos;

	bool operacionConcretada;

	public enum Item {
		Enemigos,
		Orquitos,
		Bone
	}

		void Awake(){

		if(instance == null){
			
			instance = this;
		}

		bf = new BinaryFormatter ();
		dataFilePath = Application.persistentDataPath + "/game.dat";

	}



	// Use this for initialization
	void Start () {

		camera = GameObject.Find("Main Camera");
		mainCamera = camera.GetComponent<MainCamera> ();

		activarYCamera = GameObject.Find("ActivarYCamera");

		player = GameObject.Find ("Dog");

		persController = player.GetComponent<PlayerController>();

		enemies = GameObject.FindGameObjectsWithTag("Enemigos");
		goRespuestas = GameObject.FindGameObjectsWithTag("btnRespuesta");

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
	
		bf.Serialize (fs, gData);

		fs.Close (); 	



	}


	public void LoadData () {



		if (File.Exists (dataFilePath)) {

			FileStream fs = new FileStream (dataFilePath, FileMode.Open);

			gData = (GameData)bf.Deserialize (fs);

			//gData.tiempoActual = maxtime;
	
			ui.txMonedas.text = gData.monedas.ToString ();  				

			ui.txtPuntos.text = gData.puntos.ToString ();

			if(!gData.yaJugo){
			
				gData.GuardarPosicionInicial ();
		
			}

		
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
	
		gData.monedas = 0;

		gData.puntos = 0;

		ui.txtPuntos.text = "0";

		ui.txMonedas.text = "0";

		gData.tiempoActual = gData.ResetTime ();

		gData.yaJugo = false;

		RestaurarVidas ();

		bf.Serialize (fs, gData);

		fs.Close ();										 

	}




	public void Comenzar(){

		gData.yaJugo = false;

		player.transform.position = new Vector3 (gData.x, gData.y, gData.z);
		//Debug.Log ("Metodo: Comenzar");

		timeLeft = gData.tiempoActual;

		if(!pnMenuJuegoTerminado.activeSelf){

			Time.timeScale = 1f;
		
		}
			
		MarcarOperacionesNoRealizadas ();
		EnemigosDerrotados();
		gData.cantidadTrolls = enemigosActivos;

		mainCamera.QuitarYCamera ();
		ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString();
		RestaurarVidas ();
	
	}
		

	public int GenerarSignoUIRandom(){
		
		float random = Random.Range(0f,3f);

		return (int)random;
	}

	public int GeneradorNumeroRandom(){
		 NivelDejuego();
		 float random = Random.Range((float)min,(float)max);
		 return (int)random;
	}

	public int GeneradorNumeroRandomIzquierda(){
		float random = Random.Range(0.0f,9.0f);
		return (int)random;
	
	}

	public int GeneradorNumeroRandomRespuesta(){
		float random = Random.Range(0.0f,90.0f);
		return (int)random;
	}


	//Nivel actual del jugador.
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

			min = 0;
			max = 9;
		}

	}

	public void EleccionTabla(){

		if (!trollActual.activeSelf) {
		
			trollActual.SetActive(true);
		
		}

		OkGenerar();

		if(generar){

			IngresarRespuestas();
		
			generar = false;
		
		}

	}

	 public void ResultadoOperacion(int num){

		if(!gData.yaJugo){
			gData.yaJugo = true;
		}
			
		numeroUsuaio = num;


			if (resultado == respuestas [num]) {

				die = true;
				persController.AumentarJump();
				
				posiciones.Add(tmp);
				LimpiarRespuestas ();
				DesactivarBotonRespuestas ();
				
				
				trollDeath.SetBool("Die", die);
				trollDeath.SetBool("Attack", false);
				gData.cantidadTrolls--;
				ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString ();
				
				gnScript.detenerOperacion ();
				
				sumarPuntos (Item.Enemigos);
				

			    operacionConcretada = true;
				OperacionesAritmeticasCompletadas ();
			    

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

	public void OperacionesAritmeticasCompletadas(){
		

		for(int i=0; i < enemies.Length; i++){

			if (trollActual.name == enemies [i].name && operacionConcretada) {

				gData.operaRealizadas [i] = true;

			}
		}
		
	}



	public void MarcarOperacionesNoRealizadas(){

		if(!gData.yaJugo) {

			gData.operaRealizadas = new bool[enemies.Length];

			for(int i=0; i < gData.operaRealizadas .Length; i++){

				gData.operaRealizadas [i] = false;
			}
		}

	}
		




	public int pasarNumeroDerecha(){
		
		numero1 = GeneradorNumeroRandom();

		return numero1 ;
	
	}

	public int pasarNumeroIzquierda(){

		numero2 = GeneradorNumeroRandomIzquierda();
	
		return numero2;
	
	}

	public int ObtenerSigno(){

		signo = GenerarSignoUIRandom ();
	
		return signo;
	
	}

	public void OperacionAritmetica(){

		if(signo == 0){
			
			resultado = numero1 * numero2;

		} else if(signo == 1){
		
			resultado = numero1 + numero2;
		
		}

		else if(signo == 2){
			
			operacionRestar ();
		}

		else if(signo == 3){
			
			operacionDividir ();
		
		}


	}


	void operacionRestar(){

		if (numero1 > numero2) {

			resultado = numero1 - numero2;
		
		} else if (numero2 > numero1) {

			resultado = numero2 - numero1;
		
		} else {

			resultado = numero1 - numero2;
		}

	}

	void operacionDividir(){

		float entera = numero2 / numero1;
		resultado = (int)entera;

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



	//Eligo dos respuestas al azar y agrego el resultado de la operacion
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

	public void DesactivarBotonRespuestas(){

		for(int i=0;  i < goRespuestas.Length; i++){

			goRespuestas [i].SetActive (false);

		}
	}


	public void ActivarBotonRespuestas(){

		for(int i=0;  i < goRespuestas.Length; i++){

			goRespuestas [i].SetActive (true);

		}
	}




	public void recibirTroll(GameObject untroll){

		trollActual = untroll;
		trollDeath = trollActual.GetComponent<Animator>();
		SetearActualEnemigos(trollActual);
	
	}



	// Todo sobre las vidas, fallos puntajes
	public void sumarPuntos(Item pItem){
		int itemValue = 0;

		switch (pItem) {

		case Item.Bone:

			itemValue = bigBoneValue; 
			break;

		case Item.Enemigos:

			itemValue = enemyValue;
			break;

		case Item.Orquitos:

			itemValue = orquitosValues;
			break;

		default:
			break;

		}

		gData.puntos += itemValue ;
		ui.txtPuntos.text = gData.puntos.ToString ();
	}

	public void SumarFallos(){
		
		gData.fallos++;
		txtFallos ();
		persController.DisminuirJump ();
	
	}

	public void txtFallos(){

		ui.textFallos.text = "Fallos: " + gData.fallos.ToString();
		if(gData.fallos == 5){
			restarVidas ();
		}
	}

	public void restarVidas(){
		CheckLives();
	}

	public void ActualizarUIVidas(){
		
		int tmp = gData.vidas;
		ui.vidasGo [tmp].SetActive (false);

	}

	public void RestaurarVidas(){

		for (int i = 0; i <= gData.vidas-1; i++) {

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


			//SaveData ();
			ActualizarUIVidas();
			SaveData ();
			Invoke ("RestartLevel", restartdevel);
			
		}
	
	}

	public void SumarBone(){

		gData.monedas++;
		gData.puntos += bigBoneValue;
		ui.txtPuntos.text = gData.puntos.ToString();
		ui.txMonedas.text = gData.monedas.ToString();

		if(gData.monedas == 100){
			gData.monedas = 0;
			ui.txMonedas.text = gData.monedas.ToString();
		
		}
	
	}


	public void GameOver(){
		ResetData ();
		pnMenuJuegoTerminado.SetActive (true);
	}


	public void RestartLevel(){

		SceneManager.LoadScene (gData.nivel.ToString());

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
			
			GameOver();

		} else if(timeLeft <= 20){

			ui.textTimer.color = Color.red;
		}
	}

	public void Salir(){
	
		Application.Quit();
	
	}

	public void EnemyStompsEnemy(GameObject enemy){

		// Change the enemy tag
		enemy.tag = "Untagged";

		//Destroy the enemy
		Destroy(enemy);
	

		sumarPuntos (Item.Orquitos);

	}



	public void PlayerDiedAnimaton (GameObject player){

		//Tomamos el Rigibody
		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

		rb.AddForce (new Vector2(-150f,900f));

		player.transform.Rotate (new Vector3(0f,0f,45f));

		player.GetComponent<PlayerController> ().enabled = false;

		foreach(Collider2D col in player.transform.GetComponents<Collider2D>()){
			col.enabled = false;
		}

		foreach(Transform tra in player.transform){
			tra.gameObject.SetActive (true);
		}

		Camera.main.GetComponent<MainCamera> ().enabled = false;

		rb.velocity = Vector2.zero;

		StartCoroutine ("PausaForReload", player);

	}

	IEnumerator PausaForReload(GameObject player){

		yield return new WaitForSeconds (1.0f);
		PlayerDies (player);

	}
		
	public void PlayerDies(GameObject player){

		gData.tiempoActual = timeLeft;


		SaveData ();

		player.SetActive (false);

		restarVidas();

	}

	public void EnemigosDerrotados(){

		for(int i=0; i < gData.operaRealizadas.Length; i++){

			if (gData.operaRealizadas [i] == true) {
				
				enemies [i].SetActive (false);
			} 

			else {
				
				enemigosActivos++;

			}	
		 
		}

	}


	public void CheckPointReached(Transform pos){
		gData.x = pos.position.x + 15f;
		gData.y = pos.position.y;
		gData.z = pos.position.z;
	}


	public void checkPointTXT(){
		StartCoroutine (MsjCheckpointAlcanzado());
	}


	IEnumerator MsjCheckpointAlcanzado(){
		
		ui.textCheckPoint.text = "Checkpoint ...";
		yield return new WaitForSeconds(2.0f);
		ui.textCheckPoint.text = " ";
	
	}

}
