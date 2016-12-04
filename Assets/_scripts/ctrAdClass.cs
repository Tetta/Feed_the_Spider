using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using System.Collections.Generic;

public class ctrAdClass: MonoBehaviour {
	//public GameObject adDontReadyMenu;
	//public GameObject energyGO;
	//public UILabel coinsLabel;
	//public GameObject coinsAdReward;
	public static ctrAdClass instance = null;
	public static string adStarted = "";

	private string adCategory = "";
	private int showAdLevelCounter = 0;

	//AdMob
	private bool IsInterstisialsAdReady = false;

	void Start () {
		if(instance!=null){
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad (gameObject);
		initializeEventHandlers ();

		//AdMob Interstitial
		if(AndroidAdMobController.instance.IsInited) {
			if(!AndroidAdMobController.instance.InterstisialUnitId.Equals("ca-app-pub-3014392707261195/1802093466")) {
				AndroidAdMobController.instance.SetInterstisialsUnitID("ca-app-pub-3014392707261195/1802093466");
			} 
		} else {
			AndroidAdMobController.instance.Init("ca-app-pub-3014392707261195/1802093466");
		}
		AndroidAdMobController.instance.LoadInterstitialAd ();
		AndroidAdMob.Client.OnInterstitialLoaded += OnInterstisialsLoaded; 
		AndroidAdMob.Client.OnInterstitialOpened += OnInterstisialsOpen;

	}

	public void ShowRewardedAd()
	{
		if (isAdReady ()) {
			if (adCategory == "Ad") {
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show ("rewardedVideo", options);
			} else if (adCategory == "Ad Vungle") {
				Dictionary<string, object> options = new Dictionary<string, object> ();
				options ["incentivized"] = true;
				Vungle.playAdWithOptions (options);
				//ad dont ready Unity Ads
				if (adStarted == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "energy", 1);
				if (adStarted == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "coins", 1);
				if (adStarted == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "telek", 1);
			}

			if (adStarted == "button ad energy") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "energy", 1);
			if (adStarted == "button ad coins") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "coins", 1);
			if (adStarted == "button ad telek") GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "telek", 1);
		} else {
 			//ad dont ready Vungle
			if (adStarted == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "energy", 1);
			if (adStarted == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "coins", 1);
			if (adStarted == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "telek", 1);

			//adDontReadyMenu
			if (adStarted != "button ad telek")  GameObject.Find("root/static").transform.GetChild(7).gameObject.SetActive(true);

			//#if UNITY_ANDROID || UNITY_IOS
			//#endif
		}
	}

	#if UNITY_ANDROID || UNITY_IOS
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

	void initializeEventHandlers() {
		Debug.Log ("initializeEventHandlers");
		//Event is triggered when a Vungle ad finished and provides the entire information about this event
		//These can be used to determine how much of the video the user viewed, if they skipped the ad early, etc.
		Vungle.onAdFinishedEvent += (args) => {
			Debug.Log ("Ad finished - watched time:" + args.TimeWatched + ", total duration:" + args.TotalDuration 
				+ ", was call to action clicked:" + args.WasCallToActionClicked +  ", is completed view:" 
				+ args.IsCompletedView);

			if (args.IsCompletedView) {
				setReward();
			}
		};
	}

	void setReward() {

		if (adStarted == "button ad energy") {
			GoogleAnalyticsV4.instance.LogEvent (adCategory, "finish", "energy", 1);
			ctrProgressClass.progress ["energyTime"] -= 5 * lsEnergyClass.costEnergy;
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
		}
		if (adStarted != "level") ctrProgressClass.saveProgress ();
		adStarted = "";
		adCategory = "";
	}

	bool isAdReady() {
		if (Advertisement.IsReady ("video") && adStarted == "level") {
			adCategory = "Ad";
			return true;
		} else if (Advertisement.IsReady ("rewardedVideo") && adStarted != "level") {
			adCategory = "Ad";
			return true;
		} else if (Vungle.isAdvertAvailable ()) {
			adCategory = "Ad Vungle";
			return true;
		}
		return false;

	}

	public void ShowLevelAd() {
		#if UNITY_ANDROID || UNITY_IOS
		if (ctrProgressClass.progress["complect"] == 0 && ctrProgressClass.progress["currentLevel"] >= 5) {
			showAdLevelCounter ++;

			if (showAdLevelCounter == 5 || showAdLevelCounter == 10 || showAdLevelCounter == 15) {
				ctrAdClass.adStarted = "level";

				if (IsInterstisialsAdReady && showAdLevelCounter != 15) {
					AndroidAdMobController.instance.ShowInterstitialAd();
					GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);

				} else if (isAdReady()) {
					
					if (adCategory == "Ad") {
						var options = new ShowOptions { resultCallback = HandleShowResult };
						Advertisement.Show ("video", options);
					} else if (adCategory == "Ad Vungle") {
						Dictionary<string, object> options = new Dictionary<string, object> ();
						options ["incentivized"] = false;
						Vungle.playAdWithOptions (options);
						//ad dont ready Unity Ads
						if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "level", 1);
					}
					if (showAdLevelCounter >= 15) showAdLevelCounter = 0;
					//pause
					GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);

					if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent(adCategory, "start", "level", 1);

				} else {
					if (IsInterstisialsAdReady) {
						AndroidAdMobController.instance.ShowInterstitialAd();
						GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);
						if (showAdLevelCounter >= 15) showAdLevelCounter = 0;

					} else {

						//dont ready Vungle
						if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "level", 1);

						showAdLevelCounter = 14;
					}
				}


			} 
		}
		#endif

	}

	//AdMob
	private void OnInterstisialsLoaded() {
		IsInterstisialsAdReady = true;
	}

	private void OnInterstisialsOpen() {
		IsInterstisialsAdReady = false;
		AndroidAdMobController.instance.LoadInterstitialAd ();

	}
}

