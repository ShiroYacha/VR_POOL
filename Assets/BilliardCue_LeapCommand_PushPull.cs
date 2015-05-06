using UnityEngine;
using System.Collections;
using Leap;

public class BilliardCue_LeapCommand_PushPull : MonoBehaviour {

	GameObject controller;
	bool closedHand = false;
	bool openedHand = false;
	bool enterHand = false;
	bool leaveHand = false;

	// Use this for initialization
	void Start () {
		BilliardCue_Control.OnRotateCue += RotateCueWithWheel;
		BilliardCue_Control.OnShootingCue += ShootCueWithLeftMouse;
		BilliardCue_Control.OnReleaseCue += ReleaseCueWithLeftMouse;
		BilliardCue_Control.OnUsePullingeCue += usePull;
		BilliardCue_Control.OnPullingCue += GrabbingCue;
		BilliardCue_Control.ioHandView += ioHand;
		controller = GameObject.Find ("HandController");
	}

	bool usePull(){
		return true;
	}

	/// </summary>
	/// <returns>The direction of the rotation (between -1 and 1).</returns>
	float RotateCueWithWheel()
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
		return wheel * 0.5f;// 0.5 improve the precision
	}
	
	/// <summary>
	/// Shoots the cue with left mouse.
	/// </summary>
	/// <returns><c>true</c>, if cue with mouse is shooting, <c>false</c> otherwise.</returns>
	bool ShootCueWithLeftMouse()
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
	
	/// <summary>
	/// Releases the cue with left mouse.
	/// </summary>
	/// <returns><c>true</c>, if cue with left mouse was released, <c>false</c> otherwise.</returns>
	bool ReleaseCueWithLeftMouse()
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

	float GrabbingCue(){
		LeapCommand leap = controller.GetComponent<LeapCommand> ();
		Vector position = leap.getPosition ();
		int max_dist = 150;
		float dist = 0.0f;
		if (position.z > 0)
			dist = position.z;
		if (dist > max_dist)
			dist = max_dist;
		dist = dist / max_dist * 100;
		return dist;
	}

	int ioHand(){
		LeapCommand leap = controller.GetComponent<LeapCommand> ();
		int count = leap.getHandCount ();
		return count;
	}
}