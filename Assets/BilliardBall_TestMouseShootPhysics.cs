using UnityEngine;
using System.Collections;

public class BilliardBall_TestMouseShootPhysics : BilliardBall_Physics
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
		if(Input.GetKeyDown("1"))
		{
			// Move white ball to hole left top hole
			rigidBody.position = new Vector3(1.314f,0.051f,-0.598f);
		}else if(Input.GetKeyDown("2"))
		{
			// Move white ball to hole center top hole
			rigidBody.position = new Vector3(-0.01f,0.051f,-0.588f);
		}else if(Input.GetKeyDown("3"))
		{
			// Move white ball to hole right top hole
			rigidBody.position = new Vector3(-1.317f,0.051f,-0.598f);
		}else if(Input.GetKeyDown("4"))
		{
			// Move white ball to hole right bottom hole
			rigidBody.position = new Vector3(-1.317f,0.051f,0.623f);
		}else if(Input.GetKeyDown("5"))
		{
			// Move white ball to hole center bottom hole
			rigidBody.position = new Vector3(-0.001f,0.051f,0.584f);
		}else if(Input.GetKeyDown("6"))
		{
			// Move white ball to hole left bottom hole
			rigidBody.position = new Vector3(1.31f,0.051f,0.623f);
		}
	}



}
