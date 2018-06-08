using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class LevelCompIntermedios : MonoBehaviour {

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

		levelNumber = SisJuegoIntermedia.instance.NivelLogrado();

		txtNivelActualCompletado.text = levelNumber.ToString();

		promedio = SisJuegoIntermedia.instance.obtenerPromedio();

		txtPromedio.text = promedio + " segundos ";

		txtFallos.text = SisJuegoIntermedia.instance.obtenerFallos().ToString ();


		if(promedio <= scoreForThreeStars){

			showThreeStars = true;

			SisJuegoIntermedia.instance.PuntosPorStars (3);
			SisJuegoIntermedia.instance.SetStarsAwarded (levelNumber-1, 3);

			Invoke ("ShowGoldenStars", animDelay);

		} else if(promedio <= scoreForTwoStars && promedio > scoreForThreeStars){

			showTwoStars = true;

			SisJuegoIntermedia.instance.PuntosPorStars (2);

			SisJuegoIntermedia.instance.SetStarsAwarded (levelNumber-1, 2);

			Invoke ("ShowGoldenStars", animDelay);

		} else if(promedio >= scoreForOneStars){

			SisJuegoIntermedia.instance.PuntosPorStars (2);
			SisJuegoIntermedia.instance.SetStarsAwarded (levelNumber-1, 1);

			Invoke ("ShowGoldenStars", animDelay);

		}

		score = SisJuegoIntermedia.instance.GetScore ();

		txtScore.text = "" + score;

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
		//SFXCtrl.instance.showSparkle (pImage.gameObject.transform.position);

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

//		SFXCtrl.instance.showSparkle (nextLevel.gameObject.transform.position);

		//SistemaDejuego.instance.PausarPantalla ();

	}

}
