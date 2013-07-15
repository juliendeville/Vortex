using UnityEngine;
using System.Collections;

public class AnimationTexture : MonoBehaviour {

	//vars for the whole sheet
	public int colCount;
	public int rowCount;
	public float  fps;
	public Material materiau;
	
	void Start() {
		if( renderer.sharedMaterial != null )
			materiau = renderer.material;
	}
	 
	//vars for animation
	private int totalCells = 0;
	
	//Maybe this should be a private var
    private Vector2 offset;
	private int[] indexes;
	private int index;
	
	//SetSpriteAnimation
	public void Play( int[] frames ){	
		CancelInvoke();
	 	indexes = frames;
		index = 0;	
		InvokeRepeating( "_play", 0.01f, 1/ fps );
	}
	
	public void Stop() {
		CancelInvoke();
	}

	public void _play() {
	    // Repeat when exhausting all cells
		index = index % indexes.Length;
	    // Calculate index
	    int id  = indexes[index];
		index++;
		
	 	SetFrame( id );
	}
	
	//SetSpriteKey
	public void SetFrame( int frame ){
		if( totalCells == 0 )
			totalCells = colCount * rowCount;
	 
	    // Calculate index
	    int index = frame % totalCells;
	 
	    // Size of every cell
	    float sizeX = 1.0f / colCount;
	    float sizeY = 1.0f / rowCount;
	    Vector2 size =  new Vector2(sizeX,sizeY);
	 
	    // split into horizontal and vertical index
	    var uIndex = index % colCount;
	    var vIndex = index / colCount;
	 
	    // build offset
	    // v coordinate is the bottom of the image in opengl so we need to invert.
	    float offsetX = uIndex * size.x;
	    float offsetY = (1.0f - size.y) - vIndex * size.y;
	    Vector2 offset = new Vector2(offsetX,offsetY);
	 
	    materiau.SetTextureOffset ("_MainTex", offset);
	    materiau.SetTextureScale  ("_MainTex", size);
	}
}
