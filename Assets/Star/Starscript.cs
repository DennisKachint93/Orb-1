using UnityEngine;
using System.Collections;

public class Starscript : MonoBehaviour {
	
	public float orbitRadius;
	public GameObject radius;
	public GameObject r, b;
	public Color c;
	public Texture t;
	public float starSize = 15f;
	public GameObject lightGameObject;
	public GameObject blackHole;
	public float duration = 20f;
	public float offset;
	public float random;
	//black-hole related variables
	public bool isBlackHole = false;
	public float theta;
	public float currRadius;
	//true if moving star
	public bool is_moving = false;
	//if moving, the direction to move in 
	public Vector3 dir = new Vector3(0,0,0);
	//speed of move
	public float speed = 0;
	//is winner
	public bool is_sink = false;
	//is revolving 
	public bool is_revolving = false;
	//revolution point
	public Vector3 rpoint;
	//revolution speed
	public float rspeed;
	//position at last frame
	public Vector3 last_position;
	//true if in level editor, so stars don't move
	public bool editor_freeze = false;
	
	void Start () {
	theta = 720*Mathf.PI/180;
	currRadius = starSize;
		if (isBlackHole) {
			b = Instantiate(blackHole, new Vector3 (this.transform.position.x, this.transform.position.y, 100f), new Quaternion (0, 0, 0, 0)) as GameObject;		
			b.transform.localScale *= starSize;
			b.transform.Rotate(90,0,0);
			this.transform.localScale /= 10;
		}
		else  	
			this.transform.localScale *= starSize;
		orbitRadius = starSize;
		r = Instantiate(radius, new Vector3 (this.transform.position.x, this.transform.position.y, 100f), new Quaternion (0, 0, 0, 0)) as GameObject;
		r.light.range = 2*orbitRadius;
		//+ 1/2 radial error
		random = Random.value;
		//parent radius to star for destruction
		r.transform.parent = this.transform;
	}
	
	void Update() {
		transform.RotateAround(this.transform.position, Vector3.forward, 50*Time.deltaTime*random);
		renderer.material.mainTexture = t;
        	r.light.color = c;
		//make stars glow
		offset = starSize/5;
        	float phi = Time.time / duration + offset;
        	float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;
        	r.light.intensity = amplitude*offset + starSize/30;
        
		//if star is a mover, the actual game is playing, and the star is visible move to destination point
		if(is_moving && !editor_freeze && renderer.isVisible)
		{
			transform.position += speed*new Vector3(dir.x*Time.deltaTime,dir.y*Time.deltaTime,0);
		}
		
		//if star revolves, rotate around the specified point
		if(is_revolving && !editor_freeze)
		{
			transform.RotateAround(rpoint,Vector3.forward,Time.deltaTime*rspeed);
			
			//maintain last position here so learth entry works
			if(!Manager.cur_star.Equals(gameObject))
				last_position = transform.position;
		}

	}
	
}
