using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LoginWindowGUIManager : MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject musicLibraryCanvas;
	public Dropdown dropdown;
	private bool musicChosen = false;
	private string searchPath = "";

	// Use this for initialization
	void Start () {
		searchPath =  "/Users/jimmy/Downloads/";
//		IEnumerable<string> files = Directory.GetFiles(path + "/Resources", "*.*", SearchOption.AllDirectories)
//			.Where(s => s.EndsWith(".mp3") || s.EndsWith(".wav") || s.EndsWith(".aif") || s.EndsWith(".ogg"));
		
		string[] musicfiles = Directory.GetFiles (searchPath, "*.wav", SearchOption.AllDirectories);
		List<string> musicOptions = new List<string>();
		foreach(string musicfile in musicfiles) {
			
			var fileInfo = new System.IO.FileInfo(musicfile);
			if (fileInfo.Length > 10000000) {
//				string musicOption = Path.GetFileName(musicfile);
				musicOptions.Add (musicfile);
			}
		}
		dropdown.AddOptions (musicOptions);
	}

	public void Play() {
		if (!musicChosen) {
			//get all options available within this dropdown menu
			List<Dropdown.OptionData> menuOptions = dropdown.options;
			int menuIndex = Random.Range (0, menuOptions.Count);
			SetMusic (menuIndex);
		}
		SceneManager.LoadScene ("Game");
	}

	public void Setting() {
	}
		

	public void Quit() {
		Application.Quit ();
	}

	public void DisplayMusicLibraryUI() {
		musicLibraryCanvas.SetActive (true);
		mainCanvas.SetActive (false);
	}

	public void MusicLibraryConfirm() {
		int menuIndex = dropdown.value;
		SetMusic (menuIndex);
		musicLibraryCanvas.SetActive (false);
		mainCanvas.SetActive (true);
	}

	private void SetMusic(int menuIndex) {
		//get all options available within this dropdown menu
		List<Dropdown.OptionData> menuOptions = dropdown.options;

		//get the string value of the selected index
		string musicOption = menuOptions [menuIndex].text;
		GameObject dataManager = GameObject.Find ("DataManager");
//		string path = "Music/" + musicOption;
		string path = musicOption;
		dataManager.GetComponentInChildren<DataManager> ().path = path;
		musicChosen = true;
	}

}
