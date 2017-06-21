using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeData {

	public int time;
	public float happyFactor = 0f;
	public float sadFactor = 0f;
	public int aggressiveFactor = 1;
	public bool danceable;
	public bool isBright;
	public List<string> emotions = new List<string>();

	public override string ToString ()
	{
		string result = "time: " + time + "\n"; 
		result += "happyFactor: " + Mathf.Round(happyFactor * 1000) / 1000f + "\n";
		result += "sadFactor: " + Mathf.Round(sadFactor * 1000) / 1000f + "\n";
		result += "aggressiveFactor: " + aggressiveFactor + "\n";
		result += "isBright: " + isBright + "\n";
		result += "danceable: " + danceable + "\n";
		result += "emotions: ";
		foreach (string emotion in emotions) {
			result += emotion + " ";
		}
		result += "\n";
		return result;
	}

	public string ToCSVString() {
		string result = time.ToString() + ","; 
		result += happyFactor.ToString() + ",";
		result += sadFactor.ToString() + ",";
		result += aggressiveFactor.ToString() + ",";
		result += isBright.ToString() + ",";
		result += danceable.ToString() + ",";
		for (int i = 0; i < emotions.Count; i++) {
			if (i != emotions.Count - 1) {
				result += emotions [i] + "/";
			} else {
				result += emotions [i];
			}
		}
		result += "\r";
		return result;
	}
}
