using System;
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
    public TextMeshProUGUI positionDisplay;

    // private void Update(){
    //     scoreDisplay.SetText(RepresentedPlayer.CustomProperties["Score"].ToString());
    // }

    public void SetInfo(PhotonPlayer _player, int _position){
        nameDisplay.SetText(_player.NickName);
        scoreDisplay.SetText(_player.CustomProperties["Score"].ToString());
        positionDisplay.SetText(_position.ToString());
    }
}
