using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCtrl : MonoBehaviour {


	public void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.CompareTag ("Breakable")) {

			SFXCtrl.instance.HandleBoxBreaking(other.gameObject.transform.parent.transform.position);

			gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		
			Destroy(other.gameObject.transform.parent.gameObject);
		}
	}
}
