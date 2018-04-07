using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
///  	Los datos que se van a guardar del juego.
/// </summary>

[Serializable]
public class GameData 
{

	//Contador de puntos, vidas, nivel actual, cantidad de enemigos que quedan
	public int puntos;
	public int vidas;
	public int nivel;
	public int monedas;
	public int cantidadTrolls;
	public float tiempoActual = 300f;


	//Contador global y por cada cuenta aritmetica
	public int fallos;
	public int fallosMultiplicacion;
	public int fallosSuma;
	public int fallosResta;
	public int fallosDivision;


	//Valida si el usurio ya jugo
	public bool[] operaRealizadas;
	public bool yaJugo = false;

	//Posicion del personaje

	public float x = 3.3f;
	public float y = -26.5f;
	public float z = 0f;


	public void GuardarPosicionInicial(){
		x = 3.3f;
		y = -26.5f;
		z = 0f;
	}

	public float ResetTime(){
		if(nivel == 0){

			tiempoActual = 300f;

		
		} else if(nivel == 1){

			tiempoActual = 500f;


		} else if(nivel == 2){
			
			tiempoActual = 600f;


		}

		return tiempoActual;

	}


	public void subirNivel(){
		nivel++;
	}

	public void resetNivel(){
		nivel = 0;
	}
}
