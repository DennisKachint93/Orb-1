using UnityEngine;
using System.Collections;

public class direction_shift : MonoBehaviour {

	Game_State gscpt;

	// Use this for initialization
	void Start () {
		GameObject game_state = GameObject.Find("game_state");
		gscpt = game_state.GetComponent<Game_State>();
	}
	
	// Update is called once per frame
	void Update () {
		
	    if(Input.GetKeyUp(KeyCode.Z) ){
			//Debug.Break();
            Learth_Movement.lastPos.RotateAround(Manager.l.transform.position, Vector3.forward, 90);
			Manager.energy -= Manager.DIR_SHIFT_COST;
			gscpt.dir_ammo--;
	    }
	    if(Input.GetKeyUp(KeyCode.X) ){
			//Debug.Break();
	    	Learth_Movement.lastPos.RotateAround(Manager.l.transform.position, Vector3.forward, -90);
			Manager.energy -= Manager.DIR_SHIFT_COST;
			gscpt.dir_ammo--;
	    }
	}
}
