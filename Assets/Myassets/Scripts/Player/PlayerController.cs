﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player controller.
/// Make moves player to right
///
/// </summary>
public class PlayerController : MonoBehaviour {

	Rigidbody2D rg;
	SpriteRenderer sp;

	[Tooltip("Esto es la velocidad para incrementar el movimiento")]
	public float speedBoost;

	public float jumpSpeed;

	private Animator anim;

	bool isJumping = false;

//	bool canDoublejump;
	public float delayDoubleJump;

	public bool isGrounded;
	public Transform feet;
	public float radiusfeet;
	public float boxWidth;
	public float boxHeight;
	public LayerMask whatIsGround;

	bool leftpressed;
	bool rightpressed;


	public GameObject Habilidadestatico;

	public UI ui;
	Animator animTxtHabilidad;
	Animator animTxtMsjHabilidad;

	bool cantfall;

	// Use this for initialization
	void Start () {
		
		rg = GetComponent<Rigidbody2D>();
		sp = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		Habilidadestatico.SetActive(false);
		animTxtMsjHabilidad = ui.txtMsjgrlHabilidad.GetComponent<Animator> ();
		SetearVelocidadAndJump ();

	}
		
	void Update () {
		isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x,feet.position.y),new Vector2(boxWidth,boxHeight),360.0f,whatIsGround);

		float speed = Input.GetAxisRaw ("Horizontal");
		speed *= speedBoost;

		if (speed != 0) {

			MoverHaciaAdelante (speed);
		
		} else {

			PararMovimiento (); 
		
		}
		if(Input.GetButtonDown("Jump")){

			Jump ();
		
		}
		Falling ();

		if(leftpressed){

			MoverHaciaAdelante(-speedBoost);
		
		}
		if (rightpressed) {

			MoverHaciaAdelante (speedBoost);
		
		}
	

	}

	public void OnBecameInvisible(){
		
	} 
		
	public void SetearVelocidadAndJump(){
		jumpSpeed = SistemaDejuego.instance.DevolverJump ();
		speedBoost = SistemaDejuego.instance.DevolverSpeed ();
	}

	public void MoverHaciaAdelante (float playerSpeed) {

		rg.velocity = new Vector2(playerSpeed,rg.velocity.y);
		Flip (playerSpeed);

		if(!isJumping){
			anim.SetInteger ("state",1);
		}
	}
	// 
	public void PararMovimiento () {
		rg.velocity = new Vector2(0,rg.velocity.y);

		if(!isJumping){
			anim.SetInteger ("state",0);
		}
	}


	public void Flip(float playerSpeed){
		if(playerSpeed < 0){

			sp.flipX = true;
		
		} else if(playerSpeed > 0){

			sp.flipX = false;
		
		}
	
	}

	public void Jump () {

		if(isGrounded){

			Debug.Log ("Jump: El salto esta en  " + jumpSpeed );

			rg.AddForce (new Vector2 (0, jumpSpeed)); // Solo hace saltar al personaje.
			isJumping = true;
			anim.SetInteger ("state",2);
		}
	}



	void EnableDoubleJump(){
//		canDoublejump = true;
	}


	void Falling(){
		if (rg.velocity.y < 0) {
			anim.SetInteger ("state", 3);
		} 
	}


	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.CompareTag("GROUND")){

			isJumping = false;

		} else if (other.gameObject.tag == "Enemigos" || other.gameObject.tag == "Orquito" || other.gameObject.tag == "Spike") {
			SistemaDejuego.instance.PlayerDiedAnimaton (gameObject);
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireCube (feet.position, new Vector3(boxWidth, boxHeight,0));
	}


	//Parte Movil Adaptacion
	public void MoveRight(){
		rightpressed = true;
		leftpressed = false;
	}
	public void MoveLeft(){
		leftpressed = true;
		rightpressed = false;
	}
	public void Stop(){
		rightpressed = false;
		leftpressed = false;
		PararMovimiento ();
	}
		
	public void MobileJump(){
		Jump ();
	}


	//Mejora/Empeora velocidad y salto
	public void AumentarJump(){
	
		jumpSpeed = jumpSpeed + 20f; 

		if (jumpSpeed < 1400) {
			ui.txtMsjgrlHabilidad.text = "Mejora: Salto";
			//ui.txtHabilidad.text = "SALTO ";
			SistemaDejuego.instance.GuardarEvolucionDeSaltoDePersonaje(jumpSpeed,speedBoost);
			StartCoroutine(mostrarHabilidad());
		
		} else if(jumpSpeed == 1400) {

			ui.txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada de salto";
			//txtHabilidad.text = " SALTO";
			SistemaDejuego.instance.GuardarEvolucionDeSaltoDePersonaje(jumpSpeed,speedBoost);
			StartCoroutine(mostrarHabilidad());
		}
	}

	public void AumentarSpeed(){

		speedBoost = speedBoost + 0.1f;

		if (speedBoost < 21) {

			ui.txtMsjgrlHabilidad.text = "Mejora: Velocidad";
			//ui.txtHabilidad.text = "Velocidad ";
			SistemaDejuego.instance.GuardarEvolucionDeSaltoDePersonaje(jumpSpeed,speedBoost);
			StartCoroutine(mostrarHabilidad());
		
		} else if(speedBoost == 17) {
			
			ui.txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada: Velocidad";
			//txtHabilidad.text = " Velocidad";
			StartCoroutine(mostrarHabilidad());

		}
	}

	public void DisminuirJump(){

		jumpSpeed = jumpSpeed - 0.3f;

		if(jumpSpeed >= 7){
			
			ui.txtMsjgrlHabilidad.text = "Disminuyo: Salto";
			StartCoroutine(mostrarHabilidad());
		
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
		//animTxtHabilidad.SetBool ("entrar",true);
		animTxtMsjHabilidad.SetBool ("entrar",true);
		yield return new WaitForSeconds(2.20f);
		Habilidadestatico.SetActive(false);
		ui.txtMsjgrlHabilidad.fontSize = 300;
	
	}



/*public void OnTriggerEnter2D(Collider2D other){

		switch (other.gameObject.tag )
		{
			case "Coin":

			if(sfxOn){
			  
				SFXCtrl.instance.showCoinSparkle (other.gameObject.transform.position);
			
				GameCtrl.instance.UpdateCoinCount ();
			
			}

			break;	
		
		case "Water":

			//

			garbajeCtrl.SetActive (false);

			SFXCtrl.instance.showSplash (other.gameObject.transform.position);

			GameCtrl.instance.PlayerDrowned ();

			break;	
		
		case "PowerUp_bullets":

			canFire = true;

			Vector2 powerUpos = other.gameObject.transform.position;

			Destroy (other.gameObject);

			if(sfxOn){

				SFXCtrl.instance.showSparkle(powerUpos);
			
			}

			break;	
		}
	}
*/

}
