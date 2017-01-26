using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCoinsTimerClass : MonoBehaviour {


    public GameObject rewardMenu;
    public UILabel minutes;
    public UILabel seconds;
    public GameObject coins;
    public GameObject hint;

    public static int counter = 0;
    public static DateTime timer = DateTime.Now.AddSeconds(60 * 5);
    public static int interval = 60 * 5;



    // Use this for initialization
    void Start () {
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
