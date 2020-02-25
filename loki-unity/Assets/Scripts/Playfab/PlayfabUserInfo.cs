using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

public class PlayfabUserInfo : Singleton<PlayfabUserInfo> {
    private UserAccountInfo m_accountInfo;
    public UserAccountInfo AccountInfo { get { return m_accountInfo; } }

    // Start is called before the first frame update
    public void Start() {
        name = "PLAYFAB_ACCOUNT_INFO";
        DontDestroyOnLoad(gameObject);

        GetAccountInfoRequest req = new GetAccountInfoRequest();
        PlayFab.PlayFabClientAPI.GetAccountInfo(
            req,
            (_result) => { m_accountInfo = _result.AccountInfo; },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    public string GetUsername() {
        if(m_accountInfo != null) {
            return m_accountInfo.Username;
        }
        return string.Empty;
    }
}
