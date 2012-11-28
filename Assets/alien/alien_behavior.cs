using UnityEngine;
using System.Collections;

public class alien_behavior : MonoBehaviour {
	public bool rotate = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//if aliens are on screen, move toward learth
		if(renderer.isVisible && !rotate){
			Vector2 to_ship = new Vector2(Manager.l.transform.position.x - transform.position.x, Manager.l.transform.position.y - transform.position.y);
			transform.Translate(to_ship.x*.5f*Time.deltaTime, to_ship.y*.5f*Time.deltaTime, 0);
		}
		
		//if close enough to learth, reduce energy
		if(Mathf.Abs (Vector3.Distance(this.transform.position,Manager.l.transform.position)) < Manager.ALIEN_SUCKING_DISTANCE)
		{
			Manager.energy -= Manager.ALIEN_SUCKS_ENERGY;
			transform.Translate(Learth_Movement.velocity.x, Learth_Movement.velocity.y, 0);
			rotate = true;
		}
		else{
			rotate = false;
		}
	}
}
