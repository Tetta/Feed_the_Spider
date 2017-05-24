using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrPreviewBoosterClass : MonoBehaviour
{
    public static ctrPreviewBoosterClass instance = null;
    public Transform iconParent;
    public UILabel nameLabel;
    //public Dictionary<string, UILabel> labels = new Dictionary<string, UILabel>();
    private Dictionary<string, Dictionary<string, int>> labelValues = new Dictionary<string, Dictionary<string, int>>
    {
        { "booster_white_1", new Dictionary<string, int> { { "cards", 5}, { "keys", 1}, { "coins", 50 }, { "berries", 0 }, { "hats", 0 }, { "skins", 0 } }},
        { "booster_green_1", new Dictionary<string, int> { { "cards", 5}, { "keys", 3}, { "coins", 100 }, { "berries", 1 }, { "hats", 0 }, { "skins", 0 } }},
        { "booster_blue_1", new Dictionary<string, int> { { "cards", 5}, { "keys", 7}, { "coins", 150 }, { "berries", 1 }, { "hats", 1 }, { "skins", 0 } }},
        { "booster_purple_1", new Dictionary<string, int> { { "cards", 5}, { "keys", 20}, { "coins", 200 }, { "berries", 1 }, { "hats", 1 }, { "skins", 1 } }},
        { "booster_white_10", new Dictionary<string, int> { { "cards", 50}, { "keys", 10}, { "coins", 500 }, { "berries", 0 }, { "hats", 0 }, { "skins", 0 } }},
        { "booster_green_10", new Dictionary<string, int> { { "cards", 50}, { "keys", 30}, { "coins", 1000 }, { "berries", 10 }, { "hats", 0 }, { "skins", 0 } }},
        { "booster_blue_10", new Dictionary<string, int> { { "cards", 50}, { "keys", 70}, { "coins", 1500 }, { "berries", 10 }, { "hats", 10 }, { "skins", 0 } }},
        { "booster_purple_10", new Dictionary<string, int> { { "cards", 50}, { "keys", 200}, { "coins", 2000 }, { "berries", 10 }, { "hats", 10 }, { "skins", 10 } }},
        { "sale_free", new Dictionary<string, int> { { "cards", 25}, { "keys", 36}, { "coins", 650 }, { "berries", 5 }, { "hats", 2 }, { "skins", 1 } }},
   };

    public UILabel cardsLabel;
    public UILabel keysLabel;
    public UILabel coinsLabel;
    public UILabel berriesLabel;
    public UILabel hatsLabel;
    public UILabel skinsLabel;
    public UILabel priceLabel;
    public UI2DSprite backSprite;
    public GameObject buttonBuy;


    // Use this for initialization
    void Start () {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    public void enablePreview (GameObject icon, string itemName, string price) {

        //iconParent.DestroyChildren();
        Destroy(iconParent.GetChild(0).gameObject);
        //копируем иконку
        var iconGO = Instantiate(icon, iconParent);
        //var iconGO = Instantiate(icon, transform);
        iconGO.transform.localPosition = new Vector3(0, 0, 0);
        if(itemName.Substring(itemName.Length-1) == "1")
            iconGO.transform.localScale = new Vector2(1.1F, 1.1F);
        else
            iconGO.transform.localScale = new Vector2(1.3F, 1.3F);

        //default
        berriesLabel.transform.parent.gameObject.SetActive(true);
        hatsLabel.transform.parent.gameObject.SetActive(true);
        skinsLabel.transform.parent.gameObject.SetActive(true);
        backSprite.height = 640;
        berriesLabel.transform.parent.parent.parent.gameObject.SetActive(true);

        //title
        if (itemName.Substring(itemName.Length - 6) == "payers") nameLabel.text = Localization.Get(itemName.Substring(0, 7) + "payers");
        else nameLabel.text = Localization.Get(itemName);

        //labels values
        string itemNameForLabels = itemName;
        if (itemName == "sale_1_green_payers" || itemName == "sale_2_green_payers" || itemName == "sale_3_green_payers") itemNameForLabels = "booster_green_10";
        if (itemName == "sale_1_blue_payers" || itemName == "sale_2_blue_payers" || itemName == "sale_3_blue_payers") itemNameForLabels = "booster_blue_10";
        if (itemName == "sale_1_purple_payers" || itemName == "sale_2_purple_payers" || itemName == "sale_3_purple_payers") itemNameForLabels = "booster_purple_10";
        //additional icon for sale payers
        if (itemName != itemNameForLabels)
        {
            var c = GameObject.Find("/market/inventory/market menu/items/" + itemNameForLabels + "/icon booster");
            if (c != null)
            {
                var r = Instantiate(c, iconGO.transform);
                r.transform.localPosition = new Vector3(0, 0, 0);
                r.transform.localScale = new Vector2(1, 1);
            }
        }

        if (itemName == "sale_1_free" || itemName == "sale_2_free" || itemName == "sale_3_free" || itemName == "sale_4_free") itemNameForLabels = "sale_free";

        cardsLabel.text = labelValues[itemNameForLabels]["cards"].ToString();
        keysLabel.text = labelValues[itemNameForLabels]["keys"].ToString();
        coinsLabel.text = labelValues[itemNameForLabels]["coins"].ToString();
        berriesLabel.text = labelValues[itemNameForLabels]["berries"].ToString();
        if (labelValues[itemNameForLabels]["berries"] == 0) berriesLabel.transform.parent.gameObject.SetActive(false);
        hatsLabel.text = labelValues[itemNameForLabels]["hats"].ToString();
        if (labelValues[itemNameForLabels]["hats"] == 0) hatsLabel.transform.parent.gameObject.SetActive(false);
        skinsLabel.text = labelValues[itemNameForLabels]["skins"].ToString();
        if (labelValues[itemNameForLabels]["skins"] == 0) skinsLabel.transform.parent.gameObject.SetActive(false);

        
        //price
        priceLabel.text = price;
        //need price for coins
        //...

        //height back
        if (itemName == "booster_white_1" || itemName == "booster_white_10")
        {
            backSprite.height = 450;
            berriesLabel.transform.parent.parent.parent.gameObject.SetActive(false);
            //icon coins
            buttonBuy.transform.GetChild(2).gameObject.SetActive(true);

        } else
            buttonBuy.transform.GetChild(2).gameObject.SetActive(false);


        //for update grid
        berriesLabel.transform.parent.parent.GetComponent<UIGrid>().enabled = true;

        buttonBuy.name = itemName;
        gameObject.SetActive(true);

    }

}
