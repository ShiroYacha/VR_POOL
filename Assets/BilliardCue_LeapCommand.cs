﻿using UnityEngine;
using System.Collections;
using Leap;

public class BilliardCue_LeapCommand : BilliardCue_BasicCommand {
	
	GameObject controller;
	bool closedHand = false;
	bool openedHand = false;
	
	void Start () {

		controller = GameObject.Find ("HandController");
		GameSystem_8Ball.RegisterCommand (1,this);
	}

	public override string DisplayName { get{return "Leap open/close";}}

	protected override float RotateCue()
	{
		LeapCommand leap  = controller.GetComponent<LeapCommand> ();
		Vector pos = leap.getPosition ();
		int handcount = leap.getHandCount ();
		float strength = leap.getStrength ();
		float wheel = 0.0f;
		float dist = 0.0f;
		int limit = 50;
		int sensitivity = 800;
		if (handcount > 0 && strength != 1) {
			if (pos.x > limit) {
				dist = (pos.x - limit) / sensitivity;
				wheel = dist;
			}
			if (pos.x < -limit) {
				dist = -(pos.x + limit) / sensitivity;
				wheel = -dist;
			}
		}
		return -wheel * 0.5f;// 0.5 improve the precision
	}

	protected override bool ShootCue()
	{
		LeapCommand leap  = controller.GetComponent<LeapCommand> ();
		int handcount = leap.getHandCount ();
		float strength = 0.0f;
		if (handcount > 0) {
			strength = leap.getStrength ();
			if (strength == 0)
				openedHand = true;
		} else {
			openedHand = false;
		}
		if (strength == 1.0f && openedHand) {
			closedHand = true;
			return true;
		} else {
			return false;
		}
	}

	protected override bool ReleaseCue()
	{
		LeapCommand leap  = controller.GetComponent<LeapCommand> ();
		int handcount = leap.getHandCount ();
		float strength = 0.0f;
		if (handcount > 0) {
			strength = leap.getStrength ();
		}
		if (closedHand && strength == 0.0f) {
			closedHand = false;
			return true;
		} else {
			return false;
		}
	}

	protected override int IoHand(){
		LeapCommand leap = controller.GetComponent<LeapCommand> ();
		int count = leap.getHandCount ();
		return count;
	}
}
