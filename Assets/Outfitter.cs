using UnityEngine;
using System.Collections;

public class Outfitter : MonoBehaviour {

	GameObject game_state;
	Game_State gscpt;
	
	private int BENDING_PRICE = 6;
	private int SPEED_PRICE = 8;
	private int EASY_ENTRY_PRICE = 10;
	
	private int SHIELD_PRICE = 8;
	private int ALIEN_GUN_PRICE = 12;
	private int SPACE_BOMB_PRICE = 16;
	
	private int BOOST_PRICE = 10;
	private int SPACE_JUMP_PRICE = 10;
	private int DIR_SHIFT_PRICE = 10;
	
	
	
	// Use this for initialization
	void Start () {
		game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		
		//increment level counter
		gscpt.cur_level++;
		
	}
	
	// Update is called once per frame
	void Update () {
				
	}
	
	void OnGUI() {	
		GUI.Label(new Rect(10,10,200,25), "Space Coins: "+gscpt.num_coins);
		//tier 1 buttons
		if(GUI.Button(new Rect(10, 45, 200, 25), "Bending ("+BENDING_PRICE+" coins)")){
			if(gscpt.num_coins >= BENDING_PRICE){
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Super_Bending");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_1_upgrade = pwrup;
				gscpt.num_coins -= BENDING_PRICE;
			}
		}
		if(GUI.Button (new Rect(10,75,200,25), "Super Speed ("+SPEED_PRICE+" coins)")) {
			if(gscpt.num_coins >= SPEED_PRICE){
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Super_Speed");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_1_upgrade = pwrup;
				gscpt.num_coins -= SPEED_PRICE;
			}
		}
		if(GUI.Button (new Rect(10,105,200,25), "Easier Entry ("+EASY_ENTRY_PRICE+" coins)")) {
			if(gscpt.num_coins >= EASY_ENTRY_PRICE){
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Easy_Entry");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_1_upgrade = pwrup;
				gscpt.num_coins -= EASY_ENTRY_PRICE;
			}
		}
		
		//load next level button
		if(GUI.Button (new Rect(10,Screen.height-30,200,25), "Play next level")) {
			Application.LoadLevel("scene1");
		}
	}
}
