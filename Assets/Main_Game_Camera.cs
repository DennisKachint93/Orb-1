using UnityEngine;
using System.Collections;

public class Main_Game_Camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//set camera height for beginning a game
		Camera.main.orthographicSize = Manager.CAM_START_HEIGHT;
	
		//start camera on top of learth
		Camera.main.transform.position = new Vector3(Manager.l.transform.position.x,Manager.l.transform.position.y, Camera.main.transform.position.z);	
	}
	
	// Update is called once per frame
	void Update () {
		//camera follows learth
		Camera.main.transform.position = Learth_Movement.isTangent ? 
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(Manager.cur_star.transform.position.x,Manager.cur_star.transform.position.y,Camera.main.transform.position.z),Manager.ORBIT_LERP*Time.deltaTime)
				:
				Vector3.Lerp(Camera.main.transform.position, 
				new Vector3(Manager.l.transform.position.x,Manager.l.transform.position.y,Camera.main.transform.position.z),Manager.TRAVEL_LERP*Time.deltaTime)
				;
		
		//A moves the camera farther, S moves the camera closer
		if(Input.GetKey(KeyCode.Minus) && Camera.main.orthographicSize <= Manager.CAM_MAX_DIST)
			Camera.main.orthographicSize += Manager.CAM_MOVE_SPEED;
		if(Input.GetKey(KeyCode.Plus) && Camera.main.orthographicSize >= Manager.CAM_MIN_DIST)
			Camera.main.orthographicSize -= Manager.CAM_MOVE_SPEED;
	}
}
