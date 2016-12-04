using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ctrAppsFlyer : MonoBehaviour {
	//public static ctrAppsFlyer instance = null;

	// Use this for initialization
	void Start () {
		//if (instance != null) { 
		#if UNITY_IOS  && !UNITY_EDITOR

			AppsFlyer.setAppsFlyerKey ("YOUR_DEV_KEY");
			AppsFlyer.setAppID ("YOUR_APP_ID");
			AppsFlyer.setIsDebug (true);
			AppsFlyer.getConversionData ();
			AppsFlyer.trackAppLaunch ();

		#elif UNITY_ANDROID && !UNITY_EDITOR

			// if you are working without the manifest, you can initialize the SDK programatically.
			//AppsFlyer.set ("Ura5UVbFB3YXvaig2PnvPA");
			AppsFlyer.init ("Ura5UVbFB3YXvaig2PnvPA");
			//AppsFlyer.setIsDebug (true);

			// un-comment this in case you are not working with the android manifest file
			AppsFlyer.setAppID ("gof.feedthespider"); 

			// for getting the conversion data
			//AppsFlyer.loadConversionData ("AppsFlyerTrackerCallbacks", "didReceiveConversionData", "didReceiveConversionDataWithError");

			//AppsFlyer.validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency, Dictionary additionalParametes);

			// for in app billing validation
			//AppsFlyer.createValidateInAppListener ("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure"); 

			#endif
			//Destroy(gameObject);
			//return;
		//}
		//instance = this;
		//DontDestroyOnLoad (gameObject);


	}

	/*
	//A custom event tracking
	public void Purchase(){
		Dictionary<string, string> eventValue = new Dictionary<string,string> ();
		eventValue.Add("af_revenue","1");
		eventValue.Add("af_content_type","category_a");
		eventValue.Add("af_content_id","1234567");
		eventValue.Add("af_currency","USD");
		AppsFlyer.trackRichEvent("af_purchase", eventValue);
		//AF_Sample_BGScript.pressed ();
	}
	*/
}
