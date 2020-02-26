using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

public static class PlayfabUserInfo{
    private static UserAccountInfo m_accountInfo;
    public static UserAccountInfo AccountInfo { get { return m_accountInfo; } }

    public static void Initalise() {
        GetAccountInfoRequest req = new GetAccountInfoRequest();
        PlayFab.PlayFabClientAPI.GetAccountInfo(
            req,
            (_result) => { m_accountInfo = _result.AccountInfo; },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public static string GetUsername() {
        if(m_accountInfo != null) {
            return m_accountInfo.Username;
        }
        return string.Empty;
    }
}
