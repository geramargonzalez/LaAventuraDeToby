using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  Los datos que se van a guardar del juego.
/// </summary>

[Serializable]
public class GameData {

	[Header("Informacion para guardar")]
	public int fallos;
	public int puntos;
	public int vidas;
	public int nivel;
	public int monedas;
	public int cantidadTrolls;
	public Transform positionActual;

}
