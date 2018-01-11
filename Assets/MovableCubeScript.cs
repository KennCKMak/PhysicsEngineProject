using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCubeScript : MonoBehaviour {

	// Use this for initialization
	public Vector3 startPosition;
	private bool called;
	void Start () {
		startPosition = transform.position;
		called = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < startPosition.y-5.0f && !called) {
			StartCoroutine (ResetBox());
		}
	}

	IEnumerator ResetBox(){
		called = true;
		yield return new WaitForSeconds (2.0f);
		transform.position = startPosition;
		transform.rotation = Quaternion.identity;
		called = false;
	}
}
