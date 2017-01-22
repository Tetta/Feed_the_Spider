using UnityEngine;
using System.Collections;

public class gCloudClass : MonoBehaviour {
	public AudioSource audioForward;
	public AudioSource audioBack;
	public AudioSource audioDrop;

	private string cloudState = "";
	// Use this for initialization
	void Start () {
		//transform.parent.GetComponent<Animator>().StopPlayback();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPress (bool isPressed) {
		//если используется подсказка и объект не подходит, то не нажимается
		bool flagHintUse = true;
		if (gHintClass.hintState == "pause")
		if (gHintClass.actions [gHintClass.counter].id != transform.position)
			flagHintUse = false;
		if (gHintClass.hintState == "start") flagHintUse = false;
		//

		if (!isPressed && flagHintUse) {
			if (cloudState == "") {
				//tutorial
				if (ctrProgressClass.progress["currentLevel"] == 16 && gHandClass.handState == "text1") 
					if (transform.parent.name  == "cloud 3") GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (2, 1F);

				gRecHintClass.recHint (transform);
				gHintClass.checkHint (gameObject);
				transform.parent.GetComponent<Animator>().Play("cloud disabled");
				//GetComponent<Collider2D>().isTrigger = true;
				cloudState = "disabled";
				audioBack.Play ();
			} else {
				//tutorial
				if (ctrProgressClass.progress["currentLevel"] == 16 && gHandClass.handState == "text2") 
				if (transform.parent.name  == "cloud 3") GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (-1, 0);
				
				gRecHintClass.recHint (transform);
				gHintClass.checkHint (gameObject);
				transform.parent.GetComponent<Animator>().Play("cloud enabled");
				//GetComponent<Collider2D>().isTrigger = false;
				cloudState = "";
				audioForward.Play ();

			}
		}
	}

	void OnCollisionEnter2D (Collision2D collisionObject) {
		transform.parent.GetComponent<Animator>().Play("cloud collision");
		audioDrop.Play ();

	}
}
