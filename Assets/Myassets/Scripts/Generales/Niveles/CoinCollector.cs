using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour {


	public void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Bone")){
			
			Destroy (other.gameObject);
		}
	
	}
}
