using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gHandClass : MonoBehaviour {

	public GameObject hand;
	public GameObject description;
	public GameObject text1;
	public GameObject text2;
	private string level;

	public static string handState;
	// Use this for initialization
	void Start () {
		handState = "";
		level = SceneManager.GetActiveScene().name;
		if (initLevelMenuClass.levelDemands != 1 && gHintClass.hintState == "") {
			Time.timeScale = 0;
			if (transform.childCount > 2) transform.GetChild (2).gameObject.SetActive(true);

			if (level == "level1" || level == "level14" || level == "level16" || level == "level28" || level == "level37" || level == "level39")
				StartCoroutine (addHand (1, 0));
			else if (level == "level4")
				StartCoroutine (addHand (1, 2.3F));
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public IEnumerator addHand(int textNumber, float timeWait) {
		handState = "text" + textNumber;
		if (transform.childCount > 2) {
			yield return StartCoroutine (staticClass.waitForRealTime (1F));
			transform.GetChild (2).gameObject.SetActive (false);
		}
		if (level == "level51") {
			if (gYetiClass.yetiState == "")
				Time.timeScale = 1;
		} else Time.timeScale = 1;
		yield return StartCoroutine(staticClass.waitForRealTime(timeWait));

		description.SetActive (true);
		hand.SetActive (true);
		if (level == "level1" || level == "level16" || level == "level28"  || (level == "level37" && handState == "text2") || level == "level39") hand.GetComponent<Animator>().Play("hand click");
		else  hand.GetComponent<Animator>().Play("hand drag");
		description.GetComponent<Animator>().Play("menu open");

		if (textNumber == 2) {
			text1.SetActive (false);
			text2.SetActive (true);
		}

	}


	public void delHand(int textNumber, float timeWait) {

		hand.SetActive (false);
		description.GetComponent<Animator> ().Play ("menu exit");


		if (textNumber == 1 || textNumber == 2) 
			StartCoroutine (addHand (textNumber, timeWait));
		
		else if (textNumber == -1)
			gameObject.transform.position = new Vector3 (0, 0, -10000);
	}

}
