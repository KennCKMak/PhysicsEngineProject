using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    

	private float percentage;
	private GameObject HPImg; //the inner part of health bar
	private float HPImgWidth; //57, controls inner hp placement
	private RectTransform HPImgTransform;
	private float updateSpeed = 5.0f;

	// Use this for initialization
	void Start () {
		
		HPImg = transform.GetChild (1).gameObject;
		HPImgWidth = gameObject.GetComponent<RectTransform> ().rect.width * 0.95f;
		HPImgTransform = HPImg.GetComponent<RectTransform> ();


    }


	// Update is called once per frame
	void Update () {
			
	}




	public void UpdateHealthBar(float health, float maxHealth){
		percentage = health / maxHealth;
		if (percentage < 0.0f)
			percentage = 0.0f;
		if(!HPImgTransform)
			HPImgTransform = transform.GetChild (1).gameObject.GetComponent<RectTransform> ();


		float curScaleX = HPImgTransform.localScale.x;
		HPImgTransform.localScale = new Vector3(Mathf.Lerp (
			curScaleX, 0.95f * percentage, updateSpeed * Time.deltaTime),
			0.6f, 1.0f);

		float curPosX = HPImgTransform.localPosition.x;
		HPImgTransform.localPosition = new Vector3(Mathf.Lerp (
			curPosX, 
			(-HPImgWidth/2 * (1-percentage)), 
			updateSpeed * Time.deltaTime), 
			0, 0);



//		Vector3 newPos = new Vector3 (leftMost + -leftMost*percentage, 0.0f, 0.0f);
//		HPImg.localPosition = Vector3.Lerp (HPImg.localPosition, newPos, updateSpeed*Time.deltaTime);
		//HPImg.localPosition = newPos;
		if (!HPImg) 
			return;
		

		if (percentage > 0.50f) {
			HPImg.GetComponent<Image>().color = Color.Lerp (Color.green, Color.yellow, (maxHealth - health) / (maxHealth / 2));
		} else if (percentage <= 0.50f) {
			HPImg.GetComponent<Image>().color = Color.Lerp (Color.yellow, Color.red, (maxHealth / 2 - health) / (maxHealth / 2));
		}
	}

}
