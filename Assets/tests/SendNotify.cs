using System;
using System.Collections;
using System.Collections.Generic;
using com.playGenesis.VkUnityPlugin;
using UnityEngine;

public class SendNotify : MonoBehaviour
{

    public void Send()
    {
        LocalNotification.SendNotification(1, TimeSpan.FromSeconds(5), "Title", "message");
    }

    public void Login()
    {
        VkApi.VkApiInstance.Login();
    }

    public void PutVariable()
    {
        vkExtensions.PutVariable(1, 113, b => { Debug.Log("Putted"); });
    }
    public void GetVariable()
    {
        vkExtensions.GetVariable(1, value => { Debug.Log("Getted" + value); });
    }

}
