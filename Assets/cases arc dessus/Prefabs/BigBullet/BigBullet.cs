using UnityEngine;
using System.Collections;

public class BigBullet : MonoBehaviour {
	
	public float duration= 4f;
	private float timeLeft;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		timeLeft = duration;
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate( 0, Time.deltaTime * speed,0);
		timeLeft -= Time.deltaTime;
		if( timeLeft < 0 ){
			Destroy( gameObject );
		}
	}
	
    void OnTriggerEnter(Collider other) {
		if( other.gameObject.tag == "platform" ) {
        	Destroy(other.gameObject);
		}
    }
	
	
}
