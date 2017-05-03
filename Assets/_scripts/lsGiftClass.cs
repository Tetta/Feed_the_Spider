using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class lsGiftClass : MonoBehaviour {

	public GameObject giftMenu;
    public static List<KeyValuePair<string, int>> openingCards = new List<KeyValuePair<string, int>>();
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

        openingCards.Clear();

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
	            setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

            }
	        mBoosterClass.Shuffle(openingCards);
	    }

	    for (int i = 0; i < 3; i++) {
            //название карты и количество
            string bonusName = openingCards[i].Key;
            int bonusCount = openingCards[i].Value;
            Debug.Log(bonusName + " " + bonusCount);


            //копируем карту
            card = Instantiate(mBoosterClass.instance.transform.parent.parent.GetChild(2).FindChild(bonusName).gameObject, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, Mathf.CeilToInt(UnityEngine.Random.Range(-5, 5)))) as GameObject;
			card.GetComponent<mCardClass>().functionPress = "openCardGift";
			card.transform.parent = giftMenu.transform.GetChild(0).GetChild(0);
			card.transform.localScale = new Vector2(1, 1);
			card.name = "card" + (i + 1);
	        card.layer = 5;
            ChangeLayersRecursively(card.transform, "UI");

            mBoosterClass.addLayerToCard(ref card, i);

            card.SetActive (true);
			card.transform.GetChild(0).gameObject.SetActive (false);
			card.transform.GetChild(1).gameObject.SetActive (true);
            card.transform.GetChild(1).GetChild(0).GetComponent<UISprite>().color = new Color32(154, 138, 123, 255);

            if (bonusCount > 1)
            {
                Debug.Log(card.transform.GetChild(0).GetChild(3).GetChild(3).gameObject.name);
                card.transform.GetChild(0).GetChild(3).GetChild(3).gameObject.SetActive(true);
                card.transform.GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<UILabel>().text = bonusCount.ToString();
            }

            //позиция карты
            //if (i == 0) card.transform.localPosition = new Vector3(-355, 7, -2); else if (i == 1) card.transform.localPosition = new Vector3(0, 7, -2); else if (i == 2) card.transform.localPosition = new Vector3(355, 7, -2); 

            //сохранение результата
            if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins")
	        {
	            ctrProgressClass.progress[bonusName] += bonusCount;
                //analytics
                if (bonusName != "coins")
                {
                    ctrAnalyticsClass.sendEvent("Bonuses", new Dictionary<string, string>
                        {
                            {"detail", "chest"},
                            {"name", bonusName},
                            {"count", bonusCount.ToString()}
                        });
                }
            }
            if (bonusName == "coins") ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "detail 1", "chest" }, { "coins", bonusCount.ToString() } });

            initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();

		}
		//off for tests
		//помечаем что сундук открыт
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(true);
		ctrProgressClass.progress [name] = 1; 
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


    private void setOpeningCardCommon(ref Dictionary<string, int> portions, Dictionary<string, int> portionsCount, ref List<KeyValuePair<string, int>> openingCards)
    {
        int bonusRand = UnityEngine.Random.Range(0, portions.Values.Sum()); //min [inclusive] and max [exclusive] 
        int counter = 0;
        string nameBonus = "";
        int countBonus = 0;
        foreach (var portion in portions)
        {
            if (bonusRand >= counter && bonusRand < counter + portion.Value)
            {
                //название карты 
                nameBonus = portion.Key;
                break;
            }
            counter += portion.Value;
        }
        portions[nameBonus] = Mathf.RoundToInt(portions[nameBonus] / 1.5F);
        bonusRand = UnityEngine.Random.Range(0, 100);
        float part = 100 / (((2 + portionsCount[nameBonus] - 1) / 2) * portionsCount[nameBonus]);
        countBonus = 1 + portionsCount[nameBonus] - Mathf.CeilToInt(bonusRand / part);
        openingCards.Add(new KeyValuePair<string, int>(nameBonus, countBonus));

        //Debug.Log(nameBonus + " " + countBonus);
    }
}
