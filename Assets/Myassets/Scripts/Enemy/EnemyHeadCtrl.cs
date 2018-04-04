using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 	Enemy head ctrl.
/// </summary>
public class EnemyHeadCtrl : MonoBehaviour {

	public GameObject enemy;

	void OnTriggerEnter2D(Collider2D coll){

		if (coll.gameObject.CompareTag ("PlayerFeet")) 
		{
			//GameCtrl.instance.EnemyStompsEnemy(enemy);
			SistemaDejuego.instance.EnemyStompsEnemy(enemy);

			SFXCtrl.instance.showPlayerLanding(coll.gameObject.transform.position);

		}
	
	}
}
