using UnityEngine;
using System.Collections;

public class Game_State : MonoBehaviour {
	
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
	
	//whether or not we're in game
	public bool in_game = false;
	
	//these are where upgrades are stored when bought in the ship_outfitter
	//as much as possible, upgrade behavior should be done in scripts attached to these gameobjects 
	public GameObject tier_1_upgrade;
	public GameObject tier_2_upgrade;
	public GameObject tier_3_upgrade;
	
	// Use this for initialization
	void Start () {
		//DO NO WRITE ANY CODE HERE. I'M SERIOUS.	
	}
	
	// Update is called once per frame
	void Update () {
		//DO NO WRITE ANY CODE HERE. I'M SERIOUS.	
	}
}
