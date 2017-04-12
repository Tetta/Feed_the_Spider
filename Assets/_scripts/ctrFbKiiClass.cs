using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;
using System;
using System.Linq;
using System.Security.Policy;
//using Boo.Lang;
using com.playGenesis.VkUnityPlugin;
using UnityEngine.SceneManagement;
using Facebook.MiniJSON;
using JsonOrg;
using Odnoklassniki;
using Odnoklassniki.HTTP;


public class ctrFbKiiClass : MonoBehaviour {
	public GameObject photoFriendOnMap;
    public GameObject vkFriend;


    public static ctrFbKiiClass instance = null;

	//список друзей Fb: userFbId = first_name
	public static Dictionary<string, string> friendsFirstname = new Dictionary<string, string>();

	public static Dictionary<string, Texture2D> friendsImage = new Dictionary<string, Texture2D>();
    //public static Dictionary<string, Texture2D> friendsImageForVKInvite = new Dictionary<string, Texture2D>();
    public static Texture2D userImage;

	public static string[] friendsIds = null;
    //public static string[] friendsIdsForInvite = null;
    public static string userId = "";
    public static bool isLoginKii = false;

	//список друзей Kii: userFbId -> levelNumber -> score
	public static KiiQueryResult<KiiObject> friendsScore = null;

	//список: "Кто из друзей на каком уровне?" level1 = userFbId
	public static Dictionary<string, string> friendsLastLevel = new Dictionary<string, string>();



    public Downloader d;
    public FriendManager friendManagerVK;
    //public List<VKUser> friends = new List<VKUser>();
    //public List<VKUser> friendsForVKInvite = new List<VKUser>();
    public List<FriendForInvite> friendsForInvite = new List<FriendForInvite>();

    public class FriendForInvite
    {
        public string id;
        public string name;
        //public Texture2D texture;


    }

    public string source = "0";

    // set instance
    void Start() {

        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        ctrProgressClass.getProgress();
        
        Debug.Log("socials - Start Kii Class");
        Debug.Log("lang: " + ctrProgressClass.progress["language"]);

        initializeSDK();


    }




    public static void updateUserScore(string levelName, int score, int lastLevel) {
		if (isLoginKii && userId != "") {
			KiiBucket bucket = Kii.Bucket("scores");

		    var name = userId;
            if (ctrProgressClass.progress["vk"] == 1) name = "vk" + userId;
            if (ctrProgressClass.progress["ok"] == 1) name = "ok" + userId;

            KiiObject kiiObj = bucket.NewKiiObject(name);
			kiiObj[levelName] = score;
			kiiObj["lastLevel"] = "level" + lastLevel.ToString();
			kiiObj.Save((KiiObject savedObj, Exception e) => {
				if (e != null) {
				}
				else {
				}
			});				
		}
    }





    public static void setFriendsLastLevel() {
		//Debug.Log ("setFriendsLastLevel");
		foreach (KiiObject obj in friendsScore) {
			try {
				friendsLastLevel[obj.GetString("lastLevel")] = obj.GetString("userFbId");
				if (SceneManager.GetActiveScene().name == "level menu") 
					setFriendImgMap(obj.GetString("lastLevel"), GameObject.Find("level " + obj.GetString("lastLevel").Substring(5)).transform.GetChild(0));

			} catch {
				//Debug.Log ("setFriendsLastLevel error");
			}
		}


	}

	public static void setFriendImgMap(string nameLevel, Transform levelIslandGem) {
		if (friendsLastLevel != null) {
			if (friendsLastLevel.ContainsKey(nameLevel)) {
				string id = friendsLastLevel[nameLevel];

				Debug.Log ("setFriendImgMap id: " + id);
                Debug.Log("friendsImage.ContainsKey (id): " + friendsImage.ContainsKey(id));

                if (!friendsImage.ContainsKey (id)) {
					// We don't have this players image yet, request it now
					LoadFriendImgFromID (id, pictureTexture => {
						if (pictureTexture != null) {
							friendsImage.Add (id, pictureTexture);

							GameObject newPhoto = Instantiate (ctrFbKiiClass.instance.photoFriendOnMap, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
							newPhoto.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (pictureTexture, new Rect (0, 0, pictureTexture.width, pictureTexture.height), new Vector2 (0.5F, 0.5F), 1);
                            
                            newPhoto.transform.parent = levelIslandGem;
							newPhoto.transform.localPosition = new Vector3 (-165, 0, levelIslandGem.localPosition.z);
							newPhoto.transform.localScale = new Vector3 (1, 1, 1);
                            if (ctrProgressClass.progress["ok"] == 1) newPhoto.transform.GetChild(0).localScale = new Vector3(0.8F, 0.8F, 1);
                        }
					});
				} else {
				
					GameObject newPhoto = Instantiate (ctrFbKiiClass.instance.photoFriendOnMap, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
					newPhoto.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (friendsImage[id], new Rect (0, 0, friendsImage[id].width, friendsImage[id].height), new Vector2 (0.5F, 0.5F), 1);
                    
                    newPhoto.transform.parent = levelIslandGem;
					newPhoto.transform.localPosition = new Vector3 (-165, 0, levelIslandGem.localPosition.z);
					newPhoto.transform.localScale = new Vector3 (1, 1, 1);
                    if (ctrProgressClass.progress["ok"] == 1) newPhoto.transform.GetChild(0).localScale = new Vector3(0.8F, 0.8F, 1);

                }


            }
		}
	}

	public static void setBestGamers(Transform levelMenu, int levelNumber) {
        Debug.Log("socials - setBestGamers");
        Transform socialMenu = levelMenu.GetChild(2);
        Transform bestGamersMenu;
        if (ctrProgressClass.progress["fb"] == 1 && FB.IsLoggedIn)
	    {
	        socialMenu = levelMenu.GetChild(2);
	        bestGamersMenu = socialMenu.GetChild(1);

	    }
	    else if (ctrProgressClass.progress["vk"] == 1 && VkApi.VkApiInstance.IsUserLoggedIn)
	    {
	        socialMenu = levelMenu.GetChild(3);
            bestGamersMenu = socialMenu.GetChild(1);

        }
        else if (ctrProgressClass.progress["ok"] == 1 && OK.IsLoggedIn)
	    {
	        //doing
	        socialMenu = levelMenu.GetChild(3);
            bestGamersMenu = socialMenu.GetChild(3);

        }
        else return;
	    Debug.Log(socialMenu.name);

        //if (ctrProgressClass.progress ["fb"] == 1 && FB.IsLoggedIn) {

        //выключаем панель fb login, включаем панель с лучшими игроками, выключаем каждого игрока (всего 3)
        socialMenu.GetChild (0).gameObject.SetActive (false);
        bestGamersMenu.gameObject.SetActive (true);
        bestGamersMenu.GetChild (0).gameObject.SetActive (false);
        bestGamersMenu.GetChild (1).gameObject.SetActive (false);
        bestGamersMenu.GetChild (2).gameObject.SetActive (false);

			Dictionary<string, int> scores = new Dictionary<string, int>();
			if (friendsScore != null)
				if (friendsScore.Count > 0)
			foreach (KiiObject friend in friendsScore) {
				try {
						scores[friend.GetString("userFbId")] = friend.GetInt("level" + levelNumber);
				} catch {
					//Debug.Log ("setBestGamers error");
				}

			}
			scores[userId] = ctrProgressClass.progress ["score" + levelNumber + "_1"] + ctrProgressClass.progress ["score" + levelNumber + "_2"];
			//Debug.Log (scores[userFbId]);
			scores = scores.OrderByDescending (x => x.Value).ToDictionary (x => x.Key, x => x.Value);

			//Dictionary<string, int>  sortedScores = from entry in scores orderby entry.Value ascending select entry;
			int i = 0;
			foreach (var score in scores) {
				if (i < 3) {
                bestGamersMenu.GetChild (i).gameObject.SetActive (true);

					//photo

					//если юзер
					if (userId == score.Key) {
						if (userImage != null) bestGamersMenu.GetChild (i).GetChild(0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (userImage, new Rect(0, 0, userImage.width, userImage.height), new Vector2(0.5F, 0.5F), 1);
                    bestGamersMenu.GetChild (i).GetChild (1).GetComponent<UILabel> ().text = score.Value.ToString ();
						i++;

					}
					if (friendsImage.ContainsKey(score.Key)) {
                    bestGamersMenu.GetChild (i).GetChild(0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (friendsImage[score.Key], new Rect(0, 0, friendsImage[score.Key].width, friendsImage[score.Key].height), new Vector2(0.5F, 0.5F), 1);
                    bestGamersMenu.GetChild (i).GetChild (1).GetComponent<UILabel> ().text = score.Value.ToString ();
						i++;

					
					}



				} else
					break;
			}
        //}	
        GameObject.Find("/root/static/level menu/reward for login").SetActive(false);
    }

	public void invite(string social) {
        Debug.Log("socials - invite: " + social);

	    if (social == "fb")
	    {
	        if (FB.IsInitialized)
	        {
	            ctrFbRequest.RequestChallenge();
	            //GetInvitableFriends();
	        }
	    }
	    else if (social == "vk")
	    {
	        ctrFbKiiClass.instance.setFriendsForInvite(GameObject.Find("/root/static/level menu/vk_ok/invite friends/"));
	    }
	    else if (social == "ok")
	    {
	        ctrFbKiiClass.instance.setFriendsForInvite(GameObject.Find("/root/static/level menu/vk_ok/invite friends ok/"));

	    }
        //for test
        //ctrFbKiiClass.instance.OnInviteFriendFB(new List<string> { "5", "6"});

    }



    #region Friends Img
    // Graph API call to fetch friend picture from user ID returned from FBGraph.GetScores()
    //
    // Note: /me/invitable_friends returns invite tokens instead of user ID's,
    // which will NOT work with this /{user-id}/picture Graph API call.
    private static void LoadFriendImgFromID (string userID, Action<Texture2D> callback)
	{
		FB.API(GetPictureQuery(userID, 100, 100),
			HttpMethod.GET,
			delegate (IGraphResult result)
			{
				if (result.Error != null)
				{
					//Debug.LogError(result.Error + ": for friend "+userID);
					return;
				}
				if (result.Texture ==  null)
				{
					//Debug.Log("LoadFriendImg: No Texture returned");
					return;
				}
				callback(result.Texture);
			});
	}

	// Generate Graph API query for a user/friend's profile picture
	public static string GetPictureQuery(string facebookID, int? width = null, int? height = null, string type = null, bool onlyURL = false)
	{
		string query = string.Format("/{0}/picture", facebookID);
		string param = width != null ? "&width=" + width.ToString() : "";
		param += height != null ? "&height=" + height.ToString() : "";
		param += type != null ? "&type=" + type : "";
		if (onlyURL) param += "&redirect=false";
		if (param != "") query += ("?g" + param);
		return query;
	}

	// Download an image using WWW from a given URL
	public static void LoadImgFromURL (string imgURL, Action<Texture2D> callback)
	{
		// Need to use a Coroutine for the WWW call, using Coroutiner convenience class
		Coroutiner.StartCoroutine(
			LoadImgEnumerator(imgURL, callback)
		);
	}

	public static IEnumerator LoadImgEnumerator (string imgURL, Action<Texture2D> callback)
	{
		WWW www = new WWW(imgURL);
		yield return www;

		if (www.error != null)
		{
			//Debug.LogError(www.error);
			yield break;
		}
		callback(www.texture);
	}

	// Pull out the picture image URL from a JSON user object constructed in FBGraph.GetPlayerInfo() or FBGraph.GetFriends()
	public static string DeserializePictureURL(object userObject)
	{
		// friendObject JSON format in this situation
		// {
		//   "first_name": "Chris",
		//   "id": "10152646005463795",
		//   "picture": {
		//      "data": {
		//          "url": "https..."
		//      }
		//   }
		// }
		var user = userObject as Dictionary<string, object>;

		object pictureObj;
		if (user.TryGetValue("picture", out pictureObj))
		{
			var pictureData = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj)["data"]);
			return (string)pictureData["url"];
		}
		return null;
	}
    /*
	// Pull out score from a JSON user entry object constructed in FBGraph.GetScores()
	public static int GetScoreFromEntry(object obj)
	{
		Dictionary<string,object> entry = (Dictionary<string,object>) obj;
		return Convert.ToInt32(entry["score"]);
	}
	*/
    #endregion




    private void DownloadImage(string url, VKUser user, bool self)
    {
        Action<DownloadRequest> doOnFinish = (d) =>
        {
            var fid = (long)d.CustomData[0];
            if (d.DownloadResult.error == null && user.id == fid)
            {
                //Debug.Log("DownloadImage, id: " + user.id.ToString());
                if (self && userImage == null) userImage = d.DownloadResult.texture;
                else if (!friendsImage.ContainsKey(user.id.ToString())) friendsImage.Add(user.id.ToString(), d.DownloadResult.texture);
            }

        };
        var r = new DownloadRequest
        {
            url = url,
            onFinished = doOnFinish,
            CustomData = new object[] { user.id }
        };
        VkApi.Downloader.download(r);
    }

    //if player click invite friends - VK
    public void setFriendsForInvite(GameObject go)
    {
        Debug.Log("socials - setFriendsForInvite: " + go.name);

        go.SetActive(true);
        foreach (var f in friendsForInvite)
        {
            GameObject friend = null;
            //if exist
            if (go.transform.GetChild(0).GetChild(0).FindChild(f.id) != null)
            {
                friend = go.transform.GetChild(0).GetChild(0).FindChild(f.id).gameObject;
                if (friendsImage.ContainsKey(f.id) && friend.transform.GetChild(0).GetComponent<UITexture>().mainTexture == null)
                    friend.transform.GetChild(0).GetComponent<UITexture>().mainTexture =
                        friendsImage[f.id];
            }
            else
            {
                //if dont exist
                friend = Instantiate(vkFriend) as GameObject;
                friend.transform.parent = go.transform.GetChild(0).GetChild(0);

                if (friendsImage.ContainsKey(f.id.ToString()))
                    friend.transform.GetChild(0).GetComponent<UITexture>().mainTexture =
                        friendsImage[f.id.ToString()];

                friend.transform.localScale = new Vector3(1, 1, 1);
                friend.transform.localPosition = new Vector3(0, 0, 0);

                friend.transform.GetChild(0).GetComponent<UIWidget>().depth = 2;
                friend.transform.GetChild(1).GetComponent<UILabel>().text = f.name;
                friend.name = f.id.ToString();

                //orange colors
                if (ctrProgressClass.progress["ok"] == 1)
                {
                    friend.transform.GetChild(1).GetComponent<UILabel>().color = new Color32(73, 32, 0, 255);
                    friend.transform.GetChild(2).GetComponent<UI2DSprite>().color = new Color32(213, 93, 1, 255);
                    friend.transform.GetChild(5).GetComponent<UILabel>().color = new Color32(73, 32, 0, 255);
                }

            }
            //if invited
            if (ctrProgressClass.progress.ContainsKey(f.id))
            {
                friend.transform.GetChild(2).GetChild(0).GetComponent<UILocalize>().key = "invited";
                //disable coins
                friend.transform.GetChild(5).gameObject.SetActive(false);
                friend.transform.GetChild(6).gameObject.SetActive(false);
                //disable collider button invite
                friend.transform.GetChild(2).GetComponent<Collider>().enabled = false;
            }


        }
        GameObject.Find("/root/static/level menu/button exit level menu").GetComponent<SpriteRenderer>().sortingOrder = 200;

    }

    public void inviteFriend(string social, string name)
    {
        Debug.Log("socials - inviteFriend: " + social + ", id: " + name);

        string title = Localization.Get("inviteTitle");
        string message = Localization.Get("inviteMessage");
        if (social == "vk")
        {
            VKRequest r2 = new VKRequest()
            {
                url = "apps.sendRequest?user_id=" + name + "&text=" + message + "&type=invite&name=" + title,
                CallBackFunction = OnInviteFriend,
                customParam2 = "name"
            };
            VkApi.VkApiInstance.Call(r2);
        }
        if (social == "ok")
        {
            long unixTimestamp = (long)DateTime.UtcNow.TotalSeconds() * 1000;

            //get notes
            OK.API(OKMethod.SDK.getNotes, response =>
            {
                Debug.Log("getNotes");
                Debug.Log(response.Text);
            });

            /*
            //reset note
            var args1 = new Dictionary<string, string>()
                    {
                      { "timestamp", Odnoklassniki.HTTP.JSON.Encode(unixTimestamp) }
                    };
            OK.API(OKMethod.SDK.resetNotes, Method.GET, args1, response =>
            {
                Debug.Log("resetNotes");
                Debug.Log(response.Text);
            });
            */

            //send note
            /*
            Hashtable note = new Hashtable()
                    {
                      {"uid", name},
                      //{"image", "http://pp.userapi.com/c637727/v637727333/36d5e/LcCzGvznxXA.jpg"},
                      {"image", "http://pp.userapi.com/c639517/v639517235/f390/lxmBHLosYT8.jpg"},
                      {"message", title + " " + message},
                      {"payload", ""},
                      {"timestamp", unixTimestamp}
                    };

            var args = new Dictionary<string, string>()
                    {
                      { "note", Odnoklassniki.HTTP.JSON.Encode(note) }
                    };
*/
            var args2 = new Dictionary<string, string>()
                    {
                      {"uids", name},

{"devices", "ANDROID,IOS"}
                     // {"text", title + " " + message}

        };

            OK.API("sdk.appInvite",  args2, response =>
            {
                Debug.Log("sendNote");
                Debug.Log(response.Text);
                if (response.Error == "" && int.Parse(response.Object["count"].ToString()) >= 1)
                    OnInviteFriend(name);
            });
            
        }
    }

    public void OnInviteFriendFB(IEnumerable<string> result)
    {
        Debug.Log("OnInviteFriendFB");
        int coins = 0;
        foreach (string id in result)
        {
            

            if (!ctrProgressClass.progress.ContainsKey(id)) coins += 10;
            
            ctrProgressClass.progress[id] = 1;

           
        }
        if (coins > 0)
        {
            initLevelMenuClass.instance.rewardMenu.SetActive(true);
            initLevelMenuClass.instance.rewardMenu.transform.GetChild(0)
                .GetChild(5)
                .GetChild(0)
                .GetChild(3)
                .GetChild(3)
                .GetComponent<UILabel>()
                .text = coins.ToString();
            ctrProgressClass.progress["coins"] += coins;
            initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress["coins"].ToString();
            ctrProgressClass.saveProgress();
        }
    }

    public virtual void OnInviteFriend<T>(T result)
    {
        string id = "";
        Transform friend = null;
        //vk 
        if (result.GetType() == typeof(VKRequest))
        {
            VKRequest r = result as VKRequest;
            if (r.error != null)
            {
                Debug.Log(r.error.error_msg);
                return;
            }
            Debug.Log(r.response);
            id = r.customParam2;
            friend = GameObject.Find("/root/static/level menu/vk_ok/invite friends/scroll/friends grid/" + id).transform;
        }
        //ok
        else
        {
            id = result.ToString();
            friend = GameObject.Find("/root/static/level menu/vk_ok/invite friends ok/scroll/friends grid/" + id).transform;
        }
        //label invited
        friend.GetChild(2).GetChild(0).GetComponent<UILabel>().text = Localization.Get("invited");
        //disable label "+10"
        friend.GetChild(5).gameObject.SetActive(false);
        //start anim coins
        friend.GetChild(6).GetChild(0).GetComponent<Animator>().enabled = true;
        //disable collider button invite
        friend.GetChild(2).GetComponent<Collider>().enabled = false;

        ctrProgressClass.progress["coins"] += 10;
        initLevelMenuClass.instance.coinsLabel.text = ctrProgressClass.progress["coins"].ToString();
        //map icon coins anim
        //initLevelMenuClass.instance.coinsLabel.transform.parent.GetChild(0).GetComponent<Animator>().enabled = true;
        //initLevelMenuClass.instance.coinsLabel.transform.parent.GetChild(0).GetComponent<Animator>().Play("menu enable");


        ctrProgressClass.progress[id] = 1;
        ctrProgressClass.saveProgress();

    }

    public void clickJoinGroup(string url, string name)
    {
        bool flag = false;
        if (name == "group button") flag = true;
        GameObject.Find("/settings folder/settings").transform.GetChild(5).gameObject.SetActive(false);
        GameObject.Find("/settings folder/settings").transform.GetChild(6).gameObject.SetActive(false);
        if (ctrProgressClass.progress["vk"] == 1)
        {
            VKRequest r = new VKRequest
            {
                url = url,
                //url = "users.get?user_ids=" + userId + "&photo_50",
                CallBackFunction = onGroupResult,
                customParam = flag
            };
            VkApi.VkApiInstance.Call(r);
        }
        if (ctrProgressClass.progress["fb"] == 1)
        {
            //FB.GameGroupJoin("1677948472504355", instance.onGroupResult);
            //FB.API("1411218055568852", instance.onGroupResult);
            var wwwForm = new WWWForm();
            wwwForm.AddField("member", userId);
            //wwwForm.AddField("access_token", Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);
            FB.API("400521690334115/likes", HttpMethod.GET, instance.onGroupResult);

            //FB.API("400521690334115/subscribed_apps", HttpMethod.GET);
            //FB.API("400521690334115/members", HttpMethod.POST, instance.onGroupResult, wwwForm);

            //FB.API("1411218055568852/members", HttpMethod.POST, instance.onGroupResult, wwwForm);
            //FB.API("1677948472504355/members?member=" + userId, HttpMethod.GET, instance.onGroupResult);
        }
        if (ctrProgressClass.progress["ok"] == 1)
        {
            //OKMedia.App()
        }
    }


    public void clickLogout()
    {
        if (ctrProgressClass.progress["vk"] == 1)
        {
            VkApi.VkApiInstance.Logout();
            VkApi.VkSetts.forceOAuth = true;
            VkApi.VkSetts.revoke = true;
            GameObject.Find("/settings folder/settings/vk").SetActive(false);
        };
        if (ctrProgressClass.progress["fb"] == 1)
        {
            FB.LogOut();
            GameObject.Find("/settings folder/settings/fb").SetActive(false);
        }
        if (ctrProgressClass.progress["ok"] == 1)
        {
            OK.Logout();
            GameObject.Find("/settings folder/settings/ok").SetActive(false);
        }

        //null static vars
        friendsFirstname = new Dictionary<string, string>();
        friendsImage = new Dictionary<string, Texture2D>();
        friendsIds = null;
        userId = "";
        isLoginKii = false;
        friendsScore = null;
        friendsLastLevel = new Dictionary<string, string>();

        userImage = null;

    friendsForInvite = new List<FriendForInvite>();


        ctrProgressClass.progress["fb"] = 0;
        ctrProgressClass.progress["vk"] = 0;
        ctrProgressClass.progress["ok"] = 0;
        ctrProgressClass.saveProgress();
        
    }

    public void onGroupResult<T>(T result)
    {
        Debug.Log("socials - onGroupResult");
        var social = "";
        var flagSendAnalytics = false;
        Debug.Log("socials - onRequestFriends: " + result.GetType());
        //fb
        if (result.GetType() == typeof(GraphResult))
        {
            social = "fb";
            GraphResult r = result as GraphResult;
            //if error
            if (r.Error != null) Debug.Log("socials - onGroupResult fb error: " + r.RawResult);
            if (r.Error != null) return;
            Debug.Log("socials - onGroupResult fb: " + r.RawResult);
            //if (GameObject.Find("/settings folder/settings/fb/group button") != null) GameObject.Find("/settings folder/settings/fb/group button").SetActive(false);

        }
        else if (result.GetType() == typeof(VKRequest))
        {
            social = "vk";
            VKRequest r = result as VKRequest;
            if (r.error != null) Debug.Log("socials - onGroupResult vk error: " + r.error.error_msg);
            if (r.error != null) return;
            GameObject groupGO = null;
            if (r.customParam)
            {
                groupGO = GameObject.Find("/settings folder/settings/vk/group button");
                //anim coins
                groupGO.transform.parent.GetChild(4).gameObject.SetActive(true);

                ctrProgressClass.progress["rewardGroupVK1"] = 1;
            }
            else {
                groupGO = GameObject.Find("/settings folder/settings/vk/group button 2");
                //anim coins
                groupGO.transform.parent.GetChild(5).gameObject.SetActive(true);

                ctrProgressClass.progress["rewardGroupVK2"] = 1;
            }
            if (groupGO != null) groupGO.SetActive(false);
            ctrProgressClass.progress["coins"] += 50;
        }
        if (GameObject.Find("/settings folder/settings/" + social + "/back menu") != null) GameObject.Find("/settings folder/settings/" + social + "/back menu").transform.localPosition += new Vector3(0, -120, 0);
        ctrProgressClass.saveProgress();
    }

    public void share()
    {
        /*mediatopic.post нужны права PUBLISH_TO_STREAM
        string media = "{\"media\": [{\"type\": \"text\",\"text\": \"my text\"},{\"type\": \"topic\",\"topicId\": \"66736579256464\",\"group\": \"true\"}]}";
        Dictionary<string, string> args1 = new Dictionary<string, string>();
        args1["attachment"] = media;
        OK.API("mediatopic.post", Method.GET, args1, response =>
        {
            Debug.Log("-----------------------");
            Debug.Log(response.Text);
        });
        */
        Debug.Log("share click");

        string media = "{\"media\": [{\"type\": \"text\",\"text\": \"Feed the Spider - интересная игра!\"},{\"type\": \"topic\",\"topicId\": \"66736745848976\",\"group\": \"true\"}]}";
        //string media = "{\"media\": [{\"type\": \"topic\",\"topicId\": \"66736745848976\",\"group\": \"true\"}]}";
        Dictionary<string, string> args1 = new Dictionary<string, string>();
        args1["attachment"] = media;
        args1["app"] = OK.AppId;

        OK.API("sdk.post", Method.GET, args1, response =>
        {
            Debug.Log(response.Text);
            if (GameObject.Find("/settings folder/settings/share menu") != null) GameObject.Find("/settings folder/settings/share menu").SetActive(false);
            var c = GameObject.Find("/settings folder/settings/ok/share button/coins");
            if (c != null)
            {
                c.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            }
            ctrProgressClass.progress["rewardRepostOK"] = 1;
            ctrProgressClass.progress["coins"] += 100;
            ctrProgressClass.saveProgress();
        });
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------------------
    //init sdk
    private void initializeSDK()
    {
        Debug.Log("socials - initializeSDK");
        //fb
        if (staticClass.socialDefault() == "fb")
        {
            if (!FB.IsInitialized)
            {
                FB.Init(() =>
                {
                    onInitializeSDK("fb");
                }); 
            }
        }
        
        else
        {
            //vk
            d = VkApi.VkApiInstance.gameObject.GetComponent<Downloader>();
            friendManagerVK = VkApi.VkApiInstance.gameObject.GetComponent<FriendManager>();
            onInitializeSDK("vk");
            
            //ok
            OK.Init(initSuccess =>
            { 
                if (initSuccess)
                    onInitializeSDK("ok");
            });
        }
    }

    //after init sdk check if login
    private void onInitializeSDK(string sdk)
    {
        Debug.Log("socials - onInitializeSDK: " + sdk);
        if (sdk == "fb")
        {
            if (FB.IsInitialized)
            {
                if (FB.IsLoggedIn && ctrProgressClass.progress["fb"] == 1)
                    onLogin("fb");
            }
        }
        else if (sdk == "vk")
        {
            if (VkApi.VkApiInstance.IsUserLoggedIn && ctrProgressClass.progress["vk"] == 1)
                onLogin("vk");
        }
        else if (sdk == "ok")
        {





            //getInstallSource
            OK.GetInstallSource(source =>
            {
                //source = "1"; //for test
                Debug.Log(".......................................................................................");
                Debug.Log(source);
                Debug.Log(OK.IsLoggedIn);
                Debug.Log(VkApi.VkApiInstance.IsUserLoggedIn);
                instance.source = source;

                if (source != "0" && !OK.IsLoggedIn && !(VkApi.VkApiInstance.IsUserLoggedIn && ctrProgressClass.progress["vk"] == 1))
                {
                    Debug.Log("OK.Auth, user from OK");
                    OK.Auth(success => onLogin("ok"));
                }
                else if (source != "0")
                {

                    //send stats launch
                    long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds * 1000;

                    ArrayList innerStats = new ArrayList()
                    {
                      new Hashtable()
                      {
                        {"id", "launch"},
                        {"time", unixTimestamp},
                        {"type", "status"},
                        {"data", new ArrayList() {
                            "1"
                          }
                        }
                      }
                    };

                    Hashtable stats = new Hashtable()
                    {
                      {"time", unixTimestamp},
                      {"version", "0.0.6"},
                      {"stats", innerStats}
                    };

                    var args = new Dictionary<string, string>()
                    {
                      { "stats", Odnoklassniki.HTTP.JSON.Encode(stats) }
                    };
                    OK.API("sdk.reportStats", args, r =>
                    {
                        Debug.Log("........................");
                        Debug.Log(r.Text);
                    });
                }
            });


            if (OK.IsLoggedIn )
                onLogin("ok");
        }
    }

    //connect on button click
    public void connect(string social)
    {
        Debug.Log("socials - connect: " + social);
        switch (social)
        {
            case "fb":
                Debug.Log("socials - connect: " + social + " step 1");
                if (FB.IsInitialized)
                {
                    Debug.Log("socials - connect: " + social + " step 2");
                    //var perms = new List<string>() {"public_profile", "email", "user_friends", "manage_pages", "publish_pages" , "publish_actions" };
                    var perms = new List<string>() { "public_profile", "email", "user_friends"};
                    FB.LogInWithReadPermissions(perms, onLoginFB);
                }
                break;
            case "vk":
                VkApi.VkSetts.forceOAuth = false;
                VkApi.VkApiInstance.LoggedIn += onLoginVK;
                VkApi.VkApiInstance.Login();
                break;
            case "ok":
                OK.Auth(success => onLogin("ok"));
                break;

        }
    }

    //after login: request user, friends, save social
    private void onLogin(string social)
    {
        Debug.Log("socials - onLogin: " + social);
        switch(social)
            {
            case "fb":
                break;
            case "vk":
                try
                {
                    VkApi.VkApiInstance.LoggedIn -= onLoginVK;
                }
                catch (System.Exception ex)
                {

                }
                userId = VkApi.CurrentToken.user_id;

                requestFriendsInvite();
                break;
            case "ok":
                    ctrAdClass.instance.loadAdMyTarget();
                //Console.WriteLine("Default case");

                break;

        }


        requestUser(social);
        requestFriendsApp(social);
        //delete other socials
        ctrProgressClass.progress["fb"] = 0;
        ctrProgressClass.progress["vk"] = 0;
        ctrProgressClass.progress["ok"] = 0;
        ctrProgressClass.progress[social] = 1;
        
        //reward for login
        if (ctrProgressClass.progress["rewardLogin"] == 0)
        {
            var rewardGO = GameObject.Find("/root/static/level menu/reward for login");
            if (rewardGO != null)
            {
                rewardGO.transform.GetChild(0).GetComponent<Animator>().enabled = false;
                rewardGO.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>().enabled = true;
                rewardGO.transform.GetChild(1).gameObject.SetActive(false);

            }
            ctrProgressClass.progress["boostersWhite"] ++;
            ctrProgressClass.progress["rewardLogin"] = 1;



        }


        ctrProgressClass.saveProgress();

        changeUIPanelFriends(social);
    }
    
    //change ui panel friends
    public void changeUIPanelFriends(string social)
    {
        if (SceneManager.GetActiveScene().name == "level menu")
        {
            Debug.Log("socials - changeUIPanelFriends");
            if (social == "vk" || social == "ok") social = "vk_ok";
            GameObject.Find("/root/static/level menu/" + social).transform.GetChild(0).gameObject.SetActive(false);
            if (ctrProgressClass.progress["vk"] == 1)
                GameObject.Find("/root/static/level menu/" + social).transform.GetChild(1).gameObject.SetActive(true);
            if (ctrProgressClass.progress["ok"] == 1)
                GameObject.Find("/root/static/level menu/" + social).transform.GetChild(3).gameObject.SetActive(true);

        }

    }
    //action for fb
    private void onLoginFB(ILoginResult result)
    {
        onLogin("fb");
    }
    
    //action for vk
    private void onLoginVK()
    {
        onLogin("vk");
    }

    private void requestUser(string social)
    {
        Debug.Log("socials - requestUser: " + social);
        switch (social)
        {
            case "fb":
                string queryString = "/me?fields=id,first_name,age_range,gender,picture.width(100).height(100)";
                FB.API(queryString, HttpMethod.GET, onRequestUser);
                break;
            case "vk":
                VKRequest r = new VKRequest
                {
                    url = "users.get?user_ids=" + userId + "&fields=photo_100,sex,bdate",
                    CallBackFunction = onRequestUser
                };
                VkApi.VkApiInstance.Call(r);
                break;
            case "ok":
                string[] fields =  { OKUserInfo.Field.first_name, OKUserInfo.Field.last_name, OKUserInfo.Field.age, OKUserInfo.Field.gender, OKUserInfo.Field.email, OKUserInfo.Field.uid, OKUserInfo.Field.pic128x128};
                OK.GetCurrentUser(userInfo =>
                {
                    onRequestUser(userInfo);
                }, fields);
                break;

        }

    }

    private void requestFriendsApp(string social)
    {
        Debug.Log("socials - requestFriendsApp: " + social);
        switch (social)
        {
            case "fb":
                string queryString = "/me/friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
                FB.API(queryString, HttpMethod.GET, onRequestFriends);
                break;
            case "vk":
                var request = new VKRequest()
                {
                    url = "apps.getFriendsList?user_id=" + userId + "&type=request&extended=1&count=100&fields=photo_100",
                    CallBackFunction = onRequestFriends,
                    customParam = true
                };
                VkApi.VkApiInstance.Call(request);
                break;
            case "ok":


                OK.API(OKMethod.Friends.get, Method.GET, response =>
                {
                    ArrayList uidList = response.Array;
                    OK.GetAppUsers(appUsers =>
                    {
                        KeyValuePair<bool, string[]> kv1 = new KeyValuePair<bool, string[]>(true, appUsers);
                        //app friends
                        onRequestFriends(kv1);

                        foreach (string appUser in appUsers)
                        {
                            uidList.Remove(appUser);
                        }
                        string[] uids = new string[uidList.Count];
                        for (int i = 0; i < uidList.Count; i++)
                        {
                            uids[i] = uidList[i].ToString();
                        }

                        KeyValuePair<bool, string[]> kv2 = new KeyValuePair<bool, string[]>(false, uids);
                        //for invite friends
                        onRequestFriends(kv2);
                    });
                });



               
                break;

        }
    }

    //не установившие приложение
    private void requestFriendsInvite()
    {
        var request2 = new VKRequest()
        {
            url = "apps.getFriendsList?user_id=" + userId + "&type=invite&extended=1&count=100&fields=photo_100",
            CallBackFunction = onRequestFriends,
            customParam = false
        };
        VkApi.VkApiInstance.Call(request2);
    }

    private void onRequestUser <T>(T result)
    {
        Debug.Log("socials - onRequestUser: " + result.GetType());
        //fb
        if (result.GetType() == typeof(GraphResult))
        {
            GraphResult r =  result as GraphResult;
            //if error
            if (r.Error != null) return;

            //age
            Dictionary<string, object> ageRange;
            var age = 0F;
            if (r.ResultDictionary.TryGetValue("age_range", out ageRange))
            {
                var d = ageRange["min"];
                age = float.Parse(ageRange["min"].ToString());
            }

            //gender
            var gender = "";
            r.ResultDictionary.TryGetValue("gender", out gender);

            //id
            r.ResultDictionary.TryGetValue("id", out userId);

            //Fetch player profile picture from the URL returned
            string playerImgUrl = DeserializePictureURL(r.ResultDictionary);

            //сделать для всех
            LoadImgFromURL(playerImgUrl, delegate (Texture2D pictureTexture)
            {
                // Setup the User's profile picture
                if (pictureTexture != null)
                {
                    userImage = pictureTexture;
                }

            });

            //for analytics
            string fname;
            r.ResultDictionary.TryGetValue("first_name", out fname);
            string lname;
            r.ResultDictionary.TryGetValue("last_name", out lname);
            string email;
            r.ResultDictionary.TryGetValue("email", out email);

            sendSocialAnalytics(age, gender, "fb", fname, lname, email);
        }


        //vk
        if (result.GetType() == typeof(VKRequest))
        {
            VKRequest r = result as VKRequest;
            if (r.error != null) return;

            //now we need to deserialize response in json from vk server
            var dict = Json.Deserialize(r.response) as Dictionary<string, object>;
            var users = (System.Collections.Generic.List<object>)dict["response"];
            var user = VKUser.Deserialize(users[0]);

            //убрать
            if (!string.IsNullOrEmpty(user.photo_100))
            {
                DownloadImage(user.photo_100, user, true);
            }

            //for analytics
            int age = 0;

            try
            {
                if (!string.IsNullOrEmpty(user.bdate))
                    age = Mathf.RoundToInt(((float) (((DateTime.Now - DateTime.Parse(user.bdate)).TotalDays))/365));
            }
            catch (Exception)
            {
                
            }
            var gender = "";
            if (user.sex == 1)
                gender = "female";
            else if (user.sex == 2)
                gender = "male";

            sendSocialAnalytics(age, gender, "vk", user.first_name, user.last_name);

        }

        //ok
        if (result.GetType() == typeof(OKUserInfo))
        {
            OKUserInfo r = result as OKUserInfo;
            if (r.pic128x128 != "") //сделать для всех
                LoadImgFromURL(r.pic128x128, delegate (Texture2D pictureTexture)
                {
                    // Setup the User's profile picture
                    if (pictureTexture != null)
                    {
                        userImage = pictureTexture;
                    }

                });
            //for analytics
            userId = r.uid;
            sendSocialAnalytics(r.age, r.gender, "ok", r.firstName, r.lastName);
        }

    }

    private void onRequestFriends<T>(T result)
    {
        var social = "";
        var flagSendAnalytics = false;
        Debug.Log("socials - onRequestFriends: " + result.GetType());
        //fb
        if (result.GetType() == typeof(GraphResult))
        {
            social = "fb";
            GraphResult r = result as GraphResult;
            //if error
            if (r.Error != null) return;

            object dataList;
            if (r.ResultDictionary.TryGetValue("data", out dataList))
            {
                List<object> newFriends = (List<object>) dataList;
                int i = 0;
                friendsIds = new string[newFriends.Count];
                foreach (Dictionary<string, object> newFriend in newFriends)
                {

                    string idFriend = newFriend["id"] as string;
                    string first_name = newFriend["first_name"] as string;

                    friendsFirstname[idFriend] = first_name;

                    friendsIds.SetValue(idFriend, i);

                    i++;
                }

            }
            flagSendAnalytics = true;
        }


        //vk
        if (result.GetType() == typeof(VKRequest))
        {
            social = "vk";
            VKRequest r = result as VKRequest;
            if (r.error != null) Debug.Log("socials - onRequestFriends vk: error");
            if (r.error != null) return;

            var dict = Json.Deserialize(r.response) as Dictionary<string, object>;

            var resp = (Dictionary<string, object>)dict["response"];
            var items = (List<object>)resp["items"];
            if (r.customParam)
                friendsIds = new string[items.Count];
            //else
            //friendsIdsForInvite = new string[items.Count];
            Debug.Log("socials - onRequestFriends vk friends count: " + items.Count);
            var i = 0;
            if (r.customParam) flagSendAnalytics = true;

            
            foreach (var item in items)
            {
                var friend = VKUser.Deserialize(item);
                var f = new FriendForInvite();
                f.name = friend.last_name + " " + friend.first_name;
                f.id = friend.id.ToString();

                if (!r.customParam)
                    //friends.Add(friend);
                    //else

                    friendsForInvite.Add(f);
                if (r.customParam)
                    Debug.Log("f.id: " + f.id);

                    if (!string.IsNullOrEmpty(friend.photo_100))
                {
                    DownloadImage(friend.photo_100, friend, false);
                }

                if (r.customParam)
                    friendsIds.SetValue(friend.id.ToString(), i);
                //else
                    //friendsIdsForInvite.SetValue(friend.id.ToString(), i);

                friendsFirstname[friend.id.ToString()] = friend.first_name;
                i++;

            }

        }

        //ok
        if (result.GetType() == typeof(KeyValuePair<bool, string[]>))
        {
            social = "ok";
            KeyValuePair<bool, string[]>?  r = result as KeyValuePair<bool, string[]>?;
            string[] fields = {OKUserInfo.Field.pic128x128, OKUserInfo.Field.first_name, OKUserInfo.Field.uid, OKUserInfo.Field.last_name };
            if (r.Value.Key)
                friendsIds = new string[r.Value.Value.Length];
            //else
            //friendsIdsForInvite = new string[r.Value.Value.Length];
            Debug.Log("socials - onRequestFriends ok app friends: " + r.Value.Key);

            OK.GetInfo(users =>
            {
                var i = 0;
                foreach (var friend in users)
                {
                    friendsFirstname[friend.uid] = friend.firstName;

                    if (r.Value.Key)
                        friendsIds.SetValue(friend.uid, i);
                    //else
                        //friendsIdsForInvite.SetValue(friend.uid, i);

                    //load friend icon
                    LoadImgFromURL(friend.pic128x128, delegate (Texture2D pictureTexture)
                    {
                        if (pictureTexture != null)
                        {
                            friendsImage[friend.uid] = pictureTexture;
                        }

                    });

                    var friendOK = new FriendForInvite();
                    //if (r.customParam)
                    //    friends.Add(friend);
                    //else
                    friendOK.id = friend.uid;
                    friendOK.name = friend.lastName + " " + friend.firstName;

                    if (!r.Value.Key)
                        friendsForInvite.Add(friendOK);

                    i++;
                }
                if (r.Value.Key)
                {
                    loginKii(social);
                    ctrAnalyticsClass.sendCustomDimension(7, ctrAnalyticsClass.getGroup(friendsIds.Length, ctrAnalyticsClass.friendGroups)); //invites
                }

            }, r.Value.Value, fields);

        }
        Debug.Log("flagSendAnalytics: " + flagSendAnalytics);

        if (flagSendAnalytics) loginKii(social);
        if (flagSendAnalytics) ctrAnalyticsClass.sendCustomDimension(7, ctrAnalyticsClass.getGroup(friendsIds.Length, ctrAnalyticsClass.friendGroups)); //invites
    }

    private void loginKii(string social)
    {
        Debug.Log("socials - loginKii: " + social);
        if (social == "fb")
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);
            KiiUser.LoginWithSocialNetwork(accessCredential, KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, (KiiUser user, Exception exception) =>
            {
                onLoginKii("");
            });
        }
        else
        {
            //vk or ok
            string username = social + userId;
            string password = "123ABC";
            Debug.Log("socials - kii user: " + username);

            KiiUser.LogIn(username, password, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    KiiUser.Builder builder;
                    builder = KiiUser.BuilderWithName(username);
                    KiiUser userNew = builder.Build();

                    userNew.Register("123ABC", (KiiUser registeredUser, Exception ee) => {
                        if (ee != null)
                        return;
                        else onLoginKii(social);
                    });
                }
                else
                {
                    Debug.Log("socials - kii user isset");
                    onLoginKii(social);
                }
            });
        }
    }

    private void onLoginKii(string social)
    {
        Debug.Log("socials - onLoginKii");
        isLoginKii = true;

        KiiBucket bucket = Kii.Bucket("scores");

        //save user scores
        KiiObject kiiObj = bucket.NewKiiObject(social + userId);

        for (int i = 1; i <= 100; i++)
        {
            int y = ctrProgressClass.progress["score" + i + "_1"] + ctrProgressClass.progress["score" + i + "_2"];
            if (y <= 0) break;
            kiiObj["level" + i] = y;
            kiiObj["lastLevel"] = "level" + i;

        }

        kiiObj["userFbId"] = userId;

        //запись, если есть хотя бы 1 уровень
        if (kiiObj.Has("level1"))
        {
            kiiObj.SaveAllFields(true, (KiiObject savedObj, Exception e) => {
            });
        }

        if (friendsIds != null)
        {
            string[] friendsKii = new string [friendsIds.Length];
            for (int i = 0; i < friendsIds.Length; i++)
            {
                friendsKii[i] = social + friendsIds[i];
            }
            KiiQuery query = new KiiQuery(
            KiiClause.InWithStringValue("_id", friendsKii)
            );
            query.Limit = 50;
            bucket.Query(query, (KiiQueryResult<KiiObject> result, Exception e) => {
                if (e != null)
                {
                    return;
                }
                friendsScore = result;
                setFriendsLastLevel();
            });
        }
        if (GameObject.Find("/root/static/level menu") != null) setBestGamers(GameObject.Find("/root/static/level menu").transform, ctrProgressClass.progress["currentLevel"]);

    }
    private void sendSocialAnalytics(float age, string gender, string social, string fname, string lname, string email = "")
    {
        if (age != 0) ctrAnalyticsClass.sendProfileAttribute("age", age.ToString());
        if (ctrAnalyticsClass.developerIds.Contains(userId))
            gender = "developer";
        if (gender != "") ctrAnalyticsClass.sendProfileAttribute("gender", gender);

        ctrAnalyticsClass.sendProfileAttribute("social id", userId);
        
        //for analytics - Dimension
        if (age != 0) ctrAnalyticsClass.sendCustomDimension(0, ctrAnalyticsClass.getGroup((float)age, ctrAnalyticsClass.ageGroups)); //age
        if (gender != "") ctrAnalyticsClass.sendCustomDimension(1, gender); //gender
        ctrAnalyticsClass.sendCustomDimension(6, social); //Authorisation status

        if (fname != "") ctrAnalyticsClass.setCustomerFirstName(fname);
        if (lname != "") ctrAnalyticsClass.setCustomerLastName(lname);
        if (fname != "" || lname != "") ctrAnalyticsClass.setCustomerFullName(fname + " " + lname);
        if (email != "") ctrAnalyticsClass.setCustomerEmail(email);
    }

}