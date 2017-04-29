using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private float speed;
	private string note = "";
	private List<GameObject> items = new List<GameObject>();
	private bool spawned = false;
	private GameObject spawnedFruit;
	private float scoreableTime = 2f;

	// Use this for initialization
	void Start () {
		speed = GetNoteSpeed (note) ;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.left * speed * Time.deltaTime);
		if (transform.position.x <= -10 && !spawned) {
			if (items.Count > 0) {
				int index = Random.Range (0, items.Count);
				GameObject item = items [index];
				Vector3 position = new Vector3 (Random.Range (-12.0f, 12.0f), 5, 0);
				spawnedFruit = Instantiate (item, position, Quaternion.identity);
				spawnedFruit.GetComponentInChildren<FruitController> ().SetSpeed (speed);
				spawned = true;
			}
		}
		if (spawned) {
			scoreableTime -= Time.deltaTime;
		}

		if (scoreableTime < 0) {
			FruitController fruitController = spawnedFruit.GetComponentInChildren<FruitController> ();
			fruitController.SetScoreable (false);
			Destroy (gameObject);
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
			return 0.5f;
		case "Re":
			return 1f;
		case "Mi":
			return 1.5f;
		case "Fa":
			return 2f;
		case "So":
			return 2.5f;
		case "La":
			return 3f;
		case "Xi":
			return 3.5f;
		default:
			return 5f;
		}
	}
}
