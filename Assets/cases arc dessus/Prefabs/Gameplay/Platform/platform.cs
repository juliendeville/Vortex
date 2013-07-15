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
		background = GameObject.FindWithTag("background");
	}
	
	// Update is called once per frame
	void Update () {
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
				
				if( directionH != 0 ) {
		        	balle = Instantiate(bullet, posBullet,  Quaternion.identity) as Bullet;
					balle.direction = directionH;
				} else {
			    	balle = Instantiate(bullet, posBullet,  Quaternion.identity) as Bullet;
					balle.direction = directionV;
					balle.vertical = true;
				}
				//Debug.Log( "dir : " + balle.direction );
				balle.transform.localScale = transform.localScale;
				balle.transform.rotation = transform.rotation;
			}
	   }
    }
	
	void OnApplicationQuit()
	{
	}
}
