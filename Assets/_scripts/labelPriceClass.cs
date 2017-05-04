using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class labelPriceClass : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnEnable()
    {
        //Debug.Log("-------------------------");
        //Debug.Log(name.Substring(0, name.Length - 12));
        //Debug.Log(staticClass.prices[name.Substring(0, name.Length - 12)]);
        if (staticClass.prices[name.Substring(0, name.Length - 12)] != "")
            GetComponent<UILabel>().text = staticClass.prices[name.Substring(0, name.Length - 12)];
        //if staticClass.prices.ContainsKey()[price.Key] = product.metadata.localizedPriceString;
    }

}
