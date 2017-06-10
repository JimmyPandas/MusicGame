using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.InteropServices;
using System;

public class AnalysisFileProcessor : MonoBehaviour {

	[DllImport ("AudioProcessorPlugin")]
	private static extern void detectPitch (int algoNum, string audio_file, string output_file);

	[DllImport ("AudioProcessorPlugin")]
	private static extern void extractRhythm (string audio_file, string output_file);

	[DllImport ("AudioProcessorPlugin")]
	private static extern int extractMusicSVM (string audio_file_name, string output_file_name, string profile_file_name);

	[DllImport ("AudioProcessorPlugin")]
	private static extern int extractMusic (string input_file_name, string output_file_name, string profile_file_name);

	private Clock duration = new Clock();
	private int hop;
	private DataManager dataManager;
	private LoginWindowGUIManager guiManager;
	public Dictionary<int, string> classificationFilesDic = new Dictionary<int, string> ();
	private const int OVERALL_MUSIC_FILE_INDEX = -1;

	// Use this for initialization
	void Start () {
	    dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		guiManager = gameObject.GetComponentInChildren<LoginWindowGUIManager> ();

	}


	public void SetDurationAndHopSize(InputField durationInputField, InputField hopInputField) {
		try {
			duration.seconds = int.Parse (durationInputField.text);
			hop = int.Parse (hopInputField.text);
		} catch (FormatException exception) {
			duration.seconds = 45;
			hop = 5;
		}
	}

	IEnumerator LoadAnalysisResultFiles(){
		string filename = Path.GetFileNameWithoutExtension (dataManager.path);
		string resultFolderPath = guiManager.searchPath + "/Results/" + duration.CalcTotalTime() + "_duration_" + hop + "_shift";
		if (!Directory.Exists (resultFolderPath)) {
			Directory.CreateDirectory (resultFolderPath);
		}
		string resultFilePath = resultFolderPath +  "/" + filename + "_";

		string pitch_csv_path = resultFilePath + "pitch_result.csv";
		if(!File.Exists(pitch_csv_path)) { 
			detectPitch (0, dataManager.path, pitch_csv_path);
		}
		dataManager.pitch_csv_path = pitch_csv_path;

		string beat_csv_path = resultFilePath + "beat_result.csv";
		if(!File.Exists(beat_csv_path)) { 
			extractRhythm (dataManager.path, beat_csv_path);
		}
		dataManager.beat_csv_path = beat_csv_path;
		if (!File.Exists (resultFilePath + "descriptor.txt")) {
			extractMusic (dataManager.path, resultFilePath + "descriptor.txt", "");
		}

		if (!File.Exists (resultFilePath + "classfiresult.json")) {
			extractMusicSVM (resultFilePath + "descriptor.txt", resultFilePath + "classfiresult.json", "");
		}

		GetMusicFileLength (resultFilePath + "classfiresult.json");
		classificationFilesDic.Add (OVERALL_MUSIC_FILE_INDEX, resultFilePath + "classfiresult.json");
		Debug.Log ("Finish Loading analyzing whole file");
		yield return null;
	}

	public void SplitMusicFileIntoMultipleTracks() {
		string searchPath = guiManager.searchPath;
		Clock start_time = new Clock ();
		const int smallestWindow = 5;
		int num = 0;
		string filename = Path.GetFileNameWithoutExtension (dataManager.path) + num;
		string output_file_path = searchPath + "/" + filename + ".wav";
		string resultFolderPath = searchPath + "/Results/" + duration.CalcTotalTime() + "_duration_" + hop + "_shift";
		if (!Directory.Exists (resultFolderPath)) {
			Directory.CreateDirectory (resultFolderPath);
		}
		while (start_time.CalcTotalTime() + smallestWindow < dataManager.music_length) {
			string resultFilePath = resultFolderPath + "/" + filename + "_";;
			ExecutableRunner runner = new ExecutableRunner ();
			runner.run (searchPath, start_time, output_file_path, duration);

			if (!File.Exists (resultFilePath + "descriptor.txt")) {
				extractMusic (output_file_path, resultFilePath + "descriptor.txt", "");
			}
			if (!File.Exists (resultFilePath + "classfiresult.json")) {
				extractMusicSVM (resultFilePath + "descriptor.txt", resultFilePath + "classfiresult.json", "");
			}

			classificationFilesDic.Add (start_time.CalcTotalTime (), resultFilePath + "classfiresult.json");
			File.Delete (output_file_path);
			start_time.increaseTimeBySeconds(hop);
			num++;
			filename = Path.GetFileNameWithoutExtension (dataManager.path) + num;
			output_file_path = searchPath + "/" + filename + ".wav";

		}
		Debug.Log ("Finish splitting whole file");
	}

	public void LoadAttrbuteDataFromFiles() {
		foreach(KeyValuePair<int, string> pair in classificationFilesDic ) {
			LoadAttributeData (pair.Value, pair.Key);
		}
		Debug.Log ("Finish loading whole file data");
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
}
