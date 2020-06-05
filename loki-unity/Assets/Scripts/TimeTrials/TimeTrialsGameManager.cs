using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the game
public class TimeTrialsGameManager : Singleton<TimeTrialsGameManager>{
    private TimeTrialWordFactory m_fac;
    [SerializeField] private string m_typeMe;
    [SerializeField] private string m_currentInput;
    private float m_score = 0;

    public event Action<string> eOnGetNewWord;
    public event Action<float> eOnScoreUpdate;

    private void Start(){
        m_fac = GetComponent<TimeTrialWordFactory>();

        TimeTrialGameStateManager.Instance.eOnChangedState += (_newState) => {
            if (_newState == TimeTrialGameStateManager.GameStates.Game){
                TimeTrialInputHandler.Instance.eOnKeyDown += HandleKeyPress;
            }
        };
        TimeTrialGameStateManager.Instance.eOnChangedState += (_state) => {
            if (_state is TimeTrialGameStateManager.GameStates.Game){
                m_typeMe = m_fac.GetWord();
                eOnGetNewWord?.Invoke(m_typeMe);
                eOnScoreUpdate?.Invoke(m_score);
            }
        };
    }

    private void HandleKeyPress(char _ch){
        if (_ch.Equals('\r') && m_currentInput.Equals(m_typeMe)){
            //only award score after word is submitted, rather than every char
            m_score += m_typeMe.Length; //change me to exponential

            m_typeMe = m_fac.GetWord();
            m_currentInput = string.Empty;
            eOnGetNewWord?.Invoke(m_typeMe);
            eOnScoreUpdate?.Invoke(m_score);
        }
        else if (_ch.Equals('\b')){
            if (string.IsNullOrEmpty(m_currentInput)) return;
                
            m_currentInput = m_currentInput.Remove(m_currentInput.Length - 1);
        }
        else{
            m_currentInput += _ch;
        }
    }
}