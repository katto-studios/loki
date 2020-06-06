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
        
        Random.InitState((int)PhotonNetwork.room.CustomProperties["RandomSeed"]);
    }

    private void OnScoreUpdate(int _newScore){
        m_score += _newScore;
        PhotonNetwork.player.SetCustomProperties(new Hashtable{
            {"Score", m_score} 
        });
    }

    private void Update(){
        NetworkUpdate();
    }

    private void NetworkUpdate(){
        
    }

    public PhotonPlayer[] PlayersSorted(){
        PhotonPlayer[] sortMe = PhotonNetwork.playerList;
        MergeSort(sortMe, 0, sortMe.Length - 1);
        return sortMe;
    }

    private static void MergeSort(PhotonPlayer[] _sortMe, int _left, int _right){
        if (_left >= _right) return;
        int partition = (_left + _right) / 2;
            
        MergeSort(_sortMe, _left, partition);
        MergeSort(_sortMe, partition + 1, _right);
            
        Merge(_sortMe, _left, partition, _right);
    }

    private static void Merge(PhotonPlayer[] _in, int _left, int _middle, int _right){
        int i, j, k;
        int n1 = _middle - _left + 1;
        int n2 = _right - _middle;

        PhotonPlayer[] tLeft = new PhotonPlayer[n1];
        PhotonPlayer[] tRight = new PhotonPlayer[n2];
        for (i = 0; i < n1; i++){
            tLeft[i] = _in[_left + i];
        }

        for (j = 0; j < n2; j++){
            tRight[j] = _in[_middle + 1 + j];
        }

        i = 0;
        j = 0;
        k = _left;

        while (i < n1 && j < n2){
            _in[k++] = (int) tLeft[i].CustomProperties["Score"] < (int) tRight[j].CustomProperties["Score"]
                ? tLeft[i++]
                : tRight[j++];
        }

        while (i < n1) _in[k++] = tLeft[i++];
        while (j < n2) _in[k++] = tRight[j++];
    }
}