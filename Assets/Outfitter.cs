using UnityEngine;
using System.Collections;

public class Outfitter : MonoBehaviour {

	GameObject game_state;
	Game_State gscpt;
	
/*	private int BENDING_PRICE = 10;
	private int SPEED_PRICE = 3;
	private int EASY_ENTRY_PRICE = 5;
	
	private int SHIELD_PRICE = 5; */
	private int ALIEN_GUN_PRICE = 800;
	private int SINGLE_GUN_PRICE = 25;
	private int SPACE_BOMB_PRICE = 750;
	private int SINGLE_BOMB_PRICE = 50;
	
	private int BEND_UNIT_PRICE = 1200;
	private int SINGLE_BEND_PRICE = 100;
	
//	private int BLACK_HOLE_HELPER_PRICE = 3;
	
	private int JUMP_PRICE = 500;
	private int SINGLE_JUMP_PRICE = 80;
	
	private int BOOST_PRICE = 2000;
	private int SINGLE_BOOST_PRICE = 100;
	//private int DIR_SHIFT_PRICE = 5;
	
	//lizard person
	public Texture gorn;
	
	
	//previous balance, stored so we can display it after it gets updated
	public int prev_balance;
	public int mission_net;
	
	
	// Use this for initialization
	void Start () {
		//get state object 
		game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		gscpt.in_game = false;
		
		//reset changes from previous level's powerups
		Manager.ResetConstants();
		
		//calculate total score
		prev_balance = gscpt.num_coins;
		
		mission_net = (int)((gscpt.energy_delivered/2 + gscpt.aliens_killed*4 + gscpt.coins_collected*10) - (gscpt.times_died*3 + gscpt.bombs_dropped*3 + gscpt.stars_destroyed*3));
		gscpt.num_coins += mission_net;
		
		//if you run out of coins, game over
		if(gscpt.num_coins < 0)
			Application.Quit();
	}
	
	// Update is called once per frame
	void Update () {
		//cheat
		if(Input.GetKeyDown(KeyCode.L)) {
			gscpt.num_coins+=500;
		}
	}
	
	void OnGUI() {	
		
		//bombs
		GUI.Label(new Rect(Screen.width/2 + 200,53,400,25), "....................Allows you to purchase bombs");
		if(GUI.Button(new Rect(Screen.width/2, 45, 200, 25), "Bomb License ("+SPACE_BOMB_PRICE+" coins)")){
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
			if(GUI.Button(new Rect(Screen.width/2+50, 75, 200, 25), "1 Bomb ("+SINGLE_BOMB_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOMB_PRICE) {
					gscpt.num_coins -= SINGLE_BOMB_PRICE;
					gscpt.bomb_ammo++;
				}
			}
		}
		
		//capacitor/boost
		GUI.Label(new Rect(Screen.width/2 + 200,113,400,25), "....................Gives an instant energy boost on command");
		if(GUI.Button(new Rect(Screen.width/2, 105, 200, 25), "Capacitor ("+BOOST_PRICE+" coins)")){
			if(gscpt.num_coins >= BOOST_PRICE){
			
				gscpt.capac_on = true;
				GameObject pwerup = new GameObject();
				pwerup.AddComponent("boost");
				gscpt.capac_fitting = pwerup;
				gscpt.num_coins -= BOOST_PRICE;
				DontDestroyOnLoad(pwerup);	
			}
		}
		if(gscpt.capac_on)
		{
			if(GUI.Button(new Rect(Screen.width/2+50, 135, 200, 25), "1 Charge ("+SINGLE_BOOST_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOOST_PRICE) {
					gscpt.num_coins -= SINGLE_BOOST_PRICE;
					gscpt.capac_ammo++;
				}
			}
		}
		
		//space jump
		GUI.Label(new Rect(Screen.width/2 + 200,173,400,25), "....................Lets you teleport short distances");
		if(GUI.Button(new Rect(Screen.width/2, 165, 200, 25), "Jump Unit ("+JUMP_PRICE+" coins)")){
			if(gscpt.num_coins >= JUMP_PRICE){
			
				gscpt.jump_on = true;
				GameObject pwerup = new GameObject();
				pwerup.AddComponent("space_jump");
				gscpt.jump_fitting = pwerup;
				gscpt.num_coins -= JUMP_PRICE;
				DontDestroyOnLoad(pwerup);	
			}
		}
		if(gscpt.jump_on)
		{
			if(GUI.Button(new Rect(Screen.width/2+50, 195, 200, 25), "1 Jump ("+SINGLE_JUMP_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_JUMP_PRICE) {
					gscpt.num_coins -= SINGLE_JUMP_PRICE;
					gscpt.jump_ammo++;
				}
			}
		}
		
		//alien defence gun
		GUI.Label(new Rect(Screen.width/2 + 200,233,400,25), "....................Automatically targets and destroys attacking aliens");
		if(GUI.Button(new Rect(Screen.width/2, 225, 200, 25), "Alien Defence Gun ("+ALIEN_GUN_PRICE+" coins)")){
			if(gscpt.num_coins >= ALIEN_GUN_PRICE){
			
				gscpt.gun_on = true;
				GameObject pwerup = new GameObject();
				pwerup.AddComponent("alien_gun");
				gscpt.gun_fitting = pwerup;
				gscpt.num_coins -= ALIEN_GUN_PRICE;
				DontDestroyOnLoad(pwerup);	
			}
		}
		if(gscpt.gun_on)
		{
			if(GUI.Button(new Rect(Screen.width/2+50, 255, 200, 25), "20 Torpedos ("+SINGLE_GUN_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_GUN_PRICE) {
					gscpt.num_coins -= SINGLE_GUN_PRICE;
					gscpt.gun_ammo += 20;
				}
			}
		}
		
		//Bending
		GUI.Label(new Rect(Screen.width/2 + 200,293,400,25), "....................Lets you bend more sharply");
		if(GUI.Button(new Rect(Screen.width/2, 285, 200, 25), "Bending Unit ("+BEND_UNIT_PRICE+" coins)")){
			if(gscpt.num_coins >= BEND_UNIT_PRICE){
			
				gscpt.bend_on = true;
				GameObject pwerup = new GameObject();
				pwerup.AddComponent("Super_Bending");
				gscpt.bend_fitting = pwerup;
				gscpt.num_coins -= BEND_UNIT_PRICE;
				DontDestroyOnLoad(pwerup);	
			}
		}
		if(gscpt.bend_on)
		{
			if(GUI.Button(new Rect(Screen.width/2+50, 315, 200, 25), "20 oz. Bending Fluid ("+SINGLE_BEND_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BEND_PRICE) {
					gscpt.num_coins -= SINGLE_BEND_PRICE;
					gscpt.bend_ammo += 20;
				}
			}
		}
		//ammo stats
		if(gscpt.bomb_on)
			GUI.Label(new Rect(10,Screen.height-130,150,50),"Bombs: "+gscpt.bomb_ammo);
		if(gscpt.capac_on)
			GUI.Label(new Rect(10,Screen.height-100,150,50),"Charges: "+gscpt.capac_ammo);
		if(gscpt.jump_on)
			GUI.Label(new Rect(10,Screen.height-160,150,50),"Jumps: "+gscpt.jump_ammo);
		if(gscpt.gun_on)
			GUI.Label(new Rect(10,Screen.height-190,150,50),"Torpedos: "+gscpt.gun_ammo);
		if(gscpt.bend_on)
			GUI.Label(new Rect(10,Screen.height-70,150,50),"Bending fluid: "+gscpt.bend_ammo);
		
		
			
		
		
		//coin stuff	
		GUI.Label(new Rect(Screen.width/4,45,400,25), "SPACE SHOP");
		GUI.Label(new Rect(Screen.width/4,75,400,25), "Previous mission");
		GUI.Label(new Rect(Screen.width/4,85,400,25), "-----------------------------------");
		GUI.Label(new Rect(Screen.width/4,105,400,25), "Energy Delivery Payment: "+(int)(gscpt.energy_delivered/2));
		GUI.Label(new Rect(Screen.width/4,125,400,25), "Alien Extermination Payment: "+(gscpt.aliens_killed*4));
		GUI.Label(new Rect(Screen.width/4,145,400,25), "Income From Sale Of Space Gold: "+(gscpt.coins_collected*4));
		GUI.Label(new Rect(Screen.width/4,175,400,25), "Orb Repairs: ("+(3*gscpt.times_died)+")");
		GUI.Label(new Rect(Screen.width/4,195,400,25), "Space Pollution Fees: ("+(3*gscpt.bombs_dropped+3*gscpt.stars_destroyed)+")");
		GUI.Label(new Rect(Screen.width/4,210,400,25), "-----------------------------------");
		GUI.Label(new Rect(Screen.width/4,225,400,25), "Previous Balance: "+prev_balance);
		GUI.Label(new Rect(Screen.width/4,245,400,25), "Net Earnings/Loss Of Mission: "+mission_net);
		GUI.Label(new Rect(Screen.width/4,265,400,25), "Current Balance: "+gscpt.num_coins);
		
		
		
		
		//load next level button
		if(GUI.Button (new Rect(10,Screen.height-30,200,25), "Play next level")) {
			gscpt.ResetScore();
			Application.LoadLevel("scene1");
		}
		
	}
}
