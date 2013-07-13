using UnityEngine;
using System.Collections;

public class platform : MonoBehaviour {
	
	private GameObject background;
	
	//public float timeBeforeDestroy = 5f;
	//private float timeLeft;
	
	public bool triggered = false;
	public int directionV = 0;
	public int directionH = 0;
	
	public Bullet bullet;
	public BigBullet bigBullet;
	

	// Use this for initialization
	void Start () {
		//timeLeft = timeBeforeDestroy;
		background = GameObject.FindWithTag("background");
	}
	
	// Update is called once per frame
	void Update () {
		/*
		timeLeft -= Time.deltaTime;
		if( timeLeft < 0 ){
			triggered = true;
			Destroy( gameObject );
		}
		
		*/
	}
	
    void OnBecameInvisible() {
        Destroy( gameObject );
    }
	
	
	void OnDestroy () {
		if (Application.isPlaying && background != null) {
			Pouvoirs pouv = background.GetComponent<Pouvoirs>();
			if( pouv != null )
				pouv.nbPlatforms--;
			if( triggered ) {
				//destroyed
				Vector3 posBullet = transform.position;
				if( directionH != 0 ) {
					posBullet.x += directionH * transform.localScale.x / 2;
		        	Bullet balle = Instantiate(bullet, posBullet,  Quaternion.identity) as Bullet;
					balle.direction = directionH;
				} else {
					posBullet.y += directionV * transform.localScale.y / 2;
			    	BigBullet balle = Instantiate(bigBullet, posBullet,  Quaternion.identity) as BigBullet;
					balle.direction = directionV;
				}
			}
	   }
    }
	
	void OnApplicationQuit()
	{
	}
}
