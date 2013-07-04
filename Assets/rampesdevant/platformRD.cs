using UnityEngine;
using System.Collections;

public class platformRD : MonoBehaviour {
	
	private GameObject background;
	
	public float timeBeforeDestroy = 5f;
	private float timeLeft;
	
	

	// Use this for initialization
	void Start () {
		timeLeft = timeBeforeDestroy;
		background = GameObject.FindWithTag("background");
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if( timeLeft < 0 ){
			background.GetComponent<PouvoirsRD>().nbPlatforms--;
			Destroy( gameObject );
		}
	}
}
