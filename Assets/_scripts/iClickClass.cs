using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
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
		yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
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

	void checkTutorialBuy() {
		//если хватает монет и не проходил туториал, показываем hand
		if (name == "button market" && ctrProgressClass.progress ["coins"] >= 800 && ctrProgressClass.progress ["tutorialBuy"] == 0) {
			transform.GetChild (0).gameObject.SetActive (true);
		} else if (name == "booster 1" && ctrProgressClass.progress ["coins"] >= 800 && ctrProgressClass.progress ["boosters"] == 0 && ctrProgressClass.progress ["tutorialBuy"] <= 1) {
			transform.GetChild (0).gameObject.SetActive (true);
		} else if  (name == "open booster" && ctrProgressClass.progress ["boosters"] >= 1 && ctrProgressClass.progress ["tutorialBuy"] <= 2 ) {
			transform.GetChild (0).gameObject.SetActive (true);
		}

	}

	void clickTutorialBuy() {
		if (transform.GetChild (0).gameObject.activeSelf) {
			transform.GetChild (0).gameObject.SetActive (false);
			if (name == "button market")  ctrProgressClass.progress ["tutorialBuy"] = 1;
			if (name == "booster 1")  ctrProgressClass.progress ["tutorialBuy"] = 2;
			if (name == "open booster")  ctrProgressClass.progress ["tutorialBuy"] = 3;

			ctrProgressClass.saveProgress ();
			if (name == "booster 1")
				GameObject.Find ("preview icon/button/open booster").SendMessage ("checkTutorialBuy");
		}
	}

    void buyCardForCoins() {
        int amount = 1;
        int cost = 800;
		GetComponent<AudioSource> ().Play ();
		if (ctrProgressClass.progress["coins"] < cost) transform.GetChild(1).GetComponent<Animator>().Play("alpha disable");
        else {
			GoogleAnalyticsV4.instance.LogEvent("Purchase for coins", "purchase", "booster", 1);

			ctrProgressClass.progress["coins"] -= cost;
            ctrProgressClass.progress["boosters"] += amount;
            ctrProgressClass.saveProgress();
            marketClass.instance.boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();
			marketClass.instance.boostersLabel.GetComponent<AudioSource> ().Play ();
			marketClass.instance.boostersLabel.GetComponent<Animator> ().Play("button");
			marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem> ().Stop();
			marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem> ().Play();


        }
    }



    public IEnumerator CoroutineLoadLevel() {
		if (!staticClass.sceneLoading) {

			Time.timeScale = 0;

            //проверяем энергию при запуске уровня и списываем ее
            //раскомментить при publish game
            Debug.Log(name);
            
            if (name == "button play" || name == "button play 0" || name == "button play 1" || name == "level") {
				if (!lsEnergyClass.checkLoadLevelEnergy ()) {
					yield return StartCoroutine (staticClass.waitForRealTime (100F));
				}
			}
            
            Application.backgroundLoadingPriority = ThreadPriority.High;
			AsyncOperation async = new AsyncOperation ();
			staticClass.scenePrev = SceneManager.GetActiveScene ().name;
			staticClass.sceneLoading = true;
			//if (Application.targetFrameRate != -1) Application.targetFrameRate = -1;


				
			if (name == "start level menu") {
				if (ctrProgressClass.progress ["lastLevel"] == 0) {
					staticClass.scenePrev = "level menu";
					async = SceneManager.LoadSceneAsync ("level1");
				} else
					async = SceneManager.LoadSceneAsync ("level menu");
			} else if (name == "button back")
				async = SceneManager.LoadSceneAsync ("menu");


			if (name == "restart") {
				async = SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);

			}
            else if (name == "restart 1")
            {
                initLevelMenuClass.levelDemands = 0;
                async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

            } else if (name == "restart 2") {
				initLevelMenuClass.levelDemands = 1;
				async = SceneManager.LoadSceneAsync (SceneManager.GetActiveScene ().name);

			} else if (name == "button next") {

				async = SceneManager.LoadSceneAsync ("level menu");

			} else if (name == "button play") {
				//если открыта вкладка основного прохождения
				if (transform.parent.GetChild(0).GetChild(2).gameObject.activeSelf ) 
					initLevelMenuClass.levelDemands = 0;
				else 
					initLevelMenuClass.levelDemands = 1;


				if (Application.CanStreamedLevelBeLoaded ("level" + ctrProgressClass.progress ["currentLevel"]))
					async = SceneManager.LoadSceneAsync ("level" + ctrProgressClass.progress ["currentLevel"]);
				else
					async = SceneManager.LoadSceneAsync ("level menu");


				
			} else if (name == "button play 0") {

				initLevelMenuClass.levelDemands = 0;
				if (Application.CanStreamedLevelBeLoaded ("level" + ctrProgressClass.progress ["currentLevel"]))
					async = SceneManager.LoadSceneAsync ("level" + ctrProgressClass.progress ["currentLevel"]);
				else
					async = SceneManager.LoadSceneAsync ("level menu");
			
			} else if (name == "button play 1") {

				initLevelMenuClass.levelDemands = 1;
				if (Application.CanStreamedLevelBeLoaded ("level" + ctrProgressClass.progress ["currentLevel"]))
					async = SceneManager.LoadSceneAsync ("level" + ctrProgressClass.progress ["currentLevel"]);
				else
					async = SceneManager.LoadSceneAsync ("level menu");
			} else if (name.Substring (0, 5) == "level") {
				if (ctrProgressClass.progress ["lastLevel"] >= Convert.ToInt32 (name.Substring (5)) - 1)
					async = SceneManager.LoadSceneAsync ("level" + Convert.ToInt32 (name.Substring (5)));
			}
            Debug.Log(name + "1111");

            //сбрасываем энергию
            if (staticClass.scenePrev == "level menu") lsEnergyClass.energyTake = false;

            async.allowSceneActivation = false;
			yield return StartCoroutine (staticClass.waitForRealTime (0.5F));
			async.allowSceneActivation = true;
			yield return async;
		
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
        if (name == "music button") {
			if (ctrProgressClass.progress ["music"] == 1) {
				transform.GetChild (0).gameObject.SetActive (false);
			} else {
				transform.GetChild (0).gameObject.SetActive (true);
				GameObject.Find ("music").GetComponent<AudioSource> ().mute = true;
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
			if (!Everyplay.IsRecordingSupported () || ctrProgressClass.progress ["everyplay"] == 0) {
				transform.GetChild(0).gameObject.SetActive (true);
				transform.GetChild(1).gameObject.SetActive (false);
			}
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
	}


    void resetProgress() {
		ctrProgressClass.resetProgress (name);
		SceneManager.LoadScene(0);
   }

    void loadLevel() {
		StartCoroutine(CoroutineLoadLevel());
    }



    void openMenu() {
        //D/ebug.Log("openMenu: " + name);
        GameObject menu = null;
        //if (name == "button market") marketClass.instance.gameObject.SetActive(true);
		if (name == "button market") {
			//marketClass.instance.transform.position = new Vector3 (0, 0, -1);
			marketClass.instance.gameObject.SetActive (true);
			staticClass.isTimePlay = Time.timeScale;
			Time.timeScale = 0;
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
		} else if (name == "pause") {
			menu = transform.parent.GetChild (1).gameObject;
			menu.SetActive (true);
			staticClass.isTimePlay = Time.timeScale;
			Time.timeScale = 0;
		} else if (name == "play") {
			menu = GameObject.Find ("pause menu");
			menu.SetActive (false);
			Time.timeScale = staticClass.isTimePlay;


		} else if (name == "exit energy menu")
			GameObject.Find ("energyLabel").SendMessage ("stopCoroutineEnergyMenu");
		else if (name == "button settings")
			GameObject.Find ("settings folder").transform.GetChild (0).gameObject.SetActive (true);
		else if (transform.parent.gameObject.name == "open booster") {

            if (ctrProgressClass.progress [name] > 0) {

                //отключаем все спрайты бустера
                for (int i = 0; i < 4; i++)
			    {
			        marketClass.instance.openBoosterMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetChild(i).gameObject.SetActive(false);
			    }
                //включаем , какой открываем
                marketClass.instance.openBoosterMenu.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).FindChild(name).gameObject.SetActive(true);

                marketClass.instance.openBoosterMenu.SetActive(true);
                marketClass.instance.boosterMenu.SetActive(false);

                if (Everyplay.IsRecordingSupported () && !Everyplay.IsRecording() && ctrProgressClass.progress["everyplay"] == 1)
					Everyplay.StartRecording ();

			}

		} else if (name == "coins ad") {
			GameObject.Find ("root/static").transform.FindChild ("coins menu").gameObject.SetActive (true);


		} else if (name == "get booster") {
			transform.parent.parent.parent.GetChild (0).gameObject.SendMessage ("OnClick");
		} else if (name == "reset progress") {
			transform.parent.parent.parent.GetChild (1).gameObject.SetActive(true);
	}
    }
    public void closeMenu() {
        StartCoroutine(coroutineCloseMenu());
    }

    public IEnumerator coroutineCloseMenu() {
        yield return new WaitForSeconds(0F);
        GameObject menu = null;
		if (name == "button exit level menu") {
			menu = GameObject.Find ("level menu");
			menu.transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			menu.SetActive (false);
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
		} else if (name == "play") {
			menu = GameObject.Find ("pause menu");
			yield return StartCoroutine (staticClass.waitForRealTime (0.2F));
			menu.SetActive (false);
			Time.timeScale = staticClass.isTimePlay;

			//if (gYetiClass.yetiState == "")
			//Time.timeScale = 1;

		} else if (name == "exit energy menu") {
            menu = GameObject.Find("energy menu");
            GameObject.Find ("energy menu").transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			GameObject.Find ("energy").SendMessage ("stopCoroutineEnergyMenu");
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
        } else if (name == "button settings exit") {
			menu = transform.parent.parent.gameObject;
			menu.transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			menu.SetActive (false);

		} else if (name == "button market exit") {
			//marketClass.instance.transform.position = new Vector3 (0, 0, -10000);
			marketClass.instance.gameObject.SetActive (false);
			//marketClass.instance.camera.SetActive (false);
			Time.timeScale = staticClass.isTimePlay;
		} else if (name == "exit open booster menu") {
			marketClass.instance.openBoosterMenu.SetActive (false);
			marketClass.instance.boosterMenu.SetActive (true);

		} else if (name == "exit gift menu") {
			transform.parent.gameObject.SetActive (false);
		} else if (name == "exit daily menu") {
			transform.parent.gameObject.SetActive (false);
		} else if (name == "exit energy ad menu") {
			GameObject.Find ("ad dont ready menu").transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			GameObject.Find ("ad dont ready menu").SetActive (false);
		} else if (name == "exit coins menu") {
			GameObject.Find ("coins menu").transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			GameObject.Find ("coins menu").SetActive (false);
		} else if (name == "button exit close menu") {
			menu = GameObject.Find ("close menu");
			menu.transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			menu.SetActive (false);
		} else if (name == "button exit reset progress menu") {
			menu = transform.parent.parent.gameObject;
			menu.transform.GetChild (0).GetComponent<Animator> ().Play ("menu exit");
			yield return new WaitForSeconds (0.2F);
			menu.SetActive (false);
		}
    }

    //public IEnumerator CoroutineCloseMenu(){

    //}

    void selectLanguage() {
        Localization.language = name;
		if (name == "Russian") {
			ctrProgressClass.progress ["language"] = 2;
			transform.parent.GetChild (0).GetChild (0).gameObject.SetActive (false);
		} else {
			transform.parent.GetChild (1).GetChild (0).gameObject.SetActive (false);
			ctrProgressClass.progress ["language"] = 1;
		}
		transform.GetChild (0).gameObject.SetActive (true);
		ctrProgressClass.saveProgress ();
	}


    void clickBonusesArrow() {
        if (name == "arrow right") {
            gameObject.SetActive(false);
            GameObject.Find("bonuses/tween").transform.GetChild(1).gameObject.SetActive(true);
			staticClass.bonusesView = true;
        }
        else {
            gameObject.SetActive(false);
            GameObject.Find("bonuses/tween").transform.GetChild(0).gameObject.SetActive(true);
			staticClass.bonusesView = false;
		}

    }

	void fbConnect() {
		ctrFbKiiClass.instance.connect ();
	}

	void fbInvite() {
		ctrFbKiiClass.instance.invite ();
	}

	void ShowRewardedAd() {
		ctrAdClass.adStarted = name;
		ctrAdClass.instance.ShowRewardedAd ();
	}

    void restoreEnergy()
    {
        GameObject.Find("energy").GetComponent<lsEnergyClass>().restoreEnergy();
    }

    void buyEnergy()
    {
        GameObject.Find("energy").GetComponent<lsEnergyClass>().buyEnergy();
    }

    void dreamClick()
    {
        Debug.Log("dream click");
        Debug.Log(SceneManager.GetActiveScene().name);
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
            ShowRewardedAd();
        }
    }


}
