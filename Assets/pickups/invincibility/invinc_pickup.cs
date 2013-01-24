using UnityEngine;
using System.Collections;

public class invinc_pickup : MonoBehaviour {
	
	private bool is_activated = false;
	private float start_time;
	private float DURATION = 5;
	private float grow_shrink_time = 1;
	private float grow_shrink_rate = .25f;
	private float start_grow_shrink;
	private bool shrink = false;
	
	// Use this for initialization
	void Start () {
		this.transform.Rotate(90,0,0);
		start_grow_shrink = Time.time;
	
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
	
		if(!Level_Editor.delete_button) {
			//make powerup expand and contract
			if(!is_activated && Time.time - start_grow_shrink > grow_shrink_time) {
				shrink = !shrink;
				start_grow_shrink = Time.time;
			}
			
			if(!is_activated) {
				int ctrl = shrink ? -1 : 1;
				transform.localScale += new Vector3(ctrl*grow_shrink_rate,ctrl*grow_shrink_rate,ctrl*grow_shrink_rate);
			}
		}
		
		
	}
	
	void OnCollisionEnter(Collision c) {
		if(c.transform.name == "Learth(Clone)") {
			is_activated = true;
			start_time = Time.time;
			
			GameObject go = GameObject.Find("Alien_Exp_Sound");
			Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
			ascpt.invinc.Play();
			
			//make gameobject too small to hit while script is running (then it gets destoryed later)
			transform.localScale -= transform.localScale;
		}
	}
	void OnMouseDown() {
		if(Level_Editor.delete_button) {	
			Destroy (gameObject);	
		}
	}
}
