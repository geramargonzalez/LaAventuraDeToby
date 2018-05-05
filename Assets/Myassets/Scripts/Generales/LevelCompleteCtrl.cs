using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelCompleteCtrl : MonoBehaviour {

	public Button nextLevel;
	public Sprite goldenStar;
	public Image star1;
	public Image star2;
	public Image star3;

	public Text txtNivelActualCompletado;
	public Text txtScore;
	public Text txtPromedio;
	public Text txtFallos;


		   int promedio;
		   int levelNumber;
		   int score;
		   int scoreForThreeStars;
		   int scoreForTwoStars;
		   int scoreForOneStars;
		   int scoreForNextLevel;


	public float animStarsLevel;
	public float animDelay;


	bool showThreeStars, showTwoStars;

	// Use this for initialization
	void Start () {

		scoreForThreeStars = 3;
		scoreForTwoStars = 4;
		scoreForOneStars = 8;
		animDelay = 1f;
		animStarsLevel = 0.7f;

		levelNumber = SistemaDejuego.instance.NivelLogrado();

		txtNivelActualCompletado.text = levelNumber.ToString();
	

		txtScore.text = "" + score;
	
		promedio = SistemaDejuego.instance.obtenerPromedio();

		txtPromedio.text = promedio + " segundos ";

		txtFallos.text = SistemaDejuego.instance.obtenerFallos().ToString ();


		if(promedio <= scoreForThreeStars){

			showThreeStars = true;

			SistemaDejuego.instance.PuntosPorStars (3);
			SistemaDejuego.instance.SetStarsAwarded (levelNumber-1, 3);

			Invoke ("ShowGoldenStars", animDelay);
			
		} else if(promedio <= scoreForTwoStars && promedio > scoreForThreeStars){

			showTwoStars = true;

			SistemaDejuego.instance.PuntosPorStars (2);

			SistemaDejuego.instance.SetStarsAwarded (levelNumber-1, 2);

			Invoke ("ShowGoldenStars", animDelay);
		
		} else if(promedio >= scoreForOneStars){

			SistemaDejuego.instance.PuntosPorStars (2);
			SistemaDejuego.instance.SetStarsAwarded (levelNumber-1, 1);
		
			Invoke ("ShowGoldenStars", animDelay);

		}

		score = SistemaDejuego.instance.GetScore ();
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DoAnim(Image pImage){

		pImage.rectTransform.sizeDelta = new Vector2 (200f,200f);

		pImage.sprite = goldenStar;


		RectTransform t = pImage.rectTransform;

		t.DOSizeDelta ( new Vector2 (150f,150f),0.5f,false);

		// Efecto de audio
		SFXCtrl.instance.showSparkle (pImage.gameObject.transform.position);

	}


	public void ShowGoldenStars(){

		StartCoroutine ("HandleStarFirstAnim", star1);
	}


	IEnumerator HandleStarFirstAnim(Image pImage){

		DoAnim (pImage);

		yield return new WaitForSeconds (animDelay);

		if (showThreeStars || showTwoStars) {
		
			StartCoroutine ("HandleStarSecondAnim", star2);
		
		} else {

			Invoke ("CheckLevelStatus",1.2f);
		}

	}



	IEnumerator HandleStarSecondAnim(Image pImage){

		DoAnim (pImage);

		yield return new WaitForSeconds (animDelay);

		showTwoStars = false;

		if (showThreeStars) {

			StartCoroutine ("HandleStarThirdAnim", star3);
		
		}  else {

			Invoke ("CheckLevelStatus",1.2f);
		}

	}

	IEnumerator HandleStarThirdAnim(Image pImage){
		
		DoAnim (pImage);

		showThreeStars = false;
	
		Invoke ("CheckLevelStatus",1.2f);
	
		yield return new WaitForSeconds (animDelay);

	}

	void CheckLevelStatus(){

			nextLevel.interactable = true;

			SFXCtrl.instance.showSparkle (nextLevel.gameObject.transform.position);

			SistemaDejuego.instance.PausarPantalla ();

	}




}
