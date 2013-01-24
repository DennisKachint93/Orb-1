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
	public GameObject explosion;
	
	//light effect for learth
	public GameObject radius;
	public GameObject lightGameObject;
	//actual radius object instantiated
	public GameObject r;
	
	public GameObject reset_effect;
	
	void Start () {
		
		//get gamestate
		GameObject game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
		
		
		//initialize lastPos
		lastPos = new GameObject().transform;
		
		lastPos.position = this.transform.position - new Vector3(Manager.speed, Manager.speed, 0f);	
		
		r = Instantiate(radius, new Vector3 (this.transform.position.x, this.transform.position.y, 90f), new Quaternion (0, 0, 0, 0)) as GameObject;
		r.light.range = 3*transform.localScale.x;
		//parent radius to learth for destruction
		r.transform.parent = this.transform;
		
	}	
	
	void Update () {
		//light effect every frame
		if (!Manager.IS_INVINCIBLE ) {
			r.light.color = renderer.material.color;
			r.light.range = 2.5f*transform.localScale.x;		
		}
		else {
			r.light.color = Color.white;
			r.light.range = 4.5f*transform.localScale.x;			
		}	
		//calculate velocity every frame
		velocity = this.transform.position - lastPos.position;
		lastPos.position = this.transform.position;
		//regular non-orbiting movement
		if (!isTangent) {
			this.transform.position += velocity.normalized*Manager.speed;	
		}
		
		
		//determine if a star is in learth's immediate path of tangency 
		RaycastHit hit;
		int star_colliders = 1 << 8;
        if (Physics.Raycast(transform.position, velocity.normalized, out hit) && hit.transform.name == "Collider(Clone)") {
			Starscript s = hit.transform.parent.transform.gameObject.GetComponent<Starscript>();
			//make sure not hitting the star part
	/*		hit.transform.collider.enabled = false;
			RaycastHit secondshot;
			if(Physics.Raycast(transform.position,velocity.normalized,out secondshot)) {
				print ("second hit name: "+secondshot.transform.name);	
			}
			hit.transform.collider.enabled = true; */
			s.glow = true;
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{			
			//if learth collides with a space rip, die
			if(collision.gameObject.name == "Space_Rip(Clone)") {
				if(!Manager.IS_INVINCIBLE)	
					Manager.energy -= Manager.SPACE_RIP_COST;
			}
			//if learth collides with a star, die
			if(collision.gameObject.name == "Star(Clone)") {
			
				Starscript scpt = collision.gameObject.GetComponent<Starscript>();
			
			
				//end level if reach sink
			/*	
			 * 
			 * PREVIOUS VERSION END CONDITION
			 * 
			 * 
			 * if (scpt.is_sink) {
					//record delivered energy
					Manager.gscpt.energy_delivered = Manager.energy;
					//increment level counter
					Manager.gscpt.cur_level++;
					//set in game to false
					Manager.gscpt.in_game = false;
					//open the ship outfitter
					Application.LoadLevel("Ship_Outfitter");
				} */
			
			
				//if the invincibility powerup is turned on, instead of dying, blow the star up and steal its energy
				
				
				/* else */if(Manager.IS_INVINCIBLE && !scpt.isBlackHole) {
					Manager.energy += Manager.INVINC_ENERGY_BONUS;
					Starscript col_scpt = collision.gameObject.GetComponent<Starscript>();
					col_scpt.removeStar(2);
				}
				else if (!scpt.spiral) {
					//effect
					Instantiate(reset_effect,Manager.l.transform.position,Manager.l.transform.rotation);
					Manager.Die();	
				}
			}
		
			if(collision.gameObject.name == "Wall(Clone)") {
				if (Manager.IS_INVINCIBLE) {
					Instantiate(explosion, transform.position, transform.rotation);
					Destroy(collision.gameObject);
				}
				else {
					//effect
					Instantiate(reset_effect,Manager.l.transform.position,Manager.l.transform.rotation);
					Manager.Die ();
				}
			}				
	
			if(collision.gameObject.name == "coin(Clone)") {
				GameObject go = GameObject.Find("Alien_Exp_Sound");
				Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
				ascpt.coin_grab.Play();
				gscpt.coins_collected++;
				Manager.energy += 3;
			//	Manager.points += Manager.COIN_POINTS;
				cpe = Instantiate (coin_pickup_effect, collision.gameObject.transform.position, 
					collision.gameObject.transform.rotation) as GameObject;
				Destroy(collision.gameObject);
			}
		
	}	
	
	
	
}
