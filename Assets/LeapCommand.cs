using UnityEngine;
using System.Collections;
using Leap;

public class LeapCommand : MonoBehaviour {

	private float strength = 0.0f;
	private int handCount = 0;
	private Vector hand_position;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		HandController controller = gameObject.GetComponent<HandController> ();
		Frame frame = controller.GetFrame(); // controller is a Controller object
		HandList hands = frame.Hands;
		handCount = hands.Count;
		if (handCount != 0) {
			Hand hand = hands [0];
			strength = hand.GrabStrength;
			hand_position = hand.PalmPosition;
		}
	}

	public Vector getPosition(){
		return hand_position;
	}

	public int getHandCount(){
		return handCount;
	}

	public float getStrength(){
		return strength;
	}

	void OnGUI()
	{
		GUI.Label (new Rect (0, 0, 100, 100), strength.ToString());
		GUI.Label (new Rect (0, 100, 100, 100), hand_position.ToString());
	}
}
