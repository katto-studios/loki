using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//handles the game
public class TimeTrialsGameManager : Singleton<TimeTrialsGameManager>{
    public int backLogCount = 1;
    public float CurrentCombo{ get; private set; }
    
    private TimeTrialWordFactory m_fac;
    private string m_typeMe;
    public readonly Queue<string> Backlog = new Queue<string>();
    private string m_currentInput;

    public event Action<string> eOnGetNewWord;
    public event Action<int> eOnScoreUpdate;
    public event Action eOnMiss;
    
    private void Start(){
        m_fac = GetComponent<TimeTrialWordFactory>();

        TimeTrialGameStateManager.Instance.eOnChangedState += (_newState) => {
            if (_newState is TimeTrialGameStateManager.GameStates.Game){
                TimeTrialInputHandler.Instance.eOnKeyDown += HandleKeyPress;
                m_typeMe = m_fac.GetWord();
                for(int count = 0; count < backLogCount; count++) Backlog.Enqueue(m_fac.GetWord());
                eOnGetNewWord?.Invoke(m_typeMe);
                eOnScoreUpdate?.Invoke(0);
            }
        };
    }

    private void Update(){
        if (CurrentCombo >= 0) CurrentCombo -= Time.deltaTime;
    }

    private void HandleKeyPress(char _ch){
        switch (_ch){
            case ' ' when m_currentInput.Equals(m_typeMe):{
                m_typeMe = Backlog.Dequeue();
                Backlog.Enqueue(m_fac.GetWord());
                m_currentInput = string.Empty;
                eOnGetNewWord?.Invoke(m_typeMe);
                eOnScoreUpdate?.Invoke((int)(m_typeMe.Length * 200 * CurrentCombo));
                break;
            }
            case '\b' when string.IsNullOrEmpty(m_currentInput):
                return;
            case '\b':{
                m_currentInput = m_currentInput.Remove(m_currentInput.Length - 1);
                break;
            }
            default:{
                m_currentInput += _ch;
                if (!m_typeMe.StartsWith(m_currentInput)){
                    eOnMiss?.Invoke();
                }
                else{
                    CurrentCombo = 1;
                }
                break;
            }
        }
    }
}