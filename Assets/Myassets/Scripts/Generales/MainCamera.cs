using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	// dejar publico :  se usa con el observador
	public Transform objetivoCamara;
	// ***********************************
	public bool agreY;

	 float tmp;


	public float yOff;

	void Start(){
		agreY = false;
		AgregarY();
		objetivoCamara = GameObject.Find ("Dog").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (objetivoCamara.position.x, tmp , transform.position.z);
	}


	public void AgregarY(){
		if (agreY) {
			tmp = objetivoCamara.transform.position.y + yOff;
		} else {
			tmp = this.transform.position.y;
		}
	}
}
