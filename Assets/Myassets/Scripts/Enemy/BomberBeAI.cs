using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BomberBeAI : MonoBehaviour {

	public float beeDestroydelay;
	public float beeSpeed;


	public void ActivatedBee(Vector3 playerpos){
		transform.DOMove (playerpos, beeSpeed, false);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("GROUND") || other.gameObject.CompareTag("Player"))
		{
			//SFXCtrl.instance.EnemyExplosion (other.gameObject.transform.position);
			Destroy (this.gameObject, beeDestroydelay);

		}

		if (other.gameObject.CompareTag ("Player")) {
		
			SistemaDejuego.instance.PlayerDies (other.gameObject);
		}

	}

}
