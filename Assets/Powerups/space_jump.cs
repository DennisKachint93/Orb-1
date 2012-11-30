
using UnityEngine;
using System.Collections;

public class space_jump : MonoBehaviour {

	
	private float MAX_JUMP = 350;
	
	Manager manager;
	
	private float dist_count = 50;
	private float inc_rate = 1;
	
	private bool jumped_at_limit = false;
	
	Game_State gscpt;

	// Use this for initialization
	void Start () {
		GameObject gs = GameObject.Find("game_state");
		gscpt = gs.GetComponent<Game_State>();
	}

	// Update is called once per frame
	void Update () {
	
		
		//if you hold E down, it increases the distance for the jump 
		if(Input.GetKey(KeyCode.E) && gscpt.jump_ammo > 0 && !Learth_Movement.isTangent && !Manager.is_being_attacked)
		{
			dist_count += inc_rate;
			
			//if you hold for max time, just jump
			if(dist_count >= MAX_JUMP) {
				//prevents another jump when you then later release E
				jumped_at_limit = true;
				DoJump();
			}
			
		}

		//space jump
		if(Input.GetKeyUp(KeyCode.E) && gscpt.jump_ammo > 0 && !Learth_Movement.isTangent && !Manager.is_being_attacked){
			//jump if E key is legitimately down (not down from having jumped at the limit)
			if(!jumped_at_limit)
				DoJump ();
			else
				jumped_at_limit = false;
		}
		
		if(gscpt.in_game)
		{
			GameObject go = GameObject.Find("manager");
			manager = go.GetComponent<Manager>();
		}
		
		
		
	}
	
	void DoJump()
	{
			
			GameObject go = GameObject.Find("Alien_Exp_Sound");
			Alien_Exp_Sound ascpt = go.GetComponent<Alien_Exp_Sound>();
			ascpt.jump.Play();
		
			//explosion
			Instantiate(manager.space_jump_effect,Manager.l.transform.position,Manager.l.transform.rotation);
			
			Vector3 jump = Learth_Movement.velocity * dist_count;
			Manager.l.transform.position = new Vector3(Manager.l.transform.position.x+jump.x, 
					Manager.l.transform.position.y+jump.y, Manager.l.transform.position.z+jump.z);
					
			Learth_Movement.lastPos.transform.position += jump;
			ascpt.jump.Play();
			Instantiate(manager.space_jump_effect,Manager.l.transform.position,Manager.l.transform.rotation);
			gscpt.jump_ammo--;
			dist_count = 50;
		
	}
}
