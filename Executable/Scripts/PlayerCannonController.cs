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


		if (target) {
			Aim ();
			if (active) {
				if (elapsedTime < fireRate)
					elapsedTime += Time.deltaTime;
				Fire ();
			}
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

				active = true;
				playerObj = other.gameObject;
				playerObj.GetComponent<PlayerController> ().DisableInput ();
				playerObj.GetComponent<PlayerController> ().DisableGFX ();
				playerObj.GetComponent<PlayerController> ().usingCannon = true;
				//playerObj.GetComponent<PlayerController> ().DisableCollision ();
			}

		}
	}

	protected override void Aim(){
		//Vector3 dir = target.transform.position - transform.position;
		//float distX = Mathf.Sqrt(dir.x*dir.x + dir.z*dir.z);
		if (!active) {
			angle = 15;
			Quaternion idleRotation = Quaternion.Euler (-angle, 0, 0);
			cannonTransform.rotation = Quaternion.Slerp (cannonTransform.rotation, idleRotation, rotateSpeed / 2 * Time.deltaTime);
			return;
		}
		if(active){
			CalculateFiringAngle ();
			Mathf.Clamp (angle, minAngle, maxAngle);

			Vector3 newRotation = Quaternion.LookRotation (target.transform.position - cannonTransform.position).eulerAngles;
			newRotation.x = -angle;

			cannonTransform.rotation = Quaternion.Slerp (cannonTransform.rotation, Quaternion.Euler (newRotation), rotateSpeed * Time.deltaTime);//predictedRotation;
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
		

		Vector3 lookRot = Quaternion.LookRotation (target.transform.position - cannonTransform.position).eulerAngles;
		lookRot.x = -angle;
		Quaternion projectileRotation = Quaternion.Euler (lookRot);
		GameObject projectile = Instantiate (ammo, cannonTransform.position, projectileRotation) as GameObject;
		projectile.GetComponent<Rigidbody> ().AddForce (cannonPower * projectile.transform.forward, ForceMode.VelocityChange);
		projectile.GetComponent<PlayerModelAmmo> ().LockPlayer (playerObj);
		projectile.GetComponent<PlayerModelAmmo> ().target = this.target;
		particle.Play ();


		//Debug.Log ("FIRE");
		elapsedTime = 0.0f;
		active = false;
		playerObj = null;
	}
	void OnDrawGizmos(){
		/*Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere (transform.position, maxRange);
		Gizmos.DrawWireSphere (transform.position, minRange);
	*/
	}
}
