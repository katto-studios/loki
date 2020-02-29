using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//for photon networking
public class NetworkingStuff : MonoBehaviour, IPunCallbacks {
    [Header("Console")]
    public Text networkConsole;
    private string m_consoleTxt = string.Empty;

    [Header("Gameplay")]
    public GameObject goToPlayGameScene;

    public enum PlayerStates { NONE, CREATING_ROOM, JOINING_ROOM, READY, WAITING, PLAYING };
    public PlayerStates m_currentPlayerState = PlayerStates.NONE;
    private PhotonPlayer m_opponent;

    // Start is called before the first frame update
    void Start() {
        DontDestroyOnLoad(gameObject);
        PrintToConsole("Player connection state: " + PhotonNetwork.connectionState);
    }

    // Update is called once per frame
    void Update() {
        goToPlayGameScene.SetActive(m_currentPlayerState == PlayerStates.READY);
    }

    public void WhenStartGame() {
        Paragraph para = GetProse.Instance.GetRandomProse();
        m_currentPlayerState = PlayerStates.PLAYING;
        m_opponent.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
            { "Score", 0 }
        });

        PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
            { "Paragraph", para }
        });

        //change scene
        GetComponent<SceneChanger>().ChangeScene(5);
    }

    private void PrintToConsole(string _message) {
        try {
            Debug.Log(_message);
            m_consoleTxt += string.Format("\n{0}", _message);
            networkConsole.text = m_consoleTxt;
        } catch (Exception) { }
    }

    public void WhenCreateRoom() {
        m_currentPlayerState = PlayerStates.CREATING_ROOM;
        PhotonNetwork.JoinOrCreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, new TypedLobby { Type = LobbyType.Default });
    }

    public void WhenJoinRoom() {
        m_currentPlayerState = PlayerStates.JOINING_ROOM;
        PhotonNetwork.JoinRandomRoom();
    }

    public void WhenJoinRoom(string _room) {
        m_currentPlayerState = PlayerStates.JOINING_ROOM;
        PhotonNetwork.JoinOrCreateRoom(_room, new RoomOptions() { MaxPlayers = 2 }, new TypedLobby { Type = LobbyType.Default });
    }

    #region Photon callbacks
    public void OnConnectedToPhoton() {

    }

    public void OnLeftRoom() {
        PrintToConsole("Left room");
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient) {

    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        PrintToConsole("Creation of room failed");
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        PrintToConsole("Join room failed");
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnCreatedRoom() {
        PrintToConsole("Creation of room succeeded");
        PrintToConsole(string.Format("Currently in room: {0}", PhotonNetwork.room.Name));
        m_currentPlayerState = PlayerStates.WAITING;
    }

    public void OnJoinedLobby() {

    }

    public void OnLeftLobby() {

    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause) {
        PrintToConsole(string.Format("Failed to connect to photon: {0}", cause));
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnConnectionFail(DisconnectCause cause) {
        PrintToConsole(string.Format("Failed to connect: {0}", cause));
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnDisconnectedFromPhoton() {
        PrintToConsole("Disconnected from photon");
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {

    }

    public void OnReceivedRoomListUpdate() {
        PrintToConsole("Room list updated");
    }

    public void OnJoinedRoom() {
        if (PhotonNetwork.room != null) {
            PrintToConsole("Player is in room: " + PhotonNetwork.room.Name);
            switch (PhotonNetwork.room.PlayerCount) {
                case 1:
                    m_currentPlayerState = PlayerStates.WAITING;
                    break;
                case 2:
                    m_currentPlayerState = PlayerStates.READY;
                    break;
            }
            PrintToConsole("Current player state: " + m_currentPlayerState);
        } else {
            PrintToConsole("Failed to create/join room");
            m_currentPlayerState = PlayerStates.NONE;
        }
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        if (PhotonNetwork.room != null) {
            m_opponent = newPlayer;
            PrintToConsole("Player joined room: " + newPlayer.NickName);
            switch (PhotonNetwork.room.PlayerCount) {
                case 1:
                    m_currentPlayerState = PlayerStates.WAITING;
                    break;
                case 2:
                    m_currentPlayerState = PlayerStates.READY;
                    break;
            }
            PrintToConsole("Current player state: " + m_currentPlayerState);
        } else {
            PrintToConsole("Failed to create/join room");
            m_currentPlayerState = PlayerStates.NONE;
        }
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        if (PhotonNetwork.room != null) {
            m_opponent = null;
            PrintToConsole("Player left room: " + otherPlayer.NickName);
            switch (PhotonNetwork.room.PlayerCount) {
                case 1:
                    m_currentPlayerState = PlayerStates.WAITING;
                    break;
                case 2:
                    m_currentPlayerState = PlayerStates.READY;
                    break;
            }
            PrintToConsole("Current player state: " + m_currentPlayerState);
        } else {
            PrintToConsole("Failed to create/join room");
            m_currentPlayerState = PlayerStates.NONE;
        }
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        PrintToConsole("Failed to join random room");
        m_currentPlayerState = PlayerStates.NONE;
    }

    public void OnConnectedToMaster() {

    }

    public void OnPhotonMaxCccuReached() {
        m_currentPlayerState = PlayerStates.NONE;
        Debug.LogError("PHOTON MAX PLAYERS REACHED!!");
        Application.Quit();
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {

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
