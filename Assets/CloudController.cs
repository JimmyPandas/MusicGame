using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x <= -15) {
			Destroy (gameObject);
		} else {
			transform.Translate(Vector3.left * speed);
		}
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}
}
