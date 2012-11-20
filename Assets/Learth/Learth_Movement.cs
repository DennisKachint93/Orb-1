using UnityEngine;
using System.Collections;

public class Learth_Movement : MonoBehaviour {
	
	
	public static Vector3 velocity = new Vector3(1f, 1f, 0f);
	public static Transform lastPos;
	public static bool isTangent = false;
	Game_State gscpt;
	
	//explosion prefab
	//public GameObject explosion, e;
	
	
	void Start () {
		
		//get gamestate
		
		GameObject game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		
		//initialize lastPos
		lastPos = new GameObject().transform;
		
		lastPos.position = this.transform.position - new Vector3(Manager.speed, Manager.speed, 0f);	
	}	
	
	void Update () {
		
		//calculate velocity every frame
		velocity = this.transform.position - lastPos.position;
		lastPos.position = this.transform.position;
		//regular non-orbiting movement
		if (!isTangent) {
			this.transform.position += velocity.normalized*Manager.speed;	
		}
	
		if(Manager.energy < 1)
			Application.LoadLevel("Scene1");
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
				gscpt.num_coins++;
				Manager.energy += 3;
				Destroy(collision.gameObject);
			}
			
		}
	}	
	
	
	
}
