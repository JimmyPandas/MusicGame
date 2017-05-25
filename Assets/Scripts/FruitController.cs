using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {
	
	private string note = "";
	public float scoreableTime = 0.65f;
	private string zone;
	private float differenceInScale;
	private float speed;
	private bool scoreable = true;
	public Animator animator;
	public bool onGround = false;
	private float onGroundTime = 2f;
	private bool removed = false;
	private bool noteRemoved = false;
	private string emotion;
	public Collider collider;
	public GameObject happyEmotion;
	public GameObject sadEmotion;
	public GameObject aggresiveEmotion;
	public GameObject relaxedEmotion;
	public GameObject partyEmotion;

	// Use this for initialization
	void Start () {
		int multiple = 0;
		if (int.TryParse (zone, out multiple)) {
//			differenceInScale = speed - transform.localScale.x;
//			gameObject.transform.localScale += new Vector3(differenceInScale, differenceInScale, 0);
//			gameObject.transform.position += new Vector3 (6f * differenceInScale, -6f * differenceInScale, 0);
			gameObject.transform.localScale *= Mathf.Sqrt(multiple / 2f);
		} 
		animator = GetComponentInChildren<Animator> ();
		animator.Play ("FruitSpawning");
		animator.SetFloat ("FallingSpeed", speed);
	}

	// Update is called once per frame
	void Update () {
		if (!onGround && animator.GetCurrentAnimatorStateInfo (0).IsName ("FruitFalling")) {
			transform.parent.transform.Translate (Vector3.down * Time.deltaTime * speed);
		}
		scoreableTime -= Time.deltaTime;
		if (scoreableTime < 0) {
			SetScoreable (false);
			GUIManager guiManager = GameObject.Find ("GUIManager").GetComponentInChildren<GUIManager> ();
			ResourseManager rm = guiManager.GetComponentInChildren<ResourseManager> ();
			if (!noteRemoved) {
				rm.RemoveNote (note);
				noteRemoved = true;
				ClearEmotion ();
			}
		}
		if (onGround && animator.GetCurrentAnimatorStateInfo (0).IsName ("FruitFalling")) {
			animator.SetTrigger ("OnGround");
		} 

		if (onGround) {
			onGroundTime -= Time.deltaTime;
		}

		if(onGroundTime < 0 || removed) {
			if (gameObject.transform.parent != null) {
				Destroy (gameObject.transform.parent.gameObject);
			}
		}
		Destroy (GameObject.Find ("parent"));
	
	}
		
	private void ClearEmotion() {
		switch (emotion) {
		case "happy":
			happyEmotion.SetActive (false);
			break;
		case "sad":
			sadEmotion.SetActive (false);
			break;
		case "relaxed":
			relaxedEmotion.SetActive (false);
			break;
		case "party":
			partyEmotion.SetActive(false);
			break;
		case "aggressive":
			aggresiveEmotion.SetActive(false);
			break;
		default:
			break;
		}
	}

	public void SetEmotion(string emotion){
		this.emotion = emotion;
		switch (emotion) {
		case "happy":
			happyEmotion.SetActive (true);
			break;
		case "sad":
			sadEmotion.SetActive (true);
			break;
		case "relaxed":
			relaxedEmotion.SetActive (true);
			break;
		case "party":
			partyEmotion.SetActive(true);
			break;
		case "aggressive":
			aggresiveEmotion.SetActive(true);
			break;
		default:
			break;
		}
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

	public void SetNoteRemoved(bool noteRemoved){
		this.noteRemoved = noteRemoved;
	}

	public bool IfNoteRemoved(){
		return noteRemoved;
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
		const float multiple = 1.05946f;
		switch (note) {
		case "C":
			return 1f;
		case "D":
			return Mathf.Pow (multiple, 2);
		case "E":
			return Mathf.Pow (multiple, 4);
		case "F":
			return Mathf.Pow (multiple, 5);
		case "G":
			return Mathf.Pow (multiple, 7);
		case "A":
			return Mathf.Pow (multiple, 9);
		case "B":
			return Mathf.Pow (multiple, 11);
		default:
			return 3.0f;
		}
	}

}
