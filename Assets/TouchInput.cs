using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit) && hit.transform.CompareTag("Item")) {
				Destroy (hit.transform.gameObject);
				GUIManager guiManager = GameObject.Find("GUIManager").GetComponentInChildren<GUIManager> ();
				guiManager.AddScore(10);
			}
		}
	}
}
