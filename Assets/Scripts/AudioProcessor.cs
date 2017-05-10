using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[RequireComponent(typeof(AudioSource))]
public class AudioProcessor : MonoBehaviour {


	float spawnRate = 2f;
	private int sampleRate;
	
	// Update is called once per frame
	void Update () {
		if (sampleRate == 0) {
			AudioSource audioSource = GetComponent<AudioSource> ();

			sampleRate = audioSource.clip.frequency;
		}

		if (spawnRate <= 0) {
			const int numSamples = 8192;
			float[] spectrum = new float[numSamples];
			ResourseManager resourceManager = Camera.main.GetComponentInChildren<ResourseManager> ();
			AudioListener.GetSpectrumData (spectrum, 0, FFTWindow.Hamming);

			float maxAmplitude = 0.0f;
			float amplitueSum = 0.0f;

			float freq = 0.0f;
			string note = "";
			for (int i = 1; i < spectrum.Length - 1; i++) {
				if (spectrum[i] > 0.01f && maxAmplitude < spectrum [i]) {
					maxAmplitude = spectrum [i];
					freq = i;
				}
				amplitueSum += Mathf.Abs(spectrum [i]);
			}
				
			float fundFreq = freq * sampleRate / 2 / numSamples;

			float amplitude = amplitueSum / numSamples;
			string result = calcNoteAndZone (fundFreq);
			if (result.Length == 2) {
				note = result [0].ToString ();
				string zone = result [1].ToString();
				resourceManager.InstantiateMusicSymbol (note, zone);
				spawnRate = 2f;
			}

		} else {
			spawnRate -= Time.deltaTime;
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
