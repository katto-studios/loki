using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
    [SerializeField] private TextMeshProUGUI m_backlogDisplay;
    [SerializeField] private Slider m_comboSlider;
    [SerializeField] private Slider m_timerSlider;
    public bool showFlickering = false;
    private string m_displayInputTotal;
    private string m_currentWord;
    private int m_score = 0;
    private float m_internalTimer = -1;
    private bool m_showSlash = false;
    private Coroutine m_flicker;
    
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

    private void Update(){
        if (m_internalTimer >= 0){
            m_internalTimer -= Time.deltaTime;
            m_timerSlider.value = m_internalTimer / 60;
            
            //update combo
            m_comboSlider.value = TimeTrialsGameManager.Instance.CurrentCombo;
            m_comboDisplay.SetText(TimeTrialAnalytics.Instance.CurrentChain.ToString());
        }
    }

    private IEnumerator StartFlicker(){
        while (true){
            yield return new WaitForSeconds(0.7f);
            m_showSlash = !m_showSlash;

            m_inputDisplay.SetText($"<color={(m_currentWord.StartsWith(m_displayInputTotal) ? "green" : "red")}>{m_displayInputTotal}{(m_showSlash ? "|" : "")}</color>");
        }
    }

    private void OnScoreUpdate(int _newScore){
        m_score += _newScore;
        m_scoreDisplay.SetText(m_score.ToString());
    }

    private void GotNewWord(string _newWord){
        string playWithMe = $"{TimeTrialsGameManager.Instance.CurrentLine}";
        
        m_textDisplay.SetText(playWithMe);
        m_displayInputTotal = string.Empty;
        m_currentWord = _newWord;
        m_inputDisplay.SetText("<color=red></color>");
        
        //render backlog
        StringBuilder sb = new StringBuilder();
        foreach (WordLine line in TimeTrialsGameManager.Instance.Backlog){
            sb.AppendLine($"{line}");
        }
        m_backlogDisplay.SetText(sb.ToString());
    }

    private void OnNewState(TimeTrialGameStateManager.GameStates _newState){
        switch (_newState){
            case TimeTrialGameStateManager.GameStates.Game:{
                TimeTrialInputHandler.Instance.eOnKeyDown += HandleKeyPressForGame;
                if(showFlickering) m_flicker = StartCoroutine(StartFlicker());
                m_internalTimer = 60;
                m_pre.SetActive(false);
                m_game.SetActive(true);
                break;
            }
            case TimeTrialGameStateManager.GameStates.Analytics:{
                m_game.SetActive(false);
                m_analytics.SetActive(true);
                if(m_flicker != null) StopCoroutine(m_flicker);
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
        m_combo.SetText($"x{TimeTrialAnalytics.Instance.MaxChain.ToString()}");
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
            case ' ' when TimeTrialsGameManager.Instance.TypeMe.Equals(m_displayInputTotal) :{
                m_displayInputTotal = string.Empty;
                
                break;
            }
            default:{
                m_displayInputTotal += _ch;
                break;
            }
        }

        m_inputDisplay.SetText($"<color={(m_currentWord.StartsWith(m_displayInputTotal) ? "green" : "red")}>{m_displayInputTotal}{(m_showSlash ? "|" : "")}</color>");
        m_textDisplay.SetText(
            TimeTrialsGameManager.Instance.CurrentLine.GetFancy(m_displayInputTotal)    
        );
    }
}
