using UnityEngine;
using System.Collections;

public class Learth_Movement : MonoBehaviour {
	
	
	public static Vector3 velocity = new Vector3(1f, 1f, 0f);
	public static Transform lastPos;
	public static bool isTangent = false;
	public static bool isMoving = true;
	
	//explosion prefab
	//public GameObject explosion, e;
	
	
	void Start () {
		
		//initialize lastPos
		lastPos = new GameObject().transform;
		
		lastPos.position = this.transform.position - new Vector3(Manager.speed, Manager.speed, 0f);	
	}	
	
	void Update () {
		//calculate velocity every frame
		velocity = this.transform.position - lastPos.position;
		lastPos.position = this.transform.position;
		//regular non-orbiting movement
		if (!isTangent && isMoving) {
			this.transform.position += velocity.normalized*Manager.speed;	
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		if(!Input.GetKey(KeyCode.D) || !Manager.SHIELD)
		{
			//if learth collides with a space rip, die
			if(collision.gameObject.name == "Space_Rip(Clone)") {
				Manager.Die();
			}
			//if learth collides with a star, die
			if(collision.gameObject.name == "Star(Clone)") {
				Manager.Die();	
			}
			if(collision.gameObject.name == "coin(Clone)") {
				Manager.currency++;
				Manager.energy += 3;
				Destroy(collision.gameObject);
			}
			
		}
	}	
	
	
	
}
