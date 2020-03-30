using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;
using UnityEngine.UI;

public class RoomInfoDisplay : MonoBehaviour {
    [Header("Fields")]
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI roomNumberOfPeople;
    public Button joinRoom;
    [Header("Network")]
    [Tooltip("Refreshes every (refreshTime) seconds")]
    public float refreshTime = 1;

    private RoomInfo m_roomInfo;
    private volatile bool m_shuttingDown = false;
    public void SetInfo(RoomInfo _roomInfo) {
        roomName.SetText(_roomInfo.Name);
        roomNumberOfPeople.SetText(string.Format("{0}/{1}", _roomInfo.PlayerCount.ToString(), _roomInfo.MaxPlayers.ToString()));
        m_roomInfo = _roomInfo;
        StartCoroutine(UpdateRoomNumberOfPeople());
    }

    private IEnumerator UpdateRoomNumberOfPeople() {
        while (!m_shuttingDown) {
            roomNumberOfPeople.SetText(string.Format("{0}/{1}", m_roomInfo.PlayerCount.ToString(), m_roomInfo.MaxPlayers.ToString()));
            yield return new WaitForSeconds(refreshTime);
        }
    }

    public void WhenJoinRoom() {
        joinRoom.enabled = false;
        PhotonNetwork.JoinRoom(m_roomInfo.Name);
    }

    private void OnDestroy() {
        m_shuttingDown = true;
    }

    private void OnApplicationQuit() {
        m_shuttingDown = true;
    }
}
