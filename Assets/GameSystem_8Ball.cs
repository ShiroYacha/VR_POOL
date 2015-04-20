﻿using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameSystem_8Ball : MonoBehaviour
{

	static List<BilliardBall_Physics> _eightBalls = new List<BilliardBall_Physics> ();
	static BilliardBall_Physics _whiteBall;
	static BilliardCue_Control _cue;
	static GameSystem_8Ball _table;

	public static bool Stabalized 
	{
		get 
		{
			return _whiteBall.IsSleeping && _eightBalls.TrueForAll (b => b.IsSleeping) && _cue.Hidden;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		_table = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static void BallInHole (BilliardBall_Physics objScript)
	{
		// Modify game status
		int number;
		if (objScript.gameObject.name == "WhiteBall") {
			// White ball falls
			objScript.ResetInitPosition ();
		} else if (int.TryParse (objScript.gameObject.name, out number)) {
			// 8-Ball falls
			objScript.ResetInitPosition ();
		} else
			// something else
			return;
	}

	public static void ActivateCue()
	{
		// Move it to the white ball at the direction of the closest ball to hit
		_cue.Activate (_whiteBall.Position,Vector3.zero);
	}

	public static void Register8Balls (BilliardBall_Physics eightBall)
	{
		_eightBalls.Add (eightBall);
		// Ignore collision if cue is loaded already
		if (_cue != null)
			Physics.IgnoreCollision (_cue.gameObject.GetComponent<Collider> (), eightBall.gameObject.GetComponent<Collider> ());
	}

	public static void RegisterWhiteBall (BilliardBall_Physics whiteBall)
	{
		_whiteBall = whiteBall;
	}

	public static void RegisterCue (BilliardCue_Control cue)
	{
		_cue = cue;
		// Ignore collision if the 8 balls are loaded already
		if (_eightBalls != null)
			foreach (var eightBall in _eightBalls) {
				Physics.IgnoreCollision (cue.gameObject.GetComponent<Collider> (), eightBall.gameObject.GetComponent<Collider> ());
			}
		// Ignore collision with the table
		Physics.IgnoreCollision (cue.gameObject.GetComponent<Collider> (),_table.gameObject.GetComponent<Collider>());
		// Hide it
		cue.Desactivate ();
	}
}