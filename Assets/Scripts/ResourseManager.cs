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
	private Dictionary<string, List<GameObject>> spawnedFruitsDict = new Dictionary<string, List<GameObject>>();
	private List<string> notes = new List<string>();
	private Vector3 prevSpawnedPos = new Vector3 ();

	// Use this for initialization
	void Start () {
		InitItemDict ();
		InitSpawnedFruits ();
		GameObject dataManager = GameObject.Find ("DataManager");
		path = dataManager.GetComponentInChildren<DataManager> ().path;
		audioSource = Camera.main.GetComponentInChildren<AudioSource> ();
		if(audioSource != null) {
			StartCoroutine(LoadSongCoroutine()); 
			Camera.main.GetComponentInChildren<AudioProcessor> ().enabled = true;
		}
	
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

	private void InitSpawnedFruits() {
		spawnedFruitsDict.Add ("C", new List<GameObject>());
		spawnedFruitsDict.Add ("D", new List<GameObject>());
		spawnedFruitsDict.Add ("E", new List<GameObject>());
		spawnedFruitsDict.Add ("F", new List<GameObject>());
		spawnedFruitsDict.Add ("G", new List<GameObject>());
		spawnedFruitsDict.Add ("A", new List<GameObject>());
		spawnedFruitsDict.Add ("B", new List<GameObject>());
	}

	public string GetCurrentNote() {
		string currentNote = "";
		if (notes.Count > 0) {
			currentNote = notes [0];
		}
		return currentNote;
	}

	public void CollectFruit() {
		var button = EventSystem.current.currentSelectedGameObject;
		if (button != null) {
			Debug.Log ("Clicked on : " + button.name);
			string note = button.name.Replace ("Button", "");
			List<GameObject> spawnedFruits = spawnedFruitsDict [note];
			if(spawnedFruits.Count > 0) {
				GameObject fruit = spawnedFruits[0];
				if (fruit != null) {
					Debug.Log (fruit.name);
					FruitController fruitController = fruit.GetComponentInChildren<FruitController> ();
					fruitController.SetRemoved (true);
					spawnedFruitsDict [note].Remove (fruit);
					GUIManager guiManager = GameObject.Find ("GUIManager").GetComponentInChildren<GUIManager> ();
					if (fruitController.IfScoreable ()) {
						guiManager.AddScore (10);
					} else {
						guiManager.LoseScore (10);
					}
				}
			}
		} else {
			Debug.Log ("currentSelectedGameObject is null");
		}
	}

	public void RemoveNote(string note) {
		notes.Remove (note);	
	}

	public void InstantiateMusicSymbol(string note, string zone, float pitch) {
		int multiple = 0;
		float speed = 0f;
		if (int.TryParse (zone, out multiple)) {
			speed = Mathf.Log(Mathf.Sqrt(pitch) * multiple) / 4f;
		} 

		if (itemsDict.ContainsKey (note)) {
			notes.Add (note);
			List<GameObject> items = itemsDict [note];
			int size = items.Count;
			if (size > 0) {
				int index = Random.Range (0, size);
				GameObject item = items [index];
				Vector3 position = new Vector3 (Random.Range (-10.0f, 10.0f), 1, 0);
				while (Mathf.Abs (position.x - prevSpawnedPos.x) < 2 || Mathf.Abs (position.x - prevSpawnedPos.x) > 8) {
					position = new Vector3 (Random.Range (-10.0f, 10.0f), 1, 0);
				}
				GameObject spawnedFruit = Instantiate (item, Vector3.zero, Quaternion.identity);
				GameObject parent = Instantiate (new GameObject ("parent"), position, Quaternion.identity);
				spawnedFruit.transform.SetParent (parent.transform);
				spawnedFruitsDict [note].Add (spawnedFruit);
				FruitController fruitController = spawnedFruit.GetComponentInChildren<FruitController> ();
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
