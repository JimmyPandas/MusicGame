using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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


	[MenuItem("Examples/Save Music to file")]
	void SelectFile ()
	{
		
		path = EditorUtility.OpenFilePanel(
			"Overwrite with mp3",
			"",
			"mp3, wav");
		GetComponentInChildren<DataManager>().path = path;
	}


}
