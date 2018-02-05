using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hacer la camara siga al player. 
/// </summary>
public class CameraCtrl : MonoBehaviour {

	public Transform player;
	public float yOff;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (player.position.x, player.position.y + yOff,transform.position.z);
	}
}
