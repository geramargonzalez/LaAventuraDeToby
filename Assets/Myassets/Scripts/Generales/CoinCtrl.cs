using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCtrl : MonoBehaviour {

	public enum CoinFX
	{
		Vanish,
		Fly
	}

	public CoinFX coinFX;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.CompareTag("Player")){
			if(coinFX == CoinFX.Vanish){
				Destroy(gameObject);
			}
		}
	}
}
