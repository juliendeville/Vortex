using UnityEngine;
using System.Collections;

public class DrawPath : MonoBehaviour {
	public GameObject Player;
	
	private LineRenderer lineRenderer;
	public Vector3[] myPoints = null;
	public bool endPoint = false;
	public float refreshTouch = 0.2f;
	public int idTouchPlatform = -1;
	public int idTouchPlayer = -1;
	private Vector2 lastPoint = Vector2.zero;
	private int directionV;
	private int directionH;
	public GameObject plateforme = null;
	private GameObject plateformeTarget = null;
	public float distance = 2f;
	public bool stopped = false;
	
	//saut
	public float JumpAcc = 5.0f;
	public bool jumpAsked = false;
	public bool mustJumpNormal = false;
	public bool air = false;
	
	//accelerometer
	public float seuil = 0.1f;
	private Vector3 zeroAc;
	private Vector3 curAc;
	public float sensH = 10f;
	//private float sensV = 10f;
	public float smoothAcc = 0.5f;
	private float GetAxisH = 0f;
	//private float GetAxisV = 0f;
	public float smoothingFactor = 5.5f;
	private float addHorizontalForce = 0;
	private float addVerticalForce = 0;
	public float HorizontalMaxVelocity = 9f;
	private float lastJump = 0;
	
	//collisions et rotation
	public int gravityState = 0;
	public float gravityForce = 6000;
	public Cote cote;
	//private GameObject cadrage;
	//private GameObject map;
	public float MapRotationSpeed = 5f;
	
	//stop in air
	private Vector3 SavedVelocity = Vector3.zero;
	
	
	void ResetAxes(){
	    zeroAc = Input.acceleration;
	    curAc = Vector3.zero;
	}

	void Start()
	{
		cote = new Cote( gravityState, false, false, false, false);
		//cadrage = GameObject.FindWithTag("cadrage");
		//map = GameObject.FindWithTag("map");
	    ResetAxes();
	    lineRenderer = GetComponent< LineRenderer >();        
	    lineRenderer.SetWidth( 0.2f, 0.2f );
	}
	
	void FixedUpdate() {
		int i = 0;
		if( !stopped ) {
			CanJump();
			addHorizontalForce = MovementForceRestrictions( addHorizontalForce );
			addVerticalForce = MovementForceRestrictions( addVerticalForce );
			
			Vector3 tvel;
			//Debug.Log( "h: " + addHorizontalForce + " v: " + addVerticalForce );
			if( addHorizontalForce != 0 ){
				tvel = new Vector3( addHorizontalForce * HorizontalMaxVelocity, Player.rigidbody.velocity.y, 0 );
				Player.rigidbody.velocity = Vector3.Lerp( Player.rigidbody.velocity, tvel, Time.deltaTime * smoothingFactor);
			}else if( addVerticalForce != 0 ){
				tvel = new Vector3( Player.rigidbody.velocity.x, addVerticalForce * HorizontalMaxVelocity, 0 );
				Player.rigidbody.velocity = Vector3.Lerp( Player.rigidbody.velocity, tvel, Time.deltaTime * smoothingFactor);
			}
			//animation deplacement
			Vector3 actualSpeed = Player.rigidbody.velocity;
			if( ( actualSpeed.x > seuil && gravityState == 0 ) || ( actualSpeed.x < -seuil && gravityState == 2 ) || ( actualSpeed.y > seuil && gravityState == 1 ) || ( actualSpeed.y < -seuil && gravityState == 3 ) )
				i = 1;
			if( ( actualSpeed.x < -seuil && gravityState == 0 ) || ( actualSpeed.x > seuil && gravityState == 2 ) || ( actualSpeed.y < -seuil && gravityState == 1 ) || ( actualSpeed.y > seuil && gravityState == 3 ) )
				i = -1;
			Player.GetComponent<Player>().Anim( i );
			
			// on avance tout le temps
			//addHorizontalForce = 0;
			//addVerticalForce = 0;	
			
			if( mustJumpNormal ){
				JumpNormal();
				mustJumpNormal = false;
				jumpAsked = false;
			}
		} 
		Player.GetComponent<Player>().Anim( i );
	}
	
	float MovementForceRestrictions( float force ) {
		if( force < 0.5f && force > -0.5f ) {
			force = 0;
		} else if( force < 1 && force > -1 ) {
			if( force > 0 )
				force = 0.5f;
			else 
				force = -0.5f;	
		}
		return force;
	}
	
	
	public Cote CoteWithGravity( Cote coteGiven ) {
		//Debug.Log( "given " + (coteGiven.haut?"haut ":"/haut ") + (coteGiven.bas?"bas ":"/bas ") + (coteGiven.gauche?"gauche ":"/gauche ") +(coteGiven.droite?"droite ":"/droite ") + " gravCote : " + coteGiven.gravity + " gravActual : " + gravityState   );
		Cote cotetemp = new Cote( 0, false, false, false, false );

		//coteGiven.gravity = ( coteGiven.gravity - gravityState + 4 ) % 4;
		//Debug.Log( "grav final : " + coteGiven.gravity );
		
		coteGiven.gravity = gravityState;
		//rotation selon la gravité
		if( ( coteGiven.haut && coteGiven.gravity == 0 ) || ( coteGiven.bas && coteGiven.gravity == 2 ) || ( coteGiven.droite && coteGiven.gravity == 3 ) || ( coteGiven.gauche && coteGiven.gravity == 1 ) )
			cotetemp.haut = true;
		if( ( coteGiven.haut && coteGiven.gravity == 2 ) || ( coteGiven.bas && coteGiven.gravity == 0 ) || ( coteGiven.droite && coteGiven.gravity == 1 ) || ( coteGiven.gauche && coteGiven.gravity == 3 ) )
			cotetemp.bas = true;
		if( ( coteGiven.haut && coteGiven.gravity == 1 ) || ( coteGiven.bas && coteGiven.gravity == 3 ) || ( coteGiven.droite && coteGiven.gravity == 0 ) || ( coteGiven.gauche && coteGiven.gravity == 2 ) )
			cotetemp.droite = true;
		if( ( coteGiven.haut && coteGiven.gravity == 3 ) || ( coteGiven.bas && coteGiven.gravity == 1 ) || ( coteGiven.droite && coteGiven.gravity == 2 ) || ( coteGiven.gauche && coteGiven.gravity == 0 ) )
			cotetemp.gauche = true;
		cotetemp.gravity = gravityState;
		//Debug.Log( "returned " + (cotetemp.haut?"haut ":"/haut ") + (cotetemp.bas?"bas ":"/bas ") + (cotetemp.gauche?"gauche ":"/gauche ") +(cotetemp.droite?"droite ":"/droite ")  );
		return cotetemp;
	}
	
	public void Set( Cote coteGiven ) {
		Cote coteTemp = coteGiven;//CoteWithGravity( coteGiven );
		cote.haut = coteTemp.haut;
		cote.bas = coteTemp.bas;
		cote.gauche = coteTemp.gauche;
		cote.droite = coteTemp.droite;
		//Debug.Log( "SetcurrentState " + (cote.haut?"haut ":"/haut ") + (cote.bas?"bas ":"/bas ") + (cote.gauche?"gauche ":"/gauche ") +(cote.droite?"droite ":"/droite ")  );
	}
	
	public void Add( Cote coteGiven ) {
		Cote coteTemp = coteGiven;//CoteWithGravity( coteGiven );
		if( coteTemp.haut )
			cote.haut = true;
		if( coteTemp.bas )
			cote.bas = true;
		if( coteTemp.gauche )
			cote.gauche = true;
		if( coteTemp.droite )
			cote.droite = true;
		//Debug.Log( "AddcurrentState " + (cote.haut?"haut ":"/haut ") + (cote.bas?"bas ":"/bas ") + (cote.gauche?"gauche ":"/gauche ") +(cote.droite?"droite ":"/droite ")  );
	}
	
	public void Substract( Cote coteGiven ) {
		Cote coteTemp = coteGiven;//CoteWithGravity( coteGiven );
		if( coteTemp.haut )
			cote.haut = false;
		if( coteTemp.bas )
			cote.bas = false;
		if( coteTemp.gauche )
			cote.gauche = false;
		if( coteTemp.droite )
			cote.droite = false;
		//Debug.Log( "SubcurrentState " + (cote.haut?"haut ":"/haut ") + (cote.bas?"bas ":"/bas ") + (cote.gauche?"gauche ":"/gauche ") +(cote.droite?"droite ":"/droite ")  );
	}
	
	void OnGUI () {
		string msg = "Freeze !";
		Cote realCote = CoteWithGravity( cote );
		if( realCote.droite || realCote.gauche ) {
			msg = "Rotate !";
			if ( GUI.Button (new Rect (0,Screen.height- Screen.height/5,Screen.width/5,Screen.height/5), msg) ) {
				Rotate(realCote);
			}
		} else {
			if ( GUI.Button (new Rect (0,Screen.height- Screen.height/5,Screen.width/5,Screen.height/5), msg) ) {
				Vector3 last = SavedVelocity;
				SavedVelocity = Player.rigidbody.velocity;
				Player.rigidbody.velocity = last;
				stopped = !stopped;
			}
		}
	}
	
	void Update()
	{
		//la camera suit le perso
		transform.position = new Vector3( Player.transform.position.x, Player.transform.position.y, -20 );
		
		//recupération des données de l'accelerometre
		Vector3 Acc = (Input.acceleration-zeroAc);
		if( Acc.sqrMagnitude > 0.001 ) {
		    curAc = Vector3.Lerp(curAc, Acc, (Time.deltaTime/smoothAcc));
		    //GetAxisV = Mathf.Clamp(curAc.y * sensV, -1, 1);
		    GetAxisH = Mathf.Clamp(curAc.x * sensH, -1, 1);
		    // now use GetAxisV and GetAxisH instead of Input.GetAxis vertical and horizontal
		    // If the horizontal and vertical directions are swapped, swap curAc.y and curAc.x
		    // in the above equations. If some axis is going in the wrong direction, invert the
		    // signal (use -curAc.x or -curAc.y)
			if( GetAxisH > 0 )
				GetAxisH = 1;
			else 
				GetAxisH = -1;
				
			if( gravityState == 0 ){
				addHorizontalForce = GetAxisH;//GetAxisH;
				addVerticalForce = 0;
			} else if( gravityState == 1 ){
				addVerticalForce = GetAxisH;//GetAxisH;
				addHorizontalForce = 0;
			} else if( gravityState == 2 ){
				addHorizontalForce = -GetAxisH;//GetAxisH;
				addVerticalForce = 0;
			} else if( gravityState == 3 ){
				addVerticalForce = -GetAxisH;//GetAxisH;
				addHorizontalForce = 0;
			}
		} else {
			if( Input.GetAxis("Horizontal") != 0 ) {
				if( gravityState == 0 ){
					addHorizontalForce = Input.GetAxis("Horizontal");
				} else if( gravityState == 1 ){
					addVerticalForce = Input.GetAxis("Horizontal");
				} else if( gravityState == 2 ){
					addHorizontalForce = -Input.GetAxis("Horizontal");
				} else if( gravityState == 3 ){
					addVerticalForce = -Input.GetAxis("Horizontal");
				}
			}
		}
		
		if( Input.GetAxis("Vertical") > 0 && lastJump != Input.GetAxis("Vertical") ){
			//Debug.Log(Input.GetAxis("Vertical"));
			Cote realCote = CoteWithGravity( cote );
			if( realCote.haut && !realCote.droite && !realCote.gauche ){
				Vector3 last = SavedVelocity;
				SavedVelocity = Player.rigidbody.velocity;
				Player.rigidbody.velocity = last;
				stopped = !stopped;
			} else {
				Rotate(realCote);
			}
		}
		lastJump = Input.GetAxis("Vertical");
		
		//rotation de la camera et du joueur
		var rc = transform.eulerAngles;
   		transform.rotation = Quaternion.Euler(rc.x, rc.y, Mathf.LerpAngle(rc.z, gravityState*90, MapRotationSpeed * Time.deltaTime));
		var rp = Player.transform.eulerAngles;
   		Player.transform.rotation = Quaternion.Euler(rp.x, rp.y, Mathf.LerpAngle(rp.z, gravityState*90, MapRotationSpeed * Time.deltaTime));

		//gestion des touch/swipes/gestures
	    if ( Input.touchCount > 0 )
	    {
			
			bool stillThere = false;
			for( int i = 0; i < Input.touches.Length; i++ ) {
				
				if ( Input.touches[i].phase == TouchPhase.Began ){
					Debug.Log( Time.time );
					Debug.Log( Input.touchCount + "touches" );
					Debug.Log( "touch" + i + " type = " + (Input.touches[i].phase== TouchPhase.Began?"Began":Input.touches[i].phase== TouchPhase.Canceled?"canceled":Input.touches[i].phase== TouchPhase.Ended?"ended":Input.touches[i].phase== TouchPhase.Moved?"moved":Input.touches[i].phase== TouchPhase.Stationary?"stationary":"unknown") );
		        
					Ray ray = Camera.main.ScreenPointToRay(Input.touches[i].position);
			        	RaycastHit hit ;
					
					//Check if there is a collider attached already, otherwise add one on the fly
					if(collider == null)
						gameObject.AddComponent(typeof(BoxCollider));
					
					bool bg = false;
		       		if (Physics.Raycast (ray, out hit)) {
						if( hit.collider.gameObject.tag == "platform")
							plateformeTarget = hit.collider.gameObject;
						if( hit.collider.gameObject.name == "background")
							bg = true;
					}
					if( plateformeTarget != null ) {
						Debug.Log( "plateforme" );
			       		idTouchPlatform = Input.touches[0].fingerId;
			           	InvokeRepeating( "setDirection", 0.01f, 0.1f );
					} else {
						//bouton
						if (Input.touches[i].position.x > Screen.width/5 || Input.touches[i].position.y > Screen.height/5 ) { 
							Debug.Log( Input.touches[i].position );
						    Vector3 temp = Camera.main.ScreenToWorldPoint( Input.touches[i].position );
							temp = new Vector3( temp.x , temp.y, 0 );
						
							//limiter la zone au dessus du joueur et à un rayon de 'distance'
							if( (Player.transform.position - temp).magnitude < distance ){ // joueur
								/*
								temp = new Vector3( Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
								Debug.Log( "player" );
								endPoint = false;
								myPoints = null;
					       		idTouchPlayer = Input.touches[0].fingerId;
								Cote realCote = CoteWithGravity( cote );
								//Debug.Log( "realCote " + (realCote.haut?"haut ":"/haut ") + (realCote.bas?"bas ":"/bas ") + (realCote.gauche?"gauche ":"/gauche ") +(realCote.droite?"droite ":"/droite ")  );
								if( realCote.haut && !realCote.droite && !realCote.gauche ){
									//jumpAsked = true;
									Vector3 last = SavedVelocity;
									SavedVelocity = Player.rigidbody.velocity;
									Player.rigidbody.velocity = last;
									stopped = !stopped;
								} else {
									Rotate(realCote);
								}
								*/
							} else if( bg ) { // creation plateforme
								temp = new Vector3( Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
								Debug.Log( "new platforme" );
						    	GameObject plat = Instantiate(plateforme, temp,  Quaternion.identity) as GameObject;
								//var rotplat = plat.transform.eulerAngles;
								//plat.transform.rotation = Quaternion.Euler(rotplat.x, rotplat.y, 90 * gravityState );
								if( gravityState == 1 || gravityState == 3 )
									plat.transform.localScale = new Vector3( plat.transform.localScale.y, plat.transform.localScale.x, plat.transform.localScale.z );
							}
						}
					}
				} else if( Input.touches[i].phase == TouchPhase.Ended ){
					if( Input.touches[i].fingerId == idTouchPlatform )
						CleanTouchPlatform();
				}
				
				if( Input.touches[i].fingerId == idTouchPlayer || Input.touches[i].fingerId == idTouchPlatform ) {
					stillThere = true;
				}
				
			}
			
			if( !stillThere ) {
				CleanTouchPlatform();
			}
			
	    } 
	    else
	    {
			//cleanEnded();
	    }
	}
	
	void CleanTouchPlayer(){
		idTouchPlayer = -1;
		if( SavedVelocity != Vector3.zero ) {
			Player.rigidbody.velocity = SavedVelocity;
			SavedVelocity = Vector3.zero;
		}
	}
	
	void CleanTouchPlatform(){
		if( plateformeTarget != null ) {
			platform plat = plateformeTarget.GetComponent<platform>();
			
			if( gravityState == 1 ) {
				int temp = directionV;
				directionV = directionH;
				directionH = -temp;
			} else if( gravityState == 2 ) {
				directionV = -directionV;
				directionH = -directionH;
			} else if( gravityState == 3 ) {
				int temp = directionV;
				directionV = -directionH;
				directionH = temp;
			}
			
			if( directionH != 0 ) {
				plat.directionH = directionH;
				plat.triggered = true;
				Destroy( plateformeTarget );
			} else if( directionV != 0 ) {
				plat.directionV = directionV;
				plat.triggered = true;
				Destroy( plateformeTarget );
			}
			plateformeTarget = null;
			lastPoint = Vector2.zero;
		}
		endPoint = true;
        CancelInvoke();
	}
	
	void Rotate( Cote lacote ) {
		//cote.gravity = gravityState;
		if( lacote.droite )
			gravityState--;
		else if( lacote.gauche )
			gravityState++;
		else
			return;
		//Debug.Log("rotate" + gravityState);
		gravityState = ( gravityState + 4 ) % 4;
		Player.GetComponent<Player>().gravity = gravityState;
		//Set( cote );
		Player.rigidbody.velocity = Vector3.zero;
		
		if( gravityState == 0 ) {
			Player.constantForce.force = new Vector3( 0, -gravityForce, 0 );
		} else if( gravityState == 1 ){
			Player.constantForce.force = new Vector3( gravityForce, 0, 0 );
		} else if( gravityState == 2 ){
			Player.constantForce.force = new Vector3( 0, gravityForce, 0 );
		} else if( gravityState == 3 ){
			Player.constantForce.force = new Vector3( -gravityForce, 0, 0 );
		}
		//Player.GetComponent<Player>().UpdateCollisions();
	}
	
	void setDirection() {
		for( int i = 0; i < Input.touches.Length; i++ ) {
			if( Input.touches[i].fingerId == idTouchPlatform ) {
				if( lastPoint == Vector2.zero )
					lastPoint = Input.touches[i].position;
		    	//Vector3 temp = Camera.main.ScreenToWorldPoint( touch.position );
				Vector2 temp = Input.touches[i].position;
				//Debug.Log( lastPoint - temp );
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
		if( jumpAsked ) {
			mustJumpNormal = true;
		}
		return;
	}
	
	void JumpNormal() {
		if( gravityState == 0 && Player.rigidbody.velocity.y < seuil ){
			Player.rigidbody.AddForce( 0, JumpAcc, 0, ForceMode.Acceleration );
		} else if( gravityState == 1 && Player.rigidbody.velocity.x < seuil ){
			Player.rigidbody.AddForce( -JumpAcc, 0, 0, ForceMode.Acceleration );
		} else if( gravityState == 2 && Player.rigidbody.velocity.y > -seuil ){
			Player.rigidbody.AddForce( 0, -JumpAcc, 0, ForceMode.Acceleration );
		} else if( gravityState == 3 && Player.rigidbody.velocity.x > -seuil ){
			Player.rigidbody.AddForce( JumpAcc, 0, 0, ForceMode.Acceleration );
		}
		
	}
	
}
