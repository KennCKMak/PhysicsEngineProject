using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelAmmo : MonoBehaviour {

	// Use this for initialization
	private bool locked;
	private Transform player;
	public GameObject smoke;
	public GameObject burst;
	void Start () {
		Invoke ("Reset", 10.0f);
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
			//player.eulerAngles = new Vector3 (0, player.eulerAngles.y, 0);
			//player.transform.position = other.transform.position;
			player.GetComponent<PlayerController> ().EnableInput ();
			player.GetComponent<PlayerController> ().EnableGFX ();
			player.GetComponent<PlayerController> ().usingCannon = false;
			player.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody>().velocity;


			smoke.transform.parent = null;
			smoke.transform.position = player.transform.position;
			smoke.GetComponent<ParticleSystem> ().main.loop.Equals (false);
			Destroy (smoke, 0.5f);
			burst.transform.parent = null;
			burst.transform.position = player.transform.position;
			burst.GetComponent<ParticleSystem> ().Play ();
			Destroy (burst, 0.5f);


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

	public void Reset(){
		//something went wrong; we missed our target, or the target moved out of the way

		//Here we could place our guy at a checkpoint.
		player.position = new Vector3(0, 5, 0);
		player.eulerAngles = new Vector3 (0, player.eulerAngles.y, 0);

		//player.transform.position = other.transform.position;
		player.GetComponent<PlayerController> ().EnableInput ();
		player.GetComponent<PlayerController> ().EnableGFX ();
		player.GetComponent<PlayerController> ().usingCannon = false;
		player.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody>().velocity;


		player = null;
		locked = false;
		Destroy (gameObject);

		Destroy (gameObject);
	}
}
