using UnityEngine;
using System.Collections;


public class gStarClass : MonoBehaviour {
	public AudioSource audioCollect;

	private GameObject psStarDestroy;
	// Use this for initialization
	void Start () {
		StartCoroutine (startStar());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		GameObject collectors = GameObject.Find ("bonuses/tween/collectors");
		if (collectors.GetComponent<gBonusClass> ().bonusState == "collectors wait click") {
            GameObject.Find("bonuses pictures").transform.GetChild(8).gameObject.SetActive(false);
            Time.timeScale = staticClass.isTimePlay;
            collectors.transform.GetChild(1).GetComponent<AudioSource> ().Play ();

			GameObject tempGo = Instantiate(collectors.transform.GetChild(2).gameObject, new Vector2(0, 0), Quaternion.identity) as GameObject;
			tempGo.transform.parent = GameObject.Find("root").transform;
			tempGo.transform.localScale = new Vector3(2, 2, 1);
			//tempGo.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(-0.2F, -0.2F, 0); 
			tempGo.transform.position = transform.position + new Vector3(-0.37F, -0.15F, 0); 

			StartCoroutine(coroutineCollectStar(collectors, tempGo));
			
			
		}
	}
	private IEnumerator coroutineCollectStar(GameObject collectors, GameObject tempGo){
		yield return StartCoroutine(staticClass.waitForRealTime(0.4F));
		GameObject[] guiStars = new GameObject[3];
		guiStars[0] = GameObject.Find("gui stars").transform.GetChild(0).gameObject;
		guiStars[1] = GameObject.Find("gui stars").transform.GetChild(1).gameObject;
		guiStars[2] = GameObject.Find("gui stars").transform.GetChild(2).gameObject;	
		guiStars [gBerryClass.starsCounter].SetActive (true);
		gBerryClass.starsCounter ++;
		Destroy(tempGo);
		StartCoroutine (destroyStar());
		collectors.GetComponent<gBonusClass> ().bonusState = "";
		
	}

	private IEnumerator startStar(){
		yield return StartCoroutine(staticClass.waitForRealTime(Random.value * 2));
		GetComponent<Animation> ().Play ("star 4");

		
	}

	public IEnumerator destroyStar(ctrScreenshotClass buttonEveryplayScript = null){
		audioCollect.Play ();
		GetComponent<Collider2D> ().enabled = false;
		transform.GetChild(0).GetComponent<ParticleSystem> ().Play ();
		transform.GetChild(1).position = new Vector3 (0, 0, -10000);
		//GetComponent<Animation> ().Play ("star destroy");
		yield return StartCoroutine(staticClass.waitForRealTime(0.1F));

		//Everyplay
		if (buttonEveryplayScript != null) {
			if (Everyplay.IsRecording ())
				buttonEveryplayScript.takeScreenshot ();
		}

		yield return StartCoroutine(staticClass.waitForRealTime(1F));
		Destroy (gameObject);
	}
}
