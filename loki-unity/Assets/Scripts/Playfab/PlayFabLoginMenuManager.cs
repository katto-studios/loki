using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLoginMenuManager : Singleton<PlayFabLoginMenuManager> {
    public GameObject loginPanel;
    public GameObject registerPanel;
    public Text toggleRegisterBtn;

    private PlayFabLogin m_login;
    private PlayFabRegister m_register;
    private bool m_registering = true;

    private void Start() {
        m_register = GetComponent<PlayFabRegister>();
        m_login = GetComponent<PlayFabLogin>();

        ToggleRegister();
    }

    public void OnClickLogin() {
        m_login.Login();
    }

    public void OnClickRegister() {
        m_register.Register();
    }

    public void ToggleRegister() {
        m_registering = !m_registering;

        toggleRegisterBtn.text = m_registering ? "Login" : "Register";
        loginPanel.SetActive(!m_registering);
        registerPanel.SetActive(m_registering);
    }
}
