using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourseManager : MonoBehaviour {

	/* List of fruits game object. */
	public GameObject apple;
	public GameObject strawberry;
	public GameObject pear;
	public GameObject banana;
	public GameObject orange;
	public GameObject peach;
	public GameObject cherry;
	public GameObject greenApple;
	public GameObject pineapple;
	public GameObject pitaya;
	public GameObject purpleGrape;
	public GameObject watermelon;
	public GameObject blueberry;
	public GameObject yellowPear;

	private Dictionary<string, List<GameObject>> itemsDict = new Dictionary<string, List<GameObject>>();
	private bool musicPlayed = false;
	private float musicPlayTime = 100f;
	private string path = "";
	private AudioSource audioSource;
	private List<string> notes = new List<string>();
	private Vector3 prevSpawnedPos = new Vector3 ();

	public GameObject rain;
	public GameObject sun;
	public GameObject moon;
	private Animator magicianGameAnimator;
	private DataManager dataManager;
	private List<string> directions = new List<string>{"left", "right"};

	public Text dataText;

	// Use this for initialization
	void Start () {
		InitItemDict ();
		dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		path = dataManager.path;
		audioSource = Camera.main.GetComponentInChildren<AudioSource> ();
		if(audioSource != null) {
			StartCoroutine(LoadSongCoroutine()); 
			Camera.main.GetComponentInChildren<AudioProcessor> ().enabled = true;
		}

		magicianGameAnimator = GameObject.Find ("magician_game").GetComponentInChildren<Animator> ();


	}

	/* This method is used to load a song from some file path and set music play time to the audio clip
	   length.
    */
	IEnumerator LoadSongCoroutine(){
		if (path.Length != 0) { 
			WWW www = new WWW("file://" + path);
			audioSource.clip = www.GetAudioClip();
			yield return www;
			musicPlayTime = www.GetAudioClip().length;
		}
	}

	// Update is called once per frame
	void Update () {
		musicPlayTime -= Time.deltaTime;
		if (audioSource.clip != null && !musicPlayed) {
			if (!audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded && !musicPlayed) {
				audioSource.Play ();
				musicPlayed = true;
			}
		}
		if (musicPlayTime < 0) {
			GameObject[] liveSymbols = GameObject.FindGameObjectsWithTag ("Symbol");
			if (liveSymbols.Length == 0) {
				GUIManager guiManager = GetComponent<GUIManager> ();
				guiManager.gameOver = true;
			}
		}
		AttributeData data = dataManager.currentAttributeData;
		if (data.danceable && magicianGameAnimator.GetCurrentAnimatorStateInfo (0).IsName ("MagicianGameIdel")) {
			magicianGameAnimator.SetTrigger("Dance");
		}

		if (!data.danceable && magicianGameAnimator.GetCurrentAnimatorStateInfo (0).IsName ("MagicianGameDancing")) {
			magicianGameAnimator.SetTrigger("NotDance");
		}

		float sadFactor = data.sadFactor;
		float happyFactor = data.happyFactor;

		if (sadFactor >= 0.75 && sadFactor > happyFactor) {
			if (dataManager.isBright) {
				sun.SetActive (false);
			} else {
				moon.SetActive (false);
			}
			rain.SetActive (true);
		}
		if (happyFactor >= 0.75 && happyFactor > sadFactor) {
			if (dataManager.isBright) {
				sun.SetActive (true);
			} else {
				moon.SetActive (true);
			}
			rain.SetActive (false);
		}
	}
		

	/* Initialize the note with corresponding fruits in the dictionary. */
	private void InitItemDict() {
		itemsDict.Add ("C", new List<GameObject>{apple, strawberry, cherry});
		itemsDict.Add ("D", new List<GameObject>{orange, pineapple});
		itemsDict.Add ("E", new List<GameObject>{yellowPear, banana});
		itemsDict.Add ("F", new List<GameObject>{pear, greenApple, watermelon});
		itemsDict.Add ("G", new List<GameObject>{blueberry});
		itemsDict.Add ("A", new List<GameObject>{peach});
		itemsDict.Add ("B", new List<GameObject>{purpleGrape});
	}
		

	/* Get the first note in the notes list if the list size is bigger than zero.
	   Otherwise, */
	public string GetCurrentNote() {
		string currentNote = "";
		if (notes.Count > 0) {
			currentNote = notes [0];
		}
		return currentNote;
	}

	/* Remove the given note from the notes list. */
	public void RemoveNote(string note) {
		notes.Remove (note);	
	}

	/* Ramdomly calculate fruit spawned position. */
	private Vector3 CalcFruitSpawnedPos() {
		string direction = directions[Random.Range (0, directions.Count)];
		switch (direction) {
		case "left":
			return new Vector3 (Random.Range (-2.0f, -12.0f), Random.Range (-1f, 3f), 0);
		case "right":
			return new Vector3 (Random.Range (3.0f, 12.0f), Random.Range (-1f, 3f), 0);
		default:
			return Vector3.zero;
		}
	}

	/* Apply the analysis results to calculate the fruit's speed and scoreable allow time.
	   It also changes other game settings using the analysis results. When a fruit is
	   spawned, other fruits' speed will become same. */
	public void InstantiateMusicSymbol(string note, string zone, float nextBeatInterval) {
		float speed = 0f;
		AttributeData data = dataManager.currentAttributeData;
		float bpmEstimate = 60f / (nextBeatInterval / 2f);
		speed = bpmEstimate / 100f;
		if (data.happyFactor > 0.5) {
			speed *= (1.05f + data.happyFactor);
			nextBeatInterval *= data.happyFactor;
		}
		if (data.sadFactor > 0.5) {
			speed /= (1.36f + data.sadFactor);
			nextBeatInterval *= (1f + data.sadFactor);
		}
			
		dataText.text = "CurrentTempo: " + Mathf.Round(bpmEstimate * 1000) / 1000f + "\n" + 
			"CurrentSpeed: " + Mathf.Round(speed * 1000) / 1000f + "\n" + data.ToString ();
		if (data.aggressiveFactor == 2) {
			nextBeatInterval *= 1.2f;
		}

		for (int i = 0; i < data.aggressiveFactor; i++) {
			if (itemsDict.ContainsKey (note)) {
				notes.Add (note);
				List<GameObject> items = itemsDict [note];
				int size = items.Count;
				if (size > 0) {
					int index = Random.Range (0, size);
					GameObject item = items [index];

					Vector3 position = CalcFruitSpawnedPos ();
					while (Mathf.Abs (position.x - prevSpawnedPos.x) < 3 || Mathf.Abs (position.x - prevSpawnedPos.x) > 9) {
						position = CalcFruitSpawnedPos ();
					}
					GameObject spawnedFruit = Instantiate (item, Vector3.zero, Quaternion.identity);
					GameObject parent = Instantiate (new GameObject ("parent"), position, Quaternion.identity);
					spawnedFruit.transform.SetParent (parent.transform);

					FruitController fruitController = spawnedFruit.GetComponentInChildren<FruitController> ();
					if (data.emotions.Count > 0) {
						string emotion = data.emotions [Random.Range (0, data.emotions.Count)];
						fruitController.SetEmotion (emotion);
						fruitController.ShowEmotion ();
					}

					fruitController.danceable = data.danceable;
					fruitController.scoreableTime = nextBeatInterval * 1.1f;
					fruitController.SetSpeed (speed);
					fruitController.SetNote (note);
					fruitController.SetZone (zone);
					prevSpawnedPos = position;
				}
				
		
				GameObject[] liveFruits = GameObject.FindGameObjectsWithTag ("Item");
				foreach (GameObject fruit in liveFruits) {
					FruitController fruitController = fruit.GetComponentInChildren<FruitController> ();
					fruitController.SetSpeed (speed);
			
				}

			}
		}

	}




		
}
