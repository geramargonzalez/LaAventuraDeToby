using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
///  	Los datos que se van a guardar del juego.
/// </summary>

[Serializable]
public class GameData 
{


	public int puntos;
	public int vidas;
	public int nivel;
	public int monedas;
	public int cantidadTrolls;


	//Contador global y por cada cuenta
	public int fallos;
	public int fallosMultiplicacion;
	public int fallosSuma;
	public int fallosResta;
	public int fallosDivision;



}
