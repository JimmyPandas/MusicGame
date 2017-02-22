using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

	public GameObject ground;
	public GameObject trees;
	public GameObject grass;
	public int score = 0;
	public Canvas scoreCanvas;
	private Text scoreText;

	// Use this for initialization
	void Start () {
		Instantiate (ground, ground.transform.position, Quaternion.identity);
		Instantiate (grass, grass.transform.position, Quaternion.identity);
		Instantiate (trees, trees.transform.position, Quaternion.identity);
		scoreCanvas = Instantiate (scoreCanvas, scoreCanvas.transform.position, Quaternion.identity);
		if(scoreCanvas != null) {
			scoreText = scoreCanvas.GetComponentInChildren<Text> ();
			UpdateScore();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
