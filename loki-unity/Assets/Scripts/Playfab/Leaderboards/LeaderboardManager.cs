using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;
using PlayFab.AdminModels;
using PlayFab;

public class LeaderboardManager : MonoBehaviour {
    [Header("Points")]
    public GameObject leaderboardPanel;
    public GameObject leaderboardDisplay;

    [Header("Words per min")]
    public Text wpmPoints;
    private string m_wpmText;

    // Start is called before the first frame update
    void Start() {
        Leaderboards.FetchHighScores(this);
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.ViewingLeaderBoards);
    }
    
    public void OverlapThis(Button _caller) {
        foreach(Transform child in _caller.transform.parent.parent) {
            child.GetChild(1).gameObject.SetActive(false);
        }

        _caller.transform.parent.GetChild(1).gameObject.SetActive(true);
    }

    public void AddToScores(string _name, int _score) {
        Instantiate(leaderboardDisplay, leaderboardPanel.transform).GetComponent<LeaderboardContentDisplay>().SetContent(_name, _score);
    }
}
