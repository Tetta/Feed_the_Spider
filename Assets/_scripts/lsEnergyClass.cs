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
    //public UILabel minutes2;
    //public UILabel seconds2;
    public UILabel energyLabel;
	public UISprite energyLine;
	public GameObject infinity;
	public GameObject plus;
    public UI2DSprite backBar;

    public Transform energySegments;

    //public GameObject buttonRestoreEnergy;
    public GameObject buttonAdEnergy;
    public GameObject buttonBuyEnergy;
    public GameObject hand;

    public static string energyMenuState = "";

	public static int costEnergy = 60 * 15;
	public static int maxEnergy = 4;
	public static bool energyInfinity = false;

    public static bool energyTake = false;
    public static int costEnergyForCoins = 10 * 15;

    public static int energy
    {
        get
        {
            var e =
                Mathf.CeilToInt(((int) DateTime.Now.TotalSeconds() - ctrProgressClass.progress["energyTime"])/costEnergy);
            if (e > maxEnergy) e = maxEnergy;
            if (e < 0) e = 0;

            return e;
        }
        set
        {
            Debug.Log("energy v: " + value);
            int energyTime = ctrProgressClass.progress["energyTime"];
            int now = (int)DateTime.Now.TotalSeconds();

            //if energyTime very small
            if (energyTime < now - maxEnergy * costEnergy)
                ctrProgressClass.progress["energyTime"] = now - maxEnergy * costEnergy;

            //checkEnergy(true);
            ctrProgressClass.progress["energyTime"] += -costEnergy * value;
            //checkEnergy(true);
            //if energyTime > now
            if (energyTime > now) ctrProgressClass.progress["energyTime"] = now;

            ctrProgressClass.saveProgress();
        }
    }

    // Use this for initialization
    void Start () {
        costEnergy = 60 * 15;
        maxEnergy = 4;
        bool flag = true;
        for (int e = 2; e <= 5; e++)	if (ctrProgressClass.progress["berry" + e] < 1) flag = false;
        if (flag)
        {
            //добавляем 5ю энергию
            maxEnergy = 5;
            backBar.width = 324;
            energySegments.GetChild(9).gameObject.SetActive(true);
        }
        Debug.Log("maxEnergy: " + maxEnergy);

        //уменьшение времени восстановления энергии за счет карт
        if (ctrProgressClass.progress["skin2"] >= 1) costEnergy -= 30;
        if (ctrProgressClass.progress["skin3"] >= 1) costEnergy -= 45;
        if (ctrProgressClass.progress["skin4"] >= 1) costEnergy -= 60;
        if (ctrProgressClass.progress["skin5"] >= 1) costEnergy -= 90;
        flag = true;
        for (int e = 2; e <= 5; e++) if (ctrProgressClass.progress["skin" + e] < 1) flag = false;
        if (flag) costEnergy -= 120;

		OnApplicationFocus(true);
        if (energyMenuState == "energy" && ctrProgressClass.progress["tutorialEnergy"] == 1) OnClick();
        //energy tutorial
        else if (ctrProgressClass.progress["tutorialEnergy"] == 0 && energy == 0) hand.SetActive(true);
        

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

    void OnEnable()
    {
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        energyInfinityCheck();
        StopAllCoroutines();
        StartCoroutine("Coroutine");
        if (energyMenu.activeSelf) StartCoroutine("CoroutineEnergyMenu");
        if (energyMenu.activeSelf) StartCoroutine("updateTimeEnergyCoroutine");
    }



    public IEnumerator updateTimeEnergyCoroutine()
    {
        int mod = checkEnergy(true);
        int modMin = Mathf.CeilToInt((costEnergy - mod)/60);
        //if (modMin < 10) minutes2.text = "0" + modMin.ToString();
        //else minutes2.text = modMin.ToString();

        int modSec = (costEnergy - mod) % 60;
        //if (modSec < 10) seconds2.text = "0" + modSec.ToString();
        //else seconds2.text = modSec.ToString();

        //если бесконечная энергия на день
        energyInfinityCheck();

        //выключаем labels таймеров, если max energy
        if (energy >= maxEnergy)
        {
            //transform.GetChild(8).gameObject.SetActive(false);
            //energyMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            //energyMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        }

        // остановка выполнения функции
        yield return StartCoroutine(staticClass.waitForRealTime(1));

        // запускаем корутину снова
        StartCoroutine("updateTimeEnergyCoroutine");
    }
    // Update is called once per frame

    public IEnumerator Coroutine(){
		int mod = checkEnergy(true);
        for (int i = 1; i <= 5; i++)
        {
            if (energy >= i) energySegments.GetChild(i - 1).gameObject.SetActive(true);
            else energySegments.GetChild(i - 1).gameObject.SetActive(false);
        }

        //energyLabel.text = energy.ToString();

		//energyLine.fillAmount = 1 - (float) energy / maxEnergy;
		//if (energyInfinity)
		//	energyLine.fillAmount = 1;
		if (energy >= maxEnergy || energyInfinity)
			plus.SetActive (false);
		else 
			plus.SetActive (true);
		
		// остановка выполнения функции на costEnergy секунд
		yield return StartCoroutine(staticClass.waitForRealTime(costEnergy - mod));

		// запускаем корутину снова
		StartCoroutine("Coroutine");
	}

    public static int checkEnergy(bool flag) {
        //Debug.Log("checkEnergy: " + flag);

        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        int mod = 0;
        int energyTime = ctrProgressClass.progress["energyTime"];
        int now = (int)DateTime.Now.TotalSeconds();

        if (energyTime + maxEnergy * costEnergy > now)
        {
            //total seconds for next energy
            mod = (now - energyTime) % costEnergy;
        }
        if (flag) {
			//ctrProgressClass.saveProgress();
			if (maxEnergy <= energy && GameObject.Find("energy") != null) GameObject.Find("energy").SendMessage("stopCoroutineEnergyMenu");
			return mod;
		} else {
			if (energy > 0) {
                Debug.Log("energy --");
                if (int.Parse(SceneManager.GetActiveScene().name.Substring(5)) > 5)
                    energy = -1;
			    //ctrProgressClass.progress["energyTime"] += costEnergy;
			    //checkEnergy(true);
                energyTake = true;

                ctrProgressClass.saveProgress();
				return 1;
			} else return 0;
		}

	}

	public void OnApplicationFocus(bool flag) {
		if (flag) { 
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
        int cost = costEnergyForCoins*(maxEnergy - energy) -(int) (((float) mod/costEnergy)*costEnergyForCoins);
        //int cost = costEnergy - mod;
        //for energy tutorial
        //buttonRestoreEnergy.transform.GetChild(2).gameObject.SetActive(true);
        //buttonRestoreEnergy.transform.GetChild(3).gameObject.SetActive(true);
        //buttonRestoreEnergy.transform.GetChild(5).gameObject.SetActive(false);


        if (maxEnergy > energy)
        {
            //if (ctrProgressClass.progress["tutorialEnergy"] == 1)
            //    buttonRestoreEnergy.transform.GetChild(2).GetComponent<UILabel>().text = cost.ToString();
            //if energy tutorial
            //else
            //{
            //buttonRestoreEnergy.transform.GetChild(2).gameObject.SetActive(false);
            //buttonRestoreEnergy.transform.GetChild(3).gameObject.SetActive(false);
            //buttonRestoreEnergy.transform.GetChild(5).gameObject.SetActive(true);
            //}
            if (buttonAdEnergy != null) buttonAdEnergy.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            //buttonRestoreEnergy.transform.GetChild(4).gameObject.SetActive(true);
            if (buttonAdEnergy != null) buttonAdEnergy.transform.GetChild(2).gameObject.SetActive(true);
       
        }

        //if (ctrProgressClass.progress["tutorialEnergy"] == 1 && cost > ctrProgressClass.progress["coins"]) buttonRestoreEnergy.transform.GetChild(4).gameObject.SetActive(true);


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
			
            if (energy == 0 && !lsEnergyClass.energyInfinity) {
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
        for (int i = 1; i <= 5; i++)
        {
            if (energy >= i) energySegments.GetChild(i - 1).gameObject.SetActive(true);
            else energySegments.GetChild(i - 1).gameObject.SetActive(false);
        }


        //energyLabel.text = maxEnergy.ToString();
        ctrProgressClass.progress["energyTime"] = 0;
        energy = maxEnergy;
        //buttonRestoreEnergy.transform.GetChild(2).GetComponent<UILabel>().text = "0";
        StartCoroutine("CoroutineEnergyMenu");
        if (ctrProgressClass.progress["tutorialEnergy"] == 1)
        {
            var costCurrent = costEnergyForCoins * (maxEnergy - energy) - (int)(((float)checkEnergy(true) / costEnergy) * costEnergyForCoins);

            ctrProgressClass.progress["coins"] -= costCurrent;
            ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "detail 1", "energy" }, { "coins", (-costCurrent).ToString() } });

        }
        else
        {
            ctrAnalyticsClass.sendEvent("Tutorial", new Dictionary<string, string>{{"name", "energy free"}});
            ctrProgressClass.progress["tutorialEnergy"] = 1;


        }
    }

    public void buyEnergy()
    {
        marketClass.buyEnergy();
    }
    public void buyEnergyReward()
    {

        for (int i = 1; i <= 5; i++)
        {
            if (energy >= i) energySegments.GetChild(i - 1).gameObject.SetActive(true);
            else energySegments.GetChild(i - 1).gameObject.SetActive(false);
        }

        //добавить запрос на покупку в маркет
        //energyLabel.text = maxEnergy.ToString();
        ctrProgressClass.progress["energyTime"] = 0;
        energy = maxEnergy;
        ctrProgressClass.progress["energyInfinity"] = (int) DateTime.Now.AddDays(1).TotalSeconds();
        //for test
        //ctrProgressClass.progress["energyInfinity"] = (int)DateTime.Now.AddSeconds(15).TotalSeconds();
        energyInfinityCheck();
        //buttonRestoreEnergy.transform.GetChild(2).GetComponent<UILabel>().text = "0";
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
            //energyLabel.gameObject.SetActive(false);
            energySegments.gameObject.SetActive(false);

            //включаем таймер
            if (buttonBuyEnergy != null)
            {
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
        }
        else
        {
            energyInfinity = false;
            infinity.SetActive(false);
            //energyLabel.gameObject.SetActive(true);
            energySegments.gameObject.SetActive(true);


            if (buttonBuyEnergy != null)
            {
                buttonBuyEnergy.transform.GetChild(0).gameObject.SetActive(true);
                buttonBuyEnergy.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
