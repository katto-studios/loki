using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public TMPro.TextMeshProUGUI userName;
    private string m_username;

    // Start is called before the first frame update
    void Start() {
        PlayFab.ClientModels.GetAccountInfoRequest req = new PlayFab.ClientModels.GetAccountInfoRequest();
        PlayFab.PlayFabClientAPI.GetAccountInfo(
            req,
            (_result) => { m_username = _result.AccountInfo.Username; },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

        userName.text = m_username;
    }

    // Update is called once per frame
    void Update() {
        userName.text = m_username;
    }
}
