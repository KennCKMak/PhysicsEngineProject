using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannonController : CannonController {


	private GameObject playerObj;
	// Use this for initialization
	void Start () {
		Initialize ();
		CalculateMaxRange ();
		target = null;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (oldCannonPower != cannonPower)
			CalculateMaxRange ();


		if (target) {
			Aim ();
			if (elapsedTime < fireRate)
				elapsedTime += Time.deltaTime;
			Fire ();
		}
		if (playerObj){
			playerObj.transform.position = cannonTransform.position;
			playerObj.transform.rotation = cannonTransform.rotation;
		}
	}

	void OnTriggerEnter(Collider other){

		if (other.transform.name == "Player" && other.gameObject.GetComponent<PlayerController> ()) {
			//Debug.Log ("hit player");
			if (!other.GetComponent<PlayerController> ().usingCannon){
				Debug.Log ("Got inc annon");
				target = GameObject.Find ("TargetPlatform").transform.FindChild ("Target");
				playerObj = other.gameObject;
				playerObj.GetComponent<PlayerController> ().DisableInput ();
				playerObj.GetComponent<PlayerController> ().DisableGFX ();
				playerObj.GetComponent<PlayerController> ().usingCannon = true;
				//playerObj.GetComponent<PlayerController> ().DisableCollision ();
			}

		}
	}



	protected override void Fire(){
		if (elapsedTime < fireRate)
			return;
		if (!ammo)
			return;
		if (!canShoot)
			return;
		

		GameObject projectile = Instantiate (ammo, cannonTransform.position, cannonTransform.rotation) as GameObject;
		projectile.GetComponent<Rigidbody> ().AddForce (cannonPower * projectile.transform.forward, ForceMode.VelocityChange);
		projectile.GetComponent<PlayerModelAmmo> ().LockPlayer (playerObj);

		Debug.Log ("FIRE");
		elapsedTime = 0.0f;
		playerObj = null;
		target = null;
	}

}
