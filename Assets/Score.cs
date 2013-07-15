using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
	public int score = 0;
	private int nbDigits = 5;
	private GUIStyle style = null;
	public Rect startRect = new Rect( 10, 10, 75, 50 ); // The rect the window is initially displayed at.
 
	void Awake() {
		Application.targetFrameRate = -1;
	}
	
	void Start()
	{
	}
 
	void Update()
	{
	}
 
 
	void OnGUI()
	{
		// Copy the default label skin, change the color and the alignement
		if( style == null ){
			style = new GUIStyle( GUI.skin.label );
			style.normal.textColor = Color.white;
			style.alignment = TextAnchor.MiddleCenter;
		}
 
		GUI.color = Color.white;
		startRect = GUI.Window(0, startRect, DoMyWindow, "");
	}
 
	void DoMyWindow(int windowID)
	{
		string strScore = "" + score;
		while( strScore.Length < nbDigits )
			strScore = "0" + strScore;
		GUI.Label( new Rect(0, 0, startRect.width, startRect.height), strScore, style );
	}
}