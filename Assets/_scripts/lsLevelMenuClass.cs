using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if (UNITY_ANDROID || UNITY_IOS) && UNITY_UNITYADS_API && ENABLE_UNITYADS_RUNTIME && !UNITY_EDITOR
using UnityEngine.Advertisements;
#endif

public class lsLevelMenuClass : MonoBehaviour
{
    public GameObject levelMenu;
    public UILabel titleNumberLevel;
    public GameObject stars1;
    public GameObject stars2;
    public GameObject time;
    public GameObject web;
    public GameObject sluggish;
    public GameObject destroyer;
    public GameObject yeti;
    public GameObject groot;
    public GameObject gem1Active;
    public GameObject gem2Active;
    public GameObject checked1;
    public GameObject checked2;
    public GameObject hand;
    public GameObject rateUsMenu;
    public GameObject adTiredMenu;

    public List<GameObject> completePoints;

    private int tabCounter = 0;

    public void setDefault()
    {
        // to default
        levelMenu.transform.GetChild(0).GetChild(0).GetComponent<UIToggle>().value = true;
        time.SetActive(false);
        web.SetActive(false);
        sluggish.SetActive(false);
        destroyer.SetActive(false);
        yeti.SetActive(false);
        groot.SetActive(false);
        for (int i = 1; i <= 3; i++)
        {
            stars2.transform.GetChild(i - 1).gameObject.SetActive(false);
        }
        gem1Active.SetActive(true);
        gem2Active.SetActive(true);
        checked1.SetActive(false);
        checked2.SetActive(false);
        //выключаем 2ю вкладку на начальных уровнях
        if (ctrProgressClass.progress["currentLevel"] <= 4)
            levelMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        else
            levelMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);

    }

    public void setContent2()
    {

        //init content2.stars
        int levelDemandsStars = staticClass.levels[ctrProgressClass.progress["currentLevel"], 0];
        for (int i = 1; i <= levelDemandsStars; i++)
        {
            stars2.transform.GetChild(i - 1).gameObject.SetActive(true);
        }
        stars2.transform.GetChild(3).GetComponent<UILabel>().text = levelDemandsStars.ToString();

        int levelDemands = staticClass.levels[ctrProgressClass.progress["currentLevel"], 1];
        //если условие только одно (про звезды)
        stars2.transform.localPosition = new Vector3(stars2.transform.localPosition.x, 0,
            stars2.transform.localPosition.z);
        if (levelDemands == 0)
        {
            stars2.transform.localPosition = new Vector3(stars2.transform.localPosition.x, -73,
                stars2.transform.localPosition.z);
            //остальные условия
        }
        else if (levelDemands >= 1 && levelDemands <= 99)
        {
            time.SetActive(true);
            if (levelDemands < 10)
                time.transform.GetChild(0).GetComponent<UILabel>().text = "0" + levelDemands.ToString();
            else time.transform.GetChild(0).GetComponent<UILabel>().text = levelDemands.ToString();
        }
        else if (levelDemands >= 100 && levelDemands <= 199)
        {
            web.SetActive(true);
            web.transform.GetChild(0).GetComponent<UILabel>().text = (levelDemands - 100).ToString();
            web.transform.GetChild(1).GetComponent<UILabel>().text = (levelDemands - 100).ToString();
            if (ctrProgressClass.progress["language"] == 1)
            {
                if (levelDemands - 100 > 1)
                    web.transform.GetChild(1).GetComponent<UILabel>().text = " " + (levelDemands - 100).ToString() + " times";
                else web.transform.GetChild(1).GetComponent<UILabel>().text = " " + (levelDemands - 100).ToString() + " time";
            }
            else
            {
                if (levelDemands - 100 > 1)
                    web.transform.GetChild(1).GetComponent<UILabel>().text = (levelDemands - 100).ToString() + " раз";
                else web.transform.GetChild(1).GetComponent<UILabel>().text = (levelDemands - 100).ToString() + " раза";

            }


        }
        else if (levelDemands == 201)
        {
            sluggish.SetActive(true);
        }
        else if (levelDemands == 202)
        {
            destroyer.SetActive(true);
        }
        else if (levelDemands == 203)
        {
            yeti.SetActive(true);
        }
        else if (levelDemands == 204)
        {
            groot.SetActive(true);
        }
    }

    // Use this for initialization
    public void levelMenuEnable()
    {

        setDefault();

        //init gems
        int levelProgress = ctrProgressClass.progress["level" + ctrProgressClass.progress["currentLevel"]];
        if (levelProgress == 1 || levelProgress == 3)
        {
            gem1Active.SetActive(false);
            Debug.Log("ckecked 1");
            checked1.SetActive(true);
        }
        if (levelProgress == 2 || levelProgress == 3)
        {
            gem2Active.SetActive(false);
            checked2.SetActive(true);
        }
        //reward for login
        if (ctrProgressClass.progress["rewardLogin"] == 1) transform.GetChild(4).gameObject.SetActive(false);

        titleNumberLevel.text = ctrProgressClass.progress["currentLevel"].ToString();
        setContent2();
    }

    //public void completeMenuEnable(int bonusTime, int bonusLevel, bool gem, int starsCount) {
    public void completeMenuEnable(float timeLevel, bool gem, int starsCount, int lvlNumber)
    {
        titleNumberLevel.text = ctrProgressClass.progress["currentLevel"].ToString();
        
        //выключаем root
        //GameObject.Find("root").SetActive(false);
        //выключаем bonuses
        //GameObject.Find("/default level/gui/bonuses/").SetActive(false);

        setDefault();
        stars1.SetActive(false);
        stars2.SetActive(false);
        Transform score = levelMenu.transform.GetChild(0).GetChild(4);
        score.gameObject.SetActive(true);
        if (initLevelMenuClass.levelDemands == 1)
        {
            levelMenu.transform.GetChild(0).transform.GetChild(0).GetComponent<UIToggle>().value = false;
            levelMenu.transform.GetChild(0).GetChild(1).GetComponent<UIToggle>().value = true;
        }
        //D/ebug.Log ("completeMenuEnable");

        //for tests
        GameObject.Find("for test timer").GetComponent<UILabel>().text =
            (Mathf.Ceil(Time.timeSinceLevelLoad*100)/100).ToString();
        GameObject.Find("for test timer").GetComponent<UILabel>().text = (Mathf.Ceil(timeLevel*100)/100).ToString();
        //D/ebug.Log ("timeLevel: " + timeLevel);
        //D/ebug.Log ("gem: " + gem);
        //D/ebug.Log ("starsCount: " + starsCount);
        //D/ebug.Log ("lvlNumber: " + lvlNumber);

        StartCoroutine(coroutineCompleteMenuScore(timeLevel, gem, starsCount, lvlNumber));




    }

    IEnumerator coroutineCompleteMenuScore(float timeLevel, bool gem, int starsCount, int lvlNumber)
    {
        StartCoroutine(coroutineLevelPoints(timeLevel, starsCount, lvlNumber));
        //D/ebug.Log("initLevelMenuClass.levelDemands: " + initLevelMenuClass.levelDemands);

        //убираем stars, меняем иконку плей на рестарт
        if (initLevelMenuClass.levelDemands == 1)
        {
            //transform.GetChild (1).gameObject.SetActive (false);
            //у первой вкладки плей, у второй рестарт
            transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(3).GetChild(7).GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(3).GetChild(7).GetChild(0).gameObject.SetActive(true);
        }
        //init gems (если уже получил кристалл, то показываем)
        int levelProgress = ctrProgressClass.progress[SceneManager.GetActiveScene().name];
        if ((levelProgress == 1 || levelProgress == 3) && !(gem && initLevelMenuClass.levelDemands == 0))
        {
            gem1Active.SetActive(false);
            checked1.SetActive(true);
        }
        if ((levelProgress == 2 || levelProgress == 3) && !(gem && initLevelMenuClass.levelDemands == 1))
        {
            gem2Active.SetActive(false);
            checked2.SetActive(true);
        }

        //определение переменных
        Transform scoreTransform = levelMenu.transform.GetChild(0).GetChild(4);
        //Transform grid = levelMenu.transform.GetChild (0).GetChild (4).GetChild (0);
        //UILabel scoreLvl = grid.GetChild (1).GetChild (0).GetComponent<UILabel>();
        //UILabel scoreCh = grid.GetChild (3).GetChild (0).GetComponent<UILabel>();
        int score1 = 0;
        int score2 = ctrProgressClass.progress["score" + lvlNumber + "_2"];
        int scoreFinal = 0;

        //включаем или отключаем строчки с очками в grid, в зависимости от типа прохождения levelDemands
        /*
        if (initLevelMenuClass.levelDemands == 0) {
			grid.GetChild (0).gameObject.SetActive (true);
			grid.GetChild (1).gameObject.SetActive (true);
			grid.GetChild (2).gameObject.SetActive (true);
			grid.GetChild (3).gameObject.SetActive (false);
		} else {
			grid.GetChild (0).gameObject.SetActive (false);
			grid.GetChild (1).gameObject.SetActive (false);
			grid.GetChild (2).gameObject.SetActive (false);
			grid.GetChild (3).gameObject.SetActive (true);
		}
        */

        //подсчет score1 и score2
        //all average score = 10500
        //в 2 раза уменьшаем score
        if (initLevelMenuClass.levelDemands == 0) score1 = Mathf.RoundToInt((2000*starsCount + 3000 - 100*timeLevel)/2);
        else score1 = ctrProgressClass.progress["score" + lvlNumber + "_1"];
        if (score1 < 0) score1 = 0;
        if (levelProgress == 2 || levelProgress == 3) score2 = 1500;

        Transform scoreAll = levelMenu.transform.GetChild(0).GetChild(4).GetChild(1);

        //подсчет scoreFinal
        //если текущие очки больше чем были минимум на 100, то добавляем coins (минимум 1 coins)
        if (score1 + score2 -
            (ctrProgressClass.progress["score" + lvlNumber + "_1"] +
             ctrProgressClass.progress["score" + lvlNumber + "_2"]) >
            100)
        {
            scoreFinal = score1 + score2;
            //включаем coins = all score / 100
            Debug.Log(scoreAll.GetChild(2).name + " sfsdf");
            scoreAll.GetChild(2).gameObject.SetActive(true);
            //добавление процента от карт
            int percent = 100;
            for (int e = 2; e <= 5; e++)
                if (ctrProgressClass.progress["hat" + e] >= 1) percent += e*5*ctrProgressClass.progress["hat" + e];
            if (percent == 170) percent = 200;
            //D/ebug.Log ("percent: " + percent);
            //D/ebug.Log ("scoreFinal: " + scoreFinal);
            //D/ebug.Log (ctrProgressClass.progress["score" + lvlNumber + "_1"] + ctrProgressClass.progress["score" + lvlNumber + "_2"]);
            int coinsAdd =
                Mathf.RoundToInt(((scoreFinal -
                                   (ctrProgressClass.progress["score" + lvlNumber + "_1"] +
                                    ctrProgressClass.progress["score" + lvlNumber + "_2"]))*percent)/100/100);
            if (coinsAdd < 0) coinsAdd = 0;
            scoreAll.GetChild(2).GetChild(1).GetComponent<UILabel>().text = coinsAdd.ToString();
            ctrProgressClass.progress["coins"] += coinsAdd;

            if (coinsAdd > 0)
                ctrAnalyticsClass.sendEvent("Coins",
                    new Dictionary<string, string> {{ "detail 1", "level"}, {"coins", coinsAdd.ToString()}});
            ctrStatsClass.logEvent("coins", "free", "level" + lvlNumber, coinsAdd);

            //сохранение очков в Kii
            ctrFbKiiClass.updateUserScore("level" + lvlNumber.ToString(), scoreFinal,
                ctrProgressClass.progress["lastLevel"]);

        }
        else
        {
            scoreFinal = ctrProgressClass.progress["score" + lvlNumber + "_1"] +
                         ctrProgressClass.progress["score" + lvlNumber + "_2"];
            //поправляем position total score label
            scoreAll.GetChild(1).localPosition = new Vector3(-59, -84, 0);

        }
        //

        //сохранение очков в progress
        if (ctrProgressClass.progress["score" + lvlNumber + "_1"] < score1)
            ctrProgressClass.progress["score" + lvlNumber + "_1"] = score1;
        if (ctrProgressClass.progress["score" + lvlNumber + "_2"] < score2)
            ctrProgressClass.progress["score" + lvlNumber + "_2"] = score2;
        ctrProgressClass.saveProgress();

        //hand
        //if tutorial booster == 0 and coins > 400 
        //or tutorialAdCoins
        bool tutorialAdCoins = ctrProgressClass.progress["tutorialAdCoins"] == 0 &&
                               new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(ctrProgressClass.progress["adCoinsDate"]) <
                               DateTime.Now && ctrProgressClass.progress["adCoinsDate"] != 0;

        if ((ctrProgressClass.progress["tutorialBuy"] == 0 && ctrProgressClass.progress["coins"] >= 400) || tutorialAdCoins || (lvlNumber >= 6 && ctrProgressClass.progress["tutorialFreeCoins"] == 0))
        {
            Debug.Log("tutorialAdCoins: " + ctrProgressClass.progress["tutorialAdCoins"]);
            Debug.Log("tutorialFreeCoins: " + ctrProgressClass.progress["tutorialFreeCoins"]);
            GameObject handGO = Instantiate(hand, new Vector2(0, 0), Quaternion.identity);
            handGO.transform.parent = GameObject.Find("/default level/gui/complete menu").transform;
            handGO.transform.GetChild(0).GetChild(0).gameObject.layer = LayerMask.NameToLayer("Water");
            handGO.transform.GetChild(0).GetChild(1).gameObject.layer = LayerMask.NameToLayer("Water");
            handGO.transform.GetChild(0).GetChild(2).gameObject.layer = LayerMask.NameToLayer("Water");

            handGO.transform.localScale = new Vector3(-1, 1, 1);
            handGO.transform.localPosition = new Vector3(-328, -739, 0);
            handGO.transform.rotation = Quaternion.Euler(0, 0, 109);
            handGO.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 165;
            handGO.transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 165;
            handGO.transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 164;


        }

        //анимация финальных очков
        scoreAll.gameObject.SetActive(true);
        UILabel scoreAllLbl = scoreAll.GetChild(0).GetComponent<UILabel>();
        for (int i = 0; i <= 100; i += 5)
        {
            scoreAllLbl.text = (Mathf.Round(scoreFinal*i/100)).ToString();
            yield return new WaitForSeconds(0.05F);
            //анимация звезды 1
            if (starsCount >= 1 && i == 0)
            {
                transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(3).GetChild(0).GetComponent<AudioSource>().Play();
            }
            //анимация звезды 2
            if (starsCount >= 2 && i == 50)
            {
                transform.GetChild(1).GetChild(1).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(3).GetChild(1).GetComponent<AudioSource>().Play();
            }
        }

        //анимация звезды 3
        if (starsCount >= 3)
        {
            transform.GetChild(1).GetChild(2).GetChild(0).gameObject.SetActive(true);
            transform.GetChild(3).GetChild(2).GetComponent<AudioSource>().Play();
        }

        //если еще не получал кристалл
        if (initLevelMenuClass.levelDemands == 0 && gem)
        {
            //анимация получения ключа, исчезание и появление галочки
            gem1Active.SetActive(true);
            gem1Active.GetComponent<Animator>().enabled = true;
            transform.GetChild(3).GetChild(3).GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.8F);
            gem1Active.transform.parent.GetChild(1).gameObject.SetActive(true);
        }
        if (initLevelMenuClass.levelDemands == 1 && gem)
        {
            //анимация получения ключа, исчезание и появление галочки
            gem2Active.SetActive(true);
            gem2Active.GetComponent<Animator>().enabled = true;
            transform.GetChild(3).GetChild(3).GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.8F);
            gem2Active.transform.parent.GetChild(1).gameObject.SetActive(true);
        }




        //yield return new WaitForSeconds (1F);
        //grid.parent.gameObject.SetActive (false);


        //stars1.SetActive(true);
        //stars2.SetActive(true);


        //setContent2();
    }

    public void stopCompleteMenuAnimation()
    {
        tabCounter++;
        if (tabCounter > 2)
        {

            stars1.SetActive(true);
            stars2.SetActive(true);
            transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
            setContent2();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (name == "complete menu")
        {
            //обработка кнопки "Назад" на Android
            if (Input.GetButtonDown("Cancel"))
            {
                transform.GetChild(9).SendMessage("OnPress", false);
            }
        }
    }

    public void clickTabCompleteMenu(string name)
    {
        Debug.Log("clickTabCompleteMenu: " + name);
        Transform grid = levelMenu.transform.GetChild(0).GetChild(4).GetChild(0);

        //оставляем scores
        if (initLevelMenuClass.levelDemands == 0 && name == "Tab 1" ||
            initLevelMenuClass.levelDemands == 1 && name == "Tab 2")
        {
            grid.parent.gameObject.SetActive(true);
            //levelMenu.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
            //levelMenu.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
            stars1.SetActive(false);
            stars2.SetActive(false);
            setDefault();
            int levelProgress = ctrProgressClass.progress["level" + ctrProgressClass.progress["currentLevel"]];
            if (levelProgress == 1 || levelProgress == 3)
            {
                gem1Active.SetActive(false);
                checked1.SetActive(true);
            }
            if (levelProgress == 2 || levelProgress == 3)
            {
                gem2Active.SetActive(false);
                checked2.SetActive(true);
            }

        }
        else
        {
            grid.parent.gameObject.SetActive(false);
            stars1.SetActive(true);
            stars2.SetActive(true);
            setContent2();
        }
    }


    IEnumerator coroutineLevelPoints(float timeLevel, int starsCount, int lvlNumber)
    {
        var stars1 = levelMenu.transform.GetChild(0).GetChild(4).GetChild(2).gameObject;
        var stars2 = levelMenu.transform.GetChild(0).GetChild(4).GetChild(3).gameObject;
        var time = levelMenu.transform.GetChild(0).GetChild(4).GetChild(4).gameObject;
        var web = levelMenu.transform.GetChild(0).GetChild(4).GetChild(5).gameObject;
        var sluggish = levelMenu.transform.GetChild(0).GetChild(4).GetChild(6).gameObject;
        var destroyer = levelMenu.transform.GetChild(0).GetChild(4).GetChild(7).gameObject;
        var yeti = levelMenu.transform.GetChild(0).GetChild(4).GetChild(8).gameObject;
        var groot = levelMenu.transform.GetChild(0).GetChild(4).GetChild(9).gameObject;

        //normal stars or challenge stars
        if (initLevelMenuClass.levelDemands == 0) stars1.SetActive(true);
        else
        {
            stars2.SetActive(true);



            //init content2.stars
            int levelDemandsStars = staticClass.levels[ctrProgressClass.progress["currentLevel"], 0];
            stars2.transform.GetChild(0).GetComponent<UILabel>().text = levelDemandsStars.ToString();

            int levelDemands = staticClass.levels[ctrProgressClass.progress["currentLevel"], 1];
            //если условие только одно (про звезды)
            stars2.transform.localPosition = new Vector3(stars2.transform.localPosition.x, 310,
                stars2.transform.localPosition.z);
            if (levelDemands == 0)
            {
                stars2.transform.localPosition = new Vector3(stars2.transform.localPosition.x, 245,
                    stars2.transform.localPosition.z);
                //остальные условия
            }
            else if (levelDemands >= 1 && levelDemands <= 99)
            {
                time.SetActive(true);
                if (levelDemands < 10)
                    time.transform.GetChild(0).GetComponent<UILabel>().text = "0" + levelDemands.ToString();
                else time.transform.GetChild(0).GetComponent<UILabel>().text = levelDemands.ToString();
            }
            else if (levelDemands >= 100 && levelDemands <= 199)
            {
                web.SetActive(true);
                if (ctrProgressClass.progress["language"] == 1)
                {
                    if (levelDemands - 100 > 1)
                        web.transform.GetChild(0).GetComponent<UILabel>().text = " " + (levelDemands - 100).ToString() + " times";
                    else web.transform.GetChild(0).GetComponent<UILabel>().text = " " + (levelDemands - 100).ToString() + " time";
                }
                else
                {
                    if (levelDemands - 100 > 1)
                        web.transform.GetChild(0).GetComponent<UILabel>().text = (levelDemands - 100).ToString() + " раз";
                    else web.transform.GetChild(0).GetComponent<UILabel>().text = (levelDemands - 100).ToString() + " раза";

                }
                

            }
            else if (levelDemands == 201)
            {
                sluggish.SetActive(true);
            }
            else if (levelDemands == 202)
            {
                destroyer.SetActive(true);
            }
            else if (levelDemands == 203)
            {
                yeti.SetActive(true);
            }
            else if (levelDemands == 204)
            {
                groot.SetActive(true);
            }
        }
        yield return new WaitForSeconds(0.3F);

        //icon v
        if (initLevelMenuClass.levelDemands == 0 && starsCount == 3)
        {
            stars1.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (initLevelMenuClass.levelDemands == 1)
        {
             if (staticClass.levels[lvlNumber, 0] == starsCount) stars2.transform.GetChild(1).gameObject.SetActive(true);


            int levels = staticClass.levels[lvlNumber, 1];
            yield return new WaitForSeconds(0.2F);
            if (levels >= 1 && levels <= 99 && timeLevel <= levels) time.transform.GetChild(1).gameObject.SetActive(true);
            else if (levels >= 100 && levels <= 199 && staticClass.useWeb <= (levels - 100)) web.transform.GetChild(1).gameObject.SetActive(true);
            else if (levels == 201 && staticClass.useSluggish == false) sluggish.transform.GetChild(1).gameObject.SetActive(true);
            else if (levels == 202 && staticClass.useDestroyer == false) destroyer.transform.GetChild(1).gameObject.SetActive(true);
            else if (levels == 203 && staticClass.useYeti == false) yeti.transform.GetChild(1).gameObject.SetActive(true);
            else if (levels == 204 && staticClass.useGroot == false) groot.transform.GetChild(1).gameObject.SetActive(true);

        }








        yield return new WaitForSeconds(0.05F);


    }
}
