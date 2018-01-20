using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
	
	private GameObject flag;
	private RectTransform flagTransform;
	private Cloth cloth;
	private ParticleSystem particle;

	public bool activated;
	public int checkpointNum;
	public bool finalCheckpoint = false;

	private float currentPos;
	private float topPos = 4.20f ;
	private float botPos = 2.01f;
	public bool raise = true;
	public float flagSpeed = 3.0f;
	public float windStrength = 100.0f;

	void Awake () {
		flag = transform.GetChild (1).gameObject;
		flagTransform = flag.GetComponent<RectTransform> ();
		cloth = flag.GetComponent<Cloth> ();
		particle = transform.GetChild (3).gameObject.GetComponent<ParticleSystem> ();

		activated = false;
	}
	
	// Update is called once per frame
	void Update () {
		currentPos = flagTransform.localPosition.y;
		if (raise) {
			if (Mathf.Abs(topPos - currentPos) > 0.05f) 
				MoveFlagTo (topPos);
			
		} else {
			if (Mathf.Abs(botPos - currentPos) > 0.05f) 
				MoveFlagTo (botPos);
			
		}
		if (Input.GetKeyDown (KeyCode.F))
			RaiseFlag ();
		if (Input.GetKeyDown (KeyCode.G))
			LowerFlag ();
	}

	public void RaiseFlag(){
		raise = activated = true;
		cloth.externalAcceleration = new Vector3 (0, 0, -windStrength);
		cloth.randomAcceleration = new Vector3 (5, 0, 15);
		cloth.useGravity = false;
		cloth.stretchingStiffness = 1.0f;
		cloth.bendingStiffness = 0.1f;

		particle.Play ();
	}

	public void LowerFlag(){
		raise = activated = false;
		cloth.externalAcceleration = new Vector3 (0, 0, 0);
		cloth.randomAcceleration = new Vector3 (5, 0, 0);
		cloth.useGravity = true;
		cloth.stretchingStiffness = 0.01f;
		cloth.bendingStiffness = 0.01f;
	}

	void MoveFlagTo(float destY){
		Vector3 destPos = new Vector3 (flagTransform.localPosition.x, destY, flagTransform.localPosition.z);
		flagTransform.localPosition = Vector3.MoveTowards (flagTransform.localPosition, destPos, flagSpeed * Time.deltaTime);


	}

	void OnTriggerEnter(Collider col){
		if (col.transform.tag == "Player") {
			GameManager.FindObjectOfType<GameManager> ().TriggeredCheckpoint (this.checkpointNum);
			if(!activated)
				RaiseFlag ();

		}
	}

	public Vector3 GetRespawnLocation(){
		Vector3 newLoc = transform.position;
		newLoc.y += 6.0f;
		return newLoc;
	}
}
