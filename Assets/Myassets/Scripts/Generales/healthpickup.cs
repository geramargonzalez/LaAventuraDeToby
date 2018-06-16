using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthpickup : MonoBehaviour {


	void OnTriggerEnter2D(Collider2D other)
	{

		if(other.gameObject.CompareTag("Player"))
		{

			AudioCtrl.instance.PickUpHealth (gameObject.transform);
			SistemaDejuego.instance.VidaRecuperador();
			SistemaDejuego.instance.HealthGO(this.transform);
			Destroy(this.gameObject);

		}


	}
}
