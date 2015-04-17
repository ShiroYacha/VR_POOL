using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSystem_8Ball : MonoBehaviour {

	static List<BilliardBall_Physics> _eightBalls= new List<BilliardBall_Physics>();
	static BilliardBall_Physics _whiteBall;

	public static bool Stabalized
	{
		get
		{
			return _whiteBall.IsSleeping && _eightBalls.TrueForAll(b=>b.IsSleeping);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void BallInHole(BilliardBall_Physics objScript)
	{
		// Modify game status
		int number;
		if (objScript.gameObject.name == "WhiteBall") {
			// White ball falls
			objScript.ResetInitPosition();
		} else if (int.TryParse(objScript.gameObject.name,out number)) {
			// 8-Ball falls
			objScript.ResetInitPosition();
		} else
			// something else
			return;
	}

	public static void Register8Balls(BilliardBall_Physics eightBall)
	{
		_eightBalls.Add (eightBall);
	}

	public static void RegisterWhiteBall(BilliardBall_Physics whiteBall)
	{
		_whiteBall = whiteBall;
	}
}
