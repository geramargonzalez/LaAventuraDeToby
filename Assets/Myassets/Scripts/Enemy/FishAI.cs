using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour {

	public float jumpSpeed;
	Rigidbody2D rb;
	SpriteRenderer sp;

	// Use this for initialization
	void Start () {
		sp = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();

		FishJump ();
	}
	
	// Update is called once per frame
	void Update () {
		//if (rb.velocity.y > 0) {
		//	sp.flipY = false;
		//} else {
		//	sp.flipY = true;
		
		//}
	}


	public void FishJump (){
		rb.AddForce (new Vector2 (0, jumpSpeed));

	
	
	}
}
