using UnityEngine;
using System.Collections;

public class Outfitter : MonoBehaviour {

	GameObject game_state;
	Game_State gscpt;
	
/*	private int BENDING_PRICE = 10;
	private int SPEED_PRICE = 3;
	private int EASY_ENTRY_PRICE = 5;
	
	private int SHIELD_PRICE = 5; */	
	private int SPACE_BOMB_PRICE = 750;
	private int SINGLE_BOMB_PRICE = 50;
	
	private int JUMP_PRICE = 500;
	private int SINGLE_JUMP_PRICE = 80;
	
//	private int BEND_UNIT_PRICE = 1200;
//	private int SINGLE_BEND_PRICE = 100;
	
	private int ALIEN_GUN_PRICE = 800;
	private int SINGLE_GUN_PRICE = 25;
	
	private int BLACK_HOLE_HELPER_PRICE = 3;

//	private int BOOST_PRICE = 2000;
//	private int SINGLE_BOOST_PRICE = 100;
	private int DIR_SHIFT_PRICE = 50;
	private int SINGLE_DIR_SHIFT_PRICE = 5;
	
	private int TIME_WARP_PRICE = 20;
	private int SINGLE_TIME_WARP_PRICE = 5;
	
	//lizard person
	public Texture gorn;
	//GUI controls
	public GUISkin skin;
	int toolbar_width = 250;
	float xoffset, yoffset;
	
	Rect bomb_button;
	Rect jump_unit_button;
	Rect alien_defense_button;
	Rect black_hole_button;
	Rect dir_shift_button;
	Rect timewarp_button;
	
	//button textures_
	
	public Texture bomb_text;
	public Texture jump_text;
	public Texture blackhole_text;
	public Texture gun_text;
	public Texture dir_shift_text;
	public Texture time_warp_text;
	
	
	public Color lightBlue;
	public Color darkBlue;
	
	//previous balance, stored so we can display it after it gets updated
	public int prev_balance;
	public int mission_net;
	
	
	// Use this for initialization
	void Start () {
		
		lightBlue = new Color(.416f, .84f, 1f);
		darkBlue = new Color(0f, .2f, .6f);
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
		t1: bomb, jump
		t2: alien gun, black hole
		t3: dir shift, time warp
		*/
		
		print (Input.mousePosition.x  + " y " + Input.mousePosition.y);
		
		bomb_button = new Rect(xoffset, yoffset+285, 200, 20);
		jump_unit_button = new Rect(xoffset, yoffset+310, 200, 20);
		
		black_hole_button = new Rect(xoffset,yoffset+355, 200, 20);
		alien_defense_button = new Rect(xoffset, yoffset+380, 200, 20);
		
		dir_shift_button = new Rect(xoffset,yoffset+425,200,20);
		timewarp_button = new Rect(xoffset,yoffset+450,200,20);
		
		//cheat
		if(Input.GetKeyDown(KeyCode.L)) {
			gscpt.num_coins+=500;
		}
		if(Input.GetKeyDown(KeyCode.K)) {
			//gscpt.bend_ammo = 1000;
			gscpt.bomb_ammo = 1000;
			gscpt.gun_ammo  = 1000;
			gscpt.jump_ammo = 1000;
		}
	}
	
	void OnGUI() {	
		
		GUI.skin = skin;
	//	GUI.backgroundColor = Color.black;
		
		GUI.Box(new Rect(Screen.width-toolbar_width, 0, toolbar_width, Screen.height), "");
		GUI.skin.label.normal.textColor = darkBlue;
		GUI.skin.label.fontSize = 26;
		GUI.Label(new Rect(xoffset,yoffset+5,400,25), "PREVIOUS MISSION");
		
		GUI.skin.label.fontSize = 17;
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+35,107,25), "Contract Payment :");
		GUI.skin.label.normal.textColor = Color.red;
		GUI.Label(new Rect(xoffset+107,yoffset+35,50,25),"$"+(int)(gscpt.energy_delivered/2 + 250*(gscpt.num_stars/(gscpt.time_to_complete+1))));
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+55,160,25), "Alien Extermination Payment :"); 
		GUI.skin.label.normal.textColor = Color.red;
		GUI.Label(new Rect(xoffset+160,yoffset+55,50,25), "$"+(gscpt.aliens_killed*4));
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+75,179,25), "Income From Sale Of Space Gold :"); 
		GUI.skin.label.normal.textColor = Color.green;
		GUI.Label(new Rect(xoffset+179,yoffset+75,50,25), "$"+(gscpt.coins_collected*4));
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+95,75,25), "Orb Repairs :"); 
		GUI.skin.label.normal.textColor = Color.red;
		GUI.Label(new Rect(xoffset+75,yoffset+95,50,25), "$"+3*gscpt.times_died);
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+115,120,25), "Space Pollution Fees : ");
		GUI.skin.label.normal.textColor = Color.red;
		GUI.Label(new Rect(xoffset+120,yoffset+115,50,25), "$"+(3*gscpt.bombs_dropped+3*gscpt.stars_destroyed));
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+150,100,25), "Previous Balance :");
		if (prev_balance < 0)
			GUI.skin.label.normal.textColor = Color.red;
		else 
			GUI.skin.label.normal.textColor = Color.green;
		GUI.Label(new Rect(xoffset+100,yoffset+150,50,25), "$"+prev_balance);
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+170,175,25), "Net Earnings/Loss Of Mission :");
		if (mission_net < 0)
			GUI.skin.label.normal.textColor = Color.red;
		else 
			GUI.skin.label.normal.textColor = Color.green;
		GUI.Label(new Rect(xoffset+175,yoffset+170,50,25), "$"+mission_net);
		
		GUI.skin.label.normal.textColor = lightBlue;
		GUI.Label(new Rect(xoffset,yoffset+190,100,25), "Current Balance : ");
		GUI.skin.label.fontSize = 26;
		if (gscpt.num_coins < 0)
			GUI.skin.label.normal.textColor = Color.red;
		else 
			GUI.skin.label.normal.textColor = Color.green;
		GUI.Label(new Rect(xoffset+100,yoffset+190,50,25), "$"+gscpt.num_coins);
		
		GUI.skin.label.normal.textColor = darkBlue;
		GUI.Label(new Rect(xoffset,yoffset+235,100,25), "SPACE SHOP");
		GUI.skin.label.fontSize = 21;
		GUI.Label(new Rect(xoffset,yoffset+265,40,25), "TIER 1");
		GUI.Label(new Rect(xoffset,yoffset+335,40,25), "TIER 2");
		GUI.Label(new Rect(xoffset,yoffset+405,40,25), "TIER 3");
		
		if (Input.mousePosition.x > xoffset && Input.mousePosition.x < xoffset+200) {
			if (!gscpt.bomb_on && Input.mousePosition.y < Screen.height -(yoffset+285) && Input.mousePosition.y > Screen.height-(yoffset+305) && !gscpt.bomb_on) {
				GUI.Label(new Rect(xoffset, yoffset+485,200,20), "Allows you to ");
				GUI.Label(new Rect(xoffset, yoffset+505,200,20), "purchase bombs.");
				GUI.skin.label.fontSize = 26;
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(xoffset, yoffset+525,20,20), "$"+SPACE_BOMB_PRICE);
				GUI.Label(new Rect(xoffset+120,yoffset+485,80,220),bomb_text);			
			}if (!gscpt.jump_on && Input.mousePosition.y < Screen.height-(yoffset+310) && Input.mousePosition.y > Screen.height-(yoffset+330) && !gscpt.jump_on) {
				GUI.Label(new Rect(xoffset, yoffset+485,200,20), "Lets you teleport ");
				GUI.Label(new Rect(xoffset, yoffset+505,200,20), "short distances.");
				GUI.skin.label.fontSize = 26;
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(xoffset+110,yoffset+490,110,60),jump_text);
				GUI.Label(new Rect(xoffset, yoffset+525,40,20), "$"+JUMP_PRICE);
			}if (!gscpt.blackhole_on && Input.mousePosition.y < Screen.height-(yoffset+355) && Input.mousePosition.y > Screen.height-(yoffset+375) && !gscpt.blackhole_on) {
				GUI.Label(new Rect(xoffset, yoffset+485,200,20), "Highlights black holes");
				GUI.Label(new Rect(xoffset, yoffset+505,200,20), "to make them easier");
				GUI.Label(new Rect(xoffset, yoffset+525,40,20), "to see.");
				GUI.skin.label.fontSize = 26;
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(xoffset+128,yoffset+480,90,75),blackhole_text);
				GUI.Label(new Rect(xoffset+45, yoffset+525,40,20), "$"+BLACK_HOLE_HELPER_PRICE);
			}if (!gscpt.gun_on && Input.mousePosition.y < Screen.height-(yoffset+380) && Input.mousePosition.y > Screen.height-(yoffset+400) && !gscpt.gun_on) {
				GUI.Label(new Rect(xoffset, yoffset+485,200,20), "Automatically targets and ");
				GUI.Label(new Rect(xoffset, yoffset+505,200,20), "destroys attacking aliens.");
				GUI.skin.label.fontSize = 26;
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(xoffset, yoffset+525,40,20), "$"+ALIEN_GUN_PRICE);
			//	GUI.Label(new Rect(xoffset+110,yoffset+485,80,80),gun_text);
			}if (!gscpt.direction_on && Input.mousePosition.y < Screen.height-(yoffset+425) && Input.mousePosition.y > Screen.height-(yoffset+445) && !gscpt.direction_on) {
				GUI.Label(new Rect(xoffset, yoffset+485,200,20), "Allows you to rapidly");
				GUI.Label(new Rect(xoffset, yoffset+505,200,20), "change direction by");
				GUI.Label(new Rect(xoffset, yoffset+525,200,20), " degrees.");
				GUI.skin.label.fontSize = 26;
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(xoffset, yoffset+545,40,20), "$"+DIR_SHIFT_PRICE);
				GUI.Label(new Rect(xoffset+110,yoffset+485,90,85),dir_shift_text);	
			}if (!gscpt.timewarp_on && Input.mousePosition.y < Screen.height-(yoffset+450) && Input.mousePosition.y > Screen.height-(yoffset+470) && !gscpt.timewarp_on) {
				GUI.Label(new Rect(xoffset, yoffset+485,200,20), "Allows you to travel back");
				GUI.Label(new Rect(xoffset, yoffset+505,200,20), "in time to the last star");
				GUI.Label(new Rect(xoffset, yoffset+525,200,20), "you orbited.");
				GUI.skin.label.fontSize = 26;
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(xoffset, yoffset+545,40,20), "$"+TIME_WARP_PRICE);
				//GUI.Label(new Rect(xoffset+110,yoffset+485,80,80),time_warp_text);	
			}
		}
		
		/* TIER 1 : bomb and space jump */
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
		if(gscpt.bomb_on) {
			if(GUI.Button(bomb_button, "5 Bombs ("+SINGLE_BOMB_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_BOMB_PRICE) {
					gscpt.num_coins -= SINGLE_BOMB_PRICE;
					gscpt.bomb_ammo += 5;
				}
			}
		}
		
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
		if(gscpt.jump_on) {
			if(GUI.Button(jump_unit_button, "1 Jump ("+SINGLE_JUMP_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_JUMP_PRICE) {
					gscpt.num_coins -= SINGLE_JUMP_PRICE;
					gscpt.jump_ammo++;
				}
			}
		}

		/* TIER 2 : blackhole helper and alien gun */
		//blackhole helper
		if (!gscpt.blackhole_on && !gscpt.gun_on) {
			if(GUI.Button(black_hole_button, "Black Hole Detection")){
				if(gscpt.num_coins >= BLACK_HOLE_HELPER_PRICE){
					gscpt.blackhole_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("Black_Hole_Helper");
					gscpt.blackhole_fitting = pwerup;
					gscpt.num_coins -= BLACK_HOLE_HELPER_PRICE;
					DontDestroyOnLoad(pwerup);	
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
		if(gscpt.gun_on) {
			if(GUI.Button(alien_defense_button, "20 Torpedos ("+SINGLE_GUN_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_GUN_PRICE) {
					gscpt.num_coins -= SINGLE_GUN_PRICE;
					gscpt.gun_ammo += 20;
				}
			}
		}
		
		/* TIER 3 : direction shift and time warp (reset) */
		//direction shift
		if (!gscpt.direction_on && !gscpt.timewarp_on) {
			if(GUI.Button(dir_shift_button, "Rapid Direction Shift")){
				if(gscpt.num_coins >= DIR_SHIFT_PRICE){
					gscpt.direction_on = true;
					GameObject pwerup = new GameObject();
					pwerup.AddComponent("direction_shift");
					gscpt.direction_fitting = pwerup;
					gscpt.num_coins -= DIR_SHIFT_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		if(gscpt.direction_on) {
			if(GUI.Button(dir_shift_button, "1 Shift ("+SINGLE_DIR_SHIFT_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_DIR_SHIFT_PRICE) {
					gscpt.num_coins -= SINGLE_DIR_SHIFT_PRICE;
					gscpt.dir_ammo++;
				}
			}
		}
		
		//time warp (reset)
		if (!gscpt.direction_on && !gscpt.timewarp_on) {
			if(GUI.Button(timewarp_button, "Time Machine")){
				if(gscpt.num_coins >= TIME_WARP_PRICE){
					gscpt.timewarp_on = true;
					GameObject pwerup = new GameObject();
			//		pwerup.AddComponent("direction_shift");
					gscpt.timewarp_fitting = pwerup;
					gscpt.num_coins -= TIME_WARP_PRICE;
					DontDestroyOnLoad(pwerup);	
				}
			}
		}
		if(gscpt.timewarp_on) {
			if(GUI.Button(timewarp_button, "1 Reset ("+SINGLE_TIME_WARP_PRICE+" coins)")){
				if(gscpt.num_coins >= SINGLE_TIME_WARP_PRICE) {
					gscpt.num_coins -= SINGLE_TIME_WARP_PRICE;
					gscpt.timewarp_ammo++;
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
		if(gscpt.jump_on)
			GUI.Label(new Rect(xoffset,Screen.height-60,150,50),"Jumps: "+gscpt.jump_ammo);
		if(gscpt.gun_on)
			GUI.Label(new Rect(xoffset,Screen.height-105,150,50),"Torpedos: "+gscpt.gun_ammo);
	//	if(gscpt.bend_on)
	//		GUI.Label(new Rect(xoffset,Screen.height-85,150,50),"Bending fluid: "+gscpt.bend_ammo);
		
		
		
		
		
		
		//load next level button
		if(GUI.Button (new Rect(10,Screen.height-30,200,25), "Play next level")) {
			gscpt.ResetScore();
			Application.LoadLevel("scene1");
		}
		
	}
}
