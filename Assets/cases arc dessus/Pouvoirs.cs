using UnityEngine;
using System.Collections;

public class Pouvoirs : MonoBehaviour {
	
	public Camera theCamera;
	public Transform plateforme;
	public int nbPlatformMax = -1;
	public int nbPlatforms = 0;
	public Transform player;
	public float distance = 2f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		/*
		Vector3 position = Vector3.zero;
		for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                position = Input.GetTouch(i).position;
            }
        }
		
		if( Input.touchCount == 1 ) {
			Debug.Log( "Input.touches[0].fingerId : " +  Input.touches[0].fingerId );
			if( Input.touches[0].fingerId != theCamera.GetComponent<DrawPath>().idTouch ) {
				Debug.Log( "Input.touches[0].phase : " + (Input.touches[0].phase== TouchPhase.Ended?"ended":"other") );
			}
		}
		if( Input.touchCount == 1 && Input.touches[0].fingerId != theCamera.GetComponent<DrawPath>().idTouch && Input.touches[0].phase == TouchPhase.Began ) {
			position = Input.touches[0].position;
		}
		if( position != Vector3.zero && nbPlatforms < nbPlatformMax ) {
		    Vector3 temp = Camera.main.ScreenToWorldPoint( position );
			temp = new Vector3( Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
			/*
			Ray ray = theCamera.ScreenPointToRay( position );    
    		Vector3 point =  ( ray.origin + (ray.direction * -theCamera.transform.position.z ) );  
			point.z = 0;
			
			
			//limiter la zone au dessus du joueur et à un rayon de 'distance'
			if( (player.position - temp).magnitude > distance ){
	        	platform nouvellePlateforme = Instantiate(plateforme, temp,  Quaternion.identity) as platform;
				//nouvellePlateforme.theCamera = theCamera;
				nbPlatforms++;
			}
		}
		*/
	}
}
