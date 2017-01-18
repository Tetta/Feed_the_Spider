using System;
using System.Collections;
using System.Collections.Generic;
using com.playGenesis.VkUnityPlugin;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class SendNotify : MonoBehaviour
{
    public Text Text;
    public void Update()
    {
        Text.text =string.Format("ads ready: {0}",  Advertisement.IsReady());
    }

    public void Send()
    {
        LocalNotification.SendNotification(1, TimeSpan.FromSeconds(5), "Title", "message");
    }

    public void ShowAd()
    {
        Advertisement.Show();
    }
    

}
