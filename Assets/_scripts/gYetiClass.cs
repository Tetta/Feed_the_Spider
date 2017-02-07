using UnityEngine;
using System.Collections;

public class gYetiClass : MonoBehaviour {

	public GameObject backYeti;
	public GameObject yetiSleep;
	public GameObject yetiZzz;
	public GameObject yetiBlow;

	private GameObject berry;
	private GameObject spider;
	private GameObject[] tumbleweeds;
	public static string yetiState = "";
	private GameObject[] chains;
	// Use this for initialization
	void Start () {
		berry = GameObject.Find("root/berry");
		spider = GameObject.Find("root/spider");
		tumbleweeds = GameObject.FindGameObjectsWithTag("tumbleweed");
		yetiState = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress(bool isPressed) {
		if (isPressed)	return;

		//если используется подсказка и объект не подходит, то не нажимается
		bool flagHintUse = true;
		if (gHintClass.hintState == "pause")
		if (gHintClass.actions [gHintClass.counter].id != transform.position)
			flagHintUse = false;
		if (gHintClass.hintState == "start") flagHintUse = false;
		//
		if (flagHintUse) {
			staticClass.useYeti = true;
			gRecHintClass.recHint (transform);
			gHintClass.checkHint (gameObject);
			if (yetiState == "") {
				//tutorial
				if (ctrProgressClass.progress["currentLevel"] == 28 && gHandClass.handState == "text1") 
				GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (2, 0F);

				yetiState = "active";
				berry.GetComponent<Rigidbody2D> ().angularVelocity = 0;
				berry.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
				spider.GetComponent<Rigidbody2D>().angularVelocity = 0;
				spider.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
				spider.GetComponent<Rigidbody2D> ().isKinematic = true;

				chains = GameObject.FindGameObjectsWithTag ("chain");
				for (int i = 0; i < chains.Length; i++) {
					chains [i].GetComponent<Rigidbody2D> ().isKinematic = true;
				}
				foreach (GameObject item in tumbleweeds) {
					item.GetComponent<Rigidbody2D> ().isKinematic = true;
				}
				Time.timeScale = 0;
				yetiBlow.SetActive (true);
				yetiZzz.SetActive (false);
				yetiSleep.SetActive (false);

				backYeti.SetActive (true);
			} else {
				//tutorial
				if (ctrProgressClass.progress["currentLevel"] == 28 && gHandClass.handState == "text2") 
					GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (-1, 0F);
				
				yetiState = "";
				chains = GameObject.FindGameObjectsWithTag ("chain");
				for (int i = 0; i < chains.Length; i++) {
					chains [i].GetComponent<Rigidbody2D> ().isKinematic = false;
				}
				foreach (GameObject item in tumbleweeds) {
					item.GetComponent<Rigidbody2D> ().isKinematic = false;
				}
				spider.GetComponent<Rigidbody2D> ().isKinematic = false;
				Time.timeScale = 1;
				yetiBlow.SetActive (false);
				yetiZzz.SetActive (true);
				yetiSleep.SetActive (true);
				backYeti.SetActive (false);
				//audio
				yetiSleep.transform.GetChild (0).GetComponent<AudioSource> ().Play ();

			}
		}
		
	}
}
