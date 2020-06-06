using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using cm = PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ViewProfileManager : Singleton<ViewProfileManager>
{
    public UserAccountInfo AccountInfo;

    public TextMeshProUGUI tname;
    public TextMeshProUGUI tstats;
    public TextMeshProUGUI tlevel;
    public RawImage profilePicture;

    public GameObject AddFriendButton;
    public GameObject RemoveFriendButton;

    public NetworkKeyboard keyboard;

    public void Init(UserAccountInfo u)
    {
        AccountInfo = u;
        tname.text = u.Username;
        string url = u.TitleInfo.AvatarUrl;
        StartCoroutine(FetchImage(url));
        PlayerStatsCallBack pscb = GetStats;
        PlayFabPlayerData.GetPlayerStats(u, pscb);
        PlayerInventoryCallBack picb = GetInventoryData;
        PlayFabPlayerData.GetUserInventory(u, picb);
        if (u.PlayFabId != PlayfabUserInfo.AccountInfo.PlayFabId)
        {
            bool isStranger = true;
            List<cm::FriendInfo> friends = PlayfabUserInfo.Friends;

            foreach (cm::FriendInfo friend in friends)
            {
                if (friend.FriendPlayFabId == u.PlayFabId)
                {
                    if (friend.Tags.Contains("Friends"))
                    {
                        isStranger = false;
                        RemoveFriendButton.SetActive(true);
                    }
                    else if (friend.Tags.Contains("Requestee"))
                    {
                        isStranger = false;
                    }
                }
            }

            if (isStranger)
            {
                Debug.Log("Stranger!");
                AddFriendButton.SetActive(true);
            }
        }
    }

    private IEnumerator FetchImage(string _url)
    {
        using (UnityWebRequest webReq = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return webReq.SendWebRequest();
            if (webReq.isNetworkError)
            {
                Debug.LogError("Network error: " + webReq.error);
            }
            else
            {
                profilePicture.texture = DownloadHandlerTexture.GetContent(webReq);
                profilePicture.color = Color.white;
            }
        }
    }

    public void GetInventoryData(List<ItemInstance> items, UserAccountInfo u)
    {
        keyboard.Init(items);
    }

    public void GetStats(List<Statistic> statisticValues)
    {
        string stats = "";
        foreach(Statistic entry in statisticValues)
        {
            stats += entry.StatisticName + " " + entry.Value + "<br>";
        }
        tstats.text = stats;
    }

    public void AddFriend()
    {
        ConfirmationPanel.CPCallback cpc = ConfirmAddFriend;
        ConfirmationPanel.CPCallback cpc2 = CancelAddFriend;
        PersistantCanvas.Instance.ConfirmationPanel("Confirm add " + AccountInfo.Username + " as friend?", cpc, cpc2);
    }

    public void ConfirmAddFriend()
    {
        PlayerFriendRequestCallback pfrc = AddFriendCallBack;
        PlayFabPlayerData.AddFriend(AccountInfo, pfrc);
        AddFriendButton.SetActive(false);
    }

    public void CancelAddFriend()
    {
        PopupManager.Instance.ShowPopUp("Friend Request Cancelled");
    }

    public void AddFriendCallBack()
    {
        PopupManager.Instance.ShowPopUp("Friend Request Sent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
