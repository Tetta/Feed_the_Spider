using System;
using UnityEngine;
using System.Collections;
//using UnionAssets.FLE;
using System.Collections.Generic;
using CompleteProject;
using Odnoklassniki.HTTP;

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
		//Debug.Log("Start market");
		if (GetComponent<Purchaser>() != null) GetComponent<Purchaser>().Start();
        //дальше, если market
		if (ctrProgressClass.progress.Count == 0) {
            ctrProgressClass.getProgress();
        }

		//если бустеры
		if (name != "market")
			return;
        
		if (instance != null) {
			Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        staticClass.setBoostersLabels();


        gameObject.SetActive (false);
    }

    void OnPress(bool isPressed) {
        if (!isPressed) {
            //запрос на покупку
            Debug.Log("click buy " + name);
            //preview
            var price = "";
            if (name.Substring(0, 4) == "sale")
                price = transform.GetChild(0).GetComponent<UILabel>().text;
            else
                price = transform.GetChild(2).GetChild(1).GetComponent<UILabel>().text;

            ctrPreviewBoosterClass.instance.enablePreview(transform.FindChild("icon booster").gameObject, name, price);

            GetComponent<Animator>().Play("button");
            GetComponent<AudioSource>().Play();
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

    public void setRewardForPurchase(string itemId, string transactionId, string currency, decimal price)
    {
        Debug.Log("setRewardForPurchase");
        var attr = new Dictionary<string, string> {{"category", "shop"}, { "name", itemId }, { "revenue", "100" } };
#if UNITY_IOS
        itemId = itemId.Substring(22);
#else
        itemId = itemId.Substring(27);
#endif
        int animBuyBooster = -1;
        attr["transactionId"] = transactionId;
        attr["name"] = itemId;
        string nameForOpenBooster = "";

        switch (itemId)
        {
            case "booster_green_1":
                ctrProgressClass.progress["boostersGreen"] += 1;
                attr["revenue"] = "199";
                nameForOpenBooster = "booster_green_1";
                break;
            case "booster_blue_1":
                ctrProgressClass.progress["boostersBlue"] += 1;
                attr["revenue"] = "499";
                nameForOpenBooster = "booster_blue_1";
                break;
            case "booster_purple_1":
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "999";
                nameForOpenBooster = "booster_purple_1";
                break;
            case "booster_green_10":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "1499";
                nameForOpenBooster = "booster_green_10";
                break;
            case "booster_blue_10":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "2999";
                nameForOpenBooster = "booster_blue_10";
                break;
            case "booster_purple_10":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "5999";
                nameForOpenBooster = "booster_purple_10";
                break;
            case "sale_1_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "499";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_sale";
                break;
            case "sale_2_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "299";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_sale";
                break;
            case "sale_3_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "199";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_sale";
                break;
            case "sale_4_free":
                ctrProgressClass.progress["boostersGreen"] += 3;
                ctrProgressClass.progress["boostersBlue"] += 1;
                ctrProgressClass.progress["boostersPurple"] += 1;
                attr["revenue"] = "99";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_sale";
                break;
            case "sale_1_green_payers":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "999";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_green_10";
                break;
            case "sale_1_blue_payers":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "1999";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_blue_10";
                break;
            case "sale_1_purple_payers":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "3999";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_purple_10";
                break;
            case "sale_2_green_payers":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "749";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_green_10";
                break;
            case "sale_2_blue_payers":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "1499";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_blue_10";
                break;
            case "sale_2_purple_payers":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "2999";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_purple_10";
                break;
            case "sale_3_green_payers":
                ctrProgressClass.progress["boostersGreen"] += 10;
                attr["revenue"] = "449";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_green_10";
                break;
            case "sale_3_blue_payers":
                ctrProgressClass.progress["boostersBlue"] += 10;
                attr["revenue"] = "899";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_blue_10";
                break;
            case "sale_3_purple_payers":
                ctrProgressClass.progress["boostersPurple"] += 10;
                attr["revenue"] = "1799";
                attr["category"] = "sales";
                nameForOpenBooster = "booster_purple_10";
                break;
            case "chapter":
                foreach (var block in staticClass.levelBlocks)
                {
                    if (ctrProgressClass.progress["lastLevel"] < block.Key)
                    {
                        Debug.Log(block.Key);
                        Debug.Log(block.Value);
                        ctrProgressClass.progress["lastLevel"] = block.Key;
                        if (GameObject.Find("/root/root/blocks/block " + block.Value) != null) GameObject.Find("/root/root/blocks/block " + block.Value).SetActive(false);
                        if (GameObject.Find("level " + block.Key) != null) GameObject.Find("level " + block.Key).GetComponent<lsLevelClass>().block = null;
                        if (GameObject.Find("level " + block.Key) != null) GameObject.Find("level " + block.Key).GetComponent<lsLevelClass>().Start();
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
                if (GameObject.Find("energy") != null) GameObject.Find("energy").GetComponent<lsEnergyClass>().buyEnergyReward();

                if (GameObject.Find("energy menu/panel with anim/energy") != null) GameObject.Find("energy menu/panel with anim/energy").GetComponent<lsEnergyClass>().buyEnergyReward();
                attr["revenue"] = "99";
                break;
        }


        ctrProgressClass.progress["sale"] = 0;
        ctrProgressClass.progress["saleDate"] = 0;
        lsSaleClass.setTimerSale();
        ctrProgressClass.progress["firstPurchase"] = 1;
        ctrProgressClass.progress["paymentCount"] ++;
        ctrProgressClass.saveProgress();
        //staticClass.setBoostersLabels();



        //off sales if sale
        Debug.Log("category: " + attr["category"]);
        if (attr["category"] == "sales")
        {
            GameObject.Find("button sale").GetComponent<lsSaleClass>().OnEnable();
            if (instance.isActiveAndEnabled) GameObject.Find("market menu/sale/button sale").GetComponent<lsSaleClass>().OnEnable();
            //if on map
            if (GameObject.Find("sale menu") != null) GameObject.Find("sale menu").SetActive(false);
            marketClass.instance.gameObject.SetActive(true);
            staticClass.isTimePlay = Time.timeScale;
            Time.timeScale = 0;
            marketClass.instance.transform.GetChild(0).GetComponent<UIToggle>().Set(true);

            Debug.Log("Time.timeScale: " + Time.timeScale);
        }




        Debug.Log("revenue old: " + attr["revenue"]);
        long revenue = Mathf.FloorToInt(int.Parse(attr["revenue"]) * 0.7F);
        attr["revenue"] = Mathf.FloorToInt(revenue/100) + "." + (revenue%100);
        //attr["revenue"] = int
        Debug.Log("revenue int: " + revenue);
        Debug.Log("revenue str: " + attr["revenue"]);

        //string revenueForOk = Mathf.FloorToInt(revenue / 100) + "." + (revenue % 100);
        string revenueForOk = price.ToString().Replace(",", ".");
        //send OK sdk.reportPayment

        if (ctrFbKiiClass.instance.source != "0" && ctrProgressClass.progress["ok"] == 1 && Odnoklassniki.OK.IsLoggedIn)
        {
            Debug.Log("send sdk.reportPayment");
            Dictionary<string, string> args = new Dictionary<string, string>();
            args["trx_id"] = attr["transactionId"];
            if (args["trx_id"].Length > 128)  args["trx_id"] = args["trx_id"].Substring(0, 128);
            //args["amount"] = revenueForOk;
            args["amount"] = revenueForOk;
            //args["currency"] = "USD";
            args["currency"] = currency;
            Odnoklassniki.OK.API(Odnoklassniki.OKMethod.SDK.reportPayment, Method.GET, args, response =>
            {
                Debug.Log("response sdk.reportPayment");
                Debug.Log(response.Text);
            });
        }

        ctrAnalyticsClass.sendCustomDimension(2, ctrAnalyticsClass.getGroup(ctrProgressClass.progress["paymentCount"], ctrAnalyticsClass.paymentGroups)); //paymentCount
        ctrAnalyticsClass.sendCustomDimension(3, ctrAnalyticsClass.getGroup(ctrProgressClass.progress["revenue"], ctrAnalyticsClass.revenueGroups)); //revenue

        ctrAnalyticsClass.sendEvent("Revenue", attr, revenue);

        //open booster
        if (nameForOpenBooster != "")
        {
            mBoosterClass.instance.itemName = nameForOpenBooster;
            mBoosterClass.instance.transform.parent.parent.gameObject.SetActive(true);
        }

        //anim
        /*
        Debug.Log("animBuyBooster: " + animBuyBooster);
        
        if (animBuyBooster != -1)
        {
            if (marketClass.instance.gameObject.activeSelf)
                StartCoroutine(buyBoosterAnimEnd(false));
            marketClass.instance.iconBoosterAnim.transform.GetChild(animBuyBooster).gameObject.SetActive(true);


            marketClass.instance.iconBoosterAnim.GetComponent<Animator>().Play("booster buy");
            if (marketClass.instance.gameObject.activeSelf)
                StartCoroutine(buyBoosterAnimEnd(true));
        }
    
        */
    }





    public IEnumerator buyBoosterAnimEnd(bool flag)
{
    if (flag) yield return StartCoroutine(staticClass.waitForRealTime(0.4F));
    for (int i = 0; i < 9; i++)
    {
        //off all
        marketClass.instance.iconBoosterAnim.transform.GetChild(i).gameObject.SetActive(false);
    }

    yield return null;
}


    public void buyBoosters(string name)
    {
        ctrPreviewBoosterClass.instance.gameObject.SetActive(false);
        if (name == "booster_white_1" || name == "booster_white_10")
        {
            int amount = 0;
            int cost = 0;
            if (name == "booster_white_1")
            {
                amount = 1;
                cost = 400;
            }
            else
            {
                amount = 10;
                cost = 3200;
            }
            var nameItem = name;
            ctrAnalyticsClass.sendEvent("Coins",
                new Dictionary<string, string> {{"detail 1", nameItem}, {"coins", (-cost).ToString()}});

            ctrProgressClass.progress["coins"] -= cost;
            ctrProgressClass.progress["boostersWhite"] += amount;
            ctrProgressClass.saveProgress();

            staticClass.setBoostersLabels();

            //change label coins
            GameObject.Find("/market/inventory/market menu/bars/coins/label coins").GetComponent<UILabel>().text =
                ctrProgressClass.progress["coins"].ToString();
            if (initLevelMenuClass.instance != null)
                initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress["coins"].ToString();


            mBoosterClass.instance.itemName = name;
            mBoosterClass.instance.transform.parent.parent.gameObject.SetActive(true);
        }
        else
        {
            
#if UNITY_IOS
            instance.GetComponent<Purchaser>().BuyProductID("com.mysterytag.spider." + name);
#else
            instance.GetComponent<Purchaser>().BuyProductID("com.evogames.feedthespider." + name);
#endif


        }

    }

}
