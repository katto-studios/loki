﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialGameStateManager : Singleton<TimeTrialGameStateManager>{
    public enum GameStates {Pre, Game, Analytics, No}
    private GameStates m_currentState = GameStates.Pre;
    private Coroutine m_switchState;

    public event Action<GameStates> eOnChangedState;
    public event Action<int> eOnStartGameTick;

    private void Start(){
        TimeTrialInputHandler.Instance.eOnKeyDown += HandleKeyPress;
    }

    private void HandleKeyPress(char _ch){
        switch (m_currentState){
            case GameStates.Pre:{
                if (_ch.Equals(' ') && m_switchState == null){
                    m_switchState = StartCoroutine(StartGame(3));
                }
                break;
            }
        }
    }

    public void StartMultiplayer(){
        TimeTrialInputHandler.Instance.eOnKeyDown -= HandleKeyPress;
        if (m_switchState == null){
            StartCoroutine(StartGame(5));
        }
    }

    private IEnumerator StartGame(float _delay){
        float howMuchAlr = _delay;
        float bumpWithMe = 1;    //change this if delay too long
        eOnStartGameTick?.Invoke((int)howMuchAlr);

        while (howMuchAlr > 0){
            yield return new WaitForSeconds(bumpWithMe);
            howMuchAlr -= bumpWithMe;
            eOnStartGameTick?.Invoke((int)howMuchAlr);
        }

        m_currentState = GameStates.Game;
        eOnChangedState?.Invoke(m_currentState);
        
        yield return new WaitForSeconds(60);
        m_currentState = GameStates.Analytics;
        eOnChangedState?.Invoke(m_currentState);
    }
    
    private IEnumerator ChangeState(float _delay, GameStates _newState){
        yield return new WaitForSeconds(_delay);
        m_currentState = _newState;
        eOnChangedState?.Invoke(_newState);
    }
}