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
	public int bones;

	public int cantidadTrolls;
	public float tiempoActual = 600f;
	public int posActualEnemigo = 0;

	public int fallos;

	//Estadisticas de los niveles ...
	//public List<>estadisticasTotales = new List<Estadisticas>();

	public Nivel[] niveles;

	//Valida si el usurio ya jugo
	public bool[] operaRealizadas;
	public bool yaJugo = false;

	//Posicion del personaje

	public float x = 3.3f;
	public float y = -26.5f;
	public float z = 0f;



	// Personaje, guardo los incrementos
	public float jumpSpeed = 900f;
	public float speedBoost = 20f;



	// Orquitos/Animales
	public bool[] orcosPorAnimales;
	public int cantAnimalesConvertidos;



	// Estadisticas
	public int promedio;


	// Crear una clase serializable sobre Estadisticas.



	// ***  METODOS *** //
	public void GuardarPosicionInicial(){
		x = 3.3f;
		y = -26.5f;
		z = 0f;
	}
		

	public float ResetTime(){

		if(nivel == 0){

			tiempoActual = 600f;

		
		} else if(nivel == 1){

			tiempoActual = 500f;


		} else if(nivel == 2){
			
			tiempoActual = 400f;


		}

		return tiempoActual;

	}


	public void subirNivel(){
		nivel++;
	}

	public void resetNivel(){
		nivel = 0;
	}


	public int calcularPromedio(){

		niveles[nivel].promedio = niveles[nivel].promedio / cantidadTrolls;
		 
		return niveles[nivel].promedio;
	}



	public void SetearNivelACtual(){
		subirNivel ();
		niveles [nivel].unlocked = true;
	
	}


	 
}
