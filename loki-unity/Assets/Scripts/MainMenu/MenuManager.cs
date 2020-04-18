using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public TextMeshProUGUI userName;
    public RawImage profilePic;

    // Start is called before the first frame update
    void Start() {
        UpdateName();
        UpdateProfilePic();
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

}
