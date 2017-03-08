using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrBuyBoosterAnimClass : MonoBehaviour {

    void OnEnable()
    {
        for (int i = 0; i < 8; i++)
        {
            //off all
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }


    
}

