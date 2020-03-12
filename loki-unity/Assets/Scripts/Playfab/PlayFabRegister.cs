using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabRegister : MonoBehaviour {
    public InputField inEmail, inPass, inName;

    private string m_userEmail;
    public string UserEmail { set { m_userEmail = value; } }
    private string m_userPassword;
    public string UserPassword { set { m_userPassword = value; } }
    private string m_username;
    public string Username { set { m_username = value; } }

    private void OnRegisterSuccess(RegisterPlayFabUserResult _result) {
        Debug.Log("User registered!");
        PlayerPrefs.SetString("userEmail", m_userEmail);
        PlayerPrefs.SetString("userPassword", m_userPassword);
        PlayfabUserInfo.Initalise();
        GetProse.Instance.CheckForUpdate();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    private void OnRegisterFailure(PlayFabError _error) {
        Debug.LogError(_error.GenerateErrorReport());
    }

    public void Register() {
        m_userEmail = inEmail.text;
        m_userPassword = inPass.text;
        m_username = inName.text;

        var registerReq = new RegisterPlayFabUserRequest { Email = m_userEmail, Password = m_userPassword, Username = m_username };
        PlayFabClientAPI.RegisterPlayFabUser(registerReq, OnRegisterSuccess, OnRegisterFailure);
    }
}
