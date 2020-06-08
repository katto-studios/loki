using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

//handles the game
public class WordLine{
    public List<string> Line{ get; } = new List<string>();
    private int m_current = 0;
    public int CurrentlyOn => m_current;
    public string CurrentWord => Line[m_current];
    public bool AmOut => m_current + 1 >= Line.Count;

    public void BumpCurrent() => m_current++;

    public override string ToString(){
        StringBuilder sb = new StringBuilder();
        if (m_current != 0){
            sb.Append("<color=green>");
        }

        for (int count = 0; count <= Line.Count - 1; count++){
            sb.AppendFormat("{0} ", Line[count]);
            if (m_current - 1 == count){
                sb.Append("</color>");
            }
        }

        return sb.ToString().TrimEnd();
    }

    public string GetFancy(string _whatIHave){
        StringBuilder sb = new StringBuilder();
        sb.Append("<color=green>");

        for (int count = 0; count <= Line.Count - 1; count++){
            if (m_current == count){
                int stopAt = _whatIHave.Length;
                //append each char as green
                for (int charCount = 0; charCount < _whatIHave.Length; charCount++){
                    if (_whatIHave[charCount].Equals(CurrentWord[charCount])){
                        sb.Append(_whatIHave[charCount]);
                    }
                    else{
                        stopAt = charCount;
                        break;
                    }
                }

                if (stopAt < CurrentWord.Length){
                    sb.Append($"</color><u>{CurrentWord[stopAt]}</u>");
                }
                else{
                    sb.Append("</color>");
                }

                for (int charCount = stopAt + 1; charCount < CurrentWord.Length; charCount++){
                    sb.Append(CurrentWord[charCount]);
                }

                sb.Append(" ");
            }
            else{
                sb.AppendFormat("{0} ", Line[count]);
            }
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
                TypeMe = CurrentLine.CurrentWord;
                for (int count = 0; count < backLogCount; count++) Backlog.Enqueue(m_fac.GetLine());
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
                if (!CurrentLine.AmOut){
                    CurrentLine.BumpCurrent();
                    TypeMe = CurrentLine.CurrentWord;
                }
                else{
                    //move on
                    CurrentLine = Backlog.Dequeue();
                    Backlog.Enqueue(m_fac.GetLine());
                    TypeMe = CurrentLine.CurrentWord;
                }

                eOnGetNewWord?.Invoke(TypeMe);
                eOnScoreUpdate?.Invoke((int) (TypeMe.Length * 200 * (CurrentCombo + 1)));
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