using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

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


	void SelectFile ()
	{
		#if UNITY_EDITOR
		path = EditorUtility.OpenFilePanel(
			"Overwrite with mp3",
			"",
			"wav");
		GetComponentInChildren<DataManager>().path = path;
		#endif

	}


}
