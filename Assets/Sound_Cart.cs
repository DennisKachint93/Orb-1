using UnityEngine;
using System.Collections;

public class Sound_Cart : MonoBehaviour {
	public AudioSource coin_collected;
	public AudioSource alien_expl;

	// Use this for initialization
	void Start () {
		AudioSource[] sources = GetComponents<AudioSource>();
		alien_expl = sources[0];
		coin_collected = sources[1];
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
