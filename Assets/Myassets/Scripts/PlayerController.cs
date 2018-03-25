using System.Collections;
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

	bool leftpressed;
	bool rightpressed;


	//public Text txtHabilidad;
	public Text txtMsjgrlHabilidad;
	public GameObject Habilidadestatico;

	Animator animTxtHabilidad;
	Animator animTxtMsjHabilidad;

	bool cantfall;

	// Use this for initialization
	void Start () {

		rg = GetComponent<Rigidbody2D>();
		sp = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		Habilidadestatico.SetActive(false);
		animTxtMsjHabilidad = txtMsjgrlHabilidad.GetComponent<Animator> ();

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
		transform.position = new Vector3(0,0,0);
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
		if (jumpSpeed < 1400) {
			jumpSpeed = jumpSpeed + 20f;
			txtMsjgrlHabilidad.text = "Aumento de capacidad: Salto";
			//txtHabilidad.text = "SALTO ";
			StartCoroutine(mostrarHabilidad());
		} else if(jumpSpeed == 1400) {
			txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada: Salto";
			//txtHabilidad.text = " SALTO";

			StartCoroutine(mostrarHabilidad());
		}
	}

	public void AumentarSpeed(){
		if (speedBoost < 21) {
			speedBoost = speedBoost + 0.1f;
			txtMsjgrlHabilidad.text = "Aumento de capacidad: Velocidad";
			//txtHabilidad.text = "Velocidad ";
			StartCoroutine(mostrarHabilidad());
		
		} else if(speedBoost == 17) {

			txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada: Velocidad";
			//txtHabilidad.text = " Velocidad";
			StartCoroutine(mostrarHabilidad());
		}
	}

	public void DisminuirJump(){
		if(jumpSpeed >= 7){
			jumpSpeed = jumpSpeed - 0.3f;
			txtMsjgrlHabilidad.text = "Disminuyo: Salto";
			//txtHabilidad.text = "SALTO";
			StartCoroutine(mostrarHabilidad());
		} 
	}



	IEnumerator mostrarHabilidad()
	{
		Habilidadestatico.SetActive(true);
		//animTxtHabilidad.SetBool ("Entrar",true);
		//animTxtMsjHabilidad.SetBool ("Entrar",true);
		yield return new WaitForSeconds(2.10f);
		Habilidadestatico.SetActive(false);
	
	}

	public void TrasladarPersonaje(Vector3 trasladar){
		StartCoroutine (TiempoTrasladoPersonaje(trasladar));
	}

	IEnumerator TiempoTrasladoPersonaje(Vector3 trasladar){
		yield return new WaitForSeconds(1.0f);
		transform.position = trasladar;
	}
}
