using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using cm = PlayFab.ClientModels;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Networking;

public static class PlayfabUserInfo {
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }
    private static List<cm::FriendInfo> m_friends;
    public static List<cm::FriendInfo> Friends { get { return m_friends; } }
    public static string Username {
        get {
            if (m_accountInfo != null) {
                return m_accountInfo.Username;
            }
            return string.Empty;
        }
    }
    public static Texture ProfilePicture { get; private set; }

    public enum UserState {
        InMainMenu,
        ReadyToPractice, Practicing, ViewingLeaderBoards,
        Offline, InLobby, InQueue, ReadyToType,
        WaitingForOpponent, InMatch, WaitingForNextRound
    }

    private static UserState m_userState = UserState.Offline;
    public static UserState CurrentUserState { get { return m_userState; } }

    public static void Initalise() {
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest(),
            (_result) => {
                m_accountInfo = _result.AccountInfo;
                PhotonNetwork.player.NickName = _result.AccountInfo.Username;
                SetProfilePicture();
                SetDisplayName();

                GameObject.FindObjectOfType<SceneChanger>().ChangeScene(1);

                ShowStats();
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static void SetProfilePicture() {
        if (string.IsNullOrEmpty(m_accountInfo.TitleInfo.AvatarUrl)) {
            string url = GetGravatarUrl(m_accountInfo.PrivateInfo.Email);
            //set as current pfp
            PlayFabClientAPI.UpdateAvatarUrl(
                new UpdateAvatarUrlRequest() {
                    ImageUrl = url
                },
                (_result) => {  },
                (_error) => { Debug.LogError(_error.GenerateErrorReport() + " URL: " + url); }
            );
            PersistantCanvas.Instance.StartCoroutine(DownloadProfilePicture(url));
        } else {
            PersistantCanvas.Instance.StartCoroutine(DownloadProfilePicture(GetGravatarUrl(m_accountInfo.PrivateInfo.Email)));
        }
    }

    private static IEnumerator DownloadProfilePicture(string _url) {
        using (UnityWebRequest webReq = UnityWebRequestTexture.GetTexture(_url)) {
            yield return webReq.SendWebRequest();
            if (webReq.isNetworkError) {
                Debug.LogError("Network error: " + webReq.error);
            } else {
                ProfilePicture = DownloadHandlerTexture.GetContent(webReq);
            }
        }
    }

    private static string GetGravatarUrl(string _email) {
        using (MD5 md5 = MD5.Create()) {
            byte[] inputBytes =Encoding.ASCII.GetBytes(_email.ToLower());
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            sb.Append(@"https://www.gravatar.com/avatar/");
            for (int i = 0; i < hashBytes.Length; i++) {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString().ToLower();
        }
    }

    public static void SetUserState(UserState _newState) {
		m_userState = _newState;
		//update hastable
		PhotonNetwork.player.SetCustomProperty("UserState", _newState);
        //update playfab
        PlayFabClientAPI.UpdateUserData(
            new UpdateUserDataRequest() {
                Data = new Dictionary<string, string>() {
                    { "PlayerState", FriendDisplay.UserStateToString(_newState) }
                },
                Permission = UserDataPermission.Public
            },
            (_result) => {
                if(_newState == UserState.Offline) {
                    UpdatePlayerRoom("NotInRoom");
                }
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
        //print to console
		GameplayConsole.Log(string.Format("New user state: {0}", m_userState));
	}

    public static void UpdatePlayerRoom(string _room) {
        PlayFabClientAPI.UpdateUserData(
            new cm::UpdateUserDataRequest() {
                Data = new Dictionary<string, string>() {
                        { "RoomName", _room }
                },
                Permission = PlayFab.ClientModels.UserDataPermission.Public
            },
            (_result) => { },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    private static void SetDisplayName() {
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest() { DisplayName = Username },
            (_result) => { },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static void UpdateWpm(int _totalWords, float _secondsSinceStart) {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "UpdatePlayerWpm",
                FunctionParameter = new { TotalWords = _totalWords, TotalTime = _secondsSinceStart },
            },
            (_result) => { },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static void UpdateHighscore(int _newScore) {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "UpdatePlayerHighScore",
                FunctionParameter = new { Points = _newScore },
            },
            (_result) => { },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static void UpdateTotalGames() {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "UpdateTotalGames",
            },
            (_result) => { },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static int GetPlayerStatistic(string _stat) {
        int returnThis = -1;

        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            (_result) => {
                foreach (StatisticValue stat in _result.Statistics) {
                    if (stat.StatisticName.Equals(_stat)) {
                        returnThis = stat.Value;
                        break;
                    }
                }
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

        return returnThis;
    }

	public static void UpdatePlayerMmr(int _mmr) {
		PlayFabClientAPI.ExecuteCloudScript(
			new ExecuteCloudScriptRequest()
			{
				FunctionName = "UpdateMmr",
				FunctionParameter = new { mmr_update = _mmr },
			},
			(_result) => { },
			(_error) => { Debug.LogError(_error.GenerateErrorReport()); }
		);
	}

    public static void UpdateKeycapCustomData(string instanceID, int data, KeySlot ks, InventorySlot invSlot)
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdateKeycapInfo",
                FunctionParameter = new { ItemId = instanceID, Data_update = data },
            },
            (_result) => {
                EditorManager.Instance.ChangeKey(ks, invSlot);
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static void ShowStats() {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "GetPlayerStats",
                FunctionParameter = new { UserId = m_accountInfo.PlayFabId }
            },
            (_result) => {
                List<Statistic> stats = PlayFab.Json.PlayFabSimpleJson.DeserializeObject<List<Statistic>>(_result.FunctionResult.ToString());
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    #region Friends
    public static void GetFriendsList() {
		PlayFabClientAPI.GetFriendsList(
			new GetFriendsListRequest(){
				IncludeFacebookFriends = false,
				IncludeSteamFriends = false
			},
			(_result) => {
                FriendsMenuHandler.Instance.ClearFriendsList();
				foreach (cm::FriendInfo friend in _result.Friends) {
                    if (friend.Tags.Contains("Friends")) {
                        FriendsMenuHandler.Instance.AddToFriendsList(friend);
                    }else if (friend.Tags.Contains("Requestee")) {
                        FriendsMenuHandler.Instance.AddToPendingFriendsList(friend);
                    }
                    m_friends.Add(friend);
                }
                FriendsMenuHandler.Instance.Ready = true;
			},
			(_error) => { Debug.LogError(_error.GenerateErrorReport()); }
		);
	}

	public static void AddFriend(string _friendName) {
        //get friends id
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest() {
                Username = _friendName
            },
            (_result) => {
                PlayFabClientAPI.ExecuteCloudScript(
                    new ExecuteCloudScriptRequest() {
                        FunctionName = "AddFriend",
                        FunctionParameter = new { ReceiverId = _result.AccountInfo.PlayFabId },
                    },
                    (__result) => { Debug.Log(string.Format("Friend request sent to {0}", _friendName)); },
                    (__error) => { Debug.LogError(__error.GenerateErrorReport()); }
                );
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
	}

    public static void AcceptFriend(string _friendName) {
        //get friends id
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest() {
                Username = _friendName
            },
            (_result) => {
                //actually add the friend
                PlayFabClientAPI.ExecuteCloudScript(
                    new ExecuteCloudScriptRequest() {
                        FunctionName = "AcceptFriend",
                        FunctionParameter = new { ReceiverId = _result.AccountInfo.PlayFabId },
                    },
                    (__result) => {
                        Debug.Log(string.Format("Friend request from {0} accepted", _friendName));
                        GetFriendsList();
                    },
                    (__error) => { Debug.LogError(__error.GenerateErrorReport()); }
                );
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static void DenyFriend(string _friendName) {
        //get friends id
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest() {
                Username = _friendName
            },
            (_result) => {
                //get rid of friend
                PlayFabClientAPI.ExecuteCloudScript(
                    new ExecuteCloudScriptRequest() {
                        FunctionName = "DenyFriend",
                        FunctionParameter = new { ReceiverId = _result.AccountInfo.PlayFabId },
                    },
                    (__result) => {
                        Debug.Log(string.Format("Friend request from {0} denied", _friendName));
                        GetFriendsList();
                    },
                    (__error) => { Debug.LogError(__error.GenerateErrorReport()); }
                );
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }
    #endregion
}
