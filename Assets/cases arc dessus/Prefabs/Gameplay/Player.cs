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

public struct ColliderCote {
    public GameObject gameObject;
	public Cote cote;
	
	public ColliderCote( GameObject thegameobject, int theGravity, bool isHaut, bool isBas, bool isGauche, bool isDroite ) {
		gameObject = thegameobject;
		cote = new Cote( theGravity, isHaut, isBas, isGauche, isDroite );
	}
	public ColliderCote( GameObject thegameobject, Cote theCote ) {
		gameObject = thegameobject;
		cote = theCote;
	}
}

public class Player : MonoBehaviour {
	public float seuil = 0.31f;
	private DrawPath gestion;
	private ColliderCote[] colliders;
	public int gravity;
	private Vector3 spawn;
	private AnimationTexture anim;
	private int lastdirection = 0;

	// Use this for initialization
	void Start () {
		spawn = transform.position;
		gestion = Camera.main.GetComponent<DrawPath>();
		gravity = gestion.gravityState;
		anim = gameObject.GetComponent<AnimationTexture>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void Anim( int direction ) {
		Debug.Log("direction" + direction + " lastdirection" + lastdirection);
		if( direction != lastdirection ) {
			anim.Stop();
			if( direction == 0 ) {
				if( lastdirection == -1 )
					anim.SetFrame( 0 );
				else 
					anim.SetFrame( 6 );
			} else {
				if( direction == -1 )
					anim.Play( new int[6] { 0, 1, 2, 3, 4, 5 } );
				else 
					anim.Play( new int[6] { 6, 7, 8, 9, 10, 11 } );
			}
			lastdirection = direction;
		}
	}
	
	void OnCollisionEnter( Collision theCollision ){
		//Debug.Log("Collision");
		
	    if(theCollision.gameObject.name == "floor" ||theCollision.gameObject.name == "fixe" || theCollision.gameObject.tag == "platform" )
	    {	
			gravity = gestion.gravityState;
			
		    ColliderCote[] tempColliders;
		
		    if ( colliders == null || colliders.Length < 1 )
		        tempColliders = new ColliderCote[1];
		    else
		    {
		        tempColliders = new ColliderCote[ colliders.Length + 1 ];
		        for( int j = 0; j < colliders.Length; j++)
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
			
			//Cote theCote = gestion.CoteWithGravity( new Cote( 0, setHaut, setBas, setGauche, setDroite) );
			Cote theCote = new Cote( 0, setHaut, setBas, setGauche, setDroite );
			colliders[ colliders.Length - 1 ] = new ColliderCote( theCollision.gameObject, theCote );
			gestion.Add( theCote );
			
			/*
			if( colliders.Length == 0 )
				Debug.Log( "Collisions ="+colliders.Length );
			if( colliders.Length == 1 )
				Debug.Log( "Collisions ="+colliders.Length + " " + colliders[0].gameObject.name );
			if( colliders.Length == 2 )
				Debug.Log( "Collisions ="+colliders.Length + " " + colliders[0].gameObject.name + " " + colliders[1].gameObject.name );
			if( colliders.Length >= 3 )
				Debug.Log( "Collisions ="+colliders.Length + " " + colliders[0].gameObject.name + " " + colliders[1].gameObject.name + " " + colliders[2].gameObject.name );
				*/
	    } else if( theCollision.gameObject.tag == "Respawn" ) {
			transform.position = spawn; 
		}
	}
	
	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit( Collision theCollision ){
		//Debug.Log("CollisionExit");
	    if(theCollision.gameObject.name == "floor" ||theCollision.gameObject.name == "fixe" || theCollision.gameObject.tag == "platform" )
	    {
			ColliderCote[] CollidersTemp = null;
			
			for( int i = 0; i < colliders.Length; i++ ) {
				ColliderCote collider = colliders[i];
				if( theCollision.gameObject == collider.gameObject ) {
					gestion.Substract( collider.cote );
					CollidersTemp = new ColliderCote[colliders.Length - 1];
					int nb = 0;
					for( int j = 0; j < colliders.Length; j++ ) {
						if( j != i ) {
							CollidersTemp[nb] = colliders[j];
							nb++;
						}
					}
					
				}
			}
			if( CollidersTemp != null ) {
				colliders = CollidersTemp;
			}
			//Debug.Log( "Collisions ="+colliders.Length );
	    }
	}
}
