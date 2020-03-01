using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkGameManager : TypeGameManager {
    public override void Start() {
        comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();
    }

    public override void Update() {
        base.Update();
    }
}
