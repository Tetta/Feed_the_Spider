using UnityEngine;
using System.Collections;

public class gCactusClass : MonoBehaviour {

	private Quaternion collisionRotation;

	//private GameObject restart;
	// Use this for initialization
	void Start () {
		//restart = GameObject.Find("gui/restart");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerStay2D(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "spider" || collisionObject.gameObject.name == "berry") {
			collisionObject.transform.position = transform.position;
			collisionObject.transform.rotation = collisionRotation;
		}
	}

	void OnTriggerEnter2D(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "berry" || collisionObject.gameObject.name == "spider") {
			if (collisionObject.gameObject.name == "berry")
				transform.GetChild (7).GetChild (5).gameObject.SetActive (true);


			if (collisionObject.gameObject.name == "spider")
				transform.GetChild (6).transform.localScale = new Vector3 (1.7F, 1.7F, 0);

			transform.GetChild (6).gameObject.SetActive (true);

			transform.GetChild (6).GetComponent<Animator> ().Play ("cactus 2");
			if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider sad start") &&
				!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider sad end")) {

				StartCoroutine (gSpiderClass.coroutineCry (staticClass.currentSkinAnimator, collisionObject.gameObject.name));
				staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (3).GetComponent<AudioSource> ().Play ();

			}
			collisionObject.transform.position = transform.position;
			collisionRotation = collisionObject.transform.localRotation;
			transform.GetChild (6).rotation = collisionRotation;
			if (collisionObject.gameObject.name == "berry") {
				if (collisionObject.transform.parent.name == "sluggish physics")
					collisionObject.transform.parent = collisionObject.transform.parent.parent.parent;
				collisionObject.gameObject.GetComponent<Rigidbody2D> ().isKinematic = true;

			}
		}
		
	}

}
