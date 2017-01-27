using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;
using System;
using System.Linq;
using com.playGenesis.VkUnityPlugin;
using UnityEngine.SceneManagement;
using Facebook.MiniJSON;

public class ctrFbKiiClass : MonoBehaviour {
	public GameObject fbFriendPhoto;


	public static ctrFbKiiClass instance = null;
	//список друзей Fb: userFbId = picture
	//public static Dictionary<string, string> friendsPicture = new Dictionary<string, string>();
	//список друзей Fb: userFbId = first_name
	public static Dictionary<string, string> friendsFirstname = new Dictionary<string, string>();

	public static Dictionary<string, Texture2D> friendsImage = new Dictionary<string, Texture2D>();
	public static Texture2D userImage;

	//public static string[,] friends = new string[101, 2];
	//public struct friendsFbStruct {
	//	public string id;
	//	public string first_name;
  	//	public string picture;
	//	}
	//public static  List<friendsFbStruct>  friends = new List<friendsFbStruct>();

	public static string[] friendsIds = null;
	public static string userId = "";
    public static bool isLoginKii = false;

	//список друзей Kii: userFbId -> levelNumber -> score
	public static KiiQueryResult<KiiObject> friendsScore = null;

	//список: "Кто из друзей на каком уровне?" level1 = userFbId
	public static Dictionary<string, string> friendsLastLevel = new Dictionary<string, string>();



    public Downloader d;
    public FriendManager friendManagerVK;
    public List<VKUser> friends = new List<VKUser>();

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
        if (ctrProgressClass.progress["language"] == 1)
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback);
            }
        }
        //vk
        else if (ctrProgressClass.progress["language"] == 2)
        {
            if (VkApi.VkApiInstance.IsUserLoggedIn) {
                    onLoggedInVK();
                }
        }
    }
    //fb
    private void InitCallback() {
        if (FB.IsInitialized) {
			//for tests
			//var perms = new List<string>() { "public_profile", "email", "user_friends" };
			//FB.LogInWithReadPermissions(perms, AuthCallback);


			// Signal an app activation App Event
            //FB.ActivateApp();
            // Continue with Facebook SDK
            // ...

            if (FB.IsLoggedIn) {
                // AccessToken class will have session details
                //var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                // Print current access token's granted permissions
                onLoginComplete();
            } 

        } 
    }

    //vk
    public void onLoggedInVK()
    {
        try
        {
            VkApi.VkApiInstance.LoggedIn -= onLoggedInVK;
        }
        catch (System.Exception ex)
        {

        }
        userId = VkApi.CurrentToken.user_id;

        //VkApi.VkApiInstance.
        //     VKRequest r = new VKRequest
        //{
        //    url = "users.get?user_ids=51066050&photo_50",
        //    CallBackFunction = OnGotUserInfo
        //};
        //VkApi.VkApiInstance.Call(r);

        var request = new VKRequest()
        {
            url = "friends.getAppUsers?user_id=" + VkApi.CurrentToken.user_id + "&count=100&fields=photo_100",
            CallBackFunction = OnGetFriendsIdsCompleted,
        };
        VkApi.VkApiInstance.Call(request);
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


    public void connect() {
        if (FB.IsInitialized) {
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }


    private void AuthCallback(ILoginResult result) {
        if (FB.IsLoggedIn) {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            onLoginComplete();
            // Print current access token's granted permissions

        }

    }

    private void onLoginComplete() {
		if (ctrProgressClass.progress ["fb"] == 0) {
			ctrProgressClass.progress ["fb"] = 1;
			ctrProgressClass.saveProgress ();

		}
        GetPlayerInfo();
        GetFriends();
        StartCoroutine(loginKii());

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

    IEnumerator loginKii() {
        yield return StartCoroutine(staticClass.waitForRealTime(0.2F));

        Dictionary<string, string> accessCredential = new Dictionary<string, string>();
        accessCredential.Add("accessToken", Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);
        KiiUser.LoginWithSocialNetwork(accessCredential, KiiCorp.Cloud.Storage.Connector.Provider.FACEBOOK, (KiiUser user, Exception exception) => {
            isLoginKii = true;

            KiiBucket bucket = Kii.Bucket("scores");

            //save user scores
            KiiObject kiiObj = bucket.NewKiiObject(userId);
            
            for (int i = 1; i <= 100; i++) {
                int y = ctrProgressClass.progress["score" + i.ToString() + "_1"] + ctrProgressClass.progress["score" + i.ToString() + "_2"];
                if (y <= 0) break;
                kiiObj["level" + i.ToString()] = y;
				kiiObj["lastLevel"] = "level" + i.ToString();

            }
			kiiObj["userFbId"] = userId;

             //запись, если есть хотя бы 1 уровень
            if (kiiObj.Has("level1")) {
                kiiObj.SaveAllFields(true, (KiiObject savedObj, Exception e) => {
                    if (e != null) {
                    }
                    else {
                    }
                });

            }


			if (friendsIds != null) {
                KiiQuery query = new KiiQuery(
				KiiClause.InWithStringValue("_id", friendsIds)
                );
                query.Limit = 50;
                bucket.Query(query, (KiiQueryResult<KiiObject> result, Exception e) => {
                    if (e != null) {
						//Debug.LogError("Failed to get friend scores" + e.ToString());
						return;
                    }
					friendsScore = result;
					setFriendsLastLevel();
               });
            }
        });
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

	public static void setBestGamers(Transform fbMenu, int levelNumber) {
		//Debug.Log ("setBestGamers");

		if (ctrProgressClass.progress ["fb"] == 1 && FB.IsLoggedIn) {

			//выключаем панель fb login, включаем панель с лучшими игроками, выключаем каждого игрока (всего 3)
			fbMenu.GetChild (0).gameObject.SetActive (false);
			fbMenu.GetChild (1).gameObject.SetActive (true);
			fbMenu.GetChild (1).GetChild (0).gameObject.SetActive (false);
			fbMenu.GetChild (1).GetChild (1).gameObject.SetActive (false);
			fbMenu.GetChild (1).GetChild (2).gameObject.SetActive (false);

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
					fbMenu.GetChild (1).GetChild (i).gameObject.SetActive (true);

					//photo

					//если юзер
					if (userId == score.Key) {
						if (userImage != null) fbMenu.GetChild (1).GetChild (i).GetChild(0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (userImage, new Rect(0, 0, userImage.width, userImage.height), new Vector2(0.5F, 0.5F), 1);
						fbMenu.GetChild (1).GetChild (i).GetChild (1).GetComponent<UILabel> ().text = score.Value.ToString ();
						i++;

					}
					if (friendsImage.ContainsKey(score.Key)) {
						fbMenu.GetChild (1).GetChild (i).GetChild(0).GetComponent<SpriteRenderer> ().sprite = Sprite.Create (friendsImage[score.Key], new Rect(0, 0, friendsImage[score.Key].width, friendsImage[score.Key].height), new Vector2(0.5F, 0.5F), 1);
						fbMenu.GetChild (1).GetChild (i).GetChild (1).GetComponent<UILabel> ().text = score.Value.ToString ();
						i++;

					
					}









				} else
					break;
			}
		}	
	
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
        string queryString = "/me?fields=id,first_name,picture.width(100).height(100)";
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
            CacheFriends((List<object>)dataList);
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

    private static void CacheFriends(List<object> newFriends) {
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
    void OnGetFriendsIdsCompleted(VKRequest arg1)
    {
        //проверяем на ошибки
        if (arg1.error != null)
        {
            FindObjectOfType<GlobalErrorHandler>().Notification.Notify(arg1);
            return;
        }

        var dict = Json.Deserialize(arg1.response) as Dictionary<string, object>;
        try
        {
            var resp = (List<object>)dict["response"];
            var resp2 = new List<string>();
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

    void OnGetFriendsCompleted(VKRequest arg1)
    {
        //проверяем на ошибки
        if (arg1.error != null)
        {
            FindObjectOfType<GlobalErrorHandler>().Notification.Notify(arg1);
            return;
        }

        var dict = Json.Deserialize(arg1.response) as Dictionary<string, object>;
        Debug.Log(dict["response"].GetType());

//        try
//        {
            var resp = (List<object>) dict["response"];
            //var items = (List<object>) resp["items"];
            var items = resp;
        friendsIds = new string[items.Count];
        var i = 0;
        foreach (var item in items)
        {
            var friend = VKUser.Deserialize(item);
                friends.Add(friend);
                if (!string.IsNullOrEmpty(friend.photo_100))
                {
                    DownloadFriendImage(friend.photo_100, friend);
                }
            friendsIds.SetValue(friend.id.ToString(), i);
            friendsFirstname[friend.id.ToString()] = friend.first_name;
            i++;

        }

    }

    private void DownloadFriendImage(string url, VKUser friend)
    {
        Action<DownloadRequest> doOnFinish = (d) =>
        {
            var fid = (long)d.CustomData[0];
            if (d.DownloadResult.error == null && friend.id == fid)
            {

                friendsImage.Add(friend.id.ToString(), d.DownloadResult.texture);
            }

        };
        var r = new DownloadRequest
        {
            url = url,
            onFinished = doOnFinish,
            CustomData = new object[] { friend.id }
        };
        VkApi.Downloader.download(r);
    }
    


}
