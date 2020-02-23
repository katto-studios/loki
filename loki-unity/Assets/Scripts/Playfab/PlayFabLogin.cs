using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour {
    public InputField inEmail, inPass, inName;

    private string m_userEmail;
    public string UserEmail { set { m_userEmail = value; } }
    private string m_userPassword;
    public string UserPassword { set { m_userPassword = value; } }
    private string m_username;
    public string Username { set { m_username = value; } }

    public void Start() {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) {
            PlayFabSettings.TitleId = "146EC"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        if (PlayerPrefs.HasKey("userEmail")) {
            inEmail.text = PlayerPrefs.GetString("userEmail");
            if (PlayerPrefs.HasKey("userPassword")) {
                inPass.text = PlayerPrefs.GetString("userPassword");
            }
        }
        //var request = new LoginWithEmailAddressRequest { Email = m_userEmail, Password = m_userPassword };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result) {
        Debug.Log("User logged in");
        PlayerPrefs.SetString("userEmail", m_userEmail);
        PlayerPrefs.SetString("userPassword", m_userPassword);
        GetProse.Instance.CheckForUpdate();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    private void OnLoginFailure(PlayFabError error) {
        Debug.Log(error.GenerateErrorReport());

        var registerReq = new RegisterPlayFabUserRequest { Email = m_userEmail, Password = m_userPassword, Username = m_username };
        PlayFabClientAPI.RegisterPlayFabUser(registerReq, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult _result) {
        Debug.Log("User registered!");
        PlayerPrefs.SetString("userEmail", m_userEmail);
        PlayerPrefs.SetString("userPassword", m_userPassword);
        GetProse.Instance.CheckForUpdate();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    private void OnRegisterFailure(PlayFabError _error) {
        Debug.LogError(_error.GenerateErrorReport());
    }

    public void OnClickLogin() {
        m_userEmail = inEmail.text;
        m_username = inName.text;
        m_userPassword = inPass.text;

        var request = new LoginWithEmailAddressRequest { Email = m_userEmail, Password = m_userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
}
