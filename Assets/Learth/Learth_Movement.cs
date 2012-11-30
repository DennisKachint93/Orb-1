using UnityEngine;
using System.Collections;

public class Learth_Movement : MonoBehaviour {
	
	
	public static Vector3 velocity = new Vector3(1f, 1f, 0f);
	public static Transform lastPos;
	public static bool isTangent = false;
	//particle effect for pickingup up coins
	public GameObject coin_pickup_effect, cpe;
	Game_State gscpt;
	Sound_Cart scscpt;
	
	//explosion prefab
	//public GameObject explosion, e;
	
	
	void Start () {
		
		//get gamestate
		GameObject game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		
		//get sound cart
		GameObject go = GameObject.Find ("Sound_Cart");
		scscpt = go.GetComponent<Sound_Cart>();
		
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
				//gscpt.num_coins++;
				//scscpt.audio.PlayOneShot(scscpt.alien_explosion);
				//scscpt.audio.clip = scscpt.coin_capture;
				//scscpt.audio.Play();
				//scscpt.audio.PlayOneShot(scscpt.coin_capture);
				scscpt.coin_collected.Play();
				gscpt.coins_collected++;
				Manager.energy += 3;
				cpe = Instantiate (coin_pickup_effect, collision.gameObject.transform.position, 
					collision.gameObject.transform.rotation) as GameObject;
				Destroy(collision.gameObject);
			}
			
		}
	}	
	
	
	
}
