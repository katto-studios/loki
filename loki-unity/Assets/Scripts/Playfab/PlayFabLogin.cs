using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour {
    [Header("Inputs")]
    public InputField inEmail;
    public InputField inPass;
    public Toggle rememberMe;

    private string m_userEmail;
    public string UserEmail { set { m_userEmail = value; } }
    private string m_userPassword;
    public string UserPassword { set { m_userPassword = value; } }

    private bool m_loggingIn = false;

    public void Start() {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) {
            PlayFabSettings.TitleId = "146EC";
        }
        if (PlayerPrefs.HasKey("userEmail")) {
            inEmail.text = PlayerPrefs.GetString("userEmail");
            if (PlayerPrefs.HasKey("userPassword")) {
                inPass.text = PlayerPrefs.GetString("userPassword");
            }
        }
        //auto login
        //var request = new LoginWithEmailAddressRequest { Email = m_userEmail, Password = m_userPassword };
        //PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result) {
        Debug.Log("User logged in to playfab");
        if (rememberMe.isOn) {
            PlayerPrefs.SetString("userEmail", m_userEmail);
            PlayerPrefs.SetString("userPassword", m_userPassword);
        } else {
            PlayerPrefs.DeleteKey("userEmail");
            PlayerPrefs.DeleteKey("userPassword");
        }

        //init photon
        PlayFabClientAPI.GetPhotonAuthenticationToken(
            new GetPhotonAuthenticationTokenRequest() { PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppID },
            (_result) => {
                AuthenticationValues customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom, UserId = m_userEmail };
                customAuth.AddAuthParameter("username", result.PlayFabId);
                customAuth.AddAuthParameter("token", _result.PhotonCustomAuthenticationToken);
                PhotonNetwork.AuthValues = customAuth;
                PhotonNetwork.ConnectUsingSettings("0.0.1");
                //PhotonNetwork.ConnectToMaster("13.250.188.229", 42069, "99f28bf7-f000-4d28-bd9d-29843647aacd", "0.0.1");
                Debug.Log("User logged in to photon");
            },
            OnLoginFailure
        );

        PlayfabUserInfo.Initalise();
        GetProse.Instance.CheckForUpdate();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    private void OnLoginFailure(PlayFabError error) {
        Debug.Log(error.GenerateErrorReport());
        PopupManager.Instance.ShowPopUp("Invalid Credentials, Register Instead?");
        m_loggingIn = false;
    }

    public void Login() {
        if (!m_loggingIn) {
            m_loggingIn = true;
            m_userEmail = inEmail.text;
            m_userPassword = inPass.text;

            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest { Email = m_userEmail, Password = m_userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
    }
}
