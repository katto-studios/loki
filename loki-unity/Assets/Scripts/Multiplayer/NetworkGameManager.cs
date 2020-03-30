using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkGameManager : TypeGameManager {
    //[Header("Regular stuff")]
    public Button btnStartNext;
    [Header("Networking")]
    public int maxRounds = 3;
    private HashSet<PhotonPlayer> m_opponents = new HashSet<PhotonPlayer>();
    private int m_currentRound;
    private bool changed = false;
    public override void Start() {
        foreach (PhotonPlayer other in PhotonNetwork.otherPlayers) {
            m_opponents.Add(other);
        }
        PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMatch);

        comboTimer = maxComboTimer;
        wordsString = PhotonNetwork.room.CustomProperties["Paragraph"] as string;
        //check if it has the same paragaph as the master client
        if (!PhotonNetwork.isMasterClient) {
            string otherProse = m_opponents.First(x => { return x.IsMasterClient; }).CustomProperties["ProseToWrite"] as string;

            if (!wordsString.Equals(otherProse)) {
                Debug.LogError("Client and master mismatch!");
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

        //start countdown
        if (gameState == GameState.Ready) {
            if (m_opponents.All(x => {
                return (PlayfabUserInfo.UserState)x.CustomProperties["UserState"] == PlayfabUserInfo.UserState.InMatch;
            }))
            {
                gameState = GameState.Countdown;
                readyGO.SetActive(false);
                countDownText.gameObject.SetActive(true);
                StartCoroutine(CountDown(5));
            }
        }

        if (PlayfabUserInfo.CurrentUserState == PlayfabUserInfo.UserState.WaitingForNextRound) {
            //check if opponents are ready
            if (m_opponents.All(x => {
                return (PlayfabUserInfo.UserState)x.CustomProperties["UserState"] == PlayfabUserInfo.UserState.WaitingForNextRound;
            })) {
                if (PhotonNetwork.isMasterClient) {
                    if (!changed) {
                        SetProse();
                        StartNextRound();
                        changed = true;
                    }
                } else {
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
    }

    public void LeaveGame() {
        //assume last place
        int position = m_opponents.Count + 1;
        foreach(PhotonPlayer player in m_opponents) {
            if(float.Parse(player.CustomProperties["Score"].ToString()) < score) {
                position++;
            }
        }

        Debug.Log(string.Format("Player came in {0} place", position.ToString()));
        //update mmr
        //PlayfabUserInfo.UpdatePlayerMmr(float.Parse(m_opponent.CustomProperties["Score"].ToString()) < score ? 25 : -25);
        PhotonNetwork.LeaveRoom();
        FindObjectOfType<SceneChanger>().ChangeScene(1);
    }

    public void WhenStartNextRound() {
        gameState = GameState.Ready;
        //set state
        PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.WaitingForNextRound);

        //disable button
        btnStartNext.gameObject.SetActive(false);
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

        if (++m_currentRound >= maxRounds) {
            //actually finish
            btnStartNext.GetComponentInChildren<TextMeshProUGUI>().SetText("Leave game");
            btnStartNext.onClick.RemoveAllListeners();
            btnStartNext.onClick.AddListener(LeaveGame);
        }
    }
}
