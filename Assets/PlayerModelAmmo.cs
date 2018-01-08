﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelAmmo : MonoBehaviour {

	// Use this for initialization
	private bool locked;
	private Transform player;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(locked){
			player.transform.position = transform.position;
			GetComponent<Rigidbody> ().AddTorque (transform.right * 500.0f);
		}
	}
	void OnTriggerEnter(Collider other){
		if (other.transform.name == "TargetPlatform" || other.transform.tag == "TargetPlatform") {
			
			//player.parent = null;
			player.eulerAngles = new Vector3 (0, player.eulerAngles.y, 0);

			//player.transform.position = other.transform.position;
			player.GetComponent<PlayerController> ().EnableInput ();
			player.GetComponent<PlayerController> ().EnableGFX ();
			player.GetComponent<PlayerController> ().usingCannon = false;
			player.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody>().velocity;


			player = null;
			locked = false;
			Destroy (gameObject);
		}
	}

	public void LockPlayer(GameObject playerObj){
		player = playerObj.transform;
		//player.parent = this.transform;
		locked = true;
	}
}
