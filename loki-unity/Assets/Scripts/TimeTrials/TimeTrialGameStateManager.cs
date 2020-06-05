using System;
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
            case GameStates.Game:{
                break;
            }
            case GameStates.Analytics:{
                break;
            }
            case GameStates.No:
                break;
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
    }
    
    private IEnumerator ChangeState(float _delay, GameStates _newState){
        yield return new WaitForSeconds(_delay);
        m_currentState = _newState;
        eOnChangedState?.Invoke(_newState);
    }
}
