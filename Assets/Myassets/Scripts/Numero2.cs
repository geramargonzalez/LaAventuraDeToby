using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numero2 : MonoBehaviour {

	public GameObject[] imgNumbers;


	public void setearNumero(int num){
		//Debug.Log ("muestro el elemento de Numero2 " + num);
			imgNumbers[num].SetActive(true);
		}

	public void desactivarObjetos (){
		for(int i = 0; i < 9; i++){
			imgNumbers[i].SetActive(false);
		}
	}
		
}



