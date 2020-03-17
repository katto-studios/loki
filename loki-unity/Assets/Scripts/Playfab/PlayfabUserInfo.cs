using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public static class PlayfabUserInfo {
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }
    public static List<ArtisanKeycap> playerKeycaps;
    public static Dictionary<ArtisanKeycap, int> keycapEquipInfo;

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
                        ArtisanKeycap newKey = KeycapDatabase.Instance.getKeyCapFromID(id);
                        newInventory.Add(newKey);

                        string equipIndex = "-2";
                        eachItem.CustomData.TryGetValue("EQUIP_SLOT", out equipIndex);
                        int ei = int.Parse(equipIndex);
                        Debug.Log(ei);
                        keycapEquipInfo.Add(newKey, ei);
                    }
                }

                recievedKeycaps = true;
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
        return newInventory;
    }
}
