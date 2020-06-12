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

        foreach (PhotonPlayer p in PhotonNetwork.otherPlayers)
        {
            if (p.NickName == m_networkKeyboards[currentNetworkKeyboardIndex].Username)
            {
                m_networkKeyboards[currentNetworkKeyboardIndex].UpdateScore((int)p.CustomProperties["Score"]);
            }
        }
    }

    private void Start(){
        foreach (PhotonPlayer p in PhotonNetwork.playerList){
            PlayerDataCallBack pdcb = GetAccountInfoCallBack;
            PlayerNotFoundCallBack pnfcb = gPlayerNotFoundCallBack;
            PlayFabPlayerData.SetTargetPlayer(p.NickName, pdcb, pnfcb);
        }
        
        TimeTrialGameStateManager.Instance.eOnChangedState += ChangeState;

        seedDisplay.SetText(((int) PhotonNetwork.room.CustomProperties["RandomSeed"]).ToString());
    }

    private void ChangeState(TimeTrialGameStateManager.GameStates _newState){
        if (_newState == TimeTrialGameStateManager.GameStates.Analytics){
            //clear parent
            foreach (Transform child in parent){
                Destroy(child.gameObject);
            }
            
            PhotonPlayer[] players = PlayersSorted();
            for (int count = 0; count <= players.Length - 1; count++){
                Instantiate(playerDisplayPfb, parent).GetComponent<TimeTrialsPlayer>().SetInfo(players[count], count + 1);
            }
        }
    }

    public void GetAccountInfoCallBack(UserAccountInfo u)
    {
        if (u.Username != PlayfabUserInfo.AccountInfo.Username)//Check if not the player
        {
            PlayerInventoryCallBack picb = GetInventoryCallback;
            PlayFabPlayerData.GetUserInventory(u, picb);
        }
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
    
    public PhotonPlayer[] PlayersSorted(){
        PhotonPlayer[] sortMe = PhotonNetwork.playerList;
        MergeSort(sortMe, 0, sortMe.Length - 1);
        return sortMe;
    }

    private static void MergeSort(PhotonPlayer[] _sortMe, int _left, int _right){
        if (_left >= _right) return;
        int partition = (_left + _right) / 2;
            
        MergeSort(_sortMe, _left, partition);
        MergeSort(_sortMe, partition + 1, _right);
            
        Merge(_sortMe, _left, partition, _right);
    }

    private static void Merge(PhotonPlayer[] _in, int _left, int _middle, int _right){
        int i, j, k;
        int n1 = _middle - _left + 1;
        int n2 = _right - _middle;

        PhotonPlayer[] tLeft = new PhotonPlayer[n1];
        PhotonPlayer[] tRight = new PhotonPlayer[n2];
        for (i = 0; i < n1; i++){
            tLeft[i] = _in[_left + i];
        }

        for (j = 0; j < n2; j++){
            tRight[j] = _in[_middle + 1 + j];
        }

        i = 0;
        j = 0;
        k = _left;

        while (i < n1 && j < n2){
            _in[k++] = (int) tLeft[i].CustomProperties["Score"] > (int) tRight[j].CustomProperties["Score"]
                ? tLeft[i++]
                : tRight[j++];
        }

        while (i < n1) _in[k++] = tLeft[i++];
        while (j < n2) _in[k++] = tRight[j++];
    }
}