using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour {
	
	private string note = "";
	public float scoreableTime = 0.65f;
	private string register;
	private float differenceInScale;
	private float speed;
	private bool scoreable = true;
	public Animator animator;
	private bool removed = false;
	private bool noteRemoved = false;
	private string emotion;
	public bool danceable;
	public GameObject happyEmotion;
	public GameObject sadEmotion;
	public GameObject aggresiveEmotion;
	public GameObject relaxedEmotion;
	public GameObject partyEmotion;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		int multiple = 0;
		if (int.TryParse (register, out multiple)) {
			gameObject.transform.localScale *= Mathf.Sqrt(multiple / 2f);
		} 

		animator = GetComponentInChildren<Animator> ();
		animator.Play ("FruitSpawning");
		animator.SetFloat ("FallingSpeed", speed);
	}

	// Update is called once per frame
	void Update () {

		if(animator.GetCurrentAnimatorStateInfo (0).IsName ("FruitIdel")) {
			if (danceable) {
				animator.SetTrigger ("Dance");
			} else {
				animator.SetTrigger ("NotDance");
			}
		}

		if (scoreable) {
			scoreableTime -= Time.deltaTime;
		}

		/* If scoreable time < 0, then we will remove the correponding note from notes and
		clear the emotion of the fruit.
		*/
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
	
		/* If the fruit is in removed status, then the fruit will be destroyed. */
		if(removed) {
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
	}

	public void ShowEmotion(){
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

	public void SetRegister(string register) {
		this.register = register;
	}

}
