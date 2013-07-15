using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
	
	public int wealth = 10;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void OnTriggerEnter(Collider other) {
		if( other.gameObject.name == "joueur" ) {
			Destroy( gameObject );
		}
    }
	
	void OnDestroy () {
		if (Application.isPlaying && Camera.mainCamera != null && Camera.mainCamera.GetComponent<Score>() != null) {
			Camera.mainCamera.GetComponent<Score>().score += wealth;
		}
	}
}
