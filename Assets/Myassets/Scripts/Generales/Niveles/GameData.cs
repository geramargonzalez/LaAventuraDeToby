using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
///  Los datos que se van a guardar del juego.
/// </summary>
[Serializable]
public class GameData 
{

	//Contador de puntos, vidas, nivel actual, cantidad de enemigos que quedan
	public int puntos;
	public int vidas;
	public int nivel;
	public int bones;

	// Cantidad de Trolls
	public int cantidadTrolls;
	public int numParaPromedio;

	// Tiempo 
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
	public float y = -53.0f;
	public float z = 0f;

	// Personaje, guardo los incrementos
	public float jumpSpeed = 900f;
	public float speedBoost = 20f;

	//Orquitos/Animales
	public bool[] orcosPorAnimales;
	public bool[] bonesBool;
	public int cantAnimalesConvertidos;


	// ***  METODOS *** //
	public void GuardarPosicionInicial(){
		x = 3.3f;
		y = -26.0f;
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
		niveles [nivel].aciertosPorNivel = 0;
		niveles [nivel].fallosPorNivel = 0;


	}


	public void SetearNumeroDeNiveles(){
		for (int i = 0; i < niveles.Length; i++) {
			niveles [i].nivel = i;
		}
	
	}


	public void UnLockedNivel(){

		for(int i = 0; i < niveles.Length; i++){

			if (niveles [i].nivel == nivel) {

				niveles [i].unlocked = true;

				niveles [i].aciertosPorNivel = 0;

				niveles [i].fallosPorNivel = 0;

				niveles [i].bonesStars = 0;

				niveles [i].fallosMultiplicacion = 0;

				niveles [i].fallosSuma = 0;

				niveles [i].fallosResta = 0;

				niveles [i].fallosDivision = 0;

				niveles [i].aciertosMultiplicacion = 0;

				niveles [i].aciertosSuma = 0;

				niveles [i].aciertosResta = 0;

				niveles [i].aciertosDivision = 0;

				niveles [i].promedio = 0;


			} else if (niveles [i].nivel > nivel) {

				niveles [i].unlocked = false;

				niveles [i].bonesStars = 0;

				niveles [i].aciertosPorNivel = 0;

				niveles [i].fallosPorNivel = 0;

				niveles [i].fallosMultiplicacion = 0;

				niveles [i].fallosSuma = 0;

				niveles [i].fallosResta = 0;

				niveles [i].fallosDivision = 0;

				niveles [i].aciertosMultiplicacion = 0;

				niveles [i].aciertosSuma = 0;

				niveles [i].aciertosResta = 0;

				niveles [i].aciertosDivision = 0;

				niveles [i].promedio = 0;

			}


		}
	}


	public void ResetNivelActual(int ultimoNivel){

		nivel = ultimoNivel;

		vidas = 5;

		bones = 0;

		puntos = 0;

		fallos = 0;

		UnLockedNivel ();

		tiempoActual = ResetTime ();

		yaJugo = false;

		jumpSpeed = 900f;

		speedBoost = 20f;

		posActualEnemigo = 0;
	}
	 
	public void ResetData(){

		nivel = 0;

		vidas = 5;

		bones = 0;

		puntos = 0;

		fallos = 0;

		UnLockedNivel ();

		tiempoActual = ResetTime ();

		yaJugo = false;

		jumpSpeed = 900f;

		speedBoost = 20f;

		posActualEnemigo = 0;

	}


	public int cantidadEnemigoPorNivel(){
		
		int cantEnem = 0;

		if (nivel == 0) {

			cantEnem = 8;
		
		} 
		if (nivel == 1) {

			cantEnem = 6;
		
		} 
		if (nivel == 2) {
		
			cantEnem = 9;
		
		} 
			
		if (nivel == 4) {
		
			cantEnem = 14;
		
		} 
		if (nivel == 6) {

			cantEnem = 15;
		
		} 

		cantidadTrolls = cantEnem;

		return cantEnem;
	
	}

	public int cantidadOrquitosPorNivel(){

		int cantEnem = 0;

		if (nivel == 0) {

			cantEnem = 10;

		} 
		if (nivel == 1) {

			cantEnem = 21;

		} 
		if (nivel == 2) {

			cantEnem = 21;

		} 
			
		if (nivel == 4) {

			cantEnem = 10;

		} 

		if (nivel == 6) {

			cantEnem = 21;

		} 

		return cantEnem;
	}

	
}
