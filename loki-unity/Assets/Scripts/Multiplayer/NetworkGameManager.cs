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
    public int m_currentRound;
    public override void Start() {
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMatch);

		comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();

        m_currentRound = (int)PhotonNetwork.room.CustomProperties["Round number"];
        score = (int)PhotonNetwork.player.CustomProperties["Score"];

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

        if(PlayfabUserInfo.CurrentUserState == PlayfabUserInfo.UserState.WaitingForNextRound) {
            //check if opponent is ready
            PlayfabUserInfo.UserState opponentState = (PlayfabUserInfo.UserState)m_opponent.CustomProperties["UserState"];
            if (opponentState == PlayfabUserInfo.UserState.WaitingForNextRound) {
                //start next round
                if (++m_currentRound % 2 == 0) {
                    if (PhotonNetwork.isMasterClient) {
                        SetProse();
                    }
                } else {
                    if (!PhotonNetwork.isMasterClient) {
                        SetProse();
                    }
                }
                StartNextRound();
            }
        }

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

	public void WhenStartNextRound() {
		gameState = GameState.Ready;
		//set state
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.WaitingForNextRound);
    }

    private void SetProse() {
        //determine prose
        Paragraph prose = GetProse.Instance.GetRandomProse();

        PhotonNetwork.room.SetCustomProperties(new Hashtable() {
            { "Paragraph", prose.Prose },
            { "Round number", m_currentRound }
        });
    }

	private void StartNextRound() {
        FindObjectOfType<SceneChanger>().ChangeScene(5);
    }

    protected override void Complete() {
		base.Complete();

		if(++m_currentRound >= maxRounds) {
            //actually finish
            btnStartNext.GetComponentInChildren<TextMeshProUGUI>().SetText("Leave game");
            btnStartNext.onClick.RemoveAllListeners();
			btnStartNext.onClick.AddListener(LeaveGame);

            Debug.Log(float.Parse(m_opponent.CustomProperties["Score"] as string) > score ? "Player lost" : "Player won");
		}
	}
}
