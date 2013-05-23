using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Combos : MonoBehaviour {
	
	
	
	public enum combo_members {RedStar,OrangeStar,YellowStar,};
	static int[] order = new int[5]; //the maximum combo length is 5
//	static int[][] combos = new int[10][5]; //there are 10 combos of length <= 5
	// Use this for initialization
	void Start () {
		//declaring a combo (write a method that does this)
		//combos[0][0] = (int)combo_members.RedStar;
		//combos[0][1] = (int)combo_members.RedStar;
	//	combos[0][2] = (int)combo_members.RedStar;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void AddMember(int member) {
		for(int i = 4; i > 0; i--)
			order[i] = order[i-1];
		order[0] = member;	
		CheckForCombos();
	}
	
	private static void CheckForCombos() {
	//	for(int i = 0; i < combos.GetLength(0); i++) {
			
//		}
	}
	
	

	
}
