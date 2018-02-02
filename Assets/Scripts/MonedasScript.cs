using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedasScript : MonoBehaviour {

	public GameObject gameManager;
	private SistemaDejuego sisJuego;
	private AudioSource audioCoins;


	void Start () {
		audioCoins = GetComponent<AudioSource>();
		sisJuego = gameManager.GetComponent<SistemaDejuego>();
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			audioCoins.Play();
			sisJuego.sumarMonedas();
			StartCoroutine(tiempodesactivar());
		}
	}

	IEnumerator tiempodesactivar(){
		yield return new WaitForSeconds(.1f);
		this.gameObject.SetActive (false);
	}
}
