using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class CSVParsor {
	public string path;
	private char fieldSeperator = ','; // It defines field seperate chracter
	private List<string> records = new List<string>();

	/* Read data from CSV file and store then into list of string
	records.
	*/
	public void ReadAllLines(){
		StreamReader sr = new StreamReader (path);
		string record = sr.ReadLine ();
		while(record != null) {
			records.Add (record);
			record = sr.ReadLine ();
		}
	}

	/* Fetch and remove the first record from records list.*/
	public List<string> ReadRecord() {
		if (records.Count > 0) {
			string record = records [0];
			string[] fields = record.Split (fieldSeperator);
			records.RemoveAt (0);
			return new List<string> (fields);
		} 
		return null;
	}
		

}
	