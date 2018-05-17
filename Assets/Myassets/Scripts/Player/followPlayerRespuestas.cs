using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerRespuestas : MonoBehaviour {


	Transform bar;

	// ***********************************
	bool agreY;

	float tmp;
	float yPos;
	//	 Transform posInicial;
	public float yOff;

	void Start(){

		bar = GameObject.Find ("Dog").transform;

		agreY = false;

		yPos = this.transform.position.y;


	}

	// Update is called once per frame
	void Update () {


		// Es con el nivel 2 y no con el 0 acordarse de cambiar esto.
		if (SistemaDejuego.instance.posicionXActual > 3283.7 || DataCtrl.instance.data.nivel == 0) {

			agreY = true;

		} 
			
		if (!agreY) {
			

			transform.position = new Vector3 (bar.position.x, yPos, transform.position.z);
		
		} else {
			
			transform.position = new Vector3 (bar.position.x, bar.transform.position.y + yOff, transform.position.z);
		
		}

	}

}
