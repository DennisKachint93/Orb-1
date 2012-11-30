using UnityEngine;
using System.Collections;

public class Alien_Exp_Sound : MonoBehaviour {
	public AudioSource alien_expl;
	public AudioSource coin_grab;
	public AudioSource leaving_star;
	public AudioSource entering_star;
	public AudioSource jump;
	public AudioSource star_explosion;
	
	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		alien_expl = sources[0];
		coin_grab  = sources[1];
		leaving_star = sources[2];
		entering_star = sources[3];
		jump = sources[4];
		star_explosion = sources[5];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

