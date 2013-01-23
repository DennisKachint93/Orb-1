using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	//dennis
	public Texture dennis;
	public Texture title;
	
	public GUISkin skin;

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
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnGUI () {
		GUI.skin = skin;
		GUI.backgroundColor = Color.black;
		GUI.skin.button.fontSize = 40;
	//	GUI.Box(new Rect(50, 50, Screen.width/2-100, 7*Screen.height/8), dennis);
		GUI.Box(new Rect(50,50, 1024,640 ), title);
		if(GUI.Button(new Rect(3*Screen.width/4, Screen.height/2-80, 180, 45), "Play!!!!!!!")) {
			gscpt.in_game = true;
//			Application.LoadLevel("Ship_Outfitter");
			Manager.ResetConstants();
			Application.LoadLevel("Scene1");
		}	
		if(GUI.Button(new Rect(3*Screen.width/4, Screen.height/2 - 25, 180, 45), "Level Editor"))	
			Application.LoadLevel("Level_Editor");
	}
	
}
