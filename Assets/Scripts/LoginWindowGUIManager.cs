﻿using System.Collections;
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
	private Dictionary<string, string> musicOptionsDic = new Dictionary<string, string>();

	// Use this for initialization
	void Start () {
		searchPath =  Application.dataPath;
		string parentDir = Directory.GetParent (searchPath).FullName;
		while (parentDir.Length >= 16) {
			searchPath = parentDir;
			parentDir = Directory.GetParent (searchPath).FullName;
		}

		GameObject dataManager = GameObject.Find ("DataManager");
		dataManager.GetComponentInChildren<DataManager> ().searchPath = searchPath;
	}

	public void Play() {
		RefreshMusicList ();
		if (!musicChosen) {
			//get all options available within this dropdown menu
			List<Dropdown.OptionData> menuOptions = dropdown.options;
			if (menuOptions.Count > 0) {
				int menuIndex = Random.Range (0, menuOptions.Count);
				SetMusic (menuIndex);
			}
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
		RefreshMusicList ();
	}

	public void RefreshMusicList() {
		dropdown.ClearOptions ();
		musicOptionsDic.Clear ();
		string[] musicfiles = Directory.GetFiles (searchPath, "*.wav", SearchOption.AllDirectories);
		List<string> musicOptions = new List<string>();
		foreach(string musicfile in musicfiles) {
			var fileInfo = new System.IO.FileInfo(musicfile);
			if (fileInfo.Length > 10000000) {
				string musicOption = Path.GetFileName(musicfile);
				if (!musicOptionsDic.ContainsKey (musicOption)) {
					musicOptions.Add (musicOption);
					musicOptionsDic.Add (musicOption, musicfile);
				}
			}
		}
		dropdown.AddOptions (musicOptions);
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
		if (musicOptionsDic.ContainsKey (musicOption)) {
			string path = musicOptionsDic [musicOption];
			dataManager.GetComponentInChildren<DataManager> ().path = path;
			musicChosen = true;
		}
	}

}
