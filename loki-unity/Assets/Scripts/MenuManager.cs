using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;

public class MenuManager : MonoBehaviour {
    public TextMeshProUGUI userName;
    public TextMeshProUGUI scrapMetalDisplay;
    public TextMeshProUGUI goldDisplay;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(UpdateName());
        GetCurrency();
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMainMenu);
    }

    private IEnumerator UpdateName() {
        while (true) {
            userName.text = PlayfabUserInfo.GetUsername();
            if (!userName.text.Equals("")) {
                break;
            }
            yield return null;
        }
    }

    private void GetCurrency() {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(),
            (_result) => {
                scrapMetalDisplay.SetText(string.Format("Scrap: {0}", _result.VirtualCurrency["SM"]));
                goldDisplay.SetText(string.Format("Gold: {0}", _result.VirtualCurrency["GC"]));
            },
            (_error) => { Debug.LogError(_error); }
        );
    }
}
