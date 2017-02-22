using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProcessor : MonoBehaviour {

	float sampleRate = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (sampleRate <= 0) {
			float[] spectrum = new float[256];
			ResourseManager resourceManager = Camera.main.GetComponentInChildren<ResourseManager> ();
			AudioListener.GetSpectrumData (spectrum, 0, FFTWindow.Hamming);

			var max = 0.0f;
			string note = "";
			for (int i = 1; i < spectrum.Length - 1; i++) {
				max = Mathf.Max (max, spectrum [i]);
			}
			if (0.0001f <= max && max <= 0.0006f) {
				note = "Do";
			} else if (0.0000f < max && max <= 0.0100f) {
				note = "Re";
			} else if (0.0200f < max && max <= 0.0300f) {
				note = "Mi";
			} else if (0.0400f < max && max <= 0.0500f) {
				note = "Fa";
			} else if (0.0600f < max && max <= 0.0700f) {
				note = "So";
			} else if (0.0800f < max && max <= 0.0900f) {
				note = "La";
			} else if (0.1100f <= max && max <= 0.1200f) {
				note = "Xi";
			}
			resourceManager.InstantiateMusicSymbol (note);
//			Debug.logger.Log (max);
//			StartCoroutine (sleep ());
			sampleRate = 1f;
		} else {
			sampleRate -= Time.deltaTime;
		}
	}

	IEnumerator sleep() {
		yield return new WaitForSeconds(5);
	}
}
