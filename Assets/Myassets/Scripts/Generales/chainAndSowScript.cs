using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainAndSowScript : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {

		anim = this.gameObject.GetComponent<Animator> ();
		anim.SetBool ("entrar",true);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
