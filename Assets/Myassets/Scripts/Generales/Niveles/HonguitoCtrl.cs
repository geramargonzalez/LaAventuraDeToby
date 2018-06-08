using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonguitoCtrl : MonoBehaviour {
	
	public Vector3 trasladar;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}

	void OnTriggerEnter2D(Collider2D other) {

		PlayerController ps;

		if (other.gameObject.tag == "Player") {

			ps = other.gameObject.GetComponent<PlayerController> ();

			//ps.TrasladarPersonaje (trasladar);

			this.gameObject.SetActive(false);
	

		}
	
	}


}
