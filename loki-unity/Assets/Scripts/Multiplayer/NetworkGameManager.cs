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
    private int m_currentRound;
	private bool changed = false;
    public override void Start() {
        m_opponent = PhotonNetwork.otherPlayers[0];
        PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMatch);

		comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        //check if it has the same paragaph as the master client
        if (!PhotonNetwork.isMasterClient) {
            string otherProse = m_opponent.CustomProperties["ProseToWrite"] as string;
            if (!wordsString.Equals(otherProse)) {
                wordsString = otherProse;
            }
        }
        ConvertStringToTRWords(wordsString);
        GetComponent<NetworkGameRenderer>().Initalise();

        m_currentRound = (int)PhotonNetwork.room.CustomProperties["Round number"];
        score = (int)PhotonNetwork.player.CustomProperties["Score"];

        //PhotonNetwork.room.SetRoomProperty("ReadyToStart", false);
        if (PhotonNetwork.isMasterClient) {
            PhotonNetwork.room.SetCustomProperties(new Hashtable() {
                { "Paragraph", wordsString },
                { "Round number", m_currentRound },
                { "ReadyToStart", false }
            });
        }

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
            if ((PlayfabUserInfo.UserState)m_opponent.CustomProperties["UserState"] == PlayfabUserInfo.UserState.WaitingForNextRound) {
                if (PhotonNetwork.isMasterClient) {
					if (!changed) {
						SetProse();
						StartNextRound();
						changed = true;
					}
                }else {
                    if ((bool)PhotonNetwork.room.CustomProperties["ReadyToStart"]) {
                        StartNextRound();
                    }
                }
            }
        }

        //update own hashtable
        PhotonNetwork.player.SetCustomProperties(new Hashtable() {
            { "Score", score },
            { "Progress", GetGameProgress() },
			{ "UserState", PlayfabUserInfo.CurrentUserState }
        });

        if (Input.GetKeyDown(KeyCode.P)) {
            Complete();
        }
    }

    public void LeaveGame() {
        Debug.Log(float.Parse(m_opponent.CustomProperties["Score"].ToString()) > score ? "Player lost" : "Player won");
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
            { "Round number", m_currentRound },
            { "ReadyToStart", true }
        });

        //set master client prose to write
        if (PhotonNetwork.isMasterClient) {
            PhotonNetwork.player.SetCustomProperties(new Hashtable() {
                    { "Score", score },
                    {"PlayerState", PlayfabUserInfo.UserState.InMatch },
                    { "ProseToWrite", prose.Prose }
            });
        }
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
		}
	}
}
