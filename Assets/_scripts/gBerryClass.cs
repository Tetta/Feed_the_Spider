using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Advertisements;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class gBerryClass : MonoBehaviour {

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



		//showAd, если нет комплекта
		if (ctrAdClass.instance != null) ctrAdClass.instance.ShowLevelAd();
		/*
		#if UNITY_ANDROID || UNITY_IOS
		if (ctrProgressClass.progress["complect"] == 0 && ctrProgressClass.progress["currentLevel"] >= 5) {
			staticClass.showAd ++;

			if (staticClass.showAd >= 15) {
				ctrAdClass.adStarted = "level";
				//if (ctrAdClass.instance.isAdReady()) {
					//if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent(ctrAdClass., "start", "level", 1);

				if (Advertisement.IsReady ("video")) {
					if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad", "start", "level", 1);
					var options = new ShowOptions { resultCallback = HandleShowResult };
					Advertisement.Show ("video", options);
					staticClass.showAd = 0;
					//pause
					pauseMenu.SetActive (true);
					staticClass.isTimePlay = Time.timeScale;
					Time.timeScale = 0;
				} else if (Vungle.isAdvertAvailable()) {
					// Vungle

					Dictionary<string, object> options = new Dictionary<string, object> ();
					options ["incentivized"] = false;
					Vungle.playAdWithOptions (options);
					staticClass.showAd = 0;
					//pause
					pauseMenu.SetActive (true);
					staticClass.isTimePlay = Time.timeScale;
					Time.timeScale = 0;
					//dont ready Unity aAds
					if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "level", 1);

				
				} else {
					
					//dont ready Vungle
					if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "level", 1);

					staticClass.showAd = 14;
				}
			} 
		}
		#endif
		*/

		staticClass.changeBerry ();
		//title внизу
		GameObject.Find ("label title number level").GetComponent<UILabel> ().text = SceneManager.GetActiveScene().name.Substring (5);


		guiTimer = GameObject.Find("gui timer 2").GetComponent<UILabel>();
		//timer, если доп уровень
		if (initLevelMenuClass.levelDemands == 1) {
			int levels = staticClass.levels[Convert.ToInt32(SceneManager.GetActiveScene().name.Substring(5)), 1];
			if (levels >= 1 && levels <=99) {
				GameObject.Find("gui timer").GetComponent<UILabel>().enabled = true;
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
		if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogScreen(SceneManager.GetActiveScene().name);
		//Everyplay
		buttonEveryplayScript = GameObject.Find("/default level/gui/button everyplay").GetComponent<ctrScreenshotClass>();

		//bonusesView
		if (staticClass.bonusesView) {
			GameObject.Find ("arrow right").SendMessage ("clickBonusesArrow");
			GameObject.Find ("tween").transform.localPosition = new Vector3 (880, 0, 0);
		}

		//Vungle event finished
		//initializeEventHandlers ();


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
					if (ctrProgressClass.progress["currentLevel"] > 75)
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
				staticClass.isTimePlay = Time.timeScale;
				Time.timeScale = 0;
			} else if (pauseMenu.activeSelf) {
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
			collisionObject.rigidbody.isKinematic = true;
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
		if (Everyplay.IsRecording ()) 
			buttonEveryplayScript.takeScreenshot ();


		// остановка выполнения функции на costEnergy секунд
		yield return new WaitForSeconds(1.0F);
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




		//if (ctrProgressClass.progress["level" + lvlNumber] < starsCounter) {
			//if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) GooglePlayManager.instance.IncrementAchievement("achievement_collect_all_stars", starsCounter - ctrProgressClass.progress["level" + lvlNumber]);
			//ctrProgressClass.progress["level" + lvlNumber] = starsCounter;
		//}
		
		if (lvlNumber >= ctrProgressClass.progress["lastLevel"]) ctrProgressClass.progress["lastLevel"] = lvlNumber;
		
		//ctrProgressClass.saveProgress();

		//Everyplay
		//Everyplay metadata
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

        completeMenu.SetActive(true);

        //вызов complete menu, передача полученныx очков (float timeLevel, bool gem, int starsCount, int lvlNumber)
        completeMenu.GetComponent<lsLevelMenuClass>().completeMenuEnable(Time.timeSinceLevelLoad, flagGemGetting, starsCounter, lvlNumber);
		if (GoogleAnalyticsV4.instance != null) {
			if (initLevelMenuClass.levelDemands == 0)
				GoogleAnalyticsV4.instance.LogEvent ("Level", SceneManager.GetActiveScene ().name, starsCounter + " stars", 1);
			else
				GoogleAnalyticsV4.instance.LogEvent ("Level", SceneManager.GetActiveScene ().name, "Challenge", 1);
		}
    }



	void OnDestroy() {
		//Everyplay
		if (Everyplay.IsRecording ())
			Everyplay.StopRecording ();
	}

	/*
	//Unity Ads
	#if UNITY_ANDROID || UNITY_IOS
	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			GoogleAnalyticsV4.instance.LogEvent("Ad", "finish", "level", 1);
						break;
		}
	}
	#endif	

	//Vungle
	void initializeEventHandlers() {
		//Event is triggered when a Vungle ad finished and provides the entire information about this event
		//These can be used to determine how much of the video the user viewed, if they skipped the ad early, etc.
		Vungle.onAdFinishedEvent += (args) => {
			if (args.IsCompletedView) GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "finish", "level", 1);
		};
	}
	*/
		
}
