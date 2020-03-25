using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System;

public class RoomInfoDisplay : MonoBehaviour {
    [Header("Fields")]
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI roomNumberOfPeople;

    private RoomInfo m_roomInfo;
    public void SetInfo(RoomInfo _roomInfo) {
        roomName.SetText(_roomInfo.Name);
        m_roomInfo = _roomInfo;
    }

    private IEnumerator UpdateRoomNumberOfPeople() {
        yield return null;
    }
}
