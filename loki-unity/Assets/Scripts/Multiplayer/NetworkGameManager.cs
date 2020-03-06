using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkGameManager : TypeGameManager {
    private PhotonPlayer m_opponent;
    public override void Start() {
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMatch);

		comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();

        m_opponent = PhotonNetwork.otherPlayers[0];
        //gameState = GameState.Countdown;
    }

    public override void Update() {
        base.Update();

        //update own hashtable
        PhotonNetwork.player.SetCustomProperties(new Hashtable() {
            { "Score", score },
            { "Progress", GetGameProgress() },
			{ "UserState", PlayfabUserInfo.CurrentUserState }
        });
    }

    public void LeaveGame() {
        //update mmr
        PlayfabUserInfo.UpdatePlayerMmr(float.Parse(m_opponent.CustomProperties["Score"].ToString()) < score ? 25 : -25);
        PhotonNetwork.LeaveRoom();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }
}
