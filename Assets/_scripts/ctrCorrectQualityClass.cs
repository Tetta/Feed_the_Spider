using System.Collections;
using UnityEngine;

public class ctrCorrectQualityClass : MonoBehaviour {
	
	private static bool qualitySeted = false;
    
    void Awake() {
        if (!qualitySeted) {
            qualitySeted = true;
            SetCorrectQuality ();
        }
    }


    void SetCorrectQuality() {
        int width = Screen.height>Screen.width?Screen.height:Screen.width;
		QualitySettings.SetQualityLevel (1, true);
#if UNITY_IOS
        if (width > 1024) width = 1024;
#endif

        //#if UNITY_ANDROID
        if (width > 1024) {
			//full res
			QualitySettings.SetQualityLevel (2, true);
		} else if (width > 512) {
			//half res
			QualitySettings.SetQualityLevel (1, true);
						
		} else {   
			//quarter res
			QualitySettings.SetQualityLevel (0, true);
		}

	} 

//#elif UNITY_IPHONE
     //слабые айфоны
        //QualitySettings.SetQualityLevel(iPhone.generation == iPhoneGeneration.iPhone3G || iPhone.generation == iPhoneGeneration.iPhone3GS ? 2: 0);
//#else
        //QualitySettings.SetQualityLevel (0);
//#endif
    
}
