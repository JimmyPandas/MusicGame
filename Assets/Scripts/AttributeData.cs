using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeData {

	public float happyFactor;
	public float sadFactor;
	public int aggresiveFactor = 1;
	public bool danceable;
	public bool isBright;
	public List<string> emotions = new List<string>();


	public override string ToString ()
	{
		string result = "current attribute data: \n";
		result = "happyFactor: " + happyFactor + "\n";
		result += "sadFactor: " + sadFactor + "\n";
		result += "aggresiveFactor: " + aggresiveFactor + "\n";
		result += "isBright: " + isBright + "\n";
		result += "danceable: " + danceable + "\n";
		result += "emotions: ";
		foreach (string emotion in emotions) {
			result += emotion + " ";
		}
		return result;
	}
}
