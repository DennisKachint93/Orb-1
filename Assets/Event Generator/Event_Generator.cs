using UnityEngine;
using System.Collections;

public class Event_Generator : MonoBehaviour {
	
	private static bool event_on = false; //true while an event is occuring
	
	private static Color current_color;
	private static float current_dur; //timer that counts down the duration of the event
	private static float current_mul;
	
	private static float last_event_generated;
	
	//every event_frequency seconds there's a change that an event will occur
	private static int EVENT_FREQUENCY = 30;
	
	//probability that an event occurs (set = 10 to make events always occur)
	private static int EVENT_PROBABILITY = 5;

	private static Color[] colors;
	private static int[] durations;
	private static float[] multipliers;
	
	//for popup
	public GUISkin skin;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("events");
		colors = new Color[8];
		colors[0] = Color.red;
		colors[1] = Manager.orange;
		colors[3] = Color.yellow; 
		colors[4] = Manager.green;
		colors[5] = Color.blue;
		colors[6] = Manager.aqua;
		colors[7] = Manager.purple;
		durations = new int[3];
		durations[0] = 10;
		durations[1] = 15;
		durations[2] = 20;
		multipliers = new float[5];
		multipliers[0] = 2;
		multipliers[1] = 3;
		multipliers[2] = .5f;
		multipliers[3] = 0;
		multipliers[4] = 4;
	}
	
	// Update is called once per frame
	void Update () {
		
		//end event if it has existed for its duration
		if(event_on) {
			current_dur -= Time.deltaTime;
			if(current_dur <= 0) {
				event_on = false;	
			}
		} else {
			//determine if it's time to try to generate an event
			if(Time.time - last_event_generated >= EVENT_FREQUENCY) 
			{
				//based on the constant probability, decide whether or not to generate an event
				int r = Random.Range(0,10);
				if(r < EVENT_PROBABILITY) {
					GenerateEvent();
				}
			}
		}
	}
	
	//generates and enables a point event
	static void GenerateEvent() {
	//	current_color = colors[0];
		current_color = colors[Random.Range(0,colors.Length)];
		current_dur = durations[Random.Range(0,durations.Length)];
		current_mul = multipliers[Random.Range(0,multipliers.Length)];
		event_on = true;
		last_event_generated = Time.time;
		Debug.Log("new event generated: "+current_color.ToString()+" "+current_dur+" "+current_mul);
	}
	
	//passed the current value of points that is about to be added
	//returns the modified vlue based on what events are currently active
	public static float GetPointsFromEvent(Color c, float points) {
		if(event_on) {
			if(c.Equals(current_color)) {
				//add points
				return points * current_mul;
				//Debug.Log("event success");		
			}
		}
		return 0;
	}
	
	public string colorString(Color c) {
		if (c == Color.red)
			return "red";
		else if (c == Manager.orange)
			return "orange";
		else if (c == Color.yellow)
			return "yellow";		
		else if (c == Manager.green)
			return "green";
		else if (c == Color.blue)
			return "blue";
		else if (c == Manager.purple)
			return "purple";
		else
			return "aqua";
	}
	
	
	void OnGUI() {
		GUI.skin = skin;
		//if currently in an event
		if(event_on) {
				GUI.skin.label.normal.textColor = current_color;
				GUI.skin.label.fontSize = 16;
				GUI.Box(new Rect(Screen.width-210, Screen.height-105,210,120), "");
				GUI.Label(new Rect(Screen.width-200, Screen.height-95,170,100),"For the next "+(int)current_dur+" seconds, "
					+colorString(current_color)+" orbs are worth "+current_mul+"X points."); 
			
			//display gui popup about that event (current_color, etc.)
		}
	}
}
