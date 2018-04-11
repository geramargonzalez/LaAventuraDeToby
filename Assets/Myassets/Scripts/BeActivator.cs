using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeActivator : MonoBehaviour {

	public GameObject bomberBe;
		   BomberBeAI beAI;
		   SpriteRenderer sp;

	public bool fX;

	// Use this for initialization
	void Start () {
		beAI = bomberBe.GetComponent<BomberBeAI> ();
		sp = bomberBe.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnTriggerEnter2D(Collider2D other)
	{

		if(other.gameObject.CompareTag("Player"))
		{
			beAI.ActivatedBee (other.gameObject.transform.position);

			if(fX)
			{
				sp.flipX = false;
				
			}

		}


	}


}
