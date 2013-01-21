using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	public bool visible = true;
	public float x1, x2, y1, y2;
	
	void Start () {
		if (visible || Application.loadedLevelName == "Level_Editor")
			renderer.enabled = true;
		else
			renderer.enabled = false;
	}
	
	void Update () {
	
	}
}
