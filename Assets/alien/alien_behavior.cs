using UnityEngine;
using System.Collections;

public class alien_behavior : MonoBehaviour {
	
	public GameObject learth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(renderer.isVisible){
			Vector2 to_ship = new Vector2(learth.transform.position.x - transform.position.x, learth.transform.position.y - transform.position.y);
			transform.Translate(to_ship.x*.5f*Time.deltaTime, to_ship.y*.5f*Time.deltaTime, 0);
		}
	}
}
