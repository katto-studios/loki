using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialAnalytics : Singleton<TimeTrialAnalytics>{
    public int Score{ get; private set; } = 0;
    public int Misses{ get; private set; } = 0;
    public int Wpm => CharactersTyped / 5;
    public int CurrentChain{ get; private set; } = 0;
    public int MaxChain{ get; private set; } = 0;
    public int CharactersTyped{ get; private set; } = 0;
    public int Accuracy => (int)((1 - (float)Misses / CharactersTyped) * 100);
    public RewardsPanel rewardsPanel;

    private void Start(){
        TimeTrialsGameManager.Instance.eOnScoreUpdate += ScoreUpdate;
        TimeTrialsGameManager.Instance.eOnMiss += OnMiss;
        TimeTrialsGameManager.Instance.eOnGetNewWord += OnNewWord;
        TimeTrialsGameManager.Instance.eOnHit += OnHit;

        TimeTrialGameStateManager.Instance.eOnChangedState += (_state) => {
            if (_state is TimeTrialGameStateManager.GameStates.Analytics){
                PlayfabUserInfo.UpdatePlayerExp(Score);
                rewardsPanel.gameObject.SetActive(true);
                rewardsPanel.Init(Score);
            }
        };
    }

    private void OnHit(){
        if (++CurrentChain > MaxChain){
            MaxChain = CurrentChain;
        }

        CharactersTyped++;
    }

    private void OnNewWord(string _obj){
        //Wpm++;
    }

    private void OnMiss(){
        Misses++;
        CharactersTyped--;
        CurrentChain = 0;
    }

    private void ScoreUpdate(int _newScore){
        Score += _newScore;
    }
}