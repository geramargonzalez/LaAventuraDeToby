using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numero : MonoBehaviour {

	public GameObject[] imgNumbers;
		   		   
	public void setearNumero(int num){
		imgNumbers[num].SetActive(true);
	}

	public void desactivarObjetos (){
		for(int i = 0; i <= imgNumbers.Length-1; i++){
			imgNumbers[i].SetActive(false);
		}
	}

}
