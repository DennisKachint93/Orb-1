using UnityEngine;
using System.Collections;

//this is the actual powerup script you attach to a powerup tier
//the script governing the behavior of the bomb once fired is Space_Bomb_Actual
public class Space_Bomb : MonoBehaviour {
	
	//true if a bomb has been fired but not yet detonated
	private bool bomb_out = false;
	
	//true if manager script has already been gotten
	private bool in_game = false;
	
	//manager to reference the prefab
	public Manager manager;
	
	//the bomb that gets instantiated
	public GameObject sbomb;
	
	//game state
	Game_State gscpt;
	
	private float last_bomb ;
	private float cool_down = 5;
	private bool detonating = false;
	
	// Use this for initialization
	void Start () {
		GameObject gs = GameObject.Find("game_state");
		gscpt = gs.GetComponent<Game_State>();
		bomb_out = false;
		last_bomb = Time.time - cool_down;
	}
	
	// Update is called once per frame
	void Update () {
		
		//if we're actually playing the game, allow control behavior
		if(in_game) {
			if(Input.GetKeyDown(KeyCode.D))
			{
				//if a bomb is not out, instantiate space bomb actual at learth's position
				if(!bomb_out && (Time.time-last_bomb) > cool_down)
				{
					//update last bomb time
					last_bomb = Time.time;
					//make bomb
					sbomb = Instantiate(manager.bomb,Manager.l.transform.position,new Quaternion(0,0,0,0)) as GameObject;
					Space_Bomb_Actual sba = sbomb.GetComponent<Space_Bomb_Actual>();
					//set velocity and lastpos vectors to match learth's current ones
					sba.velocity = new Vector3(Learth_Movement.velocity.x,Learth_Movement.velocity.y,0);
					sba.lastPos = new GameObject().transform;
					sba.lastPos.position = new Vector3(Learth_Movement.lastPos.position.x,Learth_Movement.lastPos.position.y,0); 
					//only one bomb allowed out at a time
					bomb_out = true;
					detonating = false;
				} 
				//if a bomb is out, pressing d again detonates it
				else if(!detonating) {
					detonating = true;
					Space_Bomb_Actual sba = sbomb.GetComponent<Space_Bomb_Actual>();
					sba.Detonate();
					bomb_out = false;
				}
			
			}
		} 
		//otherwise, keep checking if we're in the game or not (otherwise we get a null reference exception in outfitter
		//when the script is looking for the manager GO which does not get instantiated in that scene
		else {
			if(gscpt.in_game) {
				GameObject m = GameObject.Find("manager");
				manager = m.GetComponent<Manager>();
				in_game = true;
			}
		}
	}
}
