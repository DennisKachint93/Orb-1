using UnityEngine;
using System.Collections;

public class space_bomb_detonation : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(transform.localScale.x > 500) {
			
		} else {
			transform.localScale += new Vector3(5,5,0);
		}
	}
	
	
	void OnCollisionStay(Collision c)
	{
		if(c.transform.name == "Star(Clone)" || c.transform.name == "Space_Rip(Clone)")
		{
			Destroy(c.gameObject);
			//	c.transform.Translate(new Vector3(100*Time.deltaTime,0,0));
		}
		//Debug.Log("col: "+c.transform.name);
	}
}
