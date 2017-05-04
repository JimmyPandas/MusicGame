using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit) && hit.transform.CompareTag("Item")) {
			GameObject fruit = hit.transform.gameObject;
			FruitController fruitController = fruit.GetComponentInChildren<FruitController> ();
			GUIManager guiManager = GameObject.Find ("GUIManager").GetComponentInChildren<GUIManager> ();
			Destroy (fruit);
			if (fruitController.IfScoreable()){
				guiManager.AddScore (10);
			} else {
				guiManager.LoseScore (10);
			}
		}
	}
}
