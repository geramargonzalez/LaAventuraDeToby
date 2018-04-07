using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 								 // Work with Serializable
using UnityEngine.UI;


/// <summary>
/// Todo lo que tiene que ver con la interfaz grafica
/// </summary>
[Serializable]
public class UI  {


	[Header("Textos de Informacion")]

	public Text txtPuntos;

	public Text txMonedas;

	public Text txtCantEnemigos;

	public Text txtVidas;

	public Text textTimer;

	public Text texttimeOp;

	public Text textFallos;

	public Text textCheckPoint;


	[Header("Textos de Habilidades")]

	public Text txtMsjgrlHabilidad;

	[Header("Imagenes de vida")]

	public GameObject[] vidasGo;


	//public Text txtHabilidad;

}
