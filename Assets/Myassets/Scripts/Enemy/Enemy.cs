using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float visionRadius;
	public float speed;
	public GameObject player;
	public Vector3 initialPosition;




	// Use this for initialization
	void Start () {
		//player = GameObject.FindGameObjectsWithTag("Player");
		initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 target = initialPosition;
		float dist = Vector3.Distance(player.transform.position,transform.position);

		if (dist < visionRadius) target = player.transform.position;

		float fixedSpeed = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position,target,fixedSpeed);

		Debug.DrawLine (transform.position, target , Color.green);

	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, visionRadius);
	}
}
