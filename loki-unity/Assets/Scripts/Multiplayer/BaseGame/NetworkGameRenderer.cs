using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkGameRenderer : TypeGameRenderer {
    [Header("Networking")]
    public Image background;
    public GameObject playerInfoPfb;

    private HashSet<PhotonPlayer> m_opponents = new HashSet<PhotonPlayer>();
    private bool m_prevResult = true;
    protected override void Start() {
        typeGameManager = NetworkGameManager.Instance;
        Instantiate(playerInfoPfb, background.transform).GetComponent<NetworkUserDisplayInfo>().Initalise(PhotonNetwork.player);

        foreach (PhotonPlayer other in PhotonNetwork.otherPlayers) {
            m_opponents.Add(other);
            Instantiate(playerInfoPfb, background.transform).GetComponent<NetworkUserDisplayInfo>().Initalise(other);
        }
    }

    public void Initalise() {
        wordTextMesh.text = typeGameManager.wordsString;
    }

    protected override void Update() {
        //combo stuff
        slider.value = typeGameManager.GetComboTimer();
        comboTextMesh.text = "X" + typeGameManager.combo;
    }
}