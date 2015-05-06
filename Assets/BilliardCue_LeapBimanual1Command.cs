using UnityEngine;
using System.Collections;
using Leap;

public class BilliardCue_LeapBimanual1Command : MonoBehaviour
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
		BilliardCue_Control.OnRotateCue += RotateCueWithRelativePosition;
		BilliardCue_Control.OnShootingCue += ShootCueWithHandGrab;
		BilliardCue_Control.OnReleaseCue += ReleaseCueWithHandGrab;
		controller = GameObject.Find ("HandController");
	}
	
	/// </summary>
	/// <returns>The direction of the rotation (between -1 and 1).</returns>
	float RotateCueWithRelativePosition ()
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

	void OnGUI()
	{
		GUI.Label (new Rect (15, 100, 100, 100), "dist = " +dist.ToString());
	}
	
	/// <summary>
	/// Shoots the cue with left mouse.
	/// </summary>
	/// <returns><c>true</c>, if cue with mouse is shooting, <c>false</c> otherwise.</returns>
	bool ShootCueWithHandGrab ()
	{
		LeapCommandBimanual leap = controller.GetComponent<LeapCommandBimanual> ();
		if (leap.getValidGesture() && leap.getBothClose ()) {
			startShooting = true;
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// Releases the cue with left mouse.
	/// </summary>
	/// <returns><c>true</c>, if cue with left mouse was released, <c>false</c> otherwise.</returns>
	bool ReleaseCueWithHandGrab ()
	{
		LeapCommandBimanual leap = controller.GetComponent<LeapCommandBimanual> ();
		if (startShooting && leap.getValidGesture() && leap.getBothOpen()) {
			startShooting = false;
			return true;
		}
		return false;
	}
}
