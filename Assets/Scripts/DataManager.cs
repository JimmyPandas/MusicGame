using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : MonoBehaviour 
{
	public static DataManager instance;
	public string searchPath = "";
	public string path = "";
	float length;


	// Use this for initialization
	void Start () {
	}

	/// <summary>Awake is called when the script instance is being loaded.</summary>
	void Awake()
	{
		// Do not destroy this object, when we load a new scene.
		DontDestroyOnLoad(gameObject);
	}
}
