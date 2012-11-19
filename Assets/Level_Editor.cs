using UnityEngine;
using System.Collections;
using System.IO;

public class Level_Editor : MonoBehaviour {
	
	//starting variables for level
	public float cam_start_height;
	public float energy;
	
	//These controls are for our convenience as level editiors -- same functions as in manager
	//maximum distance from scene
	private float CAM_MAX_DIST = 10000;
	//minumum distance from scene
	private float CAM_MIN_DIST = 50;
	//maximum "size" of scene in terms of camera's ability to move
	private float SCENE_BOUNDARY = 20000;
	//how we can zoom in and out
	private float CAM_MOVE_SPEED = 4;
	//Camera orthographic size at start of level editing scene, not actual level we are creating, higher = see more
	private float CAM_START_HEIGHT = 500;
	
	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	public GameObject coin;
	public GameObject alien;
	
	//actual objects used in script
	public static GameObject l, s, e;
	public GameObject[] star_arr;
	public GameObject[] rip_arr;
	public GameObject[] coin_arr;
	public GameObject[] mstar_arr;
	public GameObject[] rstar_arr;
	public GameObject[] alien_arr;
	public int numStars = 0;
	
	//star colors
	public Color orange = new Color(1f, .6f, 0f, 1f);
	public Color dgray = new Color(.1f, .1f, .1f, 1f);
	public Texture tred;
	public Texture torange;
	public Texture tyellow;
	public Texture twhite;
	public Texture tgray;
	public Texture tblue;
	
	
	public bool starbut = false;
	public bool validcolor = false;
	public Color starcol;
	public float starsize;
	public Texture startex;
	public string isaycolor;
	public string isaysize;
	
	//Coin button
	public bool coin_button = false;
	
	//linearly moving star button
	public bool mstar_button = false;
	public string isay_x_dir;
	public string isay_y_dir;
	public string isay_speed;
	
	
	//alien button
	public bool alien_button = false;
	
	
	//Space rip button
	public bool spaceRipButton = false;
	
	//black hole button
	public bool blackHoleButton = false;
	
	//revolving star button
	public bool rstar_button = false;
	//true if waiting for revolution center point
	private bool waiting_for_point = false;
	//location stored while waiting for point to revolve around
	private Vector3 rev_s_location;
	
	void Start () {
		
		//set camera height for level editing
		Camera.main.orthographicSize = CAM_START_HEIGHT;
		
	}
	
	//instantiates a space rip from prefab at given location and of given dimensions, with given rotation (default = 0), returns reference to that object
	GameObject CreateSpaceRip(float x, float y, float width, float height, float rotation = 0)
	{
		GameObject rip_actual = Instantiate (rip, new Vector3 (x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		rip_actual.transform.localScale += new Vector3(width,height,0);
		rip_actual.transform.Rotate(new Vector3(0,0,rotation));
		
		//put rip in rip_arr for saving
		GameObject[] temp_arr = new GameObject[rip_arr.Length+1];
		for(int i=0;i<rip_arr.Length;i++)
			temp_arr[i] = rip_arr[i];
		rip_arr = temp_arr;
		rip_arr[rip_arr.Length-1] = rip_actual;
		
		return rip_actual;
	}
	
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

	
	//instantiates star or black hole from prefab at given xy location and of given characteristics
	GameObject CreateStar(float x, float y, Color color, Texture texture, float size, bool staticstar = true, bool isBlackHole = false)
	{
		GameObject starE = Instantiate (star, new Vector3(x,y,20), new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = color;
		starscript.t = texture;
		starscript.starSize = size; 
		starscript.isBlackHole = isBlackHole;
		
		if(isBlackHole)
			Starscript.BLACK_HOLE_HELPER = true;
		
		if(staticstar)
		{
		//expand and copy star_arr - if loading a level takes too long, this can be optimized
		GameObject[] temp_arr = new GameObject[star_arr.Length+1];
		for(int i=0;i<star_arr.Length;i++)
			temp_arr[i] = star_arr[i];
		star_arr = temp_arr;
		star_arr[star_arr.Length-1] = starE;
		numStars++;
		}
		return starE;
	}
	
	GameObject CreateMovingStar(float x, float y, Color color, Texture texture, float size, Vector3 dir, float speed)
	{
		GameObject mstar = CreateStar(x,y,color,texture,size,false);
		Starscript scpt  = mstar.GetComponent<Starscript>();
		scpt.is_moving = true;
		scpt.dir = dir;
		scpt.speed = speed;
		scpt.editor_freeze = true;
		
		GameObject[] temp_arr = new GameObject[mstar_arr.Length+1];
		for(int i=0;i<mstar_arr.Length;i++)
			temp_arr[i] = mstar_arr[i];
		mstar_arr = temp_arr;
		mstar_arr[mstar_arr.Length-1] = mstar;
		return mstar;
	}
	
	//instantiates a revolving star at the location and around the point provided
	GameObject CreateRevolvingStar(float x, float y, float r_point_x, float r_point_y,Color color, Texture texture,float size, float speed)	
	{		
		GameObject rstar = CreateStar(x,y,color,texture,size, false);
		Starscript scpt  = rstar.GetComponent<Starscript>();
		scpt.is_revolving = true;
		scpt.rpoint = new Vector3(r_point_x,r_point_y,0);
		scpt.rspeed = speed;
		scpt.editor_freeze = true;
		
		GameObject[] temp_arr = new GameObject[rstar_arr.Length+1];
		for(int i=0;i<rstar_arr.Length;i++)
			temp_arr[i] = rstar_arr[i];
		rstar_arr = temp_arr;
		rstar_arr[rstar_arr.Length-1] = rstar;
		
		return rstar;
	}
	
	void Update () {
		//A moves the camera farther,  S moves the camera closer, arrow keys move camera position up, down, left and right
		if(Input.GetKey(KeyCode.A) && Camera.main.orthographicSize <= CAM_MAX_DIST)
			Camera.main.orthographicSize += CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.S) && Camera.main.orthographicSize >= CAM_MIN_DIST)
			Camera.main.orthographicSize -= CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.RightArrow) && Camera.main.transform.position.x < SCENE_BOUNDARY) 
			Camera.main.transform.Translate(new Vector3(CAM_MOVE_SPEED, 0, 0));
		if(Input.GetKey(KeyCode.LeftArrow) && Camera.main.transform.position.x > -SCENE_BOUNDARY) 
			Camera.main.transform.Translate(new Vector3(-CAM_MOVE_SPEED, 0, 0));
		if(Input.GetKey(KeyCode.UpArrow) && Camera.main.transform.position.y > -SCENE_BOUNDARY) 
			Camera.main.transform.Translate(new Vector3(0, CAM_MOVE_SPEED, 0));
		if(Input.GetKey(KeyCode.DownArrow) && Camera.main.transform.position.y < SCENE_BOUNDARY) 
			Camera.main.transform.Translate(new Vector3(0, -CAM_MOVE_SPEED, 0));
			
		//after a specific button has been pressed, corresponding object is instantiated on mouse click
		
		if(Input.GetMouseButtonDown(0) && Input.mousePosition.x > 93) {
        	Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			//if space rip button is pressed, instantiate space rip on mouse click 
			if (spaceRipButton) {
				CreateSpaceRip (p.x, p.y, 30, 30);	
			}
			//if user has entered a valid color and star button has been pushed, instantiate star anywhere but on pop-up box location
	        if (starbut && validcolor) {				
				//instantiate star with user-defined color and radius
				Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                starsize = float.Parse(isaysize);
				CreateStar(location.x,location.y,starcol,startex,starsize);
       		}
       		
			//if user has entered a valid color and the moving star button is clicked, create a moving star
			if(mstar_button && validcolor)
			{
				Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            	starsize = float.Parse(isaysize);
            	CreateMovingStar(location.x, location.y,starcol,startex,starsize,
				new Vector3(float.Parse(isay_x_dir),float.Parse (isay_y_dir),0),float.Parse(isay_speed));
			}
			
			//if the coin button been pushed, make coins
			if(coin_button)
			{
				Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				CreateCoin(location.x,location.y);
			}
			
			//revolving star
			//behavior if placing star location
			if(rstar_button && validcolor && !waiting_for_point)
			{
				rev_s_location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				waiting_for_point = true;				
			} 
			//behavior for placing rotation point
			else if(rstar_button && validcolor && waiting_for_point) {
            	starsize = float.Parse(isaysize);
 		       	Vector3 rotation_point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            	CreateRevolvingStar(rev_s_location.x, rev_s_location.y,rotation_point.x,rotation_point.y,starcol,
					startex,starsize,float.Parse(isay_speed));
				waiting_for_point = false;
			}
			
			
			//aliens
			if(alien_button) {
				Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				CreateAlien(location.x,location.y);					
			}
			
			if(blackHoleButton) {				
				Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                starsize = float.Parse(isaysize);
				CreateStar(location.x,location.y,starcol,startex,starsize,true,true);
			}
			
		}
	}   
	
	
	//outputs level to a text file
	void SaveLevel(string path) {
		//check if file exists
		if(File.Exists(path))
		{
			//output a GUI message alerting the user to pick a different name
			
		} else {
			//write info to file
			using (StreamWriter sw = File.CreateText(path))
    		{
				//write lengths header (update this line as saving is implemented for other elements)
    			sw.WriteLine(star_arr.Length+","+rip_arr.Length+","+coin_arr.Length+","+mstar_arr.Length+","+alien_arr.Length+","+rstar_arr.Length);
				
				//stars
				for(int i = 0; i < star_arr.Length;i++)
				{
					Starscript scpt = star_arr[i].GetComponent<Starscript>();
					string color = "black";
					if(scpt.c.Equals(Color.blue))
					{
						color = "blue";
					} else if(scpt.c.Equals(Color.white))
					{
						color = "white";
					} else if (scpt.c.Equals(Color.yellow))
					{
						color = "yellow";
					} else if(scpt.c.Equals(Color.red))
					{
						color = "red";
					} 					
					string black_hole = "false";
					if(scpt.isBlackHole) {
						black_hole = "true";
					}
					//write to file if star is a static
					if(!scpt.is_moving && !scpt.is_revolving)
						sw.WriteLine(star_arr[i].transform.position.x+","+star_arr[i].transform.position.y+","+color+","+scpt.starSize+","+black_hole);
				}
				
				//rips
				for(int i = 0; i < rip_arr.Length;i++)
				{
					sw.WriteLine(rip_arr[i].transform.position.x+","+rip_arr[i].transform.position.y+",30,30,0");
				}
				//coins
				for(int i = 0; i < coin_arr.Length; i++)
				{
					sw.WriteLine(coin_arr[i].transform.position.x+","+coin_arr[i].transform.position.y);	
				}
				//moving stars
				for(int i = 0; i < mstar_arr.Length; i++)
				{
				//	Debug.Log ("trying to write a star");
					Starscript scpt = mstar_arr[i].GetComponent<Starscript>();
					string color = "red";
					if(scpt.c.Equals(Color.blue))
					{
						color = "blue";
					} else if(scpt.c.Equals(Color.white))
					{
						color = "white";
					} else if (scpt.c.Equals(Color.yellow))
					{
						color = "yellow";
					}
					sw.WriteLine(mstar_arr[i].transform.position.x+","+mstar_arr[i].transform.position.y+","+color+","+scpt.orbitRadius
						+","+scpt.dir.x+","+scpt.dir.y+","+scpt.speed);
				}
				
				//aliens
				for(int i = 0; i < alien_arr.Length; i++)
				{
					sw.WriteLine(alien_arr[i].transform.position.x+","+alien_arr[i].transform.position.y);
				}
				
				//revolving stars
				for(int i = 0; i < rstar_arr.Length; i++)
				{
					Starscript scpt = rstar_arr[i].GetComponent<Starscript>();
					string color = "red";
					if(scpt.c.Equals(Color.blue))
					{
						color = "blue";					
					} else if(scpt.c.Equals(Color.white))
					{
						color = "white";
					} else if (scpt.c.Equals(Color.yellow))
					{
						color = "yellow";
					}
					sw.WriteLine(rstar_arr[i].transform.position.x+","+rstar_arr[i].transform.position.y+","+scpt.rpoint.x+","
						+scpt.rpoint.y+","+color+","+scpt.orbitRadius+","+scpt.rspeed);
					
				}
    		}
				
			
		}
	}
    
	 void OnGUI() {        
		//toolbar area
		GUI.backgroundColor = Color.red;
		GUI.Box(new Rect(0, 0, 93, Screen.height), "");
		//spacerip button -- after pressing button, user can click to add space rips in locations 
        if (GUI.Button(new Rect(10, 15, 75, 25), "Space Rip")) {
			spaceRipButton = !spaceRipButton;
			starbut = false;
			coin_button = false;
			mstar_button = false;
			rstar_button = false;
			alien_button = false;
			blackHoleButton = false;			
		}
		//star button -- after pressing button, user can enter star size and color and then click to add space rips to locations
		if(GUI.Button(new Rect(10, 45, 70, 25), "star")){
			starbut = !starbut;
			spaceRipButton = false;
			coin_button = false;
			mstar_button = false;
			rstar_button = false;
			alien_button = false;
			blackHoleButton = false;			
		}
		//coin button
		if(GUI.Button (new Rect(10,75,70,25), "coin")) {
			coin_button = !coin_button;
			spaceRipButton = false;
			starbut = false;
			mstar_button = false;
			rstar_button = false;
			alien_button = false;
			blackHoleButton = false;		
		}
		//moving star button
		if(GUI.Button (new Rect(10,105,70,25), "mstar")) {
			mstar_button = !mstar_button;
			spaceRipButton = false;
			starbut = false;
			rstar_button = false;
			coin_button = false;
			alien_button = false;
			blackHoleButton = false;		
		}
		//revolving star button
		if(GUI.Button (new Rect(10,135,70,25), "rstar")) {
			rstar_button = !rstar_button;
			spaceRipButton = false;
			starbut = false;
			coin_button = false;
			mstar_button = false;
			alien_button = false;
			blackHoleButton = false;		
		}
		//alien button
		if(GUI.Button(new Rect (10,165,70,25), "alien")) {
			alien_button = !alien_button;
			spaceRipButton = false;
			starbut = false;
			rstar_button = false;
			coin_button = false;
			mstar_button = false;
			blackHoleButton = false;
		}
		//black hole button
		if(GUI.Button(new Rect (10, 195, 75, 25), "black hole")) {
			blackHoleButton = !blackHoleButton;
			spaceRipButton = false;
			starbut = false;
			rstar_button = false;
			coin_button = false;
			mstar_button = false;			
	 }	
		
		int ystart = 240; //starting y value of pop-up box
		//if star button has been clicked, pop up box to change star color/size
 		if(starbut || mstar_button || rstar_button || blackHoleButton){
			if(GUI.Button(new Rect(25, ystart+10, 45, 30), "Done")){
				starbut = false;
				mstar_button = false;
				rstar_button = false;
				blackHoleButton = false;
			}
			GUI.Label ( new Rect (10,ystart+50,25,20), "size");
			isaysize = GUI.TextField(new Rect(45, ystart+52, 40, 20), isaysize, 25);
			if (!blackHoleButton) {
				GUI.Label ( new Rect (10, ystart+80,30,20), "color");
				isaycolor = GUI.TextField(new Rect(45, ystart+82, 40, 20), isaycolor, 25);
				if(isaycolor == "blue"){
					starcol = Color.blue;
					startex = tblue;
					validcolor = true;
				}
				if(isaycolor == "red"){
					starcol = Color.red;
					startex = tred;
					validcolor = true;
				}
				if(isaycolor == "white"){
					starcol = Color.white;
					startex = twhite;
					validcolor = true;
				}
				if(isaycolor == "gray"){
					starcol = Color.gray;
					startex = tgray;
					validcolor = true;
				}
				if(isaycolor == "yellow"){
					starcol = Color.yellow;
					startex = tyellow;
					validcolor = true;
				}
			}
			else {
				starcol = Color.black;
				startex = twhite;
				validcolor = true;
			}
		}

		//if moving star, get direction of movement and speed
		if(mstar_button){
			GUI.Label ( new Rect (10,ystart+110,25,20), "xdir");
			isay_x_dir = GUI.TextField(new Rect(45, ystart+112, 40, 20), isay_x_dir, 25);
			GUI.Label ( new Rect (10, ystart+140,25,20), "ydir");
			isay_y_dir = GUI.TextField(new Rect(45, ystart+142, 40, 20), isay_y_dir, 25);
			GUI.Label(new Rect(10,ystart+170,25,20), "mspeed");
			isay_speed = GUI.TextField(new Rect(45, ystart+172, 40, 20), isay_speed, 25);
		}
		//if revolving star, get rotation point and speed
		if(rstar_button) {
			GUI.Label(new Rect(10,ystart+170,25,20), "speed");
			isay_speed = GUI.TextField(new Rect(45, ystart+172, 40, 20), isay_speed, 25);	
		}
	/*	if(rstar_button) {
			GUI.Label ( new Rect (10,ystart+110,25,20), "rev point x");
			isay_x_rpoint = GUI.TextField(new Rect(45, ystart+112, 40, 20), isay_x_rpoint, 25);
			GUI.Label ( new Rect (10, ystart+140,25,20), "rev point y");
			isay_y_rpoint = GUI.TextField(new Rect(45, ystart+142, 40, 20), isay_y_rpoint, 25);
			GUI.Label(new Rect(10,ystart+170,25,20), "speed");
			isay_speed = GUI.TextField(new Rect(45, ystart+172, 40, 20), isay_speed, 25);	
		}	*/
		
	//save button
	if(GUI.Button(new Rect(10, Screen.height - 30, 70, 25), "Save"))
		SaveLevel("Levels/rev_le_test.txt");
	}
		
}
	
