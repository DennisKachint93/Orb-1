using UnityEngine;
using System.Collections;

public class Learth_Movement : MonoBehaviour {

	public static Vector3 velocity = new Vector3(1f, 1f, 0f);
	Vector3 lastPos;
	public static bool isTangent = false;
	
	void Start () {
		lastPos = this.transform.position - new Vector3(.5f, .5f, 0f);	
	}	
	
	void Update () {
		//calculate velocity every frame
		velocity = this.transform.position - lastPos;
		lastPos = this.transform.position;
		//regular non-orbiting movement
		if (!isTangent) {
			this.transform.position += velocity;
		}
	}
	
}
