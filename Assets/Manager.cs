using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	
	public GameObject learth;
	public GameObject star;		
	public static GameObject l, s, s1, s2, s3, s4, s5, s6, s7;
	public GameObject[] starrArr;
	public int numStars = 0;
	
	//level related variables, not sure how this works with different scenes. might need another class for these
	
	//positions past which learth will die. levels are always rectangles
	float LEVEL_X_MAX = 200;
	float LEVEL_X_MIN = -200;
	float LEVEL_Y_MAX = 200;
	float LEVEL_Y_MIN = -200;
	
	//learth-related variables
	public static int energy = 2;
	public GameObject lastStar;
	public static Vector3 tangent;
	public bool clockwise = false;
	private int num_deaths = 0;
	
	public Color orange = new Color(255, 140, 0, 1);
	
	void Start () {
		//instantiate learth
		l = Instantiate (learth, new Vector3 (0, -35, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		
		//instantiate stars and store them in array
		starrArr = new GameObject[7]; 
		
		s1 = Instantiate (star, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s1script = s1.GetComponent<Starscript>();
		s1script.c = Color.white;
		s1script.starSize = 25f;
		
		s2 = Instantiate (star, new Vector3 (-100, 50, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s2script = s2.GetComponent<Starscript>();
		s2script.c = Color.blue;
		
		s3 = Instantiate (star, new Vector3 (-70, -20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s3script = s3.GetComponent<Starscript>();
		s3script.c = Color.yellow;
		s3script.starSize = 25f;
		
		s4 = Instantiate (star, new Vector3 (120, -50, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s4script = s4.GetComponent<Starscript>();
		s4script.c = Color.white;
		s4script.starSize = 30f;
		
		s5 = Instantiate (star, new Vector3 (90, 60, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s5script = s5.GetComponent<Starscript>();
		s5script.c = Color.red;
		s5script.starSize = 35f;
		
		s6 = Instantiate (star, new Vector3 (70, -20, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s6script = s6.GetComponent<Starscript>();
		s6script.c = Color.red;
		s6script.starSize = 25f;
		
		s7 = Instantiate (star, new Vector3 (-100, -70, 0), new Quaternion (0, 0, 0, 0)) as GameObject;
		Starscript s7script = s7.GetComponent<Starscript>();
		s7script.c = Color.blue;
		s7script.starSize = 30f;
		
		//lastVelocity = Learth_Movement.velocity;
		lastStar = s7;
		numStars+=7;
		starrArr[0] = s1;
		starrArr[1] = s2;
		starrArr[2] = s3;
		starrArr[3] = s4;
		starrArr[4] = s5;
		starrArr[5] = s6;	
		starrArr[6] = s7;
	}
	
	//puts Learth in orbit given an entrance point, a velocity, a star, and a direction
	void MoveLearthToOrbit(Vector3 entrance_point, Vector3 entrance_velocity, GameObject star, bool clockwise )
	{
		Debug.Log("moving learth");
		s = star;
		Learth_Movement.isTangent = true;
		l.transform.position = Vector3.Lerp(l.transform.position,entrance_point,100.0F);
		Learth_Movement.velocity = entrance_velocity;
		this.clockwise = clockwise;
	} 
	
	//call this anytime something "kills" the player
	void Die()
	{
		//death animation here?
		
		//if you screw up too much at the beginning, or if you've died more than 3 times, just start the level over
		if(Learth_Movement.last_star_gos[num_deaths] == null || num_deaths > 2)
		{
			ResetLevel();
		}
		//otherwise, you move back 1, 2, or 3 stars
		else {
			MoveLearthToOrbit(Learth_Movement.last_stars[num_deaths], Learth_Movement.last_stars_velocity[num_deaths], 
				Learth_Movement.last_star_gos[num_deaths], Learth_Movement.last_star_rots[num_deaths]);
			num_deaths++;
		}
		
	}
	
	//reloads the scene and modifies whatever we want to modify when the scene gets reloaded
	void ResetLevel() {
		Application.LoadLevel(Application.loadedLevel);	
	}
	
	void Update () {
		
		// for testing purposes, R causes a death.
		if(Input.GetKeyDown(KeyCode.R))
		{
			Die();
		}
		
		
		//Death conditions
		//if you run out of energy, you die, but you get a little energy back
		if(energy < 0)
		{
			Die ();
			energy = 2;
		}
		
		//if you travel outside the bounds of the level, you die
		if(l.transform.position.x > LEVEL_X_MAX
			|| l.transform.position.x < LEVEL_X_MIN
			|| l.transform.position.y > LEVEL_Y_MAX
			|| l.transform.position.y < LEVEL_Y_MIN)
			Die ();
		
		
		
		//if learth is tangent to star s, rotate around star s
		if (Learth_Movement.isTangent) {
			if (clockwise){
				l.transform.RotateAround(s.transform.position, Vector3.forward, -60*Time.deltaTime);
			}
			else  {
				l.transform.RotateAround(s.transform.position, Vector3.forward, 60*Time.deltaTime);
			}
			if (Vector3.Distance (l.transform.position, tangent) < .5) {
				energy -= 1;
			}
			//if space bar is pressed, accelerate away from star. Problem: sometimes star gets stuck in orbit because its still within orbital radius
			if (Input.GetKeyDown(KeyCode.Space)) {
				Learth_Movement.isTangent = false;
				lastStar = s;			
				energy -= 1;
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
				if (s != lastStar && Vector3.Distance(s.transform.position, l.transform.position) >= (sscript.orbitRadius - 7) && Vector3.Distance(s.transform.position, l.transform.position) <= (sscript.orbitRadius + 7) && Vector3.Distance (tangent, l.transform.position) <= 2f) {
					Learth_Movement.isTangent = true;
					
					
					//determine direction of orbit
					if (tangent.y < s.transform.position.y && l_movement.x < 0) { 
						clockwise = true;
					}
					else if (tangent.y > s.transform.position.y  && l_movement.x > 0) {
						clockwise = true;
					}		
					else if (tangent.x < s.transform.position.x && l_movement.y > 0) {
						clockwise = true;
					}
					else {
						clockwise = false;
					}
					
					//update last stars, last entrances, last velocity vectors, and last rotations to include this star
					for(int k=2; k>0;k--)
					{
						Learth_Movement.last_stars[k] = Learth_Movement.last_stars[k-1];
						Learth_Movement.last_stars_velocity[k] = Learth_Movement.last_stars_velocity[k-1];
						Learth_Movement.last_star_gos[k] = Learth_Movement.last_star_gos[k-1];
						Learth_Movement.last_star_rots[k] = Learth_Movement.last_star_rots[k-1];
					}
					Learth_Movement.last_stars[0] = l.transform.position;
					Learth_Movement.last_stars_velocity[0] = l_movement;
					Learth_Movement.last_star_gos[0] = s;
					Learth_Movement.last_star_rots[0] = clockwise;
					
					//add appropriate energy value depending on color of star
					if (sscript.c == Color.blue) {
						energy += 5;
					} else if (sscript.c == Color.white){
						energy += 4;
					} else if (sscript.c == Color.yellow) {
						energy += 3;
					} else if (sscript.c == orange) {
						energy += 2;
					} else if (sscript.c == Color.red) {
						energy += 1;
					}
					else {
						energy -= 1;
					}
					sscript.c = Color.gray;
					break;
				}
			}
		}
	
	}
	
	void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 30), "Energy: " + energy);
    }
		
}
	
