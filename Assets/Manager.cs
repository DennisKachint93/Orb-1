using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	//PROBLEMS: sometimes planet doesn't immediately leave orbit
	//			can only enter orbit from one direction (planet turns around) 
	//			hard to aim -- maybe add guide thing again?
	
	public GameObject learth;
	public GameObject star;		
	public static GameObject l, s, s1, s2, s3, s4, s5, s6, s7;
	public static Vector3 tangent;
	public GameObject[] starrArr;
	public int numStars = 0;

	void Start () {
		//instantiate learth
		l = Instantiate (learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
		//instantiate stars and store them in array
		starrArr = new GameObject[7]; 
		s1 = Instantiate (star, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s1script = s1.GetComponent<Starscript>();
		s1script.c = Color.blue;
		s1script.orbitRadius = 25f;
		
		s2 = Instantiate (star, new Vector3 (-100, 50, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s2script = s2.GetComponent<Starscript>();
		s2script.c = Color.red;
		
		s3 = Instantiate (star, new Vector3 (-70, -20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s3script = s3.GetComponent<Starscript>();
		s3script.c = Color.yellow;
		s3script.orbitRadius = 25f;
		
		s4 = Instantiate (star, new Vector3 (120, -50, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s4script = s4.GetComponent<Starscript>();
		s4script.c = Color.yellow;
		s4script.orbitRadius = 30f;
		
		s5 = Instantiate (star, new Vector3 (90, 60, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s5script = s5.GetComponent<Starscript>();
		s5script.c = Color.green;
		s5script.orbitRadius = 35f;
		
		s6 = Instantiate (star, new Vector3 (70, -20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s6script = s6.GetComponent<Starscript>();
		s6script.c = Color.white;
		s6script.orbitRadius = 25f;
		
		s7 = Instantiate (star, new Vector3 (-100, -70, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s7script = s7.GetComponent<Starscript>();
		s7script.c = Color.cyan;
		s7script.orbitRadius = 30f;
		
		numStars+=7;
		starrArr[0] = s1;
		starrArr[1] = s2;
		starrArr[2] = s3;
		starrArr[3] = s4;
		starrArr[4] = s5;
		starrArr[5] = s6;	
		starrArr[6] = s7;
	}
	
	void Update () {
		//if learth is tangent to star s, rotate around star s
		if (Learth_Movement.isTangent) {
			l.transform.RotateAround(s.transform.position, Vector3.forward, 60*Time.deltaTime);
			//if space bar is pressed, accelerate away from star. Problem: sometimes star gets stuck in orbit because its still within orbital radius
			if (Input.GetKeyDown(KeyCode.Space)) {
				Learth_Movement.isTangent = false;
				l.transform.position += Learth_Movement.velocity;				
				l.transform.position += Learth_Movement.velocity;
				l.transform.position += Learth_Movement.velocity;
				l.transform.position += Learth_Movement.velocity;
			}
		}
		//if earth is not tangent to any star, loop through array and calculate tangent vectors to every star
		else if (!Learth_Movement.isTangent) {
			for (int i = 0; i < numStars; i++){
				s = starrArr[i];
				Starscript sscript = s.GetComponent<Starscript>();
				Vector3 l_movement = Learth_Movement.velocity;
				Vector3 star_from_learth = s.transform.position - l.transform.position;
				Vector3 projection = Vector3.Project (star_from_learth, l_movement);
				tangent = projection + l.transform.position;
				//if planet is within star's orbital radius, set isTangent to true
				if (Vector3.Distance(s.transform.position, l.transform.position) >= (sscript.orbitRadius - 5) && Vector3.Distance(s.transform.position, l.transform.position) <= (sscript.orbitRadius + 5) && Vector3.Distance (tangent, l.transform.position) <= 2f) {
					Learth_Movement.isTangent = true;
					break;
				}
			}
		}
	
	}
		
}
	
