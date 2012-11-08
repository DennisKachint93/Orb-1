using UnityEngine;
using System.Collections;

public class Level_Editor : MonoBehaviour {
	
	//starting variables for level
	public float cam_start_height;
	public float energy;
	
	//These controls are for our convenience as level editiors -- same functions as in manager
		//maximum distance from scene
	private float CAM_MAX_DIST = 500;
		//minumum distance from scene
	private float CAM_MIN_DIST = 50;
		//how we can zoom in and out
	private float CAM_MOVE_SPEED = 4;
		//Camera orthographic size at start of level editing scene, not actual level we are creating, higher = see more
	private float CAM_START_HEIGHT = 300;
	
	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	//public GameObject coin;
	
	//actual objects used in script
	public static GameObject l, s, e;
	public GameObject[] star_arr;
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
	
	//Space rip controls position, size and boolean to begin space rip instantiation
	public int SR_boxWidth;
	public int SR_boxHeight;
	public int SR_box_x;
	public int SR_box_y;
    public Rect button;
	public bool createSR, spaceRipButton = false;
	//User input strings of x and y scale of space rip (and corresponding ints)
	public string spaceRipStringX = "10";
	public string spaceRipStringY = "10";
	public int spaceRipX;
	public int spaceRipY;
	
	//current number of stars added
	private int arr_size = 0;
	
	void Start () {
		
		//set camera height for level editing
		Camera.main.orthographicSize = CAM_START_HEIGHT;
		//space rip gui -- input box 
		SR_boxWidth = 115;
		SR_boxHeight = 105;
		SR_box_x = Screen.width/2 - SR_boxWidth/2;
		SR_box_y = Screen.height/2 - SR_boxHeight/2;
		//space rip gui -- button to create space rip
		button = new Rect(10, 10, 75, 25);
		
	}
	
	//instantiates a space rip from prefab at given location and of given dimensions, with given rotation (default = 0), returns reference to that object
	GameObject CreateSpaceRip(float x, float y, float width, float height, float rotation = 0)
	{
		GameObject rip_actual = Instantiate (rip, new Vector3 (x, y, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		rip_actual.transform.localScale += new Vector3(width,height,0);
		rip_actual.transform.Rotate(new Vector3(0,0,rotation));
		return rip_actual;
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
		GameObject[] temp_arr = new GameObject[arr_size+1];
		for(int i=0;i<arr_size;i++)
			temp_arr[i] = star_arr[i];
		star_arr = temp_arr;
		star_arr[arr_size] = starE;
		arr_size++;
		numStars++;
		return starE;
	}
	/*
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
	} */
	
	void Update () {
		//A moves the camera farther, S moves the camera closer
		if(Input.GetKey(KeyCode.A) && Camera.main.orthographicSize <= CAM_MAX_DIST)
			Camera.main.orthographicSize += CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.S) && Camera.main.orthographicSize >= CAM_MIN_DIST)
			Camera.main.orthographicSize -= CAM_MOVE_SPEED;
		//after a specific button has been pressed, corresponding object is instantiated
		if(Input.GetMouseButtonDown(0)) {
        	Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			if (createSR) {
				//check to see that you aren't instantiating spacerips when you want to click a button	
		if (Input.mousePosition.x > 10 && Input.mousePosition.x < 85 && Input.mousePosition.y < Screen.height - 10 && Input.mousePosition.y > Screen.height - 35);
		else 
			CreateSpaceRip (p.x, p.y, spaceRipX, spaceRipY);	
			}
		}
	}   
    
	 void OnGUI() {        
		
		//spacerip button and scale controls -- after pressing button, user can enter 
		//x and y scale of spacerip and then click to instantiate a new spacerip
        if (GUI.Button(button, "Space Rip")) {
			createSR = false;
			if (!spaceRipButton) 
				spaceRipButton = true;
			else 
				spaceRipButton = false;
		}
		if (spaceRipButton && !createSR) {
			GUI.Box(new Rect(SR_box_x, SR_box_y, SR_boxWidth, SR_boxHeight), "Space Rip");
			GUI.Label(new Rect(SR_box_x + 10, SR_box_y + 25, 60, 25), "SCALE x");
			spaceRipStringX = GUI.TextField(new Rect(SR_box_x + 75, SR_box_y + 25, 25, 22), spaceRipStringX, 25);
			bool parsed = int.TryParse(spaceRipStringX, out spaceRipX);
			GUI.Label(new Rect(SR_box_x + 10, SR_box_y + 48, 60, 25), "SCALE y");
			spaceRipStringY = GUI.TextField(new Rect(SR_box_x + 75, SR_box_y + 48, 25, 22), spaceRipStringY, 25);
			parsed = int.TryParse(spaceRipStringY, out spaceRipY);
			if (GUI.Button(new Rect(SR_box_x + 20, SR_box_y + 75, 70, 25), "Create")) {
				createSR = true;
			}
		}
		
	}
		
}
	