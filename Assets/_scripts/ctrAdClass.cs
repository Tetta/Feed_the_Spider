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
using Mycom.Target.Unity.Ads;
using Odnoklassniki;
using Object = System.Object;


public class ctrAdClass : MonoBehaviour
{
    //public GameObject adDontReadyMenu;
    //public GameObject energyGO;
    //public UILabel coinsLabel;
    //public GameObject coinsAdReward;
    public static ctrAdClass instance = null;
    public static string adStarted = "";

    private Dictionary<string, string> adsAttributes = new Dictionary<string, string>
    {
        {"name", "ad coins"},
        {"provider", "Unity Ads"},
        {"type", "rewarded"},
        {"loading", "loaded"},
        {"status", "viewed"}
    };

    private int showAdLevelCounter = 0;
    public GoogleMobileAds.Api.InterstitialAd interstitialAdMob;

    //for myTarget
    private readonly System.Object _syncRoot = new System.Object();
    private Mycom.Target.Unity.Ads.InterstitialAd rewardedMyTarget;
    private Mycom.Target.Unity.Ads.InterstitialAd imgMyTarget;
    //private uint rewardedMyTargetId = 38837; //test
    //private uint imgMyTargetId = 6481; //test

#if UNITY_ANDROID
    private uint rewardedMyTargetId = 92777; 
    private uint imgMyTargetId = 92774;
#elif UNITY_IPHONE
    private uint rewardedMyTargetId = 92793; 
    private uint imgMyTargetId = 92790; 

#endif
    public static bool rewardedMyTargetLoaded = false;
    public static bool imgMyTargetLoaded = false;
    public bool needSetRewardMyTarget = false;

    public void Start()
    {


        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        //admob
        RequestInterstitial();
#if UNITY_ANDROID
        Advertisement.Initialize("1275461", true);
#elif UNITY_IPHONE
        Advertisement.Initialize("1275214", true);

#endif

        //myTarget
        //Tracer.IsEnabled = true;
        if (ctrProgressClass.progress["ok"] == 1 && OK.IsLoggedIn)
            loadAdMyTarget();

    }

    public void ShowRewardedAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (adStarted == "ad coins" && ctrProgressClass.progress["tutorialAdCoins"] < 3)
        {
            Debug.Log("tutorialAdCoins < 3");
            ctrProgressClass.progress["tutorialAdCoins"] = 3;
            GameObject.Find("/root/static/ad coins/hand").SetActive(false);
        }



        adsAttributes["name"] = adStarted;
        adsAttributes["type"] = "rewarded";
        Debug.Log("click ShowRewardedAd");
        adsAttributes["provider"] = "Unity Ads";

        if (ctrProgressClass.progress["ok"] == 1 && OK.IsLoggedIn)
            adsAttributes["provider"] = "myTarget";



        if (isAdReady())
        {
            if (ctrProgressClass.progress["ok"] == 1 && OK.IsLoggedIn)
            {
                rewardedMyTarget.Show();
            }
            else
            {
                var options = new ShowOptions {resultCallback = HandleShowResultUnityAds};
                Advertisement.Show("rewardedVideo", options);
            }
        }
        else
        {
            //if loading failed, send analytics event
            adsAttributes["loading"] = "failed";
            adsAttributes["status"] = "failed";
            ctrAnalyticsClass.sendEvent("Advertisment", adsAttributes);
            //adDontReadyMenu
            if (initLevelMenuClass.instance != null) initLevelMenuClass.instance.adDontReadyMenu.SetActive(true);
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

    void setReward()
    {
        if (adStarted == "button ad energy")
        {
            lsEnergyClass.energy = 1;
            //energyGO
            if (GameObject.Find("root/static/energy") != null)
                GameObject.Find("root/static/energy").SendMessage("OnApplicationPause", false);
            GameObject.Find("energy menu/panel with anim/energy").SendMessage("OnApplicationPause", false);
            if (marketClass.instance.gameObject.activeSelf)
                GameObject.Find("/market/shop/market menu/bars/energy").SendMessage("OnApplicationPause", false);

        }
        else if (adStarted == "ad coins")
        {
            ctrProgressClass.progress["coins"] += 50;
            //coinsLabel
            AdCoinsTimerClass.counter++;
            ctrProgressClass.progress["adCoinsDate"] =
                (int) DateTime.Now.AddSeconds(AdCoinsTimerClass.interval).TotalSeconds();
            AdCoinsTimerClass.timer = DateTime.Now.AddSeconds(AdCoinsTimerClass.interval);
            GameObject.Find("root/static/coins/coinsLabel").GetComponent<UILabel>().text =
                ctrProgressClass.progress["coins"].ToString();
            GameObject.Find("root/static/reward menu").SetActive(true);
            ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> {{"detail 1", "video"}, {"coins", "50"}});
            initLevelMenuClass.instance.coinsMenu.SetActive(false);
        }
        else if (adStarted == "dreamShowAd")
        {
            staticClass.levelAdViewed = staticClass.levelRestartedCount + 3;
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
        ctrProgressClass.saveProgress();
        adStarted = "";
        adsAttributes["provider"] = "";
    }

    bool isAdReady()
    {
#if UNITY_ANDROID || UNITY_IOS

        if (ctrProgressClass.progress["ok"] == 1 && OK.IsLoggedIn)
        {
            adsAttributes["provider"] = "myTarget";
            //rewarded
            if (adsAttributes["type"] == "rewarded" && rewardedMyTargetLoaded)
            {
                Debug.Log("rewarded myTarget isReady");
                return true;
            }
            //skippable
            else if (adsAttributes["type"] == "skippable" && imgMyTargetLoaded)
            {
                Debug.Log("img myTarget isReady");
                return true;
            }
            Debug.Log(adsAttributes["type"] + " myTarget dont ready");

            loadAdMyTarget();
            return false;

        }
        else
        {




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
                RequestInterstitial(true);
                adsAttributes["provider"] = "Unity Ads";
                return true;
            }

            //admob


#if UNITY_ANDROID
            Advertisement.Initialize("1275461", true);
#elif UNITY_IPHONE
        Advertisement.Initialize("1275214", true);
#endif

#endif
            return false;
        }

    }

    public bool ShowLevelAd(string buttonName)
    {
#if UNITY_ANDROID || UNITY_IOS
        Debug.Log("ShowLevelAd: " + buttonName);
        if (ctrProgressClass.progress["firstPurchase"] == 0 && ctrProgressClass.progress["currentLevel"] >= 5 && (
                !staticClass.rateUsLevels.Contains(ctrProgressClass.progress["currentLevel"])
            ))
        {
            bool flag = false;

            if (((initLevelMenuClass.levelDemands == 0 && buttonName == "button play 0") ||
                 (initLevelMenuClass.levelDemands == 1 && buttonName == "button play 1")) &&
                SceneManager.GetActiveScene().name != "level menu")
                buttonName = "restart";
            if (staticClass.levelRestartedCount < 2) staticClass.levelAdViewed = 0;
            if (buttonName == "restart")
            {
                var mod = (1 + staticClass.levelRestartedCount)%3;
                if (staticClass.levelAdViewed > 1)
                    mod = (1 + staticClass.levelRestartedCount + staticClass.levelAdViewed)%3;
                if (mod == 0) flag = true;
            }
            else
            {
                //if (buttonName == "button next level" ||
                //     (initLevelMenuClass.levelDemands == 0 && buttonName == "button play 1") ||
                //    (initLevelMenuClass.levelDemands == 1 && buttonName == "button play 0")     )
                //    staticClass.levelAdViewed = 0;
                if (SceneManager.GetActiveScene().name != "level menu" && SceneManager.GetActiveScene().name != "menu" &&
                    staticClass.levelAdViewed == 0 && lsEnergyClass.energy != 0) flag = true;

            }
            adsAttributes["name"] = "level";
            adsAttributes["type"] = "skippable";
            Debug.Log("ShowLevelAd: " + buttonName);
            Debug.Log("ShowLevelAd levelRestartedCount: " + staticClass.levelRestartedCount);
            Debug.Log("ShowLevelAd flag: " + flag);
            Debug.Log("ShowLevelAd scene: " + SceneManager.GetActiveScene().name);
            Debug.Log("ShowLevelAd levelDemands: " + initLevelMenuClass.levelDemands);
            Debug.Log("ShowLevelAd staticClass.levelAdViewed: " + staticClass.levelAdViewed);
            Debug.Log("ShowLevelAd lsEnergyClass.energy: " + lsEnergyClass.energy);

            if (flag)
            {
                Debug.Log("need ShowLevelAd");
                if (isAdReady())
                {
                    ctrAdClass.adStarted = "level";
                    if (ctrProgressClass.progress["ok"] == 1 && OK.IsLoggedIn)
                    {
                        staticClass.isTimePlay = Time.timeScale;
                        Time.timeScale = 0;
                        imgMyTarget.Show();
                    }
                    else
                    {

                        if (adsAttributes["provider"] == "Unity Ads")
                        {
                            var options = new ShowOptions {resultCallback = HandleShowResultUnityAds};
                            Advertisement.Show("video", options);
                        }
                        else
                        {
                            Debug.Log("ShowLevelAd AdMob Show");
                            //pause level
                            staticClass.isTimePlay = Time.timeScale;
                            Time.timeScale = 0;
                            interstitialAdMob.Show();
                        }
                    }
                    return true;
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


        }
#endif
        return false;
    }

    //admob
    public void RequestInterstitial(bool newLoaderFlag = false)
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7139694772964316/6012233386";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-7139694772964316/5872632588";
#else
        string adUnitId = "unexpected_platform";
#endif
        if (newLoaderFlag) new AdLoader.Builder(adUnitId);
        Debug.Log("Admob RequestInterstitial");
        interstitialAdMob = null;
        // Create an interstitial.
        //InterstitialAd interstitialAdMob = new InterstitialAd(adUnitId);
        interstitialAdMob = new GoogleMobileAds.Api.InterstitialAd(adUnitId);
        // Register for ad events.
        interstitialAdMob.OnAdOpening += HandleInterstitialOpened;
        interstitialAdMob.OnAdClosed += HandleInterstitialClosed;
        interstitialAdMob.OnAdLeavingApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitialAdMob.LoadAd(createAdRequest());
        //interstitialAdMob = interstitial;

    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        return new AdRequest.Builder()
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("9ACD01B470951161A30C0AA4B6DA3A7D")
            .AddKeyword("game")
            //.SetGender(Gender.Male)
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






    //-------------------------------------------- myTarget ------------------------------------------
    public void loadAdMyTarget()
    {
        Debug.Log("loadAdMyTarget()");
        try
        {
            lock (_syncRoot)
            {
                if (!rewardedMyTargetLoaded)
                {


                    rewardedMyTarget = new Mycom.Target.Unity.Ads.InterstitialAd(rewardedMyTargetId)
                    {
                        // Задаём дополнительные параметры запроса
                        CustomParams =
                        {
                            // Задаем возраст
                            Age = 30,
                            // Задаем пол
                            Gender = GenderEnum.Female
                        }
                    };
                    rewardedMyTarget.AdClicked += OnAdClickedMyTarget;
                    rewardedMyTarget.AdDisplayed += OnAdDisplayedMyTarget;
                    rewardedMyTarget.AdVideoCompleted += OnAdVideoCompletedMyTarget;
                    rewardedMyTarget.AdLoadCompleted += OnLoadCompletedMyTarget;
                    rewardedMyTarget.Load();
                }
            }
            lock (_syncRoot)
            {
                if (!imgMyTargetLoaded)
                {


                    imgMyTarget = new Mycom.Target.Unity.Ads.InterstitialAd(imgMyTargetId)
                    {
                        // Задаём дополнительные параметры запроса
                        CustomParams =
                        {
                            // Задаем возраст
                            Age = 35,
                            // Задаем пол
                            Gender = GenderEnum.Female
                        }
                    };
                    imgMyTarget.AdClicked += OnAdClickedMyTarget2;
                    imgMyTarget.AdDisplayed += OnAdDisplayedMyTarget2;
                    imgMyTarget.AdVideoCompleted += OnAdVideoCompletedMyTarget2;
                    imgMyTarget.AdLoadCompleted += OnLoadCompletedMyTarget2;
                    imgMyTarget.Load();
                }
            }



        }
        catch (Exception e)
        {
        }


    }

    //rewarded
    private static void OnLoadCompletedMyTarget(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnLoadCompletedMyTarget");
        rewardedMyTargetLoaded = true;
    }
    private static void OnAdClickedMyTarget(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnAdClickedMyTarget");
        rewardedMyTargetLoaded = false;
        instance.adsAttributes["status"] = "clicked";
        //ctrAnalyticsClass.sendEvent("Advertisment", instance.adsAttributes);

    }

    private static void OnAdDisplayedMyTarget(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnAdDisplayedMyTarget");
        rewardedMyTargetLoaded = false;
        instance.adsAttributes["loading"] = "loaded";

    }

    public static void OnAdVideoCompletedMyTarget(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnAdVideoCompletedMyTarget");
        rewardedMyTargetLoaded = false;
        instance.adsAttributes["loading"] = "loaded";
        instance.needSetRewardMyTarget = true;
        instance.adsAttributes["status"] = "viewed";

        ctrAnalyticsClass.sendEvent("Advertisment", instance.adsAttributes);
        instance.loadAdMyTarget();
    }

    //img
    private static void OnLoadCompletedMyTarget2(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnLoadCompletedMyTarget2");
        imgMyTargetLoaded = true;
    }
    private static void OnAdClickedMyTarget2(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnAdClickedMyTarget2");
        instance.adsAttributes["status"] = "clicked";
        imgMyTargetLoaded = false;
        //ctrAnalyticsClass.sendEvent("Advertisment", instance.adsAttributes);
    }

    private static void OnAdDisplayedMyTarget2(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnAdDisplayedMyTarget2");
        imgMyTargetLoaded = false;
        instance.adsAttributes["loading"] = "loaded";
        ctrAnalyticsClass.sendEvent("Advertisment", instance.adsAttributes);
        instance.loadAdMyTarget();
    }

    private static void OnAdVideoCompletedMyTarget2(Object sender, EventArgs eventArgs)
    {
        Debug.Log("OnAdVideoCompletedMyTarget2");
        //imgMyTargetLoaded = false;
        //ctrAnalyticsClass.sendEvent("Advertisment", instance.adsAttributes);

        //instance.loadAdMyTarget();
    }


    public void OnApplicationPause(bool flag)
    {
        Debug.Log("ctrAdClass OnPause: " + flag);
        if (instance != null && !flag)
        {
            if (instance.needSetRewardMyTarget)
            {
                setReward();
                instance.needSetRewardMyTarget = false;
            }


        }

    }

}
