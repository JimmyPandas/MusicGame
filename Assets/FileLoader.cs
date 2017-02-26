using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileLoader : MonoBehaviour {
	
	float length;
	private string path = "";

	// Use this for initialization
	void Start () {
		SelectFile ();
	}
	
	// Update is called once per frame
	void Update () {

	}


	void SelectFile () {
		path = "/Users/jimmyliu/Documents/MusicGame/Assets/Music/陈希郡-那原点.mp3";
		GetComponentInChildren<DataManager> ().path = path;
		Debug.logger.Log (Application.persistentDataPath);
	}


}
