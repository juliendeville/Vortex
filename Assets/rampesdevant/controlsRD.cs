using UnityEngine;
using System.Collections;

public class controlsRD : MonoBehaviour {
	
	//player
	public Camera theCamera;
	
	//visual Element
	public GameObject Player;
	
	//with movement
	public float MaxSpeed = 5f;
	public float FallSpeed = 5f;
	public float horizontalAcc = 15f;
	
	//with jumping
	public int nbJumpMax = 5;
	public float timeJumpingMax = 0.5f;
	private float timeJumpingLeft;
	public float cooldownJumping = 0.2f;
	private float cooldownJumpingLast = 0;
	public float JumpSpeed = 5f;
	public float JumpAcc = 30f;
	int nbJumpLeft = 5;
	public float ratioPerJump = 1.1f;
	bool air = false;
	bool wasAir = false;
	bool jumping = false;
	bool mustJumpNormal = false;
	bool mustJumpInAir = false;
	float addHorizontalForce = 0;
	

	// Use this for initialization
	void Start () {
		Ground();
	}
	
	void FixedUpdate() {
		if( rigidbody.velocity.x > MaxSpeed ){
			rigidbody.velocity = new Vector3( MaxSpeed, rigidbody.velocity.y, 0 );
		}
		if( rigidbody.velocity.x < -MaxSpeed ){
			rigidbody.velocity = new Vector3( -MaxSpeed, rigidbody.velocity.y, 0 );
		}
		if( rigidbody.velocity.y > JumpSpeed ){
			rigidbody.velocity = new Vector3( rigidbody.velocity.x, JumpSpeed, 0 );
		}
		if( rigidbody.velocity.y < -FallSpeed ){
			rigidbody.velocity = new Vector3( rigidbody.velocity.x, -FallSpeed, 0 );
		}
		if( mustJumpNormal ){
			JumpNormal();
			mustJumpNormal = false;
		}
		if( mustJumpInAir ){
			JumpInAir();
			mustJumpInAir = false;
		}
		rigidbody.AddForce(addHorizontalForce * horizontalAcc, 0,0);
	}
	
	// Update is called once per frame
	void Update () {
		CanJump();
		CanMove();
		if( transform.position.y >10 )
			theCamera.transform.position = new Vector3( 0, 9 + transform.position.y - 10, -15 );
		else
			theCamera.transform.position = new Vector3( 0, 9, -15 );
	}
	
	void CanJump(){
		if( cooldownJumpingLast <= 0 ) {
			if( Input.GetAxis("Vertical") != 0 && timeJumpingLeft > 0 && nbJumpLeft > 0 ) {
				if( !jumping ) {
					wasAir = air;
				} else {
					timeJumpingLeft -= Time.deltaTime;
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
			cooldownJumpingLast -= Time.deltaTime;
		}
	}
	
	void OnCollisionEnter( Collision theCollision ){
		Debug.Log("collision");
		
	    if(theCollision.gameObject.name == "floor" || theCollision.gameObject.tag == "platform" )
	    {
	        Ground();
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
		//acceleration = Vector2.Lerp( acceleration, GravityAcc, Freinage);
		//speed += acceleration * Time.deltaTime;
		//speed += new Vector2( Input.GetAxis("Horizontal") * horizontalAcc * Time.deltaTime, 0 );
		//transform.Translate(speed * Time.deltaTime);
		
		addHorizontalForce = Input.GetAxis("Horizontal");
	}
	

	
	// called each time touches ground
	void Ground() {
		nbJumpLeft = nbJumpMax;
		//Debug.Log( nbJumpLeft );
		air = false;
		timeJumpingLeft = timeJumpingMax;
	}
	
	void JumpNormal() {
		rigidbody.AddForce(0,JumpAcc,0);
		//acceleration.y += GroundJumpAcc;
		//air = true;
	}
	
	void JumpInAir() {
		rigidbody.AddForce(0,JumpAcc * (nbJumpMax-nbJumpLeft+1) * ratioPerJump,0);
		//air = true;
	}
}
