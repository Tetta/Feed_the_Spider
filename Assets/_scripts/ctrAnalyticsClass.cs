using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Facebook.Unity;
using UnityEngine;

public class ctrAnalyticsClass: MonoBehaviour
{
    public static ctrAnalyticsClass instance = null;
    private int sessionTimeout = 60 * 5;


    public static List<string> developerIds = new List<string>
    { "15779554", "303171231", "4929221", "786955", "305568333", "51066050", "212234350", "100009826471037","100007730714188", "100004274864226", "790297741122714", "558610410993", "572497357200", "582889450510" };

    public static List<float> ageGroups = new List<float> { 0, 10, 18, 24, 35, 56 };
    public static List<float> paymentGroups = new List<float> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30 };
    public static List<float> revenueGroups = new List<float> { 0, 0.6F, 1, 1.5F, 2, 3, 5, 10, 20, 30 };
    //public static List<int> levelGroups = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,  };
    public static List<float> sessionGroups = new List<float> { 0, 1, 2, 3, 4, 5, 10, 25, 50, 100, 200  };
    public static List<float> friendGroups = new List<float> { 0, 1, 5, 10, 25, 50, 100 };

    // Use this for initialization
    void Start()
    {
        
        //if (!Debug.isDebugBuild) Debug.logger.logEnabled = false;
        Debug.Log("ctrAnalyticsClass start");
        LocalNotification.CancelAllNotifications();
        try
        {
            Debug.Log("Localytics SessionTimeoutInterval: " + sessionTimeout);
            LocalyticsUnity.Localytics.SessionTimeoutInterval = sessionTimeout;
        }
        catch (Exception)
        {
            Debug.Log("Localytics SessionTimeoutInterval error");
            //throw;
        }

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        startSession();
    }


    public static void sendEvent(string nameEvent, Dictionary<string, string> attributes2, long purchase = 0)
    {
        //LocalyticsUnity.Localytics.TagEvent(nameEvent, attributes);
        string str = "";
        str += nameEvent + "\n";

        Dictionary<string, string> attributes = new Dictionary<string, string> (attributes2);
        attributes.Add("level", ctrProgressClass.progress["lastLevel"].ToString());
        var s = ctrProgressClass.progress["sessionCount"];
        if (s == 0) s = 1;
        attributes.Add("session number", s.ToString());
        attributes.Add("social id", ctrFbKiiClass.userId);
        attributes.Add("keys count", ctrProgressClass.progress["gems"].ToString());
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

    public void OnApplicationFocus(bool flag)
    {
        Debug.Log("OnPause: " + flag);
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();
        if (!flag)
        {

            //pause
            ctrProgressClass.progress["sessionEnd"] = (int) DateTime.Now.TotalSeconds();
            ctrProgressClass.saveProgress();
            sendNotifers();


        }
        else
        {
            //PlayerPrefs.DeleteKey("progress");
            //ctrProgressClass.getProgress();
            //Debug.Log("awake");

            startSession();
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        ctrProgressClass.progress["sessionEnd"] = (int)DateTime.Now.TotalSeconds();
        ctrProgressClass.saveProgress();
        sendNotifers();
    }

    public void startSession()
    {
        if (ctrProgressClass.progress.Count == 0) ctrProgressClass.getProgress();

        Debug.Log("session end: " + ctrProgressClass.progress["sessionEnd"]);
        Debug.Log("session end 2: " + (int)DateTime.Now.AddSeconds(-sessionTimeout).TotalSeconds());
        if (ctrProgressClass.progress["sessionEnd"] < (int) DateTime.Now.AddSeconds(-sessionTimeout).TotalSeconds())
        {
            lsEnergyClass.checkEnergy(true);
            if (ctrProgressClass.progress["sessionStart"] > 1)
            {
                Debug.Log("--------------------------------------------------------------------");
                sendEvent("Session End", new Dictionary<string, string>
                {
                    {
                        "session length",
                        (Mathf.Round((ctrProgressClass.progress["sessionEnd"] -
                                      ctrProgressClass.progress["sessionStart"])/60F*100)/100F).ToString()
                    },
                    {"level play count", ctrProgressClass.progress["levelPlayCount"].ToString()},
                    {"win count", ctrProgressClass.progress["winCount"].ToString()},
                    {"energy count", lsEnergyClass.energy.ToString()}
                });
            }
            Debug.Log("startSession");

            ctrProgressClass.progress["sessionCount"]++;
            sendCustomDimension(5, getGroup(ctrProgressClass.progress["sessionCount"], ctrAnalyticsClass.sessionGroups)); //sessionCount

            ctrProgressClass.progress["sessionStart"] = (int)DateTime.Now.TotalSeconds();
            ctrProgressClass.progress["sessionEnd"] = (int)DateTime.Now.TotalSeconds();
            ctrProgressClass.progress["levelPlayCount"] = 0;
            ctrProgressClass.progress["winCount"] = 0;

            sendEvent("Session Start",
                new Dictionary<string, string>
                {
                    {"coins", ctrProgressClass.progress["coins"].ToString()},
                    {"energy", lsEnergyClass.energy.ToString()}
                });

            Debug.Log("analytics session count: " + ctrProgressClass.progress["sessionCount"]);
            Debug.Log("analytics groups count: " + ctrAnalyticsClass.sessionGroups.Count);
            Debug.Log("analytics session group: " + getGroup(ctrProgressClass.progress["sessionCount"], ctrAnalyticsClass.sessionGroups));
            
            if (ctrProgressClass.progress["firstLaunch"] == 0)
            {
                ctrProgressClass.progress["dailyBonus"] = (int)DateTime.UtcNow.TotalSeconds();
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

    public void sendNotifers()
    {
        //sale notifer
        lsSaleClass.setTimerSale();
        lsSaleClass.setSale();
        LocalNotification.CancelAllNotifications();
        var delay = new TimeSpan();
        if (lsSaleClass.timerStartSale > DateTime.Now)
        {
            delay = lsSaleClass.timerStartSale - DateTime.Now;
            var h = DateTime.Now.Add(delay).Hour;
            if (h < 10) delay.Add(new TimeSpan(10 - h, 0, 0));
            Debug.Log("notifer 1 delay: " + delay);
            var type = ctrProgressClass.progress["firstPurchase"] == 1 ? "Payers" : "Free";
            LocalNotification.SendNotification(1, delay, "", Localization.Get("notiferTitleSale") + Localization.Get("sale" + ctrProgressClass.progress["sale"] + type));
        }
        //daily notifer
        delay = DateTime.Parse("12:00:00") - DateTime.Now;
        if (delay < new TimeSpan(0)) delay = DateTime.Parse("12:00:00").AddDays(1) - DateTime.Now;
        Debug.Log("daily notifer: " + delay);
        LocalNotification.SendNotification(2, delay, "", Localization.Get("notiferTitleDay"));

        //daily 3 notifer
        delay = DateTime.Parse("12:00:00").AddDays(2) - DateTime.Now;
        if (delay < new TimeSpan(0)) delay = DateTime.Parse("12:00:00").AddDays(3) - DateTime.Now;
        Debug.Log("daily notifer 3: " + delay);
        LocalNotification.SendNotification(4, delay, "", Localization.Get("notiferTitleDay3"));

        //daily 7 notifer
        delay = DateTime.Parse("12:00:00").AddDays(6) - DateTime.Now;
        if (delay < new TimeSpan(0)) delay = DateTime.Parse("12:00:00").AddDays(7) - DateTime.Now;
        Debug.Log("daily notifer 7: " + delay);
        LocalNotification.SendNotification(5, delay, "", Localization.Get("notiferTitleDay7"));

        //daily 14 notifer
        delay = DateTime.Parse("12:00:00").AddDays(13) - DateTime.Now;
        if (delay < new TimeSpan(0)) delay = DateTime.Parse("12:00:00").AddDays(14) - DateTime.Now;
        Debug.Log("daily notifer 14: " + delay);
        LocalNotification.SendNotification(6, delay, "", Localization.Get("notiferTitleDay14"));

        //energy notifer
        lsEnergyClass.checkEnergy(true);
        if (lsEnergyClass.energy < lsEnergyClass.maxEnergy && !lsEnergyClass.energyInfinity)
        {
            //var start = new DateTime(2015, 1, 1).AddSeconds(ctrProgressClass.progress["energyTime"]);
            //var end = start.AddSeconds((lsEnergyClass.maxEnergy - lsEnergyClass.energy) * lsEnergyClass.costEnergy);
            //delay = end - DateTime.UtcNow;
            delay = new TimeSpan(0,0,
                ctrProgressClass.progress["energyTime"] + lsEnergyClass.maxEnergy*lsEnergyClass.costEnergy -
                    (int) DateTime.Now.TotalSeconds());
            Debug.Log("notifer energy delay: " + delay);
            LocalNotification.SendNotification(3, delay,"", Localization.Get("notiferTitleEnergy"));

        }



    }

}
