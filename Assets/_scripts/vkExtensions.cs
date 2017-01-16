using System;
using System.Collections;
using System.Collections.Generic;
using com.playGenesis.VkUnityPlugin;
using com.playGenesis.VkUnityPlugin.MiniJSON;
using UnityEngine;

public class vkExtensions
{

    /// <summary>
    /// https://vk.com/pages?oid=-17680044&p=storage.get
    /// </summary>
    /// <param name="number"></param>
    public static void GetScore(int userId, Action<int> response)
    {
        if (VkApi.CurrentToken == null)
            return;

        var r2 = new VKRequest()
        {
            url = "apps.getScore?user_id=" + userId,
            CallBackFunction = request =>
            {
                Debug.Log(request.response);

                var dict = Json.Deserialize(request.response) as Dictionary<string, object>;

                if (string.IsNullOrEmpty(dict["response"].ToString()))
                    response(-1);
                else 
                    response(int.Parse(dict["response"].ToString()));
            }
        };
        VkApi.VkApiInstance.Call(r2);
    }

    /// <summary>
    /// https://vk.com/pages?oid=-1&p=storage.set
    /// </summary>
    /// <param name="number"></param>
    public static void PutVariable(int number, int value, Action<bool> onCompleteSuccess)
    {
        if (VkApi.CurrentToken == null)
            return;

        var r2 = new VKRequest()
        {
            url = "storage.set?key=" + number + "&value=" + value,
            CallBackFunction = request =>
            {
                onCompleteSuccess(request.response=="1");
            }
        };
        VkApi.VkApiInstance.Call(r2);
    }
}
