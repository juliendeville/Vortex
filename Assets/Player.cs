using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private int gravity = 0;
	private bool haut = false;
	private bool bas = false;
	private bool gauche = false;
	private bool droite = false;
	public bool air = false;
	public float seuil = 0.31f;
	private DrawPath gestion;

	// Use this for initialization
	void Start () {
		gestion = Camera.main.GetComponent<DrawPath>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter( Collision theCollision ){
		Debug.Log("Collision");
		
	    if(theCollision.gameObject.name == "floor" ||theCollision.gameObject.name == "fixe" || theCollision.gameObject.tag == "platform" )
	    {	
			bool setBas, setHaut, setGauche, setDroite;
			Debug.Log("ColliderPosition x : " + theCollision.gameObject.transform.position.x + " y : " + theCollision.gameObject.transform.position.y );
			Debug.Log("PlayerPosition x : " + transform.position.x + " y : " + transform.position.y );
			float posY = transform.position.y + transform.localScale.y / 2 - theCollision.gameObject.transform.position.y + theCollision.gameObject.transform.localScale.y / 2;
			if( posY >= 0 && posY < seuil ) {
				setBas = true;
			} else 
				setBas = false;
			posY -= transform.localScale.y + theCollision.gameObject.transform.localScale.y;
			if( posY <= 0 && posY > -seuil ) {
				setHaut = true;
			} else 
				setHaut = false;
			float posX = transform.position.x + transform.localScale.x / 2 - theCollision.gameObject.transform.position.x + theCollision.gameObject.transform.localScale.x / 2;
			Debug.Log( "posX : " + posX );
			if( posX <= 0 && posX > -seuil ) {
				setGauche = true;
			} else 
				setGauche = false;
			posX -= transform.localScale.x + theCollision.gameObject.transform.localScale.x;
			Debug.Log( "posX : " + posX );
			if( posX >= 0 && posX < seuil ) {
				setDroite = true;
			} else 
				setDroite = false;
			/*
			if( theCollision.gameObject.tag == "platform" )
				Debug.Log( "haut" + (haut?1:0) + "bas" + (bas?1:0) + "gauche" + (gauche?1:0) + "droite" + (droite?1:0) );
			
			if( ( haut && gauche ) || ( haut && droite ) ) {
				gauche = false;
				droite = false;
			}
			if( ( bas && gauche ) || ( bas && droite ) ) {
				bas = false;
			}
			*/
			
			gestion.setWithGravity( setHaut, setBas, setDroite, setGauche );
	    }
	    
	}
	
	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit( Collision theCollision ){
		Debug.Log("CollisionExit");
	    if(theCollision.gameObject.name == "floor" || theCollision.gameObject.tag == "platform" )
	    {
			bool setBas, setHaut, setGauche, setDroite;
			float posY = transform.position.y - theCollision.gameObject.transform.position.y;
			if( posY >= 0 ) {
				setBas = false;
				setHaut = true;
			} else {
				setHaut = false;
				setBas = true;
			}
			float posX = transform.position.x - theCollision.gameObject.transform.position.x;
			if( posX >= 0 && posX < seuil ) {
				setGauche = false;
				setDroite = true;
			} else {
				setDroite = false;
				setGauche = true;
			}
			
			Debug.Log( "Avant Roration" + (haut?"haut ":"/haut ") + (bas?"bas ":"/bas ") + (gauche?"gauche ":"/gauche ") +(droite?"droite ":"/droite ")  );
			//rotation selon la gravité
			gestion.setWithGravity( setHaut, setBas, setDroite, setGauche );
			
	    }
	}
}
