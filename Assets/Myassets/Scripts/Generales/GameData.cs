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
	public int numParaPromedio;

	public float tiempoActual;
	public int posActualEnemigo = 0;

	public int fallos;

	//Estadisticas de los niveles ...

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
	
		niveles[nivel].promedio = niveles[nivel].promedio / numParaPromedio;
	
		return niveles[nivel].promedio;
	}



	public void SetearNivelACtual(){

		yaJugo = false;

		subirNivel();

		fallos = 0;

		niveles [nivel].unlocked = true;
		niveles [nivel].promedio = 0;
		niveles [nivel].fallosPorNivel = 0;
		niveles [nivel].bonesStars = 0;
		niveles[nivel].fallosMultiplicacion = 0;
		niveles[nivel].fallosSuma = 0;
		niveles[nivel].fallosResta = 0;
		niveles[nivel].fallosDivision = 0;
		niveles[nivel].aciertosMultiplicacion = 0;
		niveles[nivel].aciertosSuma = 0;
		niveles[nivel].aciertosResta = 0;
		niveles[nivel].aciertosDivision = 0;


	}


	public void SetearNumeroDeNiveles(){
		for (int i = 0; i < niveles.Length; i++) {
			niveles [i].nivel = i;
		}
	
	}


	 
}
