using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourseManager : MonoBehaviour {

	public GameObject musicSymbol;
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
	public GameObject item;

	private Dictionary<string, List<GameObject>> itemsDict = new Dictionary<string, List<GameObject>>();
	private Dictionary<string, Color> colorsDict = new Dictionary<string, Color>();
	private bool musicPlayed = false;
	private float musicPlayTime;
	private string path = "";

	// Use this for initialization
	void Start () {
		InitColorDict ();
		InitItemDict ();
		GameObject dataManager = GameObject.Find ("DataManager");
		path = dataManager.GetComponentInChildren<DataManager> ().path;
		AudioSource audioSource = GetComponent<AudioSource> ();
		if(audioSource != null) {
			audioSource.clip = (AudioClip) Resources.Load (path, typeof(AudioClip));
			GetComponentInChildren<AudioProcessor> ().enabled = true;
		}
	
	}

	// Update is called once per frame
	void Update () {
		AudioSource audioSource = GetComponent<AudioSource> ();
		musicPlayTime -= Time.deltaTime;
		if (audioSource.clip != null && !musicPlayed) {
			if (!audioSource.isPlaying && audioSource.clip.loadState == AudioDataLoadState.Loaded && !musicPlayed) {
				audioSource.Play ();
				musicPlayed = true;
				musicPlayTime = audioSource.clip.length;

			}
		}
		if (musicPlayTime < 0) {
			GameObject[] liveSymbols = GameObject.FindGameObjectsWithTag ("Symbol");
			if (liveSymbols.Length == 0) {
				GameObject dataManager = GameObject.Find ("DataManager");
				Destroy (dataManager);
				SceneManager.LoadScene ("LoginWindow");
			}
		}
	}
		

	private void InitColorDict() {
		colorsDict.Add ("Do", new Color(247f / 255f, 122f / 255f, 104f / 255f, 1));
		colorsDict.Add ("Re", new Color(252f / 255f, 189f / 255f, 13f / 255f, 1));
		colorsDict.Add ("Mi", new Color(255f / 255f, 241f / 255f, 77f / 255f, 1));
		colorsDict.Add ("Fa", new Color(177f / 255f, 199f / 255f, 39f / 255f, 1));
		colorsDict.Add ("So", new Color(155f / 255f, 171f / 255f, 222f / 255f, 1));
		colorsDict.Add ("La", new Color(237f / 255f, 185f / 255f, 175f / 255f, 1));
		colorsDict.Add ("Xi", new Color(106f / 255f, 64f / 255f, 83f / 255f, 1));
	}

	private void InitItemDict() {
		List<GameObject> doItems = new List<GameObject> ();
		doItems.Add (apple);
		doItems.Add(strawberry);
		doItems.Add(cherry);
		List<GameObject> reItems = new List<GameObject> ();
		reItems.Add(orange);
		reItems.Add(pineapple);
		List<GameObject> meItems = new List<GameObject> ();
		meItems.Add(yellowPear);
		meItems.Add(banana);
		List<GameObject> faItems = new List<GameObject> ();
		faItems.Add(pear);
		faItems.Add(greenApple);
		faItems.Add(watermelon);
		List<GameObject> soItems = new List<GameObject> ();
		soItems.Add(blueberry);	
		List<GameObject> laItems = new List<GameObject> ();
		laItems.Add(peach);
		List<GameObject> xiItems = new List<GameObject> ();
		xiItems.Add(purpleGrape);

		itemsDict.Add ("Do", doItems);
		itemsDict.Add ("Re", reItems);
		itemsDict.Add ("Mi", meItems);
		itemsDict.Add ("Fa", faItems);
		itemsDict.Add ("So", soItems);
		itemsDict.Add ("La", laItems);
		itemsDict.Add ("Xi", xiItems);
	}
		

	public void InstantiateMusicSymbol(string note) {
		if (colorsDict.ContainsKey (note)) {
			GameObject spawnedSymbol = Instantiate (musicSymbol, musicSymbol.transform.position
				, Quaternion.identity);
			spawnedSymbol.GetComponentInChildren<SpriteRenderer> ().color = colorsDict [note];
			List<GameObject> items = itemsDict [note];
			CloudController cloudController = spawnedSymbol.GetComponentInChildren<CloudController> ();
			cloudController.SetNote (note);
			foreach(GameObject item in items) {
				cloudController.AddItem(item);
			}

			GameObject[] liveSymbols = GameObject.FindGameObjectsWithTag ("Symbol");
			foreach (GameObject liveSymbol in liveSymbols) {
				CloudController controller = liveSymbol.GetComponentInChildren<CloudController>();
				controller.SetSpeed (controller.GetNoteSpeed (note));
			}

		}

	}




		
}
