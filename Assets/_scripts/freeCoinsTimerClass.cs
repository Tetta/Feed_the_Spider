using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class freeCoinsTimerClass : MonoBehaviour {
    private const int FreeGoldCount = 400;


    //public GameObject shrine;
    //public GameObject rewardMenu;
    public GameObject active;
    public GameObject inactive;

    public UILabel hours;
    public UILabel minutes;
    //public UILabel title;
    //public SpriteRenderer coins;
    public UILabel colon1;
    public UILabel colon2;

    public GameObject hand;

    public static int counter = 0;
    public static DateTime timer = DateTime.Now;
    public static int interval = 60 * 60 * 4;
    //public static int firstInterval = 0;
    public static int firstInterval = 60 * 5;


    // Use this for initialization
    void Start () {

        DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        Debug.Log("freeCoinsDate: " + startDate.AddSeconds(ctrProgressClass.progress["freeCoinsDate"]));

        if (ctrProgressClass.progress["lastLevel"] < 6)
        {
            //disable bonus
            transform.GetChild(0).gameObject.SetActive(false);
            //transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            return;
        }
        //first step
        else if (ctrProgressClass.progress["lastLevel"] >= 6 && ctrProgressClass.progress["freeCoinsDate"] == 0)
        {
            //enable bonus
            timer = DateTime.Now;
            //tutotial
            //off cloud
            Debug.Log("tutorialFreeCoins: " + ctrProgressClass.progress["tutorialFreeCoins"]);
            if (ctrProgressClass.progress["tutorialFreeCoins"] == 1) hand.transform.GetChild(1).gameObject.SetActive(false);
            ctrProgressClass.progress["tutorialFreeCoins"] = 0;
            if (ctrProgressClass.progress["tutorialFreeCoins"] < 2)
            {
                hand.SetActive(true);
                ctrProgressClass.progress["tutorialFreeCoins"]++;
            }
        }
        /*
        else if (ctrProgressClass.progress["lastLevel"] >= 7 && ctrProgressClass.progress["freeCoinsStep"] == 0)
        {
            ctrProgressClass.progress["freeCoinsDate"] = (int)DateTime.Now.AddSeconds(firstInterval).TotalSeconds();
            ctrProgressClass.progress["freeCoinsStep"] = 1;
            timer = DateTime.Now.AddSeconds(firstInterval);
            ctrProgressClass.saveProgress();
        }
        */


        else if (startDate.AddSeconds(ctrProgressClass.progress["freeCoinsDate"]) < DateTime.Now)
        {

            

            ctrProgressClass.progress["freeCoinsDate"] = (int) DateTime.Now.TotalSeconds();
            timer = DateTime.Now;
            ctrProgressClass.saveProgress();
        }
        else timer = startDate.AddSeconds(ctrProgressClass.progress["freeCoinsDate"]);
        StartCoroutine(updateTimeCoroutine());
    }
	

    public IEnumerator updateTimeCoroutine()
    {

        //shrine.SetActive(timer <= DateTime.Now);
        if (timer > DateTime.Now)
        {
            active.SetActive(false);
            inactive.SetActive(true);
            //coins.color = new Color(1,1,1,0.5f);
            //title.fontSize = 30;
            //title.text = String.Format(Localization.Get("coinsTimerTitle"), FreeGoldCount);
            var diff = timer - DateTime.Now;
            if (diff.TotalHours < 1)
            {
                hours.text = string.Format("{0:00}", diff.Minutes);
                minutes.text = string.Format("{0:00}", diff.Seconds);
                colon1.text = Localization.Get("m");
                colon2.text = Localization.Get("s");
            }
            else
            {
                hours.text = string.Format("{0:00}", diff.Hours);
                minutes.text = string.Format("{0:00}", diff.Minutes);
                colon1.text = Localization.Get("h");
                colon2.text = Localization.Get("m");
            }

            //transform.GetChild(2).gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            active.SetActive(true);
            inactive.SetActive(false);
            //title.text = String.Format("{0}", FreeGoldCount);
            //title.fontSize = 60;
            //coins.color = new Color(1, 1, 1, 1f);
            //transform.GetChild(2).gameObject.SetActive(false);
            //GetComponent<iClickClass>().functionPressButton = "openMenu";
            GetComponent<BoxCollider>().enabled = true;
        }
        // остановка выполнения функции
        yield return StartCoroutine(staticClass.waitForRealTime(1));

        // запускаем корутину снова
        StartCoroutine("updateTimeCoroutine");


    }

    public void OnClick()
    {

        

        ctrProgressClass.progress["coins"] += FreeGoldCount;
        //coinsLabel
        //counter++;
        Debug.Log("freeCoinsDate: " + ctrProgressClass.progress["freeCoinsDate"]);
        if (ctrProgressClass.progress["freeCoinsDate"] == 0)
        {
            ctrProgressClass.progress["freeCoinsDate"] = (int) DateTime.Now.AddSeconds(firstInterval).TotalSeconds();
            timer = DateTime.Now.AddSeconds(firstInterval);

            //enable ad coins timer
            var adCoinsGO = GameObject.Find("/root/static/ad coins");
            adCoinsGO.transform.GetChild(0).gameObject.SetActive(true);
            //adCoinsGO.transform.GetChild(2).gameObject.SetActive(true);
            adCoinsGO.GetComponent<BoxCollider>().enabled = true;
            ctrProgressClass.progress["adCoinsDate"] = (int)DateTime.Now.AddSeconds(firstInterval).TotalSeconds();
            AdCoinsTimerClass.timer = DateTime.Now.AddSeconds(firstInterval);
            adCoinsGO.GetComponent<AdCoinsTimerClass>().Start();
        }
        else
        {
            ctrProgressClass.progress["freeCoinsDate"] = (int) DateTime.Now.AddSeconds(interval).TotalSeconds();
            timer = DateTime.Now.AddSeconds(interval);
        }

        GameObject.Find("root/static/coins/coinsLabel").GetComponent<UILabel>().text = ctrProgressClass.progress["coins"].ToString();

        /* убрал выдачу реварда
        initLevelMenuClass.instance.rewardMenu.SetActive(true);
        initLevelMenuClass.instance.rewardMenu.transform.GetChild(0)
            .GetChild(5)
            .GetChild(0)
            .GetChild(3)
            .GetChild(3)
            .GetComponent<UILabel>()
            .text = FreeGoldCount.ToString();
         */

        ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> {{"detail 1", "free"}, {"coins", FreeGoldCount .ToString()} });
        //initLevelMenuClass.instance.coinsMenu.SetActive(false);
        ctrProgressClass.saveProgress();
        hand.SetActive(false);

        GameObject.Find("/root/static/button market").GetComponent<iClickClass>().checkTutorialBuy();

        //ps coins
        var t = GameObject.Find("/root/static/ps coins");
        t.SetActive(false);
        t.SetActive(true);

        active.SetActive(false);
        inactive.SetActive(true);
    }
}
