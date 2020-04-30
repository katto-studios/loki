using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Networking;

public class PlayerInRoomDisplay : MonoBehaviour {
    [Header("Displays")]
    public TextMeshProUGUI nameDisplay;
    public RawImage profilePicture;
    public bool isHost = false;

    public void SetDisplays(PhotonPlayer _player) {
        nameDisplay.SetText(_player.NickName);
        isHost = _player.IsMasterClient;
        SetProfilePicture(_player);
    }

    private void SetProfilePicture(PhotonPlayer _player) {
        //get player playfab info
        PlayFabClientAPI.GetAccountInfo(
            new GetAccountInfoRequest() {
                TitleDisplayName = _player.NickName
            },
            (_result) => {
                //get and set profile image
                StartCoroutine(DownloadImage(_result.AccountInfo.TitleInfo.AvatarUrl));
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    private IEnumerator DownloadImage(string _url) {
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
}
