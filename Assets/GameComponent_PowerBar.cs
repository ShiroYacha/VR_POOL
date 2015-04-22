using UnityEngine;
using System.Collections;

public class GameComponent_PowerBar : MonoBehaviour {
	public static float Percentage {get;set;}

	Vector2 pos = new Vector2(15,10);
	Vector2 size = new Vector2(500,15);
	private static Texture2D _staticRectTexture;
	private static GUIStyle _staticRectStyle;

	// Use this for initialization
	void OnGUI () 
	{
		if(Percentage<0.0f)
			Percentage = 0.0f;

		// draw the background
		GUIDrawRect(new Rect(pos.x,pos.y,size.x,size.y),Color.white);

		// draw the foreground
		GUIDrawRect(new Rect(pos.x,pos.y,size.x*Percentage,size.y),Color.red);
	}
		
	// Note that this function is only meant to be called from OnGUI() functions.
	public static void GUIDrawRect( Rect position, Color color )
	{
		if( _staticRectTexture == null )
		{
			_staticRectTexture = new Texture2D( 1, 1 );
		}
		
		if( _staticRectStyle == null )
		{
			_staticRectStyle = new GUIStyle();
		}
		
		_staticRectTexture.SetPixel( 0, 0, color );
		_staticRectTexture.Apply();
		
		_staticRectStyle.normal.background = _staticRectTexture;
		
		GUI.Box( position, GUIContent.none, _staticRectStyle );
		
		
	}
	
}
