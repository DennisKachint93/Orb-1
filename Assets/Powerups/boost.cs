using UnityEngine;
using System.Collections;

public class boost : MonoBehaviour {
	
	
	//size of boost
	private int BOOST_SIZE = 50;
	Game_State gscpt;
	
	// Use this for initialization
	void Start () {
		GameObject gs = GameObject.Find("game_state");
		gscpt = gs.GetComponent<Game_State>();
	}
	
	// Update is called once per frame
	void Update () {		
		if(Input.GetKeyDown(KeyCode.C) && gscpt.capac_ammo > 0 ){
			//one shot major energy boost
			Manager.energy += BOOST_SIZE;
			
			//decrease remaining boosts
			gscpt.capac_ammo--;
		}	
	}
}
