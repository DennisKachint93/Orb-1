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
	public static GameObject r;
	
	public GameObject reset_effect;
	
	public bool wall_down = false;
	
	//for flashing learth effect
	int random;
	
	//the normal of the plane in front of learth (updated every frame)
	Vector3 last_norm;
	
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
		
		random = Random.Range (0,3);
		
	}	
	
	void Update () {
		
		RaycastHit hi;
        if (Physics.Raycast(transform.position, velocity.normalized, out hi))  {
			last_norm = hi.normal;
		}
		
		//light effect every frame
		if (!Manager.IS_INVINCIBLE ) {
			r.light.color = renderer.material.color;
			r.light.range = 2.5f*transform.localScale.x;		
		}
		else {
			if (random == 0) 
				renderer.material.color = Color.blue;
			else if (random == 1)
				renderer.material.color = Color.yellow;
			else if (random == 2)
				renderer.material.color = Color.cyan; 
			random = Random.Range (0,3);
		
		//	this.renderer.material.color = Color.white;
			r.light.color = Color.white;
			r.light.range = 0;//5.5f*transform.localScale.x;			
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
					Manager.Popup(5, ""+Manager.INVINC_ENERGY_BONUS, this.transform.position);
					Manager.points += Manager.INVINC_ENERGY_BONUS;
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
				WallScript script = collision.gameObject.GetComponent<WallScript>();
				if (Manager.IS_INVINCIBLE && script.visible) {
					Instantiate(explosion, transform.position, transform.rotation);
					Destroy(collision.gameObject);
					wall_down = true;
				}
				else {
					//effect
					Instantiate(reset_effect,Manager.l.transform.position,Manager.l.transform.rotation);
				
					//functional but sketchy
					//very sketchy
					Vector3 reflected = -2 * (Vector3.Dot(velocity,Vector3.Normalize(last_norm)) * Vector3.Normalize(last_norm)) - velocity; 
					Vector3 v1 = lastPos.position - transform.position;
					Vector3 v2 = reflected;
					float angle = Vector3.Angle(v1,v2);
					Quaternion q1 = Quaternion.AngleAxis(-2*angle,Vector3.forward);
					Quaternion q2 = Quaternion.AngleAxis(-1*-2*angle,Vector3.forward);
					Vector3 vec1 = q1 * Vector3.Normalize(last_norm);
					Vector3 vec2 = q2 * Vector3.Normalize(last_norm);
					float ang1 = Vector3.Angle(vec1, reflected);
					float ang2 = Vector3.Angle(vec2, reflected);
					
					if(ang1 > ang2)
						reflected = q1 * reflected;
					else 
						reflected = q2 * reflected;
					
				
					lastPos.position = transform.position - reflected;
				
				//	Manager.Die ();
				}
			}				
	
			if(collision.gameObject.name == "coin(Clone)") {
				GameObject go = GameObject.Find("Alien_Exp_Sound");
				Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
				ascpt.coin_grab.Play();
				gscpt.coins_collected++;
				Manager.energy += 3;
				Manager.Popup(3, ""+Manager.COIN_POINTS, this.transform.position);
				Manager.points += Manager.COIN_POINTS;
				cpe = Instantiate (coin_pickup_effect, collision.gameObject.transform.position, 
					collision.gameObject.transform.rotation) as GameObject;
				Destroy(collision.gameObject);
			}
		
	}	
	
	void onGUI() {
		if (wall_down) {
			GUI.skin.label.fontSize = 30;
			GUI.skin.label.normal.textColor = Color.cyan;
			GUI.Label(new Rect(this.transform.position.x + 100,this.transform.position.y -100, 50,50), "50 pts"); 
		}
	}
	
}
