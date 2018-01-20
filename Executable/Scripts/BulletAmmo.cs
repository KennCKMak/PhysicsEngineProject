using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAmmo : MonoBehaviour {
	public int damage;
    public bool active;
	// Use this for initialization
	void Start () {
        active = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter(Collision other){
        Invoke("Disable", 1.0f);
        if (other.transform.name == "Player" || other.transform.tag == "Player") {
			if (other.gameObject.GetComponent<PlayerController> () && active) {
				other.gameObject.GetComponent<PlayerController> ().TakeDamage (damage);
				Destroy (gameObject, 2.0f);
                active = false;
			}
		}
	}

    void Disable() {
        active = false;
    }
}
