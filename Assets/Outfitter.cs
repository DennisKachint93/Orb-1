using UnityEngine;
using System.Collections;

public class Outfitter : MonoBehaviour {

	GameObject game_state;
	Game_State gscpt;
	
	private int BENDING_PRICE = 6;
	private int SPEED_PRICE = 8;
	private int EASY_ENTRY_PRICE = 10;
	
	private int SHIELD_PRICE = 1;
	private int ALIEN_GUN_PRICE = 12;
	private int SPACE_BOMB_PRICE = 1;
	private int BLACK_HOLE_HELPER_PRICE = 3;
	
	private int BOOST_PRICE = 10;
	private int SPACE_JUMP_PRICE = 10;
	private int DIR_SHIFT_PRICE = 10;
	
	//lizard person
	public Texture gorn;
	
	
	// Use this for initialization
	void Start () {
		game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
	}
	
	// Update is called once per frame
	void Update () {
				
	}
	
	void OnGUI() {	
		GUI.Label(new Rect(10,10,200,25), "Space Coins: "+gscpt.num_coins);
		//tier 1 buttons
		GUI.Label (new Rect(10,25,200,25), "Tier 1");
		if(GUI.Button(new Rect(10, 45, 200, 25), "Super Bending ("+BENDING_PRICE+" coins)")){
			if(gscpt.num_coins >= BENDING_PRICE){
				//Destroy old powerup
				Destroy (gscpt.tier_1_upgrade);
				
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Super_Bending");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_1_upgrade = pwrup;
				gscpt.num_coins -= BENDING_PRICE;
				DontDestroyOnLoad(pwrup);
			}
		}
		if(GUI.Button (new Rect(10,75,200,25), "Extra Speed ("+SPEED_PRICE+" coins)")) {
			if(gscpt.num_coins >= SPEED_PRICE){
				//Destroy old powerup
				Destroy (gscpt.tier_1_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Super_Speed");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_1_upgrade = pwrup;
				gscpt.num_coins -= SPEED_PRICE;
				DontDestroyOnLoad(pwrup);
			}
		}
		if(GUI.Button (new Rect(10,105,200,25), "Easy Entry ("+EASY_ENTRY_PRICE+" coins)")) {
			if(gscpt.num_coins >= EASY_ENTRY_PRICE){
				//Destroy old powerup
				Destroy (gscpt.tier_1_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Easy_Entry");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_1_upgrade = pwrup;
				gscpt.num_coins -= EASY_ENTRY_PRICE;
				DontDestroyOnLoad(pwrup);
			}
		}
		
		//tier two buttons
		GUI.Label (new Rect(10,145,200,25), "Tier 2");
		if(GUI.Button (new Rect(10,170,200,25), "Space Bomb ("+SPACE_BOMB_PRICE+" coins)")) {
			if(gscpt.num_coins >= SPACE_BOMB_PRICE){
				//Destroy old powerup
				Destroy (gscpt.tier_1_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Space_Bomb");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_2_upgrade = pwrup;
				gscpt.num_coins -= SPACE_BOMB_PRICE;
				DontDestroyOnLoad(pwrup);
			}
		}
		
		if(GUI.Button (new Rect(10,200,200,25), "Black Hole Helper ("+BLACK_HOLE_HELPER_PRICE+" coins)")) {
			if(gscpt.num_coins >= BLACK_HOLE_HELPER_PRICE){
				//Destroy old powerup
				Destroy (gscpt.tier_2_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Black_Hole_Helper");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_2_upgrade = pwrup;
				gscpt.num_coins -= BLACK_HOLE_HELPER_PRICE;
				DontDestroyOnLoad(pwrup);
			}
		}		
		
		if(GUI.Button (new Rect(10,230,200,25), "Shield ("+SHIELD_PRICE+" coins)")) {
			if(gscpt.num_coins >= SHIELD_PRICE) {
				//Destroy old powerup
				Destroy (gscpt.tier_2_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("shield");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_2_upgrade = pwrup;
				gscpt.num_coins -= SHIELD_PRICE;
				DontDestroyOnLoad(pwrup);
			}
		}		
		
		//load next level button
		if(GUI.Button (new Rect(10,Screen.height-30,200,25), "Play next level")) {
			Application.LoadLevel("scene1");
		}
		
		//lizard person
		GUI.Box(new Rect(250, 50, 512, 512), gorn);
	}
}
