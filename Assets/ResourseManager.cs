using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourseManager : MonoBehaviour {

	public GameObject musicSymbol;
	public GameObject apple;
	public GameObject strawberry;
	public GameObject pear;
	public GameObject item;
	private Dictionary<string, List<GameObject>> itemsDict = new Dictionary<string, List<GameObject>>();
	private Dictionary<string, Color> colorsDict = new Dictionary<string, Color>();
	private string path = "";

	// Use this for initialization
	void Start () {
		GameObject dataManager = GameObject.Find ("DataManager");
		path = dataManager.GetComponentInChildren<DataManager> ().path;
		StartCoroutine(LoadSongCoroutine()); 
		FileLoader fileLoader = dataManager.GetComponentInChildren<FileLoader>();
		fileLoader.enabled = false;

		AudioSource audioSource = Camera.main.GetComponentInChildren<AudioSource> ();
		if(audioSource != null) {
			GetComponentInChildren<AudioProcessor> ().enabled = true;
		}

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
	
	}

	// Update is called once per frame
	void Update () {
		AudioSource audioSource = GetComponent<AudioSource> ();
		if (!audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded) {
			audioSource.Play ();
		}
	}

	IEnumerator LoadSongCoroutine(){
		if (path.Length != 0) { 
			WWW www = new WWW("file://" + path);

			GetComponentInChildren<AudioSource>().clip = www.audioClip;
			yield return www;
		}
	}

	public void InstantiateMusicSymbol(string note) {
		if (colorsDict.ContainsKey (note)) {
			GameObject spawnedSymbol = Instantiate (musicSymbol, musicSymbol.transform.position
				, Quaternion.identity);
			spawnedSymbol.GetComponentInChildren<SpriteRenderer> ().color = colorsDict [note];
			List<GameObject> items = itemsDict [note];
			CloudController cloudController = spawnedSymbol.GetComponentInChildren<CloudController> ();
			foreach(GameObject item in items) {
				cloudController.AddItem(item);
				cloudController.SetNote (note);
			}

			GameObject[] liveSymbols = GameObject.FindGameObjectsWithTag ("Symbol");
			foreach (GameObject liveSymbol in liveSymbols) {
				CloudController controller = liveSymbol.GetComponentInChildren<CloudController>();
				controller.SetSpeed (controller.GetNoteSpeed (note));
			}

		}

	}




		
}
