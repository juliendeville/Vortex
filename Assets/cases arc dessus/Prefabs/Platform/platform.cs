using UnityEngine;
using System.Collections;

public class platform : MonoBehaviour {
	
	private GameObject background;
	public BigBullet bigBullet;
	
	//public float timeBeforeDestroy = 5f;
	//private float timeLeft;
	
	public bool triggered = false;
	
	

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
		Debug.Log( "invisible");
        Destroy( gameObject );
    }
	
	
	void OnDestroy () {
		if (Application.isPlaying) {
			background.GetComponent<Pouvoirs>().nbPlatforms--;
			Debug.Log( "destroyed");
			if( triggered ) {
				//destroyed
				Vector3 posBullet = transform.position;
				posBullet.y += transform.localScale.y / 2;
		    	Instantiate(bigBullet, posBullet,  Quaternion.identity);
			}
	   }
    }
	
	void OnApplicationQuit()
	{
	}
}
