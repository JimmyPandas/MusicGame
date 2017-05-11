using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {

	public GameObject ground;
	public GameObject trees;
	public GameObject magician;
	public int score = 0;
	public GameObject gameCanvas;
	private int combo = 0;

	public Slider energybar;
	public Text scoreText;
	public Text feeebackText;
	public GameObject shield;
	private bool shieldSpawned = false;
	public bool gameOver = false;
	public GameObject gameOverText;
	public Animator gameOverAnimator;
	private float animationDelay = 15f;

	// Use this for initialization
	void Start () {
		gameOverAnimator = GameObject.Find ("ItemCanvas").GetComponentInChildren<Animator> ();
		UpdateScore ();
	}

	// Update is called once per frame
	void Update () {
		if (gameOver && animationDelay >= 15f) {
			GameObject dataManager = GameObject.Find ("DataManager");
			Destroy (dataManager);
			gameOverAnimator.SetTrigger ("GameOver");
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
