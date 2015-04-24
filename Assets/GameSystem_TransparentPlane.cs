using UnityEngine;
using System.Collections;

public class GameSystem_TransparentPlane : MonoBehaviour {

	// use to pass the mouse up event to the main table
	void OnMouseUp()
	{
		GameSystem_8Ball.MouseUpEventHandler ();
	}
}
 