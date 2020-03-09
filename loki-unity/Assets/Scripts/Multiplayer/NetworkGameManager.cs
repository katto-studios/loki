using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkGameManager : TypeGameManager {
	//[Header("Regular stuff")]
	public Button btnStartNext;
	[Header("Networking")]
	public int maxRounds = 3;
    private PhotonPlayer m_opponent;
	private int m_roundNo = 1;	//round count
    public override void Start() {
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMatch);

		comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();

        m_opponent = PhotonNetwork.otherPlayers[0];

		gameState = GameState.Ready;
	}

    public override void Update() {
        base.Update();

		if(gameState == GameState.Ready) {
			if((PlayfabUserInfo.UserState)m_opponent.CustomProperties["UserState"] == PlayfabUserInfo.UserState.InMatch) {
				gameState = GameState.Countdown;
				readyGO.SetActive(false);
				countDownText.gameObject.SetActive(true);
				StartCoroutine(CountDown(5));
			}
		}

        //update own hashtable
        PhotonNetwork.player.SetCustomProperties(new Hashtable() {
            { "Score", score },
            { "Progress", GetGameProgress() },
			{ "UserState", PlayfabUserInfo.CurrentUserState }
        });

		//check for next round
		if(PlayfabUserInfo.CurrentUserState == PlayfabUserInfo.UserState.WaitingForNextRound) {
			PlayfabUserInfo.UserState opponentState = (PlayfabUserInfo.UserState)m_opponent.CustomProperties["UserState"];
			if(opponentState == PlayfabUserInfo.UserState.WaitingForNextRound) {
				//start next round
				if (m_roundNo % 2 == 0) {
					if (PhotonNetwork.isMasterClient) {
						//determine prose
					}
				} else {
					if (!PhotonNetwork.isMasterClient) {
						//determine prose
					}
				}
				GetComponent<SceneChanger>().ChangeScene(5);
			}
		}
    }

    public void LeaveGame() {
        //update mmr
        PlayfabUserInfo.UpdatePlayerMmr(float.Parse(m_opponent.CustomProperties["Score"].ToString()) < score ? 25 : -25);
        PhotonNetwork.LeaveRoom();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

	public void WhenStartNextRound() {
		gameState = GameState.Ready;
		//set state
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.WaitingForNextRound);
	}

	private void StartNextRound() {

	}

	protected override void Complete() {
		base.Complete();
		if(++m_roundNo >= maxRounds) {
			//actually finish
			btnStartNext.onClick.AddListener(() => GetComponent<SceneChanger>().ChangeScene(1));
		}
	}
}
