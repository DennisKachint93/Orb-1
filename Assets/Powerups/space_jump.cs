using UnityEngine;
using System.Collections;

public class space_jump : MonoBehaviour {

	private float SPACE_DIST = 100f;
	
	Manager manager;
	
	
	
	Game_State gscpt;

	// Use this for initialization
	void Start () {
		GameObject gs = GameObject.Find("game_state");
		gscpt = gs.GetComponent<Game_State>();
		
		
	}

	// Update is called once per frame
	void Update () {

		//space jump
		if(Input.GetKeyUp(KeyCode.E) && gscpt.jump_ammo > 0 && !Learth_Movement.isTangent && !Manager.is_being_attacked){
			
			//explosion
			Instantiate(manager.space_jump_effect,Manager.l.transform.position,Manager.l.transform.rotation);
			
			Vector3 jump = Learth_Movement.velocity * SPACE_DIST;
			Manager.l.transform.position = new Vector3(Manager.l.transform.position.x+jump.x, 
					Manager.l.transform.position.y+jump.y, Manager.l.transform.position.z+jump.z);
					
			Learth_Movement.lastPos.transform.Translate(jump);
			Instantiate(manager.space_jump_effect,Manager.l.transform.position,Manager.l.transform.rotation);
			gscpt.jump_ammo--;
		}
		
		if(gscpt.in_game)
		{
			GameObject go = GameObject.Find("manager");
			manager = go.GetComponent<Manager>();
		}
		
		
		
	}
}
