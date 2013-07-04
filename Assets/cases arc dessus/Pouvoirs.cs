using UnityEngine;
using System.Collections;

public class Pouvoirs : MonoBehaviour {
	
	public Camera theCamera;
	public Transform plateforme;
	public int nbPlatformMax = 5;
	public int nbPlatforms = 0;
	public Transform player;
	public float distance = 5f;
	
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
			Ray ray = theCamera.ScreenPointToRay( position );    
    		Vector3 point = ray.origin + (ray.direction * theCamera.transform.position.z );  
			point.z = 0;
			
			//limiter la zone au dessus du joueur et à un rayon de 'distance'
			//if( (player.position - point).magnitude < distance ){
	        	platform nouvellePlateforme = Instantiate(plateforme, point,  Quaternion.identity) as platform;
				//nouvellePlateforme.theCamera = theCamera;
				nbPlatforms++;
			//}
		}
	}
}
