using UnityEngine;
using System.Collections;


public struct Cote {
	public bool haut;
	public bool bas;
	public bool gauche;
	public bool droite;
	public int gravity;
	public Cote( int theGravity, bool isHaut, bool isBas, bool isGauche, bool isDroite ) {
		gravity = theGravity;
		haut = isHaut;
		bas = isBas;
		gauche = isGauche;
		droite = isDroite;
	}
}

public struct ColliderId {
    public int id;
	public Cote cote;
	
	public ColliderId( int idCollider, int theGravity, bool isHaut, bool isBas, bool isGauche, bool isDroite ) {
		id = idCollider;
		cote = new Cote( theGravity, isHaut, isBas, isGauche, isDroite );
	}
}

public class Player : MonoBehaviour {
	public float seuil = 0.31f;
	private DrawPath gestion;
	private ColliderId[] colliders;
	public int gravity;

	// Use this for initialization
	void Start () {
		gestion = Camera.main.GetComponent<DrawPath>();
		gravity = gestion.gravityState;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter( Collision theCollision ){
		Debug.Log("Collision");
		
	    if(theCollision.gameObject.name == "floor" ||theCollision.gameObject.name == "fixe" || theCollision.gameObject.tag == "platform" )
	    {	
			gravity = gestion.gravityState;
			
		    int j = 0;
		    ColliderId[] tempColliders;
		
		    if ( colliders == null || colliders.Length < 1 )
		        tempColliders = new ColliderId[1];
		    else
		    {
		        tempColliders = new ColliderId[ colliders.Length + 1 ];
		        for( j = 0; j < colliders.Length; j++)
		            tempColliders[j] = colliders[j];
		    }
			colliders = tempColliders;
				
			bool setBas, setHaut, setGauche, setDroite;
				
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
			if( posX <= 0 && posX > -seuil ) {
				setGauche = true;
			} else 
				setGauche = false;
			posX -= transform.localScale.x + theCollision.gameObject.transform.localScale.x;
			
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
			colliders[ colliders.Length - 1 ] = new ColliderId( theCollision.gameObject.GetInstanceID(), 0, setHaut, setBas, setGauche, setDroite );
			
			gestion.Add( new Cote( 0, setHaut, setBas, setGauche, setDroite ) );

	    }
	}
	
	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit( Collision theCollision ){
		Debug.Log("CollisionExit");
	    if(theCollision.gameObject.name == "floor" || theCollision.gameObject.tag == "platform" )
	    {
			foreach( ColliderId collider in colliders ) {
				if( theCollision.gameObject.GetInstanceID() == collider.id ) {
					gestion.Substract( collider.cote );
				}
			}
	    }
	}
}
