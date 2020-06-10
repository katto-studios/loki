using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;

public class TimeTrialsMultiplayerRenderer : Singleton<TimeTrialsMultiplayerRenderer>{
    public Transform parent;
    public GameObject playerDisplayPfb;
    public TextMeshProUGUI seedDisplay;

    public GameObject NetworkKeyboardPrefab;
    public GameObject NetworkKeyboardPosition;
    
    public List<MultiplayerKeyboard> m_networkKeyboards = new List<MultiplayerKeyboard>();
    private float m_timer = 0;
    private int currentNetworkKeyboardIndex;

    private void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > 5.0f)
        {
            m_networkKeyboards[currentNetworkKeyboardIndex].gameObject.SetActive(false);

            currentNetworkKeyboardIndex++;
            if (currentNetworkKeyboardIndex >= m_networkKeyboards.Count)
            {
                currentNetworkKeyboardIndex = 0;
            }

            m_networkKeyboards[currentNetworkKeyboardIndex].gameObject.SetActive(true);

            m_timer = 0;
        }
    }

    private void Start(){
        foreach (PhotonPlayer p in PhotonNetwork.playerList){
            Instantiate(playerDisplayPfb, parent).GetComponent<TimeTrialsPlayer>().RepresentedPlayer = p;
            PlayerDataCallBack pdcb = GetAccountInfoCallBack;
            PlayerNotFoundCallBack pnfcb = gPlayerNotFoundCallBack;
            PlayFabPlayerData.SetTargetPlayer(p.NickName, pdcb, pnfcb);
        }

        seedDisplay.SetText(((int) PhotonNetwork.room.CustomProperties["RandomSeed"]).ToString());
    }

    public void GetAccountInfoCallBack(UserAccountInfo u)
    {
        PlayerInventoryCallBack picb = GetInventoryCallback;
        PlayFabPlayerData.GetUserInventory(u, picb);
    }

    public void GetInventoryCallback(List<ItemInstance> items, UserAccountInfo u)
    {
        GameObject newKeyboard = Instantiate(NetworkKeyboardPrefab, NetworkKeyboardPosition.transform);
        MultiplayerKeyboard mpk = newKeyboard.GetComponent<MultiplayerKeyboard>();
        mpk.Init(items, u);
        m_networkKeyboards.Add(mpk);
        if (m_networkKeyboards.Count > 1)
        {
            newKeyboard.SetActive(false);
        }
    }

    public void gPlayerNotFoundCallBack()
    {

    }
}