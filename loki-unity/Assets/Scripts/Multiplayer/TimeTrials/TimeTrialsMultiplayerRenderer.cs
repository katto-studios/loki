﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTrialsMultiplayerRenderer : Singleton<TimeTrialsMultiplayerRenderer>{
    public Transform parent;
    public GameObject playerDisplayPfb;
    public TextMeshProUGUI seedDisplay;

    private void Start(){
        foreach (PhotonPlayer p in PhotonNetwork.playerList){
            Instantiate(playerDisplayPfb, parent).GetComponent<TimeTrialsPlayer>().RepresentedPlayer = p;
        }

        seedDisplay.SetText(((int) PhotonNetwork.room.CustomProperties["RandomSeed"]).ToString());
    }
}