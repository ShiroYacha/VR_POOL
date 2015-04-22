using UnityEngine;
using System.Collections;

public class BilliardBall_Physics : MonoBehaviour
{
	const float MAX_TABLE_RANGE = 5.0f;

	protected Vector3 initPosition;
	protected Rigidbody rigidBody;

	protected float friction = 0.015f;
	protected float sleepThreshold = 0.5f;
	
	protected bool isSleeping = true;

	public bool IsSleeping 
	{
		get
		{
			return isSleeping;
		}
	}

	#region Events
	// Use this for initialization
	protected virtual void Start ()
	{
		rigidBody = GetComponent<Rigidbody> ();
		// Register the init position of the ball
		initPosition= rigidBody.position;
		// Register identity
		GameSystem_8Ball.Register8Balls (this);
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		// Apply friction and sleeping rules
		if (rigidBody.velocity.magnitude > friction) {
			rigidBody.AddForce (rigidBody.velocity * (-1) * friction, ForceMode.Impulse);
		} else if(!isSleeping && rigidBody.velocity.magnitude<sleepThreshold) {
			Sleep();
		}
		// Make sure balls don't jump
		var tempVelocity = rigidBody.velocity;
		rigidBody.velocity = new Vector3(tempVelocity.x,0,tempVelocity.z);
		// Additionally check if ball falls off table
		if((rigidBody.position - Vector3.zero).magnitude>MAX_TABLE_RANGE)
		{
			if(gameObject.name=="WhiteBall")
				GameSystem_8Ball.BallInHole(this);
			ResetInitPosition();
			Sleep();
		}
	}

	public Vector3 Position
	{
		get{
			return rigidBody.position;
		}
	}

	public void Sleep()
	{
		rigidBody.angularVelocity=Vector3.zero;
		rigidBody.velocity = Vector3.zero;
		rigidBody.Sleep();
		isSleeping=true;
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name.StartsWith ("Hole")) {
			// signal the system
			GameSystem_8Ball.BallInHole(this);
		} else if(col.gameObject.name!="TableMain" && gameObject.activeSelf){
			// become active when collide with another object
			rigidBody.WakeUp();
			isSleeping=false;
		}
	}
	#endregion

	#region Public methods
	public void ResetInitPosition()
	{
		// reset velocity
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		// reset position 
		rigidBody.position = initPosition;
		// reset activeness
		if (!gameObject.activeSelf)
			gameObject.SetActive (true);
	}
	#endregion

}
