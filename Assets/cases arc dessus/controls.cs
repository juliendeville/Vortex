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
	
	public Bullet bullet;
	public BigBullet bigBullet;
	
	

	// Use this for initialization
	void Start () {
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
			rigidbody.AddForce( ( addHorizontalForce * HorizontalAerialMaxVelocity - rigidbody.velocity.x )/Time.fixedDeltaTime, 0,0);
		} else {
			rigidbody.AddForce( ( addHorizontalForce * HorizontalMaxVelocity - rigidbody.velocity.x )/Time.fixedDeltaTime, 0,0);
			if( addHorizontalForce * HorizontalAerialMaxVelocity - rigidbody.velocity.x > 0.1 || addHorizontalForce * HorizontalAerialMaxVelocity - rigidbody.velocity.x < -0.1 )
				Debug.Log( addHorizontalForce * HorizontalAerialMaxVelocity - rigidbody.velocity.x );
		}
		CanJump();
		CanMove();
	}
	
	// Update is called once per frame
	void Update () {
		/*if( transform.position.y >10 )*/
			theCamera.transform.position = new Vector3( 0, 9 + transform.position.y - 10, -15 );
		/*else
			theCamera.transform.position = new Vector3( 0, 9, -15 );
		*/
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
		
	    if(theCollision.gameObject.name == "floor" || theCollision.gameObject.tag == "platform" )
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
			if( haut ) {
	        	Ground();
			}
			if( bas ) {
				theCollision.gameObject.GetComponent<platform>().triggered = true;
				Destroy( theCollision.gameObject );
			}
			
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
		rigidbody.AddForce(0,(JumpMaxVelocity - rigidbody.velocity.y)/Time.fixedDeltaTime,0);
		Debug.Log("Jump");
	}
	
	void JumpInAir() {
		rigidbody.AddForce(0,(JumpMaxVelocity - rigidbody.velocity.y)/Time.fixedDeltaTime * (nbJumpMax-nbJumpLeft+1) * ratioPerJump,0);
		//air = true;
	}
}
