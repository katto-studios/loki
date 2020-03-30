using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDisplayManager : Singleton<RoomDisplayManager> {
    [Header("Rooms")]
    public GameObject roomDisplayPfb;
    public Transform content;

    public void UpdateRooms() {
        //delete and instanitate
        foreach (Transform child in content) {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo ri in PhotonNetwork.GetRoomList()) {
            Instantiate(roomDisplayPfb, content).GetComponent<RoomInfoDisplay>().SetInfo(ri);
        }
    }
}
