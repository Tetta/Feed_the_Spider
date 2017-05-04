using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrPreviewBoosterClass : MonoBehaviour {
    public static ctrPreviewBoosterClass instance = null;
    // Use this for initialization
    void Start () {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    public void enablePreview (GameObject icon, string itemName, string price) {
        
	}
}
