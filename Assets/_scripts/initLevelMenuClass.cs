using UnityEngine;
using System.Collections;

//using UnionAssets.FLE;
using System.Collections.Generic;

public class initLevelMenuClass : MonoBehaviour {
	public static initLevelMenuClass instance = null;

	public UILabel coinsLabel;
	public UILabel gemsLabel;
	public UILabel energyLabel;

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
		GoogleAnalyticsV4.instance.LogScreen("level menu");

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
