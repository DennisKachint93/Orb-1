using UnityEngine;
using System.Collections;

public class Line_Script_menu : MonoBehaviour {
	
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line, line;
	public float line_length;
	public Vector3 start, new_start;
	public float rate = .5f;
	public bool vertical, up, right;
	public bool complete = false, new_line = false;
	
	void Start() {
		start = this.transform.position;
		line_length = Random.Range(50,250);
	}
	
	void Update() {
		//if (!renderer.isVisible)
	//		Destroy(gameObject);
		if (!complete) {
			if (vertical) {
				if (right)
					this.transform.position += new Vector3(0,rate,0);
				else
					this.transform.position += new Vector3(0,-rate,0);
			} else {
				if (up)
					this.transform.position += new Vector3(rate,0,0);
				else
					this.transform.position += new Vector3(-rate,0,0);
			}
			if (Vector3.Distance(start,this.transform.position) >= line_length) {
				complete = true;
				Destroy(line);
			}
			
			if (!new_line && Vector3.Distance(start,this.transform.position) >= line_length/4) {
				new_line = true;
				if (vertical) 
					new_start = start + new Vector3(Random.Range(-line_length/4,line_length/4),0,0);
				else
					new_start = start + new Vector3(0, Random.Range(-line_length/4,line_length/4), 0);
				
				if (Menu.l.renderer.material.color == Color.red) 
					line = Instantiate (red_line, new_start, Menu.l.transform.rotation) as GameObject;
				else if (Menu.l.renderer.material.color == Manager.orange) 
					line = Instantiate (orange_line, new_start, Menu.l.transform.rotation) as GameObject;
				else if (Menu.l.renderer.material.color == Color.yellow) 
					line = Instantiate (yellow_line, new_start, Menu.l.transform.rotation) as GameObject;
				else if (Menu.l.renderer.material.color == Manager.green) 
					line = Instantiate (green_line, new_start, Menu.l.transform.rotation) as GameObject;
				else if (Menu.l.renderer.material.color == Color.blue) 
					line = Instantiate (blue_line, new_start, Menu.l.transform.rotation) as GameObject;
				else if (Menu.l.renderer.material.color == Manager.aqua) 
					line = Instantiate (aqua_line, new_start, Menu.l.transform.rotation) as GameObject;
				else if (Menu.l.renderer.material.color == Manager.purple) 
					line = Instantiate (purple_line, new_start, Menu.l.transform.rotation) as GameObject;
					
				//Destroy(line,5);
				Line_Script_menu scpt = line.GetComponent<Line_Script_menu>();
				scpt.vertical = !vertical;
				scpt.new_line = false;
				scpt.complete = false;
			}
		}
	}
}
