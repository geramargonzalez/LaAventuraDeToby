using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
///  	Los datos que se van a guardar del juego.
/// </summary>

[Serializable]
public class Nivel{


	public int nivel;
	public bool unlocked;
	public int bonesStars;



	//Contador global y por cada cuenta aritmetica
	public int promedio;
	public int fallosPorNivel;

	public int fallosMultiplicacion;
	public int fallosSuma;
	public int fallosResta;
	public int fallosDivision;

	public int aciertosMultiplicacion;
	public int aciertosSuma;
	public int aciertosResta;
	public int aciertosDivision;




}
