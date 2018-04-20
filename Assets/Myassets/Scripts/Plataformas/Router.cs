using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Router : MonoBehaviour {

	public void ShowPanel(){
		SistemaDejuego.instance.PausaSHow ();
	}

	public void HidePanel(){
		SistemaDejuego.instance.PausaHide ();
	}

	public void ResetNivel(){
		DataCtrl.instance.ResetData ();
	}

}
