using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioProcessor : MonoBehaviour {

	float spawnRate = 0f;
	private int sampleRate;
	private ResourseManager resourceManager;
	private CSVParsor pitchCSVParsor;
	private CSVParsor beatCSVParsor;
	float nextBeatInterval = 0f;

	// Use this for initialization
	void Start () {
		DataManager dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		pitchCSVParsor = new CSVParsor ();
		pitchCSVParsor.path = dataManager.pitch_csv_path;
		pitchCSVParsor.ReadAllLines ();

		beatCSVParsor = new CSVParsor ();
		beatCSVParsor.path = dataManager.beat_csv_path;
		beatCSVParsor.ReadAllLines ();
		Debug.Log(beatCSVParsor.ReadRecord ()[0]);

		GUIManager guiManager = GameObject.Find ("GUIManager").GetComponentInChildren<GUIManager> ();
		resourceManager = guiManager.GetComponentInChildren<ResourseManager> ();
		UpdateNextSpawnRate ();

	}

	// Update is called once per frame
	void Update () {
		LoadAttributeData ();
		if (sampleRate == 0) {
			AudioSource audioSource = GetComponent<AudioSource> ();
			sampleRate = audioSource.clip.frequency;
		}
		List<string> fields = new List<string> ();
		if (spawnRate <= 0) {
			spawnRate = nextBeatInterval;
			UpdateNextSpawnRate ();

			string note = "";
			fields = pitchCSVParsor.ReadRecord();

			while (fields != null && float.Parse(fields [0]) < Time.timeSinceLevelLoad) {
				fields = pitchCSVParsor.ReadRecord ();
			}
			if (fields != null) {
				float pitch = 0f;
				if (float.TryParse (fields [1], out pitch)) {
					string result = calcNoteAndZone (pitch);
					if (result.Length == 2) {
						note = result [0].ToString ();
						string zone = result [1].ToString ();
						resourceManager.InstantiateMusicSymbol (note, zone, nextBeatInterval);
					}
				} 
			}
		

		} else {
			spawnRate -= Time.deltaTime;
		}
	}

	private void LoadAttributeData() {
		DataManager dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		foreach (KeyValuePair<int, AttributeData> pair in dataManager.attributeDataDic) {
			int time = pair.Key;
			if (time <= Time.timeSinceLevelLoad) {
				AttributeData data = dataManager.attributeDataDic [time];
				dataManager.currentAttributeData = data;
				dataManager.attributeDataDic.Remove (time);
				break;
			}

		}
	}

	private void UpdateNextSpawnRate() {
		nextBeatInterval = 0f;
		List<string> fields = new List<string> ();
		for (int i = 0; i < 2; i++) {
			fields = beatCSVParsor.ReadRecord ();
			if (fields != null) {
				nextBeatInterval += float.Parse (fields [1]);
			}
		}
	}

	private string calcNoteAndZone(float fundFreq) {
		int zone = 1;
		List<string> notes = new List<string> {"C", "D", "E", "F", "G", "A", "B"};
		string note = "";
		float noteFreq = 32.7f;
		const float multiple = 1.05946f;
		const float MAX_NOTE_FREQ = 3951.1f;
		float prevNoteFreq = 0;
		while (fundFreq >= noteFreq && fundFreq <= MAX_NOTE_FREQ) {
			if (fundFreq >= noteFreq * 2) {
				noteFreq *= 2;
				zone++;
			} else {
				int pow = 0;
				while (fundFreq > noteFreq && pow <= 6) {
					prevNoteFreq = noteFreq;
					if (pow != 2) {
						noteFreq *= Mathf.Pow (multiple, 2);				
					} else {
						noteFreq *= multiple;
					}
					noteFreq = Mathf.Round (noteFreq * 10) / 10;
				
					if (fundFreq >= noteFreq || noteFreq - fundFreq < fundFreq - prevNoteFreq) {
						pow++;
					}

				}
				if (pow == 7) {
					pow = 0;
					zone++;
				}
				return notes [pow] + zone;
			}
		}
		return note + zone;
	}
		
}
