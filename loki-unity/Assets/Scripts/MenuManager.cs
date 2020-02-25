using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour {
    public TextMeshProUGUI userName;
    private string m_username;
    public string Username { set { m_username = value; } }

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
