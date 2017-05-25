using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;

public class ResourseManager : MonoBehaviour {

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
	private Animator magicianGameAnimator;
	private bool danceable = false;

	// Use this for initialization
	void Start () {
		InitItemDict ();
		DataManager dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		path = dataManager.path;
		audioSource = Camera.main.GetComponentInChildren<AudioSource> ();
		if(audioSource != null) {
			StartCoroutine(LoadSongCoroutine()); 
			Camera.main.GetComponentInChildren<AudioProcessor> ().enabled = true;
		}

		if (dataManager.sadFactor >= 0.6) {
			Instantiate (rain, rain.transform.position, Quaternion.identity);
		}

		magicianGameAnimator = GameObject.Find ("magician_game").GetComponentInChildren<Animator> ();
		danceable = dataManager.danceable;
	
	}

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
			
		if (danceable && magicianGameAnimator.GetCurrentAnimatorStateInfo (0).IsName ("MagicianGameIdel")) {
			magicianGameAnimator.SetTrigger("Dance");
		}
	}
		

	private void InitItemDict() {
		itemsDict.Add ("C", new List<GameObject>{apple, strawberry, cherry});
		itemsDict.Add ("D", new List<GameObject>{orange, pineapple});
		itemsDict.Add ("E", new List<GameObject>{yellowPear, banana});
		itemsDict.Add ("F", new List<GameObject>{pear, greenApple, watermelon});
		itemsDict.Add ("G", new List<GameObject>{blueberry});
		itemsDict.Add ("A", new List<GameObject>{peach});
		itemsDict.Add ("B", new List<GameObject>{purpleGrape});
	}
		

	public string GetCurrentNote() {
		string currentNote = "";
		if (notes.Count > 0) {
			currentNote = notes [0];
		}
		return currentNote;
	}

	public void RemoveNote(string note) {
		notes.Remove (note);	
	}


	public void InstantiateMusicSymbol(string note, string zone, float pitch, float nextBeatInterval) {
		DataManager dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		int multiple = 0;
		float speed = 0f;
		if (int.TryParse (zone, out multiple)) {
			
			speed = Mathf.Log (Mathf.Sqrt (pitch) * multiple) / 4f;
			if (dataManager.happyFactor > 0.5) {
				speed *= (2f + dataManager.happyFactor);
			}
			if (dataManager.sadFactor > 0.5) {
				speed /= (2f + dataManager.sadFactor);
			}
		} 
			
		for (int i = 0; i < dataManager.aggresiveFactor; i++) {
			if (itemsDict.ContainsKey (note)) {
				notes.Add (note);
				List<GameObject> items = itemsDict [note];
				int size = items.Count;
				if (size > 0) {
					int index = Random.Range (0, size);
					GameObject item = items [index];
					Vector3 position = new Vector3 (Random.Range (-10.0f, 10.0f), Random.Range (-1f, 3f), 0);
					while (Mathf.Abs (position.x - prevSpawnedPos.x) < 3 || Mathf.Abs (position.x - prevSpawnedPos.x) > 9) {
						position = new Vector3 (Random.Range (-10.0f, 10.0f), Random.Range (-1f, 3f), 0);
					}
					GameObject spawnedFruit = Instantiate (item, Vector3.zero, Quaternion.identity);
					GameObject parent = Instantiate (new GameObject ("parent"), position, Quaternion.identity);
					spawnedFruit.transform.SetParent (parent.transform);

					string emotion = dataManager.emotions [Random.Range (0, dataManager.emotions.Count)];
					FruitController fruitController = spawnedFruit.GetComponentInChildren<FruitController> ();
					fruitController.SetEmotion (emotion);
					fruitController.scoreableTime += nextBeatInterval;
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
