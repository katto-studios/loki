using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

/// <summary>
/// Callbacks for photon defined using events
/// </summary>
public class PhotonCallbacks : Singleton<PhotonCallbacks>, IPunCallbacks {
    public static event Action eOnConnectedToMaster;
    public static event Action eOnConnectedToPhoton;
    public static event Action<DisconnectCause> eOnConnectionFail;
    public static event Action eOnCreatedRoom;
    public static event Action<string> eOnCustomAuthenticationFailed;
    public static event Action<Dictionary<string, object>> eOnCustomAuthenticationResponse;
    public static event Action eOnDisconnectedFromPhoton;
    public static event Action<DisconnectCause> eOnFailedToConnectToPhoton;
    public static event Action eOnJoinedLobby;
    public static event Action eOnJoinedRoom;
    public static event Action eOnLeftLobby;
    public static event Action eOnLeftRoom;
    public static event Action eOnLobbyStatisticUpdate;
    public static event Action<PhotonPlayer> eOnMasterClientSwitched;
    public static event Action<object> eOnOwnershipRequest;
    public static event Action<object> eOnOwnershipTransfered;
    public static event Action<object> eOnPhotonCreateRoomFailed;
    public static event Action<object> eOnPhotonCustomRoomPropertiesChanged;
    public static event Action<PhotonMessageInfo> eOnPhotonInstantiate;
    public static event Action<object> eOnPhotonJoinRoomFailed;
    public static event Action eOnPhotonMaxCccuReached;
    public static event Action<object> eOnPhotonPlayerActivityChanged;
    public static event Action<PhotonPlayer> eOnPhotonPlayerConnected;
    public static event Action<PhotonPlayer> eOnPhotonPlayerDisconnected;
    public static event Action<object> eOnPhotonPlayerPropertiesChanged;
    public static event Action<object> eOnPhotonRandomJoinFailed;
    public static event Action eOnReceivedRoomListUpdate;
    public static event Action eOnUpdatedFriendList;
    public static event Action<OperationResponse> eOnWebRpcResponse;

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void OnConnectedToMaster() {
        eOnConnectedToMaster?.Invoke();
    }

    public void OnConnectedToPhoton() {
        eOnConnectedToPhoton?.Invoke();
    }

    public void OnConnectionFail(DisconnectCause cause) {
        eOnConnectionFail?.Invoke(cause);
    }

    public void OnCreatedRoom() {
        eOnCreatedRoom?.Invoke();
    }

    public void OnCustomAuthenticationFailed(string debugMessage) {
        eOnCustomAuthenticationFailed?.Invoke(debugMessage);
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {
        eOnCustomAuthenticationResponse?.Invoke(data);
    }

    public void OnDisconnectedFromPhoton() {
        eOnDisconnectedFromPhoton?.Invoke();
    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause) {
        eOnFailedToConnectToPhoton?.Invoke(cause);
    }

    public void OnJoinedLobby() {
        eOnJoinedLobby?.Invoke();
    }

    public void OnJoinedRoom() {
        eOnJoinedRoom?.Invoke();
    }

    public void OnLeftLobby() {
        eOnLeftLobby?.Invoke();
    }

    public void OnLeftRoom() {
        eOnLeftRoom?.Invoke();
    }

    public void OnLobbyStatisticsUpdate() {
        eOnLobbyStatisticUpdate?.Invoke();
    }

    public void OnMasterClientSwitched(PhotonPlayer newMasterClient) {
        eOnMasterClientSwitched?.Invoke(newMasterClient);
    }

    public void OnOwnershipRequest(object[] viewAndPlayer) {
        eOnOwnershipRequest?.Invoke(viewAndPlayer);
    }

    public void OnOwnershipTransfered(object[] viewAndPlayers) {
        eOnOwnershipTransfered(viewAndPlayers);
    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        eOnPhotonCreateRoomFailed?.Invoke(codeAndMsg);
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
        eOnPhotonCustomRoomPropertiesChanged?.Invoke(propertiesThatChanged);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        eOnPhotonInstantiate?.Invoke(info);
    }

    public void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        eOnPhotonJoinRoomFailed?.Invoke(codeAndMsg);
    }

    public void OnPhotonMaxCccuReached() {
        eOnPhotonMaxCccuReached?.Invoke();
    }

    public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer) {
        eOnPhotonPlayerActivityChanged?.Invoke(otherPlayer);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        eOnPhotonPlayerConnected?.Invoke(newPlayer);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        eOnPhotonPlayerDisconnected?.Invoke(otherPlayer);
    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {
        eOnPhotonPlayerPropertiesChanged?.Invoke(playerAndUpdatedProps);
    }

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        eOnPhotonRandomJoinFailed?.Invoke(codeAndMsg);
    }

    public void OnReceivedRoomListUpdate() {
        eOnReceivedRoomListUpdate?.Invoke();
    }

    public void OnUpdatedFriendList() {
        eOnUpdatedFriendList?.Invoke();
    }

    public void OnWebRpcResponse(OperationResponse response) {
        eOnWebRpcResponse?.Invoke(response);
    }
}