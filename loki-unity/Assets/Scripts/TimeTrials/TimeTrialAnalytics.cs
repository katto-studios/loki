using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialAnalytics : Singleton<TimeTrialAnalytics>{
    public int Score{ get; private set; } = 0;
    public int Misses{ get; private set; } = 0;
    public int Wpm{ get; private set; } = -1;

    private void Start(){
        TimeTrialsGameManager.Instance.eOnScoreUpdate += ScoreUpdate;
        TimeTrialsGameManager.Instance.eOnMiss += OnMiss;
        TimeTrialsGameManager.Instance.eOnGetNewWord += OnNewWord;

        TimeTrialGameStateManager.Instance.eOnChangedState += (_state) => {
            if (_state is TimeTrialGameStateManager.GameStates.Analytics){
                PlayfabUserInfo.UpdatePlayerExp(Score);
            }
        };
    }

    private void OnNewWord(string _obj){
        Wpm++;
    }

    private void OnMiss(){
        Misses++;
    }

    private void ScoreUpdate(int _newScore){
        Score += _newScore;
    }
}