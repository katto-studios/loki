using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using cm = PlayFab.ClientModels;

public class FriendDisplay : MonoBehaviour {
	[Header("Displays")]
	public Image backgroundImage;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI highscoreDisplay;
    public TextMeshProUGUI statusDisplay;
    public Button btnJoinGame;

    private cm::FriendInfo m_friendInfo;
    public void SetFriendInfo(cm::FriendInfo _fr) {
        m_friendInfo = _fr;
		nameDisplay.SetText(_fr.TitleDisplayName);
	}

    public void OnJoinFriend() {
        PlayFab.PlayFabClientAPI.GetAccountInfo(
            new cm.GetAccountInfoRequest() {
                TitleDisplayName = m_friendInfo.TitleDisplayName
            },
            (_result) => {
                PlayFab.PlayFabClientAPI.GetUserData(
                    new cm::GetUserDataRequest() {
                        PlayFabId = _result.AccountInfo.PlayFabId
                    },
                    (__result) => {
                        PhotonNetwork.JoinRoom(__result.Data["RoomName"].Value);
                        FindObjectOfType<SceneChanger>().ChangeScene(3);
                    },
                    (__error) => { Debug.LogError(__error.GenerateErrorReport()); }
                );
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    private bool m_updating = false;
    private void Update() {
        if (!m_updating) {
            m_updating = true;
            PlayFab.PlayFabClientAPI.GetUserData(
                new cm.GetUserDataRequest(),
                (_result) => {
                    statusDisplay.SetText(_result.Data["PlayerState"].Value);
                    m_updating = false;
                },
                (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
            );
        }
    }
}
