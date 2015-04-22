using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public enum BallColor{TBD,Full,Half}

public class GameSystem_8Ball : MonoBehaviour
{
	static List<BilliardBall_Physics> _eightBalls = new List<BilliardBall_Physics> ();
	static BilliardBall_Physics _whiteBall;
	static BilliardCue_Control _cue;
	static GameSystem_8Ball _table;

	static bool isPlayer1sTurn;
	static List<string> player1sBallInHole = new List<string>();
	static List<string> player2sBallInHole = new List<string>();
	static List<BilliardBall_Physics> tempBallInHole = new List<BilliardBall_Physics>();
	static BallColor player1Color;
	static BallColor player2Color;

	GUIStyle activeStyle;
	GUIStyle passiveStyle;

	public static bool RoundFinished
	{
		get;set;
	}

	public static bool Stabalized {
		get {
			if (_whiteBall == null || !_eightBalls.Any () || _cue == null)
				return false;
			return _whiteBall.IsSleeping && _eightBalls.TrueForAll (b => b.IsSleeping || !b.gameObject.activeSelf);
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		_table = this;
		// Initialize game rule
		isPlayer1sTurn = true;
		RoundFinished = true;
		// Initialize GUI
		player1Color = BallColor.TBD;
		player2Color = BallColor.TBD;
		activeStyle = new GUIStyle();
		activeStyle.normal.textColor = Color.red;
		passiveStyle = new GUIStyle();
		passiveStyle.normal.textColor = Color.gray;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnGUI ()
	{
		// Get player's ball list
		string player1List = " - Scored: ";
		foreach(var ball in player1sBallInHole)
			player1List+=" "+ball;
		string player2List = " - Scored: ";
		foreach(var ball in player2sBallInHole)
			player2List+=" "+ball;
		// Update GUI
		GUI.Label (new Rect (15, 30, 100, 100), "Player 1 = "+player1Color + player1List,isPlayer1sTurn?activeStyle:passiveStyle );
		GUI.Label (new Rect (15, 45, 100, 100), "Player 2 = "+player2Color+player2List,!isPlayer1sTurn?activeStyle:passiveStyle );
	}
	
	void OnMouseUp ()
	{
		if(!_cue.OnReleasing)
		{
			RoundFinished = false;
			_cue.OnReleasing = true;
		}
	}

	public static void UpdateGameStatus()
	{
		if(tempBallInHole.Any())
		{
			var currentPlayerList = isPlayer1sTurn?player1sBallInHole:player2sBallInHole;
			// if white ball falls
			if(tempBallInHole.Any(b=>b.gameObject.name=="WhiteBall"))
			{
				// Reset everything and switch player
				tempBallInHole.ForEach(b=>b.ResetInitPosition());
				isPlayer1sTurn=!isPlayer1sTurn;
			}
			// if the 8 ball is in the hole
			else if(tempBallInHole.Any(b=>b.gameObject.name=="8"))
			{
				// Reset game
				_whiteBall.ResetInitPosition();
				_eightBalls.ForEach(b=>b.ResetInitPosition());
				// Display winner
				string winningPlayer = string.Format("Player {0} wins!",isPlayer1sTurn==(currentPlayerList.Count==7)?"1":"2");
				EditorUtility.DisplayDialog("Congrats!",winningPlayer,"Ok");
			}
			else
			{
				int firstBallNumber = Int32.Parse(tempBallInHole.First().gameObject.name);
				// if it's the first ball in hole
				if(player1Color==BallColor.TBD|| player2Color==BallColor.TBD)
				{
					// Set the color of both players
					bool player1PlaysFull = isPlayer1sTurn==(firstBallNumber<8);
					player1Color = player1PlaysFull?BallColor.Full:BallColor.Half;
					player2Color = !player1PlaysFull?BallColor.Full:BallColor.Half;
				}
				// Update player's ball list
				foreach(var ball in tempBallInHole)
				{
					if(player1Color==BallColor.Full&&Int32.Parse(ball.gameObject.name)<8)
						player1sBallInHole.Add(ball.gameObject.name);
					else 
						player2sBallInHole.Add(ball.gameObject.name);
				}
			}
			// Reset
			tempBallInHole.Clear();
		}
		else // switch player if nothing happens
			isPlayer1sTurn=!isPlayer1sTurn;
	}

	public static void BallInHole (BilliardBall_Physics objScript)
	{
		// Modify game status
		int number;
		if (objScript.gameObject.name == "WhiteBall") {
			// White ball falls
			tempBallInHole.Add(objScript);
			// Sleep ball
			objScript.gameObject.SetActive(false);
		} else if (int.TryParse (objScript.gameObject.name, out number)) {
			// 8-Ball falls
			tempBallInHole.Add(objScript);
			objScript.gameObject.SetActive(false);
		} else
			// something else
			return;
	}

	public static void ActivateCue (float wheel)
	{
		// Move it to the white ball at the direction of the closest ball to hit
		_cue.Activate (_whiteBall.Position, wheel);
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
		Collider cueCollider = cue.gameObject.GetComponent<Collider> ();
		if (_eightBalls != null)
			foreach (var eightBall in _eightBalls) {
			Physics.IgnoreCollision (cueCollider, eightBall.gameObject.GetComponent<Collider> ());
			}
		// Ignore collision with the table and its children
		Physics.IgnoreCollision (cueCollider, _table.gameObject.GetComponent<Collider> ());
		foreach (var collider in _table.gameObject.GetComponentsInChildren<Collider>()) {
			Physics.IgnoreCollision (cueCollider, collider);
		}
		// Hide it
		cue.Desactivate ();
	}
}
