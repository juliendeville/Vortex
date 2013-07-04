using UnityEngine;
using System.Collections;

public class PouvoirsRD : MonoBehaviour {
	
	public Camera theCamera;
	public Transform plateforme;
	public int nbPlatformMax = 5;
	public int nbPlatforms = 0;
	public Transform player;
	public float distance = 5f;
	public float side = 1.24f;
	public float up = 0.33f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 position = Vector2.zero;
		for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                position = Input.GetTouch(i).position;
            }
        }
		if( Input.GetMouseButtonDown( 0 ) ) {
			position = Input.mousePosition;
		}
		if( position != Vector2.zero && nbPlatforms < nbPlatformMax ) {
			/*
			Ray ray = theCamera.ScreenPointToRay( position );    
    		Vector3 point = ray.origin + (ray.direction * theCamera.transform.position.z );  
			point.z = 0;
			*/
			int direction = 1;
			if( player.rigidbody.velocity.x < 0 )
				direction = -1;
			
			Vector3 point = new Vector3(player.position.x + ( direction * side), player.position.y - up, 0 );

			
			//limiter la zone au dessus du joueur et à un rayon de 'distance'
			//if( (player.position - point).magnitude < distance ){
	        	Instantiate(plateforme, point,  Quaternion.AngleAxis(direction * 45, Vector3.forward));
				nbPlatforms++;
			//}
		}
	}
}
