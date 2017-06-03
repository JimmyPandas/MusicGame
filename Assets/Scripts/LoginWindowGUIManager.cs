using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.InteropServices;
using System;

public class LoginWindowGUIManager : MonoBehaviour {
	
	[DllImport ("AudioProcessorPlugin")]
	private static extern void detectPitch (int algoNum, string audio_file, string output_file);

	[DllImport ("AudioProcessorPlugin")]
	private static extern void extractRhythm (string audio_file, string output_file);

	[DllImport ("AudioProcessorPlugin")]
	private static extern int extractMusicSVM (string audio_file_name, string output_file_name, string profile_file_name);

	[DllImport ("AudioProcessorPlugin")]
	private static extern int extractMusic (string input_file_name, string output_file_name, string profile_file_name);

	public GameObject mainCanvas;
	public GameObject musicLibraryCanvas;
	public Dropdown dropdown;
	private bool musicChosen = false;
	private string searchPath = "";
	private Dictionary<string, string> musicOptionsDic = new Dictionary<string, string>();
	public Dictionary<int, string> classificationFilesDic = new Dictionary<int, string> ();
	private const int OVERALL_MUSIC_FILE_INDEX = -1;
	private DataManager dataManager;
	public InputField durationInputField;
	public InputField hopInputField;
	private Clock duration = new Clock();
	private int hop;

	// Use this for initialization
	void Start () {
		searchPath =  Application.dataPath;
		string parentDir = Directory.GetParent (searchPath).FullName;
		while (parentDir.Length >= 16) {
			searchPath = parentDir;
			parentDir = Directory.GetParent (searchPath).FullName;
		}
		dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
	}

	public void Play() {
		RefreshMusicList ();
		if (!musicChosen) {
			//get all options available within this dropdown menu
			List<Dropdown.OptionData> menuOptions = dropdown.options;
			if (menuOptions.Count > 0) {
				int menuIndex = UnityEngine.Random.Range (0, menuOptions.Count);
				SetMusic (menuIndex);
			}
		}
		SceneManager.LoadScene ("Game");

	}

	IEnumerator LoadAnalysisResultFiles(){
		string filename = 	Path.GetFileNameWithoutExtension (dataManager.path);
		string resultFolderPath = searchPath + "/Results";
		if (!Directory.Exists (resultFolderPath)) {
			Directory.CreateDirectory (resultFolderPath);
		}
		resultFolderPath += "/" + filename + "_";

		string pitch_csv_path = resultFolderPath + "pitch_result.csv";
		if(!File.Exists(pitch_csv_path)) { 
			detectPitch (0, dataManager.path, pitch_csv_path);
		}
		dataManager.pitch_csv_path = pitch_csv_path;

		string beat_csv_path = resultFolderPath + "beat_result.csv";
		if(!File.Exists(beat_csv_path)) { 
			extractRhythm (dataManager.path, beat_csv_path);
		}
		dataManager.beat_csv_path = beat_csv_path;
		if (!File.Exists (resultFolderPath + "descriptor.txt")) {
			extractMusic (dataManager.path, resultFolderPath + "descriptor.txt", "");
		}

		if (!File.Exists (resultFolderPath + "classfiresult.json")) {
			extractMusicSVM (resultFolderPath + "descriptor.txt", resultFolderPath + "classfiresult.json", "");
		}

		GetMusicFileLength (resultFolderPath + "classfiresult.json");
		classificationFilesDic.Add (OVERALL_MUSIC_FILE_INDEX, resultFolderPath + "classfiresult.json");
		yield return null;
	}

	private void SplitMusicFileIntoMultipleTracks() {
		Clock start_time = new Clock ();
		const int smallestWindow = 5;
		int num = 0;
		string filename = Path.GetFileNameWithoutExtension (dataManager.path) + num;
		string output_file_path = searchPath + "/" + filename + ".wav";
		string resultFolderPath = searchPath + "/Results";
		if (!Directory.Exists (resultFolderPath)) {
			Directory.CreateDirectory (resultFolderPath);
		}
		Debug.Log (duration.CalcTotalTime ());
		Debug.Log (hop);
		while (start_time.CalcTotalTime() + smallestWindow < dataManager.music_length) {
			resultFolderPath = searchPath + "/Results/" + filename + "_";
			ExecutableRunner runner = new ExecutableRunner ();
			runner.run (searchPath, start_time, output_file_path, duration);
	
			if (!File.Exists (resultFolderPath + "descriptor.txt")) {
				extractMusic (output_file_path, resultFolderPath + "descriptor.txt", "");
			}
			if (!File.Exists (resultFolderPath + "classfiresult.json")) {
				extractMusicSVM (resultFolderPath + "descriptor.txt", resultFolderPath + "classfiresult.json", "");
			}

			classificationFilesDic.Add (start_time.CalcTotalTime (), resultFolderPath + "classfiresult.json");
			File.Delete (output_file_path);
			start_time.increaseTimeBySeconds(hop);
			num++;
			filename = Path.GetFileNameWithoutExtension (dataManager.path) + num;
			output_file_path = searchPath + "/" + filename + ".wav";

		}
//		yield return null;
	}

	private void LoadAttrbuteDataFromFiles() {
		foreach(KeyValuePair<int, string> pair in classificationFilesDic ) {
			LoadAttributeData (pair.Value, pair.Key);
		}

	}

	public void LoadAttributeData(string path, int start_time) {
		AttributeData data = new AttributeData();

		if (File.Exists (path)) {
			StreamReader sr = new StreamReader (path);
			string probabilityStr = sr.ReadLine ();
			while (probabilityStr != null) {
				if (probabilityStr.Contains ("probability")) {
					int startIndex = probabilityStr.IndexOf (": ") + 2;
					int length = probabilityStr.IndexOf (",") - startIndex;
					probabilityStr = probabilityStr.Substring (startIndex, length);
					string value = sr.ReadLine ();
					startIndex = value.IndexOf (": ") + 3;
					length = value.Length - startIndex - 1;
					value = value.Substring (startIndex, length);
					float probability = 0f;
					if (float.TryParse (probabilityStr, out probability)) {
						SetAttributeData (value, probability, data);
					}
		
				}
				probabilityStr = sr.ReadLine();
			}
			if (!dataManager.attributeDataDic.ContainsKey (start_time)) {
				dataManager.attributeDataDic.Add (start_time, data);
			}

		}
	}
		
	private void GetMusicFileLength(string path) {
		if (File.Exists (path)) {
			StreamReader sr = new StreamReader (path);
			string line = sr.ReadLine ();
			while (line != null) {
				if (line.Contains ("length")) {
					int startIndex = line.IndexOf (": ") + 2;
					int length = line.IndexOf (",") - startIndex;
					line = line.Substring (startIndex, length);
					dataManager.music_length = Mathf.Floor(float.Parse (line));

				}
				line = sr.ReadLine();
			}
		}
	}
		

	private void SetAttributeData(string attribute, float probability, AttributeData data) {
		switch (attribute) {
		case "bright":
			data.isBright = true;
			break;
		case "dark":
			data.isBright = false;
			break;
		case "danceable":
			data.danceable = true;
			break;
		case "happy":
			data.emotions.Add (attribute);
			data.happyFactor = probability;	
			break;
		case "sad":
			data.emotions.Add (attribute);
			data.sadFactor = probability;
			break;
		case "relaxed":
			data.emotions.Add (attribute);
			break;
		case "party":
			data.emotions.Add (attribute);
			break;
		case "aggressive":
			data.emotions.Add (attribute);
			data.aggresiveFactor = 2;
			break;
		default:
			break;
		}
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
//		dropdown.RefreshShownValue ();
	}

	public void MusicLibraryConfirm() {
		try {
			duration.seconds = int.Parse (durationInputField.text);
			hop = int.Parse (hopInputField.text);
		} catch (FormatException exception) {
			duration.seconds = 50;
			hop = 5;
		}
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
		if (musicOptionsDic.ContainsKey (musicOption)) {
			classificationFilesDic.Clear();
			dataManager.attributeDataDic.Clear ();
			string path = musicOptionsDic [musicOption];
			dataManager.path = path;
			musicChosen = true;
			StartCoroutine("LoadAnalysisResultFiles");
			SplitMusicFileIntoMultipleTracks ();
			LoadAttrbuteDataFromFiles ();
			if (dataManager.attributeDataDic.ContainsKey (OVERALL_MUSIC_FILE_INDEX)) {
				AttributeData data = dataManager.attributeDataDic [OVERALL_MUSIC_FILE_INDEX];
				dataManager.currentAttributeData = data;
				dataManager.isBright = data.isBright;
			}
		}
	}

}
