using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class mBoosterClass : MonoBehaviour {

    //public GameObject[] cards = new GameObject[5];
	//public Transform[] cardIcons = new Transform[5];
	public Transform cardsAll;
    public static int counterOpenCard;
    public Transform exitOpenBoosterMenu;
    public GameObject buttonOpenBooster;

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
		transform.GetChild(2).DestroyChildren();
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

        
        GameObject card = new GameObject();
        //GameObject bonusesTemp = GameObject.Instantiate(GameObject.Find("bonuses temp"), new Vector2(0, 0), Quaternion.identity) as GameObject;
        //bonusesTemp.transform.parent = giftMenu.transform.GetChild(0).GetChild(2);
        //bonusesTemp.transform.localScale = new Vector3(1, 1, 1);
        //шансы на картах
        Dictionary<string, int> portions = new Dictionary<string, int>();
		Dictionary<string, int> portions2 = new Dictionary<string, int>();

		portions["hints_1"] = 190; portions["hints_2"] = 380; portions["hints_3"] = 570;
		portions["webs_1"] = 50; portions["webs_2"] = 100; portions["webs_3"] = 150;
		portions["teleports_1"] = 50; portions["teleports_2"] = 100; portions["teleports_3"] = 150;
		portions["collectors_1"] = 100; portions["collectors_2"] = 200; portions["collectors_3"] = 300;
		portions["coins_50"] = 50; portions["coins_100"] = 100; portions["coins_250"] = 250;
		portions["energy_5"] = 85; portions["energy_10"] = 170; portions ["energy_15"] = 255;
		portions["skin2_0"] = 4000; portions["skin3_0"] = 5000; portions["skin4_0"] = 6000; portions ["skin5_0"] = 7000;
		portions["berry2_0"] = 1000; portions["berry3_0"] = 2000; portions["berry4_0"] = 3000; portions ["berry5_0"] = 5000;
		portions["hat2_0"] = 2000; portions["hat3_0"] = 3000; portions["hat4_0"] = 4000; portions ["hat5_0"] = 5000;

		int portionsSum = 0;
		int portionsSum2 = 0;


		foreach (var e in portions) {
			portionsSum += e.Value;
		}
		foreach (var e in portions) {

			portions2[e.Key] =  portionsSum / e.Value;
			portionsSum2 += portions2 [e.Key];
		}

		for (int i = 0; i < 5; i++) {
            //cards[i].SetActive(true);
            int counter = 0;
			int counterArr = 0;
			int bonusRand = Mathf.CeilToInt(UnityEngine.Random.Range(0, portionsSum2));



            foreach (var portion in portions2 ) {
                if (bonusRand >= counter && bonusRand < counter + portion.Value) {
					//название карты и количество
					string bonusName = portion.Key.Split(new Char[] { '_' })[0];
					int bonusCount = int.Parse(portion.Key.Split(new Char[] { '_' })[1]);

					//убираем дубликаты карт
					if (bonusCount == 0)
					if (ctrProgressClass.progress [bonusName] > 0) {
						i--;
						break;
					}
						
					//копируем карту
					card = Instantiate(cardsAll.GetChild(counterArr).gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
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
                    if (bonusName == "hints" || bonusName == "webs" || bonusName == "teleports" || bonusName == "collectors" || bonusName == "coins") ctrProgressClass.progress[bonusName] += bonusCount;
                    else if (bonusName == "energy") {
                        ctrProgressClass.progress["energyTime"] -= bonusCount * lsEnergyClass.costEnergy;
                        ctrProgressClass.progress["energy"] += bonusCount;
						//если находимся на карте, то перекрашиваем полоску энергии

						if (GameObject.Find ("root/static/energy/energy line") != null) {
							GameObject.Find ("root/static/energy/energy line").GetComponent<UISprite>().fillAmount = 1 - (float)ctrProgressClass.progress ["energy"] / lsEnergyClass.maxEnergy;
							if (lsEnergyClass.energyInfinity)
								GameObject.Find ("root/static/energy/energy line").GetComponent<UISprite>().fillAmount = 1;
						}

                    }
                    else {
                        if (ctrProgressClass.progress[bonusName] == 0) ctrProgressClass.progress[bonusName] = 1;

                    }
					if (initLevelMenuClass.instance != null) {
						if (initLevelMenuClass.instance.coinsLabel != null)
							initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress ["coins"].ToString ();
						if (initLevelMenuClass.instance.energyLabel != null)
							initLevelMenuClass.instance.energyLabel.text = ctrProgressClass.progress ["energy"].ToString ();
					}
					break;
                }
				counterArr ++;
                counter += portion.Value;
            }




        }
		GetComponent<Animator> ().Rebind ();
		GetComponent<Animator>().Play("card enable");

        ctrProgressClass.progress["boosters"]--;
        buttonOpenBooster.transform.GetChild(1).GetComponent<UILabel>().text = ctrProgressClass.progress["boosters"].ToString();
        marketClass.instance.boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();
        ctrProgressClass.saveProgress();
        //убираем бустер
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(0.01F);

    }


}
