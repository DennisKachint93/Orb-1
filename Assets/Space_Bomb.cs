using UnityEngine;
using System.Collections;

//this is the actual powerup script you attach to a powerup tier
//the script governing the behavior of the bomb once fired is Space_Bomb_Actual
public class Space_Bomb : MonoBehaviour {
	
	//hook into unity
	public GameObject bomb;
	
	//true if a bomb has been fired but not yet detonated
	private bool bomb_out = false;
	
	public Manager manager;

	// Use this for initialization
	void Start () {
		Debug.Log ("Space bomb armed");
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log("bomb ready");
		if(Input.GetKeyDown(KeyCode.C))
		{
			GameObject m = GameObject.Find("manager");
			manager = m.GetComponent<Manager>();
		}
		if(Input.GetKeyDown(KeyCode.D))
		{
			//if a bomb is not out, instantiate space bomb actual	
			if(!bomb_out)
			{
				Debug.Log("deploying bomb...");
			//	GameObject sbomb = Resources.Load("space_bomb_actual") as GameObject;
			//	sbomb.AddComponent("Space_Bomb_Actual");
				GameObject sbomb = Instantiate(manager.bomb,new Vector3(100,0,0),new Quaternion(0,0,0,0)) as GameObject;
				
			} 
			//if a bomb is out, pressing d again detonates it
			else {
					
			}
		
		} 
	}
}
