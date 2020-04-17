using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInRoomDisplay : MonoBehaviour {
    [Header("Displays")]
    public TextMeshProUGUI nameDisplay;
    public bool isHost = false;

    public void SetDisplays(PhotonPlayer _player) {
        nameDisplay.SetText(_player.NickName);
        isHost = _player.IsMasterClient;
    }
}
