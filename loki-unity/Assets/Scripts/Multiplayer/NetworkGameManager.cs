using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using PlayFab.ClientModels;
using System;

public class NetworkGameManager : TypeGameManager, IPunCallbacks {
    //[Header("Regular stuff")]
    public Button btnStartNext;
    [Header("Networking")]
    public int maxRounds = 3;
    private HashSet<PhotonPlayer> m_opponents = new HashSet<PhotonPlayer>();
    private List<MultiplayerKeyboard> m_networkKeyboards = new List<MultiplayerKeyboard>();
    private int currentNetworkKeyboardIndex;
    public GameObject NetworkKeyboardPrefab;
    public GameObject NetworkKeyboardPosition;
    private int m_currentRound;
    private bool changed = false;

    private float m_timer;

    public override void Start() {
        foreach (PhotonPlayer other in PhotonNetwork.otherPlayers) {
            m_opponents.Add(other);
            PlayerDataCallBack pdcb = GetAccountInfoCallBack;
            PlayerNotFoundCallBack pnfcb = gPlayerNotFoundCallBack;
            PlayFabPlayerData.SetTargetPlayer(other.NickName, pdcb, pnfcb);
        }
        PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMatch);

        comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        //check if it has the same paragaph as the master client
        if (!PhotonNetwork.isMasterClient) {
            string otherProse = m_opponents.First(x => { return x.IsMasterClient; }).CustomProperties["ProseToWrite"] as string;

            if (!wordsString.Equals(otherProse)) {
                Debug.LogError("Client and master mismatch!");
                wordsString = otherProse;
            }
        }
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();

        m_currentRound = (int)PhotonNetwork.room.CustomProperties["Round number"];
        score = (int)PhotonNetwork.player.CustomProperties["Score"];

        //PhotonNetwork.room.SetRoomProperty("ReadyToStart", false);
        if (PhotonNetwork.isMasterClient) {
            PhotonNetwork.room.SetCustomProperties(new Hashtable() {
                { "Paragraph", wordsString },
                { "Round number", m_currentRound },
                { "ReadyToStart", false }
            });
        }

        gameState = GameState.Ready;
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
        if(m_networkKeyboards.Count > 1)
        {
            newKeyboard.SetActive(false);
        }
    }

    public void gPlayerNotFoundCallBack()
    {

    }

    public override void Update() {
        base.Update();

        m_timer += Time.deltaTime;
        if(m_timer > 5.0f)
        {
            m_networkKeyboards[currentNetworkKeyboardIndex].gameObject.SetActive(false);

            currentNetworkKeyboardIndex++;
            if(currentNetworkKeyboardIndex >= m_networkKeyboards.Count)
            {
                currentNetworkKeyboardIndex = 0;
            }

            m_networkKeyboards[currentNetworkKeyboardIndex].gameObject.SetActive(true);

            m_timer = 0;
        }

        //start countdown
        if (gameState == GameState.Ready) {
            if (m_opponents.All(x => {
                return (PlayfabUserInfo.UserState)x.CustomProperties["UserState"] == PlayfabUserInfo.UserState.InMatch;
            }))
            {
                gameState = GameState.Countdown;
                readyGO.SetActive(false);
                countDownText.gameObject.SetActive(true);
                StartCoroutine(CountDown(5));
            }
        }

        if (PlayfabUserInfo.CurrentUserState == PlayfabUserInfo.UserState.WaitingForNextRound) {
            //check if opponents are ready
            if (m_opponents.All(x => {
                return (PlayfabUserInfo.UserState)x.CustomProperties["UserState"] == PlayfabUserInfo.UserState.WaitingForNextRound;
            }) && m_currentRound < maxRounds) {
                if (PhotonNetwork.isMasterClient) {
                    if (!changed) {
                        SetProse();
                        StartNextRound();
                        changed = true;
                    }
                } else {
                    if ((bool)PhotonNetwork.room.CustomProperties["ReadyToStart"]) {
                        StartNextRound();
                    }
                }
            }
        }

        //update own hashtable
        PhotonNetwork.player.SetCustomProperties(new Hashtable() {
            { "Score", score },
            { "Progress", GetGameProgress() },
            { "UserState", PlayfabUserInfo.CurrentUserState }
        });

        //EDITOR ONLY
        if (Input.GetKeyDown(KeyCode.P) && Application.isEditor) {
            Complete();
        }
    }

    public void LeaveGame() {
        //assume last place
        int position = m_opponents.Count + 1;
        foreach(PhotonPlayer player in m_opponents) {
            if(float.Parse(player.CustomProperties["Score"].ToString()) < score) {
                position++;
            }
        }

        Debug.Log(string.Format("Player came in {0} place", position.ToString()));
        //update mmr
        //PlayfabUserInfo.UpdatePlayerMmr(float.Parse(m_opponent.CustomProperties["Score"].ToString()) < score ? 25 : -25);
        PhotonNetwork.LeaveRoom();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    public void WhenStartNextRound() {
        gameState = GameState.Ready;
        //set state
        PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.WaitingForNextRound);

        //disable button
        btnStartNext.gameObject.SetActive(false);
    }

    private void SetProse() {
        //determine prose
        Paragraph prose = GetProse.Instance.GetRandomProse();
        string currentProses = (string)PhotonNetwork.room.CustomProperties["ProsesUsed"];
        GameplayConsole.Log(currentProses);
        while (currentProses.Contains(prose.Prose)) {
            prose = GetProse.Instance.GetRandomProse();
        }
        currentProses += prose.Prose;

        PhotonNetwork.room.SetCustomProperties(new Hashtable() {
            { "Paragraph", prose.Prose },
            { "Round number", m_currentRound },
            { "ReadyToStart", true },
            { "ProsesUsed", currentProses }
        });

        //set master client prose to write
        if (PhotonNetwork.isMasterClient) {
            PhotonNetwork.player.SetCustomProperties(new Hashtable() {
                    { "Score", score },
                    {"PlayerState", PlayfabUserInfo.UserState.InMatch },
                    { "ProseToWrite", prose.Prose }
            });
        }
    }

    private void StartNextRound() {
        FindObjectOfType<SceneChanger>().ChangeScene(5);
    }

    protected override void Complete() {
        base.Complete();

        if (++m_currentRound >= maxRounds) {
            //actually finish
            btnStartNext.GetComponentInChildren<TextMeshProUGUI>().SetText("Leave game");
            btnStartNext.onClick.RemoveAllListeners();
            btnStartNext.onClick.AddListener(LeaveGame);
        }
    }

    public override void QuitGame() {
        PhotonNetwork.LeaveRoom();
        base.QuitGame();
    }

    #region PhotonCallbacks
    public void OnConnectedToPhoton() {

    }

    public void OnLeftRoom() {

    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient) {

    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg) {

    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg) {

    }

    public void OnCreatedRoom() {

    }

    public void OnJoinedLobby() {

    }

    public void OnLeftLobby() {

    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause) {

    }

    public void OnConnectionFail(DisconnectCause cause) {

    }

    public void OnDisconnectedFromPhoton() {

    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {

    }

    public void OnReceivedRoomListUpdate() {

    }

    public void OnJoinedRoom() {

    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {

    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        PopupManager.Instance.ShowPopUp(string.Format("{0} disconnected!", otherPlayer.NickName), 3);
        if(PhotonNetwork.room.PlayerCount == 1) {
            m_currentRound = maxRounds;
            Complete();
        }
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg) {

    }

    public void OnConnectedToMaster() {

    }

    public void OnPhotonMaxCccuReached() {

    }

    public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged) {

    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {

    }

    public void OnUpdatedFriendList() {

    }

    public void OnCustomAuthenticationFailed(string debugMessage) {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {

    }

    public void OnWebRpcResponse(OperationResponse response) {

    }

    public void OnOwnershipRequest(object[] viewAndPlayer) {

    }

    public void OnLobbyStatisticsUpdate() {

    }

    public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer) {

    }

    public void OnOwnershipTransfered(object[] viewAndPlayers) {

    } 
    #endregion
}
