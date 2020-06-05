using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PlayFab.Public;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//use to update ui elements
public class TimeTrialRenderer : Singleton<TimeTrialRenderer>{
    [Header("Pre game")] 
    [SerializeField] private GameObject m_pre;
    [SerializeField] private TextMeshProUGUI m_preGameCountDown;
    
    [Header("Actual game")]
    [SerializeField] private GameObject m_game;
    [SerializeField] private TextMeshProUGUI m_inputDisplay;
    [SerializeField] private TextMeshProUGUI m_textDisplay;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;
    [SerializeField] private TextMeshProUGUI m_comboDisplay;
    [SerializeField] private Slider m_comboSlider;
    private string m_displayInputTotal;
    private string m_currentWord;
    private int m_score = 0;
    
    [Header("Analytics")]
    [SerializeField] private GameObject m_analytics;
    [SerializeField] private TextMeshProUGUI m_finalScore;
    [SerializeField] private TextMeshProUGUI m_combo;
    [SerializeField] private TextMeshProUGUI m_wpm;
    [SerializeField] private TextMeshProUGUI m_mistakes;
    [SerializeField] private TextMeshProUGUI m_acc;

    private void Start(){
        TimeTrialGameStateManager.Instance.eOnStartGameTick += WhenStartGameTick;
        TimeTrialGameStateManager.Instance.eOnChangedState += OnNewState;
        
        TimeTrialsGameManager.Instance.eOnGetNewWord += GotNewWord;
        TimeTrialsGameManager.Instance.eOnScoreUpdate += OnScoreUpdate;
    }

    private void OnScoreUpdate(int _newScore){
        m_score += _newScore;
        m_scoreDisplay.SetText(m_score.ToString());
    }

    private void GotNewWord(string _newWord){
        m_textDisplay.SetText(_newWord);
        m_displayInputTotal = string.Empty;
        m_currentWord = _newWord;
        m_inputDisplay.SetText("<color=red></color>");
    }

    private void OnNewState(TimeTrialGameStateManager.GameStates _newState){
        switch (_newState){
            case TimeTrialGameStateManager.GameStates.Game:{
                TimeTrialInputHandler.Instance.eOnKeyDown += HandleKeyPressForGame;
                m_pre.SetActive(false);
                m_game.SetActive(true);
                break;
            }
            case TimeTrialGameStateManager.GameStates.Analytics:{
                m_game.SetActive(false);
                m_analytics.SetActive(true);
                StartAnalysis();
                break;
            }
        }
    }

    private void StartAnalysis(){
        m_finalScore.SetText(TimeTrialAnalytics.Instance.Score.ToString());
        m_mistakes.SetText(TimeTrialAnalytics.Instance.Misses.ToString());
        m_wpm.SetText(TimeTrialAnalytics.Instance.Wpm.ToString());
        m_acc.SetText(((float)TimeTrialAnalytics.Instance.Misses / TimeTrialAnalytics.Instance.Wpm * 100).ToString());
    }

    private void WhenStartGameTick(int _howMuch){
        m_preGameCountDown.SetText(_howMuch.ToString());
    }

    private void HandleKeyPressForGame(char _ch){
        switch (_ch){
            case '\b':{
                if (string.IsNullOrEmpty(m_displayInputTotal)) break;
                
                m_displayInputTotal = m_displayInputTotal.Remove(m_displayInputTotal.Length - 1);
                break;
            }
            case '\r':{
                break;
            }
            default:{
                m_displayInputTotal += _ch;
                break;
            }
        }

        m_inputDisplay.SetText($"<color={(m_currentWord.StartsWith(m_displayInputTotal) ? "green" : "red")}>{m_displayInputTotal}</color>");
    }
}
