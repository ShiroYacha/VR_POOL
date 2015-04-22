using UnityEngine;
using System;
using System.Collections;
using System.Resources;

public class BilliardCue_Control : MonoBehaviour
{

	Vector3 HIDDEN_POSITION = new Vector3 (999.0f, 0.1f, 999.0f);
	float STRENGH_PER_FRAME = 50.0f;
	float MAX_STRENGH = 30.0f;
	float STRENGH_RELEASE_COEF = 3.0f;
	float TOUCH_OFFSET = 0.2f;

	Rigidbody rigidBody;
	float tempRotation = 0.0f;
	float tempStrengh = 0.0f;
	Vector3 tempOffset;
	bool onReleasing = false;

	public bool OnReleasing {
		get{ return onReleasing;}
		set{ onReleasing = value;}
	}

	// Use this for initialization
	void Start ()
	{
		// Setups 
		rigidBody = GetComponent<Rigidbody> ();
		var collider = GetComponent<Collider> ();
		tempOffset = new Vector3 (TOUCH_OFFSET, 0, 0);
		// Register identity
		GameSystem_8Ball.RegisterCue (this);
		// Hide the cue
		rigidBody.position = HIDDEN_POSITION;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (0) && tempStrengh < MAX_STRENGH) {
			tempStrengh += Time.deltaTime * STRENGH_PER_FRAME;
		} else if (onReleasing && tempStrengh > 0) {
			rigidBody.velocity = -tempOffset * tempStrengh;
			tempStrengh -= Time.deltaTime * STRENGH_PER_FRAME * STRENGH_RELEASE_COEF;
		} else if (tempStrengh < 0.0f) {
			tempStrengh = 0.0f;
			onReleasing = false;
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
			Desactivate ();
		} else if (GameSystem_8Ball.Stabalized) {
			float wheel = Input.GetAxis ("Mouse ScrollWheel");
			GameSystem_8Ball.ActivateCue (wheel*0.5f);

		}
	}

	public bool Hidden {
		get {
			return (rigidBody.position - HIDDEN_POSITION).sqrMagnitude < 0.1f;
		}
	}

	void OnGUI ()
	{
		GUI.Label (new Rect (15, 10, 100, 100), "Strengh = " + tempStrengh.ToString ("0.00"));
	}

	public void Desactivate ()
	{
		rigidBody.position = HIDDEN_POSITION;
	}

	public void Activate (Vector3 position, float wheel)
	{

		// Move it to the centor of the position with the offset of the length of the 
		tempRotation += wheel;
		tempOffset = new Vector3 ((float)(TOUCH_OFFSET * Math.Cos (tempRotation)), 0.0f, (float)(TOUCH_OFFSET * Math.Sin (tempRotation)));
		rigidBody.position = position + tempOffset;
		if (wheel != 0) {
			tempOffset.y = 0.0f;
			Vector3 reference = new Vector3 (0, 0, 1.0f);
			float sign = Mathf.Sign (Vector3.Dot (tempOffset.normalized, reference));
			float angle = Vector3.Angle (new Vector3 (-1.0f, 0, 0), tempOffset.normalized) * sign;
			rigidBody.MoveRotation (Quaternion.Euler (new Vector3 (90.9f, 0, -angle + 90.0f))); 
		}
	}
}
