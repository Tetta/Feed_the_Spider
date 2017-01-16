using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ctrlDailyBonusClass : MonoBehaviour {


	public GameObject dailyBonusMenu;

	public static bool dailyBonusGiven = false;
	public static int realTime = 0;

	private string url = "https://time.yandex.ru/sync.json?geo=10393";

	//будет дейли бонус или нет
	IEnumerator Start() {
		if (name == "daily bonus") { 
			WWW www = new WWW(url);
			yield return www;
			try {
				//D/ebug.Log( www.text.Substring(8, 10));

				DateTime now = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				now = now.AddSeconds(System.Convert.ToInt64(www.text.Substring(8, 10)));
				realTime = int.Parse(www.text.Substring(8, 10));
				if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
				DateTime dailyBonus = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				dailyBonus = dailyBonus.AddSeconds(System.Convert.ToInt64(ctrProgressClass.progress["dailyBonus"]));
				if (now.ToShortDateString() != dailyBonus.ToShortDateString()) {
					//показать окно daily bonus
					dailyBonusMenu.SetActive(true);

					//delete cards in bonuses menu
					dailyBonusMenuOpen();

					//добавить бонусы за карты ягод
				    ctrProgressClass.progress["webs"] += ctrProgressClass.progress["berry2"];
                    ctrProgressClass.progress["collectors"] += ctrProgressClass.progress["berry3"];
                    ctrProgressClass.progress["teleports"] += ctrProgressClass.progress["berry4"];
                    ctrProgressClass.progress["hints"] += ctrProgressClass.progress["berry5"];
					if (ctrProgressClass.progress["berry2"] >= 1 && ctrProgressClass.progress["berry3"] >= 1 && ctrProgressClass.progress["berry4"] >= 1 && ctrProgressClass.progress["berry5"] >= 1) ctrProgressClass.progress["hints"] ++;
					ctrProgressClass.saveProgress();
				}

			} catch (System.Exception ex) {
				Debug.Log( ex.Message);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnClick () {

	}

	void dailyBonusMenuOpen() {
		dailyBonusMenu.transform.GetChild(0).GetChild(0).DestroyChildren();


		//exit gift menu - выключаем возможность закрыть меню
		dailyBonusMenu.transform.GetChild(1).GetComponent<iClickClass>().functionPress = "";
		dailyBonusMenu.transform.GetChild(3).gameObject.SetActive(false);

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
					card.GetComponent<mCardClass>().functionPress = "openCardDaily";
					card.transform.parent = dailyBonusMenu.transform.GetChild(0).GetChild(0);
					card.transform.localScale = new Vector2(1, 1);
					card.name = portion.Key;

					card.layer = LayerMask.NameToLayer ("daily bonus");
					ChangeLayersRecursively(card.transform, "daily bonus");


					card.SetActive (true);
					card.transform.GetChild(0).gameObject.SetActive (false);
					card.transform.GetChild(1).gameObject.SetActive (true);
					//позиция карты
					if (i == 0) card.transform.localPosition = new Vector3(-355, 7, -2); else if (i == 1) card.transform.localPosition = new Vector3(0, 7, -2); else if (i == 2) card.transform.localPosition = new Vector3(355, 7, -2); 

					break;
				}
				counterArr ++;
				counter += portion.Value;
			}



		}
		ctrProgressClass.progress["dailyBonus"] = realTime;


	}

	void ChangeLayersRecursively(Transform trans, String name)
	{
		foreach (Transform child in trans)
		{
			child.gameObject.layer = LayerMask.NameToLayer(name);
			ChangeLayersRecursively(child, name);
		}
	}


}
