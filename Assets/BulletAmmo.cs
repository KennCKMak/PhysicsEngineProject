using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAmmo : MonoBehaviour {
	public int damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter(Collision other){
		if (other.transform.name == "Player" || other.transform.tag == "Player") {
			if (other.gameObject.GetComponent<PlayerController> ()) {
				other.gameObject.GetComponent<PlayerController> ().TakeDamage (damage);
				Destroy (gameObject);
			}
		}
	}
}
