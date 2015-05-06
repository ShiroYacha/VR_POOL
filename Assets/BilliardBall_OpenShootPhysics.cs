using UnityEngine;
using System.Collections;

public class BilliardBall_OpenShootPhysics : BilliardBall_Physics {
	
	float maxForce = 5.0f;
	float minForce = 3.0f;

	protected override void Start()
	{
		rigidBody = GetComponent<Rigidbody> ();
		// Register identity
		GameSystem_8Ball.RegisterWhiteBall (this);
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
		// Shoot again
		if (GameSystem_8Ball.Stabilized) {
			// wake object up
			rigidBody.WakeUp();
			isSleeping = false;
			// Apply the force
			// ResetInitPosition();
			ApplyRandomForce ();
		}
	}
	
	void ApplyRandomForce()
	{
		float force = (maxForce - minForce) * Random.value + minForce;
		rigidBody.AddForce (new Vector3(-1.0f,0.0f,0.1f*(Random.value*2.0f-1.0f))*force, ForceMode.Impulse);
	}
}
