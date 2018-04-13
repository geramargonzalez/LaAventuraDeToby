using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCtrl : MonoBehaviour {


	public static SFXCtrl instance; // Permite a otras clases acceder al objeto
	//public GameObject sfx_coin_pickup;
	public SFX sfx;

	void Awake(){
		if(instance == null){
			instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	/// <summary>
	/// Shows the coin sparkle.
	/// </summary>

	public void showCoinSparkle(Vector3 pos){
		Instantiate(sfx.sfx_coin_pickup,pos,Quaternion.identity);
	}

	/// <summary>
	/// Shows the sparkle.
	/// </summary>

	public void showSparkle(Vector3 pos){
		//Instantiate(sfx.sfx_bullets_pickup,pos,Quaternion.identity);
	}

	/// <summary>
	/// Shows the player landing dust particle effects.
	/// </summary>

	public void showPlayerLanding(Vector3 pos){
		Instantiate(sfx.sfx_playerLands,pos,Quaternion.identity);
	}

	/// <summary>
	/// Handle the box breaking effects.
	/// </summary>

	public void HandleBoxBreaking(Vector3 pos){
		Vector3 pos1 = pos;
		pos1.x += 0.3f;
		Vector3 pos2 = pos;
		pos2.x -= 0.3f;
		Vector3 pos3 = pos;
		pos3.x -= 0.3f;
		pos3.y -= 0.3f;
		Vector3 pos4 = pos;
		pos4.x += 0.3f;
		pos4.y += 0.3f;


		Instantiate(sfx.sfx_boxfragment_1,pos1,Quaternion.identity);
		Instantiate(sfx.sfx_boxfragment_2,pos2,Quaternion.identity);
		Instantiate(sfx.sfx_boxfragment_2,pos3,Quaternion.identity);
		Instantiate(sfx.sfx_boxfragment_1,pos4,Quaternion.identity);
	}

	/// <summary>
	/// 	Shows the splash hen player falls in water.
	/// </summary>


	public void showSplash(Vector3 pos){
		//Instantiate(sfx.sfx_splash,pos,Quaternion.identity);
	}


	public void EnemyExplosion(Vector3 pos){
		Instantiate(sfx.sfx_explosion, pos, Quaternion.identity);
	}

}
