using UnityEngine;
using System.Collections;
using Leap;

public class LeapCommandBimanual : MonoBehaviour {

	private float strength = 0.0f;
	private int handCount = 0;
	private Vector leftHand_position;
	private Vector rightHand_position;
	private bool valid_gesture;
	private bool both_open;
	private bool both_close;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		HandController controller = gameObject.GetComponent<HandController> ();
		Frame frame = controller.GetFrame(); // controller is a Controller object
		HandList hands = frame.Hands;
		handCount = hands.Count;
		valid_gesture=false;
		if (handCount == 2) {
			Hand hand1 = hands [0];
			Hand hand2 = hands [1];
			if(hand1.IsLeft == hand2.IsLeft) return;
			leftHand_position = hand1.IsLeft?hand1.PalmPosition:hand2.PalmPosition;
			rightHand_position = hand2.IsRight?hand2.PalmPosition:hand1.PalmPosition;
			both_open = hand1.GrabStrength==0 && hand2.GrabStrength==0;
			both_close = hand1.GrabStrength>0 && hand2.GrabStrength>0;
			valid_gesture = true;
		}
	}

	public bool getValidGesture(){
		return valid_gesture;
	}

	public Vector getLeftHandPosition(){
		return leftHand_position;
	}

	public Vector getRightHandPosition(){
		return rightHand_position;
	}


	public int getHandCount(){
		return handCount;
	}

	public bool getBothOpen(){
		return both_open;
	}

	public bool getBothClose(){
		return both_close;
	}

	void OnGUI()
	{

	}
}
