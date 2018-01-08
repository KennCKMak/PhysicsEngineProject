using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

	//velocity of first shot
	public float cannonPower;
	protected float oldCannonPower;
	public float maxRange;
	public float minRange;

	public float maxAngle = 45.0f;
	public float minAngle = 10.0f;
	public float angle;
	public float rotateSpeed = 5.0f;

	public Transform target;
	public GameObject ammo;
	protected float ammoMass = 1.0f;

	public bool canShoot;
	public float elapsedTime = 0.0f;
	public float fireRate = 5.0f;

	protected ParticleSystem particle;
	protected Transform cannonTransform;
	// Use this for initialization
	void Start () {
		Initialize ();
		CalculateMaxRange ();
	}
	
	// Update is called once per frame
	void Update () {
		if (oldCannonPower != cannonPower)
			CalculateMaxRange ();
		
		if (elapsedTime < fireRate)
			elapsedTime += Time.deltaTime;
		
		if (target){
			Aim ();
			if(ammo)
				Fire ();
		}

	}

	protected virtual void Initialize(){
		if (!target)
			target = GameObject.Find ("Player").transform;
		if (!ammo)
			ammoMass = 1.0f;
		else
			ammoMass = ammo.GetComponent<Rigidbody> ().mass;


		angle = minAngle;
		cannonTransform = transform.GetChild(0).FindChild ("CannonModel");
		particle = cannonTransform.GetChild (3).GetComponent<ParticleSystem> ();
		canShoot = true;
		elapsedTime = 0;
	}

	protected void CalculateMaxRange(){
		//45 degrees is the maximum range

		float maxVelX = cannonPower * Mathf.Cos (maxAngle * Mathf.Deg2Rad);
		float maxVelY = cannonPower * Mathf.Sin (maxAngle * Mathf.Deg2Rad);
		//vf = vi + a*t, vf = -vi...
		float time = Mathf.Abs((-maxVelY - maxVelY)/Physics.gravity.y);
		maxRange = maxVelX * time;

		float minVelX = cannonPower * Mathf.Cos (minAngle * Mathf.Deg2Rad);
		float minVelY = cannonPower * Mathf.Sin (minAngle * Mathf.Deg2Rad);
		time = Mathf.Abs((-minVelY - minVelY)/Physics.gravity.y);
		minRange = minVelX * time;


		oldCannonPower = cannonPower;
	}

	protected virtual void Aim(){
		Vector3 dir = target.transform.position - transform.position;
		float distX = Mathf.Sqrt(dir.x*dir.x + dir.z*dir.z);
		if (distX < minRange || distX > maxRange) {
			angle = 15;
			Quaternion idleRotation = Quaternion.Euler (-angle, 0, 0);
			cannonTransform.rotation = Quaternion.Slerp (cannonTransform.rotation, idleRotation, rotateSpeed / 2 * Time.deltaTime);
		} else {

			CalculateFiringAngle ();
			Mathf.Clamp (angle, minAngle, maxAngle);

			Vector3 newRotation = Quaternion.LookRotation (target.transform.position - cannonTransform.position).eulerAngles;

			cannonTransform.localEulerAngles = Vector3.Slerp (cannonTransform.rotation.eulerAngles, newRotation, rotateSpeed * Time.deltaTime);
			cannonTransform.localEulerAngles = new Vector3 (-angle, cannonTransform.localEulerAngles.y, cannonTransform.localEulerAngles.z);
		}
	}

	protected virtual void CalculateFiringAngle(){
		//https://www.youtube.com/watch?v=krzC92hZ8pA for projectile motion 


		Vector3 dir = target.transform.position - transform.position;
		float distX = Mathf.Sqrt(dir.x*dir.x + dir.z*dir.z);
		float distY = dir.y;

		if (distX < minRange || distX > maxRange) {
			angle = 15;
			canShoot = false;
			return;
		}


		//quadratic formula
		float a = (-Physics.gravity.y/2)*((distX/cannonPower)*(distX/cannonPower));
		float b = -distX;
		float c = distY + ((-Physics.gravity.y / 2) * ((distX / cannonPower)*(distX/cannonPower)));
		//tan theta = -b +- sqrt(b^2 - 4ac) all over 2a
		float angle1 = Mathf.Atan((-b + Mathf.Sqrt (b * b - 4 * a * c)) / (2 * a)) * Mathf.Rad2Deg;
		float angle2 = Mathf.Atan((-b - Mathf.Sqrt (b * b - 4 * a * c)) / (2 * a)) * Mathf.Rad2Deg;

		//Debug.Log ("Dist: " + distX + ". Angle1 = " + angle1 + ". Angle2 = " + angle2);

		if (angle1 > maxAngle || angle1 < minAngle)
			angle1 = Mathf.Infinity;
		if (angle2 > maxAngle || angle2 < minAngle)
			angle2 = Mathf.Infinity;

		if (angle1 != Mathf.Infinity && !float.IsNaN(angle1)) {
			angle = angle1;
			canShoot = true;
			return;
		}
		if (angle2 != Mathf.Infinity && !float.IsNaN(angle2)) {
			angle = angle2;
			canShoot = true;
			return;
		}
		canShoot = false;
	}

	protected virtual void Fire(){
		if (elapsedTime < fireRate)
			return;
		if (!ammo)
			return;
		if (!canShoot)
			return;

		Vector3 dir = target.transform.position - transform.position;
		float distX = Mathf.Sqrt(dir.x*dir.x + dir.z*dir.z);
		if (distX < minRange || distX > maxRange) {
			return;
		}

		GameObject projectile = Instantiate (ammo, cannonTransform.position, cannonTransform.rotation) as GameObject;
		projectile.GetComponent<Rigidbody> ().AddForce (cannonPower * projectile.transform.forward, ForceMode.VelocityChange);
		particle.Play ();
		Destroy (projectile, 7.0f);
		elapsedTime = 0.0f;
	}


}
