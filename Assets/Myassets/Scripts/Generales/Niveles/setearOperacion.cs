using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setearOperacion : MonoBehaviour {

	public GameObject[] imgSimbolos;


	public void setearSigno(int num){
		imgSimbolos[num].SetActive(true);
	}

	public void desactivarObjetos (){
		for(int i = 0; i <= imgSimbolos.Length-1; i++){
			imgSimbolos[i].SetActive(false);
		}
	}
}
