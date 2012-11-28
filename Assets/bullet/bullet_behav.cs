using UnityEngine;
using System.Collections;

public class bullet_behav : MonoBehaviour {
        private float speed = 20;
        private Vector3 target;
        private bool shot = false;


        // Use this for initialization
        void Start () {

        }

        public void SetTarget(Vector3 t){
                target = t;
                shot = true;
        }

        // Update is called once per frame
        void Update () {
                if(shot){
                        Vector3 shotat = target - transform.position;
                        transform.Translate(shotat*speed*Time.deltaTime);
                        float x1 = transform.position.x;
                        float x2 = target.x;
                        float y1 = transform.position.y;
                        float y2 = target.y;
                        if((int)x1 == (int)x2 && (int)y1 == (int)y2){
                                Destroy(gameObject);
                        }
                }
        }
}
