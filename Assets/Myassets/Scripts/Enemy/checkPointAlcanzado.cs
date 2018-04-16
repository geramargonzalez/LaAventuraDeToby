using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointAlcanzado : MonoBehaviour {


	Transform position;

	void Start(){
		position = this.transform;
	}


	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")){

			//Debug.Log (position.position.x + position.position.y);
			SistemaDejuego.instance.CheckPointReached(position);
			SistemaDejuego.instance.checkPointTXT();
		
		}
	}

}
