using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {

	private float speed;
	private bool scoreable = true;
	public Animator animator;
	public bool onGround = false;
	private float onGroundTime = 2f;

	// Use this for initialization
	void Start () {
		animator = GetComponentInChildren<Animator> ();
		animator.speed = this.speed;
		animator.Play ("FruitFalling");
	}

	// Update is called once per frame
	void Update () {
		if (onGround && animator.GetCurrentAnimatorStateInfo (0).IsName ("FruitFalling")) {
			animator.Play ("FruitOnGround");
		} 
	    if (animator.GetCurrentAnimatorStateInfo (0).IsName ("FruitOnGround")) {
			onGroundTime -= Time.deltaTime;
		}

		if(onGroundTime < 0) {
			if (gameObject.transform.parent != null) {
				Destroy (gameObject.transform.parent.gameObject);
			}
		}
		Destroy (GameObject.Find ("parent"));
	
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
