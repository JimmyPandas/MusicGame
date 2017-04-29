using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameObject gameObject = other.gameObject;
		if (gameObject.CompareTag ("Item")) {
			Destroy(gameObject);
			GUIManager guiManager = GameObject.Find("GUIManager").GetComponentInChildren<GUIManager> ();
			guiManager.LoseScore(10);
		}
	}

}
