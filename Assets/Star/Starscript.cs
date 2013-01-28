using UnityEngine;
using System.Collections;

public class Starscript : MonoBehaviour {
	
	/* BLACK HOLES */
	//boolean determined by having this powerup
	public static bool BLACK_HOLE_HELPER = false;
	//boolean to determine if star is black hole
	public bool isBlackHole = false;
	//true if star is being sucked into a black hole
	public bool spiral = false;
	//black hole prefab
	public GameObject blackHole;
	//black hole helper prefab -- used to highlight black holes in powerup
	public GameObject BHhelper;
	//center of black hole to detect collision -- i know, this is getting ridiculous
	public GameObject BHcenter;
	//actual black hole and helper objects instantiated
	public GameObject b, h, center;
	//when spiraling into black hole, lastPos identifies position before the incident
	public Vector3 lastPos;
	//identifies direction of spiral into black hole
	public bool clockwise;
	//identifies which black hole star is spiraling into
	public GameObject BHspiral;
	//how fast a star will get sucked into a black hole
	public float BLACK_HOLE_SUCKINESS = 150;
	//speed when entering a black hole
	public float speed_of_entry;
	//true if black hole has been found so that spiral effect is activated
	public bool black_hole_active = false;
	//line effect for black hole
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line, line;
	
	
	/* MOVING STARS */
	//true if moving star
	public bool is_moving = false;
	//if moving, the direction to move in 
	public Vector3 dir = new Vector3(0,0,0);
	//speed of move
	public float speed = 0;
	//back and forth
	public bool bandf = false;
	//destination
	public Vector3 destination = new Vector3(0, 0, 0);
	//starting location
	public Vector3 start_loc = new Vector3(0, 0, 0);


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
	//light for bulb in planet
	public GameObject bulb_instance;
	//actual radius and bulb objects instantiated
	public GameObject r;
	public GameObject bulb;
	public GameObject collide;
	//star properties
	public Color c;
	public Texture t;
	public float starSize = 15f;
	public float orbitRadius;
	public float duration = 1f;
	public float offset;
	//is winner
	public bool is_sink = false;	
	//is starter
	public bool is_source = false;
	public bool glow = false;
	//position at last frame
	public Vector3 last_position;
	//true if in level editor, so stars don't move
	public bool editor_freeze = false;
	//exploding star related variables
	public bool isExplodingStar=false;
 	public  double  timer=0;
	public double explodetimer=0;
 	public bool  onoff;
	public double resblink=.4;
	public double blinkspeed=.4;
	public double blowuptime=5;
	public Transform red_explosion;
	public Transform orange_explosion;
	public Transform yellow_explosion;
	public Transform green_explosion;
	public Transform blue_explosion;
	public Transform purple_explosion;
	public Transform aqua_explosion;
	
	public int BIG_EXPLOSION = 2;
	public int SMALL_EXPLOSION = 1;
	public int NO_EXPLOSION = 0;
	
	//save first position for level editor
	public Vector3 dir_actual;
	
	//true if destroyed so learth will not return to this star on death
	public bool is_destroyed = false;
	
	//game state
	Game_State gscpt;
	
	//becomes true once hit so that the explosion objected is instantiated only once
	public bool has_been_bombed = false;
	
	
	void Start () {
		
		//line effect for black hole
		line = new GameObject();
		
		GameObject go = GameObject.Find("game_state");
		gscpt = go.GetComponent<Game_State>();
		
		if(bandf){
			dir = (destination - start_loc);
			//don't normalize in the editor or the wrong values will be written to the save file
			if(!editor_freeze)
				dir.Normalize();
		}	
	
		//if the star is a black hole, instantiate cylinder to represent the black hole
		if (isBlackHole) {
			b = Instantiate(blackHole, new Vector3 (this.transform.position.x, this.transform.position.y, 800f), new Quaternion (0, 0, 0, 0)) as GameObject;		
			b.transform.localScale *= starSize;
			b.transform.Rotate(90,0,0);
			center = Instantiate(BHcenter, this.transform.position, new Quaternion (0, 0, 0, 0)) as GameObject;		
			center.transform.localScale *= starSize/5;
			//decrease size of star to represent the center of a black hole
			this.transform.localScale /= this.transform.localScale.x;			
			//parent black hole object to star
			center.transform.parent = this.transform;
			b.transform.parent = this.transform;	
			b.tag = "blackhole";
			//if powerup is selected, instantated black hole helper object behind the black hole object at the same size and scale
			if (Manager.BLACK_HOLE_HELPER || BLACK_HOLE_HELPER) {
				h = Instantiate(BHhelper, new Vector3 (this.transform.position.x, this.transform.position.y, 800f), new Quaternion (0, 0, 0, 0)) as GameObject;		
				h.transform.localScale *= starSize;
				h.transform.Rotate(90,0,0);
				//parent helper object to black hole 
				h.transform.parent = b.transform;
			}
		}
		//if the star is not a black hole, scale it to size specified and tag it as a star
		else
			this.transform.localScale *= starSize;
		//radius of learth's entry is the size of the star	
		if (!is_source && !is_sink) {
			orbitRadius = starSize;
			//instantiate light halo at star's position the size of the orbit radius including radial error 
			r = Instantiate(radius, new Vector3 (this.transform.position.x, this.transform.position.y, 90f), new Quaternion (0, 0, 0, 0)) as GameObject;
			r.light.range = 2*orbitRadius + 2*Manager.RADIAL_ERROR;
			//parent radius to star for destruction
			r.transform.parent = this.transform;
			//collider for raycasting to detect if star is tangent 	
			r.light.range = 2*orbitRadius + 2*Manager.RADIAL_ERROR;
			GameObject collider = Instantiate(collide, this.transform.position, new Quaternion (0, 0, 0, 0)) as GameObject;
			collider.transform.localScale *= r.light.range;//+ 3*Manager.RADIAL_ERROR;
			collider.transform.parent = this.transform;
			//do the same for light bulb effect
		//	bulb = Instantiate(bulb_instance, new Vector3 (this.transform.position.x, this.transform.position.y, 90f), new Quaternion (0, 0, 0, 0)) as GameObject;
		//	bulb.light.range = starSize;
		//	bulb.light.intensity = 8f;
			this.renderer.material.color = c;
		//	bulb.transform.parent = this.transform;
		}
		else if (is_source)
			orbitRadius = 110;
		else 
			orbitRadius = 0;
	
		r.light.color = c;
		if (c == Manager.orange) 
			r.light.intensity = 2f;
		else if (c == Manager.purple)
			r.light.intensity = 3f;
		else if (c == Manager.aqua)
			r.light.intensity = 3f;
		else
       		r.light.intensity = 3.5f;
	}
	
	void Update() {
		//gradually increase intensities of lights
		if (c == Manager.orange && r.light.intensity < 2f) {
				r.light.intensity += .001f;
			//	print (r.light.intensity);
		}
		else if (c == Manager.purple && r.light.intensity < 4f) 
				r.light.intensity += .002f;
		else if (r.light.intensity < 3.5f) 
				r.light.intensity += .00175f;
		
		this.renderer.material.color = c;
		//texture and color star
		//renderer.material.color = c;
		renderer.light.color = c;
		//renderer.material.mainTexture = t;
		if (glow) 
			renderer.material.shader = Shader.Find("Reflective/Bumped Specular");
		else
			renderer.material.shader = Shader.Find("Specular");
		 
		//if star is a mover, the actual game is playing, and the star is visible move to destination point
		if(is_moving && !editor_freeze /* && (renderer.isVisible */|| bandf)
		{
			transform.position += speed*new Vector3(dir.x*Time.deltaTime,dir.y*Time.deltaTime,0);
		}
		
		//
		if(bandf && (start_loc.x > destination.x && transform.position.x<destination.x) ||
                        (start_loc.y > destination.y && transform.position.y < destination.y) ||
                        (start_loc.x < destination.x && transform.position.x > destination.x) ||
                        (start_loc.y < destination.y && transform.position.y > destination.y) ||
                        (transform.position == destination)){
			dir = (start_loc - destination);
			dir.Normalize();
		}
		if(bandf && (start_loc.x > destination.x && transform.position.x>start_loc.x) ||
                        (start_loc.y > destination.y && transform.position.y > start_loc.y) ||
                        (start_loc.x < destination.x && transform.position.x < start_loc.x) ||
                        (start_loc.y < destination.y && transform.position.y < start_loc.y) ||
                        (transform.position == start_loc)){
			dir = (destination - start_loc);
			dir.Normalize();
		}

		//if star revolves, rotate around the specified point
		if(is_revolving && !editor_freeze)
		{
			transform.RotateAround(rpoint,Vector3.forward,Time.deltaTime*rspeed);
		}
		
		if(spiral) {
			speed = Mathf.Lerp(speed_of_entry, BLACK_HOLE_SUCKINESS, .5f*Time.deltaTime);
			if(is_revolving || is_moving){
				//change revolving stars to moving stars in the direction they were previously moving
				if (is_revolving) {
					is_revolving = false;
					is_moving = true;
					dir = this.transform.position - lastPos;
					speed = rspeed;
				}
				intoBlackHole();
			}
			else {
				if(clockwise) {
					this.transform.RotateAround(BHspiral.transform.position, Vector3.forward, -BLACK_HOLE_SUCKINESS*Time.deltaTime);
				}else {
					this.transform.RotateAround(BHspiral.transform.position, Vector3.forward, BLACK_HOLE_SUCKINESS*Time.deltaTime);
				}
				Vector3 perp = this.transform.position - BHspiral.transform.position;
				perp = new Vector3(perp.x, perp.y, 0);
				perp.Normalize();
				this.transform.position -= perp;
				this.transform.localScale *= .995f;
				r.light.range *= .995f;
				if (Mathf.Abs(transform.position.x - BHspiral.transform.position.x) <= 1f 
				  && Mathf.Abs(transform.position.y - BHspiral.transform.position.y) <= 1f) {
					removeStar(NO_EXPLOSION);
				}
			}
		}
	/*	if (black_hole_active) {
			Vector3 new_start = this.transform.position + starSize/2*new Vector3(Random.value, Random.value,0);
			if (Vector3.Distance(this.transform.position,line.transform.position) < 2f) {
				Destroy(line);
				
				if (Manager.l.renderer.material.color == Color.red) 
					line = Instantiate (red_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.orange) 
					line = Instantiate (orange_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Color.yellow) 
					line = Instantiate (yellow_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.green) 
					line = Instantiate (green_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Color.blue) 
					line = Instantiate (blue_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.aqua) 
					line = Instantiate (aqua_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.purple) 
					line = Instantiate (purple_line, new_start, Manager.l.transform.rotation) as GameObject;
		//	}
		//	else {
				line.transform.RotateAround(this.transform.position, Vector3.forward, 
						1/(Vector3.Distance(line.transform.position, this.transform.position)*Time.deltaTime)*10);
				Vector3 perp = line.transform.position - this.transform.position;
				perp.Normalize();
				line.transform.position -= new Vector3(perp.x, perp.y, 0)*Manager.BLACK_HOLE_SUCKINESS;
					
			}
		}*/
				
		glow = false;
	}
	
	void intoBlackHole() {
		//find tangent to star's movement
		Vector3 movement = this.transform.position - lastPos;
		Vector3 star_from_BH = BHspiral.transform.position - this.transform.position;
		Vector3 projection = Vector3.Project (star_from_BH, movement);
		Vector3 tangent = projection + this.transform.position;
		//determine if star is tangent to point to be sucked into black hole
		if (Vector3.Distance(tangent, this.transform.position) <= 10f) {
			//disable previous movement
			is_moving = false;
			is_revolving = false;
			//determine direction to be sucked in
			if (tangent.y < BHspiral.transform.position.y && movement.x < 0) { 
				clockwise = true;
			}else if (tangent.y > BHspiral.transform.position.y && movement.x > 0) {
				clockwise = true;
			}else if (tangent.x < BHspiral.transform.position.x && movement.y > 0) {
				clockwise = true;
			}else {
				clockwise = false;
			}			
		}
	}
	
	void OnCollisionEnter(Collision c)
	{
		//if the star is hit by a bomb detonation, explode	
		if(c.transform.name == "space_bomb_range(Clone)" && !has_been_bombed) {
			//if learth is close enough, send it flying in the opposite direction and give it some r
			if(Vector3.Distance(transform.position,Learth_Movement.velocity + Learth_Movement.lastPos.position) < 350) {
				Learth_Movement.lastPos.position=transform.position;
				Learth_Movement.isTangent = false;
				Manager.energy += 50;
			}
			removeStar(BIG_EXPLOSION);
			has_been_bombed = true;
			Learth_Movement.isTangent = false;
		}
		else if (!spiral && !isBlackHole && c.transform.tag == "blackhole"){
			spiral = true;
			lastPos = this.transform.position;
			BHspiral = c.gameObject;
			if(is_moving) 
				speed_of_entry = speed;
			if(is_revolving) 
				speed_of_entry = rspeed;
		}
	}
	public void BoomTime()
	{
		//Starts explosion blinker
			if (Time.time > timer)
			{
				timer = Time.time + blinkspeed;
				onoff = !onoff;
				this.renderer.material.color = onoff ? Color.white : Color.red;
				blinkspeed=blinkspeed*.9;
			}
		
	}
	public bool waitsec(double time)
	{
		//Alarm clock which returns true if explode timer is greater than however long you want
		explodetimer=explodetimer+Time.deltaTime;
		return explodetimer>time;
		
	}
	public void removeStar(int explosion_type)
	{
		gscpt.stars_destroyed++;
		
		GameObject go = GameObject.Find("Alien_Exp_Sound");
		Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
		ascpt.star_explosion.Play();
		//Debug.Break();
		//Explodes the star, and makes it impossible to be targeted again
		if(explosion_type == BIG_EXPLOSION) {
			if (c == Color.red)
				Instantiate(red_explosion, transform.position, transform.rotation);
			else if (c == Manager.orange)
				Instantiate(orange_explosion, transform.position, transform.rotation);	
			else if (c == Color.yellow)
				Instantiate(yellow_explosion, transform.position, transform.rotation);	
			else if (c == Manager.green)
				Instantiate(green_explosion, transform.position, transform.rotation);	
			else if (c == Color.blue)
				Instantiate(blue_explosion, transform.position, transform.rotation);	
			else if (c == Manager.purple)
				Instantiate(purple_explosion, transform.position, transform.rotation);	
			else if (c == Manager.aqua)
				Instantiate(aqua_explosion, transform.position, transform.rotation);	
			
			
		}
		collider.enabled=false;
		orbitRadius=0;
		is_moving=false;
		renderer.enabled=false;
		r.light.enabled=false;
		//move under the board so you definitely can't orbit it
		transform.position = new Vector3(0,0,-500);
		is_destroyed = true;
	}

	//delete in level editor ... this should be in a parent class to all level editable things
	void OnMouseDown() {
		if(Level_Editor.delete_button) {	
			Destroy (gameObject);	
		}
	}
}


