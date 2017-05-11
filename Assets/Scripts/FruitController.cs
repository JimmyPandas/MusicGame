using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {
	
	private string note = "";
	private float scoreableTime = 10f;
	private string zone;
	private float differenceInScale;
	private float speed;
	private bool scoreable = true;
	public Animator animator;
	public bool onGround = false;
	private float onGroundTime = 2f;
	private bool removed = false;

	// Use this for initialization
	void Start () {
		int multiple = 0;
		if (int.TryParse (zone, out multiple)) {
			speed = GetNoteSpeed (note) * multiple / 3;
//			float fruitScale = multiple / 4f;
//			differenceInScale = fruitScale - transform.localScale.x;
//			gameObject.transform.localScale += new Vector3(differenceInScale, differenceInScale, 0);
//			gameObject.transform.position += new Vector3 (6f * differenceInScale, -6f * differenceInScale, 0);
		} 
		animator = GetComponentInChildren<Animator> ();
		animator.speed = this.speed;
		animator.Play ("FruitFalling");
	}

	// Update is called once per frame
	void Update () {
		
		scoreableTime -= Time.deltaTime;
		if (scoreableTime < 0) {
			SetScoreable (false);
		}
		if (onGround && animator.GetCurrentAnimatorStateInfo (0).IsName ("FruitFalling")) {
			animator.SetTrigger ("OnGround");
		} 
		if (onGround) {
			onGroundTime -= Time.deltaTime;
		}

		if(onGroundTime < 0 || removed) {
			if (gameObject.transform.parent != null) {
				GUIManager guiManager = GameObject.Find ("GUIManager").GetComponentInChildren<GUIManager> ();
				ResourseManager rm = guiManager.GetComponentInChildren<ResourseManager> ();
				rm.RemoveNote (note);
				Destroy (gameObject.transform.parent.gameObject);
			}
		}
		Destroy (GameObject.Find ("parent"));
	
	}
		
	public void SetScoreable(bool scoreable){
		this.scoreable = scoreable;
	}

	public bool IfScoreable() {
		return scoreable;
	}

	public void SetRemoved(bool removed){
		this.removed = removed;
	}

	public float GetScoreTime() {
		return scoreableTime;
	}
		
	public void SetSpeed(float speed) {
		this.speed = speed;
	}


	public void SetNote(string note) {
		this.note = note;
	}

	public string GetNote() {
		return note;
	}

	public void SetZone(string zone) {
		this.zone = zone;
	}

	public float GetNoteSpeed(string note) {
		switch (note) {
		case "C":
			return 0.5f;
		case "D":
			return 0.75f;
		case "E":
			return 1f;
		case "F":
			return 1.5f;
		case "G":
			return 2f;
		case "A":
			return 2.5f;
		case "B":
			return 3f;
		default:
			return 3.5f;
		}
	}

}
