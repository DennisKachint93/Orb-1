using UnityEngine;
using System.Collections;

public class Postgame : MonoBehaviour {

	Game_State state;

	// Use this for initialization
	void Start () {
		GameObject s = GameObject.Find("game_state");
		state = s.GetComponent<Game_State>();
		
		//increment level order
		state.cur_level++;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {   
       	if (GUI.Button(new Rect(20, 20, 75, 25), "Next Level")) {
			Application.LoadLevel("Scene1");
		}
	}
}
