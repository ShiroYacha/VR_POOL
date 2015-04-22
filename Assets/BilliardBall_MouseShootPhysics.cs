using UnityEngine;
using System.Collections;

public class BilliardBall_MouseShootPhysics : BilliardBall_Physics
{
	protected override void Start()
	{
		rigidBody = GetComponent<Rigidbody> ();
		// Register the init position of the ball
		initPosition= rigidBody.position;
		// Register identity
		GameSystem_8Ball.RegisterWhiteBall (this);
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
}
