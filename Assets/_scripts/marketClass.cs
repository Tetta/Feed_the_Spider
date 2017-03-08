using UnityEngine;
using System.Collections;
//using UnionAssets.FLE;
using System.Collections.Generic;
using CompleteProject;

public class marketClass : MonoBehaviour {




    public static marketClass instance = null;
    public List <UILabel> boostersLabel;
    public GameObject openBoosterMenu;
    public GameObject boosterMenu;
	public Transform cardsAll;
	//public GameObject camera;

	public GameObject sale;
    public GameObject iconBoosterAnim;

    //init google market -> purchase -> OnProductPurchаsed (consume) -> OnProductConsumed



    // Use this for initialization
    void Start() {
		
		//дальше, если market
		if (ctrProgressClass.progress.Count == 0) {
            ctrProgressClass.getProgress();
        }
		//если бустер 3
		if (name == "booster_3") if (ctrProgressClass.progress["firstPurchase"] == 1) transform.GetChild(3).gameObject.SetActive(false);

		//если бустеры
		if (name != "market")
			return;
        
		if (instance != null) {
			//marketClass.instance.boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();
			Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        //listening for purchase and consume events
        /*
        AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;

		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;

		AndroidInAppPurchaseManager.Client.Connect();
        */

        instance.boostersLabel[0].text = ctrProgressClass.progress["boostersWhite"].ToString();
        instance.boostersLabel[1].text = ctrProgressClass.progress["boostersGreen"].ToString();
        instance.boostersLabel[2].text = ctrProgressClass.progress["boostersBlue"].ToString();
        instance.boostersLabel[3].text = ctrProgressClass.progress["boostersPurple"].ToString();

        gameObject.SetActive (false);
    }

    void OnPress(bool isPressed) {
            if (!isPressed) {
                //запрос на покупку
                Debug.Log("click buy " + name);
#if UNITY_IOS
            instance.GetComponent<Purchaser>().BuyProductID("com.mysterytag.spider." + name);
#else
            instance.GetComponent<Purchaser>().BuyProductID("com.evogames.feedthespider." + name);
#endif
            GetComponent<Animator>().Play("button");
            GetComponent<AudioSource>().Play();
            //if (name == "booster_3" && ctrProgressClass.progress ["firstPurchase"] == 0) 		
            //    AndroidInAppPurchaseManager.Client.Purchase ("booster_sale");
            //else
            //    AndroidInAppPurchaseManager.Client.Purchase (name);
        }

    }
    public static void buyChapter()
    {

            //запрос на покупку
            Debug.Log("buyChapter");
#if UNITY_IOS
            instance.GetComponent<Purchaser>().BuyProductID("com.mysterytag.spider.chapter");
#else
            instance.GetComponent<Purchaser>().BuyProductID("com.evogames.feedthespider.chapter");
#endif
    }


    public static void buyEnergy()
    {

        //запрос на покупку
        Debug.Log("buyEnergy");
#if UNITY_IOS
            instance.GetComponent<Purchaser>().BuyProductID("com.mysterytag.spider.energy_for_day");
#else
        instance.GetComponent<Purchaser>().BuyProductID("com.evogames.feedthespider.energy_for_day");
#endif
    }

    /*

        //подтверждение покупки товара
        private static void OnProductPurchased(BillingResult result) {

            if(result.IsSuccess) {
                AndroidInAppPurchaseManager.Client.Consume (result.Purchase.SKU);

            } else {
                //AndroidMessage.Create("Product Purchase Failed", result.response.ToString() + " " + result.message);
            }
        }

        //подтверждение использования товара
        private static void OnProductConsumed(BillingResult result) {

            if(result.IsSuccess) {
                int boostersCount = 0;
                float boostersPrice = 0;
                switch(result.Purchase.SKU) {
                case "booster_1":
                    boostersCount = 1;
                    boostersPrice = 0.99F;
                    break;
                case "booster_2":
                    boostersCount = 3;
                    boostersPrice = 2.99F;
                    break;
                case "booster_3":
                    boostersCount = 10;
                    boostersPrice = 7.99F;
                    break;
                case "booster_4":
                    boostersCount = 50;
                    boostersPrice = 34.99F;
                    break;
                case "booster_5":
                    boostersCount = 100;
                    boostersPrice = 49.99F;
                    break;
                case "booster_sale":
                    ctrProgressClass.progress ["firstPurchase"] = 1;
                    marketClass.instance.sale.SetActive (false);
                    boostersCount = 10;
                    boostersPrice = 0.99F;
                    break;
            }
                ctrProgressClass.progress["boosters"] += boostersCount;
                ctrProgressClass.saveProgress();
                marketClass.instance.boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();
                marketClass.instance.boostersLabel.GetComponent<AudioSource> ().Play ();
                marketClass.instance.boostersLabel.GetComponent<Animator> ().Play ("button");
                marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem> ().Stop();
                marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem> ().Play();

                GoogleAnalyticsV4.instance.LogTransaction(result.Purchase.OrderId, "Booster store", boostersPrice, 0.0F, 0.0F, "USD");
                GoogleAnalyticsV4.instance.LogItem (result.Purchase.OrderId, result.Purchase.SKU + ": " + boostersCount.ToString() + " boosters", result.Purchase.SKU, "Boosters", boostersPrice, 1, "USD");

                //AppsFlyer
                Dictionary<string, string> eventValue = new Dictionary<string,string> ();
                eventValue.Add("af_revenue",boostersPrice.ToString());
                eventValue.Add("af_content_type",result.Purchase.SKU.ToString());
                eventValue.Add("af_content_id",result.Purchase.OrderId.ToString());
                eventValue.Add("af_currency","USD");
                AppsFlyer.trackRichEvent("af_purchase", eventValue);

            } else {
                //AndroidMessage.Create("Product Cousume Failed", result.response.ToString() + " " + result.message);
            }
        }

        //после соединения с маркетом проверяем сделанные покупки 
        private static void OnBillingConnected(BillingResult result) {

            AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;


            if(result.IsSuccess) {
                //Store connection is Successful. Next we loading product and customer purchasing details
                AndroidInAppPurchaseManager.Client.RetrieveProducDetails();
                AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;

            } 

            //AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
        }




        private static void OnRetrieveProductsFinised(BillingResult result) {
            AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
            if(result.IsSuccess) {
                UpdateStoreData();
            } else {
                //AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
            }
        }



        private static void UpdateStoreData() {

            foreach(GoogleProductTemplate p in AndroidInAppPurchaseManager.Client.Inventory.Products) {
                if (AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased (p.SKU)) 
                    AndroidInAppPurchaseManager.Client.Consume(p.SKU);
            }

            //chisking if we already own some consuamble product but forget to consume those
            //if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(COINS_ITEM)) {
            //	consume(COINS_ITEM);
            //}
            //Check if non-consumable rpduct was purchased, but we do not have local data for it.
            //It can heppens if game was reinstalled or download on oher device
            //This is replacment for restore purchase fnunctionality on IOS


            //if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(COINS_BOOST)) {
            //	GameDataExample.EnableCoinsBoost();
            //}


        }




        */

    public void setRewardForPurchase(string itemId)
    {

        var attr = new Dictionary<string, string> {{"category", "shop"}, { "name", itemId }, { "revenue", "100" } };
#if UNITY_IOS
        itemId = itemId.Substring(22);
#else
        itemId = itemId.Substring(27);
#endif
        int animBuyBooster = -1;
        switch (itemId)
        {
            case "booster_green_1":
                ctrProgressClass.progress["boostersGreen"] += 1;
                attr["revenue"] = "199";
                animBuyBooster = 1;
                break;
            case "booster_blue_1":
                ctrProgressClass.progress["boostersBlue"] += 1;
                attr["revenue"] = "499";
                animBuyBooster = 2;
                break;
            case "booster_purple_1":
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "999";
                animBuyBooster = 3;
                break;
            case "booster_green_10":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "1499";
                animBuyBooster = 5;
                break;
            case "booster_blue_10":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "2999";
                animBuyBooster = 6;
                break;
            case "booster_purple_10":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "5999";
                animBuyBooster = 7;
                break;
            case "sale_1_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "499";
                animBuyBooster = 8;
                break;
            case "sale_2_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "299";
                animBuyBooster = 8;
                break;
            case "sale_3_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "199";
                animBuyBooster = 8;
                break;
            case "sale_4_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "99";
                animBuyBooster = 8;
                break;
            case "sale_1_green_payers":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "999";
                animBuyBooster = 5;
                break;
            case "sale_1_blue_payers":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "1999";
                animBuyBooster = 6;
                break;
            case "sale_1_purple_payers":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "3999";
                animBuyBooster = 7;
                break;
            case "sale_2_green_payers":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "749";
                animBuyBooster = 5;
                break;
            case "sale_2_blue_payers":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "1499";
                animBuyBooster = 6;
                break;
            case "sale_2_purple_payers":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "2999";
                animBuyBooster = 7;
                break;
            case "sale_3_green_payers":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "449";
                animBuyBooster = 5;
                break;
            case "sale_3_blue_payers":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "899";
                animBuyBooster = 6;
                break;
            case "sale_3_purple_payers":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "1799";
                animBuyBooster = 7;
                break;
            case "chapter":
                foreach (var block in staticClass.levelBlocks)
                {
                    if (ctrProgressClass.progress["lastLevel"] < block.Key)
                    {
                        ctrProgressClass.progress["lastLevel"] = block.Key;
                        break;
                    }
                }
                initLevelMenuClass.instance.unlockСhapterMenu.SetActive(false);
                attr["revenue"] = "99";
                break;
            //case "booster_sale":
            //ctrProgressClass.progress["firstPurchase"] = 1;
            //marketClass.instance.sale.SetActive(false);

            //break;
            case "energy_for_day":
                GameObject.Find("energy").GetComponent<lsEnergyClass>().buyEnergyReward();

                GameObject.Find("energy menu/panel with anim/energy").GetComponent<lsEnergyClass>().buyEnergyReward();
                attr["revenue"] = "99";
                break;
        }
        ctrProgressClass.progress["sale"] = 0;
        ctrProgressClass.progress["saleDate"] = 0;
        lsSaleClass.setTimerSale();
        ctrProgressClass.progress["firstPurchase"] = 1;
        ctrProgressClass.progress["paymentCount"] ++;
        ctrProgressClass.saveProgress();
        instance.boostersLabel[0].text = ctrProgressClass.progress["boostersWhite"].ToString();
        instance.boostersLabel[1].text = ctrProgressClass.progress["boostersGreen"].ToString();
        instance.boostersLabel[2].text = ctrProgressClass.progress["boostersBlue"].ToString();
        instance.boostersLabel[3].text = ctrProgressClass.progress["boostersPurple"].ToString();
        //marketClass.instance.boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();
        //marketClass.instance.boostersLabel.GetComponent<AudioSource>().Play();
        //marketClass.instance.boostersLabel.GetComponent<Animator>().Play("button");
        //marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        //marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        ctrAnalyticsClass.sendEvent("Revenue", attr, long.Parse( attr["revenue"]));

        ctrAnalyticsClass.sendCustomDimension(2, ctrAnalyticsClass.getGroup(ctrProgressClass.progress["paymentCount"], ctrAnalyticsClass.paymentGroups)); //paymentCount
        ctrAnalyticsClass.sendCustomDimension(3, ctrAnalyticsClass.getGroup(ctrProgressClass.progress["revenue"], ctrAnalyticsClass.revenueGroups)); //revenue


        //anim
        if (animBuyBooster != -1)
        {
            if (marketClass.instance.gameObject.activeSelf)
                StartCoroutine(buyBoosterAnimEnd(false));
            marketClass.instance.iconBoosterAnim.transform.GetChild(animBuyBooster).gameObject.SetActive(true);


            marketClass.instance.iconBoosterAnim.GetComponent<Animator>().Play("booster buy");
            if (marketClass.instance.gameObject.activeSelf)
                StartCoroutine(buyBoosterAnimEnd(true));
        }
    

    }





    public IEnumerator buyBoosterAnimEnd(bool flag)
{
    if (flag) yield return StartCoroutine(staticClass.waitForRealTime(0.5F));
    for (int i = 0; i < 8; i++)
    {
        //off all
        marketClass.instance.iconBoosterAnim.transform.GetChild(i).gameObject.SetActive(false);
    }

    yield return null;
}
}
