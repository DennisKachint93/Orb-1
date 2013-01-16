using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line, line;
	public Vector3 start;
	public float cur_time, last_time, delay = 1;
	
	void Start() {
		cur_time = 0;
		last_time = -delay;
	}
	
	void Update() {
		cur_time = Time.time;
		if (cur_time - last_time >= delay) {
			start = new Vector3 (Manager.l.transform.position.x, Manager.l.transform.position.y, 20);//+ new Vector3(Random.Range(-300, 300), Random.Range(-300, 300),0);
			if (Manager.l.renderer.material.color == Color.red) 
				line = Instantiate (red_line, start, Manager.l.transform.rotation) as GameObject;
			else if (Manager.l.renderer.material.color == Manager.orange) 
				line = Instantiate (orange_line, start, Manager.l.transform.rotation) as GameObject;
			else if (Manager.l.renderer.material.color == Color.yellow) 
				line = Instantiate (yellow_line, start, Manager.l.transform.rotation) as GameObject;
			else if (Manager.l.renderer.material.color == Manager.green) 
				line = Instantiate (green_line, start, Manager.l.transform.rotation) as GameObject;
			else if (Manager.l.renderer.material.color == Color.blue) 
				line = Instantiate (blue_line, start, Manager.l.transform.rotation) as GameObject;
			else if (Manager.l.renderer.material.color == Manager.aqua) 
				line = Instantiate (aqua_line, start, Manager.l.transform.rotation) as GameObject;
			else if (Manager.l.renderer.material.color == Manager.purple) 
				line = Instantiate (purple_line, start, Manager.l.transform.rotation) as GameObject;
			Destroy(line,5);
			Line_Script scpt = line.GetComponent<Line_Script>();
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
		}
	}	
}
