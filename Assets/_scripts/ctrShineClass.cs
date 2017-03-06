using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrShineClass : MonoBehaviour {

    void OnEnable()
    {
        transform.GetChild(0).GetComponent<Animator>().CrossFadeInFixedTime("shine card", 0, -1, 0);
        transform.GetChild(1).GetComponent<Animator>().CrossFadeInFixedTime("shine card", 0, -1, 0.3F);
        transform.GetChild(2).GetComponent<Animator>().CrossFadeInFixedTime("shine card", 0, -1, 0.6F);
        transform.GetChild(3).GetComponent<Animator>().CrossFadeInFixedTime("shine card", 0, -1, 1F);

    }


    
}

