using UnityEngine;
using System.Collections;

public class BilliardCue_MouseCommand : BilliardCue_BasicCommand {
	
	// Use this for initialization
	void Start () {
		GameSystem_8Ball.RegisterCommand (0,this);
	}

	public override string DisplayName { get{return "Mouse";}}
	
	protected override float RotateCue()
	{
		float wheel = Input.GetAxis ("Mouse ScrollWheel");
		return wheel * 0.5f;// 0.5 improve the precision
	}


	protected override bool ShootCue()
	{
		return Input.GetMouseButton (0);
	}

	protected override bool ReleaseCue()
	{
		bool leftUp = Input.GetMouseButtonUp (0);
		return leftUp;
	}
}
