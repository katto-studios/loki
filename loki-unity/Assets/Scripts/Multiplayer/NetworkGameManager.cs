using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkGameManager : TypeGameManager {
    private PhotonPlayer m_opponent;
    public override void Start() {
        comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();

        m_opponent = PhotonNetwork.otherPlayers[0];
    }

    public override void Update() {
        base.Update();

        //update own hashtable
        PhotonNetwork.player.SetCustomProperties(new Hashtable() {
            { "Score", score },
            { "Progress", GetGameProgress() }
        });
    }

    public void LeaveGame() {
        //update mmr
        PlayfabUserInfo.UpdatePlayerMmr(float.Parse(m_opponent.CustomProperties["Score"].ToString()) < score ? 25 : -25);
        PhotonNetwork.LeaveRoom();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    public override void AddCharacterToInputString(char character) {
        if (gameState == GameState.Ready) {
            gameState = GameState.Countdown;
            readyGO.SetActive(false);
            countDownText.gameObject.SetActive(true);
            StartCoroutine(CountDown(3));
        }

        if (gameState == GameState.Playing) {
            //Update input strings
            inputString += character;
            inputWord += character;

            //Check to move on to the next word
            if (character == ' ' && words[wordIndex].CompareWords(inputWord.ToCharArray())) {
                NextWord();
            }

            if (inputString == wordsString) {
                Complete();
            }

            //Update the textMesh
            UpdateTextMesh();

            if (words[wordIndex].CompareWords(inputWord.ToCharArray()) && inputString.Length > awardedString.Length) {
                awardedString += character;
                combo++;
                float scoreTimeScale = Mathf.Pow(GetComboTimer() * 10.0f, 2);
                score += (int)(scoreTimeScale * combo);

                comboTimer = maxComboTimer;

                if (combo > maxCombo) {
                    maxCombo = combo;
                }
            }

            SendMessage("UpdateInput", SendMessageOptions.DontRequireReceiver);
        }
    }
}
