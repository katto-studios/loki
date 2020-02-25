using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

public class PlayfabUserData : Singleton<PlayfabUserData> {
    private UserAccountInfo m_userInfo;
    public UserAccountInfo UserInfo { get { return m_userInfo; } }

    // Start is called before the first frame update
    void Start() {
        GetAccountInfoRequest req = new PlayFab.ClientModels.GetAccountInfoRequest();
        PlayFab.PlayFabClientAPI.GetAccountInfo(
            req,
            (_result) => { m_userInfo = _result.AccountInfo; },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    // Update is called once per frame
    void Update() {

    }
}
