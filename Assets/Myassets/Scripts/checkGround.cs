using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkGround : MonoBehaviour {

	// Use this for initialization
	private PersonajeController personaje;

	void Start () {
		personaje = GetComponentInParent<PersonajeController> ();
	}


	void OnCollisionStay2D(Collision2D col){
		if (col.gameObject.tag != "Vertical") {
			personaje.jump = false;
			personaje.grounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D col){
		personaje.grounded = false;
	}


}
