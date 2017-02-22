using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourseManager : MonoBehaviour {

	public GameObject musicSymbol;
	public GameObject apple;
	public GameObject strawberry;
	public GameObject pear;
	public GameObject item;
	public Dictionary<string, List<GameObject>> itemsDict = new Dictionary<string, List<GameObject>>();
	private Dictionary<string, Color> colorsDict = new Dictionary<string, Color>();
	public int score = 0;
	public Canvas scoreCanvas;
	public Text scoreText;

	// Use this for initialization
	void Start () {
		colorsDict.Add ("Do", Color.red);
		colorsDict.Add ("Re", new Color(253, 181, 99, 1));
		colorsDict.Add ("Mi", Color.yellow);
		colorsDict.Add ("Fa", Color.green);
		colorsDict.Add ("So", Color.cyan);
		colorsDict.Add ("La", Color.green);
		colorsDict.Add ("Xi", Color.magenta);

		List<GameObject> doItems = new List<GameObject> ();
		doItems.Add (apple);
		doItems.Add(strawberry);
		List<GameObject> reItems = new List<GameObject> ();
		reItems.Add(item);
		List<GameObject> meItems = new List<GameObject> ();
		meItems.Add(item);
		List<GameObject> faItems = new List<GameObject> ();
		faItems.Add(pear);
		List<GameObject> soItems = new List<GameObject> ();
		soItems.Add(item);
		List<GameObject> laItems = new List<GameObject> ();
		laItems.Add(item);
		List<GameObject> xiItems = new List<GameObject> ();
		xiItems.Add(item);
			
		itemsDict.Add ("Do", doItems);
		itemsDict.Add ("Re", reItems);
		itemsDict.Add ("Mi", meItems);
		itemsDict.Add ("Fa", faItems);
		itemsDict.Add ("So", soItems);
		itemsDict.Add ("La", laItems);
		itemsDict.Add ("Xi", xiItems);
		scoreCanvas = Instantiate (scoreCanvas, scoreCanvas.transform.position, Quaternion.identity);
		if(scoreCanvas != null) {
			scoreText = scoreCanvas.GetComponentInChildren<Text> ();
			UpdateScore();
		}
	}

	// Update is called once per frame
	void Update () {
	}

	public void InstantiateMusicSymbol(string note) {
		if (colorsDict.ContainsKey (note)) {
			Debug.logger.Log (note);
			GameObject spawnedSymbol = Instantiate (musicSymbol, musicSymbol.transform.position
				, Quaternion.identity);
			spawnedSymbol.GetComponentInChildren<SpriteRenderer> ().color = colorsDict [note];
			float speed = GetNoteSpeed (note) * Time.deltaTime;
			GameObject[] liveSymbols = GameObject.FindGameObjectsWithTag ("Symbol");
			foreach (GameObject symbol in liveSymbols) {
				symbol.GetComponentInChildren<CloudController> ().SetSpeed (speed);
			}

			List<GameObject> items = itemsDict [note];
			if (items.Count > 0) {
				int index = Random.Range (0, items.Count - 1);
				GameObject item = items [index];
				Vector3 position = new Vector3 (Random.Range (-12.0f, 12.0f), 5, 0);
				GameObject spawnedItem = Instantiate (item, position, Quaternion.identity);
				spawnedItem.GetComponentInChildren<SpriteRenderer> ().color = colorsDict [note];
				spawnedItem.GetComponentInChildren<ItemController> ().SetSpeed (speed);
			}
		}

	}

	private float GetNoteSpeed(string note) {
		switch (note) {
		case "Do":
			return 1;
		case "Re":
			return 2;
		case "Mi":
			return 3;
		case "Fa":
			return 4;
		case "So":
			return 5;
		case "La":
			return 6;
		case "Xi":
			return 7;
		default:
			return 0;
		}
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
