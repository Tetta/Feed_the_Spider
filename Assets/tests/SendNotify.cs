using System;
using System.Collections;
using System.Collections.Generic;
using com.playGenesis.VkUnityPlugin;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class SendNotify : MonoBehaviour
{
    void Start()
    {
        Advertisement.Initialize("1271127");
    }

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
    public void FbLogin()
    {
        FB.Init(() =>
        {
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, a => { });
        });
           
    }
    
}
