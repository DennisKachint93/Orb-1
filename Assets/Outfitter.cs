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
		Application.LoadLevel("scene1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
