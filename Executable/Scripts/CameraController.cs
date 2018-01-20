using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	public Transform camera;
	public float currDist;
	public float maxDist;
	private Vector3 startingPosition;

	public bool colliding;

	void Start () {
		maxDist = Vector3.Distance (transform.position, transform.parent.position);
		startingPosition = transform.localPosition;
		camera = transform.GetChild (0);
		camera.parent = null;// transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (colliding);
		currDist = Vector3.Distance (transform.position, transform.parent.position);


		if (!colliding) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, startingPosition, 25.0f);
			camera.transform.position = Vector3.Lerp (camera.transform.position, transform.position, 11.0f * Time.deltaTime);
			camera.transform.eulerAngles = Vector3.Lerp (camera.transform.eulerAngles, transform.eulerAngles, 11.0f * Time.deltaTime);
		} if (colliding) {
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, transform.parent.localPosition, 11.0f * Time.deltaTime);

		}
	}

	void OnTriggerEnter(Collider other){
		
		if (other.transform.tag == "Player")
			return;
		colliding = true;
	}
	void OnTriggerExit(Collider other){
		if (other.transform.tag == "Player")
			return;
		colliding = false;
	}



}
