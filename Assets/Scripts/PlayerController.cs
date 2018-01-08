using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;
	[Header("Horizontal Movement")]
	[SerializeField] private float currentSpeed = 0.0f;
	[SerializeField] private float maxSpeed = 0.0f;
	public float walkSpeed = 4.0f;
	public float runSpeed = 12.0f;

	public float acceleration = 15.0f;

	[Header("Vertical Movement")]
	public bool canJump;
	public float jumpStrength = 15.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		maxSpeed = walkSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		currentSpeed = rb.velocity.magnitude;
		canJump = isGrounded ();
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			maxSpeed = walkSpeed;

		}
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			maxSpeed = runSpeed;
		}



		if (Input.GetKey(KeyCode.W))
			rb.AddForce(transform.forward * acceleration);	
		if (Input.GetKey(KeyCode.S))
			rb.AddForce (transform.forward * -acceleration);
		
		if (Input.GetKey(KeyCode.A))
			rb.AddForce (transform.right * -acceleration);
		if (Input.GetKey(KeyCode.D))
			rb.AddForce (transform.right * acceleration);

		SpeedClamp ();

		if (Input.GetKeyDown (KeyCode.Space) && isGrounded())
			rb.AddForce (transform.up * jumpStrength);

	}

	void SpeedClamp(){

		//only checks in x and z axis
		Vector2 currVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
		if (currVelocity.magnitude > maxSpeed && isGrounded()) {
			currVelocity.Normalize ();
			rb.velocity = new Vector3 (currVelocity.x * maxSpeed, rb.velocity.y, currVelocity.y * maxSpeed);

		}
	}

	bool isGrounded(){
		return Physics.Raycast (transform.position, -Vector3.up, transform.localScale.y + 0.5f);
	}


}
