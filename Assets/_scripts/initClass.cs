using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Odnoklassniki;

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
    public GameObject logo;
    public Transform berry;
    public GameObject openBoosterMenu;
    public GameObject previewBoosterMenu;

    //socials
    public GameObject groupVK1;
    public GameObject groupVK2;
    public GameObject repostRewardOK;

    private int i;
	//private int LastNotificationId = 0;
	private int y;

	// Use this for initialization
	void Start ()
	{
        //for test iPad
        Application.targetFrameRate = 60;

#if UNITY_STANDALONE_WIN
    Screen.SetResolution(575, 1024, false);
#endif
        Debug.Log("start menu: " + Screen.currentResolution);

        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            Debug.Log(res.width + "x" + res.height);
        }
        Debug.Log("Device model: " + SystemInfo.deviceModel);
        Debug.Log("Device name: " + SystemInfo.deviceName);
       
        if (mBoosterClass.instance == null) openBoosterMenu.SetActive(true);
        else Destroy(openBoosterMenu);
        if (ctrPreviewBoosterClass.instance == null) previewBoosterMenu.SetActive(true);
        //for test
        //ctrFbKiiClass.instance.onLogin("ok");

        //ctrProgressClass.progress["lastLevel"] = 75;
        //if (ctrProgressClass.progress.Count == 0) {
        Time.maximumDeltaTime = 0.9F;
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress ();
		staticClass.initLevels ();

	    //ctrProgressClass.progress["gems"] = 19;
        //ctrProgressClass.progress["lastLevel"] = 25;
        //staticClass.keysBefore = ctrProgressClass.progress["gems"];
        //опции
        GameObject.Find ("settings folder").transform.GetChild (0).gameObject.SetActive (true);
            
        //language
		var l = staticClass.getLanguage();
        if (l == 2)
			GameObject.Find (Localization.language).transform.GetChild(0).gameObject.SetActive (true);
		else
            GameObject.Find (Localization.language).transform.GetChild(0).gameObject.SetActive (true);


        //ctrProgressClass.progress["language"] = 1;
        //everyplay
        if (ctrProgressClass.progress ["everyplay"] == 1) {
		//	Everyplay.SetDisableSingleCoreDevices (true);
		//	Everyplay.SetMaxRecordingMinutesLength(10);
			//Everyplay.SetLowMemoryDevice (true);
		}
        /*
        if (!Everyplay.IsRecordingSupported () || ctrProgressClass.progress ["everyplay"] == 0) {
			ctrProgressClass.progress ["everyplay"] = 0;
			GameObject.Find ("camera button").transform.GetChild(0).gameObject.SetActive (true);
			GameObject.Find ("camera button").transform.GetChild(1).gameObject.SetActive (false);
		}
        */
		ctrProgressClass.saveProgress ();

		if (ctrProgressClass.progress ["music"] == 0)  GameObject.Find ("music").GetComponent<AudioSource> ().mute = true;
			

		if (ctrProgressClass.progress ["sound"] == 0) AudioListener.pause = true;
			
		GameObject.Find("settings folder").transform.GetChild(0).gameObject.SetActive(false);
		staticClass.rateUsLast = ctrProgressClass.progress["lastLevel"];

		//}



        //включаем текущий скин и выключаем все остальные
        staticClass.changeSkin ();
		staticClass.changeHat ();
		staticClass.changeBerry ();

		staticClass.currentSkinAnimator.Play ("spider hi");
		staticClass.currentSkinAnimator.transform.GetChild (1).GetChild (0).gameObject.GetComponent<AudioSource> ().Play();
		//market
		market.SetActive(true);

		staticClass.sceneLoading = false;

		Time.timeScale = 1;
        Debug.Log("Time.timeScale: " + Time.timeScale);

        //listen for GooglePlayConnection events
        //off (new plugin)
        //GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
        //GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
        //if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogScreen("menu");

	    if (ctrProgressClass.progress["language"] == 1)
	    {
	        logo.transform.GetChild(0).gameObject.SetActive(true);
            berry.localPosition = new Vector3(306, -288, 0);
	    }
	    else
	    {
	        logo.transform.GetChild(1).gameObject.SetActive(true);
            berry.localPosition = new Vector3(330, -140, 0);
        }

        //social
	    if (ctrProgressClass.progress["rewardGroupVK1"] == 1)
	    {
	        groupVK1.SetActive(false);
            groupVK1.transform.parent.GetChild(2).localPosition += new Vector3(0, -120, 0);
        }
        if (ctrProgressClass.progress["rewardGroupVK2"] == 1)
        {
            groupVK2.SetActive(false);
            groupVK2.transform.parent.GetChild(2).localPosition += new Vector3(0, -120, 0);
        }
        if (ctrProgressClass.progress["rewardRepostOK"] == 1) repostRewardOK.SetActive(false);


    }

    // Update is called once per frame
    void Update () {
		y++;
		//if (y == 1) Debug.Log("Update menu: " + Time.realtimeSinceStartup);

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