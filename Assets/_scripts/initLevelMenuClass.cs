using UnityEngine;
using System.Collections;

//using UnionAssets.FLE;
using System.Collections.Generic;

public class initLevelMenuClass : MonoBehaviour {
	public static initLevelMenuClass instance = null;
    public GameObject unlockСhapterMenu;
    public GameObject rewardForCardsMenu;

    public UILabel coinsLabel;
	public UILabel gemsLabel;
	public UILabel energyLabel;

    public GameObject fb;
    public GameObject vk;

    //public static UILabel coinsLabel;
    //public static UILabel gemsLabel;
    //public static UILabel energyLabel;
    //на 3 звезды = 0, испытание = 1
    public static int levelDemands = 0;

	public static string levelMenuState = "";

	// Use this for initialization
	void Start () {
		Time.maximumDeltaTime = 0.9F;
		Time.timeScale = 1;
		//temp
		staticClass.initLevels();
		//temp
		//coinsLabel = coins;
		//gemsLabel = gems;
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
		coinsLabel.text = ctrProgressClass.progress["coins"].ToString();
		gemsLabel.text = ctrProgressClass.progress["gems"].ToString();
		energyLabel.text = ctrProgressClass.progress["energy"].ToString();
		staticClass.sceneLoading = false;
		instance = this;
		if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogScreen("level menu");

        //fb off, vk on
        if (ctrProgressClass.progress["language"] == 2)
	    {
	        fb.SetActive(false);
            vk.SetActive(true);
	    }
        //fb on, vk off
        else if (ctrProgressClass.progress["language"] == 1)
        {
            fb.SetActive(true);
            vk.SetActive(false);
        }

        //unlockСhapterMenu enable
        if (staticClass.notGemsForLevel)
	    {
	        unlockСhapterMenu.SetActive(true);
	        staticClass.notGemsForLevel = false;
	    }

	    //for test
        //staticClass.showRewardCardsMenuCollectors = 3;
        //staticClass.showRewardCardsMenuWebs = 3;
        //staticClass.showRewardCardsMenuHints = 1;
        //staticClass.showRewardCardsMenuTeleports = 22;

        //reward menu for collection cards
        if (staticClass.showRewardCardsMenuCollectors != 0 || staticClass.showRewardCardsMenuWebs != 0 ||
            staticClass.showRewardCardsMenuHints != 0 || staticClass.showRewardCardsMenuTeleports != 0)
        {
            List<Transform> bonuses = new List<Transform>();
            rewardForCardsMenu.SetActive(true);
            if (staticClass.showRewardCardsMenuCollectors != 0)
            {
                var tr = rewardForCardsMenu.transform.GetChild(0).GetChild(0);
                tr.GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text =
                    staticClass.showRewardCardsMenuCollectors.ToString();
                tr.gameObject.SetActive(true);
                bonuses.Add(tr);
            }
            if (staticClass.showRewardCardsMenuHints != 0)
            {
                var tr = rewardForCardsMenu.transform.GetChild(0).GetChild(1);
                tr.GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text =
                    staticClass.showRewardCardsMenuHints.ToString();
                tr.gameObject.SetActive(true);
                bonuses.Add(tr);
            }
            if (staticClass.showRewardCardsMenuTeleports != 0)
            {
                var tr = rewardForCardsMenu.transform.GetChild(0).GetChild(2);
                tr.GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text =
                    staticClass.showRewardCardsMenuTeleports.ToString();
                tr.gameObject.SetActive(true);
                bonuses.Add(tr);
            }
            if (staticClass.showRewardCardsMenuWebs != 0)
            {
                var tr = rewardForCardsMenu.transform.GetChild(0).GetChild(3);
                tr.GetChild(0).GetChild(3).GetChild(0).GetComponent<UILabel>().text =
                    staticClass.showRewardCardsMenuWebs.ToString();
                tr.gameObject.SetActive(true);
                bonuses.Add(tr);
            }
            if (bonuses.Count == 1)
            {
                bonuses[0].localPosition = new Vector3(16, 41, 0);
                bonuses[0].rotation = Quaternion.Euler(0, 0, -8);
            }
            if (bonuses.Count == 2)
            {
                bonuses[0].localPosition = new Vector3(-99, 54, 0);
                bonuses[1].localPosition = new Vector3(107, 36, 0);

                bonuses[0].rotation = Quaternion.Euler(0, 0, 10);
                bonuses[1].rotation = Quaternion.Euler(0, 0, -25);
            }
            if (bonuses.Count == 3)
            {
                bonuses[0].localPosition = new Vector3(-203, -48, 0);
                bonuses[1].localPosition = new Vector3(8, 57, 0);
                bonuses[2].localPosition = new Vector3(229, 54, 0);
            }
            staticClass.showRewardCardsMenuCollectors = 0;
            staticClass.showRewardCardsMenuWebs = 0;
            staticClass.showRewardCardsMenuHints = 0;
            staticClass.showRewardCardsMenuTeleports = 0;
        }



    }
	
	// Update is called once per frame
	void Update () {
		
		//обработка кнопки "Назад" на Android
		if (Input.GetButtonDown("Cancel")) {
			if (GameObject.Find ("ad dont ready menu") != null)
				GameObject.Find ("exit energy ad menu").SendMessage ("OnPress", false);
			else if (GameObject.Find ("level menu") != null) {
				GameObject.Find ("button exit level menu").SendMessage ("OnPress", false);
			} else if (GameObject.Find ("market") != null) {
				if (GameObject.Find ("market/open booster menu") == null)
					GameObject.Find ("button market exit").SendMessage ("OnPress", false);
			} else if (GameObject.Find ("energy menu") != null)
				GameObject.Find ("exit energy menu").SendMessage ("OnPress", false);
			else if  (GameObject.Find ("gift menu") != null)
				GameObject.Find ("exit gift menu").SendMessage ("OnPress", false);
			else if  (GameObject.Find ("coins menu") != null)
				GameObject.Find ("exit coins menu").SendMessage ("OnPress", false);
			else GameObject.Find("button back").SendMessage("OnPress", false);
		}



	}

	//--------------------------------------
	// EVENTS
	//--------------------------------------

	/*
	private void OnGiftResult(CEvent e) {
		GooglePlayGiftRequestResult result = e.data as GooglePlayGiftRequestResult;
		SA_StatusBar.text = "Gift Send Result:  " + result.code.ToString();
	}
	
	private void OnPendingGiftsDetected(CEvent e) {
		AndroidDialog dialog = AndroidDialog.Create("Pending Gifts Detected", "You got few gifts from your friends, do you whant to take a look?");
		dialog.addEventListener(BaseEvent.COMPLETE, OnPromtGiftDialogClose);
	}
	
	private void OnPromtGiftDialogClose(CEvent e) {
		//removing listner
		(e.dispatcher as AndroidDialog).removeEventListener(BaseEvent.COMPLETE, OnPromtGiftDialogClose);
		
		//parsing result
		switch((AndroidDialogResult)e.data) {
		case AndroidDialogResult.YES:
			GooglePlayManager.instance.ShowRequestsAccepDialog();
			break;
			
			
		}
	}
	
	
	
	private void OnGameRequestAccepted(CEvent e) {
		List<GPGameRequest> gifts = e.data as List<GPGameRequest>;
		foreach(GPGameRequest g in gifts) {
			AN_PoupsProxy.showMessage("Gfit Accepted", g.playload + " is excepted");
		}
	}
	*/
}
