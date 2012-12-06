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
			//if learth collides with a space rip, die
			if(collision.gameObject.name == "Space_Rip(Clone)") {
				if(!Manager.IS_INVINCIBLE)	
					Manager.Die();
			}
			//if learth collides with a star, die
			if(collision.gameObject.name == "Star(Clone)") {
				//if the invincibility powerup is turned on, instead of dying, blow the star up and steal its energy
				if(Manager.IS_INVINCIBLE) {
					Manager.energy += Manager.INVINC_ENERGY_BONUS;
					Starscript col_scpt = collision.gameObject.GetComponent<Starscript>();
					col_scpt.removeStar(2);
				}
				else {
					Manager.Die();	
				}
			}
			if(collision.gameObject.name == "coin(Clone)") {
				GameObject go = GameObject.Find("Alien_Exp_Sound");
				Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
				ascpt.coin_grab.Play();
				gscpt.coins_collected++;
				Manager.energy += 3;
				cpe = Instantiate (coin_pickup_effect, collision.gameObject.transform.position, 
					collision.gameObject.transform.rotation) as GameObject;
				Destroy(collision.gameObject);
			}
			
		
	}	
	
	
	
}
