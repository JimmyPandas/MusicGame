using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			int numSamples = 2048 * 4;
			float[] spectrum = new float[numSamples];
			ResourseManager resourceManager = Camera.main.GetComponentInChildren<ResourseManager> ();
			AudioListener.GetSpectrumData (spectrum, 0, FFTWindow.Hamming);

			float maxAmplitude = 0.0f;
			float amplitueSum = 0.0f;

			float freq = 0.0f;
			string note = "";
			for (int i = 1; i < spectrum.Length - 1; i++) {
				if (maxAmplitude < spectrum [i]) {
					maxAmplitude = spectrum [i];
					freq = i;
				}
				amplitueSum += Mathf.Abs(spectrum [i]);
			}
				
			float fundFreq = freq * sampleRate / numSamples;

			float amplitude = amplitueSum / numSamples;
			note = calcNote (fundFreq);
			Debug.logger.Log (fundFreq + " " + note);
			resourceManager.InstantiateMusicSymbol (note);
			spawnRate = 2f;
		} else {
			spawnRate -= Time.deltaTime;
		}
	}


	private string calcNote(float fundFreq) {
		List<string> notes = new List<string> {"C", "D", "E", "F", "G", "A", "B"};
		string note = "";
		float noteFreq = 32.7f;
		const float multiple = 1.05946f;
		const float MAX_NOTE_FREQ = 3951.1f;
		while (fundFreq >= noteFreq && fundFreq <= MAX_NOTE_FREQ) {
			if (fundFreq >= noteFreq * 2) {
				noteFreq *= 2;
			} else {
				int pow = 0;
				while (fundFreq > noteFreq && pow <= 6) {
					if (pow != 2) {
						noteFreq *= Mathf.Pow (multiple, 2);				
					} else {
						noteFreq *= multiple;
					}
					noteFreq = Mathf.Round (noteFreq * 10) / 10;
					if (fundFreq >= noteFreq) {
						pow++;
					}

				}
				return notes [pow];
			}
		}
		return note;
	}
		
}
