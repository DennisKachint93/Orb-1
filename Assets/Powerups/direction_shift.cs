using UnityEngine;
using System.Collections;

public class direction_shift : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Manager.DIRECTION_SHIFT = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyUp(KeyCode.Z) && Manager.DIRECTION_SHIFT){
            Learth_Movement.lastPos.RotateAround(Manager.l.transform.position, Vector3.forward, 90);
			Manager.energy -= Manager.DIR_SHIFT_COST;
	    }
	    if(Input.GetKeyUp(KeyCode.X) && Manager.DIRECTION_SHIFT){
	    	Learth_Movement.lastPos.RotateAround(Manager.l.transform.position, Vector3.forward, -90);
			Manager.energy -= Manager.DIR_SHIFT_COST;
	    }
	}
}
