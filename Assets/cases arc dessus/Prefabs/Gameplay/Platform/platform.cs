using UnityEngine;
using System.Collections;

public class platform : MonoBehaviour {
	
	private GameObject background;
	private DrawPath gestion;
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
		
		gestion = Camera.main.GetComponent<DrawPath>();
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
				Bullet balle;
				/*
				int gravity = gestion.gravityState;
				if( gravity == 1 || gravity == 3 ){
					int lastH = directionH;
					directionH = -directionV;
					directionV = lastH;
				}
				if( gravity == 2 || gravity == 3 ){
					directionH *= -1;
					directionV *= -1;
				}
				*/
				if( directionH != 0 ) {
					//posBullet.x += directionH * transform.localScale.x / 2;
		        	balle = Instantiate(bullet, posBullet,  Quaternion.identity) as Bullet;
					balle.direction = directionH;
				} else {
					//posBullet.y += directionV * transform.localScale.y / 2;
			    	balle = Instantiate(bullet, posBullet,  Quaternion.identity) as Bullet;
					balle.direction = directionV;
					balle.vertical = true;
				}
				Debug.Log( "dir : " + balle.direction );
				balle.transform.localScale = transform.localScale;
				balle.transform.rotation = transform.rotation;
			}
	   }
    }
	
	void OnApplicationQuit()
	{
	}
}
