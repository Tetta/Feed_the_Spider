using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class AdCoinsTimerClass : MonoBehaviour {


    public GameObject rewardMenu;
    public UILabel minutes;
    public UILabel seconds;
    public GameObject coins;
    public GameObject hint;
    public GameObject hand;

    public static int counter = 0;
    public static DateTime timer = DateTime.Now.AddSeconds(60 * 5);
    public static int interval = 60 * 5;
    public static int firstInterval = 60 * 2;



    // Use this for initialization
    void Start () {

        DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        //first time
        if (ctrProgressClass.progress["adCoinsDate"] == 0)
        {
            ctrProgressClass.progress["adCoinsDate"] = (int)DateTime.Now.AddSeconds(firstInterval).TotalSeconds();
            timer = DateTime.Now.AddSeconds(firstInterval);
            ctrProgressClass.saveProgress();
        }

        else if (startDate.AddSeconds(ctrProgressClass.progress["adCoinsDate"]) < DateTime.Now)
        {
            //tutotial
            if (ctrProgressClass.progress["tutorialAdCoins"] == 1) hand.transform.GetChild(1).gameObject.SetActive(false);
            if (ctrProgressClass.progress["tutorialAdCoins"] < 2)
            {
                hand.SetActive(true);
                ctrProgressClass.progress["tutorialAdCoins"]++;
            }
            

            ctrProgressClass.progress["adCoinsDate"] = (int) DateTime.Now.TotalSeconds();
            timer = DateTime.Now;
            ctrProgressClass.saveProgress();
        }
        else timer = startDate.AddSeconds(ctrProgressClass.progress["adCoinsDate"]);
        StartCoroutine(updateTimeCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator updateTimeCoroutine()
    {
        if (timer > DateTime.Now)
        {
            var diff = timer - DateTime.Now;

            int modMin = diff.Minutes;
            if (modMin < 10) minutes.text = "0" + modMin.ToString();
            else minutes.text = modMin.ToString();

            int modSec = diff.Seconds;
            if (modSec < 10) seconds.text = "0" + modSec.ToString();
            else seconds.text = modSec.ToString();
            GetComponent<iClickClass>().functionPressButton = "";
            transform.GetChild(2).gameObject.SetActive(true);

        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<iClickClass>().functionPressButton = "ShowRewardedAd";
        }
        // остановка выполнения функции
        yield return StartCoroutine(staticClass.waitForRealTime(1));

        // запускаем корутину снова
        StartCoroutine("updateTimeCoroutine");
    }
}
