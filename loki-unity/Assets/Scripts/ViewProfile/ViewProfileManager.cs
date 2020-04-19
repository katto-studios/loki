using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ViewProfileManager : Singleton<ViewProfileManager>
{
    public TextMeshProUGUI tname;
    public TextMeshProUGUI tstats;
    public TextMeshProUGUI tlevel;
    public RawImage profilePicture;

    public GameObject AddFriendButton;
    public GameObject RemoveFriendButton;

    public NetworkKeyboard keyboard;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init(UserAccountInfo u)
    {
        tname.text = u.Username;
        string url = u.TitleInfo.AvatarUrl;
        StartCoroutine(FetchImage(url));
        PlayerStatsCallBack pscb = GetStats;
        PlayFabPlayerData.GetPlayerStats(u, pscb);
        PlayerInventoryCallBack picb = GetInventoryData;
        PlayFabPlayerData.GetUserInventory(u, picb);
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

    public void GetInventoryData(List<ItemInstance> items)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
