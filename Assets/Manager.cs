using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	/* KNOWN BUGS */
	/* Game freeze when you call ResetLevel() before you leave the orbit of the first star */
	
	//Constants for tweaking
	//Larger the error, the wider legal orbit radius 
	private int RADIAL_ERROR = 9;
	//larger the tan error, the easier it is to enter a star at a legal radius
	private float TAN_ERROR = 8;
	//the larger this number is, the sharper bends are
	private float BEND_FACTOR = 5.5f;
	//lerp note: too high, and the game is jerky, too low and the learth goes off screen 
	//the larger this number is, the more closely the camera follows learth while in orbit
	private float ORBIT_LERP = .05f;
	//the larger this number is, the more closely the camera follows learth while not in orbit
	private static float TRAVEL_LERP = 0.7F;
	//How far the player is allowed to move the camera
	private float CAM_MAX_DIST = 400;
	//How close the player is allowed to move the camera
	private float CAM_MIN_DIST = 50;
	//how fast the player can zoom in/out
	private float CAM_MOVE_SPEED = 2;
	//Camera orthographic size at start, higher = see more
	private float CAM_START_HEIGHT = 300;
	
	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	public static GameObject cur_star;
	
	//actual objects used in script
	public static GameObject l, s, s1, s2, s3, s4, s5, s6, s7, s8;
	public GameObject[] star_arr;
	public int numStars = 0;
	
	//level related variables, not sure how this works with different scenes. might need another class for these
	//positions past which learth will die. levels are always rectangles
	float LEVEL_X_MAX = 500;
	float LEVEL_X_MIN = -500;
	float LEVEL_Y_MAX = 500;
	float LEVEL_Y_MIN = -500;
	
	//learth-related variables
	public static int energy = 2;
	public GameObject lastStar;
	public static Vector3 tangent;
	public static bool clockwise = false;
	public static int num_deaths = 0;
	public int revisit = 0;
	public static bool orbitting = false;
	
	public Color orange = new Color(1f, .6f, 0f, 1f);
	public Color dgray = new Color(.1f, .1f, .1f, 1f);
	public Texture tred;
	public Texture torange;
	public Texture tyellow;
	public Texture twhite;
	public Texture tgray;
	public Texture tblue;
	
	void Start () {
		//instantiate learth
		l = Instantiate (learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
		//instantiate stars and store them in array
		star_arr = new GameObject[8]; 
		
		//instantiate spacerips
		CreateSpaceRip(-200,-70,10,600);
		CreateSpaceRip(-200, -500,10,70);
		
		//instantiate stars
		s1 = CreateStar (s1, new Vector3 (0, 0, 0), Color.white, twhite, 30f);
		s2 = CreateStar (s2, new Vector3 (-50, -100, 0), Color.blue, tblue, 35f);
		s3 = CreateStar (s3, new Vector3 (50, -200, 0), Color.yellow, tyellow, 30f);
		s4 = CreateStar (s4, new Vector3 (-50, -300, 0), Color.white, twhite, 35f);
		s5 = CreateStar (s5, new Vector3 (50, -400, 0), Color.red, tred, 35f);
		s6 = CreateStar (s6, new Vector3 (-100, -450, 0), Color.red, tred, 35f);
		s7 = CreateStar (s7, new Vector3 (-300, 150, 0), Color.blue, tblue, 30f);
		s8 = CreateStar (s8, new Vector3 (400, 150, 0), Color.blue, tblue, 70f);
		
		lastStar = s8;
		numStars+=8;
		star_arr[0] = s1;
		star_arr[1] = s2;
		star_arr[2] = s3;
		star_arr[3] = s4;
		star_arr[4] = s5;
		star_arr[5] = s6;	
		star_arr[6] = s7; 
		star_arr[7] = s8;
		
		//set camera height for beginning a game
		Camera.main.orthographicSize = CAM_START_HEIGHT;
	}
	
	//instantiates a space rip from prefab at given location and of given dimensions, returns reference to that object
	GameObject CreateSpaceRip(float x, float y, float width, float height)
	{
		GameObject rip_actual = Instantiate (rip, new Vector3 (x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		rip_actual.transform.localScale += new Vector3(width,height,0);
		return rip_actual;
	}
	
	//instantiates star given a reference to a gameobject, a location, a color, a texture, and a size
	GameObject CreateStar(GameObject starE, Vector3 vec, Color color, Texture texture, float size)
	{
		starE = Instantiate (star, vec, new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = color;
		starscript.t = texture;
		starscript.starSize = size; 
		return starE;
	}
	
	//puts Learth in orbit given an entrance point, a velocity, a star, and a direction
	public static void MoveLearthToOrbit(Vector3 entrance_point, Vector3 entrance_velocity, GameObject star, bool cwise )
	{
		s = star;
		cur_star = s;
		Learth_Movement.isTangent = true;
		l.transform.position = Vector3.Lerp(l.transform.position,entrance_point,100.0F);
		Learth_Movement.velocity = entrance_velocity;
		clockwise = cwise;
		orbitting = true;
	} 
	
	//call this anytime something kills the player
	public static void Die()
	{
		//death animation here?
		
		//if you screw up too much at the beginning, or if you've died more than 3 times, just start the level over
		if(Learth_Movement.last_star_gos[num_deaths] == null || num_deaths > 2)
		{
			ResetLevel();
		}
		//otherwise, you move back 1, 2, or 3 stars
		else {
			//move learth to previous stars
			MoveLearthToOrbit(Learth_Movement.last_stars[num_deaths], Learth_Movement.last_stars_velocity[num_deaths], 
				Learth_Movement.last_star_gos[num_deaths], Learth_Movement.last_star_rots[num_deaths]);
				
			//move with learth
			Camera.main.transform.position = new Vector3(l.transform.position.x, l.transform.position.y, Camera.main.transform.position.z);
			num_deaths++;
		}
		
	}
	
	//reloads the scene and modifies whatever we want to modify when the scene gets reloaded
	public static void ResetLevel() {
		Application.LoadLevel(Application.loadedLevel);	
	}
	
	void Update () {
		
		/*********************DEBUGGING CONTROLS********************/
		// resetting level with T before leaving first star orbit freezes the game 
		//R causes the player to die
		if(Input.GetKeyDown(KeyCode.R))
			Die();
		//T resets the level
		if(Input.GetKeyDown(KeyCode.T))
			ResetLevel();
		//Y resets camera to learth's position
		if(Input.GetKeyDown (KeyCode.Y))
			Camera.main.transform.position = new Vector3(l.transform.position.x, l.transform.position.y, Camera.main.transform.position.z);
		//U prints position and last position and their difference on demand
		if(Input.GetKeyDown(KeyCode.U))
			Debug.Log("pos: "+l.transform.position+" last pos: "+Learth_Movement.lastPos+" dist: "
				+Vector3.Distance(l.transform.position,Learth_Movement.lastPos));
		/*********************END DEBUGGING CONTROLS*****************/
		
		//bending - each has 4 cases. this is functional enough but needs to be seriously analyzed and probably rewritten 
		if(Input.GetKey(KeyCode.Q))
		{
			if(l.transform.position.x < 0 && l.transform.position.y < 0)
				Learth_Movement.lastPos += BEND_FACTOR*Time.deltaTime*new Vector3(0.1f,-0.1f,0);

			if(l.transform.position.x < 0 && l.transform.position.y >= 0)
				Learth_Movement.lastPos += BEND_FACTOR*Time.deltaTime*new Vector3(0.1f,0.1f,0); 			
			
			if(l.transform.position.x >= 0 && l.transform.position.y < 0)
				Learth_Movement.lastPos += BEND_FACTOR*Time.deltaTime*new Vector3(-0.1f,-0.1f,0);
				
			if(l.transform.position.x >= 0 && l.transform.position.y >= 0)
				Learth_Movement.lastPos += BEND_FACTOR*Time.deltaTime*new Vector3(-0.1f, 0.1f,0);
		}
		else if (Input.GetKey(KeyCode.W))
		{		
			if(l.transform.position.x < 0  && l.transform.position.y < 0)
				Learth_Movement.lastPos -= BEND_FACTOR*Time.deltaTime*new Vector3(0.1f,-0.1f,0);
			
			if(l.transform.position.x < 0 && l.transform.position.y >= 0)
				Learth_Movement.lastPos -= BEND_FACTOR*Time.deltaTime*new Vector3(0.1f,0.1f,0); 
			
			if(l.transform.position.x >= 0 && l.transform.position.y < 0)
				Learth_Movement.lastPos -= BEND_FACTOR*Time.deltaTime*new Vector3(-0.1f,-0.1f,0);
				
			if(l.transform.position.x >= 0 && l.transform.position.y >= 0)
				Learth_Movement.lastPos -= BEND_FACTOR*Time.deltaTime*new Vector3(-0.1f, 0.1f,0);
		}
		
		
		//Death conditions
		//if you run out of energy, you die, but you get a little energy back
		if(energy < 0)
		{
			Die ();
			energy = 2;
		}
		
		//if you travel outside the bounds of the level, you die
		if(l.transform.position.x > LEVEL_X_MAX
			|| l.transform.position.x < LEVEL_X_MIN
			|| l.transform.position.y > LEVEL_Y_MAX
			|| l.transform.position.y < LEVEL_Y_MIN)
			Die ();
		
		
		//if learth is tangent to star s, rotate around star s
		if (Learth_Movement.isTangent) {
			orbitting = true;
			if (clockwise){
				l.transform.RotateAround(s.transform.position, Vector3.forward, -(Learth_Movement.SPEED)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime));
			}
			else  {
				l.transform.RotateAround(s.transform.position, Vector3.forward, Learth_Movement.SPEED/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime));
			}
			if (Vector3.Distance (l.transform.position, tangent) < 1) {
				revisit++;
				if (revisit == 1) {
					energy -= 1;
				}
			}
			else {
				revisit = 0;
			}
			//if space bar is pressed, accelerate away from star. Problem: sometimes star gets stuck in orbit because its still within orbital radius
			if (Input.GetKeyDown(KeyCode.Space)) {
				Learth_Movement.isTangent = false;
				lastStar = s;			
				energy -= 1;
				Learth_Movement.lastPos = l.transform.position - Learth_Movement.velocity.normalized*Learth_Movement.SPEED;
				orbitting = false;
			}
		}
		//if earth is not tangent to any star, loop through array and calculate tangent vectors to every star
		else if (!Learth_Movement.isTangent) {
			for (int i = 0; i < numStars; i++){
				s = star_arr[i];
				Starscript sscript = s.GetComponent<Starscript>();
				Vector3 l_movement = Learth_Movement.velocity;
				Vector3 star_from_learth = s.transform.position - l.transform.position;
				Vector3 projection = Vector3.Project (star_from_learth, l_movement);
				tangent = projection + l.transform.position;
				//if planet is within star's orbital radius, set isTangent to true
				if (s != lastStar && Vector3.Distance(s.transform.position, l.transform.position) >= (sscript.orbitRadius - RADIAL_ERROR) && Vector3.Distance(s.transform.position, l.transform.position) <= (sscript.orbitRadius + RADIAL_ERROR) && Vector3.Distance (tangent, l.transform.position) <= TAN_ERROR) {
					orbitting = true;
					cur_star = s;
					Learth_Movement.isTangent = true;
					//determine direction of orbit
					if (tangent.y < s.transform.position.y && l_movement.x < 0) { 
						clockwise = true;
					}
					else if (tangent.y > s.transform.position.y  && l_movement.x > 0) {
						clockwise = true;
					}		
					else if (tangent.x < s.transform.position.x && l_movement.y > 0) {
						clockwise = true;
					}
					else {
						clockwise = false;
					}
					
					//update last stars, last entrances, last velocity vectors, and last rotations to include this star
					for(int k=2; k>0;k--)
					{
						Learth_Movement.last_stars[k] = Learth_Movement.last_stars[k-1];
						Learth_Movement.last_stars_velocity[k] = Learth_Movement.last_stars_velocity[k-1];
						Learth_Movement.last_star_gos[k] = Learth_Movement.last_star_gos[k-1];
						Learth_Movement.last_star_rots[k] = Learth_Movement.last_star_rots[k-1];
					}
					Learth_Movement.last_stars[0] = l.transform.position;
					Learth_Movement.last_stars_velocity[0] = l_movement;
					Learth_Movement.last_star_gos[0] = s;
					Learth_Movement.last_star_rots[0] = clockwise;
					
					//add appropriate energy value depending on color of star
					if (sscript.c == Color.blue) {
						energy += 5;
					} else if (sscript.c == Color.white){
						energy += 4;
					} else if (sscript.c == Color.yellow) {
						energy += 3;
					} else if (sscript.t == torange) {
						energy += 2;
					} else if (sscript.c == Color.red) {
						energy += 1;
					}
					else {
						energy -= 1;
					}
					sscript.c = dgray;
					sscript.t = tgray;
					break;
				}
			}
		}
	
		//camera follows learth
		Camera.main.transform.position = orbitting ? 
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(l.transform.position.x,l.transform.position.y,Camera.main.transform.position.z),ORBIT_LERP*Time.deltaTime)
				:
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(l.transform.position.x,l.transform.position.y,Camera.main.transform.position.z),TRAVEL_LERP*Time.deltaTime)
				;
		
		//A moves the camera farther, S moves the camera closer
		if(Input.GetKey(KeyCode.A) && Camera.main.orthographicSize <= CAM_MAX_DIST)
			Camera.main.orthographicSize += CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.S) && Camera.main.orthographicSize >= CAM_MIN_DIST)
			Camera.main.orthographicSize -= CAM_MOVE_SPEED;
		
	
	}
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 30), "Energy: " + energy);
    }
		
}
	
