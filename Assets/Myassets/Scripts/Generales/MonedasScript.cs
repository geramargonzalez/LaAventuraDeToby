using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonedasScript : MonoBehaviour {

	public GameObject gameManager;
	private SistemaDejuego sisJuego;

	private AudioSource audioCoins;

	public enum CoinFX
	{
		Vanish,

		Fly
	}

	public CoinFX coinFX;

	public float speed;

	public bool startFlying;

	GameObject coinMeter;

	void Start () {

		audioCoins = GetComponent<AudioSource> ();
		sisJuego = gameManager.GetComponent<SistemaDejuego>();

		startFly ();
	
	}

	// Update is called once per frame
	void Update () {

		if(startFlying){

			transform.position = Vector3.Lerp (transform.position,coinMeter.transform.position,speed);

		}

	}


	public void startFly(){
		
		startFlying = false;

		if(CoinFX.Fly == coinFX){

			coinMeter = GameObject.Find ("BoneCollector");

		}
	}


	void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.tag == "Player") {

			audioCoins.Play();
		
			SistemaDejuego.instance.SumarBone ();
		
			if(coinFX == CoinFX.Vanish){

				Destroy(gameObject);

			} else if(coinFX == CoinFX.Fly){

				startFlying = true;

			}

		}
	}


}
