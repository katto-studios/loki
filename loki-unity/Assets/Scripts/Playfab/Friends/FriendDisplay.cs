using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using cm = PlayFab.ClientModels;

public class FriendDisplay : MonoBehaviour {
	[Header("Displays")]
	public Image backgroundImage;
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI highscoreDisplay;
    public TextMeshProUGUI statusDisplay;
    public Button btnJoinGame;
    public RawImage profilePicture;

    [Header("Networking")]
    [Tooltip("Number of times per second status is refreshed")]
    public float refreshRate = 1;

    private volatile bool m_shuttingDown = false;
    private float m_refRate;
    private cm::FriendInfo m_friendInfo;

    public void SetFriendInfo(cm::FriendInfo _fr) {
        m_friendInfo = _fr;
		nameDisplay.SetText(_fr.TitleDisplayName);
        m_refRate = 1 / refreshRate;
        btnJoinGame.interactable = false;
        //set profile picture
        SetImage();
        StartCoroutine(RefreshStatus());
    }

    private void SetImage() {
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest() {
                Username = m_friendInfo.Username
            },
            (_result) => {
                string url = _result.AccountInfo.TitleInfo.AvatarUrl;
                if(!string.IsNullOrEmpty(url)) StartCoroutine(FetchImage(url));
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    private IEnumerator FetchImage(string _url) {
        using (UnityWebRequest webReq = UnityWebRequestTexture.GetTexture(_url)) {
            yield return webReq.SendWebRequest();
            if (webReq.isNetworkError) {
                Debug.LogError("Network error: " + webReq.error);
            } else {
                profilePicture.texture = DownloadHandlerTexture.GetContent(webReq);
                profilePicture.color = Color.white;
            }
        }
    }

    private IEnumerator RefreshStatus() {
        while (!m_shuttingDown) {
            PlayFabClientAPI.GetUserData(
                new cm::GetUserDataRequest() {
                    PlayFabId = m_friendInfo.FriendPlayFabId
                },
                (_result) => {
                    string value = _result.Data["PlayerState"].Value;
                    btnJoinGame.interactable = value.Equals("Waiting for an opponent");
                    statusDisplay.SetText(value);
                },
                (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
            );
            yield return new WaitForSeconds(1 / refreshRate);
        }
    }

    public void OnJoinFriend() {
        PlayFabClientAPI.GetUserData(
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

    public static string UserStateToString(PlayfabUserInfo.UserState _state) {
        switch (_state) {
            case PlayfabUserInfo.UserState.InMainMenu:
                return "Main menu";
            case PlayfabUserInfo.UserState.ReadyToPractice:
                return "Practicing";
            case PlayfabUserInfo.UserState.Practicing:
                return "Practicing";
            case PlayfabUserInfo.UserState.ViewingLeaderBoards:
                return "Viewing leaderboards";
            case PlayfabUserInfo.UserState.Offline:
                return "Offline";
            case PlayfabUserInfo.UserState.InLobby:
                return "In lobby";
            case PlayfabUserInfo.UserState.InQueue:
                return "Waiting for an opponent";
            case PlayfabUserInfo.UserState.ReadyToType:
                return "In a match";
            case PlayfabUserInfo.UserState.WaitingForOpponent:
                return "Waiting for an opponent";
            case PlayfabUserInfo.UserState.InMatch:
                return "In a match";
            case PlayfabUserInfo.UserState.WaitingForNextRound:
                return "In a match";
            default:
                break;
        }

        return null;
    }
}
