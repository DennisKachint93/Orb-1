using UnityEngine;
using System.Collections;

public class Outfitter : MonoBehaviour {

	GameObject game_state;
	Game_State gscpt;
	
	private int BENDING_PRICE = 10;
	private int SPEED_PRICE = 3;
	private int EASY_ENTRY_PRICE = 5;
	
	private int SHIELD_PRICE = 5;
//	private int ALIEN_GUN_PRICE = 12;
	private int SPACE_BOMB_PRICE = 10;
	private int SINGLE_BOMB_PRICE = 5;
	private int BLACK_HOLE_HELPER_PRICE = 3;
	
	private int BOOST_PRICE = 3;
	private int SINGLE_BOOST_PRICE = 5;
//	private int SPACE_JUMP_PRICE = 10;
	private int DIR_SHIFT_PRICE = 5;
	
	//lizard person
	public Texture gorn;
	
	//true if you've already picked your powerup from that tier
	private bool t1_bought = false;
	private bool t2_bought = false;
	private bool t3_bought = false;
	

	
	// Use this for initialization
	void Start () {
		//get state object 
		game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		gscpt.in_game = false;
		
		//reset changes from previous level's powerups
		Manager.ResetConstants();
	}
	
	// Update is called once per frame
	void Update () {
				
	}
	
	void OnGUI() {	
		
		//bombs
		if(GUI.Button(new Rect(500, 45, 200, 25), "Bomb License ("+SPACE_BOMB_PRICE+" coins)")){
			if(gscpt.num_coins >= SPACE_BOMB_PRICE){
				gscpt.bomb_on = true;
				GameObject pwerup = new GameObject();
				pwerup.AddComponent("Space_Bomb");
				gscpt.bomb_fitting = pwerup;
				gscpt.num_coins -= SPACE_BOMB_PRICE;
				DontDestroyOnLoad(pwerup);	
			}
		}
		if(gscpt.bomb_on)
		{
			if(GUI.Button(new Rect(550, 75, 200, 25), "1 Bomb ("+SINGLE_BOMB_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOMB_PRICE) {
					gscpt.num_coins -= SINGLE_BOMB_PRICE;
					gscpt.bomb_ammo++;
				}
			}
		}
		
		//capacitor/boost
		if(GUI.Button(new Rect(500, 105, 200, 25), "Capacitor ("+BOOST_PRICE+" coins)")){
			if(gscpt.num_coins >= BOOST_PRICE){
			
				gscpt.capac_on = true;
				GameObject pwerup = new GameObject();
				pwerup.AddComponent("boost");
				gscpt.bomb_fitting = pwerup;
				gscpt.num_coins -= BOOST_PRICE;
				DontDestroyOnLoad(pwerup);	
			}
		}
		if(gscpt.capac_on)
		{
			if(GUI.Button(new Rect(550, 135, 200, 25), "1 Charge ("+SINGLE_BOOST_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOOST_PRICE) {
					gscpt.num_coins -= SINGLE_BOOST_PRICE;
					gscpt.boost_ammo++;
				}
			}
		}
		
		
		
		GUI.Label(new Rect(10,10,200,25), "Space Coins: "+gscpt.num_coins);
		//tier 1 buttons
		GUI.Label (new Rect(10,25,200,25), "Tier 1");
		if(GUI.Button(new Rect(10, 45, 200, 25), "Super Bending ("+BENDING_PRICE+" coins)")){
			if(gscpt.num_coins >= BENDING_PRICE && !t1_bought){
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
				t1_bought = true;
			}
		}
		if(GUI.Button (new Rect(10,105,200,25), "Extra Speed ("+SPEED_PRICE+" coins)")) {
			if(gscpt.num_coins >= SPEED_PRICE && !t1_bought){
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
				t1_bought = true;
			}
		}
		if(GUI.Button (new Rect(10,75,200,25), "Easy Entry ("+EASY_ENTRY_PRICE+" coins)")) {
			if(gscpt.num_coins >= EASY_ENTRY_PRICE && !t1_bought){
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
				t1_bought = true;
			}
		}
		
		//tier two buttons
		GUI.Label (new Rect(10,145,200,25), "Tier 2");
		if(GUI.Button (new Rect(10,170,200,25), "Space Bombs ("+SPACE_BOMB_PRICE+" coins)")) {
			if(gscpt.num_coins >= SPACE_BOMB_PRICE && !t2_bought){
				//Destroy old powerup
				Destroy (gscpt.tier_2_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("Space_Bomb");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_2_upgrade = pwrup;
				gscpt.num_coins -= SPACE_BOMB_PRICE;
				DontDestroyOnLoad(pwrup);
				t2_bought = true;
			}
		}
		
		if(GUI.Button (new Rect(10,230,200,25), "Black Hole Detection ("+BLACK_HOLE_HELPER_PRICE+" coins)")) {
			if(gscpt.num_coins >= BLACK_HOLE_HELPER_PRICE && !t2_bought){
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
				t2_bought = true;
			}
		}		
		
		if(GUI.Button (new Rect(10,200,200,25), "Shield ("+SHIELD_PRICE+" coins)")) {
			if(gscpt.num_coins >= SHIELD_PRICE && !t2_bought) {
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
				t2_bought = true;
			}
		}		
		GUI.Label (new Rect(10,270,200,25), "Tier 3");
		if(GUI.Button (new Rect(10,325,200,25), "Boost ("+BOOST_PRICE+" coins)")) {
			if(gscpt.num_coins >= BOOST_PRICE && !t3_bought) {
				//Destroy old powerup
				Destroy (gscpt.tier_3_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("boost");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_3_upgrade = pwrup;
				gscpt.num_coins -= BOOST_PRICE;
				DontDestroyOnLoad(pwrup);
				t3_bought = true;
			}
		}		
		if(GUI.Button (new Rect(10,295,200,25), "Fast Corner ("+DIR_SHIFT_PRICE+" coins)")) {
			if(gscpt.num_coins >= DIR_SHIFT_PRICE && !t3_bought) {
				//Destroy old powerup
				Destroy (gscpt.tier_3_upgrade);
				//create a new gameobject to replace the old one
				GameObject pwrup = new GameObject();
				//add the script that defines your power up 
				pwrup.AddComponent("direction_shift");
				//assign the gameobject to the proper tier in game_state
				gscpt.tier_3_upgrade = pwrup;
				gscpt.num_coins -= DIR_SHIFT_PRICE;
				DontDestroyOnLoad(pwrup);
				
				//indicate the tier has been bought
				t3_bought = true;
			}
		}		
		
		
		//load next level button
		if(GUI.Button (new Rect(10,Screen.height-30,200,25), "Play next level")) {
			Application.LoadLevel("scene1");
			
		}
		
		//lizard person
		//GUI.Box(new Rect(250, 50, 512, 512), gorn);
	}
}
