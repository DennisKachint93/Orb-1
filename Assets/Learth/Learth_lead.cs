using UnityEngine;
using System.Collections;

public class Learth_lead : MonoBehaviour {

	public GameObject coin_pickup_effect, cpe;
	
	void Start () {

	}
	
	void Update () {
		this.transform.position = Manager.l.transform.position + 25*Learth_Movement.velocity.normalized;
	}
	
	void OnCollisionEnter(Collision collision) {
		
		if(collision.gameObject.tag == "line") {
			Background.activated = false;
			print ("collision");
			Manager.energy += 3;
			cpe = Instantiate (coin_pickup_effect, collision.gameObject.transform.position, 
				collision.gameObject.transform.rotation) as GameObject;
		//	Destroy(collision.gameObject);
		}
	}
	
}
