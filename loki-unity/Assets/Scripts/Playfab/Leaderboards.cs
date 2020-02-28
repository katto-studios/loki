using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using System.Threading;

public static class Leaderboards {

    public static List<KeyValuePair<string, int>> m_scores = new List<KeyValuePair<string, int>>();

    public static List<KeyValuePair<string, int>> GetHighScores() {
        m_scores = new List<KeyValuePair<string, int>>();
        FetchHighScores();
        return m_scores;
    }

    public static void FetchHighScores() {
        PlayFabClientAPI.GetLeaderboard(
            new GetLeaderboardRequest() { StatisticName = "Points high score" },
            (_result) => {
                int limit = Mathf.Clamp(_result.Leaderboard.Count, 0, 5);
                for (int count = 0; count <= limit - 1; count++) {
                    m_scores.Add(new KeyValuePair<string, int>(_result.Leaderboard[count].DisplayName, _result.Leaderboard[count].StatValue));
                }
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );

    }
}
