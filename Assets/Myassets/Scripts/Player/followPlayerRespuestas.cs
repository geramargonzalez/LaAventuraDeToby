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

		if (SistemaDejuego.instance.posicionXActual > 3283.7) {

			agreY = true;

		} 


		if (!agreY) {
			transform.position = new Vector3 (bar.position.x, yPos, transform.position.z);
		} else {
			transform.position = new Vector3 (bar.position.x, bar.transform.position.y - yOff, transform.position.z);
		}

	}

}
