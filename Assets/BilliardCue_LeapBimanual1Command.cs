using UnityEngine;
using System.Collections;
using Leap;

public class BilliardCue_LeapBimanual1Command : BilliardCue_BasicCommand
{
	GameObject controller;
	bool closedHand = false;
	bool openedHand = false;
	bool startShooting = false;
	float dist = 0.0f;
	float DIST_MAX = 0.015f;
	float DIST_MIN = 0.07f;

	// Use this for initialization
	void Start ()
	{
		controller = GameObject.Find ("HandController");
		GameSystem_8Ball.RegisterCommand (3,this);
	}

	public override string DisplayName { get{return "Leap bimanual open/close";}}

	protected override float RotateCue()
	{
		LeapCommandBimanual leap = controller.GetComponent<LeapCommandBimanual> ();
		float wheel = 0.0f;
		if (leap.getValidGesture ()) {
			Vector leftPos = leap.getLeftHandPosition ();
			Vector rightPos = leap.getRightHandPosition ();
			bool bothOpen = leap.getBothOpen ();
			dist = 0.0f;
			int limit = 70;
			int sensitivity = 2500;
			if (bothOpen) {
				float left2rightZOffset = leftPos.z - rightPos.z;
				if(Mathf.Abs(left2rightZOffset)>limit){
					dist = (leftPos - rightPos).Magnitude / sensitivity;
					// normalize dist
					dist = (dist-DIST_MIN)/(DIST_MAX-DIST_MIN)*10.0f;
					dist = dist*dist/100.0f;
					// rescale
					dist *= 0.05f;
					wheel = Mathf.Sign (left2rightZOffset) * dist ;
				}
			}
		}
		return wheel * 0.5f;// 0.5 improve the precision
	}
	
	protected override bool ShootCue()
	{
		LeapCommandBimanual leap = controller.GetComponent<LeapCommandBimanual> ();
		if (leap.getValidGesture() && leap.getBothClose ()) {
			startShooting = true;
			return true;
		}
		return false;
	}

	protected override bool ReleaseCue()
	{
		LeapCommandBimanual leap = controller.GetComponent<LeapCommandBimanual> ();
		if (startShooting && leap.getValidGesture() && leap.getBothOpen()) {
			startShooting = false;
			return true;
		}
		return false;
	}

	protected override int IoHand(){
		LeapCommandBimanual leap = controller.GetComponent<LeapCommandBimanual> ();
		int count = leap.getHandCount ();
		return count;
	}
}
