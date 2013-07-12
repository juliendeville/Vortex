using UnityEngine;
using System.Collections;

public class DrawPath : MonoBehaviour {
	public GameObject Player;
	
	private LineRenderer lineRenderer;
	public Vector3[] myPoints = null;
	public bool endPoint = false;
	public float refreshTouch = 0.2f;
	public int idTouch = -1;
	private Vector3 lastPoint = Vector3.zero;
	private int directionV;
	private int directionH;
	public GameObject plateforme = null;
	private GameObject plateformeTarget = null;
	public float distance = 2f;
	
	void Start()
	{
	    lineRenderer = GetComponent< LineRenderer >();        
	    lineRenderer.SetWidth( 0.2f, 0.2f );
	}
	
	
	void Update()
	{
	
	    if ( Input.touchCount > 0 )
	    {
	        if ( Input.touches[0].phase == TouchPhase.Began ){
				bool joueur = false;
				Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		        	RaycastHit hit ;
				
				//Check if there is a collider attached already, otherwise add one on the fly
				if(collider == null)
					gameObject.AddComponent(typeof(BoxCollider));
				
		       		if (Physics.Raycast (ray, out hit)) 
				{
					if(hit.collider.gameObject == Player)
						joueur = true;
					else if( hit.collider.gameObject.tag == "platform")
						plateformeTarget = hit.collider.gameObject;
				}
				if( joueur ) {
					endPoint = false;
					myPoints = null;
		       		idTouch = Input.touches[0].fingerId;
					Debug.Log( " --> fingerId : " + idTouch );
		           	InvokeRepeating( "AddPoint", 0.01f, refreshTouch );
				} else if( plateformeTarget != null ) {
		       		idTouch = Input.touches[0].fingerId;
					Debug.Log( " --> fingerId : " + idTouch );
		           	InvokeRepeating( "setDirection", 0.01f, 0.1f );
				} else {
				    Vector3 temp = Camera.main.ScreenToWorldPoint( Input.touches[0].position );
					temp = new Vector3( Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
				
					//limiter la zone au dessus du joueur et à un rayon de 'distance'
					if( (Player.transform.position - temp).magnitude > distance ){
			        	Instantiate(plateforme, temp,  Quaternion.identity);
					}
				}
			}
			bool stillThere = false;
			foreach( Touch touch in Input.touches ) {
				if( touch.fingerId == idTouch ) {
					stillThere = true;
				}
			}
			if( !stillThere ) {
				cleanEnded();
			}
	    } 
	    else
	    {
			cleanEnded();
	    }
	}
	
	void cleanEnded() {
		if( plateformeTarget != null ) {
			platform plat = plateformeTarget.GetComponent<platform>();
			
			Debug.Log( "RH" + directionH + " RV" + directionV );
			if( directionH != 0 ) {
				plat.directionH = directionH;
				plat.triggered = true;
				Destroy( plateformeTarget );
			} else if( directionV != 0 ) {
				plateformeTarget.GetComponent<platform>().directionV = directionV;
				plat.triggered = true;
				Destroy( plateformeTarget );
			}
			plateformeTarget = null;
		}
		endPoint = true;
        CancelInvoke();
		idTouch = -1;
	}
	
	void setDirection() {
		if( lastPoint == Vector3.zero ) {
			lastPoint = plateformeTarget.transform.position;
		}
			lastPoint = plateformeTarget.transform.position;
		foreach( Touch touch in Input.touches ) {
			if( touch.fingerId == idTouch ) {
		    	Vector3 temp = Camera.main.ScreenToWorldPoint( touch.position );
				if( Mathf.Abs( lastPoint.x - temp.x ) > Mathf.Abs(lastPoint.y - temp.y)  ) {
					directionH = Mathf.RoundToInt( Mathf.Max( Mathf.Min( temp.x - lastPoint.x, 1 ), -1 ) );
					directionV = 0;
				} else {
					directionH = 0;
					directionV = Mathf.RoundToInt( Mathf.Max( Mathf.Min( temp.y - lastPoint.y, 1 ), -1 ) );
				}
				
				//lastPoint = touch.position;
			}
		}
	}
	
	void AddPoint()
	{
	    int j = 0;
	    Vector3[] tempPoints;
	
	    if ( myPoints == null || myPoints.Length < 1 )
	        tempPoints = new Vector3[1];
	    else
	    {
	        tempPoints = new Vector3[ myPoints.Length + 1 ];
	
	        for( j = 0; j < myPoints.Length; j++)
	            tempPoints[j] = myPoints[j];
	    }
	
	    Vector3 tempPos = Vector3.zero;
		foreach( Touch touch in Input.touches ) {
			if( touch.fingerId == idTouch ) {
				tempPos = touch.position;
			}
		}
		
		if( tempPos != Vector3.zero ) {
		    tempPos.z = 10;    
		    Vector3 temp = Camera.main.ScreenToWorldPoint( tempPos );
			temp = new Vector3( /*Mathf.Round*/( temp.x ),/* Mathf.Round*/( temp.y ), 0 );
			if( temp.x == tempPoints[ tempPoints.Length - 1 ].y && temp.y == tempPoints[ tempPoints.Length - 1 ].y ) {
				//do nothing
			} else {
				tempPoints[j] = temp;
			    myPoints = new Vector3[ tempPoints.Length ]; 
			    myPoints = tempPoints; 
			}
		}
	}
}
