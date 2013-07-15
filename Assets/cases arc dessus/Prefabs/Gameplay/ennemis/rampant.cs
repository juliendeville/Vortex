using UnityEngine;
using System.Collections;

public class rampant : MonoBehaviour {
	public Transform[] chemin = new Transform[2];
	private int pathId = 0;
	public float speed = 0.1f;
	private AnimationTexture anim;
	public int wealth = 20;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<AnimationTexture>();
		//anim.Play( new int[4] { 4, 3, 2, 3 } );
		anim.Play( new int[2] { 4, 3 } );
	}
	
	// Update is called once per frame
	void Update () {
		if( pathId < 0 || (chemin[pathId].position - transform.position).sqrMagnitude< 0.01 ){
			pathId++;
			if( pathId % 2 == 0 ) {
				//anim.Play( new int[4] { 4, 3, 2, 3 } );
				anim.Play( new int[2] { 4, 3 } );
			} else {
				//anim.Play( new int[4] { 5, 6, 7, 6 } );
				anim.Play( new int[2] { 5, 6 } );
			}
		}
		if( pathId >= chemin.Length ){
			pathId = 0;
		}
		transform.position = Vector3.MoveTowards( transform.position, chemin[pathId].position, speed);
	}
	
	
	void OnDestroy () {
		if (Application.isPlaying) {
			Camera.mainCamera.GetComponent<Score>().score += wealth;
		}
	}
}
