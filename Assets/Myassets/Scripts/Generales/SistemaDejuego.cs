using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO; 										  // Helps to work with files
using System.Runtime.Serialization.Formatters.Binary;	 // SRFB helps to work Serialization	

public class SistemaDejuego : MonoBehaviour {

	//Instancias y Tramas
	public static SistemaDejuego instance;

	//Numeros minimos y maximos para las tablas, respuestas
	public int min, max, minIzq, maxIzq, minResp, maxResp;

	int numero1, numero2, resultado;

	List<string> respuestas = new List<string>();
	string resulString;

	//Todos los enemigos por Pantalla o Operaciones a derrotar
	GameObject[] goRespuestas;
	Transform tmp;
	public GameObject[] posicionesEnemigos;
	Transform  posEnemigoActual;

	public List<Text> txtOpciones = new List<Text>();
	private List<Transform> posiciones = new List<Transform>();


	int signo;
	int posiDelaTablaAcultar;




	public float posicionXActual;



	// Variables que le dicen al enemigo que ataque.
    bool attack;
    bool generar = false;
	bool die;


	//Variables con la data del juego
	public GameData gData;

	// GAMEOVER
	public GameObject pnMenuJuegoTerminado;


	GameObject camera;

	// DataBase variables
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

	public GameObject big_coin;
	public GameObject orquito;

	//Enemigo prefab
	public GameObject trollActual;


	//Interface Grafica
	public UI ui;

	int pos;
	float restartdevel;



	// Los diferentes puntaje....
	int bigBoneValue = 10;
	int orquitosValues = 500;
	int enemyValue = 1000;
	int enemigosActivos;



	// OPERACIONES CONCRETADAS Y QUE SE CREE UN NUEVO ENEMIGO
	bool operacionConcretada;
	bool crearnuevoTroll;


	//Orquitos y Animales
	public GameObject[] posOrcosAnimales;


	Animator animTxtHabilidad;
	Animator animTxtMsjHabilidad;
	public GameObject Habilidadestatico;


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
	
		player = GameObject.Find ("Dog");

		persController = player.GetComponent<PlayerController>();
	
		attack = false;
		die = false;

		goRespuestas = GameObject.FindGameObjectsWithTag("btnRespuesta");

		//posOrcosAnimales = GameObject.FindGameObjectsWithTag("orcosAnimales");

		Habilidadestatico.SetActive(false);
		animTxtMsjHabilidad = ui.txtMsjgrlHabilidad.GetComponent<Animator> ();

		Comenzar ();

	}

	// Update is called once per frame
	void Update () {
		
		GenerarEnemigosPorAcierto ();

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

	
			ui.txtBones.text = gData.bones.ToString ();  				

			ui.txtPuntos.text = gData.puntos.ToString ();

			ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString();


			if(!gData.yaJugo){

				gData.jumpSpeed = 900f;

				gData.speedBoost = 20f;

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
	
		gData.bones = 0;

		gData.puntos = 0;

		gData.fallos = 0;

		ui.txtPuntos.text = "0";

		ui.txtBones.text = "0";

		gData.posActualEnemigo = 0;

		gData.tiempoActual = gData.ResetTime ();

		timeLeft = gData.tiempoActual;

		gData.yaJugo = false;

		gData.jumpSpeed = 900f;

		gData.speedBoost = 20f;


		gData.fallosMultiplicacion = 0;

		gData.fallosSuma = 0;

		gData.fallosResta = 0;
	
		gData.fallosDivision = 0;

		gData.aciertosMultiplicacion = 0;

		gData.aciertosSuma = 0;

		gData.aciertosResta = 0;

		gData.aciertosDivision = 0;


		MarcarOrquitosEnEscena();

		RestaurarVidas ();
	

		bf.Serialize (fs, gData);

		fs.Close ();										 

	}

	public void Comenzar(){

		crearnuevoTroll = false;

		OrcosAnimales ();

		player.transform.position = new Vector3 (gData.x, gData.y, gData.z);
	
		timeLeft = gData.tiempoActual;

		if(!pnMenuJuegoTerminado.activeSelf){

			Time.timeScale = 1f;
		
		}

		posicionXActual = gData.x;
		MarcarOperacionesNoRealizadas ();
		GenerarEnemigosPorComienzo ();
		RestaurarVidas ();
	
	}

	//Genera los enemigos actuales
	public void GenerarEnemigosPorComienzo(){

		if(!gData.operaRealizadas[gData.posActualEnemigo] && gData.posActualEnemigo <= posicionesEnemigos.Length-1){

			Instantiate (trollActual, posicionesEnemigos[gData.posActualEnemigo].transform.position, Quaternion.identity);

		
		} 
	
	}



	//Genera los enemigos actuales
	public void GenerarEnemigosPorAcierto(){

		if (crearnuevoTroll) {

			gData.posActualEnemigo = gData.posActualEnemigo + 1;

			pos = gData.posActualEnemigo;	

			if (pos <= posicionesEnemigos.Length - 1) {

				Instantiate (trollActual, posicionesEnemigos [pos].transform.position, Quaternion.identity);

				crearnuevoTroll = false;

			} else {
				
				gData.posActualEnemigo = 0;
			}

		
		} 

	  }




	public void SetearCrearNuevoTroll(bool crear){
		crearnuevoTroll = crear;
	}


	//Le paso la ultima cuenta realizada.
	public void ObtenerEnemigoActual(GameObject posActual){
	
		posEnemigoActual = posActual.transform;
		enemy = posActual;
		gnScript = enemy.GetComponent<EnemyScript> ();
	
	}


	// Setea la cantidad de operaciones a realizar
	public void OperacionesAritmeticasCompletadas(){

		//Debug.Log ("Entro: OperacionesAritmeticasCompletadas ");
		float restaDePosiciones;

		for (int i = 0; i < posicionesEnemigos.Length; i++) {

			restaDePosiciones =  posicionesEnemigos [i].transform.position.x - posEnemigoActual.transform.position.x;


			if ( (int)restaDePosiciones >= 0  && (int)restaDePosiciones <= 50) {

			
				gData.operaRealizadas [i] = true;

			
			}
		}	

	}
	
	// Poner Todas las operaciones no realizadas
	public void MarcarOperacionesNoRealizadas (){

		if (!gData.yaJugo) {

			gData.posActualEnemigo = 0;

			gData.operaRealizadas = new bool[posicionesEnemigos.Length];

			for (int i = 0; i < gData.operaRealizadas.Length; i++) {

				gData.operaRealizadas [i] = false;
				enemigosActivos++;
			}
		
		} else {

			for (int i = 0; i < gData.operaRealizadas.Length; i++) {

				if(!gData.operaRealizadas[i]){

					enemigosActivos++;	
				
				}
	
			}
		
		}

		gData.cantidadTrolls = enemigosActivos;
		ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString();

	}



	public int GenerarSignoUIRandom(){
		
		float random = Random.Range(0f,4f);

		return (int)random;
	}

	public int GeneradorNumeroRandom(){
		 NivelDejuego();
		 float random = Random.Range((float)min,(float)max);
		 return (int)random;
	}

	public int GeneradorNumeroRandomIzquierda(){
		float random = Random.Range((float)minIzq,(float)maxIzq);
		return (int)random;
	
	}

	public int GeneradorNumeroRandomRespuesta(){
		float random = Random.Range((float)minResp,(float)maxResp);
		return (int)random;
	}


	public string GenerarSignoParaRespuestas(){

		float random = Random.Range(0.0f,4.0f);
		string psigno = "";

		if((int)random == 0){

			psigno = "X";

		}else if((int)random == 1){

			psigno = "+";

		}if((int)random == 2){

			psigno = "-";

		} if((int)random == 3){

			psigno = "/";

		}

		return psigno;

	}

	//Nivel actual del jugador.
	public void NivelDejuego(){

		if (gData.nivel == 0){

			min = 1;
			max = 4;
			minIzq = 1;
			maxIzq = 11;
			minResp = 1;
			maxResp = 50;
		
		}  else if (gData.nivel == 1){
			
			max = 7;
			maxIzq = 21;
			maxResp = 100;
		
		} else if (gData.nivel == 2){
		

			max = 11;
			maxIzq = 21;
			maxResp = 150;
		
		} else if (gData.nivel == 3){
			
			max = 21;
			maxIzq = 31;
			maxResp = 200;
		}

	}

	public void EleccionTabla(){

		ElegirElementoDelaOperacionaOcultar ();

		OkGenerar();

		if(generar){

			IngresarRespuestas();
		
			generar = false;
		
		}

	}

	 public void ResultadoOperacion(int num){

		//Deja la huella que ya jugo.
		if(!gData.yaJugo){
		
			gData.yaJugo = true;
		
		}
			

		if (resulString == respuestas [num]) {

				if(attack){

					attack = false;

				}

				AumentarJump();
				
				LimpiarRespuestas ();
				DesactivarBotonRespuestas ();

				gData.cantidadTrolls--;
				ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString ();
				
				SumarAciertosPorCuentas ();
				sumarPuntos (Item.Enemigos);
				OperacionesAritmeticasCompletadas();
				gnScript.RecibirResultado (resulString);
				//die = true;
				if(gData.cantidadTrolls == 1){
					QuedaUnSoloTroll ();
				}

				if(gData.cantidadTrolls == 0){
					CeroTroll ();
				}


		  } else {
			
			    attack = true;
				SumarFallos();
	
			}
	}


	public bool obtenerAttack(){
		return attack;
	}

	public bool matarTroll(){
		return die;
	}

	public void SetAttack(bool pAttack){
		attack = false;
	}

	public void SetDie(bool pDie){
		die = pDie;
	}



	public void pasarNumeroDerecha(){
		
		numero1 = GeneradorNumeroRandom();

	//	Debug.Log ("El numero 1 es " + numero1);

	}

	public void pasarNumeroIzquierda(){

		numero2 = GeneradorNumeroRandomIzquierda();

		//Debug.Log ("El numero 2 es " + numero2);
	
	
	}

	// Primero eligo el signo
	public void ObtenerSigno(){
		
		signo = GenerarSignoUIRandom ();
	

	}

	// Segundo realizo la operacion 
	public void OperacionAritmetica(){

		pasarNumeroDerecha ();
		pasarNumeroIzquierda ();
		OrdenarNum1YNumero2 ();
		ObtenerSigno ();

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

		resultado = numero1 - numero2;
	
	}

	void operacionDividir(){

		float entera = numero1 / numero2;

		//Debug.Log ("Division: " + numero2 + " / " + numero1 + " = " +  entera);

		resultado = (int)entera;

	}

	// Tercero eligo la operacion a ocultar
	public void ElegirElementoDelaOperacionaOcultar(){

		float posElemento = Random.Range (0f,4f);
		posiDelaTablaAcultar = (int)posElemento;

		if(posiDelaTablaAcultar == 0){

			resulString = numero1.ToString();
		
		} else if(posiDelaTablaAcultar == 1){
		
			resulString = elegirElSigno();
		
		}else if(posiDelaTablaAcultar == 2){
		
			resulString = numero2.ToString();

		} else if(posiDelaTablaAcultar == 3){

			resulString = resultado.ToString();
		} 

	}


	public string elegirElSigno(){

	    string psigno = " ";

		if(signo == 0){
		   
			psigno = "X";
		
		}else if(signo == 1){

			psigno = "+";
	
		}if(signo == 2){
		
			psigno = "-";
		
		} if(signo == 3){
		
			psigno = "/";
		
		}
	
		return psigno;
	}


	public void OrdenarNum1YNumero2(){

		int tmp;
		// Numero1 debe ser mayor que numero2
		if(numero1 < numero2){
			tmp = numero1;
			numero1 = numero2;
			numero2 = tmp;
		}

	}

	public string devolverNumero1(){

		string tmp = "_"; 

		if (posiDelaTablaAcultar != 0) {

			tmp = numero1.ToString ();

		} 

		return tmp;
	}



	public string devolverSigno(){

		string tmp = "_"; 
	
		if (posiDelaTablaAcultar != 1 ) {

			tmp = elegirElSigno();

		} 

		return tmp;
	}



	public string devolverNumero2(){

		string tmp = "_"; 

		if (posiDelaTablaAcultar != 2) {

			tmp = numero2.ToString ();

		} 

		return tmp;
	}


	public string devolverResultado(){

		string tmp = "_"; 

		if (posiDelaTablaAcultar != 3) {

			tmp = resultado.ToString ();

		} 

		return tmp;
	}

	//Eligo dos respuestas al azar y agrego el resultado de la operacion
	public void IngresarRespuestas(){

		List<string> arrDes = new List<string>();

		int tmp = 0;		
		string tmpS = "";

		for(int i = 0;  i < 3; i++){

			if(i < 2){

				if (posiDelaTablaAcultar == 1) {
					
					tmpS = GenerarSignoParaRespuestas ();
					arrDes.Add(tmpS);
				
				} else {
					
					tmp = GeneradorNumeroRandomRespuesta();
					arrDes.Add(tmp.ToString());
				}


			} else if(i == 2){
				
				arrDes.Add(resulString);

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
		
			txtOpciones[i].text = respuestas[i];
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


	public int obtonerResultado(){
		return resultado;
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
		SumarFallosPorCuentas ();
		DisminuirJump ();
	
	}

	public void txtFallos(){

		ui.textFallos.text = "Fallos: " + gData.fallos.ToString();
		if(gData.fallos == 5){
			restarVidas ();
		}
	}

	public void SumarAciertosPorCuentas(){

		if(signo == 0){

			gData.aciertosMultiplicacion++;

		} else if(signo == 1){

			gData.aciertosSuma++;

		}

		else if(signo == 2){

			gData.aciertosResta++;
		}

		else if(signo == 3){

			gData.aciertosDivision++;


		}


	}


	public void SumarFallosPorCuentas (){

		if(signo == 0){

			gData.fallosMultiplicacion++;

		} else if(signo == 1){

			gData.fallosSuma++;

		}

		else if(signo == 2){

			gData.fallosResta++;
		}

		else if(signo == 3){

			gData.fallosDivision++;

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

		gData.bones++;
		gData.puntos += bigBoneValue;
		ui.txtPuntos.text = gData.puntos.ToString();
		ui.txtBones.text = gData.bones.ToString();

		if(gData.bones == 100){
		
			gData.bones = 0;
			ui.txtBones.text = gData.bones.ToString();
		
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

		if (gData.cantidadTrolls == 0) {
		

			MsjPantallaFinalizada ();
		
		} else {
		
			MsjPantallaNoFinalizada();
		
		}
	}

	public void PantallaTerminada(){
		gData.subirNivel ();
		SaveData();
		Cambiarescena();
	}
		
	public void Cambiarescena(){
		StartCoroutine (CambioDePantalla());
	}

	IEnumerator CambioDePantalla(){
		yield return new WaitForSeconds(2.20f);
		SceneManager.LoadScene (gData.nivel.ToString());
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

		gData.cantAnimalesConvertidos++;

		enemy.tag = "Untagged";

		Vector3 posNew = enemy.transform.position;

		posNew.z = 20f;

		Instantiate(big_coin, posNew, Quaternion.identity);

		SFXCtrl.instance.EnemyExplosion(posNew);

		Destroy(enemy);
	
		ConvertirAnimal(enemy.transform);

		sumarPuntos (Item.Orquitos);

	}



	public void PlayerDiedAnimaton (GameObject player){

		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

		rb.AddForce (new Vector2(-150f,900f));

		player.transform.Rotate (new Vector3(0f,0f,45f));

		player.GetComponent<PlayerController> ().enabled = false;

		// Busca el collider para desabilitarlo
		foreach(Collider2D col in player.transform.GetComponents<Collider2D>()){
			col.enabled = false;
		}

		// 
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

		//if(gData.posActualEnemigo > 0){
		
			//gData.posActualEnemigo = gData.posActualEnemigo - 1;
		
	//	}
			

		player.SetActive (false);
		restarVidas();

	}


	public void CheckPointReached(Transform pos){

		gData.x = pos.position.x + 15f;
		gData.y = pos.position.y;
		gData.z = pos.position.z;
		posicionXActual = gData.x; 
		SaveData();
	
	}


	public void checkPointTXT(){

		StartCoroutine (MsjCheckpointAlcanzado());
	
	}


	IEnumerator MsjCheckpointAlcanzado(){

		ui.textCheckPoint.text = "Checkpoint ...";
		yield return new WaitForSeconds(2.0f);
		ui.textCheckPoint.text = " ";
	
	}

	public void GuardarEvolucionDeSaltoDePersonaje(float pJump, float pSpeed){
		
		gData.jumpSpeed = pJump;
		gData.speedBoost = pSpeed;

	}

	public float DevolverJump(){

		return gData.jumpSpeed;
	
	
	}

	public float DevolverSpeed(){

		return gData.speedBoost;
	
	
	}


	// Poner los orquitos en escena
	public void MarcarOrquitosEnEscena(){

		gData.orcosPorAnimales = new bool[posOrcosAnimales.Length];

		for (int i = 0; i < gData.orcosPorAnimales.Length; i++) {
				
			gData.orcosPorAnimales[i] = false;
				

		}

	}

	public void OrcosAnimales(){

		for (int i = 0; i < gData.orcosPorAnimales.Length; i++) {

			if (gData.orcosPorAnimales[i]) {

				Instantiate (big_coin, posOrcosAnimales[i].transform.position, Quaternion.identity);
			
			} else {

				Instantiate (orquito, posOrcosAnimales [i].transform.position, Quaternion.identity);
			
			}

		}

	
	}

	public void ConvertirAnimal(Transform pos){

		float restadepos;

		for (int i = 0; i < posOrcosAnimales.Length; i++) {

		
			restadepos = posOrcosAnimales [i].transform.position.x - pos.position.x;


			if((int)restadepos >= -30 && (int)restadepos <= 30){

				gData.orcosPorAnimales [i] = true;

						
			}
		}

	}


	//Mejora/Empeora velocidad y salto

	public void AumentarJump(){

		gData.jumpSpeed = gData.jumpSpeed + 20f; 

		if (gData.jumpSpeed < 1400) {

			ui.txtMsjgrlHabilidad.text = "Mejora: Salto";
			StartCoroutine(mostrarHabilidad());

		} else if(gData.jumpSpeed == 1400) {

			ui.txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada de salto";
			persController.SetearVelocidadAndJump ();
			StartCoroutine(mostrarHabilidad());
		}
	}





	public void AumentarSpeed(){

		gData.speedBoost = gData.speedBoost + 0.1f;

		if (gData.speedBoost < 21) {

			ui.txtMsjgrlHabilidad.text = "Mejora: Velocidad";
			StartCoroutine(mostrarHabilidad());

		} else if(gData.speedBoost == 17) {

			ui.txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada: Velocidad";
			StartCoroutine(mostrarHabilidad());

		}
	}

	public void DisminuirJump(){

		gData.jumpSpeed = gData.jumpSpeed - 0.3f;

		if (gData.jumpSpeed >= 7) {

			ui.txtMsjgrlHabilidad.text = "Disminuyo: Salto";
			StartCoroutine (mostrarHabilidad ());

		} else {
		
			ui.txtMsjgrlHabilidad.text = "Minima capacidad de salto alcanzada";
			persController.SetearVelocidadAndJump ();
		
		}
	}

	public void QuedaUnSoloTroll(){
		ui.txtMsjgrlHabilidad.text = "Ultimo Troll";
		StartCoroutine(mostrarHabilidad());
	}

	public void CeroTroll(){
		ui.txtMsjgrlHabilidad.fontSize = 200;
		ui.txtMsjgrlHabilidad.text = "Moverse hacia el castillo";
		StartCoroutine(mostrarHabilidad());
	}


	IEnumerator mostrarHabilidad()
	{
		Habilidadestatico.SetActive(true);
		animTxtMsjHabilidad.SetBool ("entrar",true);
		yield return new WaitForSeconds(2.20f);
		Habilidadestatico.SetActive(false);
		ui.txtMsjgrlHabilidad.fontSize = 300;

	}

	public void MsjPantallaFinalizada(){
		ui.txtMsjgrlHabilidad.fontSize = 200;
		ui.txtMsjgrlHabilidad.text = "Completaste el nivel";
		StartCoroutine(mostrarHabilidad());
	}


	public void MsjPantallaNoFinalizada(){
		ui.txtMsjgrlHabilidad.fontSize = 150;
		ui.txtMsjgrlHabilidad.text = "Te quedan operaciones por realizar";
		StartCoroutine(mostrarHabilidad());
	}





}
