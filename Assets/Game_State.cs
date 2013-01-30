using UnityEngine;
using System.Collections;

public class Game_State : MonoBehaviour {

	//enables "test bed" behavior in game scene (for level editor)
	public bool le_test = false;
	
	//order of levels to be played
	public string[] level_order;
	
	//current level (position in level_order)
	public int cur_level = 0;
	
	//number of coins
	public int num_coins = 0;
	
	//current score
	public int score = 0;
	
	//number of lives (?)
	public int lives = 5;
	
	//points
	public float[] points;
	
	//whether or not we're in game
	public bool in_game = false;
	
	//these are where upgrades are stored when bought in the ship_outfitter
	//as much as possible, upgrade behavior should be done in scripts attached to these gameobjects 
	public GameObject tier_1_upgrade;
	public GameObject tier_2_upgrade;
	public GameObject tier_3_upgrade;
	
	//where these powerups are stored once they are purchased
	public GameObject bomb_fitting;
	public GameObject jump_fitting;
	public GameObject gun_fitting;
	public GameObject bend_fitting;
	public GameObject blackhole_fitting;
	public GameObject direction_fitting;
	public GameObject timewarp_fitting;
	
	//set to true once correspondign powerup is purchased to allow purchase of ammo/boosts/etc.
	public bool bomb_on  = false;
	public bool jump_on  = false;
	public bool gun_on   = false;
	public bool bend_on  = false;
	public bool blackhole_on = false;
	public bool direction_on = false;
	public bool timewarp_on = false;
	public bool capac_on = false;
	
	//ammunition (and default values)
	public int bomb_ammo    = 3;
	public int jump_ammo    = 3;
	public int gun_ammo     = 30;
	public int dir_ammo		= 20;
	public int timewarp_ammo= 0;
	public float bend_ammo  = 10;
	public float capac_ammo = 10;
	
	//Values used for scoring
	public float coins_collected  = 0;
	public float aliens_killed    = 0;
	public float energy_delivered = 0;
	public float times_died       = 0;
	public float bombs_dropped    = 0;
	public float stars_destroyed  = 0;
	public float num_stars		  = 0;
	public float time_to_complete = 0;
	
	// Use this for initialization
	void Start () {
		//DO NOT WRITE ANY CODE HERE.	
	}
	
	// Update is called once per frame
	void Update () {
		//DO NOT WRITE ANY CODE HERE.
	}
	
	//resets scoring values
	public void ResetScore() {
		coins_collected  = 0;
		aliens_killed    = 0;
		energy_delivered = 0;
		times_died       = 0;
		bombs_dropped 	 = 0;
		stars_destroyed  = 0;
		num_stars		 = 0;
		time_to_complete = 0;
	}
	
	void OnGUI() {
		//if we came here from the level editor 
		if(le_test) {
			//if we're actually in the game
			if(Application.loadedLevel == 1) {
				//display button that lets us return to the level editor 
				if(GUI.Button(new Rect(10, Screen.height - 200, 100, 25), "Back to editor")) {
					Application.LoadLevel("Level_Editor");
				}
				
				//display button to reset level (same function as T)
				if(GUI.Button(new Rect(10, Screen.height - 170, 100, 25), "Reset level")) {
					
				}
			}
		}
	}
}
