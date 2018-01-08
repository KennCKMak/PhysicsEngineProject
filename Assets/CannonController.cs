using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

	public float Force;
	public float mass;
	public float acceleration;

	public float angle;

	public Transform target;
	private Transform cannonTransform;
	// Use this for initialization
	void Start () {
		if (!target)
			target = GameObject.Find ("Player").transform;

		cannonTransform = transform.FindChild ("CannonGFX").FindChild ("CannonModel");
	}
	
	// Update is called once per frame
	void Update () {
		
		cannonTransform.LookAt (target);
		cannonTransform.localEulerAngles = new Vector3 (-15.0f, cannonTransform.localEulerAngles.y, cannonTransform.localEulerAngles.z);

	}
}
