using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using System.Collections.Generic;

public class lsAdClass : MonoBehaviour {
	public GameObject adDontReadyMenu;
	public GameObject energyGO;
	public UILabel coinsLabel;
	public GameObject coinsAdReward;


	void Start () {
		initializeEventHandlers ();
	}
		
	void OnClick () {
		ShowRewardedAd ();
	}

	public void ShowRewardedAd()
	{
		#if UNITY_ANDROID || UNITY_IOS


		if (Advertisement.IsReady ("rewardedVideo")) {
			//Unity Ads start
			if (name == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad", "start", "energy", 1);
			if (name == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad", "start", "coins", 1);
			if (name == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad", "start", "telek", 1);
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show ("rewardedVideo", options);

		} else if (Vungle.isAdvertAvailable()) {
			Dictionary<string, object> options = new Dictionary<string, object> ();
			options ["incentivized"] = true;
			Vungle.playAdWithOptions (options);

			//Vungle start
			if (name == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "start", "energy", 1);
			if (name == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "start", "coins", 1);
			if (name == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "start", "telek", 1);

			//ad dont ready Unity Ads
			if (name == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "energy", 1);
			if (name == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "coins", 1);
			if (name == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad", "dont ready", "telek", 1);
		} 
		else {
			//ad dont ready Vungle
			if (name == "button ad energy") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "energy", 1);
			if (name == "button ad coins") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "coins", 1);
			if (name == "button ad telek") GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "dont ready", "telek", 1);

			if (name != "button ad telek")  adDontReadyMenu.SetActive (true);
		}
		#endif
	
	}

	#if UNITY_ANDROID || UNITY_IOS

	private void HandleShowResult(ShowResult result)
	{
		// off for desktop 
		switch (result)
		{
		case ShowResult.Finished:
			//Debug.Log ("The ad was successfully shown.");
			if (name == "button ad energy") {
				GoogleAnalyticsV4.instance.LogEvent("Ad", "finish", "energy", 1);
				ctrProgressClass.progress ["energyTime"] -= 5 * lsEnergyClass.costEnergy;
				//ctrProgressClass.progress ["energy"] += 5;
				energyGO.SendMessage("OnApplicationPause", false);
			} else if (name == "button ad coins") {
				GoogleAnalyticsV4.instance.LogEvent("Ad", "finish", "coins", 1);
				ctrProgressClass.progress ["coins"] += 70;
				coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();
				ctrStatsClass.logEvent ("coins", "ad_coins", "level" + ctrProgressClass.progress["lastLevel"].ToString(), 70);
			} else if (name == "button ad telek") {
				GoogleAnalyticsV4.instance.LogEvent("Ad", "finish", "telek", 1);
				ctrProgressClass.progress ["coins"] += int.Parse(coinsAdReward.transform.GetChild(0).GetComponent<UILabel>().text.Substring(1));
				coinsAdReward.SetActive(true);
				gameObject.SetActive (false);


				ctrStatsClass.logEvent ("coins", "ad_telek", "level" + ctrProgressClass.progress["lastLevel"].ToString(), int.Parse(coinsAdReward.transform.GetChild(0).GetComponent<UILabel>().text.Substring(1)));
			}
			ctrProgressClass.saveProgress ();

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
				Debug.Log ("args.IsCompletedView: " + args.IsCompletedView);
				if (name == "button ad energy") {
					GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "finish", "energy", 1);
					ctrProgressClass.progress ["energyTime"] -= 5 * lsEnergyClass.costEnergy;
					//ctrProgressClass.progress ["energy"] += 5;
					energyGO.SendMessage("OnApplicationPause", false);
				} else if (name == "button ad coins") {
					Debug.Log ("111");
					GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "finish", "coins", 1);
					Debug.Log ("222");
					ctrProgressClass.progress ["coins"] += 70;
					Debug.Log ("333");
					coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();
					Debug.Log ("444");
					ctrStatsClass.logEvent ("coins", "ad_coins", "level" + ctrProgressClass.progress["lastLevel"].ToString(), 70);
					Debug.Log ("555");

				} else if (name == "button ad telek") {
					GoogleAnalyticsV4.instance.LogEvent("Ad Vungle", "finish", "telek", 1);
					ctrProgressClass.progress ["coins"] += int.Parse(coinsAdReward.transform.GetChild(0).GetComponent<UILabel>().text.Substring(1));
					coinsAdReward.SetActive(true);

					//выключение просто объекта ведет к ошибке, потом переделать
					//пока отключаем всех childs, collider
					//gameObject.SetActive (false);
					GetComponent<BoxCollider>().enabled = false;
					transform.GetChild(0).gameObject.SetActive(false);
					transform.GetChild(1).gameObject.SetActive(false);
					transform.GetChild(2).gameObject.SetActive(false);
					transform.GetChild(3).gameObject.SetActive(false);

					ctrStatsClass.logEvent ("coins", "ad_telek", "level" + ctrProgressClass.progress["lastLevel"].ToString(), int.Parse(coinsAdReward.transform.GetChild(0).GetComponent<UILabel>().text.Substring(1)));
				}
				ctrProgressClass.saveProgress ();
				Debug.Log ("666");

			}
		};
	}



}

