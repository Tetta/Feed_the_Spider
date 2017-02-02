using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class lsEnergyClass : MonoBehaviour {

	public GameObject energyMenu;
	public UILabel minutes;
	public UILabel seconds;
    public UILabel minutes2;
    public UILabel seconds2;
    public UILabel energyLabel;
	public UISprite energyLine;
	public GameObject infinity;
	public GameObject plus;

    public GameObject buttonRestoreEnergy;
    public GameObject buttonAdEnergy;
    public GameObject buttonBuyEnergy;
    public GameObject hand;

    public static string energyMenuState = "";

	public static int costEnergy = 60 * 15;
	public static int maxEnergy = 4;
	public static bool energyInfinity = false;

    public static bool energyTake = false;
    public static int costEnergyForCoins = 10 * 15;

    // Use this for initialization
    void Start () {
        costEnergy = 60 * 15;
        maxEnergy = 4;

        bool flag = true;
        for (int e = 2; e <= 5; e++)	if (ctrProgressClass.progress["berry" + e] < 1) flag = false;
        if (flag) maxEnergy ++;
        //уменьшение времени восстановления энергии за счет карт
        if (ctrProgressClass.progress["skin2"] >= 1) costEnergy -= 30;
        if (ctrProgressClass.progress["skin3"] >= 1) costEnergy -= 45;
        if (ctrProgressClass.progress["skin4"] >= 1) costEnergy -= 60;
        if (ctrProgressClass.progress["skin5"] >= 1) costEnergy -= 90;
        flag = true;
        for (int e = 2; e <= 5; e++) if (ctrProgressClass.progress["skin" + e] < 1) flag = false;
        if (flag) costEnergy -= 120;

		OnApplicationPause(false);
        if (energyMenuState == "energy" && ctrProgressClass.progress["tutorialEnergy"] == 1) OnClick();
        //energy tutorial
        else if (ctrProgressClass.progress["tutorialEnergy"] == 0 && ctrProgressClass.progress["energy"]== 0) hand.SetActive(true);
        

        //если бесконечная энергия на день
        energyInfinityCheck();

        //init complect, work only with scene "menu"
        //GameObject complect = GameObject.Instantiate(marketClass.instance.specialMenu.transform.GetChild(0).GetChild(0).gameObject);
        //complect.transform.parent = energyMenu.transform.GetChild(0).GetChild(0);
        //complect.transform.localPosition = new Vector3(-98, -776, -0.01F);
        //complect.transform.localScale = new Vector3(1F, 1F, 0);
        //

        StartCoroutine(updateTimeEnergyCoroutine());
    }
    public IEnumerator updateTimeEnergyCoroutine()
    {
        int mod = checkEnergy(true);
        int modMin = Mathf.CeilToInt((costEnergy - mod)/60);
        if (modMin < 10) minutes2.text = "0" + modMin.ToString();
        else minutes2.text = modMin.ToString();

        int modSec = (costEnergy - mod) % 60;
        if (modSec < 10) seconds2.text = "0" + modSec.ToString();
        else seconds2.text = modSec.ToString();

        //если бесконечная энергия на день
        energyInfinityCheck();

        //выключаем labels таймеров, если max energy
        if (ctrProgressClass.progress["energy"] >= maxEnergy)
        {
            transform.GetChild(8).gameObject.SetActive(false);
            energyMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            energyMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }

        // остановка выполнения функции
        yield return StartCoroutine(staticClass.waitForRealTime(1));

        // запускаем корутину снова
        StartCoroutine("updateTimeEnergyCoroutine");
    }
    // Update is called once per frame
    void Update () {
	    
	}

	public IEnumerator Coroutine(){
		int mod = checkEnergy(true);
		energyLabel.text = (ctrProgressClass.progress["energy"]).ToString();

		energyLine.fillAmount = 1 - (float) ctrProgressClass.progress["energy"] / maxEnergy;
		if (energyInfinity)
			energyLine.fillAmount = 1;
		if (ctrProgressClass.progress ["energy"] >= maxEnergy || energyInfinity)
			plus.SetActive (false);
		else 
			plus.SetActive (true);
		
		// остановка выполнения функции на costEnergy секунд
		yield return StartCoroutine(staticClass.waitForRealTime(costEnergy - mod));

		// запускаем корутину снова
		StartCoroutine("Coroutine");
	}

    public static int checkEnergy(bool flag) {
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        int mod = 0;
        if (ctrProgressClass.progress["energy"] < maxEnergy) {
            //var startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);


            //число секунд с 01.01.2015
            int now = (int)(DateTime.UtcNow - new DateTime(2015, 1, 1)).TotalSeconds;
            int deltaEnergy = Mathf.CeilToInt((now - ctrProgressClass.progress["energyTime"]) / costEnergy);
            ctrProgressClass.progress["energy"] += deltaEnergy;
            mod = (now - ctrProgressClass.progress["energyTime"]) % costEnergy;
            ctrProgressClass.progress["energyTime"] = now - mod;
            if (ctrProgressClass.progress["energy"] > maxEnergy) ctrProgressClass.progress["energy"] = maxEnergy;
		}
		if (flag) {
			//ctrProgressClass.saveProgress();
			if (maxEnergy <= ctrProgressClass.progress["energy"] && GameObject.Find("energy") != null) GameObject.Find("energy").SendMessage("stopCoroutineEnergyMenu");
			return mod;
		} else {
			if (ctrProgressClass.progress["energy"] > 0) {
				ctrProgressClass.progress["energy"] --;
			    energyTake = true;

                ctrProgressClass.saveProgress();
				return 1;
			} else return 0;
		}

	}

	public void OnApplicationPause(bool flag) {
		if (!flag) { 
			if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
			//StopCoroutine("Coroutine");
			StopAllCoroutines();
			StartCoroutine("Coroutine");
			if (energyMenu.activeSelf) StartCoroutine("CoroutineEnergyMenu");
		}
	}

	void OnClick() {
		//if (ctrProgressClass.progress["energy"] < maxEnergy){
			energyMenuState = "";
			energyMenu.SetActive(true);
			StartCoroutine("CoroutineEnergyMenu");
        //}

	    if (ctrProgressClass.progress["tutorialEnergy"] == 0) hand.SetActive(false);

            

    }

    public IEnumerator CoroutineEnergyMenu(){
		int mod = checkEnergy(true);
        int modMin = Mathf.CeilToInt((costEnergy - mod) / 60);
        if (modMin < 10) minutes.text = "0" + modMin.ToString();
        else minutes.text = modMin.ToString();

        int modSec = (costEnergy - mod) % 60;
		if (modSec < 10) seconds.text = "0" + modSec.ToString();
		else seconds.text = modSec.ToString();

        //стоимость восстановления
        int cost = costEnergyForCoins*(maxEnergy - ctrProgressClass.progress["energy"]) -(int) (((float) mod/costEnergy)*costEnergyForCoins);
        //int cost = costEnergy - mod;
        //for energy tutorial
        buttonRestoreEnergy.transform.GetChild(2).gameObject.SetActive(true);
        buttonRestoreEnergy.transform.GetChild(3).gameObject.SetActive(true);
        buttonRestoreEnergy.transform.GetChild(5).gameObject.SetActive(false);

        if (maxEnergy > ctrProgressClass.progress["energy"])
        {
            if (ctrProgressClass.progress["tutorialEnergy"] == 1)
                buttonRestoreEnergy.transform.GetChild(2).GetComponent<UILabel>().text = cost.ToString();
            //if energy tutorial
            else
            {
                buttonRestoreEnergy.transform.GetChild(2).gameObject.SetActive(false);
                buttonRestoreEnergy.transform.GetChild(3).gameObject.SetActive(false);
                buttonRestoreEnergy.transform.GetChild(5).gameObject.SetActive(true);
            }
        }
        else
        {
            buttonRestoreEnergy.transform.GetChild(4).gameObject.SetActive(true);
            buttonAdEnergy.transform.GetChild(5).gameObject.SetActive(true);
       
        }

        if (ctrProgressClass.progress["tutorialEnergy"] == 1 && cost > ctrProgressClass.progress["coins"]) buttonRestoreEnergy.transform.GetChild(4).gameObject.SetActive(true);


        // остановка выполнения функции
        yield return StartCoroutine(staticClass.waitForRealTime(1));

		// запускаем корутину снова
		StartCoroutine("CoroutineEnergyMenu");
	}

	void stopCoroutineEnergyMenu () {
		//energyMenu.SetActive(false);
		StopCoroutine("CoroutineEnergyMenu");
	}



	public static bool checkLoadLevelEnergy () {
		//energy, если нет комплекта
		if (ctrProgressClass.progress["complect"] == 0) {
			lsEnergyClass.checkEnergy(true);
			
            if (ctrProgressClass.progress["energy"] == 0 && !lsEnergyClass.energyInfinity) {
				//нотифер, поправить с новым плагином
				//AndroidNotificationManager.instance.ScheduleLocalNotification(Localization.Get("notiferTitleEnergy"), Localization.Get("notiferMessageEnergy"), lsEnergyClass.costEnergy * lsEnergyClass.maxEnergy);
				lsEnergyClass.energyMenuState = "energy";
				SceneManager.LoadScene ("level menu");
				return false;
			} else {
				return true;
			}
		} else return true;


	}


    public void restoreEnergy()
    {
        energyLabel.text = maxEnergy.ToString();
        ctrProgressClass.progress["energyTime"] = 0;
        ctrProgressClass.progress["energy"] = maxEnergy;
        buttonRestoreEnergy.transform.GetChild(2).GetComponent<UILabel>().text = "0";
        StartCoroutine("CoroutineEnergyMenu");
        if (ctrProgressClass.progress["tutorialEnergy"] == 1)
        {
            var costCurrent = costEnergyForCoins * (maxEnergy - ctrProgressClass.progress["energy"]) - (int)(((float)checkEnergy(true) / costEnergy) * costEnergyForCoins);

            ctrProgressClass.progress["coins"] -= costCurrent;
            ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "decome", "energy" }, { "coins", (-costCurrent).ToString() } });

        }
        else
        {
            ctrAnalyticsClass.sendEvent("Tutorial", new Dictionary<string, string>{{"name", "energy free"}});
            ctrProgressClass.progress["tutorialEnergy"] = 1;


        }
    }
    public void buyEnergy()
    {
        //добавить запрос на покупку в маркет
        energyLabel.text = maxEnergy.ToString();
        ctrProgressClass.progress["energyTime"] = 0;
        ctrProgressClass.progress["energy"] = maxEnergy;
        ctrProgressClass.progress["energyInfinity"] = (int) DateTime.Now.AddDays(1).TotalSeconds();
        //for test
        //ctrProgressClass.progress["energyInfinity"] = (int)DateTime.Now.AddSeconds(15).TotalSeconds();
        energyInfinityCheck();
        buttonRestoreEnergy.transform.GetChild(2).GetComponent<UILabel>().text = "0";
        StartCoroutine("CoroutineEnergyMenu");

    }

    void energyInfinityCheck()
    {
        DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //если бесконечная энергия на день
        if (startDate.AddSeconds(ctrProgressClass.progress["energyInfinity"]) > DateTime.Now)
        {
            energyInfinity = true;
            infinity.SetActive(true);
            energyLabel.gameObject.SetActive(false);
            //включаем таймер
            var timeDiff = startDate.AddSeconds(ctrProgressClass.progress["energyInfinity"]) - DateTime.Now;

            buttonBuyEnergy.transform.GetChild(0).gameObject.SetActive(false);
            buttonBuyEnergy.transform.GetChild(1).gameObject.SetActive(true);
            string str = "";
            str = timeDiff.Hours.ToString();
            if (timeDiff.Hours < 10) str = "0" + timeDiff.Hours;
            buttonBuyEnergy.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<UILabel>().text = str;
            str = timeDiff.Minutes.ToString();
            if (timeDiff.Minutes < 10) str = "0" + timeDiff.Minutes;
            buttonBuyEnergy.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<UILabel>().text = str;
            str = timeDiff.Seconds.ToString();
            if (timeDiff.Seconds < 10) str = "0" + timeDiff.Seconds;
            buttonBuyEnergy.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<UILabel>().text = str;


        }
        else
        {
            energyInfinity = false;
            infinity.SetActive(false);
            energyLabel.gameObject.SetActive(true);
            buttonBuyEnergy.transform.GetChild(0).gameObject.SetActive(true);
            buttonBuyEnergy.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
