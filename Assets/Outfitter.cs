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
	
	private int BLACK_HOLE_HELPER_PRICE = 3;
	
	private int JUMP_PRICE = 500;
	private int SINGLE_JUMP_PRICE = 80;
	
	private int BOOST_PRICE = 2000;
	private int SINGLE_BOOST_PRICE = 100;
	private int DIR_SHIFT_PRICE = 5;
	
	//lizard person
	public Texture gorn;
	//GUI controls
	public GUISkin skin;
	int toolbar_width = 250;
	float xoffset, yoffset;
	
	Rect bomb_button;
	Rect capacitor_button;
	Rect alien_defense_button;
	Rect jump_unit_button;
	Rect black_hole_button;
	Rect dir_shift_button;
	
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
		
		mission_net = (int)((gscpt.energy_delivered/2 + 250*(gscpt.num_stars/(gscpt.time_to_complete+1)) + gscpt.aliens_killed*4 + gscpt.coins_collected*10) - (gscpt.times_died*3 + gscpt.bombs_dropped*3 + gscpt.stars_destroyed*3));
		gscpt.num_coins += mission_net;
		
		//if you run out of coins, game over
		if(gscpt.num_coins < 0)
			Application.Quit();
	}
	
	// Update is called once per frame
	void Update () {
	
		xoffset = Screen.width - toolbar_width + 20;
		yoffset = 5;	
		
		/*
		t1: bomb jump
		t2: alien gun, black hole
		t3: dir shift, time warp
		*/
		
		bomb_button = new Rect(xoffset, yoffset+280, 200, 20);
		//capacitor_button = new Rect(xoffset, yoffset+300, 200, 20);
		alien_defense_button = new Rect(xoffset, yoffset+370, 200, 20);
		jump_unit_button = new Rect(xoffset, yoffset+305, 200, 20);
		black_hole_button = new Rect(xoffset,yoffset+345, 200, 20);
		dir_shift_button = new Rect(xoffset,yoffset+415,200,20);
		
		//cheat
		if(Input.GetKeyDown(KeyCode.L)) {
			gscpt.num_coins+=500;
		}
		if(Input.GetKeyDown(KeyCode.K)) {
			gscpt.bend_ammo = 1000;
			gscpt.bomb_ammo = 1000;
			gscpt.gun_ammo  = 1000;
			gscpt.jump_ammo = 1000;
			gscpt.capac_ammo= 1000;
		}
	}
	
	void OnGUI() {	
		
		GUI.skin = skin;
   		//style.font = gothicTech;
		
		GUI.Box(new Rect(Screen.width-toolbar_width, 0, toolbar_width, Screen.height), "");
		
		GUI.Label(new Rect(xoffset,yoffset+5,400,25), "Previous mission");
		GUI.Label(new Rect(xoffset,yoffset+20,400,25), "-----------------------------------");
		GUI.Label(new Rect(xoffset,yoffset+35,400,25), "Contract Payment: "+(int)(gscpt.energy_delivered/2 + 250*(gscpt.num_stars/(gscpt.time_to_complete+1))));
		GUI.Label(new Rect(xoffset,yoffset+55,400,25), "Alien Extermination Payment: "+(gscpt.aliens_killed*4));
		GUI.Label(new Rect(xoffset,yoffset+75,400,25), "Income From Sale Of Space Gold: "+(gscpt.coins_collected*4));
		GUI.Label(new Rect(xoffset,yoffset+95,400,25), "Orb Repairs: ("+(3*gscpt.times_died)+")");
		GUI.Label(new Rect(xoffset,yoffset+115,400,25), "Space Pollution Fees: ("+(3*gscpt.bombs_dropped+3*gscpt.stars_destroyed)+")");
		GUI.Label(new Rect(xoffset,yoffset+130,400,25), "-----------------------------------");
		GUI.Label(new Rect(xoffset,yoffset+145,400,25), "Previous Balance: "+prev_balance);
		GUI.Label(new Rect(xoffset,yoffset+165,400,25), "Net Earnings/Loss Of Mission: "+mission_net);
		GUI.Label(new Rect(xoffset,yoffset+185,400,25), "Current Balance: "+gscpt.num_coins);
		
		GUI.Label(new Rect(xoffset,yoffset+230,400,25), "SPACE SHOP");
		GUI.Label(new Rect(xoffset,yoffset+260,400,25), "TIER 1");
		GUI.Label(new Rect(xoffset,yoffset+325,400,25), "TIER 2");
		GUI.Label(new Rect(xoffset,yoffset+390,400,25), "TIER 3");
		
		if (bomb_button.Contains(Input.mousePosition) && !gscpt.bomb_on) 
			GUI.Label(new Rect(xoffset, yoffset+400,400,40), "Allows you to purchase bombs: "+SPACE_BOMB_PRICE+" coins");
		if (capacitor_button.Contains(Input.mousePosition) && gscpt.capac_on) 
			GUI.Label(new Rect(xoffset, yoffset+400,400,40), "Gives an instant energy boost on command: "+BOOST_PRICE+" coins");
		if (jump_unit_button.Contains(Input.mousePosition) && !gscpt.jump_on) 
			GUI.Label(new Rect(xoffset, yoffset+400,400,40), "Lets you teleport short distances: "+JUMP_PRICE+" coins");
		if (alien_defense_button.Contains(Input.mousePosition) && !gscpt.gun_on) 
			GUI.Label(new Rect(xoffset, yoffset+400,400,40), "Automatically targets and destroys attacking aliens: "+ALIEN_GUN_PRICE+" coins");
		
		
	
		//bomb
		if (!gscpt.bomb_on && !gscpt.jump_on) {
			if(GUI.Button(bomb_button, "Bomb License")){
				if(gscpt.num_coins >= SPACE_BOMB_PRICE){
					gscpt.bomb_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("Space_Bomb");
					gscpt.bomb_fitting = pwerup;
					gscpt.num_coins -= SPACE_BOMB_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		if(gscpt.bomb_on)
		{
			if(GUI.Button(bomb_button, "5 Bombs ("+SINGLE_BOMB_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOMB_PRICE) {
					gscpt.num_coins -= SINGLE_BOMB_PRICE;
					gscpt.bomb_ammo += 5;
				}
			}
		}
		
		//blackhole helper
		if (!gscpt.blackhole_on && !gscpt.gun_on) {
			if(GUI.Button(black_hole_button, "Black Hole Detection")){
				if(gscpt.num_coins >= SPACE_BOMB_PRICE){
					gscpt.blackhole_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("Black_Hole_Helper");
					gscpt.bomb_fitting = pwerup;
					gscpt.num_coins -= BLACK_HOLE_HELPER_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		
		//direction shift
		if (!gscpt.direction_on) {
			if(GUI.Button(dir_shift_button, "Rapid Direction Shift")){
				if(gscpt.num_coins >= SPACE_BOMB_PRICE){
					gscpt.direction_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("direction_shift");
				//	gscpt.bomb_fitting = pwerup;
					gscpt.num_coins -= DIR_SHIFT_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}

	/*	
		//capacitor/boost
		if(!gscpt.capac_on) {
			if(GUI.Button(capacitor_button, "Capacitor")){
				if(gscpt.num_coins >= BOOST_PRICE){
					gscpt.capac_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("boost");
					gscpt.capac_fitting = pwerup;
					gscpt.num_coins -= BOOST_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		if(gscpt.capac_on)
		{
			if(GUI.Button(capacitor_button, "1 Charge ("+SINGLE_BOOST_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOOST_PRICE) {
					gscpt.num_coins -= SINGLE_BOOST_PRICE;
					gscpt.capac_ammo++;
				}
			}
		}*/
		
		//space jump
		if (!gscpt.jump_on && !gscpt.bomb_on) {
			if(GUI.Button(jump_unit_button, "Jump Unit ")){
				if(gscpt.num_coins >= JUMP_PRICE){		
					gscpt.jump_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("space_jump");
					gscpt.jump_fitting = pwerup;
					gscpt.num_coins -= JUMP_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		if(gscpt.jump_on)
		{
			if(GUI.Button(jump_unit_button, "1 Jump ("+SINGLE_JUMP_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_JUMP_PRICE) {
					gscpt.num_coins -= SINGLE_JUMP_PRICE;
					gscpt.jump_ammo++;
				}
			}
		}
		
		//alien defence gun
		if(!gscpt.gun_on && !gscpt.blackhole_on) {
			if(GUI.Button(alien_defense_button, "Alien Defense Gun ")){
				if(gscpt.num_coins >= ALIEN_GUN_PRICE){		
					gscpt.gun_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("alien_gun");
					gscpt.gun_fitting = pwerup;
					gscpt.num_coins -= ALIEN_GUN_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		if(gscpt.gun_on)
		{
			if(GUI.Button(alien_defense_button, "20 Torpedos ("+SINGLE_GUN_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_GUN_PRICE) {
					gscpt.num_coins -= SINGLE_GUN_PRICE;
					gscpt.gun_ammo += 20;
				}
			}
		}
		
	/*	//Bending
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
		}*/
		//ammo stats
		if(gscpt.bomb_on)
			GUI.Label(new Rect(xoffset,Screen.height-35,150,50),"Bombs: "+gscpt.bomb_ammo);
		if(gscpt.capac_on)
			GUI.Label(new Rect(xoffset,Screen.height-10,150,50),"Charges: "+gscpt.capac_ammo);
		if(gscpt.jump_on)
			GUI.Label(new Rect(xoffset,Screen.height-60,150,50),"Jumps: "+gscpt.jump_ammo);
		if(gscpt.gun_on)
			GUI.Label(new Rect(xoffset,Screen.height-105,150,50),"Torpedos: "+gscpt.gun_ammo);
		if(gscpt.bend_on)
			GUI.Label(new Rect(xoffset,Screen.height-85,150,50),"Bending fluid: "+gscpt.bend_ammo);
		
		
		
		
		
		
		//load next level button
		if(GUI.Button (new Rect(10,Screen.height-30,200,25), "Play next level")) {
			gscpt.ResetScore();
			Application.LoadLevel("scene1");
		}
		
	}
}
