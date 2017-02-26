using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginWindowGUIManager : MonoBehaviour {

	public GameObject mainCanvas;

	public GameObject mainMenu;

	// Use this for initialization
	void Start () {
		if (mainCanvas != null) {
			mainCanvas.SetActive (true);
		}
	}

	public void Play() {
		SceneManager.LoadScene ("Game");


	}

	public void Setting() {
		GameObject.Find ("DataManager").GetComponentInChildren<FileLoader> ().enabled = true;
		GameObject dataManager = GameObject.Find ("DataManager");
		FileLoader fileLoader = dataManager.GetComponentInChildren<FileLoader>();
		fileLoader.enabled = false;
	}

	public void Quit() {
		Application.Quit ();
	}

}
