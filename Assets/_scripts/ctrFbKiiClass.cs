using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;
using System;
using System.Linq;
//using Boo.Lang;
using com.playGenesis.VkUnityPlugin;
using UnityEngine.SceneManagement;
using Facebook.MiniJSON;


public class ctrFbKiiClass : MonoBehaviour {
	public GameObject fbFriendPhoto;
    public GameObject vkFriend;


    public static ctrFbKiiClass instance = null;
	//список друзей Fb: userFbId = picture
	//public static Dictionary<string, string> friendsPicture = new Dictionary<string, string>();
	//список друзей Fb: userFbId = first_name
	public static Dictionary<string, string> friendsFirstname = new Dictionary<string, string>();

	public static Dictionary<string, Texture2D> friendsImage = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Texture2D> friendsImageForVKInvite = new Dictionary<string, Texture2D>();
    public static Texture2D userImage;

	//public static string[,] friends = new string[101, 2];
	//public struct friendsFbStruct {
	//	public string id;
	//	public string first_name;
  	//	public string picture;
	//	}
	//public static  List<friendsFbStruct>  friends = new List<friendsFbStruct>();

	public static string[] friendsIds = null;
    public static string[] friendsIdsForVKInvite = null;
    public static string userId = "";
    public static bool isLoginKii = false;

	//список друзей Kii: userFbId -> levelNumber -> score
	public static KiiQueryResult<KiiObject> friendsScore = null;

	//список: "Кто из друзей на каком уровне?" level1 = userFbId
	public static Dictionary<string, string> friendsLastLevel = new Dictionary<string, string>();



    public Downloader d;
    public FriendManager friendManagerVK;
    public List<VKUser> friends = new List<VKUser>();
    public List<VKUser> friendsForVKInvite = new List<VKUser>();

    // Awake function from Unity's MonoBehavior
    void Start() {

        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        d = VkApi.VkApiInstance.gameObject.GetComponent<Downloader>();
        friendManagerVK = VkApi.VkApiInstance.gameObject.GetComponent<FriendManager>();

        ctrProgressClass.getProgress();
        //fb
        Debug.Log("Start Kii Class");
        Debug.Log("lang: " + ctrProgressClass.progress["language"]);

        if (staticClass.socialDefault() == "fb")
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback);
            }
        }
        //vk
        else 
        {
            Debug.Log("vk isLogin: " + VkApi.VkApiInstance.IsUserLoggedIn);
            if (VkApi.VkApiInstance.IsUserLoggedIn && ctrProgressClass.progress["vk"] == 1) {
                    onLoggedInVK();
                }
        }
    }
    //fb
    private void InitCallback() {
        if (FB.IsInitialized) {
			Debug.Log("FB.IsInitialized");
            //for tests
			//var perms = new List<string>() { "public_profile", "email", "user_friends" };
			//FB.LogInWithReadPermissions(perms, AuthCallback);


			// Signal an app activation App Event
            //FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

            if (FB.IsLoggedIn && ctrProgressClass.progress["fb"] == 1) {
                Debug.Log("FB.IsLoggedIn");
                // AccessToken class will have session details
                //var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                // Print current access token's granted permissions
                onLoginCompleteFB();
            } 

        } 
    }

    //vk
    public void onLoggedInVK()
    {
        Debug.Log("onLoggedInVK");
        try
        {
            VkApi.VkApiInstance.LoggedIn -= onLoggedInVK;
        }
        catch (System.Exception ex)
        {

        }
        userId = VkApi.CurrentToken.user_id;

        VKRequest r = new VKRequest
        {
            url = "users.get?user_ids=" + userId + "&fields=photo_100,sex,bdate",
            //url = "users.get?user_ids=" + userId + "&photo_50",
            CallBackFunction = OnGotUserInfoVK
        };
        VkApi.VkApiInstance.Call(r);

        var request = new VKRequest()
        {
            //url = "friends.getAppUsers?user_id=" + userId + "&count=100&fields=photo_100",
            //CallBackFunction = OnGetFriendsIdsCompleted,
            url = "apps.getFriendsList?user_id=" + userId + "&type=request&extended=1&count=100&fields=photo_100",
            CallBackFunction = OnGetFriendsCompleted,
            customParam = true
        };
        VkApi.VkApiInstance.Call(request);


        //не установившие приложение

        var request2 = new VKRequest()
        {
            url = "apps.getFriendsList?user_id=" + userId + "&type=invite&extended=1&count=100&fields=photo_100",
            CallBackFunction = OnGetFriendsCompleted,
            customParam = false
        };
        VkApi.VkApiInstance.Call(request2);

        onLoginCompleteVK();
    }

    public void OnGotUserInfoVK(VKRequest r)
    {
        if (r.error != null)
        {
            //hande error here
            Debug.Log(r.error.error_msg);
            return;
        }

        //now we need to deserialize response in json from vk server
        var dict = Json.Deserialize(r.response) as Dictionary<string, object>;
        var users = (System.Collections.Generic.List<object>)dict["response"];
        var user = VKUser.Deserialize(users[0]);

        if (!string.IsNullOrEmpty(user.photo_100))
        {
            DownloadImage(user.photo_100, user, true);
        }
        Debug.Log("user id is " + user.id);
        //Debug.Log("user name is " + user.first_name);
        //Debug.Log("user last name is " + user.last_name);
        Debug.Log("photo " + user.photo_100);


        //for analytics - Profiles
        int age = 0;
        if (!string.IsNullOrEmpty(user.bdate)) age = Mathf.RoundToInt(      (         (float)( ((DateTime.Now - DateTime.Parse(user.bdate)).TotalDays) )      /365)     );
        if (age != 0) ctrAnalyticsClass.sendProfileAttribute("age", age.ToString());
        var gender = "";
        if (ctrAnalyticsClass.developerIds.Contains(userId))
            gender = "developer";
        else if (user.sex == 1)
            gender = "female";
        else if (user.sex == 2)
            gender = "male";
        
        if (gender != "") ctrAnalyticsClass.sendProfileAttribute("gender", gender);

        ctrAnalyticsClass.sendProfileAttribute("social id", userId);
        //for analytics - Dimension
        if (age != 0) ctrAnalyticsClass.sendCustomDimension(0, ctrAnalyticsClass.getGroup((float) age, ctrAnalyticsClass.ageGroups)); //age
        if (gender != "") ctrAnalyticsClass.sendCustomDimension(1, gender); //gender
        ctrAnalyticsClass.sendCustomDimension(6, "VK"); //Authorisation status


        if (!string.IsNullOrEmpty(user.first_name)) ctrAnalyticsClass.setCustomerFirstName(user.first_name);
        if (!string.IsNullOrEmpty(user.last_name)) ctrAnalyticsClass.setCustomerLastName(user.last_name);
        if (!string.IsNullOrEmpty(user.first_name) || !string.IsNullOrEmpty(user.last_name)) ctrAnalyticsClass.setCustomerFullName(user.first_name + " " + user.last_name);
    }


    /*
	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}
	*/

    public void connectVK()
    {
        Debug.Log("connectVK");
        VkApi.VkSetts.forceOAuth = false;
        VkApi.VkApiInstance.LoggedIn += onLoggedInVK;
        VkApi.VkApiInstance.Login();
    }


    public void connect() {
        Debug.Log("FB connect");
        Debug.Log("FB.IsInitialized:" + FB.IsInitialized);
        if (FB.IsInitialized) {
            var perms = new List<string>() { "public_profile", "email", "user_friends"};
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }


    private void AuthCallback(ILoginResult result) {
        Debug.Log("FB AuthCallback: " + FB.IsLoggedIn);
        Debug.Log(result.Error);
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            onLoginCompleteFB();
            // Print current access token's granted permissions

        }

    }

    private void onLoginCompleteVK()
    {
        Debug.Log("onLoginCompleteVK");
        if (ctrProgressClass.progress["vk"] == 0)
        {
            ctrProgressClass.progress["vk"] = 1;
            ctrProgressClass.saveProgress();

        }
        //GetPlayerInfo();
        //GetFriends();
        StartCoroutine(loginKiiVK());
        changeUIPanelFriends();
    }

    private void onLoginCompleteFB() {
		if (ctrProgressClass.progress ["fb"] == 0) {
			ctrProgressClass.progress ["fb"] = 1;
			ctrProgressClass.saveProgress ();

		}
        GetPlayerInfo();
        GetFriends();
        StartCoroutine(loginKiiFB());
        changeUIPanelFriends();

    }

    public static void updateUserScore(string levelName, int score, int lastLevel) {
		if (isLoginKii && userId != "") {
			KiiBucket bucket = Kii.Bucket("scores");
			KiiObject kiiObj = bucket.NewKiiObject(userId);
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

    IEnumerator loginKiiVK()
    {
        yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
        Debug.Log("loginKiiVK");
        Debug.Log("userId: " + userId);

        //vk
        string username = userId;
        string password = "123ABC";

        KiiUser.LogIn(username, password, (KiiUser user, Exception e) =>
        {
            if (e != null)
            {
                Debug.Log(e.HelpLink);
                Debug.Log(e.StackTrace);
                Debug.Log(e.Message);

                KiiUser.Builder builder;
                builder = KiiUser.BuilderWithName(userId);
                //builder.SetEmail("user_123456@example.com");
                //builder.SetGlobalPhone("+819012345678");
                KiiUser userNew = builder.Build();

                userNew.Register("123ABC", (KiiUser registeredUser, Exception ee) => {
                    //user.Register("123ABC", (KiiUser registeredUser, Exception ee) => {
                    if (ee != null)
                    {
                        Debug.Log(ee.HelpLink);
                        Debug.Log(ee.StackTrace);
                        Debug.Log(ee.Message);
                        // handle error
                        return;
                    }
                    else afterLoginKii();
                });


                //return;
            }
            else
            {
                afterLoginKii();

            }
        });
    }

    IEnumerator loginKiiFB() {
        yield return StartCoroutine(staticClass.waitForRealTime(0.2F));
        Debug.Log("loginKiiFB");


        Dictionary<string, string> accessCredential = new Dictionary<string, string>();
        accessCredential.Add("accessToken", Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);
        KiiUser.LoginWithSocialNetwork(accessCredential, KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, (KiiUser user, Exception exception) =>
        {
            afterLoginKii();
        });
    }

    private void afterLoginKii()
    {
        Debug.Log("afterLoginKii");
        //KiiUser.LoginWithSocialNetwork(accessCredential, KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, (KiiUser user, Exception exception) => {
        isLoginKii = true;

        KiiBucket bucket = Kii.Bucket("scores");

        //save user scores
        KiiObject kiiObj = bucket.NewKiiObject(userId);

        for (int i = 1; i <= 100; i++)
        {
            int y = ctrProgressClass.progress["score" + i.ToString() + "_1"] + ctrProgressClass.progress["score" + i.ToString() + "_2"];
            if (y <= 0) break;
            kiiObj["level" + i.ToString()] = y;
            kiiObj["lastLevel"] = "level" + i.ToString();

        }
        kiiObj["userFbId"] = userId;

        //запись, если есть хотя бы 1 уровень
        if (kiiObj.Has("level1"))
        {
            kiiObj.SaveAllFields(true, (KiiObject savedObj, Exception e) => {
                if (e != null)
                {
                }
                else
                {
                }
            });

        }


        if (friendsIds != null)
        {
            KiiQuery query = new KiiQuery(
            KiiClause.InWithStringValue("_id", friendsIds)
            );
            query.Limit = 50;
            bucket.Query(query, (KiiQueryResult<KiiObject> result, Exception e) => {
                if (e != null)
                {
                    //Debug.LogError("Failed to get friend scores" + e.ToString());
                    return;
                }
                friendsScore = result;
                setFriendsLastLevel();
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

				//Debug.Log ("setFriendImgMap");

				if (!friendsImage.ContainsKey (id)) {
					// We don't have this players image yet, request it now
					LoadFriendImgFromID (id, pictureTexture => {
						if (pictureTexture != null) {
							friendsImage.Add (id, pictureTexture);

							GameObject newPhoto = Instantiate (ctrFbKiiClass.instance.fbFriendPhoto, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
							newPhoto.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (pictureTexture, new Rect (0, 0, pictureTexture.width, pictureTexture.height), new Vector2 (0.5F, 0.5F), 1);

							newPhoto.transform.parent = levelIslandGem;
							newPhoto.transform.localPosition = new Vector3 (-165, 0, levelIslandGem.localPosition.z);
							newPhoto.transform.localScale = new Vector3 (1, 1, 1);

						}
					});
				} else {
				
					GameObject newPhoto = Instantiate (ctrFbKiiClass.instance.fbFriendPhoto, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
					newPhoto.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (friendsImage[id], new Rect (0, 0, friendsImage[id].width, friendsImage[id].height), new Vector2 (0.5F, 0.5F), 1);

					newPhoto.transform.parent = levelIslandGem;
					newPhoto.transform.localPosition = new Vector3 (-165, 0, levelIslandGem.localPosition.z);
					newPhoto.transform.localScale = new Vector3 (1, 1, 1);
				
				}


			}
		}
	}

	public static void setBestGamers(Transform levelMenu, int levelNumber) {
        Debug.Log ("setBestGamers");
        Transform socialMenu = levelMenu.GetChild(2);
        if (ctrProgressClass.progress["fb"] == 1 && FB.IsLoggedIn)
            socialMenu = levelMenu.GetChild(2);
        else if (ctrProgressClass.progress["vk"] == 1 && VkApi.VkApiInstance.IsUserLoggedIn)
            socialMenu = levelMenu.GetChild(3);
        else return;
        Debug.Log(socialMenu.name);

        //if (ctrProgressClass.progress ["fb"] == 1 && FB.IsLoggedIn) {

            //выключаем панель fb login, включаем панель с лучшими игроками, выключаем каждого игрока (всего 3)
            socialMenu.GetChild (0).gameObject.SetActive (false);
            socialMenu.GetChild (1).gameObject.SetActive (true);
            socialMenu.GetChild (1).GetChild (0).gameObject.SetActive (false);
            socialMenu.GetChild (1).GetChild (1).gameObject.SetActive (false);
			socialMenu.GetChild (1).GetChild (2).gameObject.SetActive (false);

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
                    socialMenu.GetChild (1).GetChild (i).gameObject.SetActive (true);

					//photo

					//если юзер
					if (userId == score.Key) {
						if (userImage != null) socialMenu.GetChild (1).GetChild (i).GetChild(0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (userImage, new Rect(0, 0, userImage.width, userImage.height), new Vector2(0.5F, 0.5F), 1);
                        socialMenu.GetChild (1).GetChild (i).GetChild (1).GetComponent<UILabel> ().text = score.Value.ToString ();
						i++;

					}
					if (friendsImage.ContainsKey(score.Key)) {
                        socialMenu.GetChild (1).GetChild (i).GetChild(0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (friendsImage[score.Key], new Rect(0, 0, friendsImage[score.Key].width, friendsImage[score.Key].height), new Vector2(0.5F, 0.5F), 1);
                        socialMenu.GetChild (1).GetChild (i).GetChild (1).GetComponent<UILabel> ().text = score.Value.ToString ();
						i++;

					
					}









				} else
					break;
			}
		//}	
	
	}

	public void invite() {
		if (FB.IsInitialized) {
			ctrFbRequest.RequestChallenge ();
			//GetInvitableFriends();
		}
	}
    //-----------------------------------------------------------------------------------------------------------------------------------------------
	//---------------------------------------------------------- FB GRAPH ---------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------------------------------------------------------------
	public static void GetPlayerInfo() {
        string queryString = "/me?fields=id,first_name,age_range,gender,picture.width(100).height(100)";
        FB.API(queryString, HttpMethod.GET, GetPlayerInfoCallback);
    }

    private static void GetPlayerInfoCallback(IGraphResult result) {
        //Debug.Log("GetPlayerInfoCallback");
        if (result.Error != null) {
            //Debug.LogError(result.Error);
            return;
        }
        //Debug.Log(result.RawResult);
 
        // Save player name
        string name;
        if (result.ResultDictionary.TryGetValue("first_name", out name)) {
            //GameStateManager.Username = name;
        }
        Dictionary<string, object> ageRange;
        var age = 0F;
        if (result.ResultDictionary.TryGetValue("age_range", out ageRange))
        {
            var d = ageRange["min"];
            Debug.Log(d.GetType());
            age = float.Parse( ageRange["min"].ToString());
        }
        var gender = "";
        result.ResultDictionary.TryGetValue("gender", out gender);

        string id;
        if (result.ResultDictionary.TryGetValue("id", out id)) {
            userId = id;
            //GameStateManager.Username = name;
        }
        
		//Fetch player profile picture from the URL returned
		string playerImgUrl = DeserializePictureURL(result.ResultDictionary);
		LoadImgFromURL(playerImgUrl, delegate(Texture2D pictureTexture)
			{
				// Setup the User's profile picture
				if (pictureTexture != null)
				{
					userImage = pictureTexture;
				}

				// Redraw the UI
				//GameStateManager.CallUIRedraw();
			});

        //for analytics - Profiles
        
        if (age != 0) ctrAnalyticsClass.sendProfileAttribute("age", age.ToString());
        if (ctrAnalyticsClass.developerIds.Contains(userId))
            gender = "developer";
        if (gender != "") ctrAnalyticsClass.sendProfileAttribute("gender", gender);
        
        ctrAnalyticsClass.sendProfileAttribute("social id", userId);
        //for analytics - Dimension
        if (age != 0) ctrAnalyticsClass.sendCustomDimension(0, ctrAnalyticsClass.getGroup((float)age, ctrAnalyticsClass.ageGroups)); //age
        if (gender != "") ctrAnalyticsClass.sendCustomDimension(1, gender); //gender
        ctrAnalyticsClass.sendCustomDimension(6, "FB"); //Authorisation status

        string fname;
        if (result.ResultDictionary.TryGetValue("first_name", out fname)) ctrAnalyticsClass.setCustomerFirstName(fname);
        string lname;
        if (result.ResultDictionary.TryGetValue("last_name", out lname)) ctrAnalyticsClass.setCustomerLastName(lname);
        if (fname != "" || lname != "") ctrAnalyticsClass.setCustomerFullName(fname + " " + lname);
        string email;
        if (result.ResultDictionary.TryGetValue("email", out email)) ctrAnalyticsClass.setCustomerEmail(email);


    }
    /*
	// In the above request it takes two network calls to fetch the player's profile picture.
	// If we ONLY needed the player's profile picture, we can accomplish this in one call with the /me/picture endpoint.
	//
	// Make a Graph API GET call to /me/picture to retrieve a players profile picture in one call
	// See: https://developers.facebook.com/docs/graph-api/reference/user/picture/
	public static void GetPlayerPicture()
	{
		FB.API(GraphUtil.GetPictureQuery("me", 128, 128), HttpMethod.GET, delegate(IGraphResult result)
			{
				Debug.Log("PlayerPictureCallback");
				if (result.Error != null)
				{
					Debug.LogError(result.Error);
					return;
				}
				if (result.Texture ==  null)
				{
					Debug.Log("PlayerPictureCallback: No Texture returned");
					return;
				}

				// Setup the User's profile picture
				GameStateManager.UserTexture = result.Texture;

				// Redraw the UI
				GameStateManager.CallUIRedraw();
			});
	}
	#endregion
	*/
    #region Friends
    // We can fetch information about a player's friends via the Graph API user edge /me/friends
    // This endpoint returns an array of friends who are also playing the same game.
    // See: https://developers.facebook.com/docs/graph-api/reference/user/friends
    //
    // We can use this data to provide a set of real people to play against, showing names
    // and pictures of the player's friends to make the experience feel even more personal.
    //
    // The /me/friends edge requires an additional permission, user_friends. Without
    // this permission, the response from the endpoint will be empty. If we know the user has
    // granted the user_friends permission but we see an empty list of friends returned, then
    // we know that the user has no friends currently playing the game.
    //
    // Note:
    // In this instance we are making two calls, one to fetch the player's friends who are already playing the game
    // and another to fetch invitable friends who are not yet playing the game. It can be more performant to batch 
    // Graph API calls together as Facebook will parallelize independent operations and return one combined result.
    // See more: https://developers.facebook.com/docs/graph-api/making-multiple-requests
    //
    public static void GetFriends() {
        string queryString = "/me/friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
        FB.API(queryString, HttpMethod.GET, GetFriendsCallback);
    }

    private static void GetFriendsCallback(IGraphResult result) {
        //Debug.Log("GetFriendsCallback");
        if (result.Error != null) {
            //Debug.LogError(result.Error);
            return;
        }
        //Debug.Log(result.RawResult);

        // Store /me/friends result
        object dataList;
        if (result.ResultDictionary.TryGetValue("data", out dataList)) {
            CacheFriends((System.Collections.Generic.List<object>)dataList);
            //var friendsList = (List<object>)dataList;
            //CacheFriends(friendsList);
        }
    }

    // We can fetch information about a player's friends who are not yet playing our game
    // via the Graph API user edge /me/invitable_friends
    // See more about Invitable Friends here: https://developers.facebook.com/docs/games/invitable-friends
    //
    // The /me/invitable_friends edge requires an additional permission, user_friends.
    // Without this permission, the response from the endpoint will be empty.
    //
    // Edge: https://developers.facebook.com/docs/graph-api/reference/user/invitable_friends
    // Nodes returned are of the type: https://developers.facebook.com/docs/graph-api/reference/user-invitable-friend/
    // These nodes have the following fields: profile picture, name, and ID. The ID's returned in the Invitable Friends
    // response are not Facebook IDs, but rather an invite tokens that can be used in a custom Game Request dialog.
    //
    // Note! This is different from the following Graph API:
    // https://developers.facebook.com/docs/graph-api/reference/user/friends
    // Which returns the following nodes:
    // https://developers.facebook.com/docs/graph-api/reference/user/
    //
    public static void GetInvitableFriends() {
        string queryString = "/me/invitable_friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
        FB.API(queryString, HttpMethod.GET, GetInvitableFriendsCallback);
    }

    private static void GetInvitableFriendsCallback(IGraphResult result) {
        //Debug.Log("GetInvitableFriendsCallback");
        if (result.Error != null) {
            //Debug.LogError(result.Error);
            return;
        }
        //Debug.Log(result.RawResult);

        // Store /me/invitable_friends result
        object dataList;
        if (result.ResultDictionary.TryGetValue("data", out dataList)) {
            //friends = (List<object>)dataList;
            //CacheFriends(friends);
        }
    }

    private static void CacheFriends(System.Collections.Generic.List<object> newFriends) {
		int i = 0;
		friendsIds = new string[newFriends.Count];
		foreach (Dictionary<string, object> newFriend in newFriends) {
			
			string idFriend = newFriend ["id"] as string;
			//string picture = newFriend ["picture"] as string;
			string first_name = newFriend ["first_name"] as string;
			//friendsPicture[idFriend] = picture;
			friendsFirstname[idFriend] = first_name;
			//newFriends.
			//friends.Add("qqq", "www");
			//friends.Add(new friendsFbStruct() {id = "qqq", first_name = "www", picture = "eee"});
			//var dd =  friends.Find (x => x.id.Equals("qqq"));
			//Debug.Log (dd.first_name);
			//friends[newFriend["id"] as String]["first_name"] = newFriend["first_name"] as String;
			//friends[newFriend["id"] as String]["picture"] = newFriend["picture"] as String;
			//Debug.Log ("id friend: " + idFriend);
			//Debug.Log (idFriend);
			//Debug.Log (picture);
			friendsIds.SetValue (idFriend, i);
			//friendsIds[i] = id;
			i++;
		}
        ctrAnalyticsClass.sendCustomDimension(7, ctrAnalyticsClass.getGroup(friendsIds.Length, ctrAnalyticsClass.friendGroups)); //invites

        //var friend = newFriends[0] as Dictionary<string, object>;
        //Debug.Log(friend["first_name"] as string);
        //Debug.Log(friend["id"] as string);
        /*
		if (GameStateManager.Friends != null && GameStateManager.Friends.Count > 0)
		{
			GameStateManager.Friends.AddRange(newFriends);
		}
		else
		{
			GameStateManager.Friends = newFriends;
		}
		*/
    }
    #endregion
    /*
	#region Scores
	// Fetch leaderboard scores from Scores API
	// Scores API documentation: https://developers.facebook.com/docs/games/scores
	//
	// With player scores being written to the Graph API, we now have a data set on
	// which to build a social leaderboard. By calling the /app/scores endpoint for
	// your app, with a user access token, you get back a list of the current player's
	// friends' scores, ordered by score.
	//
	public static void GetScores ()
	{
		//FB.API("/app/scores?fields=score,user.limit(20)", HttpMethod.GET, GetScoresCallback);
		FB.API("/app/scores?fields=score,user.limit(20)", HttpMethod.GET, GetScoresCallback);
	}

	private static void GetScoresCallback(IGraphResult result) 
	{
		Debug.Log("GetScoresCallback");
		if (result.Error != null)
		{
			Debug.LogError(result.Error);
			return;
		}
		Debug.Log(result.RawResult);

		// Parse scores info
		var scoresList = new List<object>();

		object scoresh;
		if (result.ResultDictionary.TryGetValue ("data", out scoresh)) 
		{
			scoresList = (List<object>) scoresh;
		}

		// Parse score data
		HandleScoresData (scoresList);

		// Redraw the UI
		GameStateManager.CallUIRedraw();
	}

	private static void HandleScoresData (List<object> scoresResponse)
	{
		var structuredScores = new List<object>();
		foreach(object scoreItem in scoresResponse) 
		{
			// Score JSON format
			// {
			//   "score": 4,
			//   "user": {
			//      "name": "Chris Lewis",
			//      "id": "10152646005463795"
			//   }
			// }

			var entry = (Dictionary<string,object>) scoreItem;
			var user = (Dictionary<string,object>) entry["user"];
			string userId = (string)user["id"];

			if (string.Equals(userId, AccessToken.CurrentAccessToken.UserId))
			{
				// This entry is the current player
				int playerHighScore = GraphUtil.GetScoreFromEntry(entry);
				Debug.Log("Local players score on server is " + playerHighScore);
				if (playerHighScore < GameStateManager.Score)
				{
					Debug.Log("Locally overriding with just acquired score: " + GameStateManager.Score);
					playerHighScore = GameStateManager.Score;
				}

				entry["score"] = playerHighScore.ToString();
				GameStateManager.HighScore = playerHighScore;
			}

			structuredScores.Add(entry);
			if (!GameStateManager.FriendImages.ContainsKey(userId))
			{
				// We don't have this players image yet, request it now
				LoadFriendImgFromID (userId, pictureTexture =>
					{
						if (pictureTexture != null)
						{
							GameStateManager.FriendImages.Add(userId, pictureTexture);
							GameStateManager.CallUIRedraw();
						}
					});
			}
		}

		GameStateManager.Scores = structuredScores;
	}
	*/
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






    //-------------------------------------------VK-----------------------------------------------------
    /*
    void OnGetFriendsIdsCompleted(VKRequest arg1)
    {
        //проверяем на ошибки
        if (arg1.error != null)
        {
            Debug.Log(arg1.error.error_msg);
            //FindObjectOfType<GlobalErrorHandler>().Notification.Notify(arg1);
            return;
        }

        var dict = Json.Deserialize(arg1.response) as Dictionary<string, object>;
        List<string> resp2 = new List<string>();
        try
        {
            var resp = (System.Collections.Generic.List<object>)dict["response"];
            resp2 = new System.Collections.Generic.List<string>();
            //friendsIdsForVKInviteTemp = resp2;
            foreach (var item in resp)
            {
                resp2.Add(item.ToString());
            }

            var g = resp.First();
           var request = new VKRequest()
            {
                url = "users.get?user_ids=" + string.Join(",", resp2.ToArray()) + "&count=100&fields=photo_100",
                CallBackFunction = OnGetFriendsCompleted,
            };
            VkApi.VkApiInstance.Call(request);
        }

        catch (System.Exception ex)
        {

        }

        
    }
    */

    void OnGetFriendsCompleted(VKRequest arg1)
    {
        //проверяем на ошибки
        if (arg1.error != null)
        {
            //FindObjectOfType<GlobalErrorHandler>().Notification.Notify(arg1);
            return;
        }

        var dict = Json.Deserialize(arg1.response) as Dictionary<string, object>;

        var resp = (Dictionary<string, object>)dict["response"];
        var items = (List<object>)resp["items"];
        if (arg1.customParam)
            friendsIds = new string[items.Count];
        else
            friendsIdsForVKInvite = new string[items.Count];
        var i = 0;
        if (arg1.customParam) ctrAnalyticsClass.sendCustomDimension(7, ctrAnalyticsClass.getGroup(friendsIds.Length, ctrAnalyticsClass.friendGroups)); //invites


        foreach (var item in items)
        {
            var friend = VKUser.Deserialize(item);
            if (arg1.customParam)
                friends.Add(friend);
            else
            
                friendsForVKInvite.Add(friend);

            if (!string.IsNullOrEmpty(friend.photo_100))
                {
                    DownloadImage(friend.photo_100, friend, false);
                }

            if (arg1.customParam)
                friendsIds.SetValue(friend.id.ToString(), i);
            else
                friendsIdsForVKInvite.SetValue(friend.id.ToString(), i);
            
            friendsFirstname[friend.id.ToString()] = friend.first_name;
            i++;

        }
        

    }

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
        //Debug.Log("setFriendsForInvite");
        go.SetActive(true);
        foreach (var f in friendsForVKInvite)
        {
            if (go.transform.GetChild(0).GetChild(0).FindChild(f.id + "(Clone)") != null)
            {
                //Debug.Log("exist vk friend");
                continue;
            }
            var friend = Instantiate(vkFriend) as GameObject;
            friend.transform.parent = go.transform.GetChild(0).GetChild(0);
            if (friendsImage.ContainsKey(f.id.ToString()))
                vkFriend.transform.GetChild(0).GetComponent<UITexture>().mainTexture =
                    friendsImage[f.id.ToString()];

            friend.transform.localScale = new Vector3(1, 1, 1);
            friend.transform.localPosition = new Vector3(0, 0, 0);
            vkFriend.transform.GetChild(0).GetComponent<UIWidget>().depth = 2;
            vkFriend.transform.GetChild(1).GetComponent<UILabel>().text = f.first_name + " " + f.last_name;
            vkFriend.name = f.id.ToString();
            //Debug.Log(vkFriend.name);
            //vkFriend.name = f.id.ToString();
        }
        GameObject.Find("/root/static/level menu/button exit level menu").GetComponent<SpriteRenderer>().sortingOrder = 200;

    }

    public void vkInviteFriend(string name)
    {
        string title = "Play Feed the Spider with me!";
        string message = "Feed the Spider is cool! Check it out.";
        Debug.Log(name);
        VKRequest r2 = new VKRequest()
        {
            url = "apps.sendRequest?user_id=" + name + "&text=" + message + "&type=invite&name=" + title,
            CallBackFunction = OnAppSendRequest
        };
        VkApi.VkApiInstance.Call(r2);
    }

    public void changeUIPanelFriends()
    {
        if (ctrProgressClass.progress["fb"] == 1 && SceneManager.GetActiveScene().name == "level menu")
        {
            
            GameObject.Find("/root/static/level menu/facebook").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("/root/static/level menu/facebook").transform.GetChild(1).gameObject.SetActive(true);
        }
        if (ctrProgressClass.progress["vk"] == 1 && SceneManager.GetActiveScene().name == "level menu")
        {
            GameObject.Find("/root/static/level menu/vk").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("/root/static/level menu/vk").transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public virtual void OnAppSendRequest(VKRequest r)
    {
        if (r.error != null)
        {
            Debug.Log(r.error.error_msg);
            //GlobalErrorHandler.Instance.Notification.Notify(r);
            return;
        }
        Debug.Log(r.response);
    
    }




    public void clickJoinGroup(string url)
    {

        if (ctrProgressClass.progress["vk"] == 1)
        {
            VKRequest r = new VKRequest
            {
                url = url,
                //url = "users.get?user_ids=" + userId + "&photo_50",
                CallBackFunction = onGroupResult
            };
            VkApi.VkApiInstance.Call(r);
        }
        if (ctrProgressClass.progress["fb"] == 1)
        {
            FB.GameGroupJoin("1677948472504355", instance.onGroupResult);
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
            GameObject.Find("/settings folder/settings/facebook").SetActive(false);
        }
        ctrProgressClass.progress["fb"] = 0;
        ctrProgressClass.progress["vk"] = 0;
        ctrProgressClass.saveProgress();
        
    }

    public void  onGroupResult<T>(T result)
    {
        Debug.Log("onGroupResult");
    }

}
