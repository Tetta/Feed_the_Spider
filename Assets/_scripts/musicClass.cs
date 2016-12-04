using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class musicClass : MonoBehaviour {
	public static musicClass instance = null;
	public AudioClip clipMenu;
	public AudioClip clipGameplay;
	public AudioClip clipGameplay2;

	// Use this for initialization





	void Start () {

		if(instance!=null){
			// level -> level menu
			//D/ebug.Log ("staticClass.scenePrev: " + staticClass.scenePrev);
			if (staticClass.scenePrev != "menu" && staticClass.scenePrev != "level menu" && SceneManager.GetActiveScene ().name == "level menu") {
				instance.GetComponent<AudioSource> ().clip = clipMenu;	
				instance.GetComponent<AudioSource> ().Play ();
			}

			// level menu -> level
			if (staticClass.scenePrev == "level menu" && SceneManager.GetActiveScene ().name != "menu" && SceneManager.GetActiveScene ().name != "level menu") {
				if (SceneManager.GetActiveScene ().name == "level1") instance.GetComponent<AudioSource> ().clip = clipGameplay;	
				else if (UnityEngine.Random.Range(0, 2) == 0) instance.GetComponent<AudioSource> ().clip = clipGameplay;	
				else instance.GetComponent<AudioSource> ().clip = clipGameplay2;	
				instance.GetComponent<AudioSource> ().Play ();
			}

			Destroy(gameObject);
			return;
		}
		instance = this;
		instance.GetComponent<AudioSource> ().ignoreListenerPause = true;
		DontDestroyOnLoad (gameObject);

		//GetComponent<AudioSource> ().time = staticClass.musicTime;
		//GetComponent<AudioSource> ().Play ();
		//if (SceneManager.GetActiveScene ().name == "menu")
			//staticClass.musicTime = GameObject.Find ("/music").GetComponent<AudioSource> ().time;
		
	}


}



