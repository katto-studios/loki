using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MulitplayerManager : Singleton<MulitplayerManager> {
    [Header("Inputs")]
    public InputField inRoom;

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
        }
    }

    public void StartClientCallback() {
        isServer = false;
        m_client = new MultiplayerClient();
    }
}
