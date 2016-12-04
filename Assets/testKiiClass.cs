using UnityEngine;
using System.Collections;
using System;
using KiiCorp.Cloud.Storage;

public class testKiiClass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string username = "user_123456";
		string password = "123ABC";
		username = "Tetta";
		password = "test";

		KiiUser.LogIn(username, password, (KiiUser user, Exception e) => {
			if (e != null)
			{
				Debug.Log(e.ToString());
				// handle error
				return;
			}
		});

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
