using UnityEngine;
using System.Collections;

public class BilliardCue_MouseCommand : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		BilliardCue_Control.OnRotateCue += RotateCueWithWheel;
		BilliardCue_Control.OnShootingCue += ShootCueWithLeftMouse;
		BilliardCue_Control.OnReleaseCue += ReleaseCueWithLeftMouse;
	}

	void RotateCueWithWheel()
	{
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		GameSystem_8Ball.ActivateCue (wheel * 0.5f);
	}

	bool ShootCueWithLeftMouse()
	{
		return Input.GetMouseButton (0);
	}

	bool ReleaseCueWithLeftMouse()
	{
		bool leftUp = Input.GetMouseButtonUp (0);
		return leftUp;
	}
}
