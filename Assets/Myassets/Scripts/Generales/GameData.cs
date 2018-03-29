using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
///  	Los datos que se van a guardar del juego.
/// </summary>

[Serializable]
public class GameData 
{

	public int fallos;
	public int puntos;
	public int vidas;
	public int nivel;
	public int monedas;
	public int cantidadTrolls;
	public bool yaJugo = false;


}
