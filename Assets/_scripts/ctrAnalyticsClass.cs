using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


public class ctrAnalyticsClass  {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void sendEvent(string nameEvent, Dictionary<string, string> attributes)
    {
        //LocalyticsUnity.Localytics.TagEvent(nameEvent, attributes);
        string str = "";
        str += nameEvent + "\n";
        foreach (var attr in attributes)
        {
            str += attr.Key + ": " + attr.Value + "\n";
        }
        Debug.Log(str);
    }
}
