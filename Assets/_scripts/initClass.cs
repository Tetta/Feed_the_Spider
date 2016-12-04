using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//D:\Programs\AndroidSDK\platform-tools\adb logcat ActivityManager:I gof.feedthespider/com.unity3d.player.UnityPlayerNativeActivity:D *:S
//
// hash 2jmj7l5rSw0yVb/vlWAYkK/YBwk=
public class initClass : MonoBehaviour {

	//private GameObject testLabel;
	public GameObject googlePlus;
	public GameObject achievements;
	public GameObject leaderboards;
	public GameObject closeMenu;
	public GameObject market;
	public GameObject spider;

	private int i;
	//private int LastNotificationId = 0;
	private int y;

	// Use this for initialization
	void Start () { 



		//AndroidGoogleAnalytics.Instance.EnableAdvertisingIdCollection (true);
		//AndroidGoogleAnalytics.Instance.StartTracking ();
		/*
		GoogleAnalyticsV4.instance.LogScreen ("start");
		GoogleAnalyticsV4.instance.LogScreen("q" + new TimingHitBuilder().GetCampaignName());
		GoogleAnalyticsV4.instance.LogScreen("w" + new TimingHitBuilder().GetCampaignContent());
		GoogleAnalyticsV4.instance.LogScreen("e" + new TimingHitBuilder().GetCampaignID());
		GoogleAnalyticsV4.instance.LogScreen("r" + new TimingHitBuilder().GetCampaignSource());
		Debug.Log ("source: " + new TimingHitBuilder().GetCampaignSource());
		Debug.Log ("Cname: " + new TimingHitBuilder().GetCampaignName());
		Debug.Log ("content: " + new TimingHitBuilder().GetCampaignContent());
		Debug.Log ("id: " + new TimingHitBuilder().GetCampaignID());
		Debug.Log ("t: " + GoogleAnalyticsV4.TIMING_HIT);
		*/
		//for test 
		//Application.targetFrameRate = -1;
		#if UNITY_STANDALONE_WIN
			//Screen.SetResolution(768, 1024, false);
		#endif
		Debug.Log("start menu: " + Time.realtimeSinceStartup);

		if (ctrProgressClass.progress.Count == 0) {
			Time.maximumDeltaTime = 0.9F;
			ctrProgressClass.getProgress ();
			staticClass.initLevels ();

			//опции
			GameObject.Find ("settings folder").transform.GetChild (0).gameObject.SetActive (true);
			//если язык не выбран или русский
			if ((ctrProgressClass.progress ["language"] == 0 && Application.systemLanguage.ToString () == "Russian") || ctrProgressClass.progress ["language"] == 2) {
				Localization.language = "Russian";
				GameObject.Find (Localization.language).transform.GetChild(0).gameObject.SetActive (true);
				ctrProgressClass.progress ["language"] = 2;
			} else {
				Localization.language = "English";
				GameObject.Find (Localization.language).transform.GetChild(0).gameObject.SetActive (true);
				ctrProgressClass.progress ["language"] = 1;
			}

			//everyplay
			if (ctrProgressClass.progress ["everyplay"] == 1) {
				Everyplay.SetDisableSingleCoreDevices (true);
				Everyplay.SetMaxRecordingMinutesLength(10);
				//Everyplay.SetLowMemoryDevice (true);
			}
			if (!Everyplay.IsRecordingSupported () || ctrProgressClass.progress ["everyplay"] == 0) {
				ctrProgressClass.progress ["everyplay"] = 0;
				GameObject.Find ("camera button").transform.GetChild(0).gameObject.SetActive (true);
				GameObject.Find ("camera button").transform.GetChild(1).gameObject.SetActive (false);
			}

			ctrProgressClass.saveProgress ();

			if (ctrProgressClass.progress ["music"] == 0)  GameObject.Find ("music").GetComponent<AudioSource> ().mute = true;
			

			if (ctrProgressClass.progress ["sound"] == 0) AudioListener.pause = true;
			


			GameObject.Find("settings folder").transform.GetChild(0).gameObject.SetActive(false);
			//
			//push
			// bug GameThrive.Init("0e3f3ad6-c7ee-11e4-9aca-47e056863935", "660292827653", HandleNotification);
			/*
			GoogleCloudMessageService.instance.InitPushNotifications ();
			//AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleDay"), "notiferTitleDay", 600);
			//AndroidNotificationManager.instance.OnNotificationIdLoaded += OnNotificationIdLoaded;
			//AndroidNotificationManager.instance.LocadAppLaunchNotificationId();

			List<LocalNotificationTemplate> PendingNotifications;
			PendingNotifications = AndroidNotificationManager.instance.LoadPendingNotifications();
            //для нотифера внизу отключен, чтоб не было warning bool flagNotifer = false;
            if (PendingNotifications != null) { 
				foreach (var PendingNotification in PendingNotifications) {
					if (PendingNotification.title == Localization.Get("notiferTitleDay")) {
						if (PendingNotification.fireDate.Day == DateTime.Now.Day) {
							AndroidNotificationManager.instance.CancelLocalNotification(PendingNotification.id);
							AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleDay"), Localization.Get("notiferMessageDay"), 60 * 60 * 24);
						}
						//flagNotifer = true; //для нотифера внизу отключен, чтоб не было warning
					}
				}
			}
*/
			//bug!!!
			//if (!flagNotifer) AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleDay"), Localization.Get("notiferMesssageDay"), 60 * 60 * 24);

			//
			//Vungle
			Vungle.init ("57833fb1d34920b24f000046", "57833fb1d34920b24f000046", "57833fb1d34920b24f000046");


		}



		//включаем текущий скин и выключаем все остальные
		staticClass.changeSkin ();
		staticClass.changeHat ();
		staticClass.changeBerry ();

		staticClass.currentSkinAnimator.Play ("spider hi");
		staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (0).gameObject.GetComponent<AudioSource> ().Play();
		//market
		market.SetActive(true);

		/*
		if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
			achievements.SetActive(true);
			leaderboards.SetActive(true);
			googlePlus.SetActive(false);
		} else if (ctrProgressClass.progress["googlePlay"] == 1) GooglePlayConnection.instance.connect ();
		*/

		staticClass.sceneLoading = false;

		Time.timeScale = 1;
		//listen for GooglePlayConnection events
		//off (new plugin)
		//GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
		//GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogScreen("menu");

	}

	// Update is called once per frame
	void Update () {
		y++;
		if (y == 1) Debug.Log("Update menu: " + Time.realtimeSinceStartup);

		//Application.RegisterLogCallback(handleLog);

		//обработка кнопки "Назад" на Android
		if (Input.GetButtonDown("Cancel")) {
			if (GameObject.Find ("daily bonus menu") != null)
				GameObject.Find ("exit daily menu").SendMessage ("OnPress", false);
			else if (GameObject.Find ("market") != null) {
				if (GameObject.Find ("market/open booster menu") == null)
					GameObject.Find ("button market exit").SendMessage ("OnPress", false);
			} else if (GameObject.Find ("settings") != null)
				GameObject.Find ("button settings exit").SendMessage ("OnPress", false);
			else if  (GameObject.Find ("close menu") != null)
				GameObject.Find ("button exit close menu").SendMessage ("OnPress", false);
			else closeMenu.SetActive(true);
		}
	}

	private void OnPlayerConnected() {
		achievements.SetActive(true);
		leaderboards.SetActive(true);
		googlePlus.SetActive(false);
		ctrProgressClass.progress["googlePlay"] = 1;
		ctrProgressClass.saveProgress();
	}

	private void OnPlayerDisconnected() {
		/*
		GooglePlayConnection.instance.disconnect ();
		achievements.SetActive(false);
		leaderboards.SetActive(false);
		googlePlus.SetActive(true);
		ctrProgressClass.progress["googlePlay"] = 0;
		ctrProgressClass.saveProgress();
		*/
	}


	


	/*
	static public void updateProgress() {
		goldLabel.text = progress["coins"].ToString();
		starsLabel.text = progress["stars"].ToString();

	}
	*/

	private void OnDestroy() {
		if(!GooglePlayConnection.IsDestroyed) {
			//off (new plugin)
			//GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
			//GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
		}
	}

	static public void setSound(bool flag) {
		UIPlaySound[] sounds = Resources.FindObjectsOfTypeAll(typeof(UIPlaySound))as UIPlaySound[];
		foreach (UIPlaySound sound in sounds) {
			if (flag) sound.enabled = true;
			else sound.enabled = false;
		}

	}

	// Gets called when the player opens the notification. (GameThrive)
	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive) {
		//D/ebug.Log("HandleNotification");
		//D/ebug.Log(message);
	}

	// Gets called when the player opens the notification. (Local)
	private void OnNotificationIdLoaded (int notificationid){
		//D/ebug.Log( "App was laucnhed with notification id: " + notificationid);
	}



}