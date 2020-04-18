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

public delegate void PlayerDataCallBack(UserAccountInfo u);
public delegate void PlayerNotFoundCallBack();
public delegate void PlayerStatsCallBack(List<Statistic> statisticValues);
public delegate void PlayerInventoryCallBack(List<ItemInstance> itemInstances);

public static class PlayFabPlayerData
{
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }

    public static void SetTargetPlayer(string username, PlayerDataCallBack playerDataCallBack, PlayerNotFoundCallBack playerNotFoundCallBack)
    {
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest()
            {
                Username = username
            },
            (_result) => {
                m_accountInfo = _result.AccountInfo;
                playerDataCallBack(_result.AccountInfo);
            },
            (_error) => {
                playerNotFoundCallBack();
                Debug.LogError(_error.GenerateErrorReport());
            }
        );
    }

    public static void GetPlayerStats(UserAccountInfo u, PlayerStatsCallBack playerStatsCallBack)
    {

        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "GetPlayerStats",
                FunctionParameter = new { UserId = u.PlayFabId }
            },
            (_result) => {
                playerStatsCallBack(PlayFab.Json.PlayFabSimpleJson.DeserializeObject<List<Statistic>>(_result.FunctionResult.ToString()));
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

    }

    public static void GetUserInventory(UserAccountInfo u, PlayerInventoryCallBack playerInventoryCallBack)
    {
        PlayFabClientAPI.ExecuteCloudScript(
            new ExecuteCloudScriptRequest()
            {
                FunctionName = "GetPlayerInventory",
                FunctionParameter = new { UserId = u.PlayFabId }
            },
            (_result) => {
                playerInventoryCallBack(PlayFab.Json.PlayFabSimpleJson.DeserializeObject<List<ItemInstance>>(_result.FunctionResult.ToString()));
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }
}
