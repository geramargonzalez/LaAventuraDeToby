using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fish spawner.
/// </summary>
public class FishSpawner : MonoBehaviour {

	public GameObject fish;
	public float spawnDelay;
		   bool canSpawn;

	// Use this for initialization
	void Start () {

		canSpawn = true;
		
	}
	
	// Update is called once per frame
	void Update () {

		if(canSpawn){
			StartCoroutine("SpawnFish");
		}
	}


	IEnumerator SpawnFish(){

		Instantiate (fish, transform.position , Quaternion.identity);
		canSpawn = false;
		yield return new WaitForSeconds (spawnDelay);
		canSpawn = true;
	
	}

}
