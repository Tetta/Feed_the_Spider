using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class gBerryClass : MonoBehaviour {

    public GameObject tutorialHint;
    public GameObject tutorialBonus;
    public GameObject tutorialDream;

    public static string berryState; 
	public static int starsCounter;
	public static int fixedCounter = 0;

	private GameObject completeMenu;
	private UILabel guiTimer;
	private GameObject pauseMenu;
	private GameObject[] guiStars = new GameObject[3];

	private GameObject back;
	private Vector3 dir = new Vector3(0, 0, 0);
	private float t = 0;

	private ctrScreenshotClass buttonEveryplayScript;

	// Use this for initialization
	void Start () {
        staticClass.sceneLoading = false;
		staticClass.initLevels ();

		//notifer
		/* OFF FOR TESTS
		List<LocalNotificationTemplate> PendingNotifications;
		PendingNotifications = AndroidNotificationManager.instance.LoadPendingNotifications();
		foreach (var PendingNotification in PendingNotifications) {
			if (PendingNotification.title == Localization.Get("notiferTitleEnergy")) {
				AndroidNotificationManager.instance.CancelLocalNotification(PendingNotification.id);
			}
		}
		*/
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();

        //for analytics
        if (gHintClass.hintState != "enable dream picture") ctrProgressClass.progress["levelPlayCount"]++;

        //запись текущего уровня
        if (ctrProgressClass.progress["currentLevel"] != int.Parse(SceneManager.GetActiveScene().name.Substring(5))) {
			ctrProgressClass.progress["currentLevel"] = int.Parse(SceneManager.GetActiveScene().name.Substring(5));
			ctrProgressClass.saveProgress ();
		}

		staticClass.useWeb = 0;
		staticClass.timer = 0;
		staticClass.useSluggish = false;
		staticClass.useDestroyer = false;
		staticClass.useYeti = false;
		staticClass.useGroot = false;

		starsCounter = 0;
		berryState = "";
		GameObject gui = GameObject.Find("gui");
		completeMenu = gui.transform.Find("complete menu").gameObject;
		pauseMenu = gui.transform.Find("pause menu").gameObject;
		guiStars[0] = GameObject.Find("gui stars").transform.GetChild(0).gameObject;
		guiStars[1] = GameObject.Find("gui stars").transform.GetChild(1).gameObject;
		guiStars[2] = GameObject.Find("gui stars").transform.GetChild(2).gameObject;




		staticClass.changeBerry ();
		//title внизу
		GameObject.Find ("label title number level").GetComponent<UILabel> ().text = SceneManager.GetActiveScene().name.Substring (5);


		guiTimer = GameObject.Find("gui timer 2").GetComponent<UILabel>();
		//timer, если доп уровень
		if (initLevelMenuClass.levelDemands == 1) {
			int levels = staticClass.levels[Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5)), 1];
			if (levels >= 1 && levels <=99) {
				GameObject.Find("gui timer").GetComponent<UILabel>().enabled = true;
                GameObject.Find("gui timer/shine timer").GetComponent<UI2DSprite>().enabled = true;
                guiTimer = GameObject.Find("gui timer 2").GetComponent<UILabel>();
				guiTimer.enabled = true;
				if (levels - staticClass.timer < 10)
					guiTimer.text = 
						"0" + (levels - staticClass.timer).ToString();
				else guiTimer.text = (levels - staticClass.timer).ToString();
			}

		}

		fixedCounter = 0;

		//backs
		back = GameObject.Find("back forest rigidbody");
		if (back == null) back = GameObject.Find("back rock");
		if (back == null) back = GameObject.Find("back ice rigidbody");
		if (back == null) back = GameObject.Find("back desert");
		//if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogScreen(SceneManager.GetActiveScene().name);
		//Everyplay
		buttonEveryplayScript = GameObject.Find("/default level/gui/button everyplay").GetComponent<ctrScreenshotClass>();

		//bonusesView
		if (staticClass.bonusesView) {
			GameObject.Find ("arrow right").SendMessage ("clickBonusesArrow");
			GameObject.Find ("tween").transform.localPosition = new Vector3 (880, 0, 0);
		}

        //если уровень запущен 1й раз
        if (staticClass.scenePrev != SceneManager.GetActiveScene().name)
        {
            staticClass.levelRestartedCount = 0;
            staticClass.levelAdViewed = 0;
        }

        //level restarted
        if (staticClass.scenePrev == SceneManager.GetActiveScene().name && gHintClass.hintState == "")
	    {
	        staticClass.levelRestartedCount++;
	    }

	    var dreamIsset = false;
	    //dream
            var p = ctrProgressClass.progress[SceneManager.GetActiveScene().name + "_dream"];
        if (staticClass.scenePrev == SceneManager.GetActiveScene().name && !((p == 1 || p == 3) && initLevelMenuClass.levelDemands == 0 || (p == 2 || p == 3)
            && initLevelMenuClass.levelDemands == 1)
            && gHintClass.hintState == "")
	    {
	       // staticClass.levelRestartedCount++;
	        if (staticClass.levelRestartedCount >= 2)
	        {
                var dream = GameObject.Find("/default level/gui/dream");
                dream.transform.GetChild(0).gameObject.SetActive(true);
                dream.transform.GetChild(1).gameObject.SetActive(false);
	            dream.GetComponent<BoxCollider>().enabled = true;
	        }
	    }
	    else if
            //если уже есть подсказка
            ((p == 1 || p == 3) && initLevelMenuClass.levelDemands == 0 ||
             (p == 2 || p == 3) && initLevelMenuClass.levelDemands == 1 
             && gHintClass.hintState == "")
        {
            var dream = GameObject.Find("/default level/gui/dream");
            dream.transform.GetChild(0).gameObject.SetActive(false);
            dream.transform.GetChild(1).gameObject.SetActive(true);
            dream.GetComponent<BoxCollider>().enabled = true;
            dreamIsset = true;
        }



	    //off if publish
        gRecHintClass.rec = "";
        gRecHintClass.counter = 0;
        gRecHintClass.recHintState = 0;

        //show tutorial hint
        if (staticClass.levelRestartedCount >= 3 && ctrProgressClass.progress["tutorialHint"] == 0 &&
            ctrProgressClass.progress["hints"] > 0 && ctrProgressClass.progress["tutorialDream"] != ctrProgressClass.progress["currentLevel"] && !dreamIsset)

        {
            var arrowTemp = GameObject.Find("/default level/gui/bonuses/tween/arrow left");
            if (arrowTemp.activeSelf)
            {
                arrowTemp.SendMessage("clickBonusesArrow");
                arrowTemp.transform.parent.transform.localPosition = new Vector3(160, 0, 0);
            }

            GameObject tutorialHintGO = Instantiate(tutorialHint, new Vector2(0, 0), Quaternion.identity) as GameObject;
            //position hand
	        var bonusesGO = GameObject.Find("/default level/gui/bonuses");
            bonusesGO.GetComponent<UIWidget>().Update();
            tutorialHintGO.transform.GetChild(0).transform.localPosition = bonusesGO.transform.localPosition + new Vector3(50, 110, 0);
            staticClass.isTimePlay = Time.timeScale;
            Time.timeScale = 0;

            //off level tutorial
            if (GameObject.Find("/default level/gui/tutorial") != null) GameObject.Find("/default level/gui/tutorial").transform.localScale = Vector3.zero;
        }


	    Debug.Log("tutorialBonus: " + ctrProgressClass.progress["tutorialBonus"]);
        Debug.Log("staticClass.levelRestartedCount: " + staticClass.levelRestartedCount);
        Debug.Log("gHintClass.hintState: " + gHintClass.hintState);

        //show tutorial bonus
        if (ctrProgressClass.progress["tutorialDream"] != ctrProgressClass.progress["currentLevel"] && staticClass.levelRestartedCount >= 3 && ctrProgressClass.progress["tutorialBonus"] == 0 &&
	        ctrProgressClass.progress["hints"] == 0 && gHintClass.hintState == "" &&
            (ctrProgressClass.progress["webs"] > 0 || ctrProgressClass.progress["teleports"] > 0 ||
	         ctrProgressClass.progress["collectors"] > 0) && !dreamIsset)
	    {
	        var arrowTemp = GameObject.Find("/default level/gui/bonuses/tween/arrow left");
            if (arrowTemp.activeSelf)
	        {
                arrowTemp.SendMessage("clickBonusesArrow");
                arrowTemp.transform.parent.transform.localPosition = new Vector3(160, 0, 0);
            }
            GameObject tutorialBonusGO = Instantiate(tutorialBonus, new Vector2(0, 0), Quaternion.identity) as GameObject;
            //position hand
	        var bonusesGO = GameObject.Find("/default level/gui/bonuses");
            bonusesGO.GetComponent<UIWidget>().Update();
            tutorialBonusGO.transform.GetChild(0).transform.localPosition = bonusesGO.transform.localPosition + new Vector3(195, 80, 0);
            staticClass.isTimePlay = Time.timeScale;
            Time.timeScale = 0;
            
            //off level tutorial
            if (GameObject.Find("/default level/gui/tutorial") != null) GameObject.Find("/default level/gui/tutorial").transform.localScale = Vector3.zero;
        }

        //show tutorial dream
        if (staticClass.levelRestartedCount == 2 && ctrProgressClass.progress["tutorialDream"] == 0 && gHintClass.hintState == "")
        {
            GameObject tutorialDreamGO = Instantiate(tutorialDream, new Vector2(0, 0), Quaternion.identity) as GameObject;
            //position hand
            var dreamButton = GameObject.Find("/default level/gui/dream");
            dreamButton.GetComponent<UIWidget>().Update();
            tutorialDreamGO.transform.GetChild(0).transform.localPosition = dreamButton.transform.localPosition + new Vector3(-88, -88, 0);

            //icon ad disable
            GameObject.Find("/default level/gui/dream").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("/default level/gui/dream").transform.GetChild(1).gameObject.SetActive(true);

            staticClass.isTimePlay = Time.timeScale;
            Time.timeScale = 0;

            //off level tutorial
            if (GameObject.Find("/default level/gui/tutorial") != null) GameObject.Find("/default level/gui/tutorial").transform.localScale = Vector3.zero;
        }

    }



    void FixedUpdate () {

        if (gHintClass.hintState == "start" && gHintClass.counter <= gHintClass.actions.Length - 1) {
				if (fixedCounter - gHintClass.fixedFrameCountLast == gHintClass.actions [gHintClass.counter].frame) { 
					Time.timeScale = 0;
					gHintClass.hint.GetComponent<AudioSource> ().Play ();



					//делается при старте полета птицы, начиная со 2й точки
					if (gHintClass.counter <= gHintClass.actions.Length - 1 && gHintClass.counter > 0) {
						if (gHintClass.actions [gHintClass.counter - 1].id.x > gHintClass.actions [gHintClass.counter].id.x)
							gHintClass.hint.transform.localScale = new Vector2 (-1, 1);
						else if (gHintClass.actions [gHintClass.counter - 1].id.x < gHintClass.actions [gHintClass.counter].id.x)
							gHintClass.hint.transform.localScale = new Vector2 (1, 1);

					}
					
					gHintClass.hintEndPos = gHintClass.actions [gHintClass.counter].id; 
					//если sluggish
					foreach (GameObject go in GameObject.FindGameObjectsWithTag("sluggish")) {
						if (go.transform.position == gHintClass.actions [gHintClass.counter].id)
							gHintClass.hintEndPos = new Vector3 (gHintClass.hintEndPos.x - 0.02F, gHintClass.hintEndPos.y - 0.33F, gHintClass.hintEndPos.z); 
					} 
					//если groot
					if (ctrProgressClass.progress["currentLevel"] >= 37)
					foreach (GameObject go in GameObject.FindGameObjectsWithTag("groot")) {
						if (go.transform.position == gHintClass.actions [gHintClass.counter].id)
							gHintClass.hintEndPos = go.transform.GetChild (2).GetChild (1).GetChild (6).transform.position;
					} 	
				
					gHintClass.hintStartPos = gHintClass.hint.transform.position;
					gHintClass.time = Time.unscaledTime;
					gHintClass.flagTransform = true;

				}

			}


		fixedCounter++;
	}

	void Awake () {
		Time.timeScale = 1;
	    staticClass.isTimePlay = 1;


        //для записи подсказки (потом удалить)
        /*
        gRecHintClass.recHintState = 0;
        gRecHintClass.counter = 0;
        gRecHintClass.rec = "";
        */
        //
    }

    // Update is called once per frame
    void Update () {

		       //acceleration start
        if (Time.time - t > 0.02F) {
			t = Time.time;

            dir.y = Input.acceleration.y * 0.95F;
            dir.x = Input.acceleration.x * 0.75F;
#if UNITY_EDITOR
            dir = new Vector3(0, -0.95F, 0);
#endif
			back.GetComponent<Rigidbody2D>().AddForce((-dir - back.transform.localPosition / 100) * 5);

            back.GetComponent<Rigidbody2D>().drag = (1 - (-new Vector2(dir.x, dir.y) - 
			                             new Vector2(back.transform.localPosition.x, back.transform.localPosition.y)  / 100).magnitude) * 10;
		}
		
        //acceleration end


		if (transform.position.x < -4 || transform.position.x > 4 || transform.position.y < -6 || transform.position.y > 6)
			if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider sad start") &&
		        !staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo (1).IsName ("spider sad end")) {

				StartCoroutine (gSpiderClass.coroutineCry (staticClass.currentSkinAnimator));
				staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (3).GetComponent<AudioSource> ().Play ();

			}
        //timer
        if (initLevelMenuClass.levelDemands == 1) {
			int levels = staticClass.levels[Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5)), 1];
			if (levels >= 1 && levels <=99) 
			if (Mathf.Ceil(Time.timeSinceLevelLoad) > staticClass.timer) {
				staticClass.timer = Convert.ToInt32(Mathf.Ceil(Time.timeSinceLevelLoad));
				if(levels - staticClass.timer <= 0)	guiTimer.text = "00";
				else if (levels - staticClass.timer < 10)
					guiTimer.text = 
						"0" + (levels - staticClass.timer).ToString();
				else guiTimer.text = (levels - staticClass.timer).ToString();
			}
		}

		//обработка кнопки "Назад" на Android

		if (Input.GetButtonDown("Cancel")) {
			if (GameObject.Find ("market") != null) {
				if (GameObject.Find ("market/open booster menu") == null)
					GameObject.Find ("button market exit").SendMessage ("OnPress", false);
			} else if (!completeMenu.activeSelf && !pauseMenu.activeSelf) {
				pauseMenu.SetActive (true);

                pauseMenu.GetComponent<lsLevelMenuClass>().setContent2();
                if (initLevelMenuClass.levelDemands == 0) pauseMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                else pauseMenu.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                staticClass.isTimePlay = Time.timeScale;
                Time.timeScale = 0;

            }
            else if (pauseMenu.activeSelf) {
				pauseMenu.SetActive (false);
				Time.timeScale = staticClass.isTimePlay;
			}
		}




	}

	void OnCollisionEnter2D (Collision2D collisionObject) {
		if (collisionObject.gameObject.name == "spider") {
			berryState = "start finish";
            gHintClass.hintState = "";

            //cut ropes
            GameObject[] webs = GameObject.FindGameObjectsWithTag("web");
			foreach (var web in webs) {
				if (web.GetComponent<gWebClass> ().webStateCollisionBerry) {
					staticClass.useWeb--;
					web.SendMessage ("OnClick");
				}
			}



			//tutorial
			if (ctrProgressClass.progress["currentLevel"] == 28) 
				GameObject.Find("default level/gui/tutorial").GetComponent<gHandClass>().delHand (-1, 0);

			if (!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider open month legs") &&
				!staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(1).IsName("spider open month"))
				staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (1).GetComponent<AudioSource> ().Play ();

			if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath"))
				staticClass.currentSkinAnimator.Play("spider open month legs", 1);
			else 
				staticClass.currentSkinAnimator.Play("spider open month", 1);
			
			//GetComponent<Animation>().Play();
			//transform.position = collisionObject.gameObject.transform.position;
			if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
			GetComponent<Rigidbody2D>().isKinematic = true;
			GetComponent<Collider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            collisionObject.rigidbody.isKinematic = true;
            collisionObject.rigidbody.angularVelocity = 0;
            collisionObject.rigidbody.velocity = new Vector2(0, 0);

            StartCoroutine(coroutineEat(collisionObject));
		}

	}

	void OnTriggerEnter2D(Collider2D collisionObject) {
		if (collisionObject.gameObject.name == "star") {

			//Everyplay
			StartCoroutine(collisionObject.gameObject.GetComponent<gStarClass>().destroyStar(buttonEveryplayScript));

			guiStars[starsCounter].SetActive(true);
			starsCounter ++;


		}

	}
	public IEnumerator coroutineEat(Collision2D collisionObject){
		collisionObject.rigidbody.isKinematic = false;
		transform.GetChild (int.Parse (staticClass.currentBerry.Substring (5, 1)) - 1).GetComponent<SpriteRenderer> ().sortingOrder = 119;
		for (float i = 0; i < 5; i+=0.5F) {
			transform.GetChild(int.Parse(staticClass.currentBerry.Substring(5, 1)) - 1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8F - i * 0.2F);

			//transform.GetChild(int.Parse(staticClass.currentBerry.Substring(5, 1)) - 1).GetComponent<AnimatedAlpha>().alpha = 0.8F - i * 0.2F;
			transform.localScale = new Vector2(1 - i * 0.05F, 1 - i * 0.05F);
			transform.position = transform.position + (collisionObject.transform.position - transform.position) * 0.2F;
			yield return new WaitForSeconds(0.015F);

		}

		if (staticClass.currentSkinAnimator.GetCurrentAnimatorStateInfo(0).IsName("spider breath"))
			staticClass.currentSkinAnimator.CrossFade ("spider eat legs", 0.5F);
		else 
			staticClass.currentSkinAnimator.CrossFade ("spider eat", 0.5F);
		staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (2).GetComponent<AudioSource> ().Play ();
		StartCoroutine(Coroutine(collisionObject));

	}

	public IEnumerator Coroutine(Collision2D collisionObject){

		yield return new WaitForSeconds(0.5F);
		//if (Everyplay.IsRecording ()) 
		//	buttonEveryplayScript.takeScreenshot ();

        //restart scene, if dream show
        if (GameObject.Find("/default level/gui/dream/ui").activeSelf) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //for analytics
        ctrProgressClass.progress["winCount"]++;
        
        // остановка выполнения функции на costEnergy секунд
        yield return new WaitForSeconds(1.0F);
		Debug.Log(gHintClass.hintState);
        gHintClass.hintState = "";
        //для записи подсказки (потом удалить)
        gRecHintClass.recHintState = 0;
		//D/ebug.Log ("rec: ");
		//D/ebug.Log (gRecHintClass.rec);
		gRecHintClass.counter = 0;
		gRecHintClass.rec = "";
		//
		staticClass.currentSkinAnimator.Play("spider idle", 1);

        berryState = "finish";

		int lvlNumber = Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5));

        /* достижение (пока коммент)
        if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			GooglePlayManager.instance.SubmitScore ("leaderboard_forest", ctrProgressClass.progress["stars"]);
			if (Application.loadedLevelName == "level1") GooglePlayManager.instance.UnlockAchievement("achievement_complete_first_level");

			if (lvlNumber > ctrProgressClass.progress["lastLevel"]) {
				GooglePlayManager.instance.IncrementAchievement("achievement_complete_5_levels", 1);
				GooglePlayManager.instance.IncrementAchievement("achievement_complete_the_game", 1);

			}


		}
        */
        bool flagGemGetting = false;
		bool flagGemGetting2 = false; //получил гем или нет за текущий уровень независимо оот того, получал или нет раньше
		if (initLevelMenuClass.levelDemands == 0 && starsCounter == 3) flagGemGetting2 = true;
			
		//получение гема за основное прохождение
		if (initLevelMenuClass.levelDemands == 0) {
			if (starsCounter == 3 && ctrProgressClass.progress[SceneManager.GetActiveScene().name] != 1 && ctrProgressClass.progress[SceneManager.GetActiveScene().name] != 3) {
                flagGemGetting = true;
                ctrProgressClass.progress["gems"] ++;
				if (ctrProgressClass.progress[SceneManager.GetActiveScene().name] == 0) ctrProgressClass.progress[SceneManager.GetActiveScene().name] = 1;
				else ctrProgressClass.progress[SceneManager.GetActiveScene().name] = 3;
			}
	}
        //ctrProgressClass.progress["stars"] = ctrProgressClass.progress["stars"] + starsCounter - ctrProgressClass.progress["level" + lvlNumber];


		//получение гема за испытание
		if (initLevelMenuClass.levelDemands == 1  && staticClass.levels[lvlNumber, 0] == starsCounter) {
			int levels = staticClass.levels[lvlNumber, 1];
			if (levels == 0) flagGemGetting2 = true;
			else if (levels >= 1 && levels <=99 && staticClass.timer <= levels) flagGemGetting2 = true;
			else if (levels >= 100 && levels <=199 && staticClass.useWeb <= (levels - 100)) flagGemGetting2 = true;
			else if (levels == 201 && staticClass.useSluggish == false) flagGemGetting2 = true;
			else if (levels == 202 && staticClass.useDestroyer == false) flagGemGetting2 = true;
			else if (levels == 203 && staticClass.useYeti == false) flagGemGetting2 = true;
			else if (levels == 204 && staticClass.useGroot == false) flagGemGetting2 = true;
			if (flagGemGetting2 && ctrProgressClass.progress[SceneManager.GetActiveScene().name] < 2) {
                flagGemGetting = true;
                ctrProgressClass.progress["gems"] ++;
				if (ctrProgressClass.progress[SceneManager.GetActiveScene().name] == 0) ctrProgressClass.progress[SceneManager.GetActiveScene().name] = 2;
				else ctrProgressClass.progress[SceneManager.GetActiveScene().name] = 3;
			}
		}

        if (flagGemGetting) endLevel(true, "complete");
        else if (flagGemGetting2) endLevel(true, "already");
        else endLevel(true);
        //if (ctrProgressClass.progress["level" + lvlNumber] < starsCounter) {
        //if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) GooglePlayManager.instance.IncrementAchievement("achievement_collect_all_stars", starsCounter - ctrProgressClass.progress["level" + lvlNumber]);
        //ctrProgressClass.progress["level" + lvlNumber] = starsCounter;
        //}

	    if (lvlNumber >= ctrProgressClass.progress["lastLevel"])
	    {
	        ctrProgressClass.progress["lastLevel"] = lvlNumber;
            ctrAnalyticsClass.sendCustomDimension(4, lvlNumber.ToString()); 

        }

        //ctrProgressClass.saveProgress();

        //Everyplay
        //Everyplay metadata
        /*
        if (Everyplay.IsRecording ()) { 
			Everyplay.SetMetadata ("Level", "Level " + ctrProgressClass.progress["currentLevel"]);
			if (initLevelMenuClass.levelDemands == 0) {
				Everyplay.SetMetadata ("Stars", starsCounter);
				Everyplay.SetMetadata ("Difficult", "Normal");
				int scoreTemp = Mathf.RoundToInt (2000 * starsCounter + 3000 - 100 * Time.timeSinceLevelLoad);
				if (scoreTemp < 0)
					scoreTemp = 0;
				Everyplay.SetMetadata ("Score", scoreTemp);
				if (flagGemGetting2) {

					Everyplay.SetMetadata ("Title", "I just got a crystal and scored "+scoreTemp+" points in level "+ctrProgressClass.progress["currentLevel"]+"!");
					Everyplay.SetMetadata ("Gem", true);
				} else {
					Everyplay.SetMetadata ("Gem", false);
					Everyplay.SetMetadata ("Title", "I just scored "+scoreTemp+" points in level "+ctrProgressClass.progress["currentLevel"]+"!");
				} 
			} else {
				Everyplay.SetMetadata ("Difficult", "Challenge");
				if (flagGemGetting2) {
					Everyplay.SetMetadata ("Gem", true);
					Everyplay.SetMetadata ("Score", 3000);
					Everyplay.SetMetadata ("Title", "I just got a crystal and scored 3000 points in level "+ctrProgressClass.progress["currentLevel"]+" on challenge difficulty!");

				} else {
					Everyplay.SetMetadata ("Gem", false);
					Everyplay.SetMetadata ("Score", 0);
					Everyplay.SetMetadata ("Title", "I just played on level "+ctrProgressClass.progress["currentLevel"]+"!");
				}

			}
			Everyplay.SetMetadata ("Time", Time.timeSinceLevelLoad);
			Everyplay.StopRecording ();
			buttonEveryplayScript.enableScreenshots();
		}
		//buttonEveryplayScript.enableScreenshots();
        */
        completeMenu.SetActive(true);


        //списание энергии
        Debug.Log("energyTake: " + lsEnergyClass.energyTake);
        if (!lsEnergyClass.energyTake) lsEnergyClass.checkEnergy(false);

        //вызов complete menu, передача полученныx очков (float timeLevel, bool gem, int starsCount, int lvlNumber)
        completeMenu.GetComponent<lsLevelMenuClass>().completeMenuEnable(Time.timeSinceLevelLoad, flagGemGetting, starsCounter, lvlNumber);
//		if (GoogleAnalyticsV4.instance != null) {
//			if (initLevelMenuClass.levelDemands == 0)
//				GoogleAnalyticsV4.instance.LogEvent ("Level", SceneManager.GetActiveScene ().name, starsCounter + " stars", 1);
//			else
//				GoogleAnalyticsV4.instance.LogEvent ("Level", SceneManager.GetActiveScene ().name, "Challenge", 1);
		//}
    }

    public void endLevel(bool result, string taskStatus = "fail")
    {
        Debug.Log("endLevel: " + result);

        var resultStr = (result) ? "win" : "lost";
        var type = (initLevelMenuClass.levelDemands == 0) ? "normal" : "challenge";

        var attr = new Dictionary<string, string>
        {
            {"level number", SceneManager.GetActiveScene().name.Substring(5)},
            {"type", type},
            {"result", resultStr},
            {"task status", taskStatus},
            { "time", Mathf.Round(Time.timeSinceLevelLoad).ToString()}
        };
        if (initLevelMenuClass.levelDemands == 0) attr.Add("stars", starsCounter.ToString());
        ctrAnalyticsClass.sendEvent("Play Level", attr);
    }

	void OnDestroy() {
		//Everyplay
		//if (Everyplay.IsRecording ())
		//	Everyplay.StopRecording ();
	}

    public void OnApplicationFocus(bool flag)
    {
        Debug.Log("OnApplicationFocus: " + staticClass.isTimePlay);
        if (flag)
        {
            Time.timeScale = staticClass.isTimePlay;

            if (staticClass.needShowAdTiredMenu)
            {

                Debug.Log("ad tired menu enable");
                var m =
                    GameObject.Find("/default level/gui/complete menu").GetComponent<lsLevelMenuClass>().adTiredMenu;
                var mGO = Instantiate(m);
                mGO.transform.parent = GameObject.Find("/default level/gui/panel back transition").transform;
                mGO.transform.localScale = new Vector3(1, 1, 1);
                mGO.transform.localPosition = new Vector3(0, 0, 0);
                //disable loading
                mGO.transform.parent.GetChild(1).localScale = Vector3.zero;

                //spider
                mGO.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().Play("spider hi");

                staticClass.adLevelCounter = 0;
                staticClass.needShowAdTiredMenu = false;
            }
        }
    }

}
