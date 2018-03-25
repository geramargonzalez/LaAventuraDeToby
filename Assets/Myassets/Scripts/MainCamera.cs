using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	// dejar publico :  se usa con el observador
	public Transform objetivoCamara;
	// ***********************************

	public float yOff;

	void Start(){
		objetivoCamara = GameObject.Find ("Dog").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (objetivoCamara.position.x,objetivoCamara.position.y + yOff,transform.position.z);
	}
}
