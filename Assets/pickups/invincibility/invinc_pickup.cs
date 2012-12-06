using UnityEngine;
using System.Collections;

public class invinc_pickup : MonoBehaviour {
	
	private bool is_activated = false;
	private float start_time;
	private float DURATION = 5;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//one activated, set isinvincible to true
		if(is_activated) {
			Manager.IS_INVINCIBLE = true;
			if(Time.time - start_time > DURATION) {
				Manager.IS_INVINCIBLE = false;
				Destroy(gameObject);
			}	
		}
		
	}
	
	void OnCollisionEnter(Collision c) {
		if(c.transform.name == "Learth(Clone)") {
			is_activated = true;
			start_time = Time.time;
			
			//make gameobject too small to hit while script is running (then it gets destoryed later)
			transform.localScale -= transform.localScale;
		}
	}
}
