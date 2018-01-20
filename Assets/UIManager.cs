using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject helpPanel;
    public bool helpEnabled;


    public PlayerController player;
    public HealthBar healthBar;

    public bool charEnabled;
    public GameObject charPanel;
    public Text charText;

    public GameObject endPanel;
    public Text textFinalMessage;

    public static UIManager instance;

    void Awake() {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        helpEnabled = true;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Alpha1)) {
            Cursor.visible = !Cursor.visible;
        }

        UpdateCharInfo();



        healthBar.UpdateHealthBar(player.currentHealth, player.maxHealth);
    }

    

    public void ToggleAdvice() {
        helpEnabled = !helpEnabled;
        helpPanel.SetActive(helpEnabled);
    }

    public void ToggleChar() {
        charEnabled = !charEnabled;
        charPanel.SetActive(charEnabled);
    }

    public void UpdateCharInfo() {
        charText.text =
            "CurrentSpeed = " + string.Format("{0:0.##}", player.currentSpeed) + "\n" +
            "MaxSpeed = " + player.maxSpeed + "\n" +
            "CanJump = " + player.canJump + "\n" +
            "Grounded = " + player.isGrounded() + "\n" +
            "OnIce = " + player.isOnIce() + "\n";
    }



    public void StartEndGame() {
        Invoke("EndGameWin", 4.0f);
        //ToggleAdvice();

    }

    public void EndGameWin() {
        textFinalMessage.text = "Victory!";
        endPanel.SetActive(true);
        Time.timeScale = 0.0f;

    }

    public void EndGame(string condition) {
        textFinalMessage.text = condition;
        endPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}


