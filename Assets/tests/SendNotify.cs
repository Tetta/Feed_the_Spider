using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendNotify : MonoBehaviour {

    public void Send()
    {
        LocalNotification.SendNotification(1,TimeSpan.FromSeconds(0), "Title","message","traaack");
    }
}
