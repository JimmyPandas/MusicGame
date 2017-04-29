using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {

	private float speed;
	private bool scoreable = true;

	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.down * speed * Time.deltaTime);
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public void SetScoreable(bool scoreable){
		this.scoreable = scoreable;
	}

	public bool IfScoreable() {
		return scoreable;
	}


}
