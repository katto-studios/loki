using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;

public class MenuManager : MonoBehaviour {

    [Header("User data")]
    public TextMeshProUGUI userName;
    public RawImage profilePic;

    [Header("Currency")]
    public TextMeshProUGUI scrap;
    public TextMeshProUGUI gold;

    [Header("Levels")]
    public TextMeshProUGUI levelDisplay;
    public Slider levelProgress;

    [Header("lmao SOLIDS amirite")]
    public bool showCurrency = true;

    // Start is called before the first frame update
    void Start() {
        UpdateName();
        UpdateProfilePic();
        if(showCurrency) UpdateCurrency();
        UpdateLevel();
		PlayfabUserInfo.SetUserState(PlayfabUserInfo.UserState.InMainMenu);
    }

    private void UpdateName() {
        int count = 0;
        while (count ++ < 10000) {
            userName.text = PlayfabUserInfo.Username;
            if (!userName.text.Equals("")) {
                break;
            }
        }
    }

    private void UpdateProfilePic() {
        int count = 0;
        while (count++ < 10000) {
            Texture txt = PlayfabUserInfo.ProfilePicture;
            if (!txt) continue;
            profilePic.texture = txt;
            profilePic.color = Color.white;
            break;
        }
    }

    private void UpdateCurrency() {
        PlayFabClientAPI.GetUserInventory(
            new GetUserInventoryRequest(),
            (_result) => {
                scrap.SetText($"{_result.VirtualCurrency["SM"]}");
                gold.SetText($"{_result.VirtualCurrency["GC"]}");
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }

    private void UpdateLevel() {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest() {
                StatisticNames = new List<string>() {
                    "Level", 
                    "Xp"
                }
            },
            (_result) => {
                int level = _result.Statistics.First(_x => _x.StatisticName.Equals("Level")).Value;
                int xp = _result.Statistics.First(_x => _x.StatisticName.Equals("Xp")).Value;
                levelDisplay.SetText(level.ToString());

                levelProgress.value = (float)xp / GetLevelXp(level);
            },
            (_error) => { Debug.LogError(_error.GenerateErrorReport()); }
        );
    }
    //lmao this is the LEAST intensive way to do it
    private readonly int[] m_levelXps = new int[] { 749, 771, 791, 809, 827, 843, 858, 872, 886, 898, 911, 922, 933, 944, 954, 964, 973, 983, 991, 1000, 1008, 1016, 1024, 1031, 1038, 1045, 1052, 1059, 1065, 1072, 1078, 1084, 1090, 1096, 1101, 1107, 1112, 1117, 1122, 1128, 1132, 1137, 1142, 1147, 1151, 1156, 1160, 1165, 1169, 1173, 1177, 1181, 1185, 1189, 1193, 1197, 1201, 1204, 1208, 1212, 1215, 1219, 1222, 1226, 1229, 1232, 1235, 1239, 1242, 1245, 1248, 1251, 1254, 1257, 1260, 1263, 1266, 1269, 1272, 1274, 1277, 1280, 1283, 1285, 1288, 1291, 1293, 1296, 1298, 1301, 1303, 1306, 1308, 1311, 1313, 1315, 1318, 1320, 1322, 1325 };
    private int GetLevelXp(int _level) {
        return m_levelXps[_level - 1];
    }
}
