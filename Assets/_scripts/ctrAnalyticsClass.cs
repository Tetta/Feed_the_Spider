using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;


public class ctrAnalyticsClass
{

    // Use this for initialization
    void Start()
    {
        //LocalyticsUnity.Localytics.AppKey. = "fc631941d160eeb3ae14250-16d5b2ca-e200-11e6-8aad-008cc99655f0";

    }

    // Update is called once per frame
    void Update()
    {

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
        try
        {
            LocalyticsUnity.Localytics.TagEvent(nameEvent, attributes);
        }
        catch (Exception)
        {
            Debug.Log("Localytics TagEvent error");
            //throw;
        }
        
    }
}
