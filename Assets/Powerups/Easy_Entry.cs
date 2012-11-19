using UnityEngine;
using System.Collections;

public class Easy_Entry : MonoBehaviour {

	void Start () {
		//increasing radial error makes it easier to enter a star
		Manager.RADIAL_ERROR += 40;
		Manager.TAN_ERROR += 5;
		
		//reset other tier 1 variables
		Manager.BEND_COST = 0;
		Manager.BEND_FACTOR = 4;
		Manager.CONSTANT_SPEED = 5f;
	}
	
	void Update () {
	
	}
}
