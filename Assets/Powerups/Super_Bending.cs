using UnityEngine;
using System.Collections;

public class Super_Bending : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Increase bend constant in manager
		Manager.BEND_FACTOR = 12;
		//bend cost
		Manager.BEND_COST = .025f;
		
		//reset stuff
		Manager.CONSTANT_SPEED = 5f;
		Manager.RADIAL_ERROR = 10;
		Manager.TAN_ERROR = 8;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
