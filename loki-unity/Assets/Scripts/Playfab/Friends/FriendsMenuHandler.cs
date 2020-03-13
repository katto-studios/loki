using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cm = PlayFab.ClientModels;
using TMPro;

public class FriendsMenuHandler : MonoBehaviour {
	[Header("Friend display")]
	public GameObject friendContent;
	public GameObject friendInfoPfb;
	[Header("Add friend")]
	public InputField inSearch;

	private List<cm::FriendInfo> m_friends = new List<cm.FriendInfo>();

	private void Awake() {
		PlayfabUserInfo.GetFriendsList(this);
	}

	public void AddToList(cm::FriendInfo _newFriend) {
		m_friends.Add(_newFriend);
		FriendDisplay instanceDisplay = Instantiate(friendInfoPfb, friendContent.transform).GetComponent<FriendDisplay>();
		instanceDisplay.SetFriendInfo(_newFriend);
	}

	public void AddFriend() {
		PlayfabUserInfo.AddFriend(inSearch.text.ToString().Trim());
		//refresh list
		PlayfabUserInfo.GetFriendsList(this);
	}
}
