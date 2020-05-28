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
    public TextMeshProUGUI roomNameDisplay;
    public GameObject playerInRoomInfo;
    public Transform detailedRoomInfoContent;
    public GameObject passwordPrompt;
    private RoomInfo m_targetRoom = null;

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
        UpdateDetailedRoomInfo();
    }

    public void LeftRoom() {
        Sequence sq = DOTween.Sequence();
        sq.Append(roomInfoScreen.DOAnchorPosX(0, 0.5f));
        sq.Append(roomSelectionScreen.DOAnchorPosX(-800, 0.5f));
    }

    public void UpdateDetailedRoomInfo() {
        foreach(Transform child in detailedRoomInfoContent) {
            Destroy(child.gameObject);
        }

        roomNameDisplay.SetText(string.Format("{0}", PhotonNetwork.room.Name));

        Instantiate(playerInRoomInfo, detailedRoomInfoContent).GetComponent<PlayerInRoomDisplay>().SetDisplays(PhotonNetwork.player);
        foreach(PhotonPlayer player in PhotonNetwork.otherPlayers) {
            Instantiate(playerInRoomInfo, detailedRoomInfoContent).GetComponent<PlayerInRoomDisplay>().SetDisplays(player);
        }
    }

    public void OnPasswordEnter() {
        if (m_targetRoom.CustomProperties["Password"].ToString().Equals(passwordPrompt.GetComponentInChildren<InputField>().text)) {
            PhotonNetwork.JoinRoom(m_targetRoom.Name);
        }else {
            PopupManager.Instance.ShowPopUp("Wrong password!", 3);
        }

        m_targetRoom = null;
        passwordPrompt.SetActive(false);
    }

    public void PasswordPrompt(RoomInfo _ri) {
        passwordPrompt.SetActive(true);
        m_targetRoom = _ri;
    }
}
