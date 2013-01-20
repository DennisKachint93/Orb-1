using UnityEngine;
using System.Collections;

public class Line_Script : MonoBehaviour {
	
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line, line;
	public float line_length;
	public Vector3 start, new_start;
	public float rate = 3f;
	public bool vertical, up, right;
	public bool complete = false, new_line = false;
	
	void Start() {
		start = this.transform.position;
		line_length = Random.Range(100,300);
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
			if (Vector3.Distance(start,this.transform.position) >= line_length) 
				complete = true;
			
			if (!new_line && Vector3.Distance(start,this.transform.position) >= line_length/4) {
				new_line = true;
				if (vertical) 
					new_start = start + new Vector3(Random.Range(-line_length/4,line_length/4),0,0);
				else
					new_start = start + new Vector3(0, Random.Range(-line_length/4,line_length/4), 0);
				
				if (Manager.l.renderer.material.color == Color.red) 
					line = Instantiate (red_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.orange) 
					line = Instantiate (orange_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Color.yellow) 
					line = Instantiate (yellow_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.green) 
					line = Instantiate (green_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Color.blue) 
					line = Instantiate (blue_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.aqua) 
					line = Instantiate (aqua_line, new_start, Manager.l.transform.rotation) as GameObject;
				else if (Manager.l.renderer.material.color == Manager.purple) 
					line = Instantiate (purple_line, new_start, Manager.l.transform.rotation) as GameObject;
					
				Destroy(line,5);
				Line_Script scpt = line.GetComponent<Line_Script>();
				scpt.vertical = !vertical;
				scpt.new_line = false;
				scpt.complete = false;
			}
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		//if (collision.gameObject.name == "Learth(Clone)") {
			//	Background.activated = false;
		//		Destroy(gameObject);
		//}
		if(collision.gameObject.tag == "blackhole") {
			print ("black hole!!");
			this.transform.RotateAround(collision.gameObject.transform.position, Vector3.forward, 
						1/(Vector3.Distance(this.transform.position, collision.gameObject.transform.position)*Time.deltaTime)*50);
			Vector3 perp = this.transform.position - collision.gameObject.transform.position;
			perp.Normalize();
			this.transform.position -= perp*Manager.BLACK_HOLE_SUCKINESS;
		}			
		if(collision.gameObject.name == "Star(Clone)") {
	//		print("DESTROY");
			Destroy(this.gameObject);
		}
	}
}
