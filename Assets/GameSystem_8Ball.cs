using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public enum BallColor
{
	TBD,
	Full,
	Half
}

public class GameSystem_8Ball : MonoBehaviour
{
	static List<BilliardBall_Physics> _eightBalls = new List<BilliardBall_Physics> ();
	static BilliardBall_WhiteBallPhysics _whiteBall;
	static BilliardCue_Control _cue;
	static GameSystem_8Ball _table;
	public static Camera _mainCamera;
	public static Camera _cueCamera;
	static UDPReceive _receiver;
	static GameObject _quad;

	public static bool isMainCameraInUse;
	static bool isPlayer1sTurn;
	static List<string> player1sBallInHole = new List<string> ();
	static List<string> player2sBallInHole = new List<string> ();
	static List<BilliardBall_Physics> tempBallInHole = new List<BilliardBall_Physics> ();
	static BallColor player1Color;
	static BallColor player2Color;
	GUIStyle activeStyle;
	GUIStyle passiveStyle;

	public static bool RoundFinished {
		get;
		set;
	}

	public static bool Stabilized {
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
		// Initialize
		_mainCamera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera> ();
		_cueCamera = GameObject.FindWithTag ("CueCamera").GetComponent<Camera> ();
		_quad = GameObject.FindWithTag ("Quad");
		_mainCamera.enabled = true;
		_cueCamera.enabled = false;
		isMainCameraInUse = true;
		// Initialize game rule
		isPlayer1sTurn = true;
		RoundFinished = true;
		// Initialize GUI
		player1Color = BallColor.TBD;
		player2Color = BallColor.TBD;
		activeStyle = new GUIStyle ();
		activeStyle.normal.textColor = Color.red;
		passiveStyle = new GUIStyle ();
		passiveStyle.normal.textColor = Color.gray;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("c")) {
			_mainCamera.enabled = !_mainCamera.enabled;
			_cueCamera.enabled = !_cueCamera.enabled;
			// store camera state
			isMainCameraInUse = _mainCamera.enabled;
		} else if(Input.GetKeyDown("f")){
			_receiver.Activated = !_receiver.Activated;
		}
		// Change the viewport if face is detected
		UpdateFaceDistance ();
	}

	void UpdateFaceDistance ()
	{	// camera position (close to far): 2.6 - 1.9 - 1.3
		if (_receiver != null) {
			string raw = _receiver.lastReceivedUDPPacket;
			float dist;
			if (raw != null && raw != "" && float.TryParse (raw.Split (' ') [2], out dist)) {
				float normDist = (dist - 0.2f) / 0.3f;
				Vector3 pos = _mainCamera.transform.position;
				float y =2.6f - normDist * 1.3f;
				if(y>1.3 && y<2.6)
					_mainCamera.transform.position = new Vector3 (pos.x,y , pos.z);
			}
		}
	}

	void OnGUI ()
	{
		// Get player's ball list
		string player1List = " - Scored: ";
		foreach (var ball in player1sBallInHole)
			player1List += " " + ball;
		string player2List = " - Scored: ";
		foreach (var ball in player2sBallInHole)
			player2List += " " + ball;
		string faceTracking = _receiver!=null && _receiver.Activated ? "ON" : "OFF";
		// Update GUI
		GUI.Label (new Rect (15, 30, 100, 100), "Player 1 = " + player1Color + player1List, isPlayer1sTurn ? activeStyle : passiveStyle);
		GUI.Label (new Rect (15, 45, 100, 100), "Player 2 = " + player2Color + player2List, !isPlayer1sTurn ? activeStyle : passiveStyle);
		GUI.Label (new Rect (15, 60, 200, 100), "Face tracking = " + faceTracking);
		GUI.Label (new Rect (15, 75, 200, 100), "Press 'R' to restart game.");
	}
	
	public static void RestoreCamera ()
	{
		_mainCamera.enabled = isMainCameraInUse;
		_cueCamera.enabled = !isMainCameraInUse;
	}

	public static void TemporarySwitchToMainCamera ()
	{
		_mainCamera.enabled = true;
		_cueCamera.enabled = false;
	}

	public static void TemporarySwitchToCueCamera ()
	{
		_mainCamera.enabled = false;
		_cueCamera.enabled = true;
	}
	
	public static void RestartGame ()
	{
		_whiteBall.ResetInitPosition ();
		_eightBalls.ForEach (b => b.ResetInitPosition ());
	}

	public static void UpdateGameStatus ()
	{
		if (tempBallInHole.Any ()) {
			var currentPlayerList = isPlayer1sTurn ? player1sBallInHole : player2sBallInHole;
			bool whiteBallFalls = tempBallInHole.Any (b => b.gameObject.name == "WhiteBall");
			// if the 8 ball is in the hole
			if (tempBallInHole.Any (b => b.gameObject.name == "8")) {
				// Reset game
				RestartGame ();
				// Display winner
				string winningPlayer = string.Format ("Player {0} wins!",
				                                     (!whiteBallFalls && isPlayer1sTurn == (currentPlayerList.Count == 7)) || (whiteBallFalls && !isPlayer1sTurn) ?
				                                     "1" : "2");
				EditorUtility.DisplayDialog ("Congrats!", winningPlayer, "Ok");
			}
			// if white ball falls when 8 ball is okay
			else if (whiteBallFalls) {
				// Reset everything and switch player
				tempBallInHole.ForEach (b => b.ResetInitPosition ());
				isPlayer1sTurn = !isPlayer1sTurn;
			} else {
				int firstBallNumber = Int32.Parse (tempBallInHole.First ().gameObject.name);
				// if it's the first ball in hole
				if (player1Color == BallColor.TBD || player2Color == BallColor.TBD) {
					// Set the color of both players
					bool player1PlaysFull = isPlayer1sTurn == (firstBallNumber < 8);
					player1Color = player1PlaysFull ? BallColor.Full : BallColor.Half;
					player2Color = !player1PlaysFull ? BallColor.Full : BallColor.Half;
				}
				// if the first ball is the opponent's 
				else if (isPlayer1sTurn != (player1Color == BallColor.Full == firstBallNumber < 8)) {
					isPlayer1sTurn = !isPlayer1sTurn;
				}

				// Update player's ball list
				foreach (var ball in tempBallInHole) {
					if (player1Color == BallColor.Full == Int32.Parse (ball.gameObject.name) < 8)
						player1sBallInHole.Add (ball.gameObject.name);
					else 
						player2sBallInHole.Add (ball.gameObject.name);
				}

			}
			// Reset
			tempBallInHole.Clear ();
		} else if (_whiteBall.WasHitByCue) {// switch player if nothing happens and the white ball is hit
			_whiteBall.WasHitByCue = false;
			isPlayer1sTurn = !isPlayer1sTurn;
		}
		// Restore ball highlighting
		foreach (var ball in _eightBalls)
			ball.RestoreAlpha ();
	}

	public static void BallInHole (BilliardBall_Physics objScript)
	{
		// Modify game status
		int number;
		if (objScript.gameObject.name == "WhiteBall") {
			// White ball falls
			tempBallInHole.Add (objScript);
			// Sleep ball
			objScript.Sleep ();
			objScript.gameObject.SetActive (false);
		} else if (int.TryParse (objScript.gameObject.name, out number)) {
			// 8-Ball falls
			tempBallInHole.Add (objScript);
			// Sleep ball
			objScript.Sleep ();
			objScript.gameObject.SetActive (false);
		} else
			// something else
			return;
	}

	public static void ActivateCue (float degree)
	{
		// Move it to the white ball at the direction of the closest ball to hit
		_cue.Activate (_whiteBall.Position, degree);
		// Update camera
		_cueCamera.transform.position = new Vector3 (_whiteBall.Position.x, 0.15f, _whiteBall.Position.z) + _cue.TempOffset * 1.5f;
		_cueCamera.transform.rotation = Quaternion.Euler (new Vector3 (15.0f, _cue.TempRotationOffset == 0.0f ? -90.0f : _cue.TempRotationOffset + 90.0f, 0));
		// Highlight target balls (inverse other)
		foreach (var ball in _eightBalls)
			ball.LerpAlpha (isPlayer1sTurn ? player1Color : player2Color);
	}
	
	public static void Register8Balls (BilliardBall_Physics eightBall)
	{
		_eightBalls.Add (eightBall);
		// Ignore collision if cue is loaded already
		if (_cue != null)
			Physics.IgnoreCollision (_cue.gameObject.GetComponent<Collider> (), eightBall.gameObject.GetComponent<Collider> ());
	}

	public static void RegisterWhiteBall (BilliardBall_WhiteBallPhysics whiteBall)
	{
		_whiteBall = whiteBall;
	}

	public static void RegisterReceiver(UDPReceive receiver)
	{
		_receiver = receiver;
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
