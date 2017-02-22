using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

	private float speed;

	// Update is called once per frame
	void Update () {
		if (transform.position.y <= -6) {
			Destroy (gameObject);
			ResourseManager resourceManager = Camera.main.GetComponentInChildren<ResourseManager> ();
			resourceManager.LoseScore(10);
		} else {
			transform.Translate(Vector3.down * speed);
		}
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}
}
