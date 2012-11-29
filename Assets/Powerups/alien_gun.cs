using UnityEngine;
using System.Collections;

public class alien_gun : MonoBehaviour {
	
	public Manager manager;
	
	private float checktime = 0f;
	private float timebetween = 1.5f;
	
	private bool in_game = false;
	
	
	Game_State gscpt;
	
	// Use this for initialization
	void Start () {	
		GameObject gs = GameObject.Find("game_state");
		gscpt = gs.GetComponent<Game_State>();
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
		if(in_game) {
			//check all aliens, if one is close enough to shoot, shoot at it 
			for(int j = 0; j<Manager.alien_arr.Length; j++){
		        if(Manager.alien_arr[j] != null 
					&& Vector3.Distance(Manager.l.transform.position, Manager.alien_arr[j].transform.position) < Manager.LEARTH_GUN_DISTANCE 
					&& (Time.time - checktime)>timebetween
					&& gscpt.gun_ammo > 0)
	            {
	                GameObject learth_bul = Instantiate(manager.lbullet, Manager.l.transform.position, Quaternion.identity) as GameObject;
	                bullet_behav bstuff = learth_bul.GetComponent<bullet_behav>();
	                bstuff.SetTarget(Manager.alien_arr[j].transform.position);
		        	checktime = Time.time;
					gscpt.gun_ammo--;
				}
			}
		}
		else {
			if(gscpt.in_game) {
				GameObject m = GameObject.Find("manager");
				manager = m.GetComponent<Manager>();
				in_game = true;
			}
		}
	}
}
