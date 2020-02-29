using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkGameRenderer : MonoBehaviour {
    NetworkGameManager typeGameManager;
    public TextMeshProUGUI wordTextMesh;
    public GameObject GamePanel;

    public Slider slider;
    public TextMeshProUGUI comboTextMesh;
    public TextMeshProUGUI scoreTextMesh;
    public TextMeshProUGUI enemyScoreTextMesh;

    private void Start() {
        typeGameManager = NetworkGameManager.Instance;
    }

    public void Initalise() {
        wordTextMesh.text = typeGameManager.wordsString;
    }

    public void Update() {
        slider.value = typeGameManager.GetComboTimer();
        comboTextMesh.text = "X" + typeGameManager.combo;
        scoreTextMesh.text = "score: " + typeGameManager.score;
    }

    public void UpdateInput() {
        string newWordString = "<color=#BBFFBB>";

        for (int i = 0; i < typeGameManager.words.Count; i++) {
            if (i == typeGameManager.wordIndex) {
                newWordString += "<color=#FFFFFF>";
            }

            newWordString += typeGameManager.words[i].word.ArrayToString();
        }

        wordTextMesh.text = newWordString;
    }

    public void GameComplete() {
        GamePanel.SetActive(false);
    }
}
