using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

//for photon networking
public class NetworkingStuff : MonoBehaviour, IPunCallbacks {
    [Header("Console")]
    public Text networkConsole;
    private string m_consoleTxt = string.Empty;

    private enum PlayerStates { NONE, CREATING_ROOM, JOINING_ROOM, READY, WAITING};
    private PlayerStates m_currentPlayerState = PlayerStates.NONE;

    // Start is called before the first frame update
    void Start() {
        PrintToConsole("Player connection state: " + PhotonNetwork.connectionState);
        if (PhotonNetwork.room != null) {
            PrintToConsole("Player is in room: " + PhotonNetwork.room.Name);
        }else {
            PrintToConsole("Player is not in room");
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private void PrintToConsole(string _message) {
        Debug.Log(_message);
        m_consoleTxt += string.Format("\n{0}", _message);
        networkConsole.text = m_consoleTxt;
    }

    public void WhenCreateRoom() {
        PhotonNetwork.JoinOrCreateRoom("XD", new RoomOptions() { }, new TypedLobby { Type = LobbyType.Default});
    }

    public void WhenJoinRoom() {
		PhotonNetwork.JoinRandomRoom();
    }

    public void WhenRaiseRoomEvent() {
        //raise room event
        Dictionary<string, object> data = new Dictionary<string, object>() { { "Hello", " world" } };

        bool result = PhotonNetwork.RaiseEvent(15, data, true, new RaiseEventOptions() { ForwardToWebhook = true, });
        Debug.Log("New room event: " + result);

        //set custom property
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "CustomProperty", "It's Value" } };
        ExitGames.Client.Photon.Hashtable expectedProperties = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.room.SetCustomProperties(properties, expectedProperties, true);
        Debug.Log("New Room Properties Set");
    }

    #region Photon callbacks
    public void OnConnectedToPhoton() {

    }

    public void OnLeftRoom() {
        PrintToConsole("Left room");
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient) {

    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        PrintToConsole("Creation of room failed");
    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        PrintToConsole("Join room failed");
    }

    public void OnCreatedRoom() {
        PrintToConsole("Creation of room succeeded");
        PrintToConsole(string.Format("Currently in room: {0}", PhotonNetwork.room.Name));
    }

    public void OnJoinedLobby() {

    }

    public void OnLeftLobby() {

    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause) {
        PrintToConsole(string.Format("Failed to connect to photon: {0}", cause));
    }

    public void OnConnectionFail(DisconnectCause cause) {
        PrintToConsole(string.Format("Failed to connect: {0}", cause));
    }

    public void OnDisconnectedFromPhoton() {
        PrintToConsole("Disconnected from photon");
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
        }
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        if (PhotonNetwork.room != null) {
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
        }
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        if (PhotonNetwork.room != null) {
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
        }
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        PrintToConsole("Failed to join random room");
    }

    public void OnConnectedToMaster() {

    }

    public void OnPhotonMaxCccuReached() {
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
