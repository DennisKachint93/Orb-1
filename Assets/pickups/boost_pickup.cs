using UnityEngine;
using System.Collections;

public class boost_pickup : MonoBehaviour {
	
	//controls behavior after being collected by learth
	private bool is_activated;
	
	//when powerup was hit
	private float time_start;
	
	//boost length
	private float BOOST_DURATION = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(is_activated)
		{
			if(Time.time - time_start >= BOOST_DURATION) {
				boost_pickup bp = gameObject.GetComponent<boost_pickup>();
				bp.enabled = false;
			}
			Debug.Log("activated!");
			
		}
	
	}
	
	void OnCollisionEnter(Collision c) {
		if(!is_activated && c.transform.name == "Learth(Clone)") {
			is_activated = true;
			time_start = Time.time;
		}
	}
}
