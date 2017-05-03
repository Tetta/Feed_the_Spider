using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lsSaleClass : MonoBehaviour {


    public GameObject rewardMenu;
    public GameObject saleMenu;
    public GameObject hand;
    public GameObject withoutSale;
    public UILabel hours;
    public UILabel minutes;
    public UILabel seconds;

    public static int counter = 0;
    public static DateTime timerStartSale = DateTime.Now;
    public static DateTime timerEndSale = DateTime.Now;



    //saleDate - start sale (DateTime.Now.TotalSeconds()), sale - number (0, 1, 2, 3)
    void Start ()
    {

    }

    public void OnEnable()
    {
        ctrProgressClass.saveProgress();
        setTimerSale();

        //next sale
        setSale();

        //enable sale if timer
        if (timerStartSale < DateTime.Now)
        {
            //tutotial
            if (ctrProgressClass.progress["tutorialSale"] == 1)
                if (hand != null) hand.transform.GetChild(1).gameObject.SetActive(false);
            if (ctrProgressClass.progress["tutorialSale"] < 2)
            {
                if (hand != null) hand.SetActive(true);
                ctrProgressClass.progress["tutorialSale"]++;
            }

            Debug.Log("sale enable");
            //off all sales
            for (int i = 0; i < transform.GetChild(0).childCount; i++)
            {
                transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
                if (i < 3) transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
            }

            transform.GetChild(ctrProgressClass.progress["firstPurchase"])
                .GetChild(ctrProgressClass.progress["sale"])
                .gameObject.SetActive(true);
            if (saleMenu != null)
            {
                saleMenu.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(1).GetChild(1).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(1).GetChild(2).gameObject.SetActive(false);
                saleMenu.transform.GetChild(0).GetChild(ctrProgressClass.progress["firstPurchase"]).GetChild(ctrProgressClass.progress["sale"]).gameObject.SetActive(true);
            }
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            if (withoutSale != null) withoutSale.SetActive(false);

            if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = true;
            ctrProgressClass.saveProgress();

            if (SceneManager.GetActiveScene().name == "menu" || SceneManager.GetActiveScene().name == "level menu") StartCoroutine(updateTimeCoroutine());

            //button market
            var marketGO = GameObject.Find("button market").transform;
            marketGO.GetChild(1).gameObject.SetActive(false);
            marketGO.GetChild(2).gameObject.SetActive(false);
            marketGO.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            //Debug.Log("sale disable");
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            if (withoutSale != null) withoutSale.SetActive(true);
            //button market
            var marketGO = GameObject.Find("button market").transform;
            marketGO.GetChild(1).gameObject.SetActive(true);
            marketGO.GetChild(2).gameObject.SetActive(true);
            marketGO.GetChild(3).gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    public IEnumerator updateTimeCoroutine()
    {
        if (timerEndSale > DateTime.Now)
        {
            var diff = timerEndSale - DateTime.Now;

            int modH = diff.Hours;
            if (modH < 10) hours.text = "0" + modH.ToString();
            else hours.text = modH.ToString();

            int modMin = diff.Minutes;
            if (modMin < 10) minutes.text = "0" + modMin.ToString();
            else minutes.text = modMin.ToString();

            int modSec = diff.Seconds;
            if (modSec < 10) seconds.text = "0" + modSec.ToString();
            else seconds.text = modSec.ToString();

            // остановка выполнения функции
            yield return StartCoroutine(staticClass.waitForRealTime(1));

            // запускаем корутину снова
            StartCoroutine("updateTimeCoroutine");
        }
        else
        {
            //gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            if (withoutSale != null) withoutSale.SetActive(true);
            if (GetComponent<BoxCollider>() != null) GetComponent<BoxCollider>().enabled = false;
            if (hand != null) hand.SetActive(false);
        }

    }

    public static void setTimerSale()
    {
       // Debug.Log("saleDate: " + ctrProgressClass.progress["saleDate"]);
        //Debug.Log("sale: " + ctrProgressClass.progress["sale"]);
        //Debug.Log("time now: " + DateTime.Now);

        DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        string str = "free";
        if (ctrProgressClass.progress["firstPurchase"] == 1) str = "payers";
        var pause = staticClass.sales["sale_" + ctrProgressClass.progress["sale"] + "_" + str].pause;
        var duration = staticClass.sales["sale_" + ctrProgressClass.progress["sale"] + "_" + str].duration;

        //timer sale end
        if (ctrProgressClass.progress["saleDate"] == 0)
            ctrProgressClass.progress["saleDate"] = (int)DateTime.Now.TotalSeconds();
        //Debug.Log("saleDate: " + ctrProgressClass.progress["saleDate"]);
        timerStartSale =
            startDate.AddSeconds(ctrProgressClass.progress["saleDate"])
                .Add(pause);

        timerEndSale =
            startDate.AddSeconds(ctrProgressClass.progress["saleDate"])
                .Add(pause)
                .Add(duration);

        //Debug.Log("sale pause: " + pause);
        //Debug.Log("sale duration: " + duration);
        //Debug.Log("sale timer end: " + timerEndSale);

    }

    public static void setSale()
    {
        if (timerEndSale < DateTime.Now)
        {
            ctrProgressClass.progress["sale"]++;
            ctrProgressClass.progress["saleDate"] = (int)DateTime.Now.TotalSeconds();

            if (ctrProgressClass.progress["firstPurchase"] == 0 && ctrProgressClass.progress["sale"] > 3)
                ctrProgressClass.progress["sale"] = 3;
            if (ctrProgressClass.progress["firstPurchase"] == 1 && ctrProgressClass.progress["sale"] > 2)
                ctrProgressClass.progress["sale"] = 2;

            setTimerSale();
            ctrProgressClass.saveProgress();

        }
    }

    void OnApplicationFocus(bool flag)
    {
        if (flag) OnEnable();
    }
}
