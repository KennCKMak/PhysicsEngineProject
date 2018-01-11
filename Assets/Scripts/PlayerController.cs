using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;
	[Header("Stats")]
	public int currentHealth;
	public int maxHealth = 100;


	[Header("Movement")]
	public bool inputEnabled = true;
	[SerializeField] private float currentSpeed = 0.0f;
	[SerializeField] private float maxSpeed = 0.0f;
	public float walkSpeed = 4.0f;
	public float runSpeed = 12.0f;

	public float acceleration = 15.0f;

	public bool canJump;
	public float jumpStrength = 15.0f;

	public bool usingCannon;


	int layerMask = 1 << 8;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		maxSpeed = walkSpeed;
		SetHealth (maxHealth);
		layerMask = ~layerMask;
	}
	
	// Update is called once per frame
	void Update () {
		currentSpeed = rb.velocity.magnitude;
		if(!canJump)
			canJump = isGrounded ();


		if (Input.GetKeyDown (KeyCode.Space) && canJump && inputEnabled) {
			canJump = false;
			rb.AddForce (transform.up * jumpStrength);
		}
		SpeedClamp ();

		if (inputEnabled) {
			if (Input.GetKeyUp (KeyCode.LeftShift) && !isOnIce()) 
				maxSpeed = walkSpeed;
			if (Input.GetKeyDown (KeyCode.LeftShift)) 
				maxSpeed = runSpeed;
			



			if (Input.GetKey (KeyCode.W))
				rb.AddForce (transform.forward * acceleration, ForceMode.VelocityChange);	
			if (Input.GetKey (KeyCode.S))
				rb.AddForce (transform.forward * -acceleration, ForceMode.VelocityChange);
		
			if (Input.GetKey (KeyCode.A))
				rb.AddForce (transform.right * -acceleration, ForceMode.VelocityChange);
			if (Input.GetKey (KeyCode.D))
				rb.AddForce (transform.right * acceleration, ForceMode.VelocityChange);

			if(Input.GetKey(KeyCode.Q))
				rb.AddTorque (Vector3.up * -acceleration, ForceMode.VelocityChange);

			if(Input.GetKey(KeyCode.E))
				rb.AddTorque (Vector3.up * acceleration, ForceMode.VelocityChange);

			rb.AddTorque (Vector3.up * Input.GetAxis ("Mouse X") * acceleration, ForceMode.VelocityChange);
		}
		if (!Input.GetKey (KeyCode.LeftShift) && maxSpeed == runSpeed && !isOnIce ())
			maxSpeed = walkSpeed;
		
		//slows down rotation 
		if (transform.eulerAngles.x != 0 || transform.eulerAngles.z != 0) {
			Vector3 newRot = new Vector3 (0, transform.eulerAngles.y, 0);
			transform.eulerAngles = Vector3.Lerp (transform.eulerAngles, newRot, 10.0f);

		}
	}



	void SpeedClamp(){

		//stops rotation
		rb.angularVelocity = rb.angularVelocity*0.95f;




		//only checks in x and z axis
		Vector2 currVelocity = new Vector2(rb.velocity.x, rb.velocity.z);
		if (currVelocity.magnitude > maxSpeed) {
			currVelocity.Normalize ();
			rb.velocity = new Vector3 (currVelocity.x * maxSpeed, rb.velocity.y, currVelocity.y * maxSpeed);

		}

	}

	bool isGrounded(){
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, transform.localScale.y + 0.01f, layerMask)){
			if(hit.transform.tag != "Player")
				return true;
			else
				return false;
		}
		return false;
	}
	

	bool isOnIce(){
		RaycastHit hit;
		if (Physics.Raycast (transform.position, -Vector3.up, out hit, transform.localScale.y + 0.01f, layerMask)) {
			if (hit.transform.tag == "Ice")
				return true;
			else
				return false;
		}
		return false;

		//return Physics.Raycast (transform.position, -Vector3.up, transform.localScale.y + 0.5f);
	}

	public void DisableInput(){
		inputEnabled = false;
	}
	public void EnableInput(){

		inputEnabled = true;
	
	}

	public bool isInputEnabled(){
		return inputEnabled;
	}

	public void DisableCollision(){
		GetComponent<CapsuleCollider> ().enabled = false;
		GetComponent<Rigidbody> ().Sleep ();
	}
	public void EnableCollision(){

		GetComponent<CapsuleCollider> ().enabled = true;
		GetComponent<Rigidbody> ().WakeUp ();
	}

	public void EnableGFX(){
		transform.GetChild (0).gameObject.SetActive (true);
	}

	public void DisableGFX(){
		transform.GetChild (0).gameObject.SetActive (false);
	}

	public void TakeDamage(int num){
		SetHealth (currentHealth - num);
	}

	public void SetHealth(int num){
		currentHealth = num;
		CheckHealth ();
	}

	public void CheckHealth(){
		if (currentHealth <= 0) {
			Debug.Log ("Dead");
		}
	}
}
