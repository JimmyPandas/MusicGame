using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	/* If fruits touch the ground collider, they will be destroyed and player
	will lose some score.
	*/
	void OnTriggerEnter(Collider other) {
		GameObject gameObject = other.gameObject;
		if (gameObject.CompareTag ("Item")) {
			GUIManager guiManager = GameObject.Find("GUIManager").GetComponentInChildren<GUIManager> ();
			guiManager.LoseScore(10);
			gameObject.GetComponentInChildren<FruitController> ().SetRemoved (true);
		}
	}

}
