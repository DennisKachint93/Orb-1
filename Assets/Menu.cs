using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {


	//State saving object
	GameObject game_state;
	
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
		Game_State gscpt = game_state.GetComponent<Game_State>();
		game_state.name="game_state";
		
		//set default ship settings here
		
		//levels that will be played in order
		string[] level_order = new string[3] {
											"Levels/level1.txt",
											"Levels/level2.txt",
											"Levels/level3.txt"};
		//state object keeps track of levels
		gscpt.level_order = level_order;
		
		//default level position is 0, but you could set it to something else here
		
		
		//load game (for debugging purposes. you should actually choose game, editor, level, etc. here
		Application.LoadLevel("Scene1");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
