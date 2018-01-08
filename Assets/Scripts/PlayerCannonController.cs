using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannonController : CannonController {


	private GameObject playerObj;
	public bool active;
	public float dist;
	// Use this for initialization
	void Start () {
		Initialize ();
		CalculateMaxRange ();
		active = false;
		if(!target)
			target = GameObject.Find ("TargetPlatform").transform.GetChild (0);
		
		Vector3 dir = target.transform.position - transform.position;
		dist = Mathf.Sqrt(dir.x*dir.x + dir.z*dir.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (oldCannonPower != cannonPower)
			CalculateMaxRange ();


		if (target && active) {
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

				active = true;
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

		if (dist < minRange || dist > maxRange)
			return;
		


		GameObject projectile = Instantiate (ammo, cannonTransform.position, cannonTransform.rotation) as GameObject;
		projectile.GetComponent<Rigidbody> ().AddForce (cannonPower * projectile.transform.forward, ForceMode.VelocityChange);
		projectile.GetComponent<PlayerModelAmmo> ().LockPlayer (playerObj);
		particle.Play ();
		Debug.Log ("FIRE");
		elapsedTime = 0.0f;
		active = false;
		playerObj = null;
	}

}
