using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginWindowGUIManager : MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject musicLibraryCanvas;
	public Dropdown dropdown;

	// Use this for initialization
	void Start () {
	}

	public void Play() {
		
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

		//get all options available within this dropdown menu
		List<Dropdown.OptionData> menuOptions = dropdown.options;

		//get the string value of the selected index
		string value = menuOptions [menuIndex].text;
		GameObject dataManager = GameObject.Find ("DataManager");
		string path = "Music/" + value;
		dataManager.GetComponentInChildren<DataManager> ().path = path;
		musicLibraryCanvas.SetActive (false);
		mainCanvas.SetActive (true);
	}

}
