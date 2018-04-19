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


	public int scoreForThreeStars;
	public int scoreForTwoStars;
	public int scoreForOneStars;
	public int scoreForNextLevel;


	public float animStarsLevel;
	public float animDelay;


	bool showThreeStars, showTwoStars;

	// Use this for initialization
	void Start () {

		txtNivelActualCompletado.text = SistemaDejuego.instance.NivelActual ().ToString();

		score = SistemaDejuego.instance.GetScore ();
		txtScore.text = "" + score;
	
		promedio = SistemaDejuego.instance.PromedioPorNivel ();

		txtPromedio.text = promedio.ToString ();

		txtFallos.text = SistemaDejuego.instance.GetFallos().ToString ();


		if(promedio <= scoreForThreeStars){

			showThreeStars = true;

			SistemaDejuego.instance.SetStarsAwarded (levelNumber, 3);

			Invoke ("ShowGoldenStars", animDelay);
			
		} else if(promedio <= scoreForTwoStars && promedio > scoreForThreeStars){

			showTwoStars = true;

			SistemaDejuego.instance.SetStarsAwarded (levelNumber, 2);

			Invoke ("ShowGoldenStars", animDelay);
		
		} else if(promedio >= scoreForOneStars){
			
			SistemaDejuego.instance.SetStarsAwarded (levelNumber, 1);

			Invoke ("ShowGoldenStars", animDelay);

		}
			
	}
	
	// Update is called once per frame
	void Update () {
		
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

	void DoAnim(Image pImage){

		pImage.rectTransform.sizeDelta = new Vector2 (100f,100f);

		pImage.sprite = goldenStar;

		RectTransform t = pImage.rectTransform;

		t.DOSizeDelta ( new Vector2 (50f,50f),0.5f,false);

		// Efecto de audio

		SFXCtrl.instance.showSparkle (pImage.gameObject.transform.position);

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

			SistemaDejuego.instance.ProximoNivel ();
	
			nextLevel.interactable = true;

			SFXCtrl.instance.showSparkle (nextLevel.gameObject.transform.position);

			//Audio Ctrl

			SistemaDejuego.instance.Unlocked (levelNumber);

	
	}

}
