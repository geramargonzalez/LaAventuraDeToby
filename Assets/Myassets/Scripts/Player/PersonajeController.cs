using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonajeController : MonoBehaviour
{


	public float speed = 1f;
	public float maxSpeed = 5f;
	public float jumpPower = 10f;
	public float jumpMaxPower = 20f;
	public bool jump;
	private bool crouch;
	private bool shoot;
	public bool grounded;

	private Rigidbody2D rb2d;

	private Animator anim;
	private SpriteRenderer spritePersonaje;



	//public Text txtHabilidad;
	//public Text txtMsjgrlHabilidad;
	//public GameObject Habilidadestatico;

	//public Animator animTxtHabilidad;
	//public Animator animTxtMsjHabilidad;





	void Start ()
	{
		spritePersonaje = GetComponent<SpriteRenderer> ();
		//Habilidadestatico.SetActive(false);
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		//animTxtHabilidad = txtHabilidad.GetComponent<Animator> ();
		//animTxtMsjHabilidad = txtMsjgrlHabilidad.GetComponent<Animator> ();
	}
		
	void Update ()
	{
		


	}

	void FixedUpdate ()
	{


		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");


		anim.SetBool ("grounded", grounded);

		anim.SetFloat ("speed", Mathf.Abs (rb2d.velocity.x));

		if(!crouch && Input.GetKey (KeyCode.D)){
			
			spritePersonaje.flipX = false;
			rb2d.AddForce (Vector2.right * speed * h);
			//anim.SetBool ("Shoot", false);
		}
		if(!crouch && Input.GetKey (KeyCode.D) &&  Input.GetKey (KeyCode.E)){
			spritePersonaje.flipX = false;
			rb2d.AddForce (Vector2.right * speed * h);

		}

		if(!crouch && Input.GetKey (KeyCode.A)){
			spritePersonaje.flipX = true;
			rb2d.AddForce (Vector2.left * speed * Mathf.Abs(h));

		}


		float limiteVelocidad = Mathf.Clamp (rb2d.velocity.x, -maxSpeed, maxSpeed);
		rb2d.velocity = new Vector2 (limiteVelocidad, rb2d.velocity.y);

		if(v>jumpMaxPower){
			v = jumpMaxPower;
		}



		if (grounded) {
			jump = false;
		}

		if( !grounded && !jump ){
			
		}

		if (Input.GetKey (KeyCode.W) && grounded) {
				grounded = false;
				//Debug.Log (v);
				jump = true;
			rb2d.AddForce(new Vector2(0f , jumpPower+v),ForceMode2D.Impulse);//AddForce (Vector2.up * jumpPower, ForceMode2D.Impulse);
		}

		if (Input.GetKey (KeyCode.S)) {
			crouch = true;
			jump = false;
		} else {
			crouch = false;

		}

		anim.SetBool ("jump", jump);
		anim.SetBool ("Crouch", crouch);

	}


	/*public void AumentarJump(){
		if (jumpPower < 14) {
			jumpPower = jumpPower + 0.3f;
			txtMsjgrlHabilidad.text = "Aumento de capacidad: ";
			txtHabilidad.text = "SALTO ";

			StartCoroutine(mostrarHabilidad());
		} else if(jumpPower == 14) {
			txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada: ";
			txtHabilidad.text = " SALTO";

			StartCoroutine(mostrarHabilidad());
		}
	}

	public void AumentarSpeed(){
		if (speed < 17) {

			speed = speed + 0.3f;
			txtMsjgrlHabilidad.text = "Aumento de capacidad: ";
			txtHabilidad.text = "Velocidad ";
			StartCoroutine(mostrarHabilidad());
		
		} else if(speed == 17) {

			txtMsjgrlHabilidad.text = "Mayor capacidad alcanzada: ";
			txtHabilidad.text = " Velocidad";
			StartCoroutine(mostrarHabilidad());
		}
	}

	public void DisminuirJump(){
		if(jumpPower >= 6){
			jumpPower = jumpPower - 0.3f;
			txtMsjgrlHabilidad.text = "Disminuyo: ";
			txtHabilidad.text = "SALTO";
			StartCoroutine(mostrarHabilidad());
		} 
	}


	public void OnBecameInvisible(){
		transform.position = new Vector3(0,0,0);
	}


	IEnumerator mostrarHabilidad()
	{
		Habilidadestatico.SetActive(true);
		animTxtHabilidad.SetBool ("Entrar",true);
		animTxtMsjHabilidad.SetBool ("Entrar",true);
		yield return new WaitForSeconds(1.10f);
		Habilidadestatico.SetActive(false);
	
	}
*/
}
