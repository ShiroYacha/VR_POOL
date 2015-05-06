using UnityEngine;
using System.Collections;

public class BilliardBall_WhiteBallPhysics : BilliardBall_Physics
{
	public bool WasHitByCue {get;set;}

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

	protected override void OnCollisionEnter (Collision col)
	{
		base.OnCollisionEnter(col);
		if(col.gameObject.name=="Cue")
		{
			WasHitByCue = true;
		}
	}
}
