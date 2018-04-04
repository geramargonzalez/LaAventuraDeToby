using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour {

	public Transform leftBound, rightBound;

	public float speed;
	public float minDelay, maxDelay;
	float originalSpeed;

	bool canTurn;

	Rigidbody2D rb;
	SpriteRenderer sp;
	Animator anim;

	// Use this for initialization
	void Start () {
		
		canTurn = true;
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		sp = GetComponent<SpriteRenderer> ();
		SetStartingDirection ();
	}

	// Update is called once per frame
	void Update () {


		SetStartingDirection ();
		FlipEdges ();
		Move ();

	}

	void FlipEdges (){

		if(!sp.flipX && transform.position.x >= rightBound.transform.position.x){

			if(canTurn){

				canTurn = false;
				originalSpeed = speed;
				speed = 0;
				StartCoroutine ("TurnLeft", originalSpeed);

			}



		} else if(sp.flipX && transform.position.x <= leftBound.transform.position.x){

			if(canTurn){

				canTurn = false;
				originalSpeed = speed;
				speed = 0;
				StartCoroutine ("TurnRight", originalSpeed);

			}


		}

	}

	IEnumerator TurnLeft(float oSpeed){

		anim.SetBool ("Idle", true);
		yield return new WaitForSeconds (Random.Range (minDelay, maxDelay));
		anim.SetBool ("Idle", false);
		sp.flipX = true;
		speed = -oSpeed;
		canTurn = true;


	}

	IEnumerator TurnRight(float oSpeed){

		anim.SetBool ("Idle", true);
		yield return new WaitForSeconds (Random.Range (minDelay, maxDelay));
		anim.SetBool ("Idle", false);
		sp.flipX = false;
		speed = -oSpeed;
		canTurn = true;

	}


	void SetStartingDirection(){

		if (speed > 0) {

			sp.flipX = false;

		} else if (speed < 0) {

			sp.flipX = true;

		}

	}

	void Move(){

		Vector2 temp = rb.velocity;
		temp.x = speed;
		rb.velocity = temp; 

	}

	void OnDrawGizmos(){

		Gizmos.DrawLine (leftBound.position, rightBound.position);

	}
}
