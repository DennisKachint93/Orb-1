using UnityEngine;
using System.Collections;

public class Easy_Entry : MonoBehaviour {

	void Start () {
		//increasing radial error makes it easier to enter a star
		Manager.EASY_ENTRY = true;
		Manager.RADIAL_ERROR += 10;
		Manager.TAN_ERROR += 5;
	}
	
	void Update () {
	
	}
}
