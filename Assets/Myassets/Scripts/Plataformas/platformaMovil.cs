using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformaMovil : MonoBehaviour {

	public Transform target;
	public float speed;
	public GameObject player;
	private Transform pos;
	private Vector3 start, end;


	void Start () {
		if(target != null){
			target.parent = null;
			start = transform.position;
			end = target.position;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if(target != null){
			float fixedSpeed = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position,target.position,fixedSpeed);
		}
		if(transform.position == target.position){
			target.position = (target.position == start) ? end : start;
		}
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.transform.parent = this.transform;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		other.gameObject.transform.parent = null;
	}
}
