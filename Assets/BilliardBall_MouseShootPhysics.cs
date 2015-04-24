using UnityEngine;
using System.Collections;

public class BilliardBall_MouseShootPhysics : BilliardBall_Physics
{
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
		// restart game if demanded
		if(Input.GetKeyDown("r"))
			GameSystem_8Ball.RestartGame();
	}



}
