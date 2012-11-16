using UnityEngine;
using System.Collections;

public class space_bomb_detonation : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//once the explosion becomes bigger than 2000, stop, otherwise grow
		if(transform.localScale.x > 1000) {
			Destroy(gameObject);	
		} else {
			transform.localScale += new Vector3(30,30,0);
		}
	}
}
