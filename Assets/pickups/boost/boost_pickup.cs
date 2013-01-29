using UnityEngine;
using System.Collections;

public class boost_pickup : MonoBehaviour {
	
	private bool is_activated = false;
	private float start_time;
	private float BOOST_DUR = 5;
	private float BOOST_RATE = 0.5f;

	// Use this for initialization
	void Start () {
		this.transform.Rotate(270,0,0);
	
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
		if(!Level_Editor.delete_button)
			transform.Rotate(new Vector3(0,0,30*Time.deltaTime));
	
	}
	
	void OnCollisionEnter(Collision c) {
		if(c.transform.name == "Learth(Clone)") {
			//sound
			Manager.points += Manager.BOOST_POINTS;
			Manager.Popup(5, ""+Manager.BOOST_POINTS, this.transform.position);
			//increment boost counter in manager
			Manager.boost_count++;
			GameObject go = GameObject.Find("Alien_Exp_Sound");
			Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
			ascpt.boost_2.Play();
			
			is_activated = true;
			start_time = Time.time;
			
			//make gameobject too small to hit while script is running (then it gets destoryed)
			transform.localScale -= transform.localScale;
		}
	}
	void OnMouseDown() {
		if(Level_Editor.delete_button) {	
			Destroy (gameObject);	
		}
	}
}
