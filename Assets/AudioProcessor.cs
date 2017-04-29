using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioProcessor : MonoBehaviour {

	float spawnRate = 1.5f;
	private int sampleRate;
	
	// Update is called once per frame
	void Update () {
		if (sampleRate == 0) {
			AudioSource audioSource = GetComponent<AudioSource> ();

			sampleRate = audioSource.clip.frequency;
			Debug.logger.Log ("rate: " + audioSource.clip.frequency);
		}

		if (spawnRate <= 0) {
			int numSamples = 2048;
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


			resourceManager.InstantiateMusicSymbol (note);
//			Debug.logger.Log (max);
//			StartCoroutine (sleep ());
			spawnRate = 1.5f;
		} else {
			spawnRate -= Time.deltaTime;
		}
	}


	private string calcNote(float fundFreq) {
		string note = "";
		if (32.7 <= fundFreq && fundFreq < 65.4) {
			note = "Do";
		} else if (65.4 <= fundFreq && fundFreq < 130.8) {
			note = "Re";
		} else if (130.8 <= fundFreq && fundFreq < 261.6) {
			note = "Mi";
		} else if (261.6 < fundFreq && fundFreq < 523.3) {
			note = "Fa";
		} else if (523.3 < fundFreq && fundFreq < 1046.5) {
			note = "So";
		} else if (1046.5 < fundFreq && fundFreq < 2093.0) {
			note = "La";
		} else if (2093.0 <= fundFreq && fundFreq <= 3952.0) {
			note = "Xi";
		}
		return note;
	}

	IEnumerator sleep() {
		yield return new WaitForSeconds(5);
	}
}
