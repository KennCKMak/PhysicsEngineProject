using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

	public Material normal;
	public Material falling;

	Vector3 startPosition;
	Rigidbody rb;
	Renderer render;
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
		GetComponent<Renderer> ().material = normal;
		rb = GetComponent<Rigidbody> ();
		render = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other){
		if (other.transform.name == "Player" && other.gameObject.GetComponent<PlayerController> ()) {
			render.material = falling;
			Invoke ("StartFalling", 0.5f);
		}
	}

	void StartFalling(){

		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		GetComponent<BoxCollider> ().enabled = false;

		Invoke ("Reset", 2.0f);
	}

	void Reset(){
		transform.position = startPosition;
		transform.rotation = Quaternion.identity;
		render.material = normal;
		StopFalling ();
	}

	void StopFalling(){
		rb.constraints = RigidbodyConstraints.FreezeAll;
		GetComponent<BoxCollider> ().enabled = true;

	}
}
