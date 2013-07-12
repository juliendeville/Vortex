using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public GameObject Player;
	public float HorizontalMaxVelocity = 9f;
	public float smoothingFactor = 7f;
	private float addHorizontalForce = 0f;
	
	private LineRenderer lineRenderer;
	private Vector3[] myPoints;
	private int idTouch = -1;
	
	void Start()
	{
	    lineRenderer = GetComponent< LineRenderer >();        
	    lineRenderer.SetWidth( 0.2f, 0.2f );
		
			
	}
	
	void FixedUpdate() {
		if( Player != null ) {
			Vector3 tvel = new Vector3( addHorizontalForce * HorizontalMaxVelocity, rigidbody.velocity.y, 0 );
			Player.rigidbody.velocity = Vector3.Lerp( rigidbody.velocity, tvel, Time.deltaTime * smoothingFactor);
		}
	}
	
	void Update()
	{
	    if ( myPoints != null && myPoints.Length > 0 )
	    {
			Debug.Log ( "update" );
			Debug.Log ( myPoints );
			Debug.Log ( "update" );
			//iTween.DrawPath(myPoints);
			/*
	        lineRenderer.SetVertexCount( myPoints.Length );
	
	        for ( int i = 0; i < myPoints.Length; i++ )
	       {
	            lineRenderer.SetPosition( i, myPoints[i] );    
	        }
	        */
	    }
	    else
	       lineRenderer.SetVertexCount(0);
	
	    if ( Input.touchCount > 0 )
	    {
	       if ( Input.touches[0].phase == TouchPhase.Began ){
	       		idTouch = Input.touches[0].fingerId;
	           InvokeRepeating( "AddPoint", 0.01f, 0.1f );
			}
	    } 
	    else
	    {
	        CancelInvoke();
	        myPoints = null;
	    }
	}
	
	void OnDrawGizmos(){
		
			Debug.Log ( "gizmo" );
			Debug.Log ( myPoints );
			Debug.Log ( "gizmo" );
	    if ( myPoints != null && myPoints.Length > 0 )
	    {
			Debug.Log ( myPoints );
			iTween.DrawPath(myPoints, Color.magenta);
		}
	}
	
	void AddPoint()
	{
	    int j = 0;
	    Vector3[] tempPoints;
	
	    if (  myPoints == null || myPoints.Length < 1 )
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
			temp = new Vector3( Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
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
