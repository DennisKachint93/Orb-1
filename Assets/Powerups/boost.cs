using UnityEngine;
using System.Collections;

public class boost : MonoBehaviour {
	
	//you get 3 boosts when you buy boost
	private int remaining_boosts = 1;
	
	//size of boost
	private int BOOST_SIZE = 50;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
		if(Input.GetKeyDown(KeyCode.C) && remaining_boosts > 0){
			//one shot major energy boost
			Manager.energy += BOOST_SIZE;
			
			//decrease remaining boosts
			remaining_boosts--;
		}	
	}
}
