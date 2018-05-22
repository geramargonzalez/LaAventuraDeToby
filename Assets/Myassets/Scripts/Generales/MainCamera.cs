using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {


			Transform objetivoCamara;


		   bool agreY;
		   float tmp;
	       float yPos;

	public float yOff;

	void Start(){

		objetivoCamara = GameObject.Find ("Dog").transform;

		if (SistemaDejuego.instance.posicionXActual < 3283.7) {

			agreY = false;
		
		} else {

			agreY = true;
		
		}

		yPos = this.transform.position.y;


	}

	// Update is called once per frame
	void Update () {

		if (!agreY) {
		
			transform.position = new Vector3 (objetivoCamara.position.x, yPos, transform.position.z);
		
		} else {

			transform.position = new Vector3 (objetivoCamara.position.x, objetivoCamara.transform.position.y + yOff, transform.position.z);
	

		}
	
	}
		

	public void setearYCamera(){
	
		agreY = true;
	
	}

	public void QuitarYCamera(){

		agreY = false;

	}

}
