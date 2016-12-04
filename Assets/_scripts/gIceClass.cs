using UnityEngine;
using System.Collections;

public class gIceClass : MonoBehaviour {
	//private GameObject ice1;
	//public GameObject[] ice;


	// Use this for initialization
	void Start () {
		//ice1 = gameObject.transform.GetChild(0).gameObject;		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D collisionObject) {
		GetComponent<Animation>().Play();
	}
	/*
	IEnumerator breakIce(bool flag) {
		yield return new WaitForSeconds(0.3F);
		if (!flag)
			ice[0].SetActive (false);
		else {
			for (int i = 1; i <= 4; i++) {
				ice[i].GetComponent<Rigidbody2D>().isKinematic = false;
			}
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Animation>().Play();

			yield return new WaitForSeconds(1F);
			gameObject.SetActive (false);
		}
	}
	*/
}
