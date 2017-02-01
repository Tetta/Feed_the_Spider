using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Facebook.Unity;
using UnityEngine;


public class ctrAnalyticsClass: MonoBehaviour
{
    public static ctrAnalyticsClass instance = null;
    private int sessionTimeout = 3;


    public static List<string> developerIds = new List<string>
    { "15779554", "303171231", "4929221", "786955", "305568333", "51066050", "212234350", "100009826471037","100007730714188", "100004274864226", "790297741122714" };

    public static List<float> ageGroups = new List<float> { 0, 10, 18, 24, 35, 56 };
    public static List<float> paymentGroups = new List<float> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30 };
    public static List<float> revenueGroups = new List<float> { 0, 0.6F, 1, 1.5F, 2, 3, 5, 10, 20, 30 };
    //public static List<int> levelGroups = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,  };
    public static List<float> sessionGroups = new List<float> { 1, 2, 3, 4, 5, 10, 25, 50, 100, 200  };
    public static List<float> friendGroups = new List<float> { 0, 1, 5, 10, 25, 50, 100 };

    // Use this for initialization
    void Start()
    {
        Debug.Log("ctrAnalyticsClass start");

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        startSession();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void sendEvent(string nameEvent, Dictionary<string, string> attributes, long purchase = 0)
    {
        //LocalyticsUnity.Localytics.TagEvent(nameEvent, attributes);
        string str = "";
        str += nameEvent + "\n";
        attributes.Add("level", ctrProgressClass.progress["lastLevel"].ToString());
        attributes.Add("session number", ctrProgressClass.progress["sessionCount"].ToString());
        attributes.Add("social id", ctrFbKiiClass.userId);
        attributes.Add("gems count", ctrProgressClass.progress["gems"].ToString());
        foreach (var attr in attributes)
        {
            str += attr.Key + ": " + attr.Value + "\n";
        }
        Debug.Log(str);
        try
        {
            LocalyticsUnity.Localytics.TagEvent(nameEvent, attributes, purchase);
        }
        catch (Exception)
        {
            Debug.Log("Localytics TagEvent error");
            //throw;
        }
        
    }
    public static void sendProfileAttribute(string key, string value)
    {
        Debug.Log("sendProfileAttribute: " + key + " = " + value);
        try
        {
            LocalyticsUnity.Localytics.SetProfileAttribute(key, value);
        }
        catch (Exception)
        {
            Debug.Log("Localytics SetProfileAttribute error");
        }
    }
    public static void sendCustomDimension(int key, string value)
    {
        Debug.Log("sendCustomDimension: " + key + " = " + value);
        try
        {
            LocalyticsUnity.Localytics.SetCustomDimension(key, value);
        }
        catch (Exception)
        {
            Debug.Log("Localytics sendCustomDimension error");
        }
    }

    public void OnApplicationPause(bool flag)
    {
        Debug.Log("OnPause: " + flag);
        if (flag)
        {
            //pause
            ctrProgressClass.progress["sessionEnd"] = (int) DateTime.Now.TotalSeconds();
            ctrProgressClass.saveProgress();
        }
        else
        {
            startSession();
        }
    }
/*
    void OnDestroy()
    {
        Debug.Log("Analytics OnDestroy");
        ctrProgressClass.progress["sessionEnd"] = (int)DateTime.Now.TotalSeconds();
        ctrProgressClass.saveProgress();
    }
*/
    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        ctrProgressClass.progress["sessionEnd"] = (int)DateTime.Now.TotalSeconds();
        ctrProgressClass.saveProgress();
    }

    public void startSession()
    {
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        if (ctrProgressClass.progress["sessionEnd"] < (int) DateTime.Now.AddSeconds(-sessionTimeout).TotalSeconds())
        {
            if (ctrProgressClass.progress["sessionStart"] != 0)
            {
                sendEvent("Session End", new Dictionary<string, string>
                {
                    {
                        "session length",
                        (Mathf.Round((ctrProgressClass.progress["sessionEnd"] -
                                      ctrProgressClass.progress["sessionStart"])/60F*100)/100F).ToString()
                    },
                    {"level play count", ctrProgressClass.progress["levelPlayCount"].ToString()},
                    {"win count", ctrProgressClass.progress["winCount"].ToString()}
                });
            }
            Debug.Log("startSession");
            lsEnergyClass.checkEnergy(true);
            sendEvent("Session Start",
                new Dictionary<string, string>
                {
                    {"coins", ctrProgressClass.progress["coins"].ToString()},
                    {"energy", ctrProgressClass.progress["energy"].ToString()}
                });
            ctrProgressClass.progress["sessionStart"] = (int) DateTime.Now.TotalSeconds();
            ctrProgressClass.progress["sessionEnd"] = (int) DateTime.Now.TotalSeconds();
            ctrProgressClass.progress["sessionCount"]++;
            ctrProgressClass.progress["levelPlayCount"] = 0;
            ctrProgressClass.progress["winCount"] = 0;
            sendCustomDimension(5, getGroup(ctrProgressClass.progress["sessionCount"], ctrAnalyticsClass.sessionGroups)); //sessionCount

            if (ctrProgressClass.progress["firstLaunch"] == 0)
            {
                sendEvent("First Launch", new Dictionary<string, string>());
                ctrProgressClass.progress["firstLaunch"] = 1;
            }
        
            ctrProgressClass.saveProgress();
        } 
    }

    public static void sendAnalyticsAfterSocialLogin()
    {
        
    }

    public static string getGroup (float value, List<float> list  )
    {
        for (int i = 1; i < list.Count; i++)
        {
            if (value < list[i])
            {
                if (value == list[i - 1]) return list[i - 1].ToString();
                else return list[i - 1] + " - " + list[i];
            }
        }
        return list[list.Count - 1] + "+";
    }

    //setCustomerFirstName
    public static void setCustomerFirstName(string name)
    {
        Debug.Log("setCustomerFirstName: " + name);
        try
        {
            LocalyticsUnity.Localytics.SetCustomerFirstName(name);
        }
        catch (Exception)
        {
            Debug.Log("Localytics setCustomerFirstName error");
        }
    }
    //setCustomerLastName
    public static void setCustomerLastName(string name)
    {
        Debug.Log("setCustomerLastName: " + name);
        try
        {
            LocalyticsUnity.Localytics.SetCustomerLastName(name);
        }
        catch (Exception)
        {
            Debug.Log("Localytics setCustomerLastName error");
        }
    }
    //setCustomerFullName
    public static void setCustomerFullName(string name)
    {
        Debug.Log("setCustomerFullName: " + name);
        try
        {
            LocalyticsUnity.Localytics.SetCustomerFullName(name);
        }
        catch (Exception)
        {
            Debug.Log("Localytics setCustomerFullName error");
        }
    }
    //setCustomerEmail
    public static void setCustomerEmail(string mail)
    {
        Debug.Log("setCustomerEmail: " + mail);
        try
        {
            LocalyticsUnity.Localytics.SetCustomerEmail(mail);
        }
        catch (Exception)
        {
            Debug.Log("Localytics setCustomerEmail error");
        }
    }

}
