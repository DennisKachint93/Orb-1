using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	//dennis
	public Texture dennis;

	//State saving object
	GameObject game_state;
	
	//gamestate script
	Game_State gscpt;
	
	//called before start
	void Awake() {
		game_state = new GameObject();
		
		//allow game_state's state to persist through scene changes
		DontDestroyOnLoad(game_state);
	}
	
	// Use this for initialization
	void Start () {
		//add script to state object
		game_state.AddComponent("Game_State");
		gscpt = game_state.GetComponent<Game_State>();
		game_state.name="game_state";
		
		//set default ship settings here
		
		//levels that will be played in order
		string[] level_order = new string[3] {
											"Levels/bhdbg.txt",
											//"Assets/blackHoleTest.txt",
											//"Levels/blackholes.txt",
											"Levels/demo-2.txt",
											"Levels/3.txt"};
		//state object keeps track of levels
		gscpt.level_order = level_order;
		
		//starting coins
		gscpt.num_coins = 10;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnGUI () {
		GUI.backgroundColor = Color.red;
		GUI.Box(new Rect(50, 50, Screen.width/2-100, 7*Screen.height/8), dennis);
		if(GUI.Button(new Rect(3*Screen.width/4 + 8, Screen.height/2, 60, 25), "Play!")) {
			gscpt.in_game = true;
			Application.LoadLevel("Scene1");
		}	
		if(GUI.Button(new Rect(3*Screen.width/4, Screen.height/2 + 35, 80, 25), "Level Editor"))	
			Application.LoadLevel("Level_Editor");
	}
	
}
