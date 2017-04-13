using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gBonusClass : MonoBehaviour {

	public string bonusState = "";
	public GameObject prefab;

	private GameObject tempGo1;
	private GameObject tempGo2;

	// Use this for initialization
	void Start () {
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();


	}

    void OnEnable()
    {
        transform.GetChild(0).GetComponent<UILabel>().text = ctrProgressClass.progress[name].ToString();
    }
	
	// Update is called once per frame
	void Update () {
		if (bonusState == "webs" || bonusState == "teleports") {
			GetComponent<UIWidget>().autoResizeBoxCollider = false;
			GetComponent<BoxCollider>().size = new Vector2(4000, 4000);

			//ветерок на пауке
			if (bonusState == "teleports") {
				tempGo1 = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity) as GameObject;
				tempGo1.transform.parent = GameObject.Find("root/spider/" + staticClass.currentSkin).transform;
				tempGo1.transform.position = GameObject.Find("spider").transform.position; 
				tempGo1.transform.localScale = new Vector3(1, 1, 1);
				//tempGo1.GetComponent<Animator>().Play ("teleport enabled");
				transform.GetChild(1).GetComponent<AudioSource> ().Play ();

			}
			bonusState = name + " wait click";
		}
		if (bonusState == "teleports wait click") {
			tempGo1.transform.rotation = Quaternion.Euler(0, 0, -transform.parent.rotation.z);

		}
        if (bonusState == "collectors") bonusState = name + " wait click";

    }
	

	void OnPress(bool flag) {
		if (!flag && bonusState == "") {
			if (ctrProgressClass.progress [name] > 0) {
                if (ctrProgressClass.progress["tutorialBonus"] != 0) staticClass.isTimePlay = Time.timeScale;
                Time.timeScale = 0;
                Debug.Log("Time.timeScale: " + Time.timeScale);

                //off tutorial bonus
                if (ctrProgressClass.progress["tutorialBonus"] == 0) {
                    if (GameObject.Find("/default level/gui/tutorial bonus(Clone)") != null)
                        GameObject.Find("/default level/gui/tutorial bonus(Clone)").SetActive(false);
                    ctrProgressClass.progress["tutorialBonus"] = 1;
                    ctrAnalyticsClass.sendEvent("Tutorial", new Dictionary<string, string> { { "name", "use bonus" } });

                }

                GetComponent<AudioSource> ().Play ();
				ctrProgressClass.progress [name]--;
				transform.GetChild (0).GetComponent<UILabel> ().text = ctrProgressClass.progress [name].ToString ();
				ctrProgressClass.saveProgress ();

                //for analytics
                var type = (initLevelMenuClass.levelDemands == 0) ? "normal" : "challenge";
                ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                {
                    { "detail", "level " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Substring(5)},
                    { "type", type},
                    { "name", name},
                    { "count", "1"}
                });


                //показываем картинку в середине
                GameObject.Find ("bonuses pictures").transform.GetChild (0).gameObject.SetActive (true);
				if (name == "webs")
					GameObject.Find ("bonuses pictures").transform.GetChild (2).gameObject.SetActive (true);
				if (name == "teleports")
					GameObject.Find ("bonuses pictures").transform.GetChild (3).gameObject.SetActive (true);
				if (name == "collectors")
					GameObject.Find ("bonuses pictures").transform.GetChild (4).gameObject.SetActive (true);

				StartCoroutine (coroutineBonusPictureEnable ());
			}
		}
	    if (!flag && (bonusState == "webs wait click" || bonusState == "teleports wait click"))
	    {
            GameObject.Find("bonuses pictures").transform.GetChild(6).gameObject.SetActive(false);
            GameObject.Find("bonuses pictures").transform.GetChild(7).gameObject.SetActive(false);
            GameObject.Find("bonuses pictures").transform.GetChild(8).gameObject.SetActive(false);
	        if (bonusState == "webs wait click")
	        {
	            Time.timeScale = staticClass.isTimePlay;
                Debug.Log("Time.timeScale: " + Time.timeScale);

            }


        }

        if (!flag && (bonusState == "webs wait click" || bonusState == "teleports wait click")) {
			GetComponent<UIWidget>().autoResizeBoxCollider = true;
			GetComponent<BoxCollider>().size = new Vector2(170, 178);
			transform.GetChild(1).GetComponent<AudioSource> ().Play ();

			if (bonusState == "teleports wait click") {
				StartCoroutine(coroutineTeleportDisable());
			//} else if (bonusState == "collectors wait click") {
			//		StartCoroutine(coroutineCollectorDisable());
			} else {
                Debug.Log("tempGo2");
                tempGo2 = Instantiate(prefab, new Vector2(0, 0), Quaternion.identity) as GameObject;
				tempGo2.transform.parent = GameObject.Find("root").transform;
				tempGo2.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                tempGo2.transform.localScale = new Vector3(1, 1, 1);
                

            }

            bonusState = "";
		}


	}
	
	
	IEnumerator coroutineTeleportDisable() {
        //yield return new WaitForSeconds(0.2F);
        Animator spiderAnimator = GameObject.Find ("root/spider/" + staticClass.currentSkin).GetComponent<Animator>();
        spiderAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        //yield return new WaitForSeconds(0.3F);
        tempGo1.GetComponent<Animator>().Play ("teleport disabled");
		spiderAnimator.Play ("spider disabled");
		yield return StartCoroutine(staticClass.waitForRealTime(0.3F));
		GameObject.Find ("root/spider").transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		GameObject.Find ("root/spider").GetComponent<Rigidbody2D> ().isKinematic = true;
        //tempGo1.GetComponent<Animator>().Play ("teleport enabled");
		//yield return StartCoroutine(waitForRealTime(0.2F));
		if (Time.timeScale == 1)
			spiderAnimator.Play ("spider enabled");
		else GameObject.Find ("root/spider/" + staticClass.currentSkin).transform.localScale = new Vector3 (1, 1, 1);
		yield return StartCoroutine(staticClass.waitForRealTime(0.3F));

		Destroy(tempGo1);
		GameObject.Find ("root/spider").GetComponent<Rigidbody2D> ().isKinematic = false;
        
        spiderAnimator.updateMode = AnimatorUpdateMode.Normal;
        Time.timeScale = staticClass.isTimePlay;
        Debug.Log("Time.timeScale: " + Time.timeScale);


    }

    IEnumerator coroutineBonusPictureEnable() {


        yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
		//yield return new WaitForSeconds(0.5F);

		if (name == "webs") GameObject.Find("bonuses pictures").transform.GetChild(2).gameObject.GetComponent<Animator>().Play("menu exit");
		if (name == "teleports") GameObject.Find("bonuses pictures").transform.GetChild(3).gameObject.GetComponent<Animator>().Play("menu exit");
		if (name == "collectors") GameObject.Find("bonuses pictures").transform.GetChild(4).gameObject.GetComponent<Animator>().Play("menu exit");
		GameObject.Find("bonuses pictures").transform.GetChild(0).gameObject.SetActive(false);

		yield return StartCoroutine(staticClass.waitForRealTime(0.3F));
	    if (name == "webs")
	    {
	        GameObject.Find("bonuses pictures").transform.GetChild(2).gameObject.SetActive(false);
            GameObject.Find("bonuses pictures").transform.GetChild(6).gameObject.SetActive(true);
        }
        if (name == "teleports")
	    {
	        GameObject.Find("bonuses pictures").transform.GetChild(3).gameObject.SetActive(false);
            GameObject.Find("bonuses pictures").transform.GetChild(7).gameObject.SetActive(true);
        }
        if (name == "collectors")
	    {
	        GameObject.Find("bonuses pictures").transform.GetChild(4).gameObject.SetActive(false);
            GameObject.Find("bonuses pictures").transform.GetChild(8).gameObject.SetActive(true);
        }
        //yield return new WaitForSeconds(0.3F);
        bonusState = name;
	}


}
