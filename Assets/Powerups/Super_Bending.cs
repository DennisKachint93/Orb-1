using UnityEngine;
using System.Collections;

public class Super_Bending : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Increase bend constant in manager
		Manager.BEND_FACTOR = 12;
		//bend cost
		Manager.BEND_COST = .025f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
