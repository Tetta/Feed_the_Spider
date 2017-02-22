using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

public class mBoosterClass : MonoBehaviour {

    //public GameObject[] cards = new GameObject[5];
	//public Transform[] cardIcons = new Transform[5];
	public Transform cardsAll;
    public static int counterOpenCard;
    public Transform exitOpenBoosterMenu;
    public GameObject buttonOpenBooster;

    private string currentBoosterColor;
    private List <KeyValuePair<string, int>> openingCards = new List<KeyValuePair<string, int>>();


    //private SpriteRenderer[] backCard = new SpriteRenderer[5];
    //private SpriteRenderer[] circleLight = new SpriteRenderer[5];
    //private SpriteRenderer[] light = new SpriteRenderer[5];
    //private ParticleSystem[] psIdle = new ParticleSystem[5];
    //private ParticleSystem[] psOpen = new ParticleSystem[5];
    //private GameObject[] infoCard = new GameObject[5];
    //private UILabel[] nameLabel = new UILabel[5];
    //private UILabel[] countLabel = new UILabel[5];
    // Use this for initialization
    void Start() {
		/*
		for (int i = 0; i < 5; i++) {
			backCard [i] = cards [i].transform.GetChild (0).GetChild (0).GetComponent<SpriteRenderer> ();
			circleLight [i] = cards [i].transform.GetChild (0).GetChild (1).GetComponent<SpriteRenderer> ();
			light [i] = cards [i].transform.GetChild (0).GetChild (2).GetComponent<SpriteRenderer> ();
			psIdle [i] = cards [i].transform.GetChild (0).GetChild (4).GetComponent<ParticleSystem> ();
			//psOpen [i] = cards [i].transform.GetChild (0).GetChild (4).GetChild (4).GetComponent<ParticleSystem> ();
			nameLabel [i] = cards [i].transform.GetChild (0).GetChild (3).GetChild (1).GetComponent<UILabel> ();
			//countLabel [i] = cards [i].transform.GetChild (0).GetChild (0).GetComponent<SpriteRenderer> ();
		}
		*/
    }

    // Update is called once per frame
    void Update() {
    }

    void OnEnable() {
        
        //delete cards in booster
        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            Destroy(transform.GetChild(2).GetChild(i).gameObject);


        }
        /*
		//default cards
		for (int i = 0; i < 5; i++) {
			if (cardIcons[i].childCount != 0) Destroy(cardIcons[i].GetChild(0).gameObject);
			//default rotation
			cards[i].transform.GetChild(0).localRotation = Quaternion.identity;
			cards[i].transform.GetChild(1).localRotation = Quaternion.identity;
			cards[i].transform.GetChild(2).localRotation = Quaternion.identity;
			//off frontside, on backside
			cards[i].transform.GetChild(0).gameObject.SetActive(false);
			cards[i].transform.GetChild(1).gameObject.SetActive(true);
			cards[i].SetActive(false);

        }
		*/

        exitOpenBoosterMenu.localPosition = new Vector3(0, 0, -10000);
        buttonOpenBooster.SetActive(false);
        //показываем бустер
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
		transform.GetChild (1).gameObject.GetComponent<ctrParticleSystem> ().enabled = false;

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(transform.GetChild(0).GetChild(0).GetChild(i).gameObject.name);
            Debug.Log(transform.GetChild(0).GetChild(0).GetChild(i).gameObject.activeSelf);
            if (transform.GetChild(0).GetChild(0).GetChild(i).gameObject.activeSelf)
                currentBoosterColor = transform.GetChild(0).GetChild(0).GetChild(i).gameObject.name;
        }
        Debug.Log(currentBoosterColor);
    }

    void OnPress(bool isPressed) {

        if (!isPressed) {
            StartCoroutine(coroutinePressBooster());
        }

    }

    public IEnumerator coroutinePressBooster() {
		transform.GetChild (1).gameObject.GetComponent<ctrParticleSystem> ().enabled = true;
		transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
		transform.GetChild(1).GetComponent<AudioSource>().Play();
		GetComponent<AudioSource>().Play();
		//for (int i = 0; i < 5; i++) cards[i].SetActive(false);

        
        GameObject card;
        //GameObject bonusesTemp = GameObject.Instantiate(GameObject.Find("bonuses temp"), new Vector2(0, 0), Quaternion.identity) as GameObject;
        //bonusesTemp.transform.parent = giftMenu.transform.GetChild(0).GetChild(2);
        //bonusesTemp.transform.localScale = new Vector3(1, 1, 1);
        //шансы на картах


        Dictionary<string, int> portionsWhite = new Dictionary<string, int>();
        Dictionary<string, int> portionsCountWhite = new Dictionary<string, int>();
        Dictionary<string, int> portionsGreen = new Dictionary<string, int>();
        Dictionary<string, int> portionsCountGreen = new Dictionary<string, int>();
        Dictionary<string, int> portionsBlue = new Dictionary<string, int>();
        Dictionary<string, int> portionsCountBlue = new Dictionary<string, int>();
        Dictionary<string, int> portionsPurple = new Dictionary<string, int>();
        Dictionary<string, int> portionsCountPurple = new Dictionary<string, int>();



        portionsWhite["webs"] = 40; portionsWhite["teleports"] = 30;
        portionsWhite["collectors"] = 20; portionsWhite["hints"] = 10;
        portionsCountWhite["webs"] = 1; portionsCountWhite["teleports"] = 1;
        portionsCountWhite["collectors"] = 1; portionsCountWhite["hints"] = 1;

        portionsGreen["webs"] = 30; portionsGreen["teleports"] = 26;
        portionsGreen["collectors"] = 24; portionsGreen["hints"] = 20;
        portionsCountGreen["webs"] = 2; portionsCountGreen["teleports"] = 2;
        portionsCountGreen["collectors"] = 2; portionsCountGreen["hints"] = 1;

        portionsBlue["webs"] = 20; portionsBlue["teleports"] = 24;
        portionsBlue["collectors"] = 26; portionsBlue["hints"] = 30;
        portionsCountBlue["webs"] = 3; portionsCountBlue["teleports"] = 3;
        portionsCountBlue["collectors"] = 2; portionsCountBlue["hints"] = 2;

        portionsPurple["webs"] = 10; portionsPurple["teleports"] = 20;
        portionsPurple["collectors"] = 30; portionsPurple["hints"] = 40;
        portionsCountPurple["webs"] = 4; portionsCountPurple["teleports"] = 4;
        portionsCountPurple["collectors"] = 3; portionsCountPurple["hints"] = 3;
        openingCards.Clear();
        if (currentBoosterColor == "boostersWhite")
        {
            openingCards.Add(new KeyValuePair<string, int>("coins", 50));
            var max = 5;
            Debug.Log("tutorialBuy:" + ctrProgressClass.progress["tutorialBuy"]);
            if (ctrProgressClass.progress["tutorialBuy"] < 3)
            {
                setOpeningCardUncommon("berry", ref openingCards);
                max = 4;
                ctrProgressClass.progress["tutorialBuy"] = 3;
                ctrAnalyticsClass.sendEvent("Tutorial", new Dictionary<string, string> { { "name", "open booster" } });

            }
            for (int i = 1; i < max; i++)
            {
                var r = UnityEngine.Random.Range(0, 100);
                Debug.Log("rand:" + r);
                if (r < 2) setOpeningCardUncommon("berry", ref openingCards);
                else setOpeningCardCommon(ref portionsWhite, portionsCountWhite, ref openingCards);
            }
            Shuffle(openingCards);
        }

        if (currentBoosterColor == "boostersGreen")
        {
            setOpeningCardUncommon("berry", ref openingCards);
            openingCards.Add(new KeyValuePair<string, int>("coins", 100));
            for (int i = 2; i < 5; i++)
            {
                if (UnityEngine.Random.Range(0, 100) < 2) setOpeningCardUncommon("hat", ref openingCards);
                else setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

            }
            Shuffle(openingCards);

        }
        if (currentBoosterColor == "boostersBlue")
        {
            setOpeningCardUncommon("berry", ref openingCards);
            setOpeningCardUncommon("hat", ref openingCards);
            openingCards.Add(new KeyValuePair<string, int>("coins", 150));
            for (int i = 3; i < 5; i++)
            {
                if (UnityEngine.Random.Range(0, 100) < 2) setOpeningCardUncommon("skin", ref openingCards);
                else setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

            }
            Shuffle(openingCards);
        }
        if (currentBoosterColor == "boostersPurple")
        {
            setOpeningCardUncommon("berry", ref openingCards);
            setOpeningCardUncommon("hat", ref openingCards);
            setOpeningCardUncommon("skin", ref openingCards);
            openingCards.Add(new KeyValuePair<string, int>("coins", 200));
            for (int i = 4; i < 5; i++)
            {
                if (UnityEngine.Random.Range(0, 100) < 2) setOpeningCardUncommon("skin", ref openingCards);
                else setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

            }
            Shuffle(openingCards);
        }
        Debug.Log("openingCards.Count: " + openingCards.Count);
        for (int i = 0; i < 5; i++) {
            //название карты и количество
			string bonusName = openingCards[i].Key;
			int bonusCount = openingCards[i].Value;
            Debug.Log(bonusName + " " + bonusCount);

            //копируем карту
            //card.transform.parent = transform.GetChild(2);
            if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins")
                card = Instantiate(cardsAll.FindChild(bonusName + "_" + bonusCount).gameObject, transform.GetChild(2));
            else
                card = Instantiate(cardsAll.FindChild(bonusName).gameObject, transform.GetChild(2));

            card.transform.parent = transform.GetChild(2) ;
			card.transform.localScale = new Vector2(1, 1);
			card.name = "card" + (i + 1);
			card.SetActive (true);
			card.transform.GetChild(0).gameObject.SetActive (false);
			card.transform.GetChild(1).gameObject.SetActive (true);
			//поворот карты
			if (i == 0) card.transform.rotation = Quaternion.Euler(0, 0, 4); else if (i == 2) card.transform.rotation = Quaternion.Euler(0, 0, -5); else if (i == 3) card.transform.rotation = Quaternion.Euler(0, 0, -12); else if (i == 4) card.transform.rotation = Quaternion.Euler(0, 0, 13);

			/*
			//иконка
            icon = Instantiate(icons.FindChild(bonusName).gameObject, icons.FindChild(bonusName).transform.position + cards[i].transform.position, Quaternion.identity) as GameObject;
			icon.transform.parent = cardIcons[i];
            icon.transform.localScale = new Vector2(1, 1);
            icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, 0);
            icon.SetActive(true);
            */
			//сохранение результата
            //if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins") ctrProgressClass.progress[bonusName] += bonusCount;
            if (bonusName == "energy") {
                ctrProgressClass.progress["energyTime"] -= bonusCount * lsEnergyClass.costEnergy;
                lsEnergyClass.energy += bonusCount;
				//если находимся на карте, то перекрашиваем полоску энергии

				if (GameObject.Find ("root/static/energy/energy line") != null) {
					GameObject.Find ("root/static/energy/energy line").GetComponent<UISprite>().fillAmount = 1 - (float)ctrProgressClass.progress ["energy"] / lsEnergyClass.maxEnergy;
					if (lsEnergyClass.energyInfinity)
						GameObject.Find ("root/static/energy/energy line").GetComponent<UISprite>().fillAmount = 1;
				}

            }
            else {
                ctrProgressClass.progress[bonusName] += bonusCount;
                if (bonusName == "coins") ctrAnalyticsClass.sendEvent("Coins", new Dictionary<string, string> { { "income", "booster" }, { "coins", bonusCount.ToString() } });


            }
            if (initLevelMenuClass.instance != null) {
				if (initLevelMenuClass.instance.coinsLabel != null)
					initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();
				if (initLevelMenuClass.instance.energyLabel != null)
					initLevelMenuClass.instance.energyLabel.text = ctrProgressClass.progress ["energy"].ToString ();
			}
			
 
        }
		GetComponent<Animator> ().Rebind ();
		GetComponent<Animator>().Play("card enable");

        ctrProgressClass.progress[currentBoosterColor]--;
        buttonOpenBooster.transform.GetChild(1).GetComponent<UILabel>().text = ctrProgressClass.progress[currentBoosterColor].ToString();

        marketClass.instance.boostersLabel[0].text = ctrProgressClass.progress["boostersWhite"].ToString();
        marketClass.instance.boostersLabel[1].text = ctrProgressClass.progress["boostersGreen"].ToString();
        marketClass.instance.boostersLabel[2].text = ctrProgressClass.progress["boostersBlue"].ToString();
        marketClass.instance.boostersLabel[3].text = ctrProgressClass.progress["boostersPurple"].ToString();

        ctrProgressClass.saveProgress();
        //убираем бустер
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(0.01F);

    }

    public static void setOpeningCardCommon(ref Dictionary<string, int> portions, Dictionary<string, int> portionsCount, ref List<KeyValuePair<string, int>> openingCards)
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
        portions[nameBonus] = Mathf.RoundToInt(portions[nameBonus]/1.5F);
        bonusRand = UnityEngine.Random.Range(0, 100);
        float part = 100/(((2 + portionsCount[nameBonus] - 1)/2) * portionsCount[nameBonus]);
        countBonus = 1 + portionsCount[nameBonus] - Mathf.CeilToInt(bonusRand/part);
        openingCards.Add(new KeyValuePair<string, int>(nameBonus, countBonus));

        //Debug.Log(nameBonus + " " + countBonus);
    }

    public static void setOpeningCardUncommon( string itemName, ref List<KeyValuePair<string, int>> openingCards)
    {
        int number = UnityEngine.Random.Range(2, 6);
        if (number == ctrProgressClass.progress[itemName + "Rare"]) number = UnityEngine.Random.Range(2, 6);
        if (number == ctrProgressClass.progress[itemName + "Rare"]) ctrProgressClass.progress[itemName + "Rare"] = 0;
        int i = 1;
        for (i = 1; i <= 5; i++)
        {
            if (ctrProgressClass.progress[itemName + i] == 0 && i != ctrProgressClass.progress[itemName + "Rare"]) break;
        }
        if (i == 6) ctrProgressClass.progress[itemName + "Rare"] = 0;

        //Debug.Log(itemName + number + " " + 1);

        openingCards.Add(new KeyValuePair<string, int>(itemName + number, 1));

    }

    public  static void Shuffle<T>(IList<T> list)
    {
        Random random = new Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
