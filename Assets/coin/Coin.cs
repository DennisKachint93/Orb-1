using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,.5f);
	
	}
	void OnMouseDown() {
		if(Level_Editor.delete_button) {	
			Destroy (gameObject);	
		}
	}
}
