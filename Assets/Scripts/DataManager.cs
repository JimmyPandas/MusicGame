using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Manages data for persistance between levels.</summary>
public class DataManager : MonoBehaviour 
{
	public static DataManager instance;
	public string pitch_csv_path = "";
	public string beat_csv_path = "";
	public string path = "";
	public AttributeData currentAttributeData = new AttributeData ();
	public Dictionary<int, AttributeData> attributeDataDic = new Dictionary<int, AttributeData>();
	public float music_length;
	public bool isBright;

	// Use this for initialization
	void Start () {
	}

	/// <summary>Awake is called when the script instance is being loaded.</summary>
	void Awake() {
		if (instance == null) {
			// Do not destroy this object, when we load a new scene.
			DontDestroyOnLoad (gameObject);
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}


}
