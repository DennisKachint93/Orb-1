using UnityEngine;
using System.Collections;

public class boost_pickup : MonoBehaviour {
	
	private bool is_activated = false;
	private float start_time;
	private float BOOST_DUR = 5;
	private float BOOST_RATE = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//if you run into the boost, steadily increase energy until boost is over, then disappear
		if(is_activated) {
			if(Time.time - start_time > BOOST_DUR) {
				Destroy(gameObject);
			} else {
				Manager.energy += BOOST_RATE;	
			}			
		}
		
		//rotate in place
		transform.Rotate(new Vector3(0,180*Time.deltaTime,0));
	
	}
	
	void OnCollisionEnter(Collision c) {
		if(c.transform.name == "Learth(Clone)") {
			is_activated = true;
			start_time = Time.time;
			
			//make gameobject too small to hit while script is running (then it gets destoryed)
			transform.localScale -= transform.localScale;
		}
	}
}
