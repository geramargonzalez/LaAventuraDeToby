using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstadisticasCtrl : MonoBehaviour {

	GameData data;
	GameObject[] promedio;
	Text[] promediostxt;
	GameObject[] aciertos;
	Text[] aciertostxt;
	GameObject[] fallos;
	Text[] fallostxt;
	GameObject[] totales;
	Text[] totalestxt;

	// Use this for initialization
	void Start () {

		Data (DataCtrl.instance.data);
		ObtenerGameObjectTextos ();
	
	}

	public void Data(GameData dat){

		data = dat;
	
	}

	public void ObtenerGameObjectTextos(){

		SeleccionarTextPromedio();
		SeleccionarTextFallos();
		SeleccionarTextAciertos();
		SeleccionarTextTotales ();
		SetearValoresDelJuego ();
		TotalValoresAciertos ();
		TotalValoresFallos();
		TotalPromedioPorNivel ();
		
	}

	public void SeleccionarTextPromedio(){

		promedio = new GameObject[6];
		promediostxt = new Text[6];

		for(int i = 0; i < promedio.Length; i++){

			int tmp = i ;

			promedio[i] = GameObject.Find ("promedio" + tmp);
			promediostxt[i] = promedio[i].GetComponent<Text> ();
		}

	}

	public void SeleccionarTextFallos(){

		fallos = new GameObject[6];
		fallostxt = new Text[6];

		for(int i = 0; i < fallos.Length; i++){

			int tmp = i ;

			fallos[i] = GameObject.Find ("fallos" + tmp);
			fallostxt[i] = fallos[i].GetComponent<Text> ();

		}

	}

	public void SeleccionarTextAciertos(){

		aciertos = new GameObject[6];
		aciertostxt = new Text[6];

		for(int i = 0; i < aciertos.Length; i++){

			int tmp = i ;

			aciertos[i] = GameObject.Find ("aciertos" + tmp);
			aciertostxt[i] = aciertos[i].GetComponent<Text> ();

		}

	}

	public void SeleccionarTextTotales(){

		totales = new GameObject[3];
		totalestxt = new Text[6];

		for(int i = 0; i < totales.Length; i++){

			int tmp = i;
			totales[i] = GameObject.Find ("total" + tmp);
			totalestxt[i] = totales[i].GetComponent<Text> ();

		}

	}



	public void SetearValoresDelJuego(){

		for(int i = 0; i < 6; i++) {

			if (i <= data.nivel) {

				promediostxt [i].text = data.niveles [i].promedio.ToString();
				fallostxt [i].text = data.niveles [i].fallosPorNivel.ToString();
				aciertostxt [i].text = data.niveles [i].aciertosPorNivel.ToString();

			} else {

				promediostxt [i].text = " - ";
				fallostxt [i].text = 	" - ";
				aciertostxt [i].text =	" - ";
			}
	
		}

	}

	public void TotalValoresAciertos(){

		int total = 0;

		for(int i = 0; i < 6; i++){

			if (i <= data.nivel) {
			
				total += data.niveles [i].aciertosPorNivel;
			
			}

		}

		totalestxt [1].text = total.ToString ();

	}

	public void TotalValoresFallos(){

		int total = 0;

		for(int i = 0; i < 6; i++){

			if (i <= data.nivel) {

				total += data.niveles [i].fallosPorNivel;

			}

		}

		totalestxt [2].text = total.ToString ();
	}


	public void TotalPromedioPorNivel(){

		int total = 0;
		int promeNivel = 0;

		for(int i = 0; i < 6; i++){

			if (i <= data.nivel) {

				total += data.niveles [i].promedio;

			}
		}

		if (total == 0) {

			totalestxt [0].text = " 0 ";

		} else {

			promeNivel = total / data.nivel+1;
			totalestxt [0].text = promeNivel.ToString ();
		}

	}




}
