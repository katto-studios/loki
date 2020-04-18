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
public delegate void PlayerStatsCallBack(List<PlayerLeaderboardEntry> statisticValues);
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

    public static void GetLeaderboardAroundPlayer(UserAccountInfo u, PlayerStatsCallBack playerStatsCallBack)
    {

        PlayFabClientAPI.GetLeaderboardAroundPlayer(
            new GetLeaderboardAroundPlayerRequest()
            {
                PlayFabId = u.PlayFabId,
                StatisticName = "Points high score",
                MaxResultsCount = 1
            },
            (_result) => {
                playerStatsCallBack(_result.Leaderboard);
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
       
    }

    public static void GetUserInventory(PlayerInventoryCallBack playerInventoryCallBack)
    {
        List<ArtisanKeycap> newInventory = new List<ArtisanKeycap>();
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest()
            {
                
            },
            (GetUserInventoryResult result) =>
            {
                playerInventoryCallBack(result.Inventory);
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }
}
