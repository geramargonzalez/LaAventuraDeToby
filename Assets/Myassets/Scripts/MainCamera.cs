using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public GameObject follow;
	public Vector2 posMin, posMax;
	public float smoothTime;

	private Vector2 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		float posX =  Mathf.SmoothDamp(transform.position.x,follow.transform.position.x,ref velocity.x,smoothTime);
		float posY = Mathf.SmoothDamp(transform.position.y,follow.transform.position.y,ref velocity.y,smoothTime);

		transform.position = new Vector3 (Mathf.Clamp(posX,posMin.x,posMax.x),
			Mathf.Clamp(posY,posMin.y,posMax.y),
			transform.position.z);
	}
}
