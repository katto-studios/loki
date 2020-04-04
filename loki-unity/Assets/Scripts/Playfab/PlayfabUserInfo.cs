using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using cm = PlayFab.ClientModels;
using System;

public struct ArtisanData
{
    public int equipInfo;
    public string itemInstanceID;

    public ArtisanData(int e, string instanceId)
    {
        equipInfo = e;
        itemInstanceID = instanceId;
    }
}

public static class PlayfabUserInfo {
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }
    public static List<ArtisanKeycap> playerKeycaps = new List<ArtisanKeycap>();
    public static Dictionary<ArtisanKeycap, ArtisanData> artisanData = new Dictionary<ArtisanKeycap, ArtisanData>();
    private static List<cm::FriendInfo> m_friends;
    public static List<cm::FriendInfo> Friends { get { return m_friends; } }

    public enum UserState {
        InMainMenu,
        ReadyToPractice, Practicing, ViewingLeaderBoards,
        Offline, InLobby, InQueue, ReadyToType,
        WaitingForOpponent, InMatch, WaitingForNextRound
    }
    private static UserState m_userState = UserState.Offline;
    public static UserState CurrentUserState { get { return m_userState; } }

    public static void Initalise() {
        GetAccountInfoRequest req = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(
            req,
            (_result) => {
                m_accountInfo = _result.AccountInfo;
                PhotonNetwork.player.NickName = _result.AccountInfo.Username;
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

        PersistantCanvas.Instance.StartCoroutine(SetDisplayName());
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

    private static IEnumerator SetDisplayName() {
        while (GetUsername().Equals("")) {
            yield return null;
        }

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest() { DisplayName = GetUsername() },
            (_result) => { },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static string GetUsername() {
        if (m_accountInfo != null) {
            return m_accountInfo.Username;
        }
        return string.Empty;
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

    //INVENTORY STUFF

    static bool recievedKeycaps;
    public static void UpdatePlayerKeycaps()
    {
        playerKeycaps.Clear();
        artisanData.Clear();
        recievedKeycaps = false;
        PersistantCanvas.Instance.StartCoroutine(GetUserInventoryRequest());
    }

    static IEnumerator GetUserInventoryRequest()
    {
        List<ArtisanKeycap> newInventory = GetUserInventory();

        while (!recievedKeycaps)
        {
            yield return null;
        }

        playerKeycaps = newInventory;

        string dText = "Inventory Items: ";
        foreach (ArtisanKeycap keycap in playerKeycaps)
        {
            dText += keycap.name + ", ";
        }

        if (Keyboard.Instance)
        {
            Debug.Log("Initing");
            Keyboard.Instance.InitKeyboard();
        }

        Debug.Log(dText);
    }

    public static List<ArtisanKeycap> GetUserInventory()
    {
        List<ArtisanKeycap> newInventory = new List<ArtisanKeycap>();
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(), 
            (GetUserInventoryResult result) =>
            {
                foreach (var eachItem in result.Inventory)
                {
                    if (eachItem.ItemClass.Equals("KEYCAP"))
                    {
                        Debug.Log(eachItem.ItemId);
                        string id = eachItem.ItemId;
                        ArtisanKeycap newKey = KeycapDatabase.Instance.getKeyCapFromID(id);
                        newInventory.Add(newKey);

                        string equipIndex = "-2";
                        try { eachItem.CustomData.TryGetValue("EQUIP_SLOT", out equipIndex); }
                        catch { };
                        int ei = int.Parse(equipIndex);
                        Debug.Log(ei);
                        artisanData.Add(newKey, new ArtisanData(ei, eachItem.ItemInstanceId));
                    }
                }

                recievedKeycaps = true;
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
        return newInventory;
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

    public static void UpdateKeycapCustomData(string instanceID, int data)
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "UpdateKeycapInfo",
                FunctionParameter = new { ItemId = instanceID, Data_update = data },
            },
            (_result) => { },
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
				}
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
