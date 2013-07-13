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
	
	//saut
	public float JumpAcc = 5.0f;
	public bool jumpAsked = false;
	public bool mustJumpNormal = false;
	public bool air = false;
	
	//accelerometer
	private Vector3 zeroAc;
	private Vector3 curAc;
	public float sensH = 10f;
	//private float sensV = 10f;
	private float smooth = 0.5f;
	private float GetAxisH = 0f;
	//private float GetAxisV = 0f;
	public float smoothingFactor = 5.5f;
	private float addHorizontalForce = 0;
	public float HorizontalMaxVelocity = 9f;
	
	//collisions et rotation
	public int gravityState = 0;
	public float gravityForce = 6000;
	public bool haut = false;
	public bool bas = false;
	public bool gauche = false;
	public bool droite = false;
	//private GameObject cadrage;
	//private GameObject map;
	public float MapRotationSpeed = 5f;
	
	void ResetAxes(){
	    zeroAc = Input.acceleration;
	    curAc = Vector3.zero;
	}

	void Start()
	{
		//cadrage = GameObject.FindWithTag("cadrage");
		//map = GameObject.FindWithTag("map");
	    ResetAxes();
	    lineRenderer = GetComponent< LineRenderer >();        
	    lineRenderer.SetWidth( 0.2f, 0.2f );
	}
	
	void FixedUpdate() {
		CanJump();	
		if( mustJumpNormal ){
			JumpNormal();
			mustJumpNormal = false;
			jumpAsked = false;
		}
		if( addHorizontalForce < 0.5f && addHorizontalForce > -0.5f ) {
			addHorizontalForce = 0;
		} else if( addHorizontalForce < 1 && addHorizontalForce > -1 ) {
			if( addHorizontalForce > 0 )
				addHorizontalForce = 0.5f;
			else 
				addHorizontalForce = -0.5f;	
		}
		
		Vector3 tvel = new Vector3( addHorizontalForce * HorizontalMaxVelocity, Player.rigidbody.velocity.y, 0 );
		Player.rigidbody.velocity = Vector3.Lerp( Player.rigidbody.velocity, tvel, Time.deltaTime * smoothingFactor);
		addHorizontalForce = 0;
	}
	
	
	public void setWithGravity( bool setHaut, bool setBas, bool setDroite, bool setGauche ) {
		//rotation selon la gravité
		if(      ( setHaut && gravityState == 0 ) || ( setBas && gravityState == 2 ) || ( setDroite && gravityState == 3 ) || ( setGauche && gravityState == 1 ) )
			haut = true;
		else if( ( setHaut && gravityState == 2 ) || ( setBas && gravityState == 0 ) || ( setDroite && gravityState == 1 ) || ( setGauche && gravityState == 3 ) )
			bas = true;
		else if( ( setHaut && gravityState == 3 ) || ( setBas && gravityState == 1 ) || ( setDroite && gravityState == 0 ) || ( setGauche && gravityState == 2 ) )
			droite = true;
		else if( ( setHaut && gravityState == 1 ) || ( setBas && gravityState == 3 ) || ( setDroite && gravityState == 2 ) || ( setGauche && gravityState == 0 ) )
			gauche = true;
	}
	
	
	void Update()
	{
		//la camera suit le perso
		transform.position = new Vector3( Player.transform.position.x, 2 + Player.transform.position.y, -20 );
		
		//recupération des données de l'accelerometre
	    curAc = Vector3.Lerp(curAc, (Input.acceleration-zeroAc), (Time.deltaTime/smooth));
	    //GetAxisV = Mathf.Clamp(curAc.y * sensV, -1, 1);
	    GetAxisH = Mathf.Clamp(curAc.x * sensH, -1, 1);
	    // now use GetAxisV and GetAxisH instead of Input.GetAxis vertical and horizontal
	    // If the horizontal and vertical directions are swapped, swap curAc.y and curAc.x
	    // in the above equations. If some axis is going in the wrong direction, invert the
	    // signal (use -curAc.x or -curAc.y)
		addHorizontalForce = GetAxisH;
		
		//rotation de la map
		var rc = transform.eulerAngles;
   		transform.rotation = Quaternion.Euler(rc.x, rc.y, Mathf.LerpAngle(rc.z, gravityState*90, MapRotationSpeed * Time.deltaTime));
		var rp = Player.transform.eulerAngles;
   		Player.transform.rotation = Quaternion.Euler(rp.x, rp.y, Mathf.LerpAngle(rp.z, gravityState*90, MapRotationSpeed * Time.deltaTime));
		
		if( gravityState == 0 )
			Player.constantForce.force = new Vector3( 0, -gravityForce, 0 );
		else if( gravityState == 1 )
			Player.constantForce.force = new Vector3( gravityForce, 0, 0 );
		else if( gravityState == 2 )
			Player.constantForce.force = new Vector3( 0, gravityForce, 0 );
		else if( gravityState == 3 )
			Player.constantForce.force = new Vector3( -gravityForce, 0, 0 );

		//gestion des touch/swipes/gestures
	    if ( Input.touchCount > 0 )
	    {
	        if ( Input.touches[0].phase == TouchPhase.Began ){
				Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		        	RaycastHit hit ;
				
				//Check if there is a collider attached already, otherwise add one on the fly
				if(collider == null)
					gameObject.AddComponent(typeof(BoxCollider));
				
	       		if (Physics.Raycast (ray, out hit)) {
					/*
					if(hit.collider.gameObject == Player)
						joueur = true;
					else */
					if( hit.collider.gameObject.tag == "platform")
						plateformeTarget = hit.collider.gameObject;
				}
				if( plateformeTarget != null ) {
		       		idTouch = Input.touches[0].fingerId;
					Debug.Log( " --> fingerId : " + idTouch );
		           	InvokeRepeating( "setDirection", 0.01f, 0.1f );
				} else {
				    Vector3 temp = Camera.main.ScreenToWorldPoint( Input.touches[0].position );
					temp = new Vector3( Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
				
					//limiter la zone au dessus du joueur et à un rayon de 'distance'
					if( (Player.transform.position - temp).magnitude < distance ){ // joueur
						endPoint = false;
						myPoints = null;
			       		idTouch = Input.touches[0].fingerId;
						if( haut && !droite && !gauche )
							jumpAsked = true;
						else {
							Debug.Log( (haut?"haut ":"/haut ") + (bas?"bas ":"/bas ") + (gauche?"gauche ":"/gauche ") +(droite?"droite ":"/droite ")  );
							if( droite )
								gravityState++;
							if( gauche )
								gravityState--;
							gravityState = ( gravityState + 4 ) % 4;
							Player.rigidbody.velocity = Vector3.zero;
							setWithGravity( haut,bas,droite,gauche);
							Debug.Log( (haut?"haut ":"/haut ") + (bas?"bas ":"/bas ") + (gauche?"gauche ":"/gauche ") +(droite?"droite ":"/droite ")  );
						}
					} else { // creation plateforme
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
			}
		}
	}
	

	void CanJump(){
		if( jumpAsked && !air ) {
			mustJumpNormal = true;
		}
		return;
	}
	
	void JumpNormal() {
		Player.rigidbody.AddForce(0, JumpAcc,0,ForceMode.Acceleration);
	}
	
}
