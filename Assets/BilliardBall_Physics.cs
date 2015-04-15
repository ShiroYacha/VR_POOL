using UnityEngine;
using System.Collections;

public class BilliardBall_Physics : MonoBehaviour
{
	Rigidbody rigidBody;
	float friction = 0.05f;
	bool isSleeping = true;

	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (rigidBody.velocity.magnitude > friction) {
			rigidBody.AddForce (rigidBody.velocity * (-1) * friction, ForceMode.Impulse);
		} else if(!isSleeping) {
			rigidBody.Sleep();
			isSleeping=true;
		}
	}
	
	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name.StartsWith ("Hole")) {
			Destroy (gameObject);
		} else if(col.gameObject.name!="TableMain"){
			rigidBody.WakeUp();
			isSleeping=false;
		}
		
	}

}
