using UnityEngine;
using System.Collections;

public class BilliardBall_Physics : MonoBehaviour
{
	protected Vector3 initPosition;
	protected Rigidbody rigidBody;

	protected float friction = 0.02f;
	protected float sleepThreshold = 0.3f;
	
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
			rigidBody.angularVelocity=Vector3.zero;
			rigidBody.velocity = Vector3.zero;
			rigidBody.Sleep();
			isSleeping=true;
		}
	}

	public Vector3 Position
	{
		get{
			return rigidBody.position;
		}
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
		if (name == "WhiteBall")
			rigidBody.position = initPosition;
		rigidBody.position = initPosition;
		// reset activeness
		if (!gameObject.activeSelf)
			gameObject.SetActive (true);
	}
	#endregion

}
