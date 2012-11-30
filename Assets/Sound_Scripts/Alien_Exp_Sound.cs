using UnityEngine;
using System.Collections;

public class Alien_Exp_Sound : MonoBehaviour {
	public AudioSource alien_expl;
	public AudioSource coin_grab;
	
	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		alien_expl = sources[0];
		coin_grab  = sources[1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

