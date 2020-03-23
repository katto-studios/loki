using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkGameRenderer : TypeGameRenderer {
    [Header("Networking")]
    public Image background;
    public NetworkUserDisplayInfo playerDisplay;
    public NetworkUserDisplayInfo opponentDisplay;

    private PhotonPlayer m_opponent;
    private bool m_prevResult = true;
    protected override void Start() {
        typeGameManager = NetworkGameManager.Instance;
        m_opponent = PhotonNetwork.otherPlayers[0];
        //Idk if it matters, but I don't feel safe putting network calls in an update function
        playerDisplay.SetName(PhotonNetwork.player.NickName);
        opponentDisplay.SetName(m_opponent.NickName);
    }

    public void Initalise() {
        wordTextMesh.text = typeGameManager.wordsString;
    }

    protected override void Update() {
        slider.value = typeGameManager.GetComboTimer();
        comboTextMesh.text = "X" + typeGameManager.combo;
        playerDisplay.SetScore(typeGameManager.score);
        playerDisplay.UpdateProgress(typeGameManager.GetGameProgress());

        if (m_opponent != null) {
            float opponentScore = float.Parse(m_opponent.CustomProperties["Score"].ToString());
            opponentDisplay.SetScore(opponentScore);
            //decide who is on the bottom
            bool isWinning = typeGameManager.score > opponentScore;
            if(isWinning != m_prevResult) {
                playerDisplay.Swap(opponentDisplay);
            }
            m_prevResult = isWinning;

            //progress
            opponentDisplay.UpdateProgress(float.Parse(m_opponent.CustomProperties["Progress"].ToString()));
        } else { //opponent left
            opponentDisplay.gameObject.SetActive(false);
        }
    }
}