using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NetworkUserDisplayInfo : MonoBehaviour {
    [Header("Displays")]
    public TextMeshProUGUI displayNameText;
    public TextMeshProUGUI displayScoreText;
    public Slider displayProgress;
    [Header("Unique values")]
    public PhotonPlayer player;
    
    //HAHHAH
    public int PlayerScore { get { return int.Parse(displayScoreText.text); } }

    private bool initalised = false;
    // Start is called before the first frame update
    void Start() {
        displayProgress.value = 0;
    }

    public void Initalise(PhotonPlayer _player) {
        player = _player;
        displayNameText.SetText(player.NickName);
        initalised = true;
    }

    // Update is called once per frame
    void Update() {
        if (initalised) {
            displayScoreText.SetText(player.CustomProperties["Score"].ToString());
            displayProgress.value = float.Parse(player.CustomProperties["Progress"].ToString());

            //check if got someone ontop
            int siblingIndex = transform.GetSiblingIndex();
            if (siblingIndex > 0) {
                Transform onTop = transform.parent.GetChild(siblingIndex - 1);
                while(onTop.GetComponent<NetworkUserDisplayInfo>().PlayerScore < PlayerScore) {
                    transform.SetSiblingIndex(--siblingIndex);
                    if(siblingIndex > 0) {
                        onTop = transform.parent.GetChild(siblingIndex - 1);
                    }else {
                        break;
                    }
                }
            }

         }
    }
}
