using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Facebook.Unity;

public class ctrlDailyBonusClass : MonoBehaviour {


	public GameObject dailyBonusMenu;

	public static bool dailyBonusGiven = false;
	public static int realTime = 0;

	private string url = "https://time.yandex.ru/sync.json?geo=10393";
    private List<KeyValuePair<string, int>> openingCards = new List<KeyValuePair<string, int>>();

    //будет дейли бонус или нет
    void Start() {

        if (name == "daily bonus") { 
			//WWW www = new WWW(url);
            //yield return www;
			//try {
            //D/ebug.Log( www.text.Substring(8, 10));
            if (ctrProgressClass.progress["dailyBonus"] == 0) ctrProgressClass.progress["dailyBonus"] = (int)DateTime.UtcNow.TotalSeconds();
            /*
            DateTime now = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            now = now.AddSeconds(System.Convert.ToInt64(www.text.Substring(8, 10)));
            realTime = int.Parse(www.text.Substring(8, 10));
            if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
            DateTime dailyBonus = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dailyBonus = dailyBonus.AddSeconds(System.Convert.ToInt64(ctrProgressClass.progress["dailyBonus"]));
            */
            DateTime now = DateTime.UtcNow;
            DateTime dailyBonus =  new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(ctrProgressClass.progress["dailyBonus"]);

            Debug.Log("now: " + now.ToShortDateString());
            Debug.Log("dailyBonus: " + dailyBonus.ToShortDateString());
            //Debug.Log("realTime: " + realTime);
            //Debug.Log("TotalSeconds: " + DateTime.UtcNow.TotalSeconds());
            if (now.ToShortDateString() != dailyBonus.ToShortDateString()) {
                //показать окно daily bonus
                //dailyBonusMenu.SetActive(true);
                Debug.Log("..........................................daily bonus");

                //delete cards in bonuses menu
                //dailyBonusMenuOpen();
                //добавить бонусы за карты ягод
                ctrProgressClass.progress["webs"] += ctrProgressClass.progress["berry2"];
                ctrProgressClass.progress["collectors"] += ctrProgressClass.progress["berry3"];
                ctrProgressClass.progress["teleports"] += ctrProgressClass.progress["berry4"];
                ctrProgressClass.progress["hints"] += ctrProgressClass.progress["berry5"];

                //analytics
                if (ctrProgressClass.progress["berry2"] > 0)
                {
                    ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                    {
                        {"detail", "item"},
                        {"name", "webs"},
                        {"count", ctrProgressClass.progress["berry2"].ToString()}
                    });
                }
                if (ctrProgressClass.progress["berry3"] > 0)
                {
                    ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                    {
                        {"detail", "item"},
                        {"name", "collectors"},
                        {"count", ctrProgressClass.progress["berry3"].ToString()}
                    });
                }
                if (ctrProgressClass.progress["berry4"] > 0)
                {
                    ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                    {
                        {"detail", "item"},
                        {"name", "teleports"},
                        {"count", ctrProgressClass.progress["berry4"].ToString()}
                    });
                }
                if (ctrProgressClass.progress["berry5"] > 0)
                {
                    ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                    {
                        {"detail", "item"},
                        {"name", "hints"},
                        {"count", ctrProgressClass.progress["berry5"].ToString()}
                    });
                }

                //for reward menu on map
                staticClass.showRewardCardsMenuWebs = ctrProgressClass.progress["berry2"];
                staticClass.showRewardCardsMenuCollectors = ctrProgressClass.progress["berry3"];
                staticClass.showRewardCardsMenuTeleports = ctrProgressClass.progress["berry4"];
                staticClass.showRewardCardsMenuHints = ctrProgressClass.progress["berry5"];

                    
                if (ctrProgressClass.progress["berry2"] >= 1 && ctrProgressClass.progress["berry3"] >= 1 &&
				    ctrProgressClass.progress["berry4"] >= 1 && ctrProgressClass.progress["berry5"] >= 1)
				{
				    ctrProgressClass.progress["hints"] += 3;
				    staticClass.showRewardCardsMenuHints += 3;

                }
                ctrProgressClass.progress["dailyBonus"] = (int)now.TotalSeconds();
                ctrProgressClass.saveProgress();
			}

			//} catch (Exception ex) {
            //    Debug.Log("Exception daily login: " + ex.Message);
			//}
		}
	}
	

	void dailyBonusMenuOpen() {
		dailyBonusMenu.transform.GetChild(0).GetChild(0).DestroyChildren();


		//exit gift menu - выключаем возможность закрыть меню
		dailyBonusMenu.transform.GetChild(1).GetComponent<iClickClass>().functionPress = "";
		dailyBonusMenu.transform.GetChild(3).gameObject.SetActive(false);

		GameObject card = new GameObject();

        //шансы на картах
        Dictionary<string, int> portionsGreen = new Dictionary<string, int>();
        Dictionary<string, int> portionsCountGreen = new Dictionary<string, int>();
        openingCards.Clear();

        portionsGreen["webs"] = 30; portionsGreen["teleports"] = 26;
        portionsGreen["collectors"] = 24; portionsGreen["hints"] = 20;
        portionsCountGreen["webs"] = 2; portionsCountGreen["teleports"] = 2;
        portionsCountGreen["collectors"] = 2; portionsCountGreen["hints"] = 1;
        openingCards.Add(new KeyValuePair<string, int>("coins", 100));
        for (int i = 1; i < 3; i++)
        {
            //if (UnityEngine.Random.Range(0, 100) < 5) mBoosterClass.setOpeningCardUncommon("berry", ref openingCards);
            //else mBoosterClass.setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);
        }
        mBoosterClass.Shuffle(openingCards);

        for (int i = 0; i < 3; i++) {
            //название карты и количество
            string bonusName = openingCards[i].Key;
            int bonusCount = openingCards[i].Value;
            Debug.Log(bonusName + " " + bonusCount);

            //копируем карту
            if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" ||
                bonusName == "coins")
            {
                card =
                    Instantiate(marketClass.instance.cardsAll.Find(bonusName + "_" + bonusCount).gameObject,
                            new Vector3(0, 0, 0), Quaternion.Euler(0, 0, Mathf.CeilToInt(UnityEngine.Random.Range(-5, 5))))
                        as GameObject;
                if (bonusName != "coins")
                {
                    ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                        {
                            {"detail", "daily_gift"},
                            {"name", bonusName},
                            {"count", bonusCount.ToString()}
                        });
                }

            }
            else
            {
                card =
                    Instantiate(marketClass.instance.cardsAll.Find(bonusName).gameObject, new Vector3(0, 0, 0),
                        Quaternion.Euler(0, 0, Mathf.CeilToInt(UnityEngine.Random.Range(-5, 5)))) as GameObject;
            }
            if (bonusName == "coins") ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "detail 1", "daily" }, { "coins", bonusCount.ToString() } });

            card.GetComponent<mCardClass>().functionPress = "openCardGift";
			card.transform.parent = dailyBonusMenu.transform.GetChild(0).GetChild(0);
			card.transform.localScale = new Vector2(1, 1);
			card.name = "card" + (i + 1);

            //card.layer = LayerMask.NameToLayer ("daily bonus");
			//ChangeLayersRecursively(card.transform, "daily bonus");
            card.transform.GetChild(1).GetChild(0).GetComponent<UISprite>().color = new Color32(154, 138, 123, 255);
            mBoosterClass.addLayerToCard(ref card, i);

            card.SetActive (true);
			card.transform.GetChild(0).gameObject.SetActive (false);
			card.transform.GetChild(1).gameObject.SetActive (true);
			//позиция карты
			//if (i == 0) card.transform.localPosition = new Vector3(-355, 34, -2); else if (i == 1) card.transform.localPosition = new Vector3(0, 34, -2); else if (i == 2) card.transform.localPosition = new Vector3(355, 34, -2);
            
            //сохранение результата
            ctrProgressClass.progress[bonusName] += bonusCount;

        }
		ctrProgressClass.progress["dailyBonus"] = realTime;
        ctrProgressClass.saveProgress();

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
