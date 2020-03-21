using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public static class PlayfabUserInfo {
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }
    public static List<ArtisanKeycap> playerKeycaps;

	public enum UserState {
		InMainMenu, 
		ReadyToPractice, Practicing, ViewingLeaderBoards,
		Disconnected, InLobby, InQueue, ReadyToType,
		WaitingForOpponent, InMatch, WaitingForNextRound
	}
	private static UserState m_userState = UserState.Disconnected;
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
		GameplayConsole.Log(string.Format("New user state: {0}", m_userState));
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
        PersistantCanvas.Instance.StartCoroutine(SetWpm(_totalWords, (int)_secondsSinceStart));
    }

    private static IEnumerator SetWpm(int _totalWords, int _time) {
        bool done = false;

        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "UpdatePlayerWpm",
                FunctionParameter = new { TotalWords = _totalWords, TotalTime = _time },
            },
            (_result) => {
                Debug.Log("Done updating wpm");
                done = true;
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

        while (!done) {
            yield return null;
        }
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

    public static void UpdatePlayerMmr(int _mmr) {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest() {
                FunctionName = "UpdateMmr",
                FunctionParameter = new { mmr_update = _mmr },
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
                        newInventory.Add(KeycapDatabase.Instance.getKeyCapFromID(id));
                    }
                }

                recievedKeycaps = true;
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
        return newInventory;
    }
}
