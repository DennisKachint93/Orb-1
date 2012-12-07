using UnityEngine;
using System.Collections;

public class Learth_Outfitter : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
		transform.Rotate(0, Time.deltaTime*10, 0);
	}
}
