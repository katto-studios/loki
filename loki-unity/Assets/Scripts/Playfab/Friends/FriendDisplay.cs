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
    [Header("Networking")]
    [Tooltip("Number of times per second status is refreshed")]
    public float refreshRate = 1;
    private bool m_shuttingDown = false;
    private float m_refRate;

    private cm::FriendInfo m_friendInfo;
    public void SetFriendInfo(cm::FriendInfo _fr) {
        m_friendInfo = _fr;
		nameDisplay.SetText(_fr.TitleDisplayName);
        m_refRate = 1 / refreshRate;
        StartCoroutine(RefreshStatus());
    }

    private IEnumerator RefreshStatus() {
        while (!m_shuttingDown) {
            PlayFab.PlayFabClientAPI.GetUserData(
                new cm::GetUserDataRequest() {
                    PlayFabId = m_friendInfo.FriendPlayFabId
                },
                (_result) => {
                    statusDisplay.SetText(_result.Data["PlayerState"].Value);
                },
                (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
            );
            yield return new WaitForSeconds(1 / refreshRate);
        }
    }

    public void OnJoinFriend() {
        PlayFab.PlayFabClientAPI.GetUserData(
            new cm::GetUserDataRequest() {
                PlayFabId = m_friendInfo.FriendPlayFabId
            },
            (_result) => {
                PhotonNetwork.JoinRoom(_result.Data["RoomName"].Value);
                FindObjectOfType<SceneChanger>().ChangeScene(3);
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    private void OnDestroy() {
        m_shuttingDown = true;
    }

    private void OnApplicationQuit() {
        m_shuttingDown = true;
    }
}
