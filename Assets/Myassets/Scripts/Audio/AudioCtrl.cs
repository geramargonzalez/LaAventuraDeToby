using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour {

	public static AudioCtrl instance;
	public PlayerAudio playerAudio;
	public EnemyAudio enemyAudio;
	public MultiploAudio multAudio;

	public AudioEffects audioEffects;
	public Transform player;
	public GameObject bsos;

	public bool audioOn;

	// Use this for initialization
	void Start () {

		if(instance == null){
		
			instance = this;

		}



		if (audioOn) {

			bsos.SetActive (true);

		} else {

			bsos.SetActive (false);
		
		}

	}

	public void EnemyHit(Transform playerPos){

		if(audioOn){

			Vector3 pos = new Vector3 (playerPos.position.x, playerPos.position.y, -3f);
			AudioSource.PlayClipAtPoint (playerAudio.enemyHit1, pos);
			//GameObject go = Instantiate(playerAudio.hit, playerPos.position, Quaternion.identity);
			//StartCoroutine(destruirdespuesSeg(go));
		}

	}


	public void PlayerDied(Transform playerPos){

		if(audioOn){

			//AudioSource.PlayClipAtPoint (playerAudio.playerDied, playerPos.position);
			Instantiate(playerAudio.playerDied, playerPos.position, Quaternion.identity);


		}

	}

	public void PickUpHealth(Transform playerPos){

		if(audioOn){

			//AudioSource.PlayClipAtPoint (enemyAudio.trollDeath, enemyPos.position);
			GameObject go = Instantiate(playerAudio.pickUp, playerPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}

	public void CheckPoint(Transform playerPos){

		if(audioOn){

			GameObject go = Instantiate(playerAudio.checkPoint, playerPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}

	public void TrollShout(Transform enemyPos){

		if(audioOn){

			//Vector3 pos = new Vector3 (enemyPos.position.x, enemyPos.position.y, -3f);
			//udioSource.PlayClipAtPoint (enemyAudio.trollShoutSound , pos);
			GameObject go = Instantiate(enemyAudio.trollShout, enemyPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}


	public void TrollDeath(Transform enemyPos){

		if(audioOn){

			//Vector3 pos = new Vector3 (enemyPos.position.x, enemyPos.position.y, -3f);

			//AudioSource.PlayClipAtPoint (enemyAudio.trollDeathSound, pos);
			GameObject go = Instantiate(enemyAudio.trollDeath, enemyPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}



	public void AciertosMultiplo(Transform enemyPos){

		if(audioOn){
	
			GameObject go = Instantiate(multAudio.acierto, enemyPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}

	public void GameOverMultiplo(Transform enemyPos){

		if(audioOn){

			GameObject go = Instantiate(multAudio.win, enemyPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}

	public void LoseMultiplo(Transform enemyPos){

		if(audioOn){

			GameObject go = Instantiate(multAudio.lose, enemyPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}




	public void ErrorMultiplo(Transform enemyPos){

		if(audioOn){

			GameObject go = Instantiate(multAudio.error, enemyPos.position, Quaternion.identity);
			StartCoroutine(destruirdespuesSeg(go));

		}

	}




	public void PararBSO(){

		bsos.SetActive (false);
	
	}

	IEnumerator destruirdespuesSeg(GameObject go){
		yield return new WaitForSeconds(0.8f);
		Destroy (go);
	}


}
