using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

//handles the game
public class WordLine{
    public Queue<string> Line{ get; } = new Queue<string>();

    public override string ToString(){
        StringBuilder sb = new StringBuilder();
        foreach (string s in Line){
            sb.Append($"{s} ");
        }

        return sb.ToString();
    }
}

public class TimeTrialsGameManager : Singleton<TimeTrialsGameManager>{
    public int backLogCount = 1;
    public float CurrentCombo{ get; private set; }
    public int CurrentChain{ get; private set; } = 0;
    
    private TimeTrialWordFactory m_fac;
    public string TypeMe{ get; private set; }
    public WordLine CurrentLine{ get; private set; } = null;
    public readonly Queue<WordLine> Backlog = new Queue<WordLine>();
    private string m_currentInput;

    public event Action<string> eOnGetNewWord;
    public event Action<int> eOnScoreUpdate;
    public event Action eOnMiss;
    public event Action eOnHit;
    
    private void Start(){
        m_fac = GetComponent<TimeTrialWordFactory>();

        TimeTrialGameStateManager.Instance.eOnChangedState += (_newState) => {
            if (_newState is TimeTrialGameStateManager.GameStates.Game){
                TimeTrialInputHandler.Instance.eOnKeyDown += HandleKeyPress;
                CurrentLine = m_fac.GetLine();
                TypeMe = CurrentLine.Line.Dequeue();
                for(int count = 0; count < backLogCount; count++) Backlog.Enqueue(m_fac.GetLine());
                eOnGetNewWord?.Invoke(TypeMe);
                eOnScoreUpdate?.Invoke(0);
            }
        };
    }

    private void Update(){
        if (CurrentCombo >= 0) CurrentCombo = Mathf.Max(CurrentCombo - Time.deltaTime, 0);
    }

    private void HandleKeyPress(char _ch){
        switch (_ch){
            case ' ' when m_currentInput.Equals(TypeMe):{
                m_currentInput = string.Empty;
                if (CurrentLine.Line.Count > 0){
                    TypeMe = CurrentLine.Line.Dequeue();
                }
                else{
                    //move on
                    CurrentLine = Backlog.Dequeue();
                    TypeMe = CurrentLine.Line.Dequeue();
                    Backlog.Enqueue(m_fac.GetLine());
                }
                eOnGetNewWord?.Invoke(TypeMe);
                eOnScoreUpdate?.Invoke((int)(TypeMe.Length * 200 * (CurrentCombo + 1)));
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
                if (!TypeMe.StartsWith(m_currentInput)){
                    eOnMiss?.Invoke();
                }
                else{
                    eOnHit?.Invoke();
                    CurrentCombo = 1;
                }
                break;
            }
        }
    }
}