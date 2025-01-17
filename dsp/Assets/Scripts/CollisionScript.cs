﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;



public class CollisionScript : MonoBehaviour {
	private bool beenTouched = false;
	public Text touchText;
	public Image flickersauce;
	private float levelTimer;
	public float TIMER_VALUE = 40.0f;

	void Start(){
		//In the LEARNING phase
		if (GameControl.control.startedTrials==false){
			//Shows the initial text and locks movement.
			GameControl.control.movementLocked = true;

			touchText.text = "When you're ready, follow the red arrows while paying attention to the objects on the side. \nPress X to continue.";

		}

		//In the TRIAL phase
		if(GameControl.control.startedTrials == true) {
			//Shows the initial text and locks movement.
			GameControl.control.movementLocked = true;
			flickersauce.enabled = true;

			touchText.text = "Take a few moments to look around and orient yourself.\nWhen you're ready, press X.";
			//Do we want this to also be X?^
	}
		TIMER_VALUE = PlayerPrefs.GetFloat("trialTime");
		levelTimer = TIMER_VALUE;
	}

	void Update(){

		/*
		Handles activity in the LEARNING phase.
		*/
		if (GameControl.control.startedTrials == false) {
			//Handles the lap progress.
			if (GameControl.control.arrowList.Count == 13) {
				GameControl.control.laps++;
				GameControl.control.arrowList.Clear ();
				Debug.Log ("Lap added! Laps: " + GameControl.control.laps);
			}
			if (GameControl.control.arrowList.Contains ("a13") && GameControl.control.arrowList.Count == 1) {
				GameControl.control.arrowList.Remove ("a13");
			}

			if (GameControl.control.laps == GameControl.control.LapsNumber) {
				GameControl.control.rollNextLevel ();
				GameControl.control.startedTrials = true;
			}

			//Pressing F starts the LEARNING phase.
			if (Input.GetKey (KeyCode.X) || Input.GetKey("joystick button 1")) {
				GameControl.control.movementLocked = false;
				touchText.color = Color.red;
				touchText.text = "Laps Remaining: " + (GameControl.control.LapsNumber - GameControl.control.laps);
			}
			//Pressing CTRL+F rolls the next level.
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.F)) {
				GameControl.control.rollNextLevel ();
				GameControl.control.startedTrials = true;
			}

			//Shows the remaining laps
			if (GameControl.control.movementLocked == false) {
				touchText.text= "Laps Remaining: " + (GameControl.control.LapsNumber - GameControl.control.laps);
			}
		}

		/*
		Handles activity in the TRIAL phase.
		*/
		else {
			//Pressing G starts the TRIAL phase.
			if (Input.GetKey (KeyCode.X) || Input.GetKey("joystick button 1")) {
				GameControl.control.movementLocked = false;
				flickersauce.color = Color.gray;
				touchText.color = Color.red;
				touchText.text = "Please Navigate to the " + GameControl.control.currentTargetObject;
			}
			//Pressing CTRL+F rolls the next level.
			if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.F)) {
				// GameControl.control.rollRankingLevel ();
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Ranking Level");
			}
			//Handles the timer.
			if (GameControl.control.movementLocked == false) {
				levelTimer -= Time.deltaTime;
				if (levelTimer < 0) {
					GameControl.control.movementLocked = true;
					// GameControl.control.rollRankingLevel();
					UnityEngine.SceneManagement.SceneManager.LoadScene ("Ranking Level");
				}

			}
		}
	}

	void OnTriggerEnter(Collider other){
		//in the learning phase
		if (GameControl.control.startedTrials == false) {
			if (other.gameObject.name.StartsWith("a") && !GameControl.control.arrowList.Contains(other.gameObject.name)){
				GameControl.control.arrowList.Add(other.gameObject.name);
				Debug.Log("Touched the " + other + " and added to list");
				Debug.Log (GameControl.control.arrowList.Count);
			}
		}

		//in the trial phase
		else {
			//updates the current box
			if(other.gameObject.name.StartsWith("box")){
				GameControl.control.currentBox = other.gameObject.name;
			}

			if (beenTouched == false) {
				Debug.Log ("Touched the " + other.gameObject.name);
				beenTouched = true;
			}
			if (beenTouched == true) {
				if (other.gameObject.name == GameControl.control.currentTargetObject) {
					// GameControl.control.rollRankingLevel ();
					UnityEngine.SceneManagement.SceneManager.LoadScene ("Ranking Level");
				}
			}
		}
	}
}
