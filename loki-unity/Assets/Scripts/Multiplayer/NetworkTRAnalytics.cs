using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkTRAnalytics : TRAnalytics {
    // Start is called before the first frame update
    protected override void Start() {
        typeGameManager = NetworkGameManager.Instance;
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public override void GameComplete() {
        hasStarted = false;
        analyticsPanel.SetActive(true);

        float WPM = typeGameManager.words.Count / (timeSinceStart / 60);
        string WPMString = string.Format("{0:00.0}", WPM);
        WPMText.text = WPMString;

        float ACC = (1 - ((float)typeGameManager.mistakeWords.Count / (float)typeGameManager.words.Count)) * 100;
        string ACCString = string.Format("{0:00.0}", ACC);
        ACCText.text = ACCString;

        string MISSString = typeGameManager.mistakeWords.Count.ToString();
        MISSText.text = MISSString;

        SCOREText.text = typeGameManager.score.ToString();

        COMBOText.text = typeGameManager.maxCombo.ToString();

        SourceText.text = "from " + typeGameManager.CurrentParagraph.Source + " by " + typeGameManager.CurrentParagraph.Author;
    }
}
