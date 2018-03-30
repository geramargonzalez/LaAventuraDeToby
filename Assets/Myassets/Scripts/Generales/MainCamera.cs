using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	// dejar publico :  se usa con el observador
	 Transform objetivoCamara;
	// ***********************************
	 bool agreY;

	 float tmp;
	float yPos;
//	 Transform posInicial;


	public float yOff;

	void Start(){
		agreY = false;
		yPos = this.transform.position.y;
		objetivoCamara = GameObject.Find ("Dog").transform;

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
		//transform.position = new Vector3 (objetivoCamara.position.x, this.transform.position.y, transform.position.z);
	}

}
