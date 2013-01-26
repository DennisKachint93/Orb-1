using UnityEngine;
using System.Collections;

public class Background_menu: MonoBehaviour {
	
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line, line;
	public GameObject purple;
	public Vector3 start;
	public float cur_time, last_time, delay = .4f;
	public static bool activated = true;
	public int random;
	
	void Start() {
		cur_time = 0;
		last_time = -delay;
		random = Random.Range (0,4);
		//purple = Instantiate (purple_line, this.transform.position, this.transform.rotation) as GameObject;
		//Line_Script script = purple.GetComponent<Line_Script>();
		//Component.Destroy(script);
	}
	
	void Update() {
		cur_time = Time.time;
		if (activated) {
			if (cur_time - last_time >= delay) {
				start = new Vector3 (this.transform.position.x, this.transform.position.y, 0);//+ new Vector3(Random.Range(-300, 300), Random.Range(-300, 300),0);
				//if (cur_time%2 == 1) 
				//	line = Instantiate (red_line, start, this.transform.rotation) as GameObject;
				//else if (cur_time%10 == 2) 
				//	line = Instantiate (orange_line, start, this.transform.rotation) as GameObject;
				if (random == 0) 
					line = Instantiate (yellow_line, start, this.transform.rotation) as GameObject;
			//	else if (this.renderer.material.color == Manager.green) 
			//		line = Instantiate (green_line, start, this.transform.rotation) as GameObject;
				else if (random == 1)
					line = Instantiate (blue_line, start, this.transform.rotation) as GameObject;
				else if (random == 2)
					line = Instantiate (aqua_line, start, this.transform.rotation) as GameObject;
				else //if (this.renderer.material.color == Manager.purple) 
					line = Instantiate (purple_line, start, this.transform.rotation) as GameObject;
			//	Destroy(line,5);
				Line_Script_menu scpt = line.GetComponent<Line_Script_menu>();
				scpt.vertical = false;
				if (Learth_Movement.velocity.x <= 0)
					scpt.right = true;
				else
					scpt.right = false;
				if (Learth_Movement.velocity.y <= 0) 
					scpt.up = false;
				else 
					scpt.up = true;
				last_time = cur_time;
				random = Random.Range (0,4);
			}
		}
	}	
}
