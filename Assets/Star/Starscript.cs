using UnityEngine;
using System.Collections;

public class Starscript : MonoBehaviour {
	
	/* BLACK HOLES */
	//boolean determined by having this powerup
	public static bool BLACK_HOLE_HELPER = false;
	//boolean to determine if star is black hole
	public bool isBlackHole = false;	
	//black hole prefab
	public GameObject blackHole;
	//black hole helper prefab -- used to highlight black holes in powerup
	public GameObject BHhelper;
	//actual black hole and helper objects instantiated
	public GameObject b, h;
	
	/* MOVING STARS */
	//true if moving star
	public bool is_moving = false;
	//if moving, the direction to move in 
	public Vector3 dir = new Vector3(0,0,0);
	//speed of move
	public float speed = 0;

	/* REVOLVING STARS */
	//is revolving 
	public bool is_revolving = false;
	//revolution point
	public Vector3 rpoint;
	//revolution speed
	public float rspeed;
	
	/* ALL STARS */
	//light effect for star radius	
	public GameObject radius;
	public GameObject lightGameObject;
	//actual radius object instantiated
	public GameObject r;
	//star properties
	public Color c;
	public Texture t;
	public float starSize = 15f;
	public float orbitRadius;
	public float duration = 20f;
	public float offset;
	public float random;
	//is winner
	public bool is_sink = false;	
	//position at last frame
	public Vector3 last_position;
	//true if in level editor, so stars don't move
	public bool editor_freeze = false;
	
	void Start () {
		//if the star is a black hole, instantiate cylinder to represent the black hole
		if (isBlackHole) {
			b = Instantiate(blackHole, new Vector3 (this.transform.position.x, this.transform.position.y, 100f), new Quaternion (0, 0, 0, 0)) as GameObject;		
			b.transform.localScale *= starSize;
			b.transform.Rotate(90,0,0);
			//decrease size of star to represent the center of a black hole
			this.transform.localScale /= 10;			
			//parent black hole object to star
			b.transform.parent = this.transform;			
			//if powerup is selected, instantated black hole helper object behind the black hole object at the same size and scale
			if (Manager.BLACK_HOLE_HELPER || BLACK_HOLE_HELPER) {
				h = Instantiate(BHhelper, new Vector3 (this.transform.position.x, this.transform.position.y, 100f), new Quaternion (0, 0, 0, 0)) as GameObject;		
				h.transform.localScale *= starSize;
				h.transform.Rotate(90,0,0);
				//parent helper object to star 
				h.transform.parent = this.transform;
			}
		}
		//if the star is not a black hole, scale it to size specified
		else  	
			this.transform.localScale *= starSize;
		//radius of learth's entry is the size of the star	
		orbitRadius = starSize;
		//instantiate light halo at star's position the size of the orbit radius including radial error 
		r = Instantiate(radius, new Vector3 (this.transform.position.x, this.transform.position.y, 100f), new Quaternion (0, 0, 0, 0)) as GameObject;
		r.light.range = 2*orbitRadius + 2*Manager.RADIAL_ERROR;
		//parent radius to star for destruction
		r.transform.parent = this.transform;
		//random value for star's random rotation
		random = Random.value;		
	}
	
	void Update() {
		//rotate star on its axis at a fixed random rate
		transform.RotateAround(this.transform.position, Vector3.forward, 50*Time.deltaTime*random);
		//texture and color star
		renderer.material.mainTexture = t;
        r.light.color = c;
		//make star glow and pulse 
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
		}

	}
	
	
	void OnCollisionStay(Collision c)
	{
		//if colliding with a space bomb explosion, move away from the center of the explosion	
		if(c.transform.name == "space_bomb_range(Clone)")
		{
			transform.Translate(200 * Time.deltaTime*(transform.position - c.transform.position).normalized,Space.World);
		}
	}
}
