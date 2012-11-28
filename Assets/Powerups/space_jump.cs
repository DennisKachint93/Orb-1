using UnityEngine;
using System.Collections;

public class space_jump : MonoBehaviour {

	private float SPACE_DIST = 100f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		//space jump
		if(Input.GetKeyUp(KeyCode.C)){
        	Vector3 jump = Learth_Movement.velocity * SPACE_DIST;
            Manager.l.transform.position = new Vector3(Manager.l.transform.position.x+jump.x, 
				Manager.l.transform.position.y+jump.y, Manager.l.transform.position.z+jump.z);
           	Learth_Movement.lastPos.transform.Translate(jump);
        }
	}
}
