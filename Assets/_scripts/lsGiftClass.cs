using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class lsGiftClass : MonoBehaviour {

	public GameObject giftMenu;
    private List<KeyValuePair<string, int>> openingCards = new List<KeyValuePair<string, int>>();
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
        Dictionary<string, int> portionsGreen = new Dictionary<string, int>();
        Dictionary<string, int> portionsCountGreen = new Dictionary<string, int>();


        portionsGreen["webs"] = 30; portionsGreen["teleports"] = 26;
        portionsGreen["collectors"] = 24; portionsGreen["hints"] = 20;
        portionsCountGreen["webs"] = 2; portionsCountGreen["teleports"] = 2;
        portionsCountGreen["collectors"] = 2; portionsCountGreen["hints"] = 1;


	    if (name == "gift7")
	    {
            openingCards.Add(new KeyValuePair<string, int>("webs", 2));
            openingCards.Add(new KeyValuePair<string, int>("coins", 100));
            openingCards.Add(new KeyValuePair<string, int>("collectors", 1));

        } else if (name == "gift8")
	    {
            openingCards.Add(new KeyValuePair<string, int>("coins", 150));
            openingCards.Add(new KeyValuePair<string, int>("teleports", 1));
            openingCards.Add(new KeyValuePair<string, int>("hints", 1));
        }
        else
	    {
	        openingCards.Add(new KeyValuePair<string, int>("coins", 100));
	        for (int i = 1; i < 3; i++)
	        {
	            mBoosterClass.setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

            }
	        mBoosterClass.Shuffle(openingCards);
	    }

	    for (int i = 0; i < 3; i++) {
            //название карты и количество
            string bonusName = openingCards[i].Key;
            int bonusCount = openingCards[i].Value;
            Debug.Log(bonusName + " " + bonusCount);


            //копируем карту
            card = Instantiate(marketClass.instance.cardsAll.FindChild(bonusName + "_" + bonusCount).gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			card.GetComponent<mCardClass>().functionPress = "openCardGift";
			card.transform.parent = giftMenu.transform.GetChild(0).GetChild(0);
			card.transform.localScale = new Vector2(1, 1);
			card.name = "card" + (i + 1);
			card.SetActive (true);
			card.transform.GetChild(0).gameObject.SetActive (false);
			card.transform.GetChild(1).gameObject.SetActive (true);
			//позиция карты
			if (i == 0) card.transform.localPosition = new Vector3(-355, 7, -2); else if (i == 1) card.transform.localPosition = new Vector3(0, 7, -2); else if (i == 2) card.transform.localPosition = new Vector3(355, 7, -2); 

			//сохранение результата
			if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins") ctrProgressClass.progress[bonusName] += bonusCount;
            if (bonusName == "coins") ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "income", "chest" }, { "coins", bonusCount.ToString() } });

            initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();

		}
		//off for tests
		//помечаем что сундук открыт
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
		ctrProgressClass.progress [name] = 1; 
		ctrProgressClass.saveProgress();

	}
}
