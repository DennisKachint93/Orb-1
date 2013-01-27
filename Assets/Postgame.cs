using UnityEngine;
using System.Collections;

public class Postgame : MonoBehaviour {

	Game_State state;
	//skin for GUI
	public GUISkin skin;
	public int starty,startx,endy,orb_offset,orb_color_offset;
	//game object for visual representations
	public GameObject star, coin, boost;

	// Use this for initialization
	void Start () {
		
		startx = Screen.width/4;
		starty = Screen.height/4;
		orb_offset = 160;
		orb_color_offset = 185;
		
		GameObject s = GameObject.Find("game_state");
		state = s.GetComponent<Game_State>();
		
		//increment level order if player has gotten enough points
		if (Manager.points >= Manager.req_points && state.cur_level < state.level_order.Length-1)
			state.cur_level++;
		
		//instantiate visual representations of objects next to text
	/*	GameObject starE = Instantiate (star, Camera.main.ScreenToWorldPoint(new Vector3(startx+orb_offset-30, starty+50, 30)), new Quaternion(0,0,0,0)) as GameObject;
		GameObject coin_actual = Instantiate(coin, Camera.main.ScreenToWorldPoint(new Vector3(startx+orb_offset-30, starty+120, 30)), new Quaternion (90, 0, 0, 0)) as GameObject;
		GameObject boost_actual= Instantiate(boost, Camera.main.ScreenToWorldPoint(new Vector3(startx+orb_offset-30, starty+80, 30)), new Quaternion (0, 0, 0, 0)) as GameObject;
		boost_actual.transform.localScale /= 15;
		coin_actual.transform.Rotate(90,0,0);
		coin_actual.transform.localScale /= 5;
		Starscript script = starE.GetComponent<Starscript>();
		script.c = Color.red;
		starE.transform.localScale /= 3;*/
		
	}
	
	void Update () {
	
	}
	
	void OnGUI() {   
		GUI.skin = skin;
		GUI.backgroundColor = Color.black;
		
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.label.fontSize = 30;
		//total points
		GUI.Label(new Rect(startx, starty-10, 500, 50), "YOU EARNED " + Manager.points + " POINTS");
		
		GUI.skin.label.fontSize = 24;
		//total orbs
		GUI.Label(new Rect(startx+orb_offset, starty+50, 300, 50), Manager.red_orbs+Manager.orange_orbs+Manager.yellow_orbs+
				Manager.green_orbs+Manager.blue_orbs+Manager.purple_orbs+Manager.aqua_orbs+" ORBS");
		
		//breakdown of orbs by color
		GUI.skin.label.normal.textColor = Color.red;
		GUI.skin.label.fontSize = 18;
		GUI.Label(new Rect(startx+orb_color_offset, starty+80, 100, orb_color_offset), Manager.red_orbs + " red ");
		GUI.skin.label.normal.textColor = Manager.orange;
		GUI.Label(new Rect(startx+orb_color_offset, starty+100, 150, orb_color_offset), Manager.orange_orbs + " orange ");
		GUI.skin.label.normal.textColor = Color.yellow;
		GUI.Label(new Rect(startx+orb_color_offset, starty+120, 150, orb_color_offset), Manager.yellow_orbs + " yellow ");
		GUI.skin.label.normal.textColor = Manager.green;
		GUI.Label(new Rect(startx+orb_color_offset, starty+140, 150, orb_color_offset), Manager.green_orbs + " green ");
		GUI.skin.label.normal.textColor = Color.blue;
		GUI.Label(new Rect(startx+orb_color_offset, starty+160, 150, orb_color_offset), Manager.blue_orbs + " blue ");
		GUI.skin.label.normal.textColor = Color.cyan;
		GUI.Label(new Rect(startx+orb_color_offset, starty+180, 150, orb_color_offset), Manager.aqua_orbs + " aqua ");
		GUI.skin.label.normal.textColor = Manager.purple;
		GUI.Label(new Rect(startx+orb_color_offset, starty+200, 150, orb_color_offset), Manager.purple_orbs + " purple ");
		
		//coin count
		GUI.skin.label.normal.textColor = Color.white;
		GUI.skin.label.fontSize = 24;
		GUI.Label(new Rect(startx+orb_offset, starty+230, 300, orb_color_offset), state.coins_collected + " COINS");
		GUI.Label(new Rect(startx+orb_offset, starty+260, 300, orb_color_offset), Manager.boost_count + " GEMS");
		
		//tally up energy from stars (seperated by color), coins, and gems
		//text depends on whether point criteria was met
		
		GUI.skin.button.fontSize = 30;
		GUI.skin.button.normal.textColor = Color.white;
		GUI.skin.button.hover.textColor = Color.red;
		if (Manager.points >= Manager.req_points) {
       		if (state.cur_level<state.level_order.Length-1) {
				if (GUI.Button(new Rect(startx+100, starty+310, 300, 40), "Play Next Level")) 
					Application.LoadLevel("Scene1");
			}
			else {
				if (GUI.Button(new Rect(startx+100, starty+310, 300, 40), "Credits")) 
					Application.LoadLevel("End_Scene");
			}
		} 
		else {
			if (GUI.Button(new Rect(startx+120, starty+310, 300, 40), "Try Again")) { 
				Manager.ResetLevel();
				Application.LoadLevel("Scene1");
			}
		}
	}
}
