using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginWindowGUIManager : MonoBehaviour {

	public GameObject mainCanvas;

	public GameObject mainMenu;
	public GameObject character;

	// Use this for initialization
	void Start () {
		mainCanvas.SetActive (true);
		Instantiate (character, character.transform.position, Quaternion.identity);
	}

	public void Play() {
		SceneManager.LoadScene ("Game");
	}

	public void Setting() {
		GameObject.Find ("DataManager").GetComponentInChildren<FileLoader> ().enabled = true;
	}

	public void Quit() {
		Application.Quit ();
	}

}
