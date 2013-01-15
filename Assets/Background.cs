using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	
	public GameObject red_line, orange_line, yellow_line, green_line,
		blue_line, aqua_line, purple_line, line;
	public float line_length;
	public Vector3 start;
	public float rate = 2.0f;
	public bool vertical = true;
	
	void Start() {
		start = Manager.l.transform.position+new Vector3(Random.Range(-10, 10),Random.Range(-10, 10),0);
		line = Instantiate (red_line, start, Manager.l.transform.rotation) as GameObject;
		line_length = Random.Range(0,10);
	}
	
	void Update() {
		if (vertical) {
			line.transform.position += new Vector3(0,rate,0);
		//	if (Vector3.Distance(start,line.transform.position) > line_length) 
				
		}
		else {
			line.transform.position += new Vector3(rate,0,0);
		}
	}
	
}
