using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameObject ground;
	public GameObject trees;
	public GameObject girl;
	public int score = 0;
	public GameObject gameCanvas;
	private int combo = 0;

	public Text scoreText;
	public Text feeebackText;

	// Use this for initialization
	void Start () {
		Instantiate (girl, girl.transform.position, girl.transform.rotation);
		Instantiate (trees, trees.transform.position, trees.transform.rotation);
		Instantiate (ground, ground.transform.position, ground.transform.rotation);
		UpdateScore ();
	}


	public void AddScore (int scorePoint) {
		combo++;
		score += (int) (scorePoint * Mathf.Pow(combo, 2f));
		SetFeedback ();
		UpdateScore ();
	}

	void UpdateScore () {	
		scoreText.text = "Score: " + score;
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
		score -= scorePoint;
		combo = 0;
		SetFeedback ();
		UpdateScore ();
	}
		
}
