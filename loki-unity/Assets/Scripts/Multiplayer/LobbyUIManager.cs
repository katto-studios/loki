using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyUIManager : Singleton<LobbyUIManager> {
    [Header("Room selection")]
    public RectTransform roomSelectionScreen;
    public GameObject roomDisplayPfb;
    public Transform roomSelection;
    [Header("Room information")]
    public RectTransform roomInfoScreen;

    void Start() {

    }

    void Update() {

    }

    public void UpdateRooms() {
        //delete and instanitate
        foreach (Transform child in roomSelection) {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo ri in PhotonNetwork.GetRoomList()) {
            Instantiate(roomDisplayPfb, roomSelection).GetComponent<RoomInfoDisplay>().SetInfo(ri);
        }
    }

    public void JoinedRoom() {
        Sequence sq = DOTween.Sequence();
        sq.Append(roomSelectionScreen.DOAnchorPosX(0, 0.5f));
        sq.Append(roomInfoScreen.DOAnchorPosX(-800, 0.5f));
    }

    public void LeftRoom() {
        Sequence sq = DOTween.Sequence();
        sq.Append(roomInfoScreen.DOAnchorPosX(0, 0.5f));
        sq.Append(roomSelectionScreen.DOAnchorPosX(-800, 0.5f));
    }
}
