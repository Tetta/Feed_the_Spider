using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;

public class AdCoinsTimerClass : MonoBehaviour {


    public GameObject shrine;
    public GameObject rewardMenu;
    public UILabel minutes;
    public UILabel seconds;
    public GameObject coins;
    public GameObject hint;
    public GameObject hand;
    public UILabel title;

    public static int counter = 0;
    public static DateTime timer = DateTime.Now.AddSeconds(60 * 5);
    public static int interval = 60 * 5;
    //public static int firstInterval = 60 * 4;



    // Use this for initialization
    public void Start () {
        if (ctrProgressClass.progress["adCoinsDate"] == 0)
        {
            //disable bonus
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            return;
        }


        DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        Debug.Log("adCoinsDate: " + startDate.AddSeconds(ctrProgressClass.progress["adCoinsDate"]));

        //first time
        /*
        if (ctrProgressClass.progress["adCoinsDate"] == 0)
        {
            ctrProgressClass.progress["adCoinsDate"] = (int)DateTime.Now.AddSeconds(firstInterval).TotalSeconds();
            timer = DateTime.Now.AddSeconds(firstInterval);
            ctrProgressClass.saveProgress();
        }
        

        else*/
        if (startDate.AddSeconds(ctrProgressClass.progress["adCoinsDate"]) < DateTime.Now)
        {
            //tutotial
            //off cloud
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
        shrine.SetActive(timer <= DateTime.Now);
        if (timer > DateTime.Now)
        {
            var diff = timer - DateTime.Now;
            minutes.text = string.Format("{0:00}", diff.Minutes);
            seconds.text = string.Format("{0:00}", diff.Seconds);

            GetComponent<iClickClass>().functionPressButton = "";
            transform.GetChild(2).gameObject.SetActive(true);

        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(false);
            GetComponent<iClickClass>().functionPressButton = "openMenu";
        }
        // остановка выполнения функции
        yield return StartCoroutine(staticClass.waitForRealTime(1));

        // запускаем корутину снова
        StartCoroutine("updateTimeCoroutine");
    }
}
