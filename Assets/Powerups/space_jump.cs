using UnityEngine;
using System.Collections;

public class space_jump : MonoBehaviour {

	private float SPACE_DIST = 100f;

	Game_State gscpt;

	// Use this for initialization
	void Start () {
		GameObject gs = GameObject.Find("game_state");
		gscpt = gs.GetComponent<Game_State>();
	}
	
	// Update is called once per frame
	void Update () {
		
		//space jump
		if(Input.GetKeyUp(KeyCode.E) && gscpt.jump_ammo > 0 && !Learth_Movement.isTangent ){
        	Vector3 jump = Learth_Movement.velocity * SPACE_DIST;
            Manager.l.transform.position = new Vector3(Manager.l.transform.position.x+jump.x, 
				Manager.l.transform.position.y+jump.y, Manager.l.transform.position.z+jump.z);
           	Learth_Movement.lastPos.transform.Translate(jump);
			gscpt.jump_ammo--;
        }
	}
}
