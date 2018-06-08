using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputValue : MonoBehaviour {

	public void RecibirValor(string value){
		int valor;
		int.TryParse(value, out valor);
		SisJuegoIntermedia.instance.RecibirMultiplo (valor);
	}
}
