using UnityEngine;
using System.Collections;

public class Starscript : MonoBehaviour {
	
	public float orbitRadius = 15f;
	public GameObject radius;
	public Color c = Color.gray;
	
	void Start () {
		//instantiate orbital radius guide
		GameObject r = Instantiate(radius, this.transform.position, new Quaternion (0, 0, 0, 0)) as GameObject;
		r.transform.Rotate(new Vector3 (90, 0, 0));
		r.transform.localScale *= (2*orbitRadius);
	}
	
	void Update () {
	
		renderer.material.color = c;
			
	}
}
