using UnityEngine;
using System.Collections;

public class End_Scene : MonoBehaviour {
	
	Game_State state;
	public GUISkin skin;
	public int starty,startx,credits_offset;
	
	//star for visuals
	public GameObject star;

	// Use this for initialization
	void Start () {
		
		startx = Screen.width/3;
		starty = Screen.height/6;
		credits_offset = 10;
		GameObject s = GameObject.Find("game_state");
		state = s.GetComponent<Game_State>();
		
		GameObject starE = Instantiate (star, new Vector3(0,0,0), new Quaternion(0,0,0,0)) as GameObject;
		Starscript starscript = starE.GetComponent<Starscript>();
		starscript.c = Color.red;
		starscript.starSize = 30; 
		starscript.is_revolving = true;
		starscript.rpoint = new Vector3(20,20,0);
		starscript.rspeed = 45;
		starscript.editor_freeze = true;
		
	}
	
	void Update () {
	
	}
	
	void OnGUI() {   
		GUI.skin = skin;
		GUI.backgroundColor = Color.black;
		
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.label.fontSize = 24;
		//total points
		GUI.Label(new Rect(startx, starty, 500, 50), "Well, that's it. Good work, sport.");
		
		GUI.skin.label.fontSize = 30;
		GUI.Label(new Rect(startx+20, starty+50, 500, 50), "You completed the game!");
	
		GUI.skin.button.fontSize = 24;	
		GUI.skin.button.normal.textColor = Color.yellow;
		GUI.skin.button.hover.textColor = Color.blue;
       	if (GUI.Button(new Rect(startx+150, starty+150, 200, 60), "Play Again?")) {
			state.cur_level = 0;
			Manager.ResetConstants();
			Application.LoadLevel("Scene1");
		} 
		
		//breakdown of orbs by color
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.label.fontSize = 30;
		GUI.Label(new Rect(startx+credits_offset+50, starty+290, 200, 50), "CREDITS");
		GUI.skin.label.fontSize = 19;
		GUI.skin.label.normal.textColor = Color.blue;
		GUI.Label(new Rect(startx+credits_offset, starty+350, 400, 50), "Melissa Ewing	");
		GUI.skin.label.normal.textColor = Manager.aqua;
		GUI.Label(new Rect(startx+credits_offset+175, starty+350, 400, 50),"Malcolm Balch-Crystal");
		GUI.skin.label.fontSize = 16;
		GUI.skin.label.normal.textColor = Manager.orange;
		GUI.Label(new Rect(startx+credits_offset+30, starty+380, 400, 50), "Dennis Kachintsev");
		GUI.skin.label.normal.textColor = Color.red;
		GUI.Label(new Rect(startx+credits_offset+205, starty+380, 400, 50), "Ethan Zimmermann  ");
		GUI.skin.label.normal.textColor = Manager.purple;
		GUI.Label(new Rect(startx+credits_offset+70, starty+420, 400, 50), "Special thanks to Tom Wexler");
		GUI.Label(new Rect(startx+credits_offset+60, starty+450, 400, 50), "and almost developer Charles Marks");

	}
}
