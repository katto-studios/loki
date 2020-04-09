using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class GameplayConsole : Singleton<GameplayConsole> {
    [Header("Fields")]
    public Text outText;
    public InputField inConsole;

    private bool m_isActive = false;
    private CanvasGroup m_cg;
    private string m_consoleText = "Console:";

    private void Start() {
        DontDestroyOnLoad(gameObject);
        m_cg = GetComponent<CanvasGroup>();
    }

    public void Update() {
        if (Input.GetKey(KeyCode.F1)) {
            if (Input.GetKeyDown(KeyCode.LeftAlt)) {
                ToggleActive();
            }
        }
    }

    public void SetActive(bool _state) {
        if(_state == m_isActive) {
            return;
        }

        ToggleActive();
    }
    
    public void ToggleActive() {
        m_isActive = !m_isActive;
        m_cg.alpha = m_isActive ? 1 : 0;
        m_cg.blocksRaycasts = m_isActive;
    }

    public static void Log(string _logMessage) {
        Debug.Log(_logMessage);
        Instance.m_consoleText += string.Format("\n{0}", _logMessage);
        Instance.outText.text = Instance.m_consoleText;
    }

    public void ParseEvent() {
        switch (inConsole.text) {
            case "Connect":
                break;
            default:
                Log("Command not found");
                break;
        }
    }

    private void OnApplicationQuit() {
        PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.Offline);
    }
}
