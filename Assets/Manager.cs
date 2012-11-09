using UnityEngine;
using System.Collections;
using System.IO;

public class Manager : MonoBehaviour {
	
	//Constants

	/*GAMEPLAY CONTROLS */
	//Larger the error, the wider legal orbit radius 
	private int RADIAL_ERROR = 10;
	//larger the tan error, the easier it is to enter a star at a legal radius
	private float TAN_ERROR = 8;
	//the larger this number is, the sharper bends are (est. useful range: 0 - 2)
	private float BEND_FACTOR = 0.01f;
	//larger the number, the faster the learth moves overall
	private float MOVEMENT_SPEED = 0.70f;
	//larger the number, the faster learth moves when orbiting (doesn't affect speed, but makes aiming easier)
	private float ORBIT_SPEED_FACTOR = .85f;
	
	/*CAMERA CONTROLS */
	//the larger this number is, the more closely the camera follows learth while in orbit
	private float ORBIT_LERP = .06f;
	//the larger this number is, the more closely the camera follows learth while not in orbit
	private float TRAVEL_LERP = 0.6F;
	//How far the player is allowed to move the camera
	private float CAM_MAX_DIST = 5000;
	//How close the player is allowed to move the camera
	private float CAM_MIN_DIST = 50;
	//how fast the player can zoom in/out
	private float CAM_MOVE_SPEED = 10;
	//Camera orthographic size at start, higher = see more
	private float CAM_START_HEIGHT = 400;
	
	/*ENERGY CONTROLS */	
	//How much energy is reduced each frame while bending
	private float BEND_COST = .025f;
	//How much energy is reduced each frame while invincible
	private float INVINC_COST = .05f;
	//this much energy is subtracted each frame the learth is not in orbit
	private float FLYING_COST = .0025f;
	//this much energy is subtracted each frame the learth is in orbit
	private float ORBITING_COST = .000025f;
	//this much energy is subtracted when they player hits the space bar to launch from a star
	private float LEAVING_COST = 0;
	
	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	public GameObject coin;
	public GameObject plane;
	public GameObject alien;
	public static GameObject cur_star;
	
	//actual objects used in script
	public static GameObject l, s, e, p;
	public GameObject[] star_arr;
	public GameObject[] rip_arr;
	public GameObject[] coin_arr;
	public GameObject[] alien_arr;
	public int numStars = 0;
	
	//level related variables, not sure how this works with different scenes. might need another class for these
	//positions past which learth will die. levels are always rectangles
	float LEVEL_X_MAX = 10000;
	float LEVEL_X_MIN = -1000;
	float LEVEL_Y_MAX = 1000;
	float LEVEL_Y_MIN = -1000;
	
	//learth-related variables
	public static float speed = 0;
	public static float energy = 24f;
	public static GameObject lastStar;
	public static Vector3 tangent;
	public static bool clockwise = false;
	public static int num_deaths = 0;
	public int revisit = 0;
	
	//star colors and textures
	public Color orange = new Color(1f, .6f, 0f, 1f);
	public Color dgray = new Color(.1f, .1f, .1f, 1f);
	public Texture tred;
	public Texture torange;
	public Texture tyellow;
	public Texture twhite;
	public Texture tgray;
	public Texture tblue;
	
	public GameObject al1;
	public GameObject al2;
	public GameObject al3;

    //energy gauge
    public Texture gaugeTexture;

	//currency
	public static int currency = 0;
	
	void Start () {
		//instantiate learth
		l = Instantiate (learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
	 	l.renderer.material.color = Color.red;	
		//instantiate star storage
		star_arr = new GameObject[0];
		alien_arr = new GameObject[3];
		
		al1 = Instantiate(alien, new Vector3(1800, 400, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
		alien_behavior al1b = al1.GetComponent<alien_behavior>();
		al1b.learth = l;
		alien_arr[0] = al1;
		al2 = Instantiate(alien, new Vector3(1800, 100, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
		alien_behavior al2b = al2.GetComponent<alien_behavior>();
		al2b.learth = l;
		alien_arr[1] = al2;		
		al3 = Instantiate(alien, new Vector3(1800, -100, 0), new Quaternion(0, 0, 0, 0)) as GameObject;		
		alien_behavior al3b = al3.GetComponent<alien_behavior>();
		al3b.learth = l;
		alien_arr[2] = al3;
		
		//set camera height for beginning a game
		Camera.main.orthographicSize = CAM_START_HEIGHT;
		
		//instantiate background based on level constraints
		for (int i = -2500; i < (int)LEVEL_X_MAX; i+=2500) {
			for (int j = -4000; j < 4000; j+=1250) {
				p = Instantiate (plane, new Vector3(i, j, 100), transform.rotation) as GameObject;
				p.transform.Rotate(270, 0, 0);
			}
		}
		//load a level
		LoadLevel("Assets/level3.txt");
	}
	
	
	/* HOW LEVELS WORK
	 * in case you want to make/change a level
	 * 
	 * each level is stored in a text file in the Assets directory ex: level3.txt
	 * 
	 * the first line of that file contains the number of each type of level design element to be specified, delimited by commas
	 * ex: 1,0,0,2 == 1 static star, 0 space rips, 0 coins, 2 moving stars
	 * 
	 * Following that are the arguments for each method call that will instantiate the specified elements
	 * 
	 * The Learth begins the level orbiting the first static star specified and at least one static star must be specified
	 * 
	 * To change levels, you must call UnloadCurrentLevel() before you call LoadLevel(fname) 
	 * 
	 */	
	//Destroys all elements of currently loaded level
	//this must be called before you load another level, unless you want to compose multiple levels
	public void UnloadCurrentLevel() 
	{
		//destroy stars
		for(int i = 0; i < star_arr.Length; i++)
			Destroy(star_arr[i]);
		//reset star_arr and counter
		star_arr = new GameObject[0];
		numStars = 0;
		
		//destroy space rips
		for(int i = 0; i < rip_arr.Length; i++)
			Destroy (rip_arr[i]);
		rip_arr = new GameObject[0];
		
		//destroy coins
		for(int i = 0; i < coin_arr.Length; i++)
			Destroy (coin_arr[i]);
		
		//reset energy
		energy = 20f;
		
		//reset currency (maybe don't do this for design reasons)
		currency = 0;
		
		//make sure learth is not tangent 
		Learth_Movement.isTangent = false;
	}
	
	//instantiates level design elements as specified in the text file in the argument
	public void LoadLevel(string fname) 
	{
		string line;
		char[] delim = {','};
		StreamReader file = new StreamReader(fname);
		string numels = file.ReadLine();
		
		//get numbers of each type of element
		string[] sp = numels.Split(delim);
		int stars = int.Parse(sp[0]);
		int rips  = int.Parse(sp[1]);
		int coins = int.Parse(sp[2]);
		int mstars = int.Parse (sp[3]);
		
		//create all stars specified in the text file
		for(int i=0; i<stars;i++)
		{
			line = file.ReadLine();
			string [] args = line.Split(delim);
			
			//get color and texture objects
			Color starcol = Color.white;
			Texture startex = twhite;
			if(args[2] == "blue"){
				starcol = Color.blue;
				startex = tblue;
				
			} else if(args[2] == "white") {
				starcol = Color.white;
				startex = twhite;
			} else if(args[2] == "red") {
				starcol = Color.red;
				startex = tred;
			} else if (args[2] == "yellow") {
				starcol = Color.yellow;
				startex = tyellow;
			}
			
			//make the star
			GameObject newstar = CreateStar(float.Parse(args[0]),float.Parse(args[1]), starcol, startex, float.Parse(args[3]));
			
			//learth starts in orbit around first star specified
			if(i == 0)
				GoToOrbit(newstar,float.Parse(args[3]));	
			//last star is the sink
			if(i == stars-1){
				Starscript scpt = newstar.GetComponent<Starscript>();
				scpt.is_sink = true;
			}
				
		}
		
		
		
		//create all space rips specified in the text file
		for(int i = 0; i < rips;i++)
		{
			line = file.ReadLine();
			string[] args = line.Split(delim);
			
			CreateSpaceRip(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2]),float.Parse(args[3]),float.Parse(args[4]));
		}
		
		//create all coins specified in the text file
		for(int i = 0; i < coins; i++)
		{
			line = file.ReadLine();
			string[] args = line.Split(delim);
			
			CreateCoin(float.Parse(args[0]),float.Parse(args[1]));
		}
		
		//create all moving stars specified in the text file
		for(int i = 0; i < mstars;i++)
		{
			line = file.ReadLine();
			string [] args = line.Split(delim);
			
			//get color and texture objects
			Color starcol = Color.white;
			Texture startex = twhite;
			if(args[2] == "blue"){
				starcol = Color.blue;
				startex = tblue;
				
			} else if(args[2] == "white") {
				starcol = Color.white;
				startex = twhite;
			} else if(args[2] == "red") {
				starcol = Color.red;
				startex = tred;
			} else if (args[2] == "yellow") {
				starcol = Color.yellow;
				startex = tyellow;
			}
			
			//make the star
			CreateMovingStar(float.Parse(args[0]),float.Parse(args[1]), 
				starcol, startex, float.Parse(args[3]), new Vector3(float.Parse (args[4]), float.Parse(args[5]),0), float.Parse(args[6]));
			
		}
	}
	
	//puts learth in orbit given a valid radius
	public static void GoToOrbit(GameObject star, float radius)
	{
		l.transform.position = new Vector3(star.transform.position.x+radius,star.transform.position.y,0);
		cur_star = star;
		s = star;
		Learth_Movement.isTangent = true;
	}
	
	//instantiates a coin at the location provided
	GameObject CreateCoin(float x, float y)
	{
		GameObject coin_actual = Instantiate(coin, new Vector3(x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
		//put rip in rip_arr for unloading
		GameObject[] temp_arr = new GameObject[coin_arr.Length+1];
		for(int i=0;i<coin_arr.Length;i++)
			temp_arr[i] = coin_arr[i];
		coin_arr = temp_arr;
		coin_arr[coin_arr.Length-1] = coin_actual;
		return coin_actual;
	}
	
	//instantiates a space rip from prefab at given location and of given dimensions, with given rotation (default = 0), returns reference to that object
	GameObject CreateSpaceRip(float x, float y, float width, float height, float rotation = 0)
	{
		GameObject rip_actual = Instantiate (rip, new Vector3 (x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		rip_actual.transform.localScale += new Vector3(width,height,0);
		rip_actual.transform.Rotate(new Vector3(0,0,rotation));
		
		//put rip in rip_arr for unloading
		GameObject[] temp_arr = new GameObject[rip_arr.Length+1];
		for(int i=0;i<rip_arr.Length;i++)
			temp_arr[i] = rip_arr[i];
		rip_arr = temp_arr;
		rip_arr[rip_arr.Length-1] = rip_actual;
		
		return rip_actual;
	}
	
	//instantiates a star that moves in the direction given at the speed given
	GameObject CreateMovingStar(float x, float y, Color color, Texture texture, float size, Vector3 dir, float speed)
	{
		GameObject mstar = CreateStar(x,y,color,texture,size);
		Starscript scpt  = mstar.GetComponent<Starscript>();
		scpt.is_moving = true;
		scpt.dir = dir;
		scpt.speed = speed;
		return mstar;
	}

	//instantiates star from prefab at given xy location and of given characteristics
	GameObject CreateStar(float x, float y, Color color, Texture texture, float size)
	{
		GameObject starE = Instantiate (star, new Vector3(x,y,0), new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = color;
		starscript.t = texture;
		starscript.starSize = size; 
		
		//expand and copy star_arr - if loading a level takes too long, this can be optimized
		GameObject[] temp_arr = new GameObject[star_arr.Length+1];
		for(int i=0;i<star_arr.Length;i++)
			temp_arr[i] = star_arr[i];
		star_arr = temp_arr;
		star_arr[star_arr.Length-1] = starE;
		lastStar = starE;
		numStars++;
		return starE;
	}
	
	//puts Learth in orbit given an entrance point, an energy value, a velocity, a star, and a direction
	public static void MoveLearthToOrbit(Vector3 entrance_point, Vector3 entrance_velocity, float lastEnergy, GameObject star, bool cwise )
	{
		energy = lastEnergy;
		s = star;
		cur_star = s;
		Learth_Movement.isTangent = true;
		l.transform.position = Vector3.Lerp(l.transform.position,entrance_point,100.0F);
		Learth_Movement.velocity = entrance_velocity;
		clockwise = cwise;
	} 
	
	//call this anytime something kills the player
	public static void Die()
	{		
		Starscript scpt = lastStar.GetComponent<Starscript>();
		GoToOrbit(lastStar,scpt.orbitRadius);
	}
	
	//reloads the scene and modifies whatever we want to modify when the scene gets reloaded
	public static void ResetLevel() {
		Application.LoadLevel(Application.loadedLevel);	
		energy = 11;
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
		//U prints position and last position and their difference
		if(Input.GetKeyDown(KeyCode.U))
			Debug.Log("pos: "+l.transform.position+" last pos: "+Learth_Movement.lastPos.position+" dist: "
				+Vector3.Distance(l.transform.position,Learth_Movement.lastPos.position));
		//f increases energy by 1
		if(Input.GetKeyDown(KeyCode.F))
			energy++;
		//H unloads the current level
		if(Input.GetKeyDown (KeyCode.H))
			UnloadCurrentLevel();
		if(Input.GetKeyDown(KeyCode.J))
			LoadLevel("assets/level2.txt");
				
		/*********************END DEBUGGING CONTROLS*****************/
		
		//alieeeeens!
		for(int i = 0; i<3; i++){
			Debug.Log(i);
			if(Mathf.Abs(Vector3.Distance(l.transform.position, alien_arr[0].transform.position)) < 7){
				energy -= .04f;
			}
		}
		
		//Speed increases logarithmically with energy
		speed = Mathf.Log(energy)*MOVEMENT_SPEED;
		
		//bending
		if(Input.GetKey(KeyCode.Q))
		{
			energy -= BEND_COST;
			Learth_Movement.lastPos.RotateAround(Learth_Movement.velocity,Vector3.forward,-1*Time.deltaTime*BEND_FACTOR);
		}
		else if (Input.GetKey(KeyCode.W))
		{		
			energy -= BEND_COST;
			Learth_Movement.lastPos.RotateAround(Learth_Movement.velocity,Vector3.forward,Time.deltaTime*BEND_FACTOR);
		}
		
		//temporary invincibility, logic implemented in Learth_Movement.cs 
		if(Input.GetKey(KeyCode.E))
		{
			l.renderer.material.color = Color.green;
			energy -= INVINC_COST;
		}
		//change learth color back to normal
		if(Input.GetKeyUp (KeyCode.E))
			l.renderer.material.color = Color.red;
		
		//Death conditions
		//if you run out of energy, you die, but you get a little energy back
		if(energy < 1)
		{
			Die ();
			energy = 6f;
		}
		
		//if you travel outside the bounds of the level, you die
		if(l.transform.position.x > LEVEL_X_MAX
			|| l.transform.position.x < LEVEL_X_MIN
			|| l.transform.position.y > LEVEL_Y_MAX
			|| l.transform.position.y < LEVEL_Y_MIN)
			Die ();
		
		
		//if learth is tangent to star s
		if (Learth_Movement.isTangent) {
			//if in orbit, decrease energy at correct rate
			energy -= ORBITING_COST;
			
			//if learth is orbiting a moving planet, translate it with the planet to maintain circular orbit
			Starscript scpt = cur_star.GetComponent<Starscript>();
			if(scpt.is_moving)
				l.transform.Translate(scpt.dir.x*scpt.speed*Time.deltaTime,scpt.dir.y*scpt.speed*Time.deltaTime,0,Space.World);
			
			
			//rotate around star s
			if (clockwise){
				l.transform.RotateAround(s.transform.position, Vector3.forward, 
					-(speed > 1 ? speed : 1)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime)*ORBIT_SPEED_FACTOR);
			}
			else  {
				l.transform.RotateAround(s.transform.position, 
					Vector3.forward, (speed > 1 ? speed : 1)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime/ORBIT_SPEED_FACTOR));
			}
			if (Vector3.Distance (l.transform.position, tangent) < 2) {
				revisit++;
				if (revisit == 1) {
					energy -= 1f;
				}
			}
			else {
				revisit = 0;
			}
			
			//if space bar is pressed, accelerate away from star. 
			if (Input.GetKeyDown(KeyCode.Space)) {
				Learth_Movement.isTangent = false;
				lastStar = s;			
				energy -= LEAVING_COST;
				Learth_Movement.lastPos.position = l.transform.position - Learth_Movement.velocity.normalized*speed;
			}
		}
		//if earth is not tangent to any star
		else if (!Learth_Movement.isTangent) {
			//if not in orbit, decrease energy at correct rate
			energy -= FLYING_COST;
			
			//loop through array and calculate tangent vectors to every star
			for (int i = 0; i < numStars; i++){
				s = star_arr[i];
				Starscript sscript = s.GetComponent<Starscript>();
				Vector3 l_movement = Learth_Movement.velocity;
				Vector3 star_from_learth = s.transform.position - l.transform.position;
				Vector3 projection = Vector3.Project (star_from_learth, l_movement);
				tangent = projection + l.transform.position;
				//if planet is within star's orbital radius, set isTangent to true
				if (s != lastStar 
					&& Vector3.Distance(s.transform.position, l.transform.position) >= (sscript.orbitRadius - RADIAL_ERROR) 
					&& Vector3.Distance(s.transform.position, l.transform.position) <= (sscript.orbitRadius + RADIAL_ERROR) 
					&& Vector3.Distance (tangent, l.transform.position) <= TAN_ERROR) 
				{	
					Learth_Movement.isTangent = true;
					cur_star = s;
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
					
					//update last stars, last energy value, last entrances, last velocity vectors, and last rotations to include this star
					for(int k=2; k>0;k--)
					{
						
						Learth_Movement.last_stars[k] = Learth_Movement.last_stars[k-1];
						Learth_Movement.last_energies[k] = Learth_Movement.last_energies[k-1];
						Learth_Movement.last_stars_velocity[k] = Learth_Movement.last_stars_velocity[k-1];
						Learth_Movement.last_star_gos[k] = Learth_Movement.last_star_gos[k-1];
						Learth_Movement.last_star_rots[k] = Learth_Movement.last_star_rots[k-1];
					}
					Learth_Movement.last_stars[0] = l.transform.position;
					Learth_Movement.last_energies[0] = energy;
					Learth_Movement.last_stars_velocity[0] = l_movement;
					Learth_Movement.last_star_gos[0] = s;
					Learth_Movement.last_star_rots[0] = clockwise;
					
					//add appropriate energy value depending on color of star
					if (sscript.c == Color.blue) {
						energy += 5f;
					} else if (sscript.c == Color.white){
						energy += 3f;
					} else if (sscript.c == Color.yellow) {
						energy += 3f;
					} else if (sscript.t == torange) {
						energy += 2f;
					} else if (sscript.c == Color.red) {
						energy += 8f;
					}
					else {
						energy -= 1f;
					}
					sscript.c = dgray;
					sscript.t = tgray;
					break;
				}
			}
		}
	
		//camera follows learth
		Camera.main.transform.position = Learth_Movement.isTangent ? 
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(l.transform.position.x,l.transform.position.y,Camera.main.transform.position.z),ORBIT_LERP*Time.deltaTime*50)
				:
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(l.transform.position.x,l.transform.position.y,Camera.main.transform.position.z),TRAVEL_LERP*Time.deltaTime*50)
				;
		
		//A moves the camera farther, S moves the camera closer
		if(Input.GetKey(KeyCode.A) && Camera.main.orthographicSize <= CAM_MAX_DIST)
			Camera.main.orthographicSize += CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.S) && Camera.main.orthographicSize >= CAM_MIN_DIST)
			Camera.main.orthographicSize -= CAM_MOVE_SPEED;
		
	
	}
	
	void OnGUI() {
		Starscript scpt = cur_star.GetComponent<Starscript>();
		if(scpt.is_sink)
			GUI.Label(new Rect(10, Screen.height-80,150,50), "YOU WIN!");
        GUI.Label(new Rect(10, Screen.height-65, 150, 50), "$pace Dollar$: "+currency);
		GUI.Label(new Rect(10, Screen.height-50,150,50), "Energy:");
   		GUI.DrawTexture(new Rect(10, Screen.height-30, energy*10, 20), gaugeTexture, ScaleMode.ScaleAndCrop, true, 10F); 
	}
		
}
	
