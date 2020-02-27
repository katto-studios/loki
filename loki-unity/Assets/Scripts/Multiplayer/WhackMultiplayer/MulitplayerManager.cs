using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulitplayerManager : Singleton<MulitplayerManager> {
    [Header("Inputs")]
    public InputField inIp;
    public InputField inChat;

    [Header("Console")]
    public Text console;

    public bool isServer = false;
    private MultiplayerClient m_client;
    private MultiplayerServer m_server;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
       if(m_client != null){
            console.text = m_client.ConsoleTxt;

            if (Input.GetKeyDown(KeyCode.Return)) {
                m_client.SendMessageToServer(inChat.text);
            }
        }
    }

    public void StartServerCallback() {
        isServer = true;
        m_server = new MultiplayerServer("192.168.1.136");
        //m_client = new MultiplayerClient();
    }

    public void StartClientCallback() {
        isServer = false;
        m_client = new MultiplayerClient("192.168.1.136");
    }
}
