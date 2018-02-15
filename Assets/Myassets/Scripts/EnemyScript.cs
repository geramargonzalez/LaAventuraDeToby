using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	Animator anim;
	 GameObject target;
	float speed;
	public GameObject gameManager;
	private SistemaDejuego sisJuego;


	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		sisJuego = gameManager.GetComponent<SistemaDejuego>();
		target = GameObject.Find ("Dog");
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void ocultarTroll(){
		this.gameObject.SetActive(false);
		anim.SetBool ("Die", false);

	}

	public void atacarTarget(){
	
	}

	public void finAttack(){
		sisJuego.attack = false;
		anim.SetBool ("Attack", sisJuego.attack);
	}
}
