using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class PlayfabUserInfo {
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }

    public static void Initalise() {
        GetAccountInfoRequest req = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(
            req,
            (_result) => { m_accountInfo = _result.AccountInfo; },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

        PersistantCanvas.Instance.StartCoroutine(SetDisplayName());
    }

    private static IEnumerator SetDisplayName() {
        while (GetUsername().Equals("")) {
            yield return null;
        }

        PlayFab.PlayFabClientAPI.UpdateUserTitleDisplayName(
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
                GeneratePlayStreamEvent = true,
            },
            (_result) => {

            },
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
}
