using UnityEngine;
using System.Collections;
using System.IO;

public class Level_Editor : MonoBehaviour {
	
	//starting variables for level
	public float cam_start_height;
	public float energy;
	
	//These controls are for our convenience as level editiors -- same functions as in manager
		//maximum distance from scene
	private float CAM_MAX_DIST = 1000;
		//minumum distance from scene
	private float CAM_MIN_DIST = 50;
		//how we can zoom in and out
	private float CAM_MOVE_SPEED = 4;
		//Camera orthographic size at start of level editing scene, not actual level we are creating, higher = see more
	private float CAM_START_HEIGHT = 500;
	
	//Hook into unity
	public GameObject learth;
	public GameObject star;		
	public GameObject rip;
	//public GameObject coin;
	
	//actual objects used in script
	public static GameObject l, s, e;
	public GameObject[] star_arr;
	public GameObject[] rip_arr;
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
	
	//Space rip controls position, size and boolean to begin space rip instantiation
    	public Rect button;
	public bool spaceRipButton = false;
	
	//current number of stars added
	private int arr_size = 0;
	
	void Start () {
		
		//set camera height for level editing
		Camera.main.orthographicSize = CAM_START_HEIGHT;
		//space rip gui -- button to create space rip
		button = new Rect(10, 10, 75, 25);
		
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
			
		//after a specific button has been pressed, corresponding object is instantiated on mouse click
		if(Input.GetMouseButtonDown(0)) {
        		Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			//if space rip button is pressed, instantiate space rip on mouse click 
			if (spaceRipButton) {
				//check to see that you aren't instantiating spacerips when you want to click a button	
				if (Input.mousePosition.x > 10 && Input.mousePosition.x < 85 && Input.mousePosition.y < Screen.height - 10 && Input.mousePosition.y > Screen.height - 35);
				else {  
					CreateSpaceRip (p.x, p.y, 30, 30);	
				}
			}
			//if user has entered a valid color and star button has been pushed, instantiate star anywhere but on pop-up box location
	               	if (starbut && validcolor) {
				//check to see that you aren't instantiating stars when you click to change star color/size
				if (Input.mousePosition.x > 90 && Input.mousePosition.x < 235 && Input.mousePosition.y < Screen.height - 45  && Input.mousePosition.y > Screen.height - 155);
				//instantiate star with user-defined color and radius
				else {
					Vector3 location = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                	        	starsize = float.Parse(isaysize);
                	        	GameObject starE = Instantiate (star, new Vector3(location.x, location.y,20), new Quaternion(0,0,0,0)) as GameObject;
                	        	Starscript starscript = starE.GetComponent<Starscript>();
                       	 		starscript.c = starcol;
                        		starscript.t = startex;
                        		starscript.starSize = starsize;
				}
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
    			sw.WriteLine("0,"+rip_arr.Length+",0,0,0");
				
				//stars
				//rips
				for(int i = 0; i < rip_arr.Length;i++)
				{
					sw.WriteLine(rip_arr[i].transform.position.x+","+rip_arr[i].transform.position.y+",30,30,0");
				}
				//coins
				//moving stars
				//aliens
				
    		}
				
			
		}
	}
    
	 void OnGUI() {        
		//spacerip button -- after pressing button, user can click to add space rips in locations 
        	if (GUI.Button(button, "Space Rip")) {
			spaceRipButton = !spaceRipButton;
		}
		//star button -- after pressing button, user can enter star size and color and then click to add space rips to locations
		if(GUI.Button(new Rect(10, 45, 70, 25), "star")){
			starbut = !starbut;
		}
		//if star button has been clicked, pop up box to change star color/size
 		if(starbut){
			GUI.Box (new Rect (90, 45, 145, 110), "");
			GUI.Label ( new Rect (100,50,30,20), "size");
			isaysize = GUI.TextField(new Rect(140, 52, 80, 20), isaysize, 25);
			GUI.Label ( new Rect (100, 80,30,20), "color");
			isaycolor = GUI.TextField(new Rect(140, 82, 80, 20), isaycolor, 25);
			//if done button is clicked, don't instatiate any more stars
			if(GUI.Button(new Rect(130, 120, 80, 30), "Done")){
				starbut = false;
			}
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
		
	}
		
}
	
