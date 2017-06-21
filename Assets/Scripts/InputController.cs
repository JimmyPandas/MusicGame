using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour {


	// Update is called once per frame
	void Update () {

		/* Set the input controller of the game. */
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (SceneManager.GetActiveScene ().name.Equals ("LoginWindow")) {
			if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) {
				if (Physics.Raycast (ray, out hit) && hit.transform.tag.Contains("Button")) {
					LoginWindowGUIManager guiManager = 
						GameObject.Find("GUIManager").GetComponentInChildren<LoginWindowGUIManager> ();
					string methodName = hit.transform.name.Replace ("Button", "");
					guiManager.Invoke(methodName, 0);
				}
			}
		} else {
	
			if (Physics.Raycast (ray, out hit) && hit.transform.CompareTag ("Item")) {
				GameObject fruit = hit.transform.gameObject;
				GUIManager guiManager = GameObject.Find ("GUIManager").GetComponentInChildren<GUIManager> ();
				ResourseManager rm = guiManager.GetComponentInChildren<ResourseManager>();
				FruitController fruitController = fruit.GetComponentInChildren<FruitController> ();
				if (fruitController.GetNote ().Equals (rm.GetCurrentNote ())) {
					fruitController.SetRemoved (true);
					if (!fruitController.IfNoteRemoved ()) {
						rm.RemoveNote (fruitController.GetNote ());
					}
					if (fruitController.IfScoreable ()) {
						guiManager.AddScore (10);
					} else {
						guiManager.LoseScore (10);
					}
				} else if(!fruitController.IfScoreable()) {
					fruitController.SetRemoved (true);
					guiManager.LoseScore (10);
				}
			}
		}
	}
}
