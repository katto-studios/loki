using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab.AdminModels;
using PlayFab;

public class LeaderboardManager : MonoBehaviour {
    public Text leaderboardsPoints;
    private string m_leaderboardText;


    // Start is called before the first frame update
    void Start() {
        StartCoroutine(SetScoreboard());
    }

    // Update is called once per frame
    void Update() {
    }

    IEnumerator SetScoreboard() {
        Leaderboards.FetchHighScores();
        while(Leaderboards.m_scores.Count <= 0) {
            yield return null;
        }
        m_leaderboardText = "Leaderboards:";

        foreach (KeyValuePair<string, int> kvp in Leaderboards.m_scores) {
            m_leaderboardText += "\n" + kvp.Key + ":\t\t" + kvp.Value;
        }

        leaderboardsPoints.text = m_leaderboardText;
    }
    
}
