using System;
using UnityEngine;
using System.Collections;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using System.Collections.Generic;
using Facebook.Unity;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;


public class ctrAdClass: MonoBehaviour {
	//public GameObject adDontReadyMenu;
	//public GameObject energyGO;
	//public UILabel coinsLabel;
	//public GameObject coinsAdReward;
	public static ctrAdClass instance = null;
	public static string adStarted = "";

    private Dictionary<string, string>adsAttributes = new Dictionary<string, string>
        {{"name", "ad coins"},{"provider", "Unity Ads"}, {"type", "rewarded"},  {"loading", "loaded"},  {"status", "viewed"}};

    private int showAdLevelCounter = 0;
    public InterstitialAd interstitialAdMob;

    public void Start () {
        

        if (instance!=null){
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad (gameObject);

        //admob
        RequestInterstitial();
#if UNITY_ANDROID
        Advertisement.Initialize("1275461", true);
#elif UNITY_IPHONE
        Advertisement.Initialize("1275214", true);

#endif

    }

    public void ShowRewardedAd()
	{
#if UNITY_ANDROID || UNITY_IOS
	    if (adStarted == "ad coins" && ctrProgressClass.progress["tutorialAdCoins"] < 2)
	    {
	        ctrProgressClass.progress["tutorialAdCoins"] = 2;
            GameObject.Find("/root/static/ad coins/hand").SetActive(false);
        }



        adsAttributes["name"] = adStarted;
        adsAttributes["type"] = "rewarded";
        Debug.Log("click ShowRewardedAd");
        adsAttributes["provider"] = "Unity Ads";

        if (isAdReady())
	    {

	        var options = new ShowOptions {resultCallback = HandleShowResultUnityAds};
	        Advertisement.Show("rewardedVideo", options);
        }
        else
	    {
	        //if loading failed, send analytics event
	        adsAttributes["loading"] = "failed";
            adsAttributes["status"] = "failed";
            ctrAnalyticsClass.sendEvent("Advertisment", adsAttributes);
            //adDontReadyMenu
            GameObject.Find("root/static").transform.GetChild(7).gameObject.SetActive(true);
        }
	
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
	//Unity Ads event
	private void HandleShowResultUnityAds(ShowResult result)
	{
        adsAttributes["loading"] = "loaded";
        // off for desktop 
        switch (result)
	    {
	        case ShowResult.Finished:
	            setReward();
                adsAttributes["status"] = "viewed";
                break;
	        case ShowResult.Skipped:
                adsAttributes["status"] = "skip";
                break;
	        case ShowResult.Failed:
                adsAttributes["status"] = "failed";
                break;
	    }
	    ctrAnalyticsClass.sendEvent("Advertisment", adsAttributes);
    }

#endif

    void setReward() {
        if (adStarted == "ad energy") {
			ctrProgressClass.progress ["energyTime"] -= 1 * lsEnergyClass.costEnergy;
			//energyGO
			GameObject.Find ("root/static/energy").SendMessage ("OnApplicationPause", false);
		} else if (adStarted == "ad coins") {
			ctrProgressClass.progress ["coins"] += 50;
            //coinsLabel
            AdCoinsTimerClass.counter++;
            ctrProgressClass.progress["adCoinsDate"] = (int)DateTime.Now.AddSeconds(AdCoinsTimerClass.interval).TotalSeconds();
            AdCoinsTimerClass.timer = DateTime.Now.AddSeconds(AdCoinsTimerClass.interval);
            GameObject.Find ("root/static/coins/coinsLabel").GetComponent<UILabel> ().text = ctrProgressClass.progress ["coins"].ToString ();
            ctrAnalyticsClass.sendEvent("Gold", new Dictionary<string, string>{ {"video", "50"} });
		} else if (adStarted == "dream")
        {
            staticClass.levelAdViewed = 1;
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
        ctrProgressClass.saveProgress ();
		adStarted = "";
        adsAttributes["provider"] = "";
	}

	bool isAdReady() {
#if UNITY_ANDROID || UNITY_IOS
        //rewarded
        if (adsAttributes["type"] == "rewarded" && Advertisement.IsReady("rewardedVideo"))
	    {
            adsAttributes["provider"] = "Unity Ads";
	        return true;
	    }
        //skippable
        else if (adsAttributes["type"] == "skippable" && interstitialAdMob.IsLoaded())
	    {
            adsAttributes["provider"] = "AdMob";
	        return true;
	    }
	    else if (adsAttributes["type"] == "skippable" && Advertisement.IsReady("video"))
	    {

            adsAttributes["provider"] = "Unity Ads";
            return true;
	    }
#endif
	        return false;
	    }

	    public void ShowLevelAd(string buttonName) {
#if UNITY_ANDROID || UNITY_IOS
		if (ctrProgressClass.progress["firstPurchase"] == 0 && ctrProgressClass.progress["currentLevel"] >= 5)
		{
		    bool flag = false;
            if (buttonName == "restart")
		    {
		        var mod = (1 + staticClass.levelRestartedCount) % 3;
                if (mod == 0) flag = true;
            }
		    else
            {
                if (SceneManager.GetActiveScene().name != "level menu" && SceneManager.GetActiveScene().name != "menu" && staticClass.levelAdViewed == 0 && ctrProgressClass.progress["energy"] != 0) flag = true;

            }
            adsAttributes["name"] = "level";
            adsAttributes["type"] = "skippable";
		    if (flag)
		        if (isAdReady())
		        {
		            ctrAdClass.adStarted = "level";
		            if (adsAttributes["provider"] == "Unity Ads")
		            {
                        var options = new ShowOptions { resultCallback = HandleShowResultUnityAds };
                        Advertisement.Show("video", options);
                    }
		            else
		            {
                        Debug.Log("ShowLevelAd AdMob Show");

		                interstitialAdMob.Show();
		            }

                    //GameObject.Find("default level/gui/pause").SendMessage("OnPress", false);
                }
                else
		        {
                    adsAttributes["loading"] = "failed";
                    adsAttributes["status"] = "failed";
                    Debug.Log("ShowLevelAd fail");
                    ctrAnalyticsClass.sendEvent("Advertisment", adsAttributes);
                }


            

        }
#endif

    }

    //admob
    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7139694772964316/6012233386";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-7139694772964316/5872632588";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create an interstitial.
        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        interstitial.OnAdOpening += HandleInterstitialOpened;
        interstitial.OnAdClosed += HandleInterstitialClosed;
        interstitial.OnAdLeavingApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitial.LoadAd(createAdRequest());
        interstitialAdMob = interstitial;

    }
    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("9ACD01B470951161A30C0AA4B6DA3A7D")
                .AddKeyword("game")
                .SetGender(Gender.Male)
                .SetBirthday(new DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();

    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        staticClass.levelAdViewed = 1;
        adsAttributes["loading"] = "loaded";
        Debug.Log("HandleInterstitialOpened event received my");
        //RequestInterstitial();
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        adsAttributes["status"] = "skip";
        ctrAnalyticsClass.sendEvent("Advertisment", adsAttributes);

        Debug.Log("HandleInterstitialClosed event received my");
        RequestInterstitial();
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        adsAttributes["status"] = "clicked";
        ctrAnalyticsClass.sendEvent("Advertisment", adsAttributes);

        Debug.Log("HandleInterstitialLeftApplication event received my");
        RequestInterstitial();
    }
}

