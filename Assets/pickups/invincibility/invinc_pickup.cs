using UnityEngine;
using System.Collections;

public class invinc_pickup : MonoBehaviour {
	
	private bool is_activated = false;
	private float start_time;
	private float DURATION = 5f;
	private float grow_shrink_time = 1;
	private float grow_shrink_rate = .25f;
	private float start_grow_shrink;
	private bool shrink = false;
	
	//use for learth trail -- visual representation of effect
	public GameObject invinc_trail;
	public GameObject red_learth_trail, orange_learth_trail, yellow_learth_trail, green_learth_trail,
		blue_learth_trail, aqua_learth_trail, purple_learth_trail;
	
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
			Manager.speed *= 1.2f;
			if(Time.time - start_time > DURATION) {
				Manager.IS_INVINCIBLE = false;
				Destroy(gameObject);
				Destroy(Manager.lt);
				Starscript scpt = Manager.lastStar.GetComponent<Starscript>();
				if (scpt.c == Color.red)
					Manager.lt = Instantiate (red_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				else if (scpt.c == Manager.orange)
					Manager.lt = Instantiate (orange_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				else if (scpt.c == Color.yellow)
					Manager.lt = Instantiate (yellow_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				else if (scpt.c == Manager.green)
					Manager.lt = Instantiate (green_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				else if (scpt.c == Color.blue)
					Manager.lt = Instantiate (blue_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				else if (scpt.c == Manager.purple)
					Manager.lt = Instantiate (purple_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				else if (scpt.c == Manager.aqua)
					Manager.lt = Instantiate (aqua_learth_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
				
				Manager.lt.transform.parent = Manager.l.transform;
				Manager.l.renderer.material.color = scpt.c;
				Learth_Movement.r.light.color = scpt.c;
				Learth_Movement.r.light.range = 2.5f*Manager.l.transform.localScale.x;	
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
			
			//particle trail for learth
			Destroy(Manager.lt);
			Manager.lt = Instantiate (invinc_trail, Manager.l.transform.position, Manager.l.transform.rotation) as GameObject;
			Manager.lt.transform.parent = Manager.l.transform;
		}
	}
	void OnMouseDown() {
		if(Level_Editor.delete_button) {	
			Destroy (gameObject);	
		}
	}
}
