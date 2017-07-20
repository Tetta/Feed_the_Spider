using UnityEngine;
using System.Collections;

public class gTerrainClass : MonoBehaviour {

	public GameObject ps;
	public AudioSource audioDrop;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D collisionObject) {
	    if (collisionObject.gameObject.name != "sluggish helper")
	    {
	        GameObject psNew =
	            GameObject.Instantiate(ps, collisionObject.contacts[0].point, Quaternion.identity) as GameObject;
	        audioDrop.Play();
	        Destroy(psNew, 1);
	    }
	}
}
