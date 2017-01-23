using UnityEngine;
using System.Collections;
#if (UNITY_ANDROID || UNITY_IOS) && UNITY_UNITYADS_API && ENABLE_UNITYADS_RUNTIME && !UNITY_EDITOR
using UnityEngine.Advertisements;
#endif
using System.Collections.Generic;
using GoogleMobileAds.Api;


public class ctrAdClass: MonoBehaviour {
	//public GameObject adDontReadyMenu;
	//public GameObject energyGO;
	//public UILabel coinsLabel;
	//public GameObject coinsAdReward;
	public static ctrAdClass instance = null;
	public static string adStarted = "";

	private string adCategory = "";
	private int showAdLevelCounter = 0;
    public InterstitialAd interstitialAdMob;

    void Start () {
        

        if (instance!=null){
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad (gameObject);

        //admob
        RequestInterstitial();

	}

	public void ShowRewardedAd()
	{
#if (UNITY_ANDROID || UNITY_IOS) && UNITY_UNITYADS_API && ENABLE_UNITYADS_RUNTIME && !UNITY_EDITOR
		if (isAdReady ()) {
			if (adCategory == "Ad Unity") {
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show ("rewardedVideo", options);
			} else if (adCategory == "Ad Admob") {
                interstitialAdMob.Show();
                //ad dont ready Unity Ads
                if (adStarted == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad Unity", "dont ready", "energy", 1);
				if (adStarted == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad Unity", "dont ready", "coins", 1);
				if (adStarted == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad Unity", "dont ready", "telek", 1);
				if (adStarted == "dream") GoogleAnalyticsV4.instance.LogEvent("Ad Unity", "dont ready", "dream", 1);
			}

			if (adStarted == "button ad energy") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "energy", 1);
			if (adStarted == "button ad coins") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "coins", 1);
			if (adStarted == "button ad telek") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "telek", 1);
			if (adStarted == "dream") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "dream", 1);
		} else {
            //ad dont ready Admob
            if (adStarted == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad Admob", "dont ready", "energy", 1);
			if (adStarted == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad Admob", "dont ready", "coins", 1);
			if (adStarted == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad Admob", "dont ready", "telek", 1);
            if (adStarted == "dream") GoogleAnalyticsV4.instance.LogEvent("Ad Admob", "dont ready", "dream", 1);

			//adDontReadyMenu
			if (adStarted != "button ad telek")  GameObject.Find("root/static").transform.GetChild(7).gameObject.SetActive(true);

			//#if UNITY_ANDROID || UNITY_IOS
			//#endif
		}
#endif
    }

#if (UNITY_ANDROID || UNITY_IOS) && UNITY_UNITYADS_API && ENABLE_UNITYADS_RUNTIME && !UNITY_EDITOR
	//Unity Ads event
	private void HandleShowResult(ShowResult result)
	{
		// off for desktop 
		switch (result)
		{
		case ShowResult.Finished:
			setReward ();
			break;
		}
		
	}

#endif

    //on admob rewarded finish

    void setReward() {

		if (adStarted == "button ad energy") {
			GoogleAnalyticsV4.instance.LogEvent (adCategory, "finish", "energy", 1);
			ctrProgressClass.progress ["energyTime"] -= 1 * lsEnergyClass.costEnergy;
			//energyGO
			GameObject.Find ("root/static/energy").SendMessage ("OnApplicationPause", false);
		} else if (adStarted == "button ad coins") {
			GoogleAnalyticsV4.instance.LogEvent (adCategory, "finish", "coins", 1);
			ctrProgressClass.progress ["coins"] += 70;
			//coinsLabel
			GameObject.Find ("root/static/coins/coinsLabel").GetComponent<UILabel> ().text = ctrProgressClass.progress ["coins"].ToString ();
			ctrStatsClass.logEvent ("coins", "ad_coins", "level" + ctrProgressClass.progress ["lastLevel"].ToString (), 70);
		} else if (adStarted == "button ad telek") {
			GoogleAnalyticsV4.instance.LogEvent (adCategory, "finish", "telek", 1);
			int coinsAdReward = int.Parse (GameObject.Find ("default level/gui/complete menu/panel with anim/coins ad reward/label coins ad").GetComponent<UILabel> ().text.Substring (1));
			ctrProgressClass.progress ["coins"] += coinsAdReward;

			//coinsAdReward on
			GameObject.Find ("default level/gui/complete menu/panel with anim/coins ad reward").SetActive (true);
			//telek off
			GameObject.Find ("default level/gui/complete menu/panel with anim/button ad telek").SetActive (false);


			ctrStatsClass.logEvent ("coins", "ad_telek", "level" + ctrProgressClass.progress ["lastLevel"].ToString (), coinsAdReward);
		} else if (adStarted == "level") {
			GoogleAnalyticsV4.instance.LogEvent(adCategory, "finish", "level", 1);
		} else if (adStarted == "dream")
        {
            GoogleAnalyticsV4.instance.LogEvent(adCategory, "finish", "dream", 1);
            //сохраняем dream
            var nameScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            var p = ctrProgressClass.progress[nameScene + "_dream"];

            if (p == 0 && initLevelMenuClass.levelDemands == 0)
                ctrProgressClass.progress[nameScene + "_dream"] = 1;
            else if (p == 0 && initLevelMenuClass.levelDemands == 1)
                ctrProgressClass.progress[nameScene + "_dream"] = 2;
            else
                ctrProgressClass.progress[nameScene + "_dream"] = 3;
            ctrProgressClass.saveProgress();
            gHintClass.initDream();
        }
        if (adStarted != "level") ctrProgressClass.saveProgress ();
		adStarted = "";
		adCategory = "";
	}

	bool isAdReady() {
#if (UNITY_ANDROID || UNITY_IOS) && UNITY_UNITYADS_API && ENABLE_UNITYADS_RUNTIME && !UNITY_EDITOR
		if (Advertisement.IsReady ("video") && adStarted == "level") {
			adCategory = "Ad Unity";
			return true;
		} else if (Advertisement.IsReady ("rewardedVideo") && adStarted != "level") {
			adCategory = "Ad Unity";
			return true;
		} else if (interstitialAdMob.IsLoaded()) {
			adCategory = "Ad Admob";
			return true;
		}
#endif
        return false;

	}

	public void ShowLevelAd() {
#if (UNITY_ANDROID || UNITY_IOS) && UNITY_UNITYADS_API && ENABLE_UNITYADS_RUNTIME && !UNITY_EDITOR
		if (ctrProgressClass.progress["complect"] == 0 && ctrProgressClass.progress["currentLevel"] >= 5) {
			showAdLevelCounter ++;

			if (showAdLevelCounter == 5 || showAdLevelCounter == 10 || showAdLevelCounter == 15) {
				ctrAdClass.adStarted = "level";

				if (interstitialAdMob.IsLoaded() && showAdLevelCounter != 15) {
                    interstitialAdMob.Show();

                    GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);

				} else if (isAdReady()) {
					
					if (adCategory == "Ad Unity") {
						var options = new ShowOptions { resultCallback = HandleShowResult };
						Advertisement.Show ("video", options);
					} else if (adCategory == "Ad Admob") {
						interstitialAdMob.Show();
						//ad dont ready Unity Ads
						if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad Unity", "dont ready", "level", 1);
					}
					if (showAdLevelCounter >= 15) showAdLevelCounter = 0;
					//pause
					GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);

					if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "level", 1);

				} else {
					if (interstitialAdMob.IsLoaded()) {
                        interstitialAdMob.Show();
                        GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);
						if (showAdLevelCounter >= 15) showAdLevelCounter = 0;

					} else {

                        //dont ready Admob
                        if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad Admob", "dont ready", "level", 1);

						showAdLevelCounter = 14;
					}
				}


			} 
		}
#endif

    }

    //AdMob
    private void OnAdOpening() {
        Debug.Log("AdMob OnAdOpening");
        RequestInterstitial();

	}


    //admob
    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3014392707261195/1802093466";
#elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
        interstitialAdMob = interstitial;
    }

}

