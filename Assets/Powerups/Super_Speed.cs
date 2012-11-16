using UnityEngine;
using System.Collections;

public class Super_Speed : MonoBehaviour {

	void Start () {
		Manager.CONSTANT_SPEED = 5f;	
		
		//reset other tier 1 powerups		
		Manager.BEND_COST = 0;
		Manager.BEND_FACTOR = 4;
		Manager.RADIAL_ERROR = 10;
		Manager.TAN_ERROR = 8;
	}
	
	void Update () {
	
	}
}
