using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float duration= 4f;
	private float timeLeft;
	public float speed = 5f;
	public int direction = 1;
	public bool vertical = false;
	public float growth = 1.05f;

	// Use this for initialization
	void Start () {
		timeLeft = duration;
	
	}
	
	// Update is called once per frame
	void Update () {
		if( !vertical )
			transform.Translate( Time.deltaTime * speed * direction, 0, 0 );
		else 
			transform.Translate( 0, Time.deltaTime * speed * direction, 0 );
		timeLeft -= Time.deltaTime;
		if( timeLeft < 0 ){
			Destroy( gameObject );
		}
		//transform.localScale = new Vector3( transform.localScale.x + transform.localScale.x * growth * Time.deltaTime, transform.localScale.y + transform.localScale.y * growth * Time.deltaTime, transform.localScale.z );
	}
	
    void OnBecameInvisible() {
        Destroy( gameObject );
    }
	
	
    void OnTriggerEnter(Collider other) {
		if( other.gameObject.tag == "platform" ) {
			other.gameObject.GetComponent<platform>().triggered = true;
			if( !vertical )
				other.gameObject.GetComponent<platform>().directionV = direction;
			else
				other.gameObject.GetComponent<platform>().directionH = -direction;
        	Destroy(other.gameObject);
		} else if( other.gameObject.tag == "ennemi" ) {
			Destroy( other.gameObject );
			Destroy( gameObject );
		}
    }
	
	
}
