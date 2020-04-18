using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cm = PlayFab.ClientModels;
using TMPro;
using System;
using ExitGames.Client.Photon;

public class FriendsMenuHandler : Singleton<FriendsMenuHandler> {
	[Header("Friend display")]
	public GameObject friendContent;
	public GameObject friendInfoPfb;
    public GameObject passwordPrompt;

    [Header("Add friend")]
	public InputField inSearch;

    [Header("Pending")]
    public GameObject pendingFriends;
    public GameObject pendingFriendPfb;

    public bool Ready { get; set; }

    private List<cm::FriendInfo> m_friends = new List<cm::FriendInfo>();
    private List<cm::FriendInfo> m_pending = new List<cm::FriendInfo>();
    private RoomInfo m_targetRoom = null;

    private void Awake() {
        Ready = false;
		PlayfabUserInfo.GetFriendsList();
	}

	public void AddToFriendsList(cm::FriendInfo _newFriend) {
		m_friends.Add(_newFriend);
		FriendDisplay instanceDisplay = Instantiate(friendInfoPfb, friendContent.transform).GetComponent<FriendDisplay>();
		instanceDisplay.SetFriendInfo(_newFriend);
	}

    public void AddToPendingFriendsList(cm::FriendInfo _pending) {
        m_pending.Add(_pending);
        PendingDisplay instanceDisplay = Instantiate(pendingFriendPfb, pendingFriends.transform).GetComponent<PendingDisplay>();
        instanceDisplay.Initalise(_pending.TitleDisplayName);
    }

	public void AddFriend() {
		PlayfabUserInfo.AddFriend(inSearch.text);
		//refresh list
		PlayfabUserInfo.GetFriendsList();
	}

    //listener for button
    public void AcceptFriend() {
        PlayfabUserInfo.AcceptFriend(GetComponent<FriendDisplay>().nameDisplay.text);
        //refresh list
        PlayfabUserInfo.GetFriendsList();
    }

    public void DenyFriend() {

    }

    public void ClearFriendsList() {
        m_friends.Clear();
        m_pending.Clear();
    }

    public void OnPasswordEnter() {
        if (passwordPrompt.GetComponentInChildren<InputField>().text.Equals(m_targetRoom.CustomProperties["Password"].ToString())) {
            PhotonNetwork.JoinRoom(m_targetRoom.Name);
        }else {
            //wrong password
            m_targetRoom = null;
            passwordPrompt.SetActive(false);
        }
    }

    public void PromptPassword(RoomInfo _ri) {
        m_targetRoom = _ri;
        passwordPrompt.SetActive(true);
    }
}
