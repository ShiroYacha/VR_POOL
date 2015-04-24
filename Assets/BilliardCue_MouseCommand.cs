using UnityEngine;
using System.Collections;

public class BilliardCue_MouseCommand : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		BilliardCue_Control.OnRotateCue += RotateCueWithWheel;
		BilliardCue_Control.OnShootingCue += ShootCueWithLeftMouse;
		BilliardCue_Control.OnReleaseCue += ReleaseCueWithLeftMouse;
	}

	/// <summary>
	/// Rotates the cue with wheel.
	/// </summary>
	/// <returns>The direction of the rotation (between -1 and 1).</returns>
	float RotateCueWithWheel()
	{
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		return wheel * 0.5f;// 0.5 improve the precision
	}

	/// <summary>
	/// Shoots the cue with left mouse.
	/// </summary>
	/// <returns><c>true</c>, if cue with mouse is shooting, <c>false</c> otherwise.</returns>
	bool ShootCueWithLeftMouse()
	{
		return Input.GetMouseButton (0);
	}

	/// <summary>
	/// Releases the cue with left mouse.
	/// </summary>
	/// <returns><c>true</c>, if cue with left mouse was released, <c>false</c> otherwise.</returns>
	bool ReleaseCueWithLeftMouse()
	{
		bool leftUp = Input.GetMouseButtonUp (0);
		return leftUp;
	}
}
