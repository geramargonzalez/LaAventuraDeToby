using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour {


	public Transform pos1, pos2;

	public float speed;

	Vector3 nextPos;

	public Transform startPos;

	// Use this for initialization
	void Start () {

		nextPos = startPos.position;
	
	}
	
	// Update is called once per frame
	void Update () {

		if(transform.position == pos1.position){
			nextPos = pos2.position;
		}

		if(transform.position == pos2.position){
			nextPos = pos1.position;
		}

		transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
	}

	void OnDrawGizmos(){
		Gizmos.DrawLine(pos1.position, pos2.position);
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.transform.parent = this.transform;
		}

	}

	void OnTriggerExit2D(Collider2D other){

		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.transform.parent = null;
		}

	}




}


