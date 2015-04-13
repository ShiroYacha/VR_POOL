using UnityEngine;
using System.Collections;

public class BilliardBallTestPhysics : MonoBehaviour
{

	Vector3 initPosition = new Vector3 (0.0f, 0.055f, 0.0f);
	float friction = 0.05f;
	float maxForce = 40.0f;
	float minForce = 20.0f;

	Rigidbody rigidBody;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody> ();
		ResetBall ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (rigidBody.velocity.magnitude > friction) {
			rigidBody.AddForce (rigidBody.velocity * (-1) * friction, ForceMode.Impulse);
		} else {
			ResetBall();
		}
	}

	void ApplyRandomForce()
	{
		float force = (maxForce - minForce) * Random.value + minForce;
		rigidBody.AddForce (new Vector3(Random.value,0.0f,Random.value)*force, ForceMode.Impulse);
	}

	void ResetBall()
	{
		this.transform.position = initPosition;
		ApplyRandomForce ();
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name.StartsWith ("Hole")) {
			ResetBall();
		}
	}

}
