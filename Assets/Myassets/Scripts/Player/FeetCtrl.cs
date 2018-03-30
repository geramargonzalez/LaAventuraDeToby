using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		 Ayuda con las particulas de polvo cuando el personaje choca con el piso
/// </summary>
public class FeetCtrl : MonoBehaviour {

	public Transform dustParticlepos;
	PlayerController player;

	void Start(){
	
		player = gameObject.transform.parent.gameObject.GetComponent<PlayerController> ();
	
	}

	public void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("GROUND")) {

			SFXCtrl.instance.showPlayerLanding(dustParticlepos.position);
	
		}


	}


}
