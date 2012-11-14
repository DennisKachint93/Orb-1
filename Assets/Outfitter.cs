using UnityEngine;
using System.Collections;

public class Outfitter : MonoBehaviour {

	GameObject game_state;
	Game_State gscpt;
	
	// Use this for initialization
	void Start () {
		game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		
		//increment level counter
		gscpt.cur_level++;
		
		//load the game scene, which loads the next level
	}
	
	// Update is called once per frame
	void Update () {
		//N advances to next level
		if(Input.GetKeyDown(KeyCode.N))
			Application.LoadLevel("scene1");
			
		//testing powerup addition
		if(Input.GetKeyDown(KeyCode.B)) {
			//create a new gameobject to replace the old one
			GameObject pwrup = new GameObject();
			//add the script that defines your power up 
			pwrup.AddComponent("Super_Bending");
			//assign the gameobject to the proper tier in game_state
			gscpt.tier_1_upgrade = pwrup;
		}
		
	}
}
