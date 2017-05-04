using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class test : MonoBehaviour {

	void Update()
	{
		int numSamples = 2048;
		float[] spectrum = new float[numSamples];

		AudioListener.GetSpectrumData (spectrum, 0, FFTWindow.Hamming);
		for (int i = 1; i < spectrum.Length - 1; i++) {
			Debug.DrawLine (new Vector3 (i - 1, spectrum [i] + 10, 0), new Vector3 (i, spectrum [i + 1] + 10, 0), Color.red);
			Debug.DrawLine (new Vector3 (i - 1, Mathf.Log (spectrum [i - 1]) + 10, 2), new Vector3 (i, Mathf.Log (spectrum [i]) + 10, 2), Color.red);

		}
	}
}
