using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardContentDisplay : MonoBehaviour {
    [Header("Fields")]
    public TextMeshProUGUI username;
    public TextMeshProUGUI score;

    public void SetContent(string _username, int _score) {
        username.SetText(_username);
        score.SetText(_score.ToString());
    }
}
