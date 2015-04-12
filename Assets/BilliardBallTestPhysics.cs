using UnityEngine;
using System.Collections;

public class BilliardBallTestPhysics : MonoBehaviour {
	
	Vector3 initForce = new Vector3(1.0f,0.0f,1.0f)*5;
	Rigidbody rigidBody;

	// Use this for initialization
	void Start () {

		rigidBody = GetComponent<Rigidbody> ();
		rigidBody.AddForce (initForce,ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		if (rigidBody.velocity.magnitude > 0.05f) {
			rigidBody.AddForce (rigidBody.velocity * (-1) * 0.05f, ForceMode.Impulse);
		} else {
			rigidBody.Sleep();
		}
	}

}
