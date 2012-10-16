using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
		public GameObject learth;
		public GameObject test_star;
		GameObject l, s;

	// Use this for initialization
	void Start ()
	{
		l = Instantiate (learth, new Vector3 (-20, -40, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		s = Instantiate(test_star, new Vector3(0,40,0), new Quaternion(0,0,0,0)) as GameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 l_movement = 
	
	}
}
