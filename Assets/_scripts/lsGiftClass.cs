using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class lsGiftClass : MonoBehaviour {

	public GameObject giftMenu;
	//public int giftLevel;
	// Use this for initialization
	void Start () {
		if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();

		//открыт или нет сундук
		if (ctrProgressClass.progress[name] == 1) {
			transform.GetChild(0).gameObject.SetActive(false);
			transform.GetChild(1).gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick () {
		if (ctrProgressClass.progress [name] == 0 && int.Parse(name.Substring(4)) <= ctrProgressClass.progress["lastLevel"]) {

			GetComponent<AudioSource> ().Play ();
			giftMenu.SetActive (true);
			//delete cards in bonuses menu
			giftMenu.transform.GetChild(0).GetChild(0).DestroyChildren();
			StartCoroutine(clickGift());
		}
	}

	public IEnumerator clickGift() {
		yield return new WaitForSeconds(0.01F);


		//exit gift menu - выключаем возможность закрыть меню
		giftMenu.transform.GetChild(1).GetComponent<iClickClass>().functionPress = "";
		giftMenu.transform.GetChild(2).gameObject.SetActive(false);

		GameObject card = new GameObject();

		//шансы на картах
		Dictionary<string, int> portions = new Dictionary<string, int>();
		Dictionary<string, int> portions2 = new Dictionary<string, int>();

		portions["hints_1"] = 190; 
		portions["webs_1"] = 50; portions["webs_2"] = 100; 
		portions["teleports_1"] = 50; 
		portions["collectors_1"] = 100;
		portions["coins_50"] = 10; portions["coins_100"] = 100; portions["coins_250"] = 250;
		portions["energy_5"] = 25; portions["energy_10"] = 70; portions["energy_15"] = 155;


		int portionsSum = 0;
		int portionsSum2 = 0;


		foreach (var e in portions) {
			portionsSum += e.Value;
		}
		foreach (var e in portions) {

			portions2[e.Key] =  portionsSum / e.Value;
			portionsSum2 += portions2 [e.Key];
		}

		for (int i = 0; i < 3; i++) {
			int counter = 0;
			int counterArr = 0;
			int bonusRand = Mathf.CeilToInt(UnityEngine.Random.Range(0, portionsSum2));

			foreach (var portion in portions2 ) {
				if (bonusRand >= counter && bonusRand < counter + portion.Value) {

					//копируем карту
					card = Instantiate(marketClass.instance.cardsAll.FindChild(portion.Key).gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
					card.GetComponent<mCardClass>().functionPress = "openCardGift";
					card.transform.parent = giftMenu.transform.GetChild(0).GetChild(0);
					card.transform.localScale = new Vector2(1, 1);
					card.name = "card" + (i + 1);
					card.SetActive (true);
					card.transform.GetChild(0).gameObject.SetActive (false);
					card.transform.GetChild(1).gameObject.SetActive (true);
					//позиция карты
					if (i == 0) card.transform.localPosition = new Vector3(-355, 7, -2); else if (i == 1) card.transform.localPosition = new Vector3(0, 7, -2); else if (i == 2) card.transform.localPosition = new Vector3(355, 7, -2); 

					//название карты и количество
					string bonusName = portion.Key.Split(new Char[] { '_' })[0];
					int bonusCount = int.Parse(portion.Key.Split(new Char[] { '_' })[1]);

					//сохранение результата
					if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins") ctrProgressClass.progress[bonusName] += bonusCount;
					else if (bonusName == "energy") {
						ctrProgressClass.progress["energyTime"] -= bonusCount * lsEnergyClass.costEnergy;
						ctrProgressClass.progress["energy"] += bonusCount;

						if (GameObject.Find ("root/static/energy/energy line") != null) {
							GameObject.Find ("root/static/energy/energy line").GetComponent<UISprite>().fillAmount = 1 - (float)ctrProgressClass.progress ["energy"] / lsEnergyClass.maxEnergy;
							if (lsEnergyClass.energyInfinity)
								GameObject.Find ("root/static/energy/energy line").GetComponent<UISprite>().fillAmount = 1;
						}
					}
					else {
						if (ctrProgressClass.progress[bonusName] == 0) ctrProgressClass.progress[bonusName] = 1;

					}
					initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();
					initLevelMenuClass.instance.energyLabel.text = ctrProgressClass.progress ["energy"].ToString ();

					break;
				}
				counterArr ++;
				counter += portion.Value;
			}




		}
		//off for tests
		//помечаем что сундук открыт
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
		ctrProgressClass.progress [name] = 1; 
		ctrProgressClass.saveProgress();

	}
}
