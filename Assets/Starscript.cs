using UnityEngine;
using System.Collections;

public class Starscript : MonoBehaviour {
	
	public float orbitRadius;
	public GameObject radius;
	public GameObject r;
	public Color c = Color.gray;
	public float starSize = 15f;
	
	void Start () {
		this.transform.localScale *= starSize;
		orbitRadius = starSize;
		
		//instantiate orbital radius guide
		r = Instantiate(radius, this.transform.position, new Quaternion (0, 0, 0, 0)) as GameObject;
		r.transform.Rotate(new Vector3 (90, 0, 0));
		r.transform.localScale *= (2*orbitRadius);
		
	}
	
	void Update() {
		renderer.material.color = c;
		r.renderer.material.color = c;
	}
	
}
