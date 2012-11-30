using UnityEngine;
using System.Collections;

public class Alien_Exp_Sound : MonoBehaviour {
	public AudioSource alien_expl;
	public AudioSource coin_grab;
	public AudioSource leaving_star;
	
	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		alien_expl = sources[0];
		coin_grab  = sources[1];
		leaving_star = sources[2];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

