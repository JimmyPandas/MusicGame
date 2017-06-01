using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {

	public int score = 0;
	public GameObject gameCanvas;
	private int maxCombo;
	private int combo = 0;

	public Slider energybar;
	public Text scoreText;
	public Text feeebackText;
	public GameObject shield;
	private bool shieldSpawned = false;
	public bool gameOver = false;
	public GameObject gameOverText;
	private Animator gameOverAnimator;
	private float animationDelay = 15f;
	public Text gameOverScoreText;
	public Text gameOverMaxComboText;
	public GameObject tree;
	public GameObject grassGround;
	public GameObject dayBackground;
	public GameObject nightBackground;
	private DataManager dataManager;


	// Use this for initialization
	void Start () {
		gameOverAnimator = GameObject.Find ("ItemCanvas").GetComponentInChildren<Animator> ();
		UpdateScore ();
		dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		if (dataManager.isBright) {
//			nightBackground.SetActive (false);
			nightBackground.GetComponentInChildren<SpriteRenderer> ().sortingOrder = -1;
		} else {
//			dayBackground.SetActive (false);
			dayBackground.GetComponentInChildren<SpriteRenderer> ().sortingOrder = -1;
		}
	}

	// Update is called once per frame
	void Update () {
		if (gameOver && animationDelay == 15f) {
			if (dataManager.isBright) {
				gameOverAnimator.SetTrigger ("DayBackgroundGameOver");
			} else {
				gameOverAnimator.SetTrigger ("NightBackgroundGameOver");
			}
			gameOverScoreText.text = "" + score;
			maxCombo = Mathf.Max (combo, maxCombo);
			gameOverMaxComboText.text = "" + maxCombo;
		}

		if (gameOver) {
			animationDelay -= Time.deltaTime;
		}
		if (animationDelay < 0) {
			SceneManager.LoadScene ("LoginWindow");
		}
	}

	public void AddScore (int scorePoint) {
		combo++;
		energybar.value += combo;
		if (energybar.value >= energybar.maxValue) {
			if (!shieldSpawned) {
				Instantiate (shield, shield.transform.position, shield.transform.rotation);
				shieldSpawned = true;
			}
		}
		score += (int) (scorePoint * Mathf.Pow(combo, 2f));
		SetFeedback ();
		UpdateScore ();
	}

	void UpdateScore () {	
		scoreText.text = "" + score;
	}

	void SetFeedback() {
		Animator animator = feeebackText.GetComponentInChildren<Animator> ();
		if (combo > 0) {
			feeebackText.text = "COMBO X" + combo;
			animator.Play("ComboAnimation");
			if (combo == 6) {
				feeebackText.text = "Good!";
			} else if (combo == 12) {
				feeebackText.text = "Fantastic!";
			} else if (combo == 36) {
				feeebackText.text = "Godlike!";
			}
			animator.Play("ComboAnimation");
		} else {
			feeebackText.text = "Fighting!";
			animator.Play("ComboAnimation");
		}

	}

	public void LoseScore (int scorePoint) {
		if (!shieldSpawned) {
			score -= scorePoint;
			maxCombo = Mathf.Max (combo, maxCombo);
			combo = 0;
			energybar.value /= 2;
			SetFeedback ();
			UpdateScore ();
		} else {
			Destroy (GameObject.FindGameObjectWithTag ("Shield"));
			feeebackText.text = "Guard!";
			shieldSpawned = false;
			energybar.value /= 2;
		}
	}


		
}
