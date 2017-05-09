using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour {

	private Vector3 center;

	// Use this for initialization
	void Start () {
		center = GameObject.Find (gameObject.name + "Button").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(center, new Vector3(0, 0, 1), 20 * Time.deltaTime);
	}
}
