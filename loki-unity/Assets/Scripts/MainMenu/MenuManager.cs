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
        StartCoroutine(UpdateName());
        StartCoroutine(UpdateProfilePic());
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMainMenu);
    }

    private IEnumerator UpdateName() {
        while (true) {
            userName.text = PlayfabUserInfo.Username;
            if (!userName.text.Equals("")) {
                break;
            }
            yield return null;
        }
    }

    private IEnumerator UpdateProfilePic() {
        while (true) {
            Texture txt = PlayfabUserInfo.ProfilePicture;
            if (txt) {
                profilePic.texture = txt;
                profilePic.color = Color.white;
                break;
            }
            yield return null;
        }
    }

}
