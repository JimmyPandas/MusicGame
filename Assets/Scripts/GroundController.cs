using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameObject gameObject = other.gameObject;
		if (gameObject.CompareTag ("Item")) {
			GUIManager guiManager = GameObject.Find("GUIManager").GetComponentInChildren<GUIManager> ();
			gameObject.GetComponentInChildren<FruitController> ().onGround = true;
			guiManager.LoseScore(10);
			gameObject.GetComponentInChildren<FruitController> ().SetRemoved (true);
		}
	}

}
