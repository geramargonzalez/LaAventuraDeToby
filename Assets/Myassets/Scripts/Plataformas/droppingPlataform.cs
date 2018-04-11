using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Dropping plataform.
/// </summary>
public class droppingPlataform : MonoBehaviour {


	Rigidbody2D rg;
	public float droppingDelay;

	void Start () {
		rg = GetComponent<Rigidbody2D>();
	}
	


	void  OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("PlayerFeet")){
			Invoke ("DroppingDelay", droppingDelay);
		}
	}

	void DroppingDelay(){
		rg.isKinematic = false;
	}
}
