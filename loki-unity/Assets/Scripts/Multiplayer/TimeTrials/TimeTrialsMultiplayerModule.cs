using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class TimeTrialsMultiplayerModule : Singleton<TimeTrialsMultiplayerModule>{
    private int m_score = 0;
    
    private void Start(){
        TimeTrialsGameManager.Instance.eOnScoreUpdate += OnScoreUpdate;
        
        TimeTrialGameStateManager.Instance.StartMultiplayer();

        PhotonCallbacks.eOnPhotonPlayerDisconnected += OnPlayerLeft;
        
        Random.InitState((int)PhotonNetwork.room.CustomProperties["RandomSeed"]);
    }

    private void OnScoreUpdate(int _newScore){
        m_score += _newScore;
        PhotonNetwork.player.SetCustomProperties(new Hashtable{
            {"Score", m_score} 
        });
    }

    private void OnPlayerLeft(PhotonPlayer _otherPlayer){
        PopupManager.Instance.ShowPopUp($"{_otherPlayer.NickName} left!");
        if (PhotonNetwork.otherPlayers.Length <= 0){
            TimeTrialGameStateManager.Instance.StopGame();
        }
    }
}