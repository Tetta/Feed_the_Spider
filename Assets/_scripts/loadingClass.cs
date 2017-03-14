using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class loadingClass : MonoBehaviour {
	

	IEnumerator Start() {
	//void Start() {
		Debug.Log("start: " + Time.realtimeSinceStartup);
		Application.backgroundLoadingPriority = ThreadPriority.Low;
		AsyncOperation async = SceneManager.LoadSceneAsync("menu");

		//Text label = GetComponent<Text>();
		while( !async.isDone ){
			//Debug.Log(string.Format( "Loading {0}%", async.progress*100 ));
			//Debug.Log(Time.realtimeSinceStartup);
			//label.text = string.Format( "Loading {0}%", async.progress*100 ) ;
			yield return null;
		}
		//label.text = "Loading complete";

		yield return async;
	}

	void Update () {
		if (Time.frameCount == 1) Debug.Log("update 1: " + Time.realtimeSinceStartup);
	
	}

}
