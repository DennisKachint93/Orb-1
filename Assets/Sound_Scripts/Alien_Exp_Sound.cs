using UnityEngine;
using System.Collections;

public class Alien_Exp_Sound : MonoBehaviour {
	public AudioSource alien_expl;
	
	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		alien_expl = sources[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

