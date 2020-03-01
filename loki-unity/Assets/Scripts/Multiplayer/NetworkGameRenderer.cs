using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkGameRenderer : TypeGameRenderer {
    [Header("Networking")]
    public TextMeshProUGUI opponentScoreText;

    private PhotonPlayer m_opponent;
    private string m_myName, m_opponentName;
    private Vector3 m_bottom, m_top;
    protected override void Start() {
        typeGameManager = NetworkGameManager.Instance;
        m_opponent = PhotonNetwork.otherPlayers[0];
        //Idk if it matters, but I don't feel safe putting network calls in an update function
        m_myName = PhotonNetwork.player.NickName;
        m_opponentName = m_opponent.NickName;

        //I can't even explain this one
        m_bottom = opponentScoreText.transform.position;
        m_top = scoreTextMesh.transform.position;
    }

    public void Initalise() {
        wordTextMesh.text = typeGameManager.wordsString;
    }

    protected override void Update() {
        slider.value = typeGameManager.GetComboTimer();
        comboTextMesh.text = "X" + typeGameManager.combo;
        scoreTextMesh.text = m_myName + "\nScore: " + typeGameManager.score;

        if (m_opponent != null) {
            float opponentScore = float.Parse(m_opponent.CustomProperties["Score"].ToString());
            opponentScoreText.text = m_opponentName + "\nScore: " + opponentScore.ToString();
            //decide who is on the bottom
            bool isWinning = typeGameManager.score > opponentScore;
            scoreTextMesh.transform.position = isWinning ? m_top : m_bottom;
            opponentScoreText.transform.position = !isWinning ? m_top : m_bottom;
        }else { //opponent left
            opponentScoreText.gameObject.SetActive(false);
            scoreTextMesh.transform.position = m_top;
        }
    }
}
