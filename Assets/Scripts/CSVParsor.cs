using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class CSVParsor : MonoBehaviour{
	public TextAsset csvFile; // Reference of CSV file
	public string path;
	private char lineSeperater = '\n'; // It defines line seperate character
	private char fieldSeperator = ','; // It defines field seperate chracter

	// Read data from CSV file
	public string readData(){
		StreamReader sr = new StreamReader (path);
		string record = sr.ReadLine ();
		if(record != null) {
			string[] fields = record.Split(fieldSeperator);
			foreach(string field in fields) {
				
			}
			Debug.Log (fields [1]);
			return fields [1];
		}
		return "";
	}
		

}
	