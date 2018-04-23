using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO; 										  // Helps to work with files
using System.Runtime.Serialization.Formatters.Binary;	 // SRFB helps to work Serialization	
using DG.Tweening;

public class SistemaDejuego : MonoBehaviour {

	//Instancias y Tramas
	public static SistemaDejuego instance = null;

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
	int nivelLogrado;


	public float posicionXActual;


	// Variables que le dicen al enemigo que ataque.
    bool attack;
    bool generar = false;
	bool die;
	bool isPaused;
	bool timerOn;




	//Variables con la data del juego

	[HideInInspector]
	public GameData gData;

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

	// Animales y Orquitos
	public GameObject big_coin;
	public GameObject orquito;

	//Enemigo prefab
	public GameObject trollActual;


	//Interface Grafica
	public UI ui;

	int pos;
	float restartdevel;

	 int promedioPorNivel;
	 int fallosDelNivel;
     int  scoreNivel;

	// Los diferentes puntaje....
	int bigBoneValue = 10;
	int orquitosValues = 500;
	int enemyValue = 1000;

	int enemigosActivos;
	int promedioTotalDeOpeciones;



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
			

	}



	// Use this for initialization
	void Start () {

		DataCtrl.instance.RefreshData ();
		gData = DataCtrl.instance.data;
		RefreshUI ();

		camera = GameObject.Find("Main Camera");
		player = GameObject.Find ("Dog");

		persController = player.GetComponent<PlayerController>();
	
		attack = false;
		die = false;
		isPaused = false;
		timerOn = true;

		goRespuestas = GameObject.FindGameObjectsWithTag("btnRespuesta");

		Habilidadestatico.SetActive(false);
		animTxtMsjHabilidad = ui.txtMsjgrlHabilidad.GetComponent<Animator> ();

		Comenzar ();

	}

	public void RefreshUI () {


		if(gData.yaJugo == false){

			MarcarOrquitosEnEscena ();

			gData.jumpSpeed = 900f;

			gData.speedBoost = 20f;

			gData.GuardarPosicionInicial ();

			gData.numParaPromedio = gData.cantidadTrolls;


		}

		ui.txtBones.text = gData.bones.ToString ();  				

		ui.txtPuntos.text = gData.puntos.ToString ();

		ui.txtCantEnemigos.text = gData.cantidadTrolls.ToString();

	}





	// Update is called once per frame
	void Update () {
		
		GenerarEnemigosPorAcierto ();

		if(timeLeft > 0 && timerOn){

			UpdateTime();

		}
			

		if (isPaused == true) {

			Time.timeScale = 0;
		
		} else {

			Time.timeScale = 1;
		}
			
	}


	void OnEnable(){

		RefreshUI ();

	}

	void OnDisable(){
		DataCtrl.instance.SaveData (gData);
	}



	public void ResetData () {

		DataCtrl.instance.ResetData ();

		ui.txtPuntos.text = "0";
		ui.txtBones.text = "0";
		timeLeft = gData.tiempoActual;
		MarcarOrquitosEnEscena();
		RestaurarVidas ();

	}

	public void Comenzar(){

		if(gData.yaJugo == false){
			MsjNivelDeJuego();
		}

		crearnuevoTroll = false;

		OrcosAnimales ();

		player.transform.position = new Vector3 (gData.x, gData.y, gData.z);
	
		timeLeft = gData.ResetTime();

		if(Time.timeScale == 0){

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


	public int NivelActual(){
		return gData.nivel;

	}
		
	public int NivelLogrado (){
		return nivelLogrado + 1;
	}

	public int GetScore () {
		return gData.puntos;
	}


	public void SetNoJugo() {
		gData.yaJugo = false;
	}


	public void ProximoNivel(){
		gData.SetearNivelACtual ();
	}

	public void SetNivelRep(int levelNumber){
		gData.nivel = levelNumber;
		//DataCtrl.instance.SaveData (gData);
	}

	public GameData DataActual(){
		return gData;
	}
		
	public int obtenerPromedio (){
		return promedioPorNivel;
	}

	public int obtenerFallos(){
		return fallosDelNivel;
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
		gData.numParaPromedio = gData.cantidadTrolls;

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

			min = 3;
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
				
			if(gData.cantidadTrolls == 1){
					QuedaUnSoloTroll ();

				}

				if(gData.cantidadTrolls == 0){
					
					CeroTroll ();
					fallosDelNivel = gData.fallos;
					promedioPorNivel = gData.calcularPromedio ();
					nivelLogrado = gData.nivel;
					ProximoNivel ();
					DataCtrl.instance.SaveData (gData);

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

	}

	public void pasarNumeroIzquierda(){

		numero2 = GeneradorNumeroRandomIzquierda();

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

			gData.niveles[gData.nivel].aciertosMultiplicacion++;

		} else if(signo == 1){

			gData.niveles[gData.nivel].aciertosSuma++;

		}

		else if(signo == 2){

			gData.niveles[gData.nivel].aciertosResta++;
		}

		else if(signo == 3){

			gData.niveles[gData.nivel].aciertosDivision++;


		}


	}


	public void SumarFallosPorCuentas (){

		if(signo == 0){

			gData.niveles[gData.nivel].fallosMultiplicacion++;

		} else if(signo == 1){

			gData.niveles[gData.nivel].fallosSuma++;

		}

		else if(signo == 2){

			gData.niveles[gData.nivel].fallosResta++;
		}

		else if(signo == 3){

			gData.niveles[gData.nivel].fallosDivision++;

		}

	
	}

	public void ReciboTiempoParaPromedios(float tiempoOperacion){

		gData.niveles[gData.nivel].promedio += (int)tiempoOperacion;

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
			DataCtrl.instance.SaveData (gData);
			Invoke ("GameOver", restartdevel);
		
		} else {
			
			ActualizarUIVidas();
			DataCtrl.instance.SaveData (gData);
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
		if(timerOn){
			timerOn = false;
		}
		ui.pnMenuJuegoTerminado.gameObject.GetComponent<RectTransform> ().DOAnchorPosY (0, 0.7f, false);

	}


	public void RestartLevel(){
		SceneManager.LoadScene (gData.nivel.ToString());

	}


	public void PantallaTerminada(){
		
		if (gData.cantidadTrolls == 0) {




			 if(timerOn){
				timerOn = false;
			}

			ui.levelComplete.SetActive (true);

			//StartCoroutine(SetPaused());

		} else {

			FaltanEnemigos ();
		
		}

	}
		

	/// <summary>
	///  Metodos para cambiar de escena
	/// </summary>

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

		if(gnScript != null){
			gnScript.PararTiempoOperaciones ();
		}

		player.SetActive (false);
		restarVidas();

	}


	public void CheckPointReached(Transform pos){

		gData.x = pos.position.x + 15f;
		gData.y = pos.position.y;
		gData.z = pos.position.z;
		posicionXActual = gData.x; 

		DataCtrl.instance.SaveData (gData);

	
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
			ui.txtMsjgrlHabilidad.fontSize = 200;
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
			ui.txtMsjgrlHabilidad.fontSize = 200;
			ui.txtMsjgrlHabilidad.text = "Disminuyo: Salto";
			StartCoroutine (mostrarHabilidad ());

		} else {
		
			ui.txtMsjgrlHabilidad.text = "Minima capacidad de salto alcanzada";
			persController.SetearVelocidadAndJump ();
		
		}
	}

	public void QuedaUnSoloTroll(){
		ui.txtMsjgrlHabilidad.fontSize = 300;
		ui.txtMsjgrlHabilidad.text = "Ultimo Enemigo";
		StartCoroutine(mostrarHabilidad());
	}

	public void CeroTroll(){
		ui.txtMsjgrlHabilidad.fontSize = 160;
		ui.txtMsjgrlHabilidad.text = "Excelente!, ir al castillo";
		StartCoroutine(mostrarHabilidad());
	}

	public void FaltanEnemigos(){
		ui.txtMsjgrlHabilidad.fontSize = 160;
		ui.txtMsjgrlHabilidad.text = "Faltan enemigos";
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


	public void MsjNivelDeJuego(){
		ui.txtMsjgrlHabilidad.fontSize = 300;
		ui.txtMsjgrlHabilidad.color = Color.red;
		int tmp = 1 + gData.nivel;
		ui.txtMsjgrlHabilidad.text = "    Nivel actual:   " + tmp;
		StartCoroutine(mostrarHabilidad());
	}



	public int SetStarsAwarded (int levelNumber, int stars){
		
		return gData.niveles[levelNumber].bonesStars = stars;

	}

	public void Unlocked(int levelNumber){
	
		gData.niveles[levelNumber].unlocked = true ;
	}


	public void PausaShow (){
		
		ui.panelPausa.gameObject.GetComponent<RectTransform> ().DOAnchorPosY (0, 0.7f, false);

		PausarPantalla ();

	}


	public void PausaHide(){

		isPaused = false;

		ui.panelPausa.gameObject.GetComponent<RectTransform> ().DOAnchorPosY (5000f, 0.7f, false);


	}


	public void PausarPantalla(){
		StartCoroutine (SetPaused());
	}

	IEnumerator SetPaused(){
		
		yield return new WaitForSeconds(0.9f);
		isPaused = true;
	}


}
