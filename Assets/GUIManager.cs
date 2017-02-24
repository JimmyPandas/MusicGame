using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameObject ground;
	public GameObject trees;
	public GameObject grass;

	public int score = 0;
	public GameObject gameCanvas;

	public Text scoreText;

	// Use this for initialization
	void Start () {
		Instantiate (ground, ground.transform.position, Quaternion.identity);
		Instantiate (grass, grass.transform.position, Quaternion.identity);
		Instantiate (trees, trees.transform.position, Quaternion.identity);
	}


	public void AddScore (int scorePoint) {
		score += scorePoint;
		UpdateScore ();
	}

	void UpdateScore () {	
		if (scoreText != null) {
			scoreText.text = "Score: " + score;
		}
	}

	public void LoseScore (int scorePoint) {
		score -= scorePoint;
		UpdateScore ();
	}
		
}
