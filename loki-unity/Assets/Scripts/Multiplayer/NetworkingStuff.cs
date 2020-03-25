using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//for photon networking
public class NetworkingStuff : MonoBehaviour, IPunCallbacks {
    [Header("Console")]
    public InputField inCreateRoom;
    public InputField inJoinRoom;

    [Header("Gameplay")]
    public GameObject goToPlayGameScene;

    [Header("Room list")]
    public GameObject roomDisplay;
    public GameObject pfbRoomDisplay;

    private PhotonPlayer m_opponent;

    // Start is called before the first frame update
    void Start() {
        PrintToConsole("Player connection state: " + PhotonNetwork.connectionState);
        //load all rooms
        foreach(RoomInfo ri in PhotonNetwork.GetRoomList()) {
            Instantiate(pfbRoomDisplay, roomDisplay.transform).GetComponent<RoomInfoDisplay>().SetInfo(ri);
        }
    }

    // Update is called once per frame
    void Update() {
        goToPlayGameScene.SetActive(PlayfabUserInfo.CurrentUserState == PlayfabUserInfo.UserState.ReadyToType);
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
            {"PlayerState", PlayfabUserInfo.CurrentUserState }
        });

        if (PlayfabUserInfo.CurrentUserState == PlayfabUserInfo.UserState.WaitingForOpponent) {
			PlayfabUserInfo.UserState opponentState = (PlayfabUserInfo.UserState)m_opponent.CustomProperties["PlayerState"];
            if (opponentState == PlayfabUserInfo.UserState.WaitingForOpponent) {
                if (PhotonNetwork.isMasterClient) {
                    Paragraph para = GetProse.Instance.GetRandomProse();

                    //set opponent
                    m_opponent.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
                        { "Score", 0 },
                        {"PlayerState", PlayfabUserInfo.UserState.InMatch },
                    });

                    //setself
                    PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
                        { "Score", 0 },
                        {"PlayerState", PlayfabUserInfo.UserState.InMatch },
                        { "ProseToWrite", para.Prose }
                    });

                    PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
                        { "Paragraph", para.Prose },
                        { "Round number", 0 },
                        { "ReadyToStart", true }
                     });

                    //change scene
                    GetComponent<SceneChanger>().ChangeScene(5);
                } else {
                    //wait for readytostart
                    if ((bool)PhotonNetwork.room.CustomProperties["ReadyToStart"]) {
                        //change scene
                        GetComponent<SceneChanger>().ChangeScene(5);
                    }
                }
            }
        }
    }

    public void WhenStartGame() {
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.WaitingForOpponent);
	}

	private void PrintToConsole(string _message) {
        try {
            GameplayConsole.Log(_message);
        } catch (Exception) { }
    }

    public void WhenCreateRoom() {
		//check inCreate for text
		PhotonNetwork.CreateRoom(inCreateRoom.text.Equals("") ? Helper.GenerateRandomString(10) : inCreateRoom.text, new RoomOptions() { MaxPlayers = 8 }, new TypedLobby { Type = LobbyType.Default });
    }

    public void WhenJoinRoom() {
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InQueue);
		if (inJoinRoom.text.Equals("")) {
            PhotonNetwork.JoinRandomRoom();
        }else {
            PhotonNetwork.JoinRoom(inJoinRoom.text);
        }
    }

    public void WhenLeaveRoom() {
        if(PhotonNetwork.room != null) {
            PhotonNetwork.LeaveRoom();
        }

        GetComponent<SceneChanger>().ChangeScene(1);
    }

    #region Photon callbacks
    public void OnConnectedToPhoton() {

    }

    public void OnLeftRoom() {
        PrintToConsole("Left room");
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
	}

	public void OnMasterClientSwitched(PhotonPlayer newMasterClient) {

    }

    public void OnPhotonCreateRoomFailed(object[] codeAndMsg) {
        PrintToConsole("Creation of room failed");
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
	}

	public void OnPhotonJoinRoomFailed(object[] codeAndMsg) {
        PrintToConsole("Join room failed");
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
	}

	public void OnCreatedRoom() {
        PrintToConsole("Creation of room succeeded");
        PrintToConsole(string.Format("Currently in room: {0}", PhotonNetwork.room.Name));
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InQueue);
        PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() {
            { "ReadyToStart", false }
        });
    }

	public void OnJoinedLobby() {

    }

    public void OnLeftLobby() {

    }

    public void OnFailedToConnectToPhoton(DisconnectCause cause) {
        PrintToConsole(string.Format("Failed to connect to photon: {0}", cause));
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
	}

	public void OnConnectionFail(DisconnectCause cause) {
        PrintToConsole(string.Format("Failed to connect: {0}", cause));
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
	}

	public void OnDisconnectedFromPhoton() {
        PrintToConsole("Disconnected from photon");
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.Disconnected);
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info) {

    }

    public void OnReceivedRoomListUpdate() {
        PrintToConsole("Room list updated");
    }

    public void OnJoinedRoom() {
        if (PhotonNetwork.room != null) {
            PrintToConsole("Player is in room: " + PhotonNetwork.room.Name);
            switch (PhotonNetwork.room.PlayerCount) {
                case 1:
					PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InQueue);
					break;
                case 2:
					PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.ReadyToType);
					m_opponent = PhotonNetwork.otherPlayers[0];
                    break;
            }
            PrintToConsole("Current player state: " + PlayfabUserInfo.CurrentUserState.ToString());
        } else {
            PrintToConsole("Failed to create/join room");
			PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
		}
	}

    public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        if (PhotonNetwork.room != null) {
            m_opponent = newPlayer;
            PrintToConsole("Player joined room: " + newPlayer.NickName);
            switch (PhotonNetwork.room.PlayerCount) {
                case 1:
					PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InQueue);
					break;
                case 2:
					PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.ReadyToType);
					break;
            }
            PrintToConsole("Current player state: " + PlayfabUserInfo.CurrentUserState.ToString());
        } else {
            PrintToConsole("Failed to create/join room");
			PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
		}
	}

    public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        if (PhotonNetwork.room != null) {
            m_opponent = null;
            PrintToConsole("Player left room: " + otherPlayer.NickName);
            switch (PhotonNetwork.room.PlayerCount) {
                case 1:
					PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InQueue);
					break;
                case 2:
					PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.ReadyToType);
					break;
            }
            PrintToConsole("Current player state: " + PlayfabUserInfo.CurrentUserState.ToString());
        } else {
            PrintToConsole("Failed to create/join room");
			PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
		}
	}

    public void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        PrintToConsole("Failed to join random room");
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
	}

	public void OnConnectedToMaster() {

    }

    public void OnPhotonMaxCccuReached() {
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InLobby);
		Debug.LogError("PHOTON MAX PLAYERS REACHED!!");
        Application.Quit();
    }

    public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {

    }

    public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps) {

    }

    public void OnUpdatedFriendList() {

    }

    public void OnCustomAuthenticationFailed(string debugMessage) {

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) {

    }

    public void OnWebRpcResponse(OperationResponse response) {

    }

    public void OnOwnershipRequest(object[] viewAndPlayer) {

    }

    public void OnLobbyStatisticsUpdate() {

    }

    public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer) {

    }

    public void OnOwnershipTransfered(object[] viewAndPlayers) {

    }
    #endregion
}
