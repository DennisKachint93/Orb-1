using UnityEngine;
using System.Collections;

public class Event_Generator : MonoBehaviour {
	
	private bool event_on = false; //true while an event is occuring
	
	private Color current_color;
	private float current_dur; //timer that counts down the duration of the event
	private float current_mul;
	
	private float last_event_generated;
	
	//every event_frequency seconds there's a change that an event will occur
	private int EVENT_FREQUENCY = 30;
	
	//probability that an event occurs (set = 10 to make events always occur)
	private int EVENT_PROBABILITY = 5;

	private Color[] colors;
	private int[] durations;
	private float[] multipliers;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("events");
		colors = new Color[7];
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
	void GenerateEvent() {
		current_color = colors[Random.Range(0,colors.Length)];
		current_dur = durations[Random.Range(0,durations.Length)];
		current_mul = multipliers[Random.Range(0,multipliers.Length)];
		event_on = true;
		last_event_generated = Time.time;
	}
	
	void OnGUI() {
		//if currently in an event
		if(event_on) {
			//display gui popup about that event (current_color, etc.)
		}
	}
}
