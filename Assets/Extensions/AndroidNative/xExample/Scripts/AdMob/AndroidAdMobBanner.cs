////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using UnityEngine.SceneManagement;
#endif



//Attach the script to the empty gameobject on your sceneS
public class AndroidAdMobBanner : MonoBehaviour {


	public string BannersUnityId;
	public GADBannerSize size = GADBannerSize.SMART_BANNER;
	public TextAnchor anchor = TextAnchor.LowerCenter;



	private static Dictionary<string, GoogleMobileAdBanner> _refisterdBanners = null;


	// --------------------------------------
	// Unity Events
	// --------------------------------------
	
	void Awake() {
		if (name == "bannerScript") {
			if (AndroidAdMobController.instance.IsInited) {
				if (!AndroidAdMobController.instance.BannersUunitId.Equals (BannersUnityId)) {
					AndroidAdMobController.instance.SetBannersUnitID (BannersUnityId);
				} 
			} else {
				AndroidAdMobController.instance.Init (BannersUnityId);
			}
		}
	}

	void Start() {
		if (name == "bannerScript") ShowBanner();
	}

	void OnDestroy() {
		if (name == "bannerScript") HideBanner();
	}


	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	public void ShowBanner() {
		GoogleMobileAdBanner banner;
		if(registerdBanners.ContainsKey(sceneBannerId)) {
			banner = registerdBanners[sceneBannerId];
			transform.GetChild (0).gameObject.SetActive (true);
		}  else {
			banner = AndroidAdMobController.instance.CreateAdBanner(anchor, size);
			registerdBanners.Add(sceneBannerId, banner);
		}

		if(banner.IsLoaded && !banner.IsOnScreen) {
			banner.Show();
			transform.GetChild (0).gameObject.SetActive (true);
		}
	}

	public void HideBanner() {
		Debug.Log ("banner.Hide1");
		if(registerdBanners.ContainsKey(sceneBannerId)) {
			GoogleMobileAdBanner banner = registerdBanners[sceneBannerId];
			if(banner.IsLoaded) {
				if(banner.IsOnScreen) {
					banner.Hide();
					Debug.Log ("banner.Hide222");
				}
			} else {
				banner.ShowOnLoad = false;
				Debug.Log ("banner.Hide3");
			}
		}
	}

	// --------------------------------------
	// GET / SET
	// --------------------------------------


	public static Dictionary<string, GoogleMobileAdBanner> registerdBanners {
		get {
			if(_refisterdBanners == null) {
				_refisterdBanners = new Dictionary<string, GoogleMobileAdBanner>();
			}

			return _refisterdBanners;
		}
	}

	public string sceneBannerId {
		get {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			return Application.loadedLevelName + "_" + this.gameObject.name;
			#else
			return SceneManager.GetActiveScene().name + "_" + this.gameObject.name;
			#endif
		}
	}

	public void OnPress(bool flag) {
		if (!flag) if (transform.parent.childCount >= 10) transform.parent.GetChild(9).GetChild(0).GetComponent<AndroidAdMobBanner>().HideBanner();

	}
}
