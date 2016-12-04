using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;
//using UnityEngine.Cloud.Analytics;

public class ctrGoogleAnalyticsClass : MonoBehaviour {
	public GoogleAnalyticsV4 googleAnalytics;
	void Start () {
			if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.LogScreen("start");
			if (GoogleAnalyticsV4.instance != null) GoogleAnalyticsV4.instance.enableAdId = true;
		}
	
	
	//
}
