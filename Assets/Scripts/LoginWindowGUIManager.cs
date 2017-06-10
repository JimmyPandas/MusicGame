using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class LoginWindowGUIManager : MonoBehaviour {

	public GameObject mainCanvas;
	public GameObject musicLibraryCanvas;
	public Dropdown dropdown;
	private bool musicChosen = false;
	public string searchPath = "";
	private Dictionary<string, string> musicOptionsDic = new Dictionary<string, string>();

	private DataManager dataManager;
	public InputField durationInputField;
	public InputField hopInputField;
	private AnalysisFileProcessor analysisFileProcessor;
	private const int OVERALL_MUSIC_FILE_INDEX = -1;

	// Use this for initialization
	void Start () {
		searchPath =  Application.dataPath;
		string parentDir = Directory.GetParent (searchPath).FullName;
		while (parentDir.Length >= 16) {
			searchPath = parentDir;
			parentDir = Directory.GetParent (searchPath).FullName;
		}
		dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		analysisFileProcessor = gameObject.GetComponentInChildren<AnalysisFileProcessor> ();

		StreamWriter sw = new StreamWriter(searchPath + "/FilePathSetting.txt", true);
		sw.Flush ();
		sw.WriteLine ("We will search all local wav files within directory: " + searchPath);
		sw.WriteLine("You shold put ffmpeg exe in: " + searchPath + "/ffmpeg/ffmpeg");
		sw.Close();

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
	}

	public void MusicLibraryConfirm() {
		analysisFileProcessor.SetDurationAndHopSize (durationInputField, hopInputField);
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
			analysisFileProcessor.classificationFilesDic.Clear();
			dataManager.attributeDataDic.Clear ();
			string path = musicOptionsDic [musicOption];
			dataManager.path = path;
			musicChosen = true;
			analysisFileProcessor.StartCoroutine("LoadAnalysisResultFiles");
			analysisFileProcessor.SplitMusicFileIntoMultipleTracks ();
			analysisFileProcessor.LoadAttrbuteDataFromFiles ();
			if (dataManager.attributeDataDic.ContainsKey (OVERALL_MUSIC_FILE_INDEX)) {
				AttributeData data = dataManager.attributeDataDic [OVERALL_MUSIC_FILE_INDEX];
				dataManager.currentAttributeData = data;
				dataManager.isBright = data.isBright;
			}
		}
	}

}
