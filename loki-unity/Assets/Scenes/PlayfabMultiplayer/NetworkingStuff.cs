using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkingStuff : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnCreateRoom() {
        PhotonNetwork.JoinOrCreateRoom("XD", new RoomOptions() { }, new TypedLobby { Type = LobbyType.Default});
    }

	public void OnJoinRoom() {
		PhotonNetwork.JoinRandomRoom();
	}

    public void OnRaiseRoomEvent() {
        //raise room event
        Dictionary<string, object> data = new Dictionary<string, object>() { { "Hello", " world" } };

        bool result = PhotonNetwork.RaiseEvent(15, data, true, new RaiseEventOptions() { ForwardToWebhook = true, });
        Debug.Log("New room event: " + result);

        //set custom property
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable() { { "CustomProperty", "It's Value" } };
        ExitGames.Client.Photon.Hashtable expectedProperties = new ExitGames.Client.Photon.Hashtable();
        PhotonNetwork.room.SetCustomProperties(properties, expectedProperties, true);
        Debug.Log("New Room Properties Set");
    }
}
