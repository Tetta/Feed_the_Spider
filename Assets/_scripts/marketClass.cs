using UnityEngine;
using System.Collections;
//using UnionAssets.FLE;
using System.Collections.Generic;
using CompleteProject;

public class marketClass : MonoBehaviour {




    public static marketClass instance = null;
    public UILabel boostersLabel;
    public GameObject openBoosterMenu;
    public GameObject boosterMenu;
	public Transform cardsAll;
	//public GameObject camera;

	public GameObject sale;
	
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

        
        //boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();

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
        switch (itemId)
        {
            case "booster_green_1":
                ctrProgressClass.progress["boostersGreen"] += 1;
                attr["revenue"] = "139";
                break;
            case "booster_blue_1":
                ctrProgressClass.progress["boostersBlue"] += 1;
                attr["revenue"] = "349";
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
        }
        ctrProgressClass.progress["firstPurchase"] = 1;
        ctrProgressClass.progress["paymentCount"] ++;
        ctrProgressClass.saveProgress();
        //marketClass.instance.boostersLabel.text = ctrProgressClass.progress["boosters"].ToString();
        //marketClass.instance.boostersLabel.GetComponent<AudioSource>().Play();
        //marketClass.instance.boostersLabel.GetComponent<Animator>().Play("button");
        //marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        //marketClass.instance.boostersLabel.transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        ctrAnalyticsClass.sendEvent("Revenue", attr, long.Parse( attr["revenue"]));

        ctrAnalyticsClass.sendCustomDimension(2, ctrAnalyticsClass.getGroup(ctrProgressClass.progress["paymentCount"], ctrAnalyticsClass.paymentGroups)); //paymentCount
        ctrAnalyticsClass.sendCustomDimension(3, ctrAnalyticsClass.getGroup(ctrProgressClass.progress["revenue"], ctrAnalyticsClass.revenueGroups)); //revenue
    }

}
