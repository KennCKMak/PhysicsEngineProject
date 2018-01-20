using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int currentCheckpoint;
	public GameObject[] CheckpointsArray;

	public GameObject player;

    public static GameManager instance;

    void Awake() {

        instance = this;
        Time.timeScale = 1.0f;
    }

	// Use this for initialization
	void Start () {
		if (!player)
			player = GameObject.Find ("Player").gameObject;
		Initialize ();

	}

	
	// Update is called once per frame
	void Update () {
		if (!player)
			return;
		CheckPlayer ();
	}

	public void Initialize(){
		InitializeCheckpoints ();
	}

	public void InitializeCheckpoints(){
		currentCheckpoint = 0;
		CheckpointsArray [0].GetComponent<Checkpoint> ().RaiseFlag ();
		for (int i = 1; i < CheckpointsArray.Length; i++) {
			if (CheckpointsArray[i] == null)
				return;
			CheckpointsArray [i].GetComponent<Checkpoint> ().LowerFlag ();
			CheckpointsArray [i].GetComponent<Checkpoint> ().checkpointNum = i;
		}
		Debug.Log ("Initialized");
	}

	public void TriggeredCheckpoint(int num){
		if (currentCheckpoint < num) {
			currentCheckpoint = num;
		}
		if (CheckpointsArray [num].GetComponent<Checkpoint> ().finalCheckpoint) {
			//ENDGAME
			Debug.Log("Finished Game");
            //Invoke ("RestartGame", 5.0f);
            GameObject.Find("Canvas").GetComponent<UIManager>().StartEndGame();
		}
	}



	public void CheckPlayer(){
		//Check if player is dead
		if (player.transform.position.y < -10.0f) {
			ResetPlayer ();
		}
	}
	public void ResetPlayer(){
		//Move to checkpoint
		player.transform.position = CheckpointsArray [currentCheckpoint].GetComponent<Checkpoint>().GetRespawnLocation ();
		player.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
	}


}
