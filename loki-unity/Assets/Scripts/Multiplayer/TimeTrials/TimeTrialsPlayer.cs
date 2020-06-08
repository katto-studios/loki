﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTrialsPlayer : MonoBehaviour{
    private PhotonPlayer m_player;
    public PhotonPlayer RepresentedPlayer{
        get => m_player;
        set{
            m_player = value;
            nameDisplay.SetText(m_player.NickName);
        }
    }

    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI nameDisplay;

    private void Update(){
        scoreDisplay.SetText(RepresentedPlayer.CustomProperties["Score"].ToString());
    }
}