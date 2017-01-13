using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Advertisements;
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

	public static string energyMenuState = "";

	public static int costEnergy = 60 * 15;
	public static int maxEnergy = 30;
	public static bool energyInfinity = false;

    public static bool energyTake = false;

    // Use this for initialization
    void Start () {

		//увеличение максимума энергии за счет карт
		int addEnergy = 0;
		for (int e = 2; e <= 5; e++)	if (ctrProgressClass.progress["skin" + e] >= 1) addEnergy += (e * 5 - 5) * ctrProgressClass.progress["skin" + e];
		maxEnergy = 30 + addEnergy;

		//если собраны все скины
		if (addEnergy == 50) {
			energyInfinity = true;
			infinity.SetActive (true);
			energyLabel.gameObject.SetActive (false);
		}


		OnApplicationPause(false);
		if (energyMenuState == "energy") OnClick();

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
            //число секунд с 01.01.2015
            int now = (int)(DateTime.UtcNow - new DateTime(2015, 1, 1)).TotalSeconds;
            int deltaEnergy = Mathf.CeilToInt((now - ctrProgressClass.progress["energyTime"]) / costEnergy);
            ctrProgressClass.progress["energy"] += deltaEnergy;
            mod = (now - ctrProgressClass.progress["energyTime"]) % costEnergy;
            ctrProgressClass.progress["energyTime"] = now - mod;
            if (ctrProgressClass.progress["energy"] > maxEnergy) ctrProgressClass.progress["energy"] = maxEnergy;
		}
		if (flag) {
			ctrProgressClass.saveProgress();
			if (maxEnergy <= ctrProgressClass.progress["energy"]) GameObject.Find("energy").SendMessage("stopCoroutineEnergyMenu");
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
		if (ctrProgressClass.progress["energy"] < maxEnergy){
			energyMenuState = "";
			energyMenu.SetActive(true);
			StartCoroutine("CoroutineEnergyMenu");
		}

	}

	public IEnumerator CoroutineEnergyMenu(){
		int mod = checkEnergy(true);
        int modMin = Mathf.CeilToInt((costEnergy - mod) / 60);
        if (modMin < 10) minutes.text = "0" + modMin.ToString();
        else minutes.text = modMin.ToString();

        int modSec = (costEnergy - mod) % 60;
		if (modSec < 10) seconds.text = "0" + modSec.ToString();
		else seconds.text = modSec.ToString();

		// остановка выполнения функции
		yield return StartCoroutine(staticClass.waitForRealTime(1));

		// запускаем корутину снова
		StartCoroutine("CoroutineEnergyMenu");
	}

	void stopCoroutineEnergyMenu () {
		energyMenu.SetActive(false);
		StopCoroutine("CoroutineEnergyMenu");
	}



	public static bool checkLoadLevelEnergy () {
		//energy, если нет комплекта
		if (ctrProgressClass.progress["complect"] == 0) {
			int energyCount = lsEnergyClass.checkEnergy(true);
			
            if (energyCount == 0 && !lsEnergyClass.energyInfinity) {
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




}
