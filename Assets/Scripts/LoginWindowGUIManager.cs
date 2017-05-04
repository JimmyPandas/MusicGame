using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginWindowGUIManager : MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject mainMenu;
	public GameObject magician;
	public Dropdown dropdown;

	// Use this for initialization
	void Start () {
		mainCanvas.SetActive (true);
		Instantiate (magician, magician.transform.position, Quaternion.identity);
	}

	public void Play() {
		int menuIndex = dropdown.value;

		//get all options available within this dropdown menu
		List<Dropdown.OptionData> menuOptions = dropdown.options;

		//get the string value of the selected index
		string value = menuOptions [menuIndex].text;
		GameObject dataManager = GameObject.Find ("DataManager");
		string path = "Music/" + value;
		dataManager.GetComponentInChildren<DataManager> ().path = path;

		SceneManager.LoadScene ("Game");
	}

	public void Setting() {
	}
		

	public void Quit() {
		Application.Quit ();
	}

}
