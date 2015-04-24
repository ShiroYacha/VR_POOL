using UnityEngine;
using System;
using System.Collections;
using System.Resources;

public class BilliardCue_Control : MonoBehaviour
{
	public static event Action OnRotateCue;
	public static event Func<bool> OnShootingCue;
	public static event Func<bool> OnReleaseCue;

	Vector3 HIDDEN_POSITION = new Vector3 (999.0f, 0.1f, 999.0f);
	float STRENGH_PER_FRAME = 50.0f;
	float MAX_STRENGH = 30.0f;
	float STRENGH_RELEASE_COEF = 3.0f;
	float TOUCH_OFFSET = 0.2f;
	Rigidbody rigidBody;
	float tempRotation = 0.0f;
	float tempStrengh = 0.0f;
	bool onReleasing = false;
	Vector3 tempOffset;
	float tempRotationOffset;

	public bool OnReleasing {
		get{ return onReleasing;}
		set{ onReleasing = value;}
	}

	public Vector3 TempOffset{ get{return tempOffset;}}
	public float TempRotationOffset{ get{return tempRotationOffset;}}

	// Use this for initialization
	void Start ()
	{
		// Setups 
		rigidBody = GetComponent<Rigidbody> ();
		var collider = GetComponent<Collider> ();
		tempOffset = new Vector3 (TOUCH_OFFSET, 0, 0);
		tempRotationOffset = 0.0f;
		// Register identity
		GameSystem_8Ball.RegisterCue (this);
		// Hide the cue
		rigidBody.position = HIDDEN_POSITION;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameSystem_8Ball.Stabalized) {
			if (!onReleasing) {
				OnRotateCue();
			}
			if (!GameSystem_8Ball.RoundFinished && tempStrengh == 0.0f) {
				GameSystem_8Ball.UpdateGameStatus ();
				GameSystem_8Ball.RoundFinished = true;
				GameSystem_8Ball.RestoreCamera();
			}
			else if(OnReleaseCue())
			{
				if (!OnReleasing) {
					GameSystem_8Ball.RoundFinished = false;
					OnReleasing = true;
					// switch to main camera
					GameSystem_8Ball.TemporarySwitchToMainCamera();
				}
			}
		}
		if (!Hidden) {
			if (tempStrengh < MAX_STRENGH && OnShootingCue()) {
				tempStrengh += STRENGH_PER_FRAME*Time.deltaTime;
			} else if (onReleasing && tempStrengh > 0) {
				rigidBody.velocity = -tempOffset * tempStrengh;
				tempStrengh -= Time.deltaTime * STRENGH_PER_FRAME * STRENGH_RELEASE_COEF;
			}
			GameComponent_PowerBar.Percentage = tempStrengh/MAX_STRENGH;
		}
		if (tempStrengh <= 0.0f && onReleasing) {
			// Disactivate the cue
			Desactivate ();
		} 
	
	}

	public bool Hidden {
		get {
			return (rigidBody.position - HIDDEN_POSITION).sqrMagnitude < 0.1f;
		}
	}
	
	public void Desactivate ()
	{
		// Reset
		tempStrengh = 0.0f;
		onReleasing = false;
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		// Hide the ball
		rigidBody.position = HIDDEN_POSITION;
	}

	/// <summary>
	/// Puts the cue at the position with an offset, and rotate it with a degree (-1 to 1, 0 means don't rotate).
	/// </summary>
	/// <param name="position">Position.</param>
	/// <param name="degree">Degree (between -1 and 1).</param>
	public void Activate (Vector3 position, float degree)
	{

		// Move it to the centor of the position with the offset of the length of the 
		tempRotation += degree;
		tempOffset = new Vector3 ((float)(TOUCH_OFFSET * Math.Cos (tempRotation)), 0.0f, (float)(TOUCH_OFFSET * Math.Sin (tempRotation)));
		rigidBody.position = position + tempOffset;
		if (degree != 0) {
			tempOffset.y = 0.0f;
			Vector3 reference = new Vector3 (0, 0, 1.0f);
			float sign = Mathf.Sign (Vector3.Dot (tempOffset.normalized, reference));
			float angle = Vector3.Angle (new Vector3 (-1.0f, 0, 0), tempOffset.normalized) * sign;
			tempRotationOffset = angle;
			rigidBody.MoveRotation (Quaternion.Euler (new Vector3 (90.9f, 0, -angle + 90.0f))); 
		}
	}
}
