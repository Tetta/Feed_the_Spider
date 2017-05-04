using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine.SceneManagement;

public class iClickClass : MonoBehaviour {
    public static GameObject backTransition = null;

    public string functionStart = "";
    public string functionEnable = "";
    public string functionClick = "";
    public string functionPress = "";
    public string functionPressButton = "";
    public string functionPressButtonTransition = "";
    public string functionDragStart = "";
    public string functionDrag = "";
    public static GameObject currentButton = null;
    //public List<EventDelegate> onFinished = new List<EventDelegate>();

    //private float timeSinceTouch = 0;
    //private bool enableDrag = false;
    //public string functionDestroy = "";

    // Use this for initialization
    void Start() {
        if (functionStart != "") SendMessage(functionStart);
    }



    void OnEnable() {
        if (functionEnable != "") SendMessage(functionEnable);
    }

    void OnClick() {
        if (functionClick != "") SendMessage(functionClick);

	}


    void OnPress(bool isPressed) {
        if (!isPressed && functionPress != "") SendMessage(functionPress, isPressed);
        if (!isPressed && functionPressButton != "") StartCoroutine("coroutinePressButton");
        if (!isPressed && functionPressButtonTransition != "") StartCoroutine("coroutinePressButtonTransition");

    }

    void OnDragStart() {
        if (functionDragStart != "") SendMessage(functionDragStart);
    }
    void OnDrag() {
        if (functionDrag != "") SendMessage(functionDrag);
    }

    public IEnumerator coroutinePressButton() {
        GetComponent<Animator>().Play("button");
		GetComponent<AudioSource>().Play();
		yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
        //D/ebug.Log("coroutinePressButton: " + functionPressButton);
        SendMessage(functionPressButton);
    }
    public IEnumerator coroutinePressButtonTransition() {
		GetComponent<Animator>().Play("button");
		GetComponent<AudioSource>().Play();
		yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
        GameObject.Find("panel back transition").GetComponent<Animator>().Play("back transition enabled");
        //GameObject.Find("back transition").GetComponent<Animator> ().Play ("back transition exit");
        //yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
       	//D/ebug.Log("functionPressButtonTransition: " + functionPressButtonTransition);
        SendMessage(functionPressButtonTransition);
        //yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
    }

    void buttonOpenBooster() {
        transform.parent.FindChild("items/booster").gameObject.SetActive(false);
        transform.parent.FindChild("items/booster").gameObject.SetActive(true);
    }

	public void checkTutorialBuy() {
		//Debug.Log("checkTutorialBuy: " + name + ", step: " + ctrProgressClass.progress["tutorialBuy"]);
        if (GetComponent<UIToggle>() != null) Debug.Log(GetComponent<UIToggle>().value);
        //если хватает монет и не проходил туториал, показываем hand
        if (name == "button market" && ctrProgressClass.progress ["coins"] >= 400 && ctrProgressClass.progress ["tutorialBuy"] == 0) {
			transform.GetChild (0).gameObject.SetActive (true);
		} else if (name == "booster_white_1" && ctrProgressClass.progress["coins"] >= 400 && ctrProgressClass.progress["tutorialBuy"] <= 1)
	    {
	        transform.GetChild(0).gameObject.SetActive(true);
	    }
        //else if (name == "tab boosters" && ctrProgressClass.progress["boostersWhite"] <= 1)
        //   GetComponent<UIToggle>().value = true;

        /*
                else if (name == "tab boosters" && ctrProgressClass.progress["boostersWhite"] >= 1 &&
	             ctrProgressClass.progress["tutorialBuy"] <= 2 && !GetComponent<UIToggle>().value)
	    {
	        transform.GetChild(0).gameObject.SetActive(true);
	    } else if  (name == "boostersWhite" && ctrProgressClass.progress ["boostersWhite"] >= 1 && ctrProgressClass.progress ["tutorialBuy"] <= 3 ) {
			transform.GetChild (1).gameObject.SetActive (true);
		}
        */
        else if (name == "label coins")
        {
            GetComponent<UILabel>().text = ctrProgressClass.progress["coins"].ToString();
        }
        

    }

	void clickTutorialBuy() {
        Debug.Log("clickTutorialBuy");
        if (transform.FindChild("hand").gameObject.activeSelf) {
            Debug.Log(name);
            transform.FindChild("hand").gameObject.SetActive (false);
            if (name == "button market")
            {
                ctrProgressClass.progress ["tutorialBuy"] = 1;
                marketClass.instance.transform.GetChild(2).GetChild(0).GetComponent<UIToggle>().Set(true);
                marketClass.instance.transform.GetChild(2).GetChild(1).GetComponent<UIToggle>().Set(false);
                marketClass.instance.transform.GetChild(2).GetChild(2).GetComponent<UIToggle>().Set(false);
                marketClass.instance.transform.GetChild(2).GetChild(3).GetComponent<UIToggle>().Set(false);
            }
            if (name == "booster_white_1")
            {
                ctrProgressClass.progress ["tutorialBuy"] = 2;
                ctrAnalyticsClass.sendEvent("Tutorial", new Dictionary<string, string> { { "name", "buy booster" } });
                //tutorial
                //marketClass.instance.transform.GetChild(1).SendMessage("checkTutorialBuy");

            }
            if (name == "tab inventory")
            {
                ctrProgressClass.progress["tutorialBuy"] = 3;
            }
            //if (transform.parent.name == "open booster")  ctrProgressClass.progress ["tutorialBuy"] = 3;
            if (name == "boostersWhite")
            {
                ctrProgressClass.progress["tutorialBuy"] = 4;
            }
            
            ctrProgressClass.saveProgress ();
			//if (name == "booster 1")
			//	GameObject.Find ("preview icon/button/open booster").SendMessage ("checkTutorialBuy");
		}
	}

    void buyCardForCoins() {
       
        int amount = 0;
        int cost = 0;
        if (name == "booster_white_1")
        {
            amount = 1;
            cost = 400;
        }
        else
        {
            amount = 10;
            cost = 3200;
        }
        GetComponent<AudioSource> ().Play ();
		if (ctrProgressClass.progress["coins"] < cost) transform.GetChild(1).GetComponent<Animator>().Play("alpha disable");
        else {


            var nameItem = name;
            ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "detail 1", nameItem }, { "coins", (-cost).ToString() } });

            ctrProgressClass.progress["coins"] -= cost;
            ctrProgressClass.progress["boostersWhite"] += amount;
            ctrProgressClass.saveProgress();

            staticClass.setBoostersLabels();
    
            //change label coins
            GameObject.Find("/market/inventory/market menu/bars/coins/label coins").GetComponent<UILabel>().text = ctrProgressClass.progress["coins"].ToString();
            if (initLevelMenuClass.instance != null) initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress["coins"].ToString();

            //anim booster
            /*
            StartCoroutine(buyBoosterAnimEnd(false));

            if (name == "booster_white_1")
		        marketClass.instance.iconBoosterAnim.transform.GetChild(0).gameObject.SetActive(true);
		    else
                marketClass.instance.iconBoosterAnim.transform.GetChild(4).gameObject.SetActive(true);

            marketClass.instance.iconBoosterAnim.GetComponent<Animator>().Play("booster buy");
            StartCoroutine(buyBoosterAnimEnd(true));
            */
            mBoosterClass.instance.itemName = name;
            mBoosterClass.instance.transform.parent.parent.gameObject.SetActive(true);

        }
    }

    /*
    public IEnumerator buyBoosterAnimEnd(bool flag)
    {
        if (flag) yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
        for (int i = 0; i < 8; i++)
        {
            //off all
            marketClass.instance.iconBoosterAnim.transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return null;
    }
    */
    public IEnumerator CoroutineLoadLevel() {
		if (!staticClass.sceneLoading) {
            Time.timeScale = 0;
            Debug.Log("Time.timeScale: " + Time.timeScale);
            bool sceneCanLoad = true;

            Debug.Log(name);
            
            if (name == "button play" || name == "button play 0" || name == "button play 1" || name == "level" || name == "button next level") {
                if ((name == "button play 0" || name == "button play 1") && transform.GetChild(1).name == "icon restart" &&
                    transform.GetChild(1).gameObject.activeSelf)
                {
                    
                } else 
                if (!lsEnergyClass.checkLoadLevelEnergy ()) {
					yield return StartCoroutine (staticClass.waitForRealTime (100F));
				}
			}
            //showAd в конце уровня, рестарте, выходе в меню с уровня
		    if (name == "button play" || name == "button play 0" || name == "button play 1" || name.Substring(0, 5) == "level" || name == "restart" || name == "button next" || name == "button next level")
		    {
		        if (ctrAdClass.instance != null)
		        {
		            var ad = ctrAdClass.instance.ShowLevelAd(name);
		            if (ad && name != "restart")
		            {
		                staticClass.adLevelCounter++;
		                if (staticClass.adLevelCounter >= 5)
		                {
                            staticClass.needShowAdTiredMenu = true;
		                    for (int i = 0; i < 15; i++)
		                    {
                                yield return StartCoroutine(staticClass.waitForRealTime(1.0F));
                                if (!staticClass.needShowAdTiredMenu) break;
                            }
                            yield return StartCoroutine(staticClass.waitForRealTime(2.0F));
                            Debug.Log("ad tired menu exit");



                        }
                    }
                    
                }
		    }
            if (name == "start level menu") if (GameObject.Find("berry") != null && GameObject.Find("berry").GetComponent<gBerryClass>() != null) GameObject.Find("berry").GetComponent<gBerryClass>().endLevel("lost");
            if (name == "restart")
		    {
		        if (GameObject.Find("berry") != null && GameObject.Find("berry").GetComponent<gBerryClass>() != null)
		        {
                    if (!staticClass.sendPlayLevelStats)  GameObject.Find("berry").GetComponent<gBerryClass>().endLevel("restart");
		            staticClass.sendPlayLevelStats = false;

		        }
		    }
		    Application.backgroundLoadingPriority = ThreadPriority.High;
			AsyncOperation async = new AsyncOperation ();
			staticClass.scenePrev = SceneManager.GetActiveScene ().name;
            staticClass.sceneLoading = true;
            //if (Application.targetFrameRate != -1) Application.targetFrameRate = -1;

		    var sceneNeedLoad = "";

            if (name == "start level menu")
		    {
		        if (ctrProgressClass.progress["lastLevel"] == 0)
		        {
                    Debug.Log("lastLevel == 0");
                    staticClass.scenePrev = "level menu";
		            sceneNeedLoad = "level1";
                    //async = SceneManager.LoadSceneAsync("level1");
		        }
		        else
		        {
                    sceneNeedLoad = "level menu";

                    //async = SceneManager.LoadSceneAsync("level menu");
		        }

		    }
		    else if (name == "button back")
                //async = SceneManager.LoadSceneAsync("menu");
                sceneNeedLoad = "menu";


            if (name == "restart") {
				//async = SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
                sceneNeedLoad = SceneManager.GetActiveScene().name;

            }
            else if (name == "restart 1")
            {
                initLevelMenuClass.levelDemands = 0;
                //async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
                sceneNeedLoad = SceneManager.GetActiveScene().name;


            }
            else if (name == "restart 2") {
				initLevelMenuClass.levelDemands = 1;
				//async = SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);
                sceneNeedLoad = SceneManager.GetActiveScene().name;

            }
            else if (name == "button next") {
                if (staticClass.scenePrev.Substring(0, 5) == "level")
                {
                    //rate us
                    Debug.Log("staticClass.rateUsLast: " + staticClass.rateUsLast);
                    Debug.Log("int.Parse(staticClass.scenePrev.Substring(5): " + int.Parse(staticClass.scenePrev.Substring(5)));
                    if (staticClass.rateUsLast < int.Parse(staticClass.scenePrev.Substring(5)) &&
                        staticClass.rateUsLevels.Contains(int.Parse(staticClass.scenePrev.Substring(5)) ))
                    {
                        rateUsMenuEnable();
                        sceneCanLoad = false;
                        staticClass.rateUsSceneNext = "level menu";
                    }
                    else
                    {
                        sceneNeedLoad = "level menu";
                        //async = SceneManager.LoadSceneAsync("level menu");
                    }

                }
                else
                    //async = SceneManager.LoadSceneAsync ("level menu");
                    sceneNeedLoad = "level menu";
            } else if (name == "button play") {
				//если открыта вкладка основного прохождения
				if (transform.parent.GetChild(0).GetChild(2).gameObject.activeSelf ) 
					initLevelMenuClass.levelDemands = 0;
				else 
					initLevelMenuClass.levelDemands = 1;


				if (Application.CanStreamedLevelBeLoaded ("level" + ctrProgressClass.progress ["currentLevel"]))
                    sceneNeedLoad = "level" + ctrProgressClass.progress["currentLevel"];
                    //async = SceneManager.LoadSceneAsync ("level" + ctrProgressClass.progress ["currentLevel"]);
				else
                    sceneNeedLoad = "level menu";
                    //async = SceneManager.LoadSceneAsync ("level menu");

				
			} else if (name == "button play 0") {

                
                if (Application.CanStreamedLevelBeLoaded ("level" + ctrProgressClass.progress ["currentLevel"]))
                    sceneNeedLoad = "level" + ctrProgressClass.progress["currentLevel"]; //async = SceneManager.LoadSceneAsync ("level" + ctrProgressClass.progress ["currentLevel"]);
				else
                    sceneNeedLoad = "level menu";

                if (sceneNeedLoad == SceneManager.GetActiveScene().name && initLevelMenuClass.levelDemands == 1)

                    staticClass.levelRestartedCount = -1;

                initLevelMenuClass.levelDemands = 0;

            }
            else if (name == "button play 1") {

				if (Application.CanStreamedLevelBeLoaded ("level" + ctrProgressClass.progress ["currentLevel"]))
                    sceneNeedLoad = "level" + ctrProgressClass.progress["currentLevel"];
                    //async = SceneManager.LoadSceneAsync ("level" + ctrProgressClass.progress ["currentLevel"]);
                else
                    sceneNeedLoad = "level menu"; //async = SceneManager.LoadSceneAsync ("level menu");
                if (sceneNeedLoad == SceneManager.GetActiveScene().name && initLevelMenuClass.levelDemands == 0)

                    staticClass.levelRestartedCount = -1;

                initLevelMenuClass.levelDemands = 1;

            }
            else if (name.Substring (0, 5) == "level") {
                //добавить проверку на гемс
                if (ctrProgressClass.progress["lastLevel"] >= Convert.ToInt32(name.Substring(5)) - 1)
			    {
                    //async = SceneManager.LoadSceneAsync("level" + Convert.ToInt32(name.Substring(5)));
                    sceneNeedLoad = "level" + Convert.ToInt32(name.Substring(5));
                }
			} else if (name == "button next level")
			{
                var nextLevelNumber = int.Parse(staticClass.scenePrev.Substring(5)) + 1;

                //next level load
           
			    
                    if ((staticClass.levelBlocks.ContainsKey(nextLevelNumber) && staticClass.levelBlocks[nextLevelNumber] <= ctrProgressClass.progress["gems"])
			    
			        ||
			        !staticClass.levelBlocks.ContainsKey(nextLevelNumber)
			            || ctrProgressClass.progress["lastLevel"] >= nextLevelNumber)
                {
                    //rate us
                    initLevelMenuClass.levelDemands = 0;
                    if (staticClass.rateUsLast < nextLevelNumber - 1 && staticClass.rateUsLevels.Contains(nextLevelNumber - 1))
                    {
                        rateUsMenuEnable();
                        sceneCanLoad = false;
                        staticClass.rateUsSceneNext = "level" + nextLevelNumber;
                    } else
                        sceneNeedLoad = "level" + nextLevelNumber;
                    //async = SceneManager.LoadSceneAsync("level" + nextLevelNumber);
			    }
                //notGemsForLevel
                else
                {
                    sceneNeedLoad = "level menu";

                    //async = SceneManager.LoadSceneAsync("level menu");
                    staticClass.notGemsForLevel = true;
                    }
                

            }else if (name == "exit rate us menu")
		    {
                Debug.Log(name);
		        staticClass.rateUsLast = ctrProgressClass.progress["lastLevel"];
                sceneNeedLoad = staticClass.rateUsSceneNext;
                //async = SceneManager.LoadSceneAsync(staticClass.rateUsSceneNext);
            }
            else if (name == "button rate us")
            {
                Debug.Log(name);
                staticClass.rateUsLast = ctrProgressClass.progress["lastLevel"];
                sceneNeedLoad = staticClass.rateUsSceneNext;
                //async = SceneManager.LoadSceneAsync(staticClass.rateUsSceneNext);
            }
            else if (name == "restart after dream")
            {
                sceneNeedLoad = SceneManager.GetActiveScene().name;
                //async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

            }

            //сбрасываем энергию
            Debug.Log("prev: " + staticClass.scenePrev);
            Debug.Log("sceneNeedLoad: " + sceneNeedLoad);
            if (staticClass.scenePrev != sceneNeedLoad) lsEnergyClass.energyTake = false;

            if (sceneNeedLoad != "") async = SceneManager.LoadSceneAsync(sceneNeedLoad);


            if (sceneCanLoad)
		    {
		        async.allowSceneActivation = false;
		        yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
		        async.allowSceneActivation = true;
		        yield return async;
		    }

		}

    }

    void pressMarketItem(bool isPressed) {
        if (!isPressed) {
            GetComponent<Animator>().Play("button");
        }
        //off for tests
        //if (onFinished != null) EventDelegate.Execute(onFinished);

        //old code
        //transform.parent.GetComponent<UIScrollView>().Press(isPressed);
        //if (!isPressed) {
        //	GetComponent<UIPlayAnimation>().enabled = true;
        //	GetComponent<UIButtonScale>().enabled = true;
        //	enableDrag = false;
        //}
        //timeSinceTouch = Time.time;
    }


    void OnDestroy() {
    }


    void initSettings() {
        Debug.Log("initSettings");
        if (name == "music button") {
			if (ctrProgressClass.progress ["music"] == 1) {
				transform.GetChild (0).gameObject.SetActive (false);
			} else {
				transform.GetChild (0).gameObject.SetActive (true);
				GameObject.Find ("music").GetComponent<AudioSource> ().mute = true;
			}

            //fb and vk

            //fb on, vk off
            if (ctrProgressClass.progress["fb"] == 1 && Facebook.Unity.FB.IsLoggedIn)
            {
                transform.parent.parent.GetChild(1).gameObject.SetActive(true);
                transform.parent.parent.GetChild(2).gameObject.SetActive(false);
                transform.parent.parent.GetChild(3).gameObject.SetActive(false);
            }
            //fb off, vk on
            if (ctrProgressClass.progress["vk"] == 1 && com.playGenesis.VkUnityPlugin.VkApi.VkApiInstance.IsUserLoggedIn)
            {
                transform.parent.parent.GetChild(2).gameObject.SetActive(true);
                transform.parent.parent.GetChild(1).gameObject.SetActive(false);
                transform.parent.parent.GetChild(3).gameObject.SetActive(false);

            }
            //ok on
            if (ctrProgressClass.progress["ok"] == 1 && Odnoklassniki.OK.IsLoggedIn)
            {
                transform.parent.parent.GetChild(2).gameObject.SetActive(false);
                transform.parent.parent.GetChild(1).gameObject.SetActive(false);
                transform.parent.parent.GetChild(3).gameObject.SetActive(true);
            }

        }
        if (name == "sound button") {
			if (ctrProgressClass.progress ["sound"] == 1)
				transform.GetChild (0).gameObject.SetActive (false);
			else {
				transform.GetChild (0).gameObject.SetActive (true);
				AudioListener.pause = true;
			}
        }
		if (name == "Russian") {
			if ((ctrProgressClass.progress ["language"] == 0 && Application.systemLanguage.ToString () == "Russian") || ctrProgressClass.progress ["language"] == 2) {
				transform.GetChild (0).gameObject.SetActive (true);
			} 
		}
		if (name == "English") {
			if (ctrProgressClass.progress ["language"] == 1) {
				transform.GetChild (0).gameObject.SetActive (true);
			} 
		}
		if (name == "camera button") {
            /*
            if (!Everyplay.IsRecordingSupported () || ctrProgressClass.progress ["everyplay"] == 0) {
				transform.GetChild(0).gameObject.SetActive (true);
				transform.GetChild(1).gameObject.SetActive (false);
			}
            */
            
		}

    }

    void clickSound() {
        if (ctrProgressClass.progress["sound"] == 0) {
            ctrProgressClass.progress["sound"] = 1;
			ctrProgressClass.saveProgress();
            transform.GetChild(0).gameObject.SetActive(false);
			AudioListener.pause = false;


        }
        else {
            ctrProgressClass.progress["sound"] = 0;
            ctrProgressClass.saveProgress();
            transform.GetChild(0).gameObject.SetActive(true);
			AudioListener.pause = true;


        }
    }

    void clickMusic() {
        if (ctrProgressClass.progress["music"] == 0) {
			GameObject.Find ("music").GetComponent<AudioSource> ().mute = false;
            ctrProgressClass.progress["music"] = 1;
            ctrProgressClass.saveProgress();
			transform.GetChild (0).gameObject.SetActive(false);
        }
        else {
			GameObject.Find("music").GetComponent<AudioSource>().mute = true;
            ctrProgressClass.progress["music"] = 0;
            ctrProgressClass.saveProgress();
			transform.GetChild(0).gameObject.SetActive(true);
        }
    }

	void clickSettingCamera() {
/*
        if (ctrProgressClass.progress["everyplay"] == 0) {

            if (Everyplay.IsRecordingSupported ()) {
				ctrProgressClass.progress ["everyplay"] = 1;
				ctrProgressClass.saveProgress ();
				transform.GetChild (0).gameObject.SetActive (false);
				transform.GetChild (1).gameObject.SetActive (true);
			}

		}
		else {
			ctrProgressClass.progress["everyplay"] = 0;
			ctrProgressClass.saveProgress();
			transform.GetChild (0).gameObject.SetActive (true);
			transform.GetChild (1).gameObject.SetActive (false);


		}
        */
	}


    void resetProgress() {
		ctrProgressClass.resetProgress (name);
		SceneManager.LoadScene(0);
   }

    void loadLevel() {
		StartCoroutine(CoroutineLoadLevel());
    }



    void openMenu() {
        Debug.Log("openMenu: " + name);
        GameObject menu = null;
        if (name == "button market") {
			//marketClass.instance.transform.position = new Vector3 (0, 0, -1);
			marketClass.instance.gameObject.SetActive (true);
			staticClass.isTimePlay = Time.timeScale;
			Time.timeScale = 0;
            Debug.Log("Time.timeScale: " + Time.timeScale);

            if (lsSaleClass.timerStartSale < DateTime.Now) marketClass.instance.transform.GetChild(0).GetComponent<UIToggle>().Set(true);

            /*
            } else if (name == "next level menu") {
                menu = GameObject.Find ("level menu");
                menu.SetActive (false);
                menu = transform.parent.parent.GetChild (3).gameObject;
                menu.SetActive (true);
            } else if (name == "prev level menu") {
                menu = GameObject.Find ("level menu 2");
                menu.SetActive (false);
                menu = transform.parent.parent.GetChild (2).gameObject;
                menu.SetActive (true);
            */
        }
        else if (name == "pause") {
			menu = transform.parent.GetChild (1).gameObject;
			menu.SetActive (true);
            Debug.Log(menu.GetComponent<lsLevelMenuClass>());
            menu.GetComponent<lsLevelMenuClass>().setContent2();
            if (initLevelMenuClass.levelDemands == 0) menu.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
            else menu.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
            staticClass.isTimePlay = Time.timeScale;
			Time.timeScale = 0;
            Debug.Log("Time.timeScale: " + Time.timeScale);

        }
        else if (name == "play") {
			menu = GameObject.Find ("pause menu");
			menu.SetActive (false);
			Time.timeScale = staticClass.isTimePlay;
            Debug.Log("Time.timeScale: " + Time.timeScale);


        }
        else if (name == "exit energy menu")
			GameObject.Find ("energyLabel").SendMessage ("stopCoroutineEnergyMenu");
		else if (name == "button settings")
        {
            Transform go = GameObject.Find("settings folder").transform.GetChild(0);
            go.gameObject.SetActive(true);
            //anim coins
            go.GetChild(2).GetChild(4).gameObject.SetActive(false);
            go.GetChild(2).GetChild(5).gameObject.SetActive(false);
            if (ctrProgressClass.progress["rewardRepostOK"] == 1) go.GetChild(3).GetChild(0).GetChild(3).gameObject.SetActive(false);
        }
        else if (transform.parent.gameObject.name == "open booster")
        {
            Debug.Log("open booster");
            Debug.Log(name + ": " + ctrProgressClass.progress[name]);
            //for test
            
            if (ctrProgressClass.progress[name] > 0)
            {
                //отключаем все спрайты бустера
                for (int i = 0; i < 4; i++)
                {
                    marketClass.instance.openBoosterMenu.transform.GetChild(1)
                        .GetChild(1)
                        .GetChild(0)
                        .GetChild(0)
                        .GetChild(i)
                        .gameObject.SetActive(false);
                }
                //включаем , какой открываем
                marketClass.instance.openBoosterMenu.transform.GetChild(1)
                    .GetChild(1)
                    .GetChild(0)
                    .GetChild(0)
                    .FindChild(name)
                    .gameObject.SetActive(true);

                marketClass.instance.openBoosterMenu.SetActive(true);
                //marketClass.instance.boosterMenu.SetActive(false);

                //if (Everyplay.IsRecordingSupported () && !Everyplay.IsRecording() && ctrProgressClass.progress["everyplay"] == 1)
                //	Everyplay.StartRecording ();

            }

            //} else if (name == "coins ad") {
            //	GameObject.Find ("root/static").transform.FindChild ("coins menu").gameObject.SetActive (true);


        }
        else if (name == "get booster")
        {
            transform.parent.parent.parent.GetChild(0).gameObject.SendMessage("OnClick");
        }
        else if (name == "reset progress")
        {
            transform.parent.parent.parent.GetChild(1).gameObject.SetActive(true);
        }
        else if (name == "button sale")
        {
            if (ctrProgressClass.progress["tutorialSale"] < 2) ctrProgressClass.progress["tutorialSale"] = 2;
            GameObject.Find("/root/static/button sale/hand").SetActive(false);
            ctrProgressClass.saveProgress();
            //sale menu
            initLevelMenuClass.instance.saleMenu.SetActive(true);
        }
        else if (name == "ad coins")
        {
            //coins menu
            initLevelMenuClass.instance.coinsMenu.SetActive(true);
        }
        else if (name == "group button")
        {
            //join group menu 1
            transform.parent.parent.GetChild(5).gameObject.SetActive(true);
        }
        else if (name == "group button 2")
        {
            //join group menu 2
            transform.parent.parent.GetChild(6).gameObject.SetActive(true);
        }
        else if (name == "share button")
        {
            //share menu
            transform.parent.parent.GetChild(7).gameObject.SetActive(true);
        }
    }
    public void closeMenu() {
        StartCoroutine(coroutineCloseMenu());
    }

    public IEnumerator coroutineCloseMenu()
    {
        yield return new WaitForSeconds(0F);
        GameObject menu = null;
        if (name == "button exit level menu")
        {
            menu = GameObject.Find("level menu");
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
            /*
            } else if (name == "next level menu") {
                menu = GameObject.Find ("level menu");
                menu.SetActive (false);
                menu = transform.parent.parent.GetChild (3).gameObject;
                menu.SetActive (true);
            } else if (name == "prev level menu") {
                menu = GameObject.Find ("level menu 2");
                menu.SetActive (false);
                menu = transform.parent.parent.GetChild (2).gameObject;
                menu.SetActive (true);
            } else if (name == "pause") {
                menu = transform.parent.GetChild (1).gameObject;
                menu.SetActive (true);
                staticClass.isTimePlay = Time.timeScale;
                Time.timeScale = 0;
            */
        }
        else if (name == "play")
        {
            menu = GameObject.Find("pause menu");
            yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
            menu.SetActive(false);
            Time.timeScale = staticClass.isTimePlay;
            Debug.Log("Time.timeScale: " + Time.timeScale);

            //if (gYetiClass.yetiState == "")
            //Time.timeScale = 1;

        }
        else if (name == "exit energy menu")
        {
            menu = GameObject.Find("energy menu");
            Debug.Log(menu.name);
            Debug.Log(menu.transform.parent.name);
            GameObject.Find("energy menu").transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
            GameObject.Find("energy").SendMessage("stopCoroutineEnergyMenu");
            menu.SetActive(false);
            /*
            } else if (name == "exit thanks menu") {
                menu = GameObject.Find ("root/Camera/UI Root/thanks menu");
                if (menu != null) {
                    menu.transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
                    yield return new WaitForSeconds (0.2F);
                    Destroy (menu);
                } else {
                    menu = marketClass.instance.thanksMenu;
                    menu.transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
                    yield return new WaitForSeconds (0.2F);
                    menu.SetActive (false);
                }
            */
        }
        else if (name == "button settings exit")
        {
            menu = transform.parent.parent.gameObject;
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);

        }
        else if (name == "button market exit")
        {
            //marketClass.instance.transform.position = new Vector3 (0, 0, -10000);
            marketClass.instance.gameObject.SetActive(false);
            //marketClass.instance.camera.SetActive (false);
            Time.timeScale = staticClass.isTimePlay;
            Debug.Log("Time.timeScale: " + Time.timeScale);

        }
        else if (name == "exit open booster menu")
        {
            Debug.Log(name);
            mBoosterClass.instance.transform.parent.parent.gameObject.SetActive(false);

            Debug.Log(staticClass.getBoosterForOK);
            if (staticClass.getBoosterForOK)
            {
                ctrProgressClass.progress["boostersWhite"]++;
                ctrProgressClass.progress["rewardLogin"] = 1;
                mBoosterClass.instance.itemName = "booster_white_1";
                mBoosterClass.instance.transform.parent.parent.gameObject.SetActive(true);
                ctrProgressClass.saveProgress();
                
            }

        }
        else if (name == "exit gift menu")
        {
            transform.parent.gameObject.SetActive(false);
        }
        else if (name == "exit daily menu")
        {
            transform.parent.gameObject.SetActive(false);
        }
        else if (name == "exit energy ad menu")
        {
            GameObject.Find("ad dont ready menu").transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
            GameObject.Find("ad dont ready menu").SetActive(false);
        }
        else if (name == "exit coins menu")
        {
            GameObject.Find("coins menu").transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            GameObject.Find("coins menu").SetActive(false);
        }
        else if (name == "button exit close menu")
        {
            menu = GameObject.Find("close menu");
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
        else if (name == "button exit reset progress menu")
        {
            menu = transform.parent.parent.gameObject;
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
        else if (name == "exit sale menu")
        {
            menu = GameObject.Find("sale menu");
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
        else if (name == "exit reward menu")
        {
            menu = GameObject.Find("root/static/reward menu");
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
    
        else if (name == "exit unlock chapter menu")
        {
            menu = initLevelMenuClass.instance.unlockСhapterMenu;
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
        else if (name == "exit reward for cards menu")
        {
            menu = initLevelMenuClass.instance.rewardForCardsMenu;
            menu.transform.GetChild(0).GetComponent<Animator>().Play("menu exit");
            yield return new WaitForSeconds(0.2F);
            menu.SetActive(false);
        }
        else if (name == "exit invite friends menu")
        {
            menu = GameObject.Find("/root/static/level menu/vk_ok/invite friends/");
            if (menu != null) menu.SetActive(false);
            menu = GameObject.Find("/root/static/level menu/vk_ok/invite friends ok/");
            if (menu != null) menu.SetActive(false);
            GameObject.Find("/root/static/level menu/button exit level menu").GetComponent<SpriteRenderer>().sortingOrder = 137;
        }
        else if (name == "exit dream menu")
        {
            GameObject.Find("default level/gui/dream menu").transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (name == "exit disable level menu")
        {
            initLevelMenuClass.instance.disableLevelMenu.SetActive(false);
        }
        else if (name == "join group menu exit")
        {
            GameObject.Find("/settings folder/settings/join group menu").SetActive(false);

        }
        else if (name == "join group menu 2 exit")
        {
            GameObject.Find("/settings folder/settings/join group menu 2").SetActive(false);

        }
        else if (name == "share menu exit")
        {
            GameObject.Find("/settings folder/settings/share menu").SetActive(false);
        }
    }

    //public IEnumerator CoroutineCloseMenu(){

    //}

    void selectLanguage() {
        Localization.language = name;
        var logo = GameObject.Find("/UI Root (2D)/root/logo");
        var berry = GameObject.Find("/UI Root (2D)/root/berry");
        if (name == "Russian") {
			ctrProgressClass.progress ["language"] = 2;
			transform.parent.GetChild (0).GetChild (0).gameObject.SetActive (false);
            logo.transform.GetChild(0).gameObject.SetActive(false);
            logo.transform.GetChild(1).gameObject.SetActive(true);
            berry.transform.localPosition = new Vector3(330, -140, 0);

        }
        else {
			transform.parent.GetChild (1).GetChild (0).gameObject.SetActive (false);
			ctrProgressClass.progress ["language"] = 1;
            logo.transform.GetChild(1).gameObject.SetActive(false);
            logo.transform.GetChild(0).gameObject.SetActive(true);
            berry.transform.localPosition = new Vector3(306, -288, 0);
        }
        transform.GetChild (0).gameObject.SetActive (true);
		ctrProgressClass.saveProgress ();
	}


    void clickBonusesArrow() {
        if (name == "arrow right") {
            gameObject.SetActive(false);
            GameObject.Find("bonuses/tween").transform.GetChild(1).gameObject.SetActive(true);
			staticClass.bonusesView = true;

            //off hand, if tutorial bonus
            if (ctrProgressClass.progress["tutorialBonus"] == 0) if (GameObject.Find("/default level/gui/tutorial bonus(Clone)/hand") != null )
                    GameObject.Find("/default level/gui/tutorial bonus(Clone)/hand").SetActive(false);
        }
        else {
            gameObject.SetActive(false);
            GameObject.Find("bonuses/tween").transform.GetChild(0).gameObject.SetActive(true);
			staticClass.bonusesView = false;
		}

    }

	void socialConnect() {
		ctrFbKiiClass.instance.connect (name);
	}
    void socialInvite()
    {
        ctrFbKiiClass.instance.invite(name);
    }
    //for vk and ok
    void vkInviteFriend()
    {
        //ctrFbKiiClass.instance.inviteFriend("vk", transform.parent.name.Substring(0, transform.parent.name.Length - 7));

        if (ctrProgressClass.progress["vk"] == 1)
            ctrFbKiiClass.instance.inviteFriend("vk", transform.parent.name);
        if (ctrProgressClass.progress["ok"] == 1)
            ctrFbKiiClass.instance.inviteFriend("ok", transform.parent.name);
    }


    void ShowRewardedAd() {
		ctrAdClass.adStarted = name;
       // if (ctrAdClass.instance == null) ctrAdClass.

        if (ctrAdClass.instance != null) ctrAdClass.instance.ShowRewardedAd ();
	}

    void restoreEnergy()
    {
        //GameObject.Find("energy").GetComponent<lsEnergyClass>().restoreEnergy();
    }

    void buyEnergy()
    {
        GameObject.Find("energy").GetComponent<lsEnergyClass>().buyEnergy();
    }

    void dreamClick()
    {
        Debug.Log("dream click");
        Debug.Log(SceneManager.GetActiveScene().name);
        if (staticClass.levelRestartedCount == 2 && ctrProgressClass.progress["tutorialDream"] == 0)
        {
            ctrProgressClass.progress[SceneManager.GetActiveScene().name + "_dream"] = 3;
            ctrProgressClass.progress["tutorialDream"] = ctrProgressClass.progress["currentLevel"];
            ctrAnalyticsClass.sendEvent("Tutorial", new Dictionary<string, string> { { "name", "use dream" } });
            ctrProgressClass.saveProgress();
        }
        var p = ctrProgressClass.progress[SceneManager.GetActiveScene().name + "_dream"];
        GetComponent<AudioSource>().Play();

        //если уже есть подсказка
        if ((p == 1 || p == 3) && initLevelMenuClass.levelDemands == 0 ||
            (p == 2 || p == 3) && initLevelMenuClass.levelDemands == 1)
        {

            gHintClass.initDream();
        }
        //если нет, то смотрим сначала видео
        else
        {
            //for publish ShowRewardedAd
            if (name == "dream")
                GameObject.Find("default level/gui/dream menu").transform.GetChild(0).gameObject.SetActive(true);
            else
            {
                GameObject.Find("default level/gui/dream menu").transform.GetChild(0).gameObject.SetActive(false);
                ShowRewardedAd();
            } //gHintClass.initDream();
        }
    }

    public void buyChapter()
    {
        marketClass.buyChapter();
    }

    public void rateUs()
    {
        Debug.Log("rate us click: " + transform.GetChild(3).GetComponent<UILabel>().text);
        if (int.Parse( transform.GetChild(3).GetComponent<UILabel>().text) >= 4) 
        
#if UNITY_ANDROID
                Application.OpenURL("market://details?id=com.evogames.feedthespider");
#elif UNITY_IPHONE
             Application.OpenURL("itms-apps://itunes.apple.com/app/id1194487188");
        //??? https://itunes.apple.com/us/app/feed-the-spider/id1194487188?l=ru&ls=1&mt=8
#endif
    }


    private void rateUsMenuEnable()
    {
        Debug.Log("rate us menu enable");
        var rateUs =
            GameObject.Find("/default level/gui/complete menu").GetComponent<lsLevelMenuClass>().rateUsMenu;
        var rateUsGO = Instantiate(rateUs);
        rateUsGO.transform.parent = GameObject.Find("/default level/gui/panel back transition").transform;
        rateUsGO.transform.localScale = new Vector3(1, 1, 1);
        rateUsGO.transform.localPosition = new Vector3(0, -62, 0);
        //disable loading
        rateUsGO.transform.parent.GetChild(1).localScale = Vector3.zero;

        staticClass.sceneLoading = false;
    }

    public void rateUsStarClick()
    {
        Debug.Log("rateUsStarClick: " + name);
        //enable/disable stars
        for (int i = 0; i < transform.parent.childCount; i ++)
        {
            if (int.Parse(name) >= int .Parse(transform.parent.GetChild(i).name)) transform.parent.GetChild(i).GetChild(1).gameObject.SetActive(true);
            else transform.parent.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
        //enable rate us button
        transform.parent.parent.GetChild(1).GetComponent<Collider>().enabled = true;
        transform.parent.parent.GetChild(1).GetChild(2).gameObject.SetActive(false);
        transform.parent.parent.GetChild(1).GetChild(3).GetComponent<UILabel>().text = name;

        //disable hand
        transform.parent.parent.parent.GetChild(5).gameObject.SetActive(false);

    }




    public void clickJoinGroup()
    {
        string url = "groups.join?group_id=139520787";
        if (name == "group button 2") url = "groups.join?group_id=78616012";
        Debug.Log("clickJoinGroup: " + url);
        ctrFbKiiClass.instance.clickJoinGroup(url, name);
    }

    public void clickLogout()
    {
        Debug.Log("clickLogout");
        ctrFbKiiClass.instance.clickLogout();
    }

    public void clickTabCompleteMenu()
    {
        GameObject.Find("/default level/gui/complete menu").GetComponent<lsLevelMenuClass>().clickTabCompleteMenu(name);
    }

    public void closeCompleteMenu()
    {
        GameObject.Find("/default level/gui/complete menu/camera for menu").SetActive(false);
    }
    public void okGroupUrl()
    {
        Application.OpenURL("https://ok.ru/group/53192970862736");
    }
    public void okGroupInfoUrl()
    {
        Application.OpenURL("https://ok.ru/group/53192970862736/topic/66736745848976");
    }

    public void shareClick()
    {
        Debug.Log("clickLogout");
        ctrFbKiiClass.instance.share();
    }
}
