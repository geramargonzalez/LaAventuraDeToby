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

	bool canDoublejump;
	public float delayDoubleJump;

	public bool isGrounded;
	public Transform feet;
	public float radiusfeet;
	public float boxWidth;
	public float boxHeight;
	public LayerMask whatIsGround;

//	public Transform leftBulletSpawnPos, rightBulletSpawnPos;
	//public GameObject leftBullet, rightBullet;

	bool leftpressed;
	bool rightpressed;

//	public bool sfxOn;
//	public bool canFire;

	//public Text txtHabilidad;
	//public Text txtMsjgrlHabilidad;
	//public GameObject Habilidadestatico;

	//public Animator animTxtHabilidad;
	//public Animator animTxtMsjHabilidad;

	bool cantfall;

	// Use this for initialization
	void Start () {
		rg = GetComponent<Rigidbody2D>();
		sp = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();


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
			rg.AddForce (new Vector2 (0, jumpSpeed)); // Solo hace saltar al personaje.
			isJumping = true;
			anim.SetInteger ("state",2);
			//Invoke("EnableDoubleJump",delayDoubleJump);
		}

		/*if(canDoublejump && !isGrounded){
			rg.velocity = Vector2.zero;
			rg.AddForce (new Vector2 (0, jumpSpeed)); // Solo hace saltar al personaje.
			anim.SetInteger ("state",2);
			canDoublejump = false;
		}*/
	}



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

	void EnableDoubleJump(){
		canDoublejump = true;
	}


	void Falling(){
		if (rg.velocity.y < 0) {
			anim.SetInteger ("state", 3);

		} 
	}

	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.CompareTag("GROUND")){
			isJumping = false;
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireCube (feet.position, new Vector3(boxWidth, boxHeight,0));
	}




}
