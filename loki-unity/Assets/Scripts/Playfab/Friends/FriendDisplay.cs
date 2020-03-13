using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using cm = PlayFab.ClientModels;

public class FriendDisplay : MonoBehaviour {
	[Header("Displays")]
	public Image backgroundImage;
	public TextMeshProUGUI nameDisplay;

	public void SetFriendInfo(cm::FriendInfo _fr) {
		nameDisplay.SetText(_fr.TitleDisplayName);
	}
}
