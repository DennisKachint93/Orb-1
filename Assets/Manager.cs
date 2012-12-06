using UnityEngine;
using System.Collections;
using System.IO;


public class Manager : MonoBehaviour {
	
	/***********************************************************************************************/
	/*********************IF YOU WANT TO CHANGE CONSTANTS, CHANGE THEM HERE*************************/
	/***********************************************************************************************/
	public static void ResetConstants() {
		/*GAMEPLAY CONTROLS */
		//Larger the error, the wider legal orbit radius 
		RADIAL_ERROR = 55;
		//larger the tan error, the easier it is to enter a star at a legal radius
		TAN_ERROR = 8;
		//the larger this number is, the sharper bends are
		BEND_FACTOR = 13;
		//larger the number, the faster the learth moves overall
		MOVEMENT_SPEED = 0.72f;
		//speed you move at without energy
		CONSTANT_SPEED = 5f;
		//larger the number, the faster learth moves when orbiting (doesn't affect speed, but makes aiming easier)
		ORBIT_SPEED_FACTOR = .35f;
		
		/*CAMERA CONTROLS */
		//the larger this number is, the more closely the camera follows learth while in orbit
		ORBIT_LERP = 3f;
		//the larger this number is, the more closely the camera follows learth while not in orbit
		TRAVEL_LERP = 2.5f;
		//How far the player is allowed to move the camera
		CAM_MAX_DIST = 5000;
		//How close the player is allowed to move the camera
		CAM_MIN_DIST = 50;
		//how fast the player can zoom in/out
		CAM_MOVE_SPEED = 10;
		//Camera orthographic size at start, higher = see more
		CAM_START_HEIGHT = 600;
		
		/*ENERGY CONTROLS */	
		//starting energy
		STARTING_ENERGY = 35f;
		//How much energy is reduced each frame while bending
		BEND_COST = 0.050f;
		//How much energy is reduced each frame while invincible
		INVINC_COST = .2f;
		//this much energy is subtracted each frame the learth is not in orbit
		FLYING_COST = .027f;
		//this much energy is subtracted each frame the learth is in orbit
		ORBITING_COST = .00025f;
		//this much energy is subtracted when they player hits the space bar to launch from a star
		LEAVING_COST = 0;
		//cost of a directional shift
		DIR_SHIFT_COST = 15;
		//determines whether shield is activeable
		SHIELD = false;
	 
	 	/*BLACK HOLE CONSTANTS*/
	 	//how fast black holes suck you into them when you are trapped--LOWER VALUES ARE SUCKIER
		BLACK_HOLE_SUCKINESS = 5f;	
		//energy it takes to escape a black hole on each press of space bar
		BH_ESCAPE_ENERGY = .2f;
		//distance you travel when you press space to escape a black hole
		BH_ESCAPE_DISTANCE = 300f;
		
		/*ALIEN CONSTANTS*/
		//when aliens are within distance, they start to suck your energy
		ALIEN_SUCKING_DISTANCE = 100f;
		//this much energy is sucked from player when alien is within alien_sucking_distance
		ALIEN_SUCKS_ENERGY = .025f;	
		   
		//black hole helper
		BLACK_HOLE_HELPER = false;

		
	}
	
	
	
	//Constants
	/*DON'T CHANGE ANY OF THESE. IT WILL HAVE NO EFFECT. TO ADD A NEW CONSTANT, DECLARE IT HERE AND INITIALIZE IN ResetConstants()*/
	//Larger the error, the wider legal orbit radius 
	public static int RADIAL_ERROR = 10;
	//larger the tan error, the easier it is to enter a star at a legal radius
	public static float TAN_ERROR = 8;
	//the larger this number is, the sharper bends are
	public static float BEND_FACTOR = 4;
	//larger the number, the faster the learth moves overall
	public static float MOVEMENT_SPEED = 0.72f;
	//speed you move at without energy
	public static float CONSTANT_SPEED = 1f;
	//larger the number, the faster learth moves when orbiting (doesn't affect speed, but makes aiming easier)
	private static float ORBIT_SPEED_FACTOR = .55f;
	
	/*CAMERA CONTROLS */
	//the larger this number is, the more closely the camera follows learth while in orbit
	public static float ORBIT_LERP = 3f;
	//the larger this number is, the more closely the camera follows learth while not in orbit
	public static float TRAVEL_LERP = 5f;
	//How far the player is allowed to move the camera
	public static float CAM_MAX_DIST = 5000;
	//How close the player is allowed to move the camera
	public static float CAM_MIN_DIST = 50;
	//how fast the player can zoom in/out
	public static float CAM_MOVE_SPEED = 10;
	//Camera orthographic size at start, higher = see more
	public static float CAM_START_HEIGHT = 600;
	
	/*ENERGY CONTROLS */	
	//starting energy
	public static float STARTING_ENERGY = 35f;
	//How much energy is reduced each frame while bending
	public static float BEND_COST = 0;
	//How much energy is reduced each frame while invincible
	private static float INVINC_COST = .2f;
	//this much energy is subtracted each frame the learth is not in orbit
	private static float FLYING_COST = .027f;
	//this much energy is subtracted each frame the learth is in orbit
	private static float ORBITING_COST = .00025f;
	//this much energy is subtracted when they player hits the space bar to launch from a star
	private static float LEAVING_COST = 0;
	//cost of a directional shift
	public static float DIR_SHIFT_COST = 15;
	//energy value for specific star colors
	public static float RED_ENERGY = 5f;
	public static float ORANGE_ENERGY = 10f;
	public static float YELLOW_ENERGY = 15f;
	public static float GREEN_ENERGY = 20f;
	public static float BLUE_ENERGY = 25f;
	public static float AQUA_ENERGY = 30f;
	public static float PURPLE_ENERGY = 35f;
	//energy from blowing up a star by flying through it while invincible
	public static float INVINC_ENERGY_BONUS = 50;

	//POWERUP BOOLEANS (these are special cases. try to avoid using powerup booleans. use a new script.)
    public static bool SHIELD = false;
    public static bool BLACK_HOLE_HELPER = false;
	
	
 	
 	/*BLACK HOLE CONSTANTS*/
 	//how fast black holes suck you into them when you are trapped--LOWER VALUES ARE SUCKIER
	private static float BLACK_HOLE_SUCKINESS = 5f;	
	//energy it takes to escape a black hole on each press of space bar
	private static float BH_ESCAPE_ENERGY = .2f;
	//distance you travel when you press space to escape a black hole
	private static float BH_ESCAPE_DISTANCE = 300f;
	
	/*ALIEN CONSTANTS*/
	//when aliens are within distance, they start to suck your energy
	public static float ALIEN_SUCKING_DISTANCE = 100f;
	//this much energy is sucked from player when alien is within alien_sucking_distance
	public static float ALIEN_SUCKS_ENERGY = .025f;	

	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	public GameObject coin;
	public GameObject plane;
	public GameObject alien;
	public GameObject bomb;
	public GameObject lbullet;
	public GameObject boost;
	public GameObject invinc;
	
	public static GameObject cur_star;
	public GameObject star_background_star;
	
	//actual objects used in script
	public static GameObject l, s, e, p;
	//particle trails for learth
	public GameObject red_learth_trail, orange_learth_trail, yellow_learth_trail, green_learth_trail,
		blue_learth_trail, aqua_learth_trail, purple_learth_trail, lt;
	public GameObject[] star_arr;
	public GameObject[] rip_arr;
	public GameObject[] coin_arr;
	public static  GameObject[] alien_arr = new GameObject[0];
	public GameObject[] boost_arr;
	public GameObject[] invinc_arr;
	public int numStars = 0;
	
	//positions past which learth will die. levels are always rectangles. 
	float level_x_max = -200000;
	float level_x_min = 200000;
	float level_y_max = -200000;
	float level_y_min = 200000;
	
	//learth-related variables
	public static float speed = 0;
	public static float energy;
	
	//store 3 last stars (refactor this into an array)
	public static GameObject lastStar;
	public static GameObject _lastStar;
	public static GameObject __lastStar;
	
	public static Vector3 tangent;
	public static bool clockwise = false;
	public static bool escaping_black_hole = false;
	public static Vector3 point_of_escape;
	public static int num_deaths = 0;
	
	//star colors and textures
	public Color orange = new Color(.9f, .45f, 0f, 1f);
	public Color dgray = new Color(.1f, .1f, .1f, 1f);
	public Color aqua = new Color(0, .4f, .8f, 1f);
	public Color purple = new Color(.4f, 0, .4f, 1f);
	public Color green = new Color(0,.4f, 0, 1f);
	public Texture tred;
	public Texture torange;
	public Texture tyellow;
	public Texture tgreen;
	public Texture tblue;
	public Texture taqua;
	public Texture tpurple;	
	public Texture tgray;
	
    //energy gauge
    public Texture gaugeTexture;

	//timer
	public float start_time;
	
	//performance tools
	public float updateInterval = 0.5F;
    private float lastInterval;
    private int frames = 0;
    private float fps;
    
	//game state
	GameObject game_state;
	static Game_State gscpt;
	
	//true if being attackign
	public static bool is_being_attacked = false;
	
	//explosions
	public Transform space_jump_effect;
	
	//testing audio
	public AudioClip test_aud;
	
	//flag set by the invincibility powerup
	public static bool IS_INVINCIBLE = false;
	
	
	void Start () {
		//performance
		lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        
		//find the gamestate object	
		game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		
		//let powerups know that it's time to active
		gscpt.in_game = true;
		
		//instantiate learth
		l = Instantiate (learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
	 	l.renderer.material.color = Color.red;	
		lt = Instantiate (red_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
		lt.transform.parent = l.transform;
		
		//initialize timer
		start_time = Time.time;
		
		//load next level
		LoadLevel(gscpt.level_order[gscpt.cur_level]);
		
		//set lastStar to first star (because it's the last star you visited)
		lastStar = star_arr[0];
		
		//instantiate background based on level constraints --this is going to change.
		for (int i = (int)level_x_min-2500; i <= (int)level_x_max+2500; i+=2500) {
			for (int j = (int)level_y_min-2500; j <= (int)level_y_max+2500; j+=2500) {
				p = Instantiate (plane, new Vector3(i, j, 100), transform.rotation) as GameObject;
				p.transform.Rotate(270, 0, 0);
			}
		}
		
		
	}
	
	//check to see if location of object determines level boundaries
	public void checkBoundaries(float x, float y) {
		if(x < level_x_min)
			level_x_min = x;
		if(x > level_x_max)
			level_x_max = x;
		if(y < level_y_min)
			level_y_min = y;
		if(y > level_y_max)
			level_y_max = y;
	}
	
	/* HOW LEVELS WORK
	 * in case you want to make/change a level
	 * 
	 * each level is stored in a text file in the Assets directory ex: level3.txt
	 * 
	 * the first line of that file contains the number of each type of level design element to be specified, delimited by commas
	 * ex: 1,0,0,2,0 == 1 static star, 0 space rips, 0 coins, 2 moving stars, 0 aliens
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
		
		//destroy aliens
		for(int i = 0; i < alien_arr.Length; i++)
			Destroy(alien_arr[i]);
		
		//reset energy
		//energy = 20f;
		
		
		//make sure learth is not tangent 
		Learth_Movement.isTangent = false;
	}
	
	//instantiates level design elements as specified in the text file in the argument
	public void LoadLevel(string fname) 
	{
		string line;
		char[] delim = {','} ;
		StreamReader file = new StreamReader(fname);
		string numels = file.ReadLine();
		
		//reset energy
		energy = STARTING_ENERGY;
		
		//get numbers of each type of element
		string[] sp = numels.Split(delim);
		int stars = int.Parse(sp[0]);
		int rips  = int.Parse(sp[1]);
		int coins = int.Parse(sp[2]);
		int mstars = int.Parse (sp[3]);
		int aliens = int.Parse(sp[4]);
		int rstars = int.Parse (sp[5]);
		int boosts = int.Parse (sp[6]);
		int invincs = int.Parse(sp[7]);
		
		//create all stars specified in the text file
		for(int i=0; i<stars;i++)
		{
			line = file.ReadLine();
			string [] args = line.Split(delim);
			
			//get color and texture objects
			Color starcol = Color.black;
			Texture startex = tgray;
			if(args[2] == "red") {
				starcol = Color.red;
				startex = tred;
			}  else if (args[2] == "orange") {
				starcol = orange;
				startex = torange;
			}  else if (args[2] == "yellow") {
				starcol = Color.yellow;
				startex = tyellow;
			}  else if (args[2] == "green") {
				starcol = green;
				startex = tgreen;
			}  else if(args[2] == "blue"){
				starcol = Color.blue;
				startex = tblue;
			}  else if(args[2] == "aqua") {
				starcol = aqua;
				startex = taqua;
			}  else if(args[2] == "purple") {
				starcol = purple;
				startex = tpurple;
			}
			
			//make the star
			GameObject newstar = CreateStar(float.Parse(args[0]),float.Parse(args[1]), starcol, startex, float.Parse(args[3]), bool.Parse(args[4]));
			
			//learth starts in orbit around first star specified
			if(i == 0) 
				GoToOrbit(newstar,float.Parse(args[3]));	
			//last star is the sink
			if(i == stars-1){
				Starscript scpt = newstar.GetComponent<Starscript>();
				scpt.is_sink = true;
			}  else {
				Starscript scpt = newstar.GetComponent<Starscript>();
				scpt.is_sink = false;
			}
			checkBoundaries(float.Parse(args[0]), float.Parse(args[1]));
		}
		
		
		
		//create all space rips specified in the text file
		for(int i = 0; i < rips;i++)
		{
			line = file.ReadLine();
			string[] args = line.Split(delim);
			
			CreateSpaceRip(float.Parse(args[0]),float.Parse(args[1]),float.Parse(args[2]),float.Parse(args[3]),float.Parse(args[4]));
			checkBoundaries(float.Parse(args[0]), float.Parse(args[1]));
		}
		
		//create all coins specified in the text file
		for(int i = 0; i < coins; i++)
		{
			line = file.ReadLine();
			string[] args = line.Split(delim);
			
			CreateCoin(float.Parse(args[0]),float.Parse(args[1]));
			checkBoundaries(float.Parse(args[0]), float.Parse(args[1]));
		}
		
		//create all moving stars specified in the text file
		for(int i = 0; i < mstars;i++)
		{
			line = file.ReadLine();
			string [] args = line.Split(delim);
			
			//get color and texture objects
			Color starcol = Color.black;
			Texture startex = tgray;
			if(args[2] == "red") {
				starcol = Color.red;
				startex = tred;
			}  else if (args[2] == "orange") {
				starcol = orange;
				startex = torange;
			}  else if (args[2] == "yellow") {
				starcol = Color.yellow;
				startex = tyellow;
			}  else if (args[2] == "green") {
				starcol = green;
				startex = tgreen;
			}  else if(args[2] == "blue"){
				starcol = Color.blue;
				startex = tblue;
			}  else if(args[2] == "aqua") {
				starcol = aqua;
				startex = taqua;
			}  else if(args[2] == "purple") {
				starcol = purple;
				startex = tpurple;
			}
			
			//make the star
			CreateMovingStar(float.Parse(args[0]),float.Parse(args[1]), 
				starcol, startex, float.Parse(args[3]), new Vector3(float.Parse (args[4]), float.Parse(args[5]),0), float.Parse(args[6]), bool.Parse(args[7]));
			checkBoundaries(float.Parse(args[0]), float.Parse(args[1]));
		}
		
		//create all aliens in the file
		for(int i = 0; i < aliens; i++)
		{	
			line = file.ReadLine();
			string[] args = line.Split(delim);
			CreateAlien(float.Parse(args[0]),float.Parse(args[1]));
			checkBoundaries(float.Parse(args[0]), float.Parse(args[1]));
		}
		
		//create a revolving star 
		for(int i = 0; i < rstars; i++)
		{
			line = file.ReadLine();
			string[] args = line.Split(delim);
			
			//get color and texture objects
			Color starcol = Color.black;
			Texture startex = tgray;
			if(args[4] == "red") {
				starcol = Color.red;
				startex = tred;
			}  else if (args[4] == "orange") {
				starcol = orange;
				startex = torange;
			}  else if (args[4] == "yellow") {
				starcol = Color.yellow;
				startex = tyellow;
			}  else if (args[4] == "green") {
				starcol = green;
				startex = tgreen;
			}  else if(args[4] == "blue"){
				starcol = Color.blue;
				startex = tblue;
			}  else if(args[4] == "aqua") {
				starcol = aqua;
				startex = taqua;
			}  else if(args[4] == "purple") {
				starcol = purple;
				startex = tpurple;
			}
			
			CreateRevolvingStar(float.Parse (args[0]),float.Parse(args[1]),float.Parse(args[2]),float.Parse(args[3]),
				starcol, startex, float.Parse(args[5]),float.Parse (args[6]));
			checkBoundaries(float.Parse(args[0]), float.Parse(args[1]));
			checkBoundaries(float.Parse(args[2]), float.Parse(args[3]));
		}
		
		//create boosts
		for(int i = 0; i < boosts; i++) {
			line = file.ReadLine();
			string[] args = line.Split(delim);
			CreateBoost(float.Parse(args[0]), float.Parse(args[1]));
		}
		
		//create invinces
		for(int i = 0; i < invincs; i++) {
			line = file.ReadLine();
			string[] args = line.Split(delim);
			CreateInvinc(float.Parse(args[0]), float.Parse(args[1]));
		}
		
	}
	
	//puts learth in orbit given a valid radius
	public static void GoToOrbit(GameObject star, float radius)
	{
		l.transform.position = new Vector3(star.transform.position.x+radius+RADIAL_ERROR,star.transform.position.y,0);
		cur_star = star;
		s = star;
		Learth_Movement.isTangent = true;
	}
	
	//instantiates a revolving star at the location and around the point provided
	GameObject CreateRevolvingStar(float x, float y, float r_point_x, float r_point_y,Color color, Texture texture,float size, float speed)	
	{		
		GameObject rstar = CreateStar(x,y,color,texture,size);
		Starscript scpt  = rstar.GetComponent<Starscript>();
		scpt.is_revolving = true;
		scpt.rpoint = new Vector3(r_point_x,r_point_y,0);
		scpt.rspeed = speed;
		return rstar;
	}
	
	//instantiates an alien at the location provided
	GameObject CreateAlien(float x, float y) 
	{
		GameObject alien_actual = Instantiate (alien, new Vector3(x,y,0),new Quaternion(0,0,0,0)) as GameObject;
		
		//expand and put in array
		GameObject[] temp_arr = new GameObject[alien_arr.Length+1];
		for(int i=0;i<alien_arr.Length;i++)
			temp_arr[i] = alien_arr[i];
		alien_arr = temp_arr;
		alien_arr[alien_arr.Length-1] = alien_actual;
		
		return alien_actual;
	}
	
	//instantiates a boost pick at the location provided
	GameObject CreateBoost(float x, float y) {
		GameObject boost_actual= Instantiate(boost, new Vector3(x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
		//put rip in boost_arr for unloading
		GameObject[] temp_arr = new GameObject[boost_arr.Length+1];
		for(int i=0;i<boost_arr.Length;i++)
			temp_arr[i] = boost_arr[i];
		boost_arr = temp_arr;
		boost_arr[boost_arr.Length-1] = boost_actual;
		return boost_actual;
	}
	
	GameObject CreateInvinc(float x, float y) {
			
		GameObject invinc_actual= Instantiate(invinc, new Vector3(x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
		//put rip in boost_arr for unloading
		GameObject[] temp_arr = new GameObject[invinc_arr.Length+1];
		for(int i=0;i<invinc_arr.Length;i++)
			temp_arr[i] = invinc_arr[i];
		invinc_arr = temp_arr;
		invinc_arr[invinc_arr.Length-1] = invinc_actual;
		return invinc_actual;
		
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
	GameObject CreateMovingStar(float x, float y, Color color, Texture texture, float size, Vector3 dir, float speed, bool bandf = false)
	{
		//bool bandf = false;
		GameObject mstar = CreateStar(x,y,color,texture,size);
		Starscript scpt  = mstar.GetComponent<Starscript>();
		scpt.is_moving = true;
		if(bandf){
			scpt.destination = dir;
			scpt.start_loc = mstar.transform.position;
		}
		else{
			scpt.dir = dir;
		}
		scpt.speed = speed;
		return mstar;
	}

	//instantiates star from prefab at given xy location and of given characteristics
	GameObject CreateStar(float x, float y, Color color, Texture texture, float size, bool isBlackHole = false, bool isExplodingStar=false)
	{
		GameObject starE = Instantiate (star, new Vector3(x,y,0), new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = color;
		starscript.t = texture;
		starscript.starSize = size; 
		//starscript.isBlackHole = false;
		starscript.isBlackHole = isBlackHole;
		
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
	
	//call this anytime something kills the player
	public static void Die()
	{
		gscpt.times_died++;
		if(energy > STARTING_ENERGY)
			energy -= 25;
		
		//check the last 3 stars and go to the first one that hasn't been destroyed
		//if they've all been destroyed, reload the level
		//this can probably be written better
		Starscript scpt = lastStar.GetComponent<Starscript>();
		if(!scpt.is_destroyed) {
			GoToOrbit(lastStar,scpt.orbitRadius);
		} else {
			Starscript _scpt = _lastStar.GetComponent<Starscript>();
			if(!_scpt.is_destroyed) {
				GoToOrbit(_lastStar,_scpt.orbitRadius);
			} else {
				Starscript __scpt = __lastStar.GetComponent<Starscript>();
				if(!__scpt.is_destroyed) {
					GoToOrbit(__lastStar,__scpt.orbitRadius);
				} else {
					Application.LoadLevel(Application.loadedLevel);
				}
			}
		}
		
		
	}
	
	//reloads the scene and modifies whatever we want to modify when the scene gets reloaded
	public static void ResetLevel() {
		Application.LoadLevel(Application.loadedLevel);	
		energy = STARTING_ENERGY;
	}
	
	
	void Update () {
		
		//performance
		++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval) {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
        }
		
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
		//f increases energy by 1
		if(Input.GetKeyDown(KeyCode.F))
			energy++;
		//P advances to next level
		if(Input.GetKeyDown(KeyCode.P))
		{
			//increment level counter
			gscpt.cur_level++;
			
			//set in game to false
			gscpt.in_game = false;
			
			//open the ship outfitter
			Application.LoadLevel("Ship_Outfitter");
			
		}
		//O goes to outfitter before previous level
		if(Input.GetKeyDown(KeyCode.O))
		{
			gscpt.in_game = false;
			Application.LoadLevel("Ship_Outfitter");
		}
		//l prints learth's current position
		if(Input.GetKeyDown(KeyCode.L)) {
		}
		/*********************END DEBUGGING CONTROLS*****************/
		
		//Speed increases logarithmically with energy
		speed = Mathf.Log(energy)*MOVEMENT_SPEED + CONSTANT_SPEED;
		
		//end level if reach sink
		Starscript starscpt = cur_star.GetComponent<Starscript>();
		if(starscpt.is_sink) {
		
			//record delivered energy
			gscpt.energy_delivered = energy;
			
			//increment level counter
			gscpt.cur_level++;
			
			//set in game to false
			gscpt.in_game = false;
			
			//open the ship outfitter
			Application.LoadLevel("Ship_Outfitter");
		}

		//bending
		if(Input.GetKey(KeyCode.Q))
		{
			energy -= BEND_COST;
			Learth_Movement.lastPos.RotateAround(l.transform.position,Vector3.forward,Time.deltaTime*BEND_FACTOR);
		}
		else if (Input.GetKey(KeyCode.W))
		{		
			energy -= BEND_COST;
			Learth_Movement.lastPos.RotateAround(l.transform.position,Vector3.forward,-1*Time.deltaTime*BEND_FACTOR);
		}
		
		//temporary invincibility, logic implemented in Learth_Movement.cs 
		if(Input.GetKey(KeyCode.D) && SHIELD)
		{
			l.renderer.material.color = Color.green;
			energy -= INVINC_COST;
		}
		
		//change learth color back to normal - causes color change of learth when bombs fired with key D as well
	/*	if(Input.GetKeyUp (KeyCode.D))
			l.renderer.material.color = Color.red; */
			
		//keep track of aliens potentially turning off powerups
		is_being_attacked = false;
		for(int j = 0; j<alien_arr.Length; j++){
			if(alien_arr[j] != null && Vector3.Distance(l.transform.position, alien_arr[j].transform.position) < ALIEN_SUCKING_DISTANCE)
			{
				is_being_attacked = true;
			} 
        }

		//Death conditions
		//if you run out of energy, you die, but you get a little energy back
	/*	if(energy < 1)
		{
			Die ();
		}  */
		
		//if you travel outside the bounds of the level, you die
		//max/mins are calcualted based on star centers, not rotations, so you die immediately if you start at the edge without the +/- 100s
		if(l.transform.position.x > level_x_max+1000
			|| l.transform.position.x < level_x_min-1000
			|| l.transform.position.y > level_y_max+1000
			|| l.transform.position.y < level_y_min-1000)
		{
			Die ();
		}
		
		
		//if learth is tangent to star s
		if (Learth_Movement.isTangent) {
			//if in orbit, decrease energy at correct rate
			energy -= ORBITING_COST;
			
			//if learth is orbiting a moving planet, translate it with the planet to maintain circular orbit
			Starscript scpt = cur_star.GetComponent<Starscript>();
					
			//translate learth for moving stars to compensate for star movement
			if(scpt.is_revolving || scpt.spiral || scpt.is_moving)
			{
				//difference in star positions since last frame
				Vector3 vec = cur_star.transform.position - scpt.last_position;
				
				//prevents incorrect movement in the case that the last position variable in the star hasn't been set correctly
				//this happens the first time you try to translate with a star
				//this is kind of a bad solution, but it will work until we have time to reevaluate the movement code
				if(!(Mathf.Abs(vec.x) > 10)) {
					l.transform.Translate(vec.x,vec.y,0,Space.World);
					Learth_Movement.lastPos.Translate(vec.x,vec.y,0,Space.World);
				}
				
				//set last position
				scpt.last_position = cur_star.transform.position;
			}
			//if star is a black hole and you haven't traveled boost distance after pressing space bar, get sucked into center of black hole
			if(scpt.isBlackHole && !escaping_black_hole) {
				//speed up to increase dramatic effect
				speed *= BLACK_HOLE_SUCKINESS/2;
				Vector3 perp = l.transform.position - cur_star.transform.position;
				perp.Normalize();
				l.transform.position -= perp/BLACK_HOLE_SUCKINESS;
			}	
			//if star is black hole and you recently pressed space to escape, don't get sucked and but travel towards outer edges of black hole
			else if(scpt.isBlackHole && escaping_black_hole) 
				l.transform.position = Vector3.Lerp(l.transform.position, point_of_escape + BH_ESCAPE_DISTANCE*speed*Learth_Movement.velocity.normalized, Time.deltaTime);	
			//rotate around star s
			if (clockwise){
					l.transform.RotateAround(s.transform.position, Vector3.forward, 
						-(speed > 1 ? speed : 1)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime)*ORBIT_SPEED_FACTOR);
			}
			else  {
				l.transform.RotateAround(s.transform.position, 
				Vector3.forward, (speed > 1 ? speed : 1)/(Vector3.Distance(l.transform.position, s.transform.position)*Time.deltaTime/ORBIT_SPEED_FACTOR));
			}
			
			//EXPLODING STAR 
			if(scpt.isExplodingStar)
			{
				//Stars Exploding timer begins
				scpt.BoomTime();
				//waits 5 seconds
				if(scpt.waitsec(scpt.blowuptime))
				{
					//make learth accelerate away from star and removestar
					Learth_Movement.isTangent = false;
					Learth_Movement.lastPos.position=scpt.transform.position;
					scpt.removeStar(scpt.BIG_EXPLOSION);
				}
				
			}
			
			
			
			
			//if space bar is pressed, accelerate away from star. 
			if (Input.GetKeyDown(KeyCode.Space)) {
				//if star is a black hole, then lerp your way out
				if (scpt.isBlackHole) {
					//each time space bar is pressed energy is consumed
					energy -= BH_ESCAPE_ENERGY;
					escaping_black_hole = true;
					point_of_escape = l.transform.position;
			 		l.transform.position = Vector3.Lerp(l.transform.position, point_of_escape + BH_ESCAPE_DISTANCE*speed*Learth_Movement.velocity.normalized, Time.deltaTime);
					//if outside of black hole radius, then black hole stops having an effect on learth
					if (Vector3.Distance(l.transform.position, s.transform.position) >= scpt.orbitRadius/2) {
						escaping_black_hole = false;
						Learth_Movement.isTangent = false;
						Learth_Movement.lastPos.position = l.transform.position - Learth_Movement.velocity.normalized*speed;
					}				
				}
				//if star is a normal star, shoot out of orbit immediately with energy cost
				else {
					//play sound
					
					GameObject go = GameObject.Find("Alien_Exp_Sound");
					Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
					ascpt.leaving_star.Play();
						
					
					if(scpt.isExplodingStar){
						//reset all Exploding Star variables if left on time
						scpt.explodetimer=0;
						scpt.renderer.material.color = Color.white;
						scpt.blinkspeed=scpt.resblink;
					}
					Learth_Movement.isTangent = false;
					__lastStar = _lastStar;
					_lastStar = lastStar;
					lastStar = s;			
					energy -= LEAVING_COST;
					Learth_Movement.lastPos.position = l.transform.position - Learth_Movement.velocity.normalized*speed;
				}
			}
			//if space was recently pressed but learth hasn't traveled full black hole escape distance, set escaping_black_hole to true, but if full distance has been traveled set to false 
			Vector3 perp2 = l.transform.position - cur_star.transform.position;
			perp2.Normalize();			
			if (!escaping_black_hole && scpt.isBlackHole && Vector3.Distance(l.transform.position, Vector3.Distance(point_of_escape, cur_star.transform.position)*perp2) <= BH_ESCAPE_DISTANCE) 
				escaping_black_hole = true;			 		
			else
			 	escaping_black_hole = false;
		}
		//if earth is not tangent to any star
		else if (!Learth_Movement.isTangent) {
			//if not in orbit, decrease energy at correct rate
			energy -= FLYING_COST;
			
			//loop through array and calculate tangent vectors to every star
			for (int i = 0; i < star_arr.Length ; i++){
				s = star_arr[i];
				if (s != null) {
					Starscript sscript = s.GetComponent<Starscript>();
					Vector3 l_movement = Learth_Movement.velocity;
					Vector3 star_from_learth = s.transform.position - l.transform.position;
					Vector3 projection = Vector3.Project (star_from_learth, l_movement);
					tangent = projection + l.transform.position;
					//if planet is within star's orbital radius, set isTangent to true
					float innerOrbit, outerOrbit;
					if (sscript.isBlackHole) {
						innerOrbit = sscript.orbitRadius;
						outerOrbit = sscript.orbitRadius/2;
					}
					else {
						outerOrbit = -RADIAL_ERROR;
						innerOrbit = RADIAL_ERROR;
					}
					if (s != lastStar 
					&& Vector3.Distance(s.transform.position, l.transform.position) >= (sscript.orbitRadius - innerOrbit) 
					&& Vector3.Distance(s.transform.position, l.transform.position) <= (sscript.orbitRadius - outerOrbit) 
					&& Vector3.Distance (tangent, l.transform.position) <= TAN_ERROR) 
					{	
						//play entry sound
						GameObject go = GameObject.Find("Alien_Exp_Sound");
						Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
						ascpt.entering_star.Play();
						
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
						
						if (!sscript.isBlackHole) {
							//add appropriate energy value depending on color of star and change learth's trail color
							if(sscript.c == Color.red) {
								energy += RED_ENERGY;
								Destroy(lt);
								lt = Instantiate (red_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								
								l.renderer.material.color = Color.red;
							}  else if (sscript.c == orange) {
								energy += ORANGE_ENERGY;
								Destroy(lt);
								lt = Instantiate (orange_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								l.renderer.material.color = orange;
							}  else if (sscript.c == Color.yellow) {
								energy += YELLOW_ENERGY;
								Destroy(lt);
								lt = Instantiate (yellow_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								l.renderer.material.color = Color.yellow;
							}  else if (sscript.c == green) {
								Destroy(lt);
								lt = Instantiate (green_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								energy += GREEN_ENERGY;
								l.renderer.material.color = green;
							}  else if(sscript.c == Color.blue){
								energy += BLUE_ENERGY;
								Destroy(lt);
								lt = Instantiate (blue_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								l.renderer.material.color = Color.blue;
							}  else if(sscript.c == aqua) {
								energy += AQUA_ENERGY;
								Destroy(lt);
								lt = Instantiate (aqua_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								l.renderer.material.color = aqua;
							}  else if(sscript.c == purple) {
								energy += PURPLE_ENERGY;
								Destroy(lt);
								lt = Instantiate (purple_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
								lt.transform.parent = l.transform;
								l.renderer.material.color = purple;
							}
							sscript.c = dgray;
							sscript.t = tgray;
						}
						break;
					}
				}
			}
		}
	}
	
	void OnGUI() {
		//performance
		GUI.Label(new Rect(10,10,150,50), "FPS: "+fps.ToString("f2"));
		Starscript scpt = cur_star.GetComponent<Starscript>();
		if(scpt.is_sink) {
			gscpt.time_to_complete = (Time.time - start_time);
			gscpt.num_stars = star_arr.Length;
		/*	GUI.Label(new Rect(10, Screen.height-80,150,50), "YOU WIN!");
			GUI.Label (new Rect(10, Screen.height-95,150,50), "Time: "+(Time.time - start_time)); */
		}
		
		//ammo stats
		if(gscpt.bomb_on)
			GUI.Label(new Rect(10,Screen.height-130,150,50),"Bombs: "+gscpt.bomb_ammo);
		if(gscpt.capac_on)
			GUI.Label(new Rect(10,Screen.height-100,150,50),"Charges: "+gscpt.capac_ammo);
		if(gscpt.jump_on)
			GUI.Label(new Rect(10,Screen.height-160,150,50),"Jumps: "+gscpt.jump_ammo);
		if(gscpt.gun_on)
			GUI.Label(new Rect(10,Screen.height-190,150,50),"Torpedos: "+gscpt.gun_ammo);
		if(gscpt.bend_on)
			GUI.Label(new Rect(10,Screen.height-70,150,50),"Bending fluid: "+gscpt.bend_ammo);
		
		
      //  GUI.Label(new Rect(10, Screen.height-65, 150, 50), "Space Coins: "+(gscpt.num_coins));
		GUI.Label(new Rect(10, Screen.height-50,150,50), "Energy:");
   		GUI.DrawTexture(new Rect(10, Screen.height-30, energy*3, 20), gaugeTexture, ScaleMode.ScaleAndCrop, true, 10F); 
	}
		
}
	

