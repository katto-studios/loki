using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using cm = PlayFab.ClientModels;

public static class PlayfabUserInfo {
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }
	private static List<cm::FriendInfo> m_friends;
	public static List<cm::FriendInfo> Friends { get { return m_friends; } }

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
            (_result) => { m_accountInfo = _result.AccountInfo; },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

		PlayFabClientAPI.GetFriendsList(
			new GetFriendsListRequest() {
				IncludeFacebookFriends = false,
				IncludeSteamFriends = false,
				XboxToken = null
			},
			(_results) => {
				m_friends = _results.Friends;
			},
			(_error) => { Debug.LogError(_error.GenerateErrorReport()); }
		);

		SetUserState(UserState.InMainMenu);

        PersistantCanvas.Instance.StartCoroutine(SetDisplayName());
    }

	public static void SetUserState(UserState _newState) {
		m_userState = _newState;
		//update playfab
		PlayFabClientAPI.UpdateUserData(
			new UpdateUserDataRequest() {
				Data = new Dictionary<string, string>() {
					{ "PlayerState", _newState.ToString() }
				}
			},
			(_result) => { },
			(_error) => { Debug.LogError(_error.GenerateErrorReport()); }
		);
		//update hastable
		PhotonNetwork.player.SetCustomProperty("UserState", _newState);
		Debug.Log(string.Format("New user state: {0}", m_userState));
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
                FunctionParameter = new { TotalWords = _totalWords, TotalTime = (int)_secondsSinceStart },
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
}
