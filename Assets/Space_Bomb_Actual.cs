using UnityEngine;
using System.Collections;

public class Space_Bomb_Actual : MonoBehaviour {
	
	public Vector3 velocity;
	public Transform lastPos;
	
	// Use this for initialization
	void Start () {
		Debug.Log("bomb deployed");
	//	lastPos = new GameObject().transform;
	}
	
	// Update is called once per frame
	void Update () {
		//calculate velocity every frame
		velocity = this.transform.position - lastPos.position;
		lastPos.position = this.transform.position;
		//regular movement
		this.transform.position += velocity.normalized*Manager.speed*2;	
	}
}
