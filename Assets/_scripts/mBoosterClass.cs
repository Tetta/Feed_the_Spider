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
    //public static int counterOpenCard;
    public Transform exitOpenBoosterMenu;
    public GameObject buttonOpenBooster;
    public string itemName = "";
    private bool boostersInInv = false;
    //public List<string> openingCardsList;

    //private string currentBoosterColor;
    //private List <KeyValuePair<string, int>> openingCards = new List<KeyValuePair<string, int>>();

    public Dictionary<string, int> openingCards = new Dictionary<string, int>();


    public static mBoosterClass instance = null;

    void Awake() {
        Debug.Log("start booster");
        if ( ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.parent.parent);
            //revert openingCards
            string openingCardStr = PlayerPrefs.GetString("cards");


            string strKey = "", strValue = "";
            bool flag = true;
            for (int i = 0; i < openingCardStr.Length; i++)
            {
                if (openingCardStr.Substring(i, 1) == "=") flag = false;

                else if (openingCardStr.Substring(i, 1) == ";")
                {
                    flag = true;
                    openingCards[strKey] = int.Parse(strValue);
                    strKey = "";
                    strValue = "";
                }
                else if (flag) strKey += openingCardStr.Substring(i, 1);
                else if (!flag) strValue += openingCardStr.Substring(i, 1);
            }
            Debug.Log("cards: " + openingCards.Count);
            existBoosters();
            Debug.Log("boostersInInv: " + boostersInInv);
            if (openingCards.Count == 0 && !boostersInInv) transform.parent.parent.gameObject.SetActive(false);

        }
        else
        {

            Destroy(transform.parent.parent.gameObject);
            return;
        }
        
    }

    void OnEnable() {
        Debug.Log("enable booster");
        bool flag2 = false;
        //удаляем карты в бустере
        transform.GetChild(2).DestroyChildren();
 


        

        //отключаем все спрайты бустера
        for (int i = 0; i < 9; i++)
        {
            transform
                .GetChild(0)
                .GetChild(0)
                .GetChild(i)
                .gameObject.SetActive(false);
        }
        //включаем, какой открываем
        transform
            .GetChild(0)
            .GetChild(0)
            .Find(itemName)
            .gameObject.SetActive(true);



        //label
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        if (staticClass.getBoosterForOK && ctrProgressClass.progress["rewardLogin"] == 1)
        {
            //label
            transform.GetChild(4).gameObject.SetActive(true);
            staticClass.getBoosterForOK = false;
        }

        transform.GetChild(1).gameObject.GetComponent<ctrParticleSystem>().enabled = false;
        exitOpenBoosterMenu.localPosition = new Vector3(0, 0, -10000);


        if (openingCards.Count != 0 || boostersInInv)
        {
            boostersInInv = false;
            transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(coroutinePressBooster());
            return;
        }


        //показываем бустер
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
       
    }

    void OnPress(bool isPressed) {

        if (!isPressed) {
            StartCoroutine(coroutinePressBooster());
        }

    }

    public IEnumerator coroutinePressBooster() {
        GameObject card;
        //заполняем openingCards
        if (openingCards.Count == 0) setOpeningCards();

        List<string> openingCardsList = new List<string>(openingCards.Keys);
        Shuffle(openingCardsList);
        Debug.Log("press booster");
        int ii = 0;
        
        Dictionary<string, int> openingCardsTemp = new Dictionary<string, int>(openingCards);
        openingCards.Clear();
        foreach (var currentCardName in openingCardsList)
        {
            openingCards.Add(currentCardName, openingCardsTemp[currentCardName]);
        }
        
        foreach (var currentCardName in openingCardsList)
        {

            //название карты и количество
            string bonusName = currentCardName;
			int bonusCount = openingCards[currentCardName];
            Debug.Log(bonusName + " " + bonusCount);


            //копируем карту
            card = Instantiate(cardsAll.Find(bonusName).gameObject, transform.GetChild(2));

            card.transform.localPosition = new Vector3(0, 0, 0);
            card.transform.rotation = Quaternion.Euler(0, 0, Mathf.CeilToInt(UnityEngine.Random.Range(-5, 5)));
                
            //change label count
            card.transform.GetChild(0).GetChild(3).GetChild(3).GetChild(0).GetComponent<UILabel>().text = bonusCount.ToString();
            card.transform.localScale = new Vector2(1, 1);
            card.name = bonusName;
            card.SetActive (true);
			card.transform.GetChild(0).gameObject.SetActive (false);
			card.transform.GetChild(1).gameObject.SetActive (true);


            //card colors
            if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "gems" || bonusName == "coins") card.transform.GetChild(1).GetChild(0).GetComponent<UISprite>().color = new Color32(154, 138, 123, 255);// new Color(154, 138, 123, 1);
            else if (bonusName.Substring(0, 3) == "hat") card.transform.GetChild(1).GetChild(0).GetComponent<UISprite>().color = new Color32(58, 67, 112, 255);
            else if (bonusName.Length > 5 && bonusName.Substring(0, 5) == "berry") card.transform.GetChild(1).GetChild(0).GetComponent<UISprite>().color = new Color32(58, 112, 78, 255);

            ii++;
            //переворот верхней(последней) карты

            addLayerToCard(ref  card,  ii);
            if (ii == openingCardsList.Count) StartCoroutine( card.GetComponent<mCardClass>().openCard(true));


            if (initLevelMenuClass.instance != null) {
				if (initLevelMenuClass.instance.coinsLabel != null)
					initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();
			}
        }
        //GetComponent<Animator> ().Rebind ();
        //GetComponent<Animator>().Play("card enable");

        //ctrProgressClass.progress[currentBoosterColor]--;
        
        //staticClass.setBoostersLabels();

        //ctrProgressClass.saveProgress();
        //убираем бустер
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(0.01F);

    }

    public static void setOpeningCardCommon(ref Dictionary<string, int> portions, Dictionary<string, int> portionsCount, ref Dictionary<string, int> openingCards)
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
        
        if (!openingCards.ContainsKey(nameBonus)) openingCards[nameBonus] = 0;
        openingCards[nameBonus] += countBonus;
        //Debug.Log(nameBonus + " " + countBonus);
    }

    public static void setOpeningCardUncommon( string itemName, ref Dictionary<string, int> openingCards)
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
        if (!openingCards.ContainsKey(itemName + number)) openingCards[itemName + number] = 0;
        openingCards[itemName + number] ++;

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
    static void ChangeLayersRecursively(Transform trans, String name)
    {
        foreach (Transform child in trans)
        {
            child.gameObject.layer = LayerMask.NameToLayer(name);
            ChangeLayersRecursively(child, name);
        }
    }


    public static void addLayerToCard(ref GameObject card, int i)
    {
        //layers start
        var sprites = card.transform.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var sprite in sprites)
        {
            sprite.sortingLayerName = "card1";
            sprite.sortingOrder += 100 * i;
        }
        var panels = card.transform.GetComponentsInChildren<UIPanel>(true);
        foreach (var panel in panels)
        {
            //panel.sortingLayerName = "card" + (i + 1);
            panel.sortingLayerName = "card1";
            panel.sortingOrder += 100 * i;

        }
        //card.transform.GetComponentInChildren<ParticleSystem>(true).GetComponent<Renderer>().sortingLayerName = "card" + (i + 1);
        var pss = card.transform.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in pss)
        {
            //ps.GetComponent<Renderer>().sortingLayerName = "card" + (i + 1);
            ps.GetComponent<Renderer>().sortingLayerName = "card1";
            ps.GetComponent<Renderer>().sortingOrder += 100 * i;

        }
        card.GetComponent<UIPanel>().sortingOrder = i*100 + 164;
        card.GetComponent<UIPanel>().depth = i * 100 + 164;
        //card.AddComponent<UIWidget>().depth = i * 100 + 164;

        card.transform.localPosition = new Vector3(Mathf.CeilToInt(UnityEngine.Random.Range(-30, 30)), Mathf.CeilToInt(UnityEngine.Random.Range(-30, 30)), 0);
        //layers end
    }

    public void setOpeningCards()
    {

        transform.GetChild(1).gameObject.GetComponent<ctrParticleSystem>().enabled = true;
        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
        transform.GetChild(1).GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().Play();

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
        openingCards["coins"] = 0;
        openingCards["gems"] = 0;

        List<string> boosterNames = new List<string> { "boostersWhite", "boostersGreen", "boostersBlue", "boostersPurple" };
        foreach (var currentBoosterColor in boosterNames)
        {
            for (var b = 0; b < ctrProgressClass.progress[currentBoosterColor]; b++)
            {

                if (currentBoosterColor == "boostersWhite")
                {
                    openingCards["coins"] += 50;
                    openingCards["gems"] += 1;
                    var max = 5;
                    Debug.Log("tutorialBuy: " + ctrProgressClass.progress["tutorialBuy"]);
                    if (ctrProgressClass.progress["tutorialBuy"] < 5)
                    {
                        setOpeningCardUncommon("berry", ref openingCards);
                        max = 4;
                        ctrProgressClass.progress["tutorialBuy"] = 5;
                        ctrAnalyticsClass.sendEvent("Tutorial",
                            new Dictionary<string, string> { { "name", "open booster" } });

                    }
                    for (int i = 2; i < max; i++)
                    {
                        var r = UnityEngine.Random.Range(0, 100);
                        Debug.Log("rand: " + r);
                        if (r < 2) setOpeningCardUncommon("berry", ref openingCards);
                        else setOpeningCardCommon(ref portionsWhite, portionsCountWhite, ref openingCards);
                    }
                }

                if (currentBoosterColor == "boostersGreen")
                {
                    setOpeningCardUncommon("berry", ref openingCards);
                    openingCards["coins"] += 100;
                    openingCards["gems"] += 3;
                    for (int i = 3; i < 5; i++)
                    {
                        if (UnityEngine.Random.Range(0, 100) < 2) setOpeningCardUncommon("hat", ref openingCards);
                        else setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

                    }
                }
                if (currentBoosterColor == "boostersBlue")
                {
                    setOpeningCardUncommon("berry", ref openingCards);
                    setOpeningCardUncommon("hat", ref openingCards);
                    openingCards["coins"] += 150;
                    openingCards["gems"] += 7;
                    for (int i = 4; i < 5; i++)
                    {
                        if (UnityEngine.Random.Range(0, 100) < 2) setOpeningCardUncommon("skin", ref openingCards);
                        else setOpeningCardCommon(ref portionsGreen, portionsCountGreen, ref openingCards);

                    }
                }
                if (currentBoosterColor == "boostersPurple")
                {
                    setOpeningCardUncommon("berry", ref openingCards);
                    setOpeningCardUncommon("hat", ref openingCards);
                    setOpeningCardUncommon("skin", ref openingCards);
                    openingCards["coins"] += 200;
                    openingCards["gems"] += 20;
                }

            }
            ctrProgressClass.progress[currentBoosterColor] = 0;
            
        }
        //minus boosters, save cards
        Debug.Log("////////openingCards: " + openingCards.Count);
        saveCards();
        ctrProgressClass.saveProgress();

    }

    public void saveCards()
    {
        //if (openingCards.Count > 1)
        //{
            string strCards = "";
            foreach (var item in openingCards)
            {
                strCards += item.Key + "=" + item.Value + ";";
            }
            PlayerPrefs.SetString("cards", strCards);
            PlayerPrefs.Save();
        //}
    }

    public void existBoosters()
    {
        List<string> boosterNames = new List<string> { "boostersWhite", "boostersGreen", "boostersBlue", "boostersPurple" };
        foreach (var currentBoosterColor in boosterNames)
        {
            if (ctrProgressClass.progress[currentBoosterColor] > 0) boostersInInv = true;
        }
        
    }
}
