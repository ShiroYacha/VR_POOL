using UnityEngine;
using System;
using System.Collections;

public class BilliardCue_Control : MonoBehaviour
{

	Vector3 HIDDEN_POSITION = new Vector3 (999.0f, 0.1f, 999.0f);
	Rigidbody rigidBody;
	Vector3 offset;

	// Use this for initialization
	void Start ()
	{
		// Setups 
		rigidBody = GetComponent<Rigidbody> ();
		var collider = GetComponent<Collider> ();
		offset = new Vector3 (0.3f, 0.0f, 0.0f);
		// Register identity
		GameSystem_8Ball.RegisterCue (this);
		// Hide the cue
		rigidBody.position = HIDDEN_POSITION;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// Activate the cue
		if (GameSystem_8Ball.Stabalized) 
		{
			GameSystem_8Ball.ActivateCue ();
		}
	}

	public bool Hidden 
	{
		get 
		{
			return (rigidBody.position-HIDDEN_POSITION).sqrMagnitude<0.1f;
		}
	}

	void OnMouseDown ()
	{
//		if (Input.GetMouseButtonDown (0)) 
//		{
//			// Left button controls the angle
//			rigidBody.angularVelocity = new Vector3 (-1.0f, 0, 0);
//			rigidBody.velocity = Vector3.zero;
//		}
//		 if(Input.GetMouseButtonDown (1)) 
		{
			// Right button controls the strength
			rigidBody.velocity = new Vector3 (-1.0f, 0, 0) * 5.0f;
			rigidBody.angularVelocity = Vector3.zero;
		}
	}
	
	void OnMouseUp ()
	{
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		Desactivate ();
	}

	public void Desactivate ()
	{
		rigidBody.position = HIDDEN_POSITION;
	}

	public void Activate (Vector3 position, Vector3 orientation)
	{

		// Move it to the centor of the position with the offset of the length of the 
		rigidBody.position = position + offset;
		
	}
}
