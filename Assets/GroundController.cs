using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	void OnTriggerStay(Collider other) {
		Debug.logger.Log("Collide!");
		GameObject gameObject = other.gameObject;
		if (gameObject.CompareTag ("Item")) {
			gameObject.transform.Translate(Vector3.right * 2 * Time.deltaTime);
			gameObject.GetComponentInChildren<ItemController> ().SetSpeed (0);
			Destroy(gameObject);
			GUIManager guiManager = Camera.main.GetComponentInChildren<GUIManager> ();
			guiManager.LoseScore(10);
		}
	}
}
