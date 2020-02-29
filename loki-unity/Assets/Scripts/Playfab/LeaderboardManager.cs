using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab.AdminModels;
using PlayFab;

public class LeaderboardManager : MonoBehaviour {
    [Header("Networking")]
    public int timeoutThreshold = 20000;

    [Header("Points")]
    public Text leaderboardsPoints;
    private string m_leaderboardText;

    [Header("Words per min")]
    public Text wpmPoints;
    private string m_wpmText;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(SetHighScoreHighscoreBoard());
        StartCoroutine(SetWpmHighscoreBoard());
    }

    // Update is called once per frame
    void Update() {
    }

    private IEnumerator SetHighScoreHighscoreBoard() {
        Leaderboards.FetchHighScores();
        int i = 0;
        while (Leaderboards.HighScores.Count <= 0 && ++i <= timeoutThreshold) {
            yield return null;
        }
        m_leaderboardText = "Leaderboards:";

        for(int count = 0; count <= Leaderboards.HighScores.Count - 1; count++) {
            m_leaderboardText += string.Format("\n{0}.\t{1}:\t\t{2}", (count + 1).ToString(), Leaderboards.HighScores[count].Key, Leaderboards.HighScores[count].Value);
        }

        leaderboardsPoints.text = m_leaderboardText;
    }
    
    private IEnumerator SetWpmHighscoreBoard() {
        Leaderboards.FetchWpmScores();
        int i = 0;
        while(Leaderboards.HighScores.Count <= 0 && ++i <= timeoutThreshold) {
            yield return null;
        }
        m_wpmText = "Leaderboards:";

        for (int count = 0; count <= Leaderboards.WpmScores.Count - 1; count++) {
            m_wpmText += string.Format("\n{0}.\t{1}:\t\t{2}", (count + 1).ToString(), Leaderboards.WpmScores[count].Key, Leaderboards.WpmScores[count].Value);
        }

        wpmPoints.text = m_wpmText;
        wpmPoints.transform.parent.gameObject.SetActive(false);
    }

    public void OverlapThis(Button _caller) {
        foreach(Transform child in _caller.transform.parent.parent) {
            child.GetChild(1).gameObject.SetActive(false);
        }

        _caller.transform.parent.GetChild(1).gameObject.SetActive(true);
    }
}
