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

    public GameObject keyboard;

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
        PlayFabPlayerData.GetLeaderboardAroundPlayer(u, pscb);
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

    public void GetStats(List<PlayerLeaderboardEntry> statisticValues)
    {
        string stats = "";
        foreach(PlayerLeaderboardEntry entry in statisticValues)
        {
            stats += entry.DisplayName + " " + entry.StatValue + "<br>";
        }
        tstats.text = stats;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
