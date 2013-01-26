using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	//star visuals --prefabs
	public GameObject star,menu_learth,yellow_learth_trail;
	//actual objects
	public static GameObject l,lt;
	public GameObject starR;
	//speed learth rotates around star
	public float speed = 55;
	
	public Texture title;
	
	public GUISkin skin,skin2;

	//State saving object
	GameObject game_state;
	
	//gamestate script
	Game_State gscpt;
	
	//called before start
	void Awake() {
		if(GameObject.Find ("game_state") == null) {
		
			game_state = new GameObject();
		
			//allow game_state's state to persist through scene changes
			DontDestroyOnLoad(game_state);
		
			//allow background music object to persist
			GameObject bgm = GameObject.Find ("Background_Music");
			DontDestroyOnLoad(bgm);
			
			//add script to state object
			game_state.AddComponent("Game_State");
			gscpt = game_state.GetComponent<Game_State>();
			game_state.name="game_state";
			
			//set default ship settings here
			
			//levels that will be played in order
			string[] level_order = new string[7] {

				"Levels/1.txt",
				"Levels/level3a.txt",
				"Levels/triangle2.txt",
				"Levels/master.txt",
				"Levels/NO.txt",
				"Levels/maze.txt",
				"Levels/points.txt"
									};
			
			//state object keeps track of levels
			gscpt.level_order = level_order;
			
			//starting coins
			gscpt.num_coins = 2500;
			
		} 
	}
	
	void Start () {
		
	/*	GameObject starE = Instantiate (star, new Vector3(70,70,0), new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = Color.red;
		starscript.starSize = 15; 
		starscript.is_revolving = true;
		starscript.rpoint = new Vector3(0,0,0);
		starscript.rspeed = 65;*/
		//instantiate menu learth
		l = Instantiate (menu_learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
	 	l.renderer.material.color = Color.yellow;	
		lt = Instantiate (yellow_learth_trail, l.transform.position, l.transform.rotation) as GameObject;
		lt.transform.parent = l.transform;
		starR = Instantiate (star, new Vector3(-40,25,0), new Quaternion(0,0,0,0)) as GameObject;
		Starscript script = starR.GetComponent<Starscript>();
		script.c = Color.cyan;
		script.starSize = 75; 
		l.transform.position = new Vector3(starR.transform.position.x+script.starSize+Manager.RADIAL_ERROR,starR.transform.position.y,0);
	}
	
	void Update () {
		l.transform.RotateAround(starR.transform.position, Vector3.forward, speed*Time.deltaTime);
		
	}
		
	void OnGUI () {
		GUI.skin = skin;
		GUI.backgroundColor = Color.black;
		
		GUI.skin.label.fontSize = 1000;
//		GUI.Label(new Rect(Screen.width/2-350,Screen.height/2-600,500,1000),"O");
		GUI.skin.label.fontSize = 400;
		GUI.skin.label.normal.textColor = new Color(0,1,1,.5f);
		GUI.Label(new Rect(Screen.width/2+100,Screen.height/2-260,1000,1000), "RB"); 
		GUI.skin = skin2;
	//	GUI.Box(new Rect(50, 50, Screen.width/2-100, 7*Screen.height/8), dennis);
//		GUI.Box(new Rect(50,50, 1024,640 ), title);
		
		GUI.skin.button.fontSize = 40;
		GUI.skin.button.hover.textColor = Color.cyan;
		if(GUI.Button(new Rect(Screen.width/6, Screen.height-200, 180, 45), "Play!")) {
			gscpt.in_game = true;
			//Application.LoadLevel("End_Scene");
			Manager.ResetConstants();
			Application.LoadLevel("Scene1");
		}
		if(GUI.Button(new Rect(5*Screen.width/6-300, Screen.height-200, 300, 45), "Level Editor"))	
			Application.LoadLevel("Level_Editor");
	}
	
}
