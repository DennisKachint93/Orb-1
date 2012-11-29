using UnityEngine;
using System.Collections;

public class Super_Bending : MonoBehaviour {
	
	Game_State gscpt;
	
	//amount bending ammo is decreased each frame
	private float AMMO_DEPLETION_RATE = 0.025f;
	
	// Use this for initialization
	void Start () {
		
		GameObject go = GameObject.Find("game_state");
		gscpt = go.GetComponent<Game_State>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(gscpt.bend_ammo <= 0 )
		{
			gscpt.bend_ammo = 0;
			Manager.BEND_FACTOR = 6;
		}
		else if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W))
		{
			Manager.BEND_FACTOR = 13;
			gscpt.bend_ammo -= AMMO_DEPLETION_RATE;
		}
	}
}
