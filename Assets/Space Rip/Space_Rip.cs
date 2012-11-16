using UnityEngine;
using System.Collections;

public class Space_Rip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionStay(Collision c)
	{
		//if colliding with a space bomb explosion, move away from the center of the explosion	
		if(c.transform.name == "space_bomb_range(Clone)")
		{
			transform.Translate(200 * Time.deltaTime*(transform.position - c.transform.position).normalized,Space.World);
		}
	}
}
