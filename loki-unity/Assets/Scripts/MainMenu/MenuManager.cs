using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class MenuManager : MonoBehaviour {
    [Header("User data")]
    public TextMeshProUGUI userName;
    public RawImage profilePic;

    [Header("Currency")]
    public TextMeshProUGUI scrap;
    public TextMeshProUGUI gold;

    // Start is called before the first frame update
    void Start() {
        UpdateName();
        UpdateProfilePic();
        UpdateCurrency();
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMainMenu);
    }

    private void UpdateName() {
        int count = 0;
        while (true && count ++ < 10000) {
            userName.text = PlayfabUserInfo.Username;
            if (!userName.text.Equals("")) {
                break;
            }
        }
    }

    private void UpdateProfilePic() {
        int count = 0;
        while (true && count++ < 10000) {
            Texture txt = PlayfabUserInfo.ProfilePicture;
            if (txt) {
                profilePic.texture = txt;
                profilePic.color = Color.white;
                break;
            }
        }
    }

    private void UpdateCurrency() {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(),
            (_result) => {
                scrap.SetText(string.Format("Scrap: {0}", _result.VirtualCurrency["SM"]));
                gold.SetText(string.Format("Gold: {0}", _result.VirtualCurrency["GC"]));
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }
}
