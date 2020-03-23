using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PendingDisplay : MonoBehaviour {
    [Header("Displays")]
    public TextMeshProUGUI nameDisplay;

    public string RequesteeName { get; private set; }
    
    public void Initalise(string _requestee) {
        RequesteeName = _requestee;
        nameDisplay.SetText(_requestee);
    }

    public void Accept() {
        PlayfabUserInfo.AcceptFriend(RequesteeName);
    }

    public void Deny() {
        PlayfabUserInfo.DenyFriend(RequesteeName);
    }
}
