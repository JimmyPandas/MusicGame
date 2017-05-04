﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private float speed;
	private string note = "";
	private List<GameObject> items = new List<GameObject>();
	private bool spawned = false;
	private List<GameObject> spawnedFruits = new List<GameObject>();
	private float scoreableTime = 2f;

	// Use this for initialization
	void Start () {
		speed = GetNoteSpeed (note) ;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.left * speed * Time.deltaTime);
		if (transform.position.x <= -10 && !spawned) {
			foreach(GameObject item in items) {
				Vector3 position = new Vector3 (Random.Range (-10.0f, 10.0f), 1	, 0);
				GameObject spawnedFruit = Instantiate (item, Vector3.zero, Quaternion.identity);
				GameObject parent = Instantiate (new GameObject ("parent"), position, Quaternion.identity);
				spawnedFruit.transform.SetParent (parent.transform);
				spawnedFruit.GetComponentInChildren<FruitController> ().SetSpeed (speed);
				spawnedFruits.Add(spawnedFruit);
				spawned = true;
			}
		}
		if (spawned) {
			scoreableTime -= Time.deltaTime;
		}

		if (scoreableTime < 0) {
			foreach (GameObject spawnedFruit in spawnedFruits) {
				if (spawnedFruit != null) {
					FruitController fruitController = spawnedFruit.GetComponentInChildren<FruitController> ();
					fruitController.SetScoreable (false);
				}
			}
			if (gameObject != null) {
				Destroy (gameObject);
			}
		}
	}
		
	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public float GetScoreTime() {
		return scoreableTime;
	}

	public void AddItem(GameObject item) {
		items.Add (item);
	}

	public void SetNote(string note) {
		this.note = note;
	}

	public float GetNoteSpeed(string note) {
		switch (note) {
		case "Do":
			return 0.25f;
		case "Re":
			return 0.5f;
		case "Mi":
			return 1f;
		case "Fa":
			return 1.5f;
		case "So":
			return 2f;
		case "La":
			return 2.5f;
		case "Xi":
			return 3f;
		default:
			return 5f;
		}
	}
}