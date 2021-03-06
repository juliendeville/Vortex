using UnityEngine;
using System.Collections;

public class controls : MonoBehaviour {
	
	//player
	public Camera theCamera;
	
	//visual Element
	public GameObject Player;
	
	//max speeds
	public float HorizontalMaxVelocity = 5f;
	public float HorizontalAerialMaxVelocity = 5f;
	public float JumpMaxVelocity = 5f;
	
	
	//with jumping
	public int nbJumpMax = 5;
	public float timeJumpingMax = 0.5f;
	private float timeJumpingLeft;
	public float cooldownJumping = 0.3f;
	private float cooldownJumpingLast = 0;
	public float ratioPerJump = 0.3f;
	int nbJumpLeft = 5;
	bool air = false;
	bool wasAir = false;
	bool jumping = false;
	bool mustJumpNormal = false;
	bool mustJumpInAir = false;
	float addHorizontalForce = 0;
	
	//collisiton platforme
	public float seuil = 0.31f;
	public float smoothingFactor = 0.5f;
	
	public Bullet bullet;
	public BigBullet bigBullet;
	
	// Use this for initialization
	void Start () {
		//iTween.Init( gameObject );
		
		Ground();
	}
	
	void FixedUpdate() {
		if( mustJumpNormal ){
			JumpNormal();
			mustJumpNormal = false;
		}
		if( mustJumpInAir ){
			JumpInAir();
			mustJumpInAir = false;
		}
		if( air /*&& rigidbody.velocity.y < 0*/ ) {
			//rigidbody.velocity = new Vector3( addHorizontalForce * HorizontalAerialMaxVelocity, rigidbody.velocity.y, 0 );
			//rigidbody.AddForce( ( addHorizontalForce * HorizontalAerialMaxVelocity - rigidbody.velocity.x )/Time.fixedDeltaTime, 0,0);
			Vector3 tvel = new Vector3( addHorizontalForce * HorizontalAerialMaxVelocity, rigidbody.velocity.y, 0 );
			rigidbody.velocity = Vector3.Lerp( rigidbody.velocity, tvel, Time.deltaTime * smoothingFactor);
			//rigidbody.AddForce( ( addHorizontalForce * HorizontalMaxVelocity - rigidbody.velocity.x )/Time.fixedDeltaTime, 0,0);
			//if( addHorizontalForce * HorizontalAerialMaxVelocity > 0.1 || addHorizontalForce * HorizontalAerialMaxVelocity < -0.1 )
				//Debug.Log( rigidbody.velocity.x );
		} else {
			
			Vector3 tvel = new Vector3( addHorizontalForce * HorizontalMaxVelocity, rigidbody.velocity.y, 0 );
			rigidbody.velocity = Vector3.Lerp( rigidbody.velocity, tvel, Time.deltaTime * smoothingFactor);
			//rigidbody.AddForce( ( addHorizontalForce * HorizontalMaxVelocity - rigidbody.velocity.x )/Time.fixedDeltaTime, 0,0);
			//if( addHorizontalForce * HorizontalMaxVelocity > 0.1 || addHorizontalForce * HorizontalMaxVelocity < -0.1 )
				//Debug.Log( rigidbody.velocity.x );
		}
		CanJump();
		CanMove();
	}
	
	// Update is called once per frame
	void Update () {
		/*if( transform.position.y >10 )*/
		/*else
			theCamera.transform.position = new Vector3( 0, 9, -15 );
		*/
		/*
		if( theCamera != null ) {
			myPoints = theCamera.GetComponent<DrawPath>().myPoints;
			bool endPoint = theCamera.GetComponent<DrawPath>().endPoint;
		    if ( myPoints != null && myPoints.Length > 0 ) {
				//disable gravity
				rigidbody.useGravity = false;
				rigidbody.velocity = Vector3.zero;
				if( endPoint ) {
					if( pathId < 0 || (myPoints[pathId] - transform.position).sqrMagnitude< 0.01 ){
						pathId++;
							
						if( pathId >= myPoints.Length ){
							theCamera.GetComponent<DrawPath>().myPoints = null;
							theCamera.GetComponent<DrawPath>().endPoint = false;
							endPoint = false;
							pathId = -1;
						}
					}
					if( endPoint ) {
						//transform.position = new Vector3( transform.position.x, transform.position.y + 0.1f, 0f);
						transform.position = Vector3.MoveTowards( transform.position, myPoints[pathId], 0.1f);
					}
				} else {
					pathId = -1;
				}
			} else {
				theCamera.transform.position = Vector3.Lerp( theCamera.transform.position, new Vector3( transform.position.x, 2 + transform.position.y, -20 ),0.2f );
				rigidbody.useGravity = true;
				pathId = -1;
			}
			
			if( (Input.touchCount == 2 && ( Input.touches[0].phase == TouchPhase.Stationary || Input.touches[1].phase == TouchPhase.Stationary )) || !air ) {
			}
			//enable gravity when movement ended
		}
		*/
	}
	
	/*
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
			temp = new Vector3( ( temp.x ),( temp.y ), 0 );//Mathf.Round( temp.x ), Mathf.Round( temp.y ), 0 );
			if( temp.x == tempPoints[ tempPoints.Length - 1 ].y && temp.y == tempPoints[ tempPoints.Length - 1 ].y ) {
				//do nothing
			} else {
				tempPoints[j] = temp;
			    myPoints = new Vector3[ tempPoints.Length ]; 
			    myPoints = tempPoints; 
			}
		}
	}
	
	*/
	void OnDrawGizmos(){
		
		if( theCamera != null ) {
			
			
			Vector3[] myPoints = theCamera.GetComponent<DrawPath>().myPoints;
		    if ( myPoints != null && myPoints.Length > 0 )
		    {
				//CRSpline.GizmoDraw (myPoints, 100f);
				iTween.DrawPath(myPoints, Color.magenta);
			}
		}
		
	}
	
	void CanJump(){
		if( cooldownJumpingLast <= 0 ) {
			if( Input.GetAxis("Vertical") != 0 && timeJumpingLeft > 0 && nbJumpLeft > 0 ) {
				if( !jumping ) {
					wasAir = air;
				} else {
					timeJumpingLeft -= Time.fixedDeltaTime;
				}
				if( !wasAir ) {
					mustJumpNormal = true;
				}
				else {
					mustJumpInAir = true;
				}
				jumping = true;
			} else {
				if( air && jumping ) {
					nbJumpLeft -= 1;
					cooldownJumpingLast = cooldownJumping;
				}
				timeJumpingLeft = timeJumpingMax;
				jumping = false;
			}
		} else {
			cooldownJumpingLast -= Time.fixedDeltaTime;
		}
	}
	
	void OnCollisionEnter( Collision theCollision ){
		theCamera.GetComponent<DrawPath>().myPoints = null;
		theCamera.GetComponent<DrawPath>().endPoint = false;
		Debug.Log("Collision");
		
	    if(theCollision.gameObject.name == "floor" ||theCollision.gameObject.name == "fixe" || theCollision.gameObject.tag == "platform" )
	    {	
			bool haut = false;
			bool bas = false;
			bool gauche = false;
			bool droite = false;
			float posY = transform.position.y + transform.localScale.y / 2 - theCollision.gameObject.transform.position.y + theCollision.gameObject.transform.localScale.y / 2;
			if( posY >= 0 && posY < seuil ) {
				bas = true;
			}
			posY -= transform.localScale.y + theCollision.gameObject.transform.localScale.y;
			if( posY <= 0 && posY > -seuil ) {
				haut = true;
			}
			float posX = transform.position.x + transform.localScale.x / 2 - theCollision.gameObject.transform.position.x + theCollision.gameObject.transform.localScale.x / 2;
			if( posX >= 0 && posX < seuil ) {
				gauche = true;
			}
			posX -= transform.localScale.x + theCollision.gameObject.transform.localScale.x;
			if( posX <= 0 && posX > -seuil ) {
				droite = true;
			}
			if( theCollision.gameObject.tag == "platform" )
				Debug.Log( "haut" + (haut?1:0) + "bas" + (bas?1:0) + "gauche" + (gauche?1:0) + "droite" + (droite?1:0) );
			
			if( ( haut && gauche ) || ( haut && droite ) ) {
				gauche = false;
				droite = false;
			}
			if( ( bas && gauche ) || ( bas && droite ) ) {
				bas = false;
			}
			/*
			if( gauche ) {
				Vector3 posBullet = theCollision.gameObject.transform.position;
				posBullet.x += theCollision.gameObject.transform.localScale.x / 2;
	        	Instantiate(bullet, posBullet,  Quaternion.identity);
				
				Destroy( theCollision.gameObject );
			}
			if( droite ) {
				Vector3 posBullet = theCollision.gameObject.transform.position;
				posBullet.x -= theCollision.gameObject.transform.localScale.x / 2;
	        	Bullet balle = Instantiate(bullet, posBullet,  Quaternion.identity) as Bullet;
				balle.direction = -1;
				
				Destroy( theCollision.gameObject );
			}
			*/
			if( haut ) {
	        	Ground();
			}
			/*
			if( bas ) {
				theCollision.gameObject.GetComponent<platform>().triggered = true;
				Destroy( theCollision.gameObject );
			}
			*/
	    }
	    
	}
	 
	//consider when character is jumping .. it will exit collision.
	void OnCollisionExit( Collision theCollision ){
	    if(theCollision.gameObject.name == "floor" || theCollision.gameObject.tag == "platform" )
	    {
	        air = true;
	    }
	}
	
	void CanMove(){
		addHorizontalForce = Input.GetAxis("Horizontal");
	}
	

	
	// called each time touches ground
	void Ground() {
		nbJumpLeft = nbJumpMax;
		air = false;
		timeJumpingLeft = timeJumpingMax;
		cooldownJumpingLast = 0;
	}
	
	void JumpNormal() {
		rigidbody.AddForce(0,(JumpMaxVelocity - rigidbody.velocity.y)/Time.fixedDeltaTime,0,ForceMode.Acceleration);
	}
	
	void JumpInAir() {
		rigidbody.AddForce(0,(JumpMaxVelocity - rigidbody.velocity.y)/Time.fixedDeltaTime * (nbJumpMax-nbJumpLeft+1) * ratioPerJump,0,ForceMode.Acceleration);
		//air = true;
	}
}
