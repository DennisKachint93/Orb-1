using UnityEngine;
using System.Collections;

public class Line_Script : MonoBehaviour {
	
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line;
	public GameObject line;
	public float line_length;
	public Vector3 start, new_start;
	public float rate = .3f;
	public bool vertical, up, right;
	public bool complete = false, new_line = false;
	
	void Start() {
		start = this.transform.position;
		line_length = Random.Range(50,175);
//		line = new GameObject();
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
				else 
					line = Instantiate (purple_line, new_start, Manager.l.transform.rotation) as GameObject;
					
				//Destroy(line,5);
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
		if(collision.gameObject.name == "Black Hole(Clone)") {
			//TrailRenderer trail = this.gameObject.GetComponent<TrailRenderer>();
		//	trail.time = 50;
			print ("black hole!!");
		//	Starscript bhole = collision.gameObject.GetComponent<Starscript>();
		//	bhole.black_hole_active = true;
			complete = false;
			this.transform.RotateAround(new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, 0), Vector3.forward, 30*Time.deltaTime);
			Vector3 perp = this.transform.position - collision.gameObject.transform.position;
			perp.Normalize();
			this.transform.position -= new Vector3(perp.x, perp.y, 0)*Manager.BLACK_HOLE_SUCKINESS;
		}			
		if(collision.gameObject.name == "Center(Clone)") {
			print("DESTROY");
			Destroy(this.gameObject);
		}
	}
}
