using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	// Use this for initialization
	void Start () {
		Instantiate (magician, magician.transform.position, magician.transform.rotation);
		Instantiate (trees, trees.transform.position, trees.transform.rotation);
		Instantiate (ground, ground.transform.position, ground.transform.rotation);
		UpdateScore ();
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
